import React, { useState } from "react";
import './App.css';


interface Item {
  id: number;
  productId: string;
  quantity: number;
  paymentMethod: "Bar" | "Karte" | "Rechnung";
  transactionType:  "Verkauf";
}

const MyComponent: React.FC = () => {
  const [item, setItem] = useState<Item>({
    id: 1, // Initial ID
    productId: "",
    quantity: 0,
    paymentMethod: "Bar",
    transactionType: "Verkauf",
  });
  const [items, setItems] = useState<Item[]>([]);
  const [editingItem, setEditingItem] = useState<Item | null>(null);

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
    setItems([...items, { ...item, id: Date.now() }]);
    setItem({
      ...item, // Keep other properties unchanged
      id: Date.now(), // Generate a new ID
    });
  };

  const handleDelete = (id: number) => {
    setItems(items.filter((item) => item.id !== id));
  };

  const handleEdit = (editItem: Item) => {
    setEditingItem(editItem);
  };

  const handleUpdate = (e: React.FormEvent) => {
    e.preventDefault();
    setItems(
      items.map((existingItem) =>
        existingItem.id === editingItem!.id ? editingItem! : existingItem
      )
    );
    setEditingItem(null);
  };

  const handleEditChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>
  ) => {
    const { name, value } = e.target;
    setEditingItem((prevItem) => ({
      ...prevItem!,
      [name]: value,
    }));
  };

  const handleCancelEdit = () => {
    setEditingItem(null);
  };

  return (
    <div>
      <h1>Verkauf</h1>
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
              <option value="Bar">Bar</option>
              <option value="Karte">Karte</option>
              <option value="Rechnung">Rechnung</option>
            </select>
          </label>
        </div>
        <button type="submit">Speichern</button>
      </form>
      <div>
        <h2>Gespeicherte Items</h2>
        <ul>
          {items.map((item) => (
            <li key={item.id}>
              Produkt ID: {item.productId}, Anzahl: {item.quantity},{" "}
              Zahlungsmethode: {item.paymentMethod}, Geschäftstyp:{" "}
              {item.transactionType}{" "}
              <button onClick={() => handleEdit(item)}>Bearbeiten</button>{" "}
              <button onClick={() => handleDelete(item.id)}>Löschen</button>
            </li>
          ))}
        </ul>
      </div>
      {editingItem && (
        <div>
          <h2>Bearbeite Item</h2>
          <form onSubmit={handleUpdate}>
            <div>
              <label>
                Produkt ID:
                <input
                  type="text"
                  name="productId"
                  value={editingItem.productId}
                  onChange={handleEditChange}
                  disabled // Disable editing of productId
                />
              </label>
            </div>
            <div>
              <label>
                Anzahl:
                <input
                  type="number"
                  name="quantity"
                  value={editingItem.quantity}
                  onChange={handleEditChange}
                />
              </label>
            </div>
            <div>
              <label>
                Zahlungsmethode:
                <select
                  name="paymentMethod"
                  value={editingItem.paymentMethod}
                  onChange={handleEditChange}
                >
                  <option value="Bar">Bar</option>
                  <option value="Karte">Karte</option>
                  <option value="Rechnung">Rechnung</option>
                </select>
              </label>
            </div>
            <div>
              <label>
                Geschäftstyp:
                <select
                  name="transactionType"
                  value={editingItem.transactionType}
                  onChange={handleEditChange}
                >
                  <option value="Verkauf">Verkauf</option>
                </select>
              </label>
            </div>
            <button type="submit">Aktualisieren</button>
            <button onClick={handleCancelEdit}>Abbrechen</button>
          </form>
        </div>
      )}
    </div>
  );
};



const handleSubmit = async (e: React.FormEvent) => {
  e.preventDefault();
  // Logik für einfügen der Produktinfos 

  try {
      const response = await fetch('https://babacus.com/api/products', {
          method: 'POST',
          headers: {
              'Content-Type': 'application/json',
              
          },
          body: JSON.stringify(item) 
      });

      if (response.ok) {
          console.log('Produkt erfolgreich an das Backend gesendet.');
          
      } else {
          console.error('Fehler beim Senden des Produkts an das Backend.');
          
      }
  } catch (error) {
      console.error('Fehler beim Senden der Anfrage:', error);
      
  }
};

export default MyComponent;
