class FormBuilder {
    constructor(model) {
        this.model = model;
    }

    build = () => {
        const nodes = [];
        this.model.questions
            .sort((a, b) => (a.index > b.index ? 1 : -1))
            .forEach((model) => {
                const question = new QuestionBuilder(this.model, model);
                nodes.push(question.build());
            });
        return nodes;
    };

    static createEditableInput = (model, type = "text") => {
        const node = document.createElement("input");
        node.id = model.id;
        node.type = type;
        node.value = model.value;
        node.setAttribute("data-initial-value", model.value);

        node.oninput = (event) => this.onInputEdited(event);

        return node;
    }

    static createPlaceholderInput = () => {
        const node = document.createElement("input");
        node.type = "text";
        node.placeholder = "Click here to add";

        node.oninput = (event) => this.onInputEdited(event);

        return node;
    }

    static onInputEdited = (event) => {
        const element = event.path[0];
        element.setAttribute("data-initial-value", element.value);
    };
}

class QuestionBuilder {
    constructor(holder, model) {
        this.holder = holder;
        this.model = model;
    }

    build = () => {
        const node = document.createElement("div");
        node.className = "question";
        node.setAttribute("for", this.model.id);

        const container = document.createElement("div");
        const input = FormBuilder.createEditableInput(this.model);
        container.appendChild(input)
        node.appendChild(container)

        const options = this.buildOptions();
        options.forEach((element) => node.append(element));

        const placeholder = FormBuilder.createPlaceholderInput();
        placeholder.onclick = async () => await this.introduceOption(node, placeholder);

        node.appendChild(placeholder);

        return node;
    }

    introduceOption = async (node, placeholder) => {
        const length = this.model.options.length + 1;
        const model = {
            index: length + 1,
            value: `Option ${length}`
        }

        const url = `https://localhost:5002/form/${this.holder.id}/question/${this.model.id}/option`;
        const introduced = await request(url, "POST", model);
        
        this.model.options.push(introduced);

        const option = new OptionBuilder(this.model, introduced);
        node.insertBefore(option.build(), placeholder);
    }

    buildOptions = () => {
        const nodes = [];
        this.model.options
            .sort((a, b) => (a.index > b.index ? 1 : -1))
            .forEach((model) => {
                const option = new OptionBuilder(this.model, model);
                nodes.push(option.build());
            });
        return nodes;
    }
}

class OptionBuilder {
    constructor(holder, model) {
        this.holder = holder;
        this.model = model;
    }

    build = () => {
        switch (this.holder.type) {
            case 0:
                return this.buildMultipleChoice();
            case 1:
                return this.buildLinearScale();
            case 2:
                //No setup required
                break;
            default:
                throw `Could not build options for ${this.holder.id}, given type was ${this.holder.type}`;
        }
    }

    buildMultipleChoice = () => {
        const container = document.createElement("div");
        const span = document.createElement("span");
        const radio = document.createElement("input");
        radio.type = "radio";
        radio.disabled = true;

        span.append(radio, FormBuilder.createEditableInput(this.model));
        container.appendChild(span);

        return container;
    }

    buildLinearScale = () => {
        return FormBuilder.createEditableInput(this.model, "number")
    }
}

class FormTracker {
    constructor(model, nodes) {
        this.model = model;
        this.nodes = nodes;
    }

    render = (id) => {
        const element = document.getElementById(id);
        this.nodes.forEach((node) => element.appendChild(node));
    }

    autoSave = () => {
        let timeout;

        document.onkeypress = () => {
            clearTimeout(timeout);
            timeout = setTimeout(() => this.save(), 1500)
        };
    };

    save = () => {
        console.log("Saving...");

        this.model.questions
            .forEach((question) => {
                question.value = document.getElementById(question.id).getAttribute("data-initial-value");
                question.options.forEach((option) => option.value = document.getElementById(option.id).getAttribute("data-initial-value"));
            });

        const url = `https://localhost:5002/form/${this.model.id}`;
        request(url, "PUT", this.model).then((r) => {
            console.log("Saved");
        });
    };
}

const startEditor = (model) => {
    const builder = new FormBuilder(model);
    const nodes = builder.build()

    const tracker = new FormTracker(model, nodes);
    tracker.render("form");
    tracker.autoSave();
}