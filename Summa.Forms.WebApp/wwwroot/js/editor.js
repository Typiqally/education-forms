createNode = (element, className) => {
    const node = document.createElement(element);
    if (className !== undefined) node.className = className;
    return node;
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
        const node = createNode("div", "form-editor"),
            container = createNode("div", "container form"),
            content = createNode("div", "container-content"),
            titleInput = FormTracker.buildTrackedInput(this.form, "text", "title"),
            descriptionInput = FormTracker.buildTrackedInput(this.form, "text", "description"),
            toolbar = FormTracker.buildTrackedFormToolbar();

        node.id = this.form.id;
        titleInput.className = "display-3";

        content.append(
            FormBuilder.buildLineInput(titleInput),
            FormBuilder.buildLineInput(descriptionInput)
        );

        container.append(content, toolbar);
        node.appendChild(container);

        this.form.questions.orderedForEach(question => {
            const builder = new QuestionBuilder(this.form, question);
            const questionNode = builder.build()

            node.append(questionNode);
        });

        return node;
    }

    static buildMaterialIcon = (icon, className) => {
        const classes = "material-icons".concat(" ", className);
        const node = createNode("span", classes);
        node.innerHTML = icon

        return node;
    }

    static buildLineInput = (node) => {
        const container = createNode("div", "line-input"),
            line = createNode("span", "line");

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
        const container = createNode("div", "container question"),
            content = createNode("div", "container-content"),
            input = FormTracker.buildTrackedInput(this.question, "text", "title"),
            toolbar = FormTracker.buildTrackedQuestionToolbar();

        container.id = this.question.id;
        container.append(FormBuilder.buildLineInput(input));

        const nodes = this.buildOptions();
        nodes.forEach((element) => content.append(element));

        container.append(content, toolbar);

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
            const node = createNode("div", "option add"),
                icon = FormBuilder.buildMaterialIcon("radio_button_unchecked"),
                input = createNode("input"),
                lineInput = FormBuilder.buildLineInput(input);

            input.placeholder = "Add an option";
            node.append(icon, lineInput);

            nodes.push(node);
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
            radio = FormBuilder.buildMaterialIcon("radio_button_unchecked"),
            input = FormTracker.buildTrackedTitleValueInput(this.option),
            remove = FormBuilder.buildMaterialIcon("close", "btn-icon remove");

        container.id = this.option.id;
        container.className = "option";

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

    renderQuestion = (question, predecessor) => {
        predecessor = predecessor !== undefined ? predecessor : this.node.children[0];
        
        const builder = new QuestionBuilder(this.form, question);
        const questionNode = builder.build();
        
        this.node.insertBefore(questionNode, predecessor.nextSibling)
        this.trackQuestion(question);
    }

    renderOption = (question, option) => {
        const node = document.getElementById(question.id).querySelector(".container-content");
        const builder = new OptionBuilder(this.form, question, option);
        const optionNode = builder.build();
        
        node.insertBefore(optionNode, node.lastChild);
        this.trackOption(question, option);
    }

    track = () => {
        const formNode = document.getElementById(this.form.id),
            toolbar = formNode.querySelector(".container-footer");

        Array.from(toolbar.querySelectorAll(".add")).forEach(input => input.onclick = async () => await this.addAndRenderQuestion());

        this.form.questions.forEach(question => {
            this.trackQuestion(question);

            question.options.forEach(option => {
                this.trackOption(question, option);
            });
        });
    }

    trackQuestion = (question) => {
        const questionNode = document.getElementById(question.id),
            content = questionNode.querySelector(".container-content"),
            toolbar = questionNode.querySelector(".container-footer");

        Array.from(content.querySelectorAll(".add")).forEach(input => input.onclick = async () => await this.addAndRenderOption(question));
        Array.from(toolbar.querySelectorAll(".remove")).forEach(input => input.onclick = async () => await this.removeQuestion(question, questionNode));
        Array.from(toolbar.querySelectorAll(".add")).forEach(input => input.onclick = async () => await this.addAndRenderQuestion(questionNode));
    }

    trackOption = (question, option) => {
        const optionNode = document.getElementById(option.id),
            removeNodes = optionNode.getElementsByClassName("remove");

        Array.from(removeNodes).forEach(input =>
            input.onclick = async () => await this.removeOption(question, option, optionNode)
        );
    }

    updateIndexes = (node, key) => {
        const array = Array.from(node.children);

        array.filter(element => element.className === "container question").forEach((element, index) => {
            this.form[key].find(object => object.id === element.id).index = index;
        });
    }

    save = async () => {
        console.debug("Saving...");

        this.updateIndexes(this.node, "questions")

        this.form.title = document.getElementById(`${this.form.id}-title`).getAttribute("data-initial-title");
        this.form.description = document.getElementById(`${this.form.id}-description`).getAttribute("data-initial-description");

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

    addQuestion = async (index) => {
        const count = this.form.questions.nextIndex();

        const type = parseInt(prompt("Enter type number", "0"));
        const model = {
            index: index,
            title: `Question ${count + 1}`,
            type: type
        };

        const url = `/form/${this.form.id}/question`;
        return await request(url, "POST", model);
    }

    addOption = async (question) => {
        const index = question.options.nextIndex()
        const model = {
            index: index,
            title: `Option ${index + 1}`,
            value: 0
        };

        const url = `/form/${this.form.id}/question/${question.id}/option`;
        return await request(url, "POST", model);
    }

    addAndRenderQuestion = async (node) => {
        const index = node !== undefined ? node.parentElement.childElementCount : 0;

        const question = await this.addQuestion(index);
        this.form.questions.push(question);

        this.renderQuestion(question, node);
    }

    addAndRenderOption = async (question) => {
        const option = await this.addOption(question);
        question.options.push(option);
        
        this.renderOption(question, option);
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

    static buildTrackedQuestionToolbar = () => {
        const container = createNode("div", "container-footer"),
            add = FormBuilder.buildMaterialIcon("add_circle_outline", "btn-icon add"),
            remove = FormBuilder.buildMaterialIcon("close", "btn-icon remove");

        container.append(remove, add);

        return container;
    }

    static buildTrackedFormToolbar = () => {
        const container = createNode("div", "container-footer"),
            add = FormBuilder.buildMaterialIcon("add_circle_outline", "btn-icon add");

        container.append(add);

        return container;
    }
}