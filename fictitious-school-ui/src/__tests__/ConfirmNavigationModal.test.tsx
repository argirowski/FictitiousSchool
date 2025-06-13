import React from "react";
import { render, screen, fireEvent } from "@testing-library/react";
import ConfirmNavigationModal from "../components/modals/ConfirmNavigationModal";

describe("ConfirmNavigationModal", () => {
  const onHide = jest.fn();
  const onConfirm = jest.fn();

  const setup = (show = true) =>
    render(
      <ConfirmNavigationModal
        show={show}
        onHide={onHide}
        onConfirm={onConfirm}
      />
    );

  afterEach(() => {
    jest.clearAllMocks();
  });

  test("renders modal with correct text when shown", () => {
    setup();
    expect(screen.getByText("Confirm Navigation")).toBeInTheDocument();
    expect(
      screen.getByText(
        "Do you really want to go back? You will lose all of your changes."
      )
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
    expect(screen.queryByText("Confirm Navigation")).not.toBeInTheDocument();
  });
});
