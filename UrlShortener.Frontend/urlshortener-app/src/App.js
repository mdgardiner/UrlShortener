import { Routes, Route } from "react-router-dom";
import "./App.css";
import Home from "./components/Home";
import RedirectUrl from "./components/RedirectUrl";

function App() {
  return (
    <div className="App App-header">
      <Routes>
        <Route exact path="/" element={<Home />} />
        <Route path="/:shortCode" element={<RedirectUrl />} />
      </Routes>
    </div>
  );
}

export default App;
