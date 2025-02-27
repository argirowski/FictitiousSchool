import React from "react";
import { Spinner, Container, Row, Col } from "react-bootstrap";

const LoadingSpinner: React.FC = () => {
  return (
    <Container
      className="d-flex justify-content-center align-items-center"
      style={{ height: "100vh" }}
    >
      <Row>
        <Col className="text-center">
          <Spinner animation="border" role="status">
            <span className="visually-hidden">Loading...</span>
          </Spinner>
        </Col>
      </Row>
    </Container>
  );
};

export default LoadingSpinner;
