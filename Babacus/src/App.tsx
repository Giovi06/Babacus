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

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (editingIndex !== null) {
      const updatedProducts = [...products];
      updatedProducts[editingIndex] = product;
      setProducts(updatedProducts);
      setEditingIndex(null);
    } else {
      setProducts([...products, product]);
    }

    // Prepare the data to be sent in the POST request
    const data = {
      boughtProductsList: [product],
      payment: {
        method: "SomeMethod", // Change this to your actual payment method
        Amount: "SomeAmount", // Change this to the actual amount
      },
    };

    try {
      const response = await fetch("base Url/product/boughtproducts", {
        //UrL muss geändert werden.
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

  const handleUpdate = async () => {
    if (editingIndex !== null) {
      try {
        const response = await fetch(`baseURL/product/updateinfo`, {
          method: "PUT",
          headers: {
            //merke muss no eindeutige id verwenden so nicht so ideal.
            "Content-Type": "application/json",
          },
          body: JSON.stringify({ index: editingIndex, product }),
        });

        if (response.ok) {
          console.log("Product updated successfully!");
          const updatedProducts = [...products];
          updatedProducts[editingIndex] = product;
          setProducts(updatedProducts);
          setEditingIndex(null);
        } else {
          console.error("Failed to update product!");
        }
      } catch (error) {
        console.error("Error:", error);
      }
    } else {
      console.error("No product selected for editing!");
    }
  };

  const Delete = async (productId: string) => {
    try {
      const response = await fetch(`baseURL/product/${productId}`, {
        method: "DELETE",
      });

      if (response.ok) {
        console.log("Product deleted successfully!");
        setProducts((prevProducts) =>
          prevProducts.filter((product) => product.id !== productId)
        ); //Id für jedes element
      } else {
        console.error("Failed to delete product!");
      }
    } catch (error) {
      console.error("Error:", error);
    }
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
