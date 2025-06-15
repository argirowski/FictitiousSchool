import React from "react";
import { render, screen, fireEvent, waitFor } from "@testing-library/react";
import AddSchoolApplication from "../components/AddSchoolApplication";
import * as applicationService from "../components/services/applicationService";
import { MemoryRouter } from "react-router-dom";

jest.mock("../components/services/applicationService");

const mockCourses = [
  { id: 1, name: "Math" },
  { id: 2, name: "Science" },
];
const mockCourseDates = [
  { id: "cd1", date: "2025-06-13T00:00:00Z" },
  { id: "cd2", date: "2025-07-01T00:00:00Z" },
];

describe("AddSchoolApplication", () => {
  beforeEach(() => {
    jest.clearAllMocks();
    (applicationService.fetchCourses as jest.Mock).mockResolvedValue(
      mockCourses
    );
    (applicationService.fetchCourseDates as jest.Mock).mockResolvedValue(
      mockCourseDates
    );
    (applicationService.submitSchoolApplication as jest.Mock).mockResolvedValue(
      undefined
    );
  });

  test("renders form and loads courses", async () => {
    render(
      <MemoryRouter>
        <AddSchoolApplication />
      </MemoryRouter>
    );
    expect(await screen.findByText("Add New Application")).toBeInTheDocument();
    expect(await screen.findByText("Math")).toBeInTheDocument();
    expect(await screen.findByText("Science")).toBeInTheDocument();
  });

  test("loads course dates when course is selected", async () => {
    render(
      <MemoryRouter>
        <AddSchoolApplication />
      </MemoryRouter>
    );
    fireEvent.change(await screen.findByLabelText("Course Name"), {
      target: { value: "1" },
    });
    expect(
      await screen.findByText(
        new Date(mockCourseDates[0].date).toLocaleDateString()
      )
    ).toBeInTheDocument();
  });

  test("submits form with valid data", async () => {
    render(
      <MemoryRouter>
        <AddSchoolApplication />
      </MemoryRouter>
    );
    fireEvent.change(await screen.findByLabelText("Course Name"), {
      target: { value: "1" },
    });
    fireEvent.change(await screen.findByLabelText("Course Date"), {
      target: { value: "cd1" },
    });
    fireEvent.change(screen.getByLabelText("Company Name"), {
      target: { value: "TestCo" },
    });
    fireEvent.change(screen.getByLabelText("Company Phone"), {
      target: { value: "123456" },
    });
    fireEvent.change(screen.getByLabelText("Company Email"), {
      target: { value: "test@co.com" },
    });
    fireEvent.change(screen.getByLabelText("Name"), {
      target: { value: "Alice" },
    });
    fireEvent.change(screen.getByLabelText("Phone"), {
      target: { value: "111" },
    });
    fireEvent.change(screen.getByLabelText("Email"), {
      target: { value: "alice@co.com" },
    });
    fireEvent.click(screen.getByText("Add Application"));
    await waitFor(() => {
      expect(applicationService.submitSchoolApplication).toHaveBeenCalled();
    });
  });

  test("shows validation errors if required fields are missing", async () => {
    render(
      <MemoryRouter>
        <AddSchoolApplication />
      </MemoryRouter>
    );
    fireEvent.click(await screen.findByText("Add Application"));
    expect(await screen.findAllByText(/required/i)).not.toHaveLength(0);
  });
});
