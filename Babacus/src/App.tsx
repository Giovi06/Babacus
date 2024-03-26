import React, { useState } from "react";

interface Product {
  name: string;
  price: number;
  description: string;
  supplier_id: string;
  quantity: number;
}

const MyComponent: React.FC = () => {
  const [product, setProduct] = useState<Product>({
    name: "",
    price: 0,
    description: "",
    supplier_id: "",
    quantity: 0,
  });
  const [products, setProducts] = useState<Product[]>([]);
  const [editingIndex, setEditingIndex] = useState<number | null>(null);

  const handleChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>
  ) => {
    const { name, value } = e.target;
    setProduct((prevProduct) => ({
      ...prevProduct,
      [name]: value,
    }));
  };

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    if (editingIndex !== null) {
      const updatedProducts = [...products];
      updatedProducts[editingIndex] = product;
      setProducts(updatedProducts);
      setEditingIndex(null);
    } else {
      setProducts([...products, product]);
    }
    setProduct({
      name: "",
      price: 0,
      description: "",
      supplier_id: "",
      quantity: 0,
    });
  };

  const handleDelete = (index: number) => {
    setProducts((prevProducts) => prevProducts.filter((_, i) => i !== index));
  };

  const handleEdit = (index: number) => {
    setProduct(products[index]);
    setEditingIndex(index);
  };

  return (
    <div>
      <h1>Produkte kaufen</h1>
      <form onSubmit={handleSubmit}>
        <div>
          <label>
            Produktname:
            <input
              type="text"
              name="name"
              value={product.name}
              onChange={handleChange}
            />
          </label>
        </div>
        <div>
          <label>
            Preis:
            <input
              type="number"
              name="price"
              value={product.price}
              onChange={handleChange}
            />
          </label>
        </div>
        <div>
          <label>
            Beschreibung:
            <input
              type="text"
              name="description"
              value={product.description}
              onChange={handleChange}
            />
          </label>
        </div>
        <div>
          <label>
            Lieferanten ID:
            <input
              type="text"
              name="supplier_id"
              value={product.supplier_id}
              onChange={handleChange}
            />
          </label>
        </div>
        <div>
          <label>
            Menge:
            <input
              type="number"
              name="quantity"
              value={product.quantity}
              onChange={handleChange}
            />
          </label>
        </div>
        <button type="submit">
          {editingIndex !== null ? "Aktualisieren" : "Produkt hinzufügen"}
        </button>
      </form>
      <div>
        <h2>Gekaufte Produkte</h2>
        <ul>
          {products.map((product, index) => (
            <li key={index}>
              <strong>Name:</strong> {product.name}, <strong>Preis:</strong>{" "}
              {product.price}, <strong>Beschreibung:</strong>{" "}
              {product.description}, <strong>Lieferanten ID:</strong>{" "}
              {product.supplier_id}, <strong>Menge:</strong> {product.quantity}
              <button onClick={() => handleEdit(index)}>Bearbeiten</button>
              <button onClick={() => handleDelete(index)}>Löschen</button>
            </li>
          ))}
        </ul>
      </div>
    </div>
  );
};

export default MyComponent;
