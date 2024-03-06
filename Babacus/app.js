document.addEventListener("DOMContentLoaded", function () {
  const form = document.querySelector("#productForm");
  const confirmationList = document.querySelector("#confirmation-list");

  form.addEventListener("submit", (event) => {
    event.preventDefault();

    const product_id = document.querySelector("#product_id").value;
    const payment_method = document.querySelector("#payment_method").value;

    const listItem = document.createElement("li");
    listItem.textContent = `Product ID: ${product_id}, Payment method: ${payment_method}`;

    confirmationList.appendChild(listItem);

    document.querySelector("#confirmation").style.display = "block";
    form.reset();
  });
});
document.addEventListener("DOMContentLoaded", function () {
  const form = document.querySelector("#productForm");

  form.addEventListener("submit", (event) => {
    event.preventDefault();

    const product_id = document.querySelector("#product_id").value;
    const payment_method = document.querySelector("#payment_method").value;

    const data = {
      product_id: product_id,
      payment_method: payment_method,
    };

    fetch("/api/products", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(data),
    })
      .then((response) => response.json())
      .then((data) => {
        const confirmationList = document.querySelector("#confirmation-list");
        const listItem = document.createElement("li");
        listItem.textContent = `Product ID: ${product_id}, Payment method: ${payment_method}`;
        confirmationList.appendChild(listItem);
        document.querySelector("#confirmation").style.display = "block";
        form.reset();
      })
      .catch((error) => {
        console.error("Error:", error);
      });
  });
});


