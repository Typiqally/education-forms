Array.prototype.containsKey = function (obj, key) {
    for (const item of this) {
        if (item[key] === obj[key])
            return true;
    }

    return false;
}

class GraphBuilder {
    constructor(response) {
        this.response = response;
    }

    render = (element, labels, data) => {
        const context = element.getContext("2d");
        return new Chart(context, {
            type: "radar",
            data: {
                labels: labels,
                datasets: [
                    {
                        backgroundColor: "#d700967e",
                        borderColor: "#d70096d0",
                        pointBackgroundColor: "#d70096",
                        pointBorderColor: "#fff",
                        pointHoverBackgroundColor: "#fff",
                        pointHoverBorderColor: "#d70096d0",
                        pointRadius: 7,
                        pointHoverRadius: 7,
                        data: data,
                    },
                ],
            },
            options: {
                tooltips: {
                    titleFontSize: 15,
                    titleFontStyle: "normal",
                    titleMarginBottom: -1,
                    displayColors: false,
                    bodyFontSize: 0,
                },
                legend: {
                    display: false,
                },
                scale: {
                    pointLabels: {
                        fontSize: 16,
                    },
                },
            },
        });
    }

    buildAndRender = async (id) => {
        const url = `/form/${this.response.formId}/category/`;
        const categories = await request(url, "GET");
        const labels = [];
        const data = [];

        categories.forEach((category) => {
            labels.push(category.value);

            let value = 0;
            this.response.answers.filter(x => x.category.id === category.id).forEach((answer) => {
                value += answer.value;
            });

            data.push(value);
        });

        const element = document.getElementById(id);
        this.render(element, labels, data);
    }
}