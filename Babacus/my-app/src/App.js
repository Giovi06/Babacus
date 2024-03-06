import logo from './logo.svg';
import './App.css';

function MyButton() {
  return (
    <button>
      Bestätigen
    </button>
 );
}

function MyRefund() {
  return(
    <button>
      Stornieren
    </button>
  );

}

export default function MyApp() {
  return (
    <div>
      <h1>Babacus</h1>
      <MyButton />
      <MyRefund /> 
    </div>
  );
}



