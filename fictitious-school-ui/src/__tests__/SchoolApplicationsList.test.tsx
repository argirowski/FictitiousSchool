import React from "react";
import { render, screen, fireEvent, waitFor } from "@testing-library/react";
import SchoolApplicationsList from "../components/SchoolApplicationsList";
import * as applicationService from "../components/services/applicationService";
import { MemoryRouter } from "react-router-dom";

jest.mock("../components/services/applicationService");

const mockApplications = [
  {
    id: "1",
    course: { id: 1, name: "Math" },
    courseDate: { id: "cd1", date: "2025-06-13T00:00:00Z" },
    company: { name: "TestCo", phone: "123456", email: "test@com.com" },
    participants: [
      { name: "Alice", phone: "111", email: "alice@com.com" },
      { name: "Bob", phone: "222", email: "bob@com.com" },
    ],
  },
];

describe("SchoolApplicationsList", () => {
  beforeEach(() => {
    jest.clearAllMocks();
  });

  test("renders loading spinner while fetching", async () => {
    (applicationService.fetchSchoolApplications as jest.Mock).mockReturnValue(
      new Promise(() => {})
    );
    render(
      <MemoryRouter>
        <SchoolApplicationsList />
      </MemoryRouter>
    );
    expect(screen.getByRole("status")).toBeInTheDocument();
  });

  test("renders applications after fetch", async () => {
    (applicationService.fetchSchoolApplications as jest.Mock).mockResolvedValue(
      mockApplications
    );
    render(
      <MemoryRouter>
        <SchoolApplicationsList />
      </MemoryRouter>
    );
    expect(await screen.findByText("Math")).toBeInTheDocument();
    expect(screen.getByText("TestCo")).toBeInTheDocument();
    expect(screen.getByText("test@com.com")).toBeInTheDocument();
    expect(screen.getByText("View")).toBeInTheDocument();
    expect(screen.getByText("Edit")).toBeInTheDocument();
    expect(screen.getByText("Delete")).toBeInTheDocument();
  });

  test("opens and closes delete modal", async () => {
    (applicationService.fetchSchoolApplications as jest.Mock).mockResolvedValue(
      mockApplications
    );
    render(
      <MemoryRouter>
        <SchoolApplicationsList />
      </MemoryRouter>
    );
    expect(await screen.findByText("Delete")).toBeInTheDocument();
    fireEvent.click(screen.getByText("Delete"));
    expect(screen.getByText("Confirm Delete")).toBeInTheDocument();
    fireEvent.click(screen.getByText("No"));
    await waitFor(() =>
      expect(screen.queryByText("Confirm Delete")).not.toBeInTheDocument()
    );
  });

  test("calls deleteSchoolApplication and removes row on confirm", async () => {
    (applicationService.fetchSchoolApplications as jest.Mock).mockResolvedValue(
      mockApplications
    );
    (applicationService.deleteSchoolApplication as jest.Mock).mockResolvedValue(
      undefined
    );
    render(
      <MemoryRouter>
        <SchoolApplicationsList />
      </MemoryRouter>
    );
    expect(await screen.findByText("Delete")).toBeInTheDocument();
    fireEvent.click(screen.getByText("Delete"));
    expect(screen.getByText("Confirm Delete")).toBeInTheDocument();
    fireEvent.click(screen.getByText("Yes"));
    await waitFor(() =>
      expect(screen.queryByText("Math")).not.toBeInTheDocument()
    );
    expect(applicationService.deleteSchoolApplication).toHaveBeenCalledWith(
      "1"
    );
  });
});
