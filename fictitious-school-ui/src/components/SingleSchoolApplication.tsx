import React, { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { Container, Card, ListGroup, Button } from "react-bootstrap";
import LoadingSpinner from "./loader/LoadingSpinner";
import { FictitiousSchoolApplicationDTO } from "../interfaces/interfaces";
import { fetchSchoolApplicationById } from "./services/applicationService";

const SingleSchoolApplication: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const [application, setApplication] =
    useState<FictitiousSchoolApplicationDTO | null>(null);
  const navigate = useNavigate();

  useEffect(() => {
    const fetchData = async () => {
      try {
        const data = await fetchSchoolApplicationById(id!);
        setApplication(data);
      } catch (error) {
        console.error("There was an error fetching the data!", error);
      }
    };

    fetchData();
  }, [id]);

  if (!application) {
    return <LoadingSpinner />;
  }

  return (
    <Container className="component-container">
      <Card>
        <Card.Header as="h2">School Application Details</Card.Header>
        <Card.Body>
          <Card.Title>Course Details</Card.Title>
          <Card.Text>
            <span className="form-label">Course Name:</span>{" "}
            <span className="form-value"> {application.course.name}</span>
          </Card.Text>
        </Card.Body>
        <Card className="mt-3">
          <Card.Body>
            <Card.Title>Company Details</Card.Title>
            <Card.Text>
              <span className="form-label">Company Name:</span>{" "}
              <span className="form-value"> {application.company.name}</span>
            </Card.Text>
            <Card.Text>
              <span className="form-label">Company Phone:</span>{" "}
              <span className="form-value"> {application.company.phone}</span>
            </Card.Text>
            <Card.Text>
              <span className="form-label">Company Email:</span>{" "}
              <span className="form-value">{application.company.email}</span>
            </Card.Text>
          </Card.Body>
          <Card className="mt-3">
            <Card.Body>
              <Card.Title>Participants</Card.Title>
              <ListGroup variant="flush">
                {application.participants.map((participant, index) => (
                  <ListGroup.Item key={index}>
                    <span className="form-label">Name:</span>
                    <span className="form-value">{participant.name}</span>
                    <br />
                    <span className="form-label">Phone:</span>{" "}
                    <span className="form-value"> {participant.phone}</span>
                    <br />
                    <span className="form-label">Email:</span>{" "}
                    <span className="form-value"> {participant.email}</span>
                  </ListGroup.Item>
                ))}
              </ListGroup>
            </Card.Body>
          </Card>
        </Card>
      </Card>
      <div className="d-flex justify-content-start mt-3">
        <Button
          variant="primary"
          onClick={() => navigate(`/applications/${application.id}/edit`)}
          className="me-3"
          size="lg"
        >
          Edit Book
        </Button>
        <Button variant="secondary" onClick={() => navigate(-1)} size="lg">
          Close
        </Button>
      </div>
    </Container>
  );
};

export default SingleSchoolApplication;
