import React from "react";
import { Modal, Button } from "react-bootstrap";
import { ConfirmDeleteModalProps } from "../../interfaces/interfaces";

const ConfirmDeleteModal: React.FC<ConfirmDeleteModalProps> = ({
  show,
  onHide,
  onConfirm,
}) => {
  return (
    <Modal show={show} onHide={onHide}>
      <Modal.Header closeButton>
        <Modal.Title>Confirm Delete</Modal.Title>
      </Modal.Header>
      <Modal.Body>Are you sure you want to delete this item?</Modal.Body>
      <Modal.Footer>
        <Button variant="danger" onClick={onConfirm}>
          Yes
        </Button>
        <Button variant="secondary" onClick={onHide}>
          No
        </Button>
      </Modal.Footer>
    </Modal>
  );
};

export default ConfirmDeleteModal;
