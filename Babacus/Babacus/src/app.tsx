import { useState } from 'preact/hooks'
import preactLogo from './assets/preact.svg'
import './app.css'



// Interface für die Todo-Objekte
interface Todo {
  id: number;
  text: string;
  completed: boolean;
}

// Komponente für die Todo-Liste
const TodoList: React.FC<{ todos: Todo[] }> = ({ todos }) => (
  <ul>
    {todos.map(todo => (
      <li key={todo.id}>{todo.text}</li>
    ))}
  </ul>
);

// Hauptkomponente für die Todo-App
const TodoApp: React.FC = () => {
  // Zustand für die Todo-Liste
  const [todos, setTodos] = useState<Todo[]>([]);

  // Funktion zum Hinzufügen einer neuen Todo
  const addTodo = () => {
    const newTodo: Todo = {
      id: todos.length + 1,
      text: `Todo ${todos.length + 1}`,
      completed: false,
    };
    setTodos(prevTodos => [...prevTodos, newTodo]);
  };

  return (
    <div>
      <h1>Todo List</h1>
      <TodoList todos={todos} />
      <button onClick={addTodo}>Add Todo</button>
    </div>
  );
};

export default TodoApp;

