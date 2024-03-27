import React, { useState } from "react";
import "./Einkauf.css";

interface Product {
  id: number;
  name: string;
  price: number;
  description: string;
  supplier_id: string;
  quantity: number;
  method: "Bar" | "Karte" | "Rechnung";
  amount: number;
}

const MyComponent: React.FC = () => {
  const [products, setProducts] = useState<Product[]>([]);
  const [newProduct, setNewProduct] = useState<Product>({
    id: 1,
    name: "",
    price: 0,
    description: "",
    supplier_id: "",
    quantity: 0,
    method: "Bar",
    amount: 0,
  });

  const handleChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>
  ) => {
    const { name, value } = e.target;
    setNewProduct({
      ...newProduct,
      [name]: value,
    });
  };

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    const calculatedAmount = newProduct.price * newProduct.quantity;
    const updatedProduct = {
      ...newProduct,
      amount: calculatedAmount,
    };
    setProducts([...products, updatedProduct]);
    setNewProduct({
      ...newProduct,
      id: newProduct.id + 1,
      name: "",
      price: 0,
      description: "",
      supplier_id: "",
      quantity: 0,
      method: "Bar",
      amount: 0,
    });

    // POST-Request auslösen
    const postData = {
      BoughtProductsList: [
        {
          name: updatedProduct.name,
          price: updatedProduct.price.toString(),
          description: updatedProduct.description, //Eventuell ist tostring falsch
          supplier_id: updatedProduct.supplier_id,
          quantity: updatedProduct.quantity.toString(),
        },
      ],
      payment: {
        method: updatedProduct.method,
        Amount: updatedProduct.amount,
      },
    };

    fetch("{{baseUrl}}/product/boughtproducts", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(postData),
    })
      .then((response) => {
        if (!response.ok) {
          throw new Error("Network response was not ok");
        }
        // Handle success
        console.log("POST request successful");
      })
      .catch((error) => {
        // Handle error
        console.error("There was a problem with the POST request:", error);
      });
  };

  return (
    <div>
      <form onSubmit={handleSubmit}>
        <label>
          Name:
          <input
            type="text"
            name="name"
            value={newProduct.name}
            onChange={handleChange}
          />
        </label>
        <br />
        <label>
          Preis:
          <input
            type="number"
            name="price"
            value={newProduct.price.toString()}
            onChange={handleChange}
          />
        </label>
        <br />
        <label>
          Beschreibung:
          <input
            type="text"
            name="description"
            value={newProduct.description}
            onChange={handleChange}
          />
        </label>
        <br />
        <label>
          Lieferanten-ID:
          <input
            type="text"
            name="supplier_id"
            value={newProduct.supplier_id}
            onChange={handleChange}
          />
        </label>
        <br />
        <label>
          Menge:
          <input
            type="number"
            name="quantity"
            value={newProduct.quantity.toString()}
            onChange={handleChange}
          />
        </label>
        <br />
        <label>
          Zahlungsmethode:
          <select
            name="method"
            value={newProduct.method}
            onChange={handleChange}
          >
            <option value="Bar">Bar</option>
            <option value="Karte">Karte</option>
            <option value="Rechnung">Rechnung</option>
          </select>
        </label>
        <br />
        <button type="submit">Produkt hinzufügen</button>
      </form>
      <div>
        <h2>Produktinformationen:</h2>
        {products.map((product) => (
          <div key={product.id}>
            <p>Name: {product.name}</p>
            <p>Preis: {product.price}</p>
            <p>Beschreibung: {product.description}</p>
            <p>Lieferanten-ID: {product.supplier_id}</p>
            <p>Menge: {product.quantity}</p>
            <p>Zahlungsmethode: {product.method}</p>
            <p>Betrag: {product.amount}</p>
          </div>
        ))}
      </div>
    </div>
  );
};

export default MyComponent;
