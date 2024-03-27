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
  const [product, setProduct] = useState<Product>({
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
    setProduct({
      ...product,
      [name]: value,
    });
  };

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    // Berechnen Sie den Betrag basierend auf Preis und Menge
    const calculatedAmount = product.price * product.quantity;
    setProduct({
      ...product,
      amount: calculatedAmount,
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
            value={product.name}
            onChange={handleChange}
          />
        </label>
        <br />
        <label>
          Preis:
          <input
            type="number"
            name="price"
            value={product.price.toString()}
            onChange={handleChange}
          />
        </label>
        <br />
        <label>
          Beschreibung:
          <input
            type="text"
            name="description"
            value={product.description}
            onChange={handleChange}
          />
        </label>
        <br />
        <label>
          Lieferanten-ID:
          <input
            type="text"
            name="supplier_id"
            value={product.supplier_id}
            onChange={handleChange}
          />
        </label>
        <br />
        <label>
          Menge:
          <input
            type="number"
            name="quantity"
            value={product.quantity.toString()}
            onChange={handleChange}
          />
        </label>
        <br />
        <label>
          Zahlungsmethode:
          <select name="method" value={product.method} onChange={handleChange}>
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
        <p>Name: {product.name}</p>
        <p>Preis: {product.price}</p>
        <p>Beschreibung: {product.description}</p>
        <p>Lieferanten-ID: {product.supplier_id}</p>
        <p>Menge: {product.quantity}</p>
        <p>Zahlungsmethode: {product.method}</p>
        <p>Betrag: {product.amount}</p>
      </div>
    </div>
  );
};

export default MyComponent;
