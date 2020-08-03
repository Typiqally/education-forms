const processForm = async (event, node, model) => {
    event.preventDefault();

    const answers = [];

    model.questions.forEach(question => {
        const answer = {
            questionId: question.id,
            value: parseInt(node.elements[question.id].value),
        };

        answers.push(answer);
    });

    const response = await request(`/form/${model.id}/response`, 'POST', answers);
    console.log(response);

    window.location.replace(`/response/${response.id}`);

    return false;
}