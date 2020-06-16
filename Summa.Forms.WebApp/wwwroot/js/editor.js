createNode = (element) => {
    return document.createElement(element);
}

Object.defineProperty(Array.prototype, "orderedForEach", {
    value: function orderedForEach(callback) {
        this.sort((a, b) => (a.index > b.index ? 1 : -1))
            .forEach(callback);
    },
    writable: true,
    configurable: true
});

Object.defineProperty(Array.prototype, "nextIndex", {
    value: function nextIndex(_default = 0) {
        const length = this.length;
        return length > 0 ? this[length - 1].index + 1 : _default;
    },
    writable: true,
    configurable: true
});

class FormBuilder {
    constructor(form) {
        this.form = form;
    }

    build() {
        const node = createNode("div");
        node.id = this.form.id;
        node.className = "form-editor";

        this.form.questions.orderedForEach(question => {
            const builder = new QuestionBuilder(this.form, question);
            const questionNode = builder.build()

            node.append(questionNode);
        });

        const input = FormTracker.buildPlaceholderInput("Click here to add a question");
        node.appendChild(input);

        return node;
    }

    static buildLineInput(node) {
        const container = createNode("div"),
            line = createNode("span");
        
        container.className = "line-input";
        line.className = "line";
        
        container.append(node, line);
        
        return container;
    }
}

class QuestionBuilder {
    constructor(form, question) {
        this.form = form;
        this.question = question;
    }

    build() {
        const container = createNode("div"),
            input = FormTracker.buildTrackedInput(this.question, "text", "title"),
            remove = FormTracker.buildRemoveInput();

        container.id = this.question.id;
        container.className = "container question";
        
        container.append(FormBuilder.buildLineInput(input));

        const nodes = this.buildOptions();
        nodes.forEach((element) => container.append(element));
        
        container.append(remove);

        return container;
    }

    buildOptions() {
        const nodes = [];

        this.question.options.orderedForEach(option => {
            const builder = new OptionBuilder(this.form, this.question, option);
            const node = builder.build()

            nodes.push(node);
        });

        if (this.question.type === 0) {
            const add = FormTracker.buildPlaceholderInput("Click to add an option");
            nodes.push(add);
        }

        return nodes;
    }
}

class OptionBuilder {
    constructor(form, question, option) {
        this.form = form;
        this.question = question;
        this.option = option;
    }

    build() {
        const types = {
            0: this.buildMultipleChoice(),
            1: this.buildLinearScale(),
            2: this.buildOpen(),
        }

        return types[this.question.type];
    }

    buildMultipleChoice() {
        const container = createNode("div"),
            radio = createNode("span"),
            input = FormTracker.buildTrackedTitleValueInput(this.option),
            remove = FormTracker.buildRemoveInput();

        container.id = this.option.id;
        container.className = "option";

        radio.className = "material-icons";
        radio.innerHTML = "radio_button_unchecked";

        container.appendChild(radio);
        input.forEach(node => container.appendChild(FormBuilder.buildLineInput(node)));
        container.appendChild(remove);

        return container;
    }

    buildLinearScale() {
        const container = createNode("div"),
            input = FormTracker.buildTrackedTitleValueInput(this.option);

        container.id = this.option.id;
        container.className = "option";

        input.forEach(node => container.appendChild(FormBuilder.buildLineInput(node)));

        return container;
    }

    buildOpen() {

    }
}

class FormTracker {
    constructor(form, node) {
        this.form = form;
        this.node = node;
    }

    render = (id) => {
        const element = document.getElementById(id);
        element.appendChild(this.node);
    }

    track = () => {
        const formNode = document.getElementById(this.form.id),
            addNodes = formNode.querySelectorAll(":scope > .add");

        addNodes.forEach(input => {
            input.onclick = async () => await this.addQuestion(formNode, input);
        });

        this.form.questions.forEach(question => {
            this.trackQuestion(question);

            question.options.forEach(option => {
                this.trackOption(question, option);
            });
        });
    }

    trackQuestion = (question) => {
        const questionNode = document.getElementById(question.id),
            addNodes = questionNode.getElementsByClassName("add"),
            removeNodes = questionNode.getElementsByClassName("remove");

        Array.from(removeNodes).forEach(input =>
            input.onclick = async () => await this.removeQuestion(question, questionNode)
        );

        Array.from(addNodes).forEach(input =>
            input.onclick = async () => await this.addOption(questionNode, question, input)
        );
    }

