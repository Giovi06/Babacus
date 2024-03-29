document.addEventListener("DOMContentLoaded", function () {
  const form = document.querySelector("#productForm");
  const confirmationList = document.querySelector("#confirmation-list");

  form.addEventListener("submit", (event) => {
    event.preventDefault();

    const product_id = document.querySelector("#product_id").value;
    const payment_method = document.querySelector("#payment_method").value;
    const trademethod = document.querySelector("#trademethod").value;
    const quantity = document.querySelector("#quantity").value;

    const listItem = document.createElement("li");
    listItem.textContent = `Product ID: ${product_id}, Quantity: ${quantity}, Payment method: ${payment_method}, Trade method: ${trademethod} `;

    const deleteButton = document.createElement("button");
    deleteButton.textContent = "Delete";
    deleteButton.addEventListener("click", () => {
    listItem.remove();


    
  });
  confirmationList.appendChild(listItem);
  listItem.appendChild(deleteButton);
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
    const trademethod = document.querySelector("#trademethod").value;
    const quantity = document.querySelector("#quantity").value;

    const data = {
      product_id: product_id,
      quantity: quantity,
      payment_method: payment_method,
      trademethod: trademethod,
      
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


