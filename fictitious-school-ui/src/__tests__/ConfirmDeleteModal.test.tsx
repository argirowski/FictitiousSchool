import React from "react";
import { render, screen, fireEvent } from "@testing-library/react";
import ConfirmDeleteModal from "../components/modals/ConfirmDeleteModal";

describe("ConfirmDeleteModal", () => {
  const onHide = jest.fn();
  const onConfirm = jest.fn();

  const setup = (show = true) =>
    render(
      <ConfirmDeleteModal show={show} onHide={onHide} onConfirm={onConfirm} />
    );

  afterEach(() => {
    jest.clearAllMocks();
  });

  test("renders modal with correct text when shown", () => {
    setup();
    expect(screen.getByText("Confirm Delete")).toBeInTheDocument();
    expect(
      screen.getByText("Are you sure you want to delete this item?")
    ).toBeInTheDocument();
    expect(screen.getByText("Yes")).toBeInTheDocument();
    expect(screen.getByText("No")).toBeInTheDocument();
  });

  test("calls onConfirm when Yes button is clicked", () => {
    setup();
    fireEvent.click(screen.getByText("Yes"));
    expect(onConfirm).toHaveBeenCalledTimes(1);
  });

  test("calls onHide when No button is clicked", () => {
    setup();
    fireEvent.click(screen.getByText("No"));
    expect(onHide).toHaveBeenCalledTimes(1);
  });

  test("does not render modal when show is false", () => {
    setup(false);
    // Modal content should not be in the document
    expect(screen.queryByText("Confirm Delete")).not.toBeInTheDocument();
  });
});
