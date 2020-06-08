class FormBuilder {
    constructor(form) {
        this.form = form;
    }

    build = () => {
        const root = document.createElement("div");
        root.className = "form";
        root.setAttribute("for", this.form.id);

        this.form.questions
            .sort((a, b) => (a.index > b.index ? 1 : -1))
            .forEach((question) => {
                const builder = new QuestionBuilder(this.form, question);
                const node = builder.build()
                
                this.addRemoveListener(question, node);
                root.appendChild(node);
            });

        const placeholder = FormBuilder.createPlaceholderInput("Click here to add a question");
        placeholder.onclick = async () => await this.addQuestion(root, placeholder);
        root.appendChild(placeholder);

        return root;
    };

    addRemoveListener = (question, node) => {
        const elements = node.getElementsByClassName("remove-question");

        Array.from(elements).forEach((element) => {
            element.onclick = () => this.removeQuestion(question, node);
        });
    }

    addQuestion = async (node, placeholder) => {
        const length = this.form.questions.length;
        let index = 0;
        if (length > 0) {
            index = this.form.questions[length - 1].index + 1
        }
        
        const typeProvider = parseInt(prompt("Enter type number", "0"));
        const model = {
            index: index,
            value: `Question ${index + 1}`,
            type: typeProvider
        }

        const url = `https://localhost:5002/form/${this.form.id}/question`;
        const question = await request(url, "POST", model);

        this.form.questions.push(question);

        const builder = new QuestionBuilder(this.form, question);
        const questionElement = builder.build();

        node.insertBefore(questionElement, placeholder);
        this.addRemoveListener(question, questionElement);
    }

    removeQuestion = async (question, node) => {
        const url = `https://localhost:5002/form/${this.form.id}/question/${question.id}`;
        await request(url, "DELETE");

        const index = this.form.questions.indexOf(question);
        if (index > -1) {
            this.form.questions.splice(index, 1);
        }

        node.remove();
    }

    static createEditableInput = (model, type = "text") => {
        const node = document.createElement("input");
        node.id = model.id;
        node.type = type;
        node.value = model.value;
        node.setAttribute("data-initial-value", model.value);

        node.oninput = (event) => this.onInputEdited(event);

        return node;
    }

    static createPlaceholderInput = (placeholder) => {
        const node = document.createElement("input");
        node.type = "text";
        node.placeholder = placeholder;

        node.oninput = (event) => this.onInputEdited(event);

        return node;
    }

    static onInputEdited = (event) => {
        const element = event.path[0];
        element.setAttribute("data-initial-value", element.value);
    };
}

class QuestionBuilder {
    constructor(form, question) {
        this.form = form;
        this.question = question;

        this.root = document.createElement("div");
        this.root.className = "question";
        this.root.setAttribute("for", this.question.id);
    }

    build = () => {
        const container = document.createElement("div");
        const span = document.createElement("span");
        const input = FormBuilder.createEditableInput(this.question);

        const remove = document.createElement("span");
        remove.className = "remove-question";
        remove.innerHTML = "&times;"

        span.append(input, remove)

        container.appendChild(span)
        this.root.appendChild(container)

        const options = this.buildOptions(this.root);
        options.forEach((element) => this.root.append(element));

        return this.root;
    }

    buildOptions = () => {
        const nodes = [];
        this.question.options
            .sort((a, b) => (a.index > b.index ? 1 : -1))
            .forEach((option) => {
                const builder = new OptionBuilder(this.question, option);
                const node = builder.build()
                
                this.addRemoveListener(option, node);
                nodes.push(node);
            });

        if (this.question.type === 0) {
            nodes.push(this.buildPlaceholder());
        }

        return nodes;
    }

    buildPlaceholder = () => {
        const placeholder = FormBuilder.createPlaceholderInput("Click here to add a option");
        placeholder.onclick = async () => await this.addOption(placeholder);

        return placeholder;
    }

    addRemoveListener = (option, node) => {
        const elements = node.getElementsByClassName("remove-option");

        Array.from(elements).forEach((element) => {
            element.onclick = () => this.removeOption(option, node)
        });
    }

    addOption = async (placeholder) => {
        const length = this.question.options.length;
        let index = 0;
        if (length > 0) {
            index = this.question.options[length - 1].index + 1
        }
        
        const model = {
            index: index,
            value: `Option ${index + 1}`
        }

        const url = `https://localhost:5002/form/${this.form.id}/question/${this.question.id}/option`;
        const option = await request(url, "POST", model);

        this.question.options.push(option);

        const builder = new OptionBuilder(this.question, option);
        const optionElement = builder.build();

        this.root.insertBefore(optionElement, placeholder);
        this.addRemoveListener(option, optionElement);
    }

    removeOption = async (option, node) => {
        const url = `https://localhost:5002/form/${this.form.id}/question/${this.question.id}/option/${option.id}`;
        await request(url, "DELETE");

        const index = this.question.options.indexOf(option);
        if (index > -1) {
            this.question.options.splice(index, 1);
        }
        
        node.remove();
    }
}

class OptionBuilder {
    constructor(question, option) {
        this.question = question;
        this.option = option;
    }

    build = () => {
        switch (this.question.type) {
            case 0:
                return this.buildMultipleChoice();
            case 1:
                return this.buildLinearScale();
            case 2:
                //No setup required
                break;
            default:
                throw `Could not build options for ${this.question.id}, given type was ${this.question.type}`;
        }
    }

    buildMultipleChoice = () => {
        const container = document.createElement("div");
        const span = document.createElement("span");
        const radio = document.createElement("input");
        radio.type = "radio";
        radio.disabled = true;

        const remove = document.createElement("span");
        remove.className = "remove-option";
        remove.innerHTML = "&times;"

        span.append(radio, FormBuilder.createEditableInput(this.option), remove);
        container.appendChild(span);

        return container;
    }

    buildLinearScale = () => {
        return FormBuilder.createEditableInput(this.option, "number")
    }
}

class FormTracker {
    constructor(model, node) {
        this.model = model;
        this.node = node;
    }

    render = (id) => {
        const element = document.getElementById(id);
        element.appendChild(this.node);
    }

    startAutoSave = () => {
        let timeout;

        const resetTimer = () => {
            clearTimeout(timeout);
            timeout = setTimeout(async () => await this.save(), 1500)
        };
        
        document.onkeypress = () => resetTimer();
        document.onmouseup = () => resetTimer();
    };

    save = async () => {
        console.log("Saving...");

        this.model.questions
            .forEach((question) => {
                question.value = document.getElementById(question.id).getAttribute("data-initial-value");
                question.options.forEach((option) => option.value = document.getElementById(option.id).getAttribute("data-initial-value"));
            });

        const url = `https://localhost:5002/form/${this.model.id}`;
        await request(url, "PUT", this.model);

        console.log("Saved");
    };
}

const startEditor = (model) => {
    const builder = new FormBuilder(model);
    const nodes = builder.build()

    const tracker = new FormTracker(model, nodes);
    tracker.render("form");
    tracker.startAutoSave();
}