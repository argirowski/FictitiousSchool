import React from "react";
import { Outlet, useNavigation } from "react-router-dom";
import LoadingSpinner from "./components/loader/LoadingSpinner";

function App() {
  const navigation = useNavigation();

  return (
    <div className="App">
      {navigation.state === "loading" && <LoadingSpinner />}
      <Outlet />
    </div>
  );
}

export default App;