    trackOption = (question, option) => {
        const optionNode = document.getElementById(option.id),
            removeNodes = optionNode.getElementsByClassName("remove");

        Array.from(removeNodes).forEach(input =>
            input.onclick = async () => await this.removeOption(question, option, optionNode)
        );
    }

    save = async () => {
        console.debug("Saving...");

        this.form.questions
            .forEach((question) => {
                question.title = document.getElementById(`${question.id}-title`).getAttribute("data-initial-title");

                question.options.forEach((option) => {
                    option.title = document.getElementById(`${option.id}-title`).getAttribute("data-initial-title");
                    option.value = parseInt(document.getElementById(`${option.id}-value`).getAttribute("data-initial-value"));
                });
            });

        const url = `/form/${this.form.id}`;
        await request(url, "PUT", this.form);

        console.debug("Saved");
    };

    startAutoSave = () => {
        let timeout;

        const resetTimer = () => {
            clearTimeout(timeout);
            timeout = setTimeout(async () => await this.save(), 1000)
        };

        document.onkeypress = () => resetTimer();
        document.onmouseup = () => resetTimer();
    };

    addQuestion = async (formNode, placeholder) => {
        const index = this.form.questions.nextIndex(),
            type = parseInt(prompt("Enter type number", "0")),
            model = {
                index: index,
                title: `Question ${index + 1}`,
                type: type
            };

        const url = `/form/${this.form.id}/question`,
            question = await request(url, "POST", model);

        this.form.questions.push(question);

        const builder = new QuestionBuilder(this.form, question);
        const questionNode = builder.build();

        formNode.insertBefore(questionNode, placeholder);
        this.trackQuestion(question);
    }

    addOption = async (questionNode, question, placeholder) => {
        const index = question.options.nextIndex(),
            model = {
                index: index,
                title: `Option ${index + 1}`,
                value: 0
            };

        const url = `/form/${this.form.id}/question/${question.id}/option`,
            option = await request(url, "POST", model);

        question.options.push(option);

        const builder = new OptionBuilder(this.form, question, option),
            optionNode = builder.build();

        questionNode.insertBefore(optionNode, placeholder);
        this.trackOption(question, option);
    }

    removeTrackedNode = (node, list, object) => {
        const index = list.indexOf(object);
        if (index > -1) {
            list.splice(index, 1);
        }

        node.remove();
    }

    removeQuestion = async (question, node) => {
        const url = `/form/${this.form.id}/question/${question.id}`;
        await request(url, "DELETE");

        this.removeTrackedNode(node, this.form.questions, question);
    }

    removeOption = async (question, option, node) => {
        if (question.options.length <= 1)
            return;

        const url = `/form/${this.form.id}/question/${question.id}/option/${option.id}`;
        await request(url, "DELETE");

        this.removeTrackedNode(node, question.options, option);
    }

    static onInputEdited = (event, name) => {
        const element = event.path[0];
        element.setAttribute(name, element.value);
    };

    static buildTrackedInput = (object, type, key) => {
        const node = createNode("input");
        const name = `data-initial-${key}`;

        node.id = `${object.id}-${key}`;
        node.type = type;
        node.value = object[key];
        node.setAttribute(name, object[key]);
        node.oninput = event => this.onInputEdited(event, name);

        return node;
    }

    static buildTrackedTitleValueInput = (object) => {
        const title = FormTracker.buildTrackedInput(object, "text", "title"),
            input = FormTracker.buildTrackedInput(object, "number", "value");

        return [title, input];
    }

    static buildPlaceholderInput = (placeholder) => {
        const node = createNode("input");
        node.type = "text";
        node.placeholder = placeholder;
        node.className = "add";

        return node;
    }

    static buildRemoveInput = () => {
        const node = createNode("span");
        node.innerHTML = "close"
        node.className = "btn-icon remove material-icons";

        return node;
    }
}

const startEditor = (model) => {
    const builder = new FormBuilder(model);
    const nodes = builder.build()

    const tracker = new FormTracker(model, nodes);
    tracker.render("form");
    tracker.track();
    tracker.startAutoSave();
}