import React, { useState } from "react";
import "./Einkauf.css";

interface Product {
  id: string;
  name: string;
  price: number;
  description: string;
  supplier_id: string;
  quantity: number;
}

const MyComponent: React.FC = () => {
  const [product, setProduct] = useState<Product>({
    id: "",
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

  const generateId = (): string => {
    return Math.random().toString(36).substr(2, 9);
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    const newProduct: Product = {
      ...product,
      id: generateId(),
    };
    if (editingIndex !== null) {
      const updatedProducts = [...products];
      updatedProducts[editingIndex] = newProduct;
      setProducts(updatedProducts);
      setEditingIndex(null);
    } else {
      setProducts([...products, newProduct]);
    }

    // Prepare the data to be sent in the POST request
    const data = {
      boughtProductsList: [newProduct],
      payment: {
        method: "SomeMethod", // Change this to your actual payment method
        Amount: "SomeAmount", // Change this to the actual amount
      },
    };

    try {
      const response = await fetch("base Url/product/boughtproducts", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(data),
      });

      if (response.ok) {
        console.log("Product added successfully!");
      } else {
        console.error("Failed to add product!");
      }
    } catch (error) {
      console.error("Error:", error);
    }

    setProduct({
      id: "",
      name: "",
      price: 0,
      description: "",
      supplier_id: "",
      quantity: 0,
    });
  };

  const handleDelete = async (productId: string) => {
    try {
      const response = await fetch(`baseURL/product/${productId}`, {
        method: "DELETE",
      });

      if (response.ok) {
        console.log("Product deleted successfully!");
        setProducts((prevProducts) =>
          prevProducts.filter((product) => product.id !== productId)
        );
      } else {
        console.error("Failed to delete product!");
      }
    } catch (error) {
      console.error("Error:", error);
    }
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
            <li key={product.id}>
              <strong>Name:</strong> {product.name}, <strong>Preis:</strong>{" "}
              {product.price}, <strong>Beschreibung:</strong>{" "}
              {product.description}, <strong>Lieferanten ID:</strong>{" "}
              {product.supplier_id}, <strong>Menge:</strong> {product.quantity}
              <button onClick={() => handleEdit(index)}>Bearbeiten</button>
              <button onClick={() => handleDelete(product.id)}>Löschen</button>
            </li>
          ))}
        </ul>
      </div>
    </div>
  );
};

export default MyComponent;
