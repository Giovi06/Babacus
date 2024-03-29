import React, { useState } from "react";
import ReactDOM from "react-dom/client";
import Einkauf from "./Einkauf";
import Verkauf from "./Verkauf";

const Main: React.FC = () => {
  const [isEinkaufLoaded, setIsEinkaufLoaded] = useState(false);

  const handleEinkaufClick = () => {
    setIsEinkaufLoaded(true);
  };

  const handleVerkaufClick = () => {
    setIsEinkaufLoaded(false);
  };

  return (
    <div>
      <button onClick={handleEinkaufClick}>Einkauf laden</button>
      <button onClick={handleVerkaufClick}>Verkauf laden</button>

      {isEinkaufLoaded ? <Einkauf /> : <Verkauf />}
    </div>
  );
};

ReactDOM.createRoot(document.getElementById("root") as HTMLElement).render(
  <React.StrictMode>
    <Main />
  </React.StrictMode>
);
