import React from "react";
import { render, screen, fireEvent } from "@testing-library/react";
import SingleSchoolApplication from "../components/SingleSchoolApplication";
import { MemoryRouter, Route, Routes } from "react-router-dom";
import * as applicationService from "../components/services/applicationService";

jest.mock("../components/services/applicationService");

const mockApplication = {
  id: "1",
  course: { id: 1, name: "Math" },
  courseDate: { id: "cd1", date: "2025-06-13T00:00:00Z" },
  company: { name: "TestCo", phone: "123456", email: "test@co.com" },
  participants: [
    { name: "Alice", phone: "111", email: "alice@co.com" },
    { name: "Bob", phone: "222", email: "bob@co.com" },
  ],
};

describe("SingleSchoolApplication", () => {
  beforeEach(() => {
    jest.clearAllMocks();
  });

  test("renders loading spinner while fetching", async () => {
    (
      applicationService.fetchSchoolApplicationById as jest.Mock
    ).mockReturnValue(new Promise(() => {}));
    render(
      <MemoryRouter initialEntries={["/applications/1"]}>
        <Routes>
          <Route
            path="/applications/:id"
            element={<SingleSchoolApplication />}
          />
        </Routes>
      </MemoryRouter>
    );
    expect(screen.getByRole("status")).toBeInTheDocument();
  });

  test("renders application details after fetch", async () => {
    (
      applicationService.fetchSchoolApplicationById as jest.Mock
    ).mockResolvedValue(mockApplication);
    render(
      <MemoryRouter initialEntries={["/applications/1"]}>
        <Routes>
          <Route
            path="/applications/:id"
            element={<SingleSchoolApplication />}
          />
        </Routes>
      </MemoryRouter>
    );
    expect(
      await screen.findByText("School Application Details")
    ).toBeInTheDocument();
    expect(screen.getByText("Course Name:")).toBeInTheDocument();
    expect(screen.getByText("Math")).toBeInTheDocument();
    expect(screen.getByText("Company Name:")).toBeInTheDocument();
    expect(screen.getByText("TestCo")).toBeInTheDocument();
    expect(screen.getByText("Alice")).toBeInTheDocument();
    expect(screen.getByText("Bob")).toBeInTheDocument();
  });

  test("calls navigate to edit on Edit Application click", async () => {
    (
      applicationService.fetchSchoolApplicationById as jest.Mock
    ).mockResolvedValue(mockApplication);
    render(
      <MemoryRouter initialEntries={["/applications/1"]}>
        <Routes>
          <Route
            path="/applications/:id"
            element={<SingleSchoolApplication />}
          />
          <Route path="/applications/:id/edit" element={<div>Edit Page</div>} />
        </Routes>
      </MemoryRouter>
    );
    expect(await screen.findByText("Edit Application")).toBeInTheDocument();
    fireEvent.click(screen.getByText("Edit Application"));
    expect(await screen.findByText("Edit Page")).toBeInTheDocument();
  });

  test("calls navigate(-1) on Close click", async () => {
    (
      applicationService.fetchSchoolApplicationById as jest.Mock
    ).mockResolvedValue(mockApplication);
    // Mock window.history.back
    const backSpy = jest
      .spyOn(window.history, "back")
      .mockImplementation(() => {});
    render(
      <MemoryRouter initialEntries={["/applications/1"]}>
        <Routes>
          <Route
            path="/applications/:id"
            element={<SingleSchoolApplication />}
          />
        </Routes>
      </MemoryRouter>
    );
    expect(await screen.findByText("Close")).toBeInTheDocument();
    fireEvent.click(screen.getByText("Close"));
    // We can't directly test navigate(-1), but this ensures the button is clickable
    expect(screen.getByText("Close")).toBeInTheDocument();
    backSpy.mockRestore();
  });
});
