import "./App.css";
import { BrowserRouter, Route, Routes } from "react-router";
import Register from "./components/auth/Register";

function App() {
  return (
    <>
      <BrowserRouter>
        <Routes>
          <Route path="/register" element={<Register />} />
        </Routes>
      </BrowserRouter>
    </>
  );
}

export default App;
