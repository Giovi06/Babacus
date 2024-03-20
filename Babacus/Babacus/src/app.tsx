// app.tsx
import React from "react";

interface Item {
  productId: string;
  quantity: number;
  paymentMethod: "bar" | "karte";
  transactionType: "Einkauf" | "Verkauf";
}

const MyComponent: React.FC = () => {
  const [item, setItem] = useState<Item>({
    productId: "",
    quantity: 0,
    paymentMethod: "bar",
    transactionType: "Einkauf",
  });

  const handleChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>
  ) => {
    const { name, value } = e.target;
    setItem((prevItem) => ({
      ...prevItem,
      [name]: value,
    }));
  };

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    console.log("Item:", item);
    // Hier kannst du die Logik zum Speichern des Items implementieren
  };

  return (
    <div>
      <h1>Meine Anwendung</h1>
      <form onSubmit={handleSubmit}>
        <div>
          <label>
            Produkt ID:
            <input
              type="text"
              name="productId"
              value={item.productId}
              onChange={handleChange}
            />
          </label>
        </div>
        <div>
          <label>
            Anzahl:
            <input
              type="number"
              name="quantity"
              value={item.quantity}
              onChange={handleChange}
            />
          </label>
        </div>
        <div>
          <label>
            Zahlungsmethode:
            <select
              name="paymentMethod"
              value={item.paymentMethod}
              onChange={handleChange}
            >
              <option value="bar">Bar</option>
              <option value="karte">Karte</option>
            </select>
          </label>
        </div>
        <div>
          <label>
            Geschäftstyp:
            <select
              name="transactionType"
              value={item.transactionType}
              onChange={handleChange}
            >
              <option value="Einkauf">Einkauf</option>
              <option value="Verkauf">Verkauf</option>
            </select>
          </label>
        </div>
        <button type="submit">Speichern</button>
      </form>
    </div>
  );
};

export default MyComponent;
