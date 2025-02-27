import { createBrowserRouter } from "react-router-dom";
import App from "./App";
import HomePage from "./components/HomePage";
import AddSchoolApplication from "./components/AddSchoolApplication";
import SchoolApplicationsList from "./components/SchoolApplicationsList";
import SingleSchoolApplication from "./components/SingleSchoolApplication";
import EditSchoolApplication from "./components/EditSchoolApplication";

const router = createBrowserRouter([
  {
    path: "/",
    element: <App />,
    children: [
      { path: "/", element: <HomePage /> },
      { path: "/applications", element: <SchoolApplicationsList /> },
      { path: "/applications/add", element: <AddSchoolApplication /> },
      { path: "/applications/:id", element: <SingleSchoolApplication /> },
      { path: "/applications/:id/edit", element: <EditSchoolApplication /> },
    ],
  },
]);

export default router;
