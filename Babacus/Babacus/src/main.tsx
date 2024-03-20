import { render } from "preact";
import App from "./app";
import "bootstrap/dist/css/bootstrap.css";

render(<App />, document.getElementById("app") as HTMLElement);
