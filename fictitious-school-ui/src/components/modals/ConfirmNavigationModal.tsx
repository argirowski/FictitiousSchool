import React from "react";
import { Modal, Button } from "react-bootstrap";
import { ConfirmNavigationModalProps } from "../../interfaces/interfaces";

const ConfirmNavigationModal: React.FC<ConfirmNavigationModalProps> = ({
  show,
  onHide,
  onConfirm,
}) => {
  return (
    <Modal show={show} onHide={onHide}>
      <Modal.Header closeButton>
        <Modal.Title>Confirm Navigation</Modal.Title>
      </Modal.Header>
      <Modal.Body>
        Do you really want to go back? You will lose all of your changes.
      </Modal.Body>
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

export default ConfirmNavigationModal;
