const request = async (url, method = "GET", body) => {
  return await fetch(url, {
    method: method,
    body: JSON.stringify(body),
    headers: {
      "Content-Type": "application/json",
    },
  })
    .then((response) =>
      response
        .clone()
        .json()
        .catch(() => response.text())
    )
    .then((data) => {
      return data;
    })
    .catch((error) => {
      console.error(error);
    });
};
