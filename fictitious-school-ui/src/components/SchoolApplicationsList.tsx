import React, { useEffect, useState } from "react";
import { Container, Table, Button, Row, Col } from "react-bootstrap";
import { useNavigate } from "react-router-dom";
import ConfirmDeleteModal from "./modals/ConfirmDeleteModal";
import LoadingSpinner from "./loader/LoadingSpinner";
import { FictitiousSchoolApplicationDTO } from "../interfaces/interfaces";
import {
  deleteSchoolApplication,
  fetchSchoolApplications,
} from "./services/applicationService";

const SchoolApplicationsList: React.FC = () => {
  const [applications, setApplications] = useState<
    FictitiousSchoolApplicationDTO[]
  >([]);
  const [loading, setLoading] = useState<boolean>(true);
  const [deleting, setDeleting] = useState<boolean>(false);
  const [showDeleteModal, setShowDeleteModal] = useState(false);
  const [selectedApplicationId, setSelectedApplicationId] = useState<
    string | null
  >(null);
  const navigate = useNavigate();

  useEffect(() => {
    const fetchData = async () => {
      try {
        const data = await fetchSchoolApplications();
        setApplications(data);
        setLoading(false);
      } catch (error) {
        console.error("There was an error fetching the data!", error);
        setLoading(false);
      }
    };

    fetchData();
  }, []);

  const handleView = (applicationId: string) => {
    navigate(`/applications/${applicationId}`);
  };

  const handleEdit = (applicationId: string) => {
    navigate(`/applications/${applicationId}/edit`);
  };

  const handleDelete = (applicationId: string) => {
    setSelectedApplicationId(applicationId);
    setShowDeleteModal(true);
  };

  const confirmDelete = async () => {
    if (selectedApplicationId) {
      setDeleting(true);
      try {
        await deleteSchoolApplication(selectedApplicationId);
        setApplications(
          applications.filter((app) => app.id !== selectedApplicationId)
        );
        setShowDeleteModal(false);
        setSelectedApplicationId(null);
        setDeleting(false);
      } catch (error) {
        console.error("There was an error deleting the application!", error);
        setDeleting(false);
      }
    }
  };

  if (loading || deleting) {
    return <LoadingSpinner />;
  }

  return (
    <Container className="component-container">
      <Table striped bordered responsive>
        <thead>
          <tr>
            <th>Course Name</th>
            <th>Company Name</th>
            <th>Company Email</th>
            <th>Participants</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          {applications.map((application) => (
            <React.Fragment key={application.id}>
              <tr>
                <td>{application.course.name}</td>
                <td>{application.company.name}</td>
                <td>{application.company.email}</td>
                <td>
                  <Table striped bordered responsive>
                    <tbody>
                      {application.participants.map((participant, index) => (
                        <tr key={index}>
                          <td>{participant.name}</td>
                        </tr>
                      ))}
                    </tbody>
                  </Table>
                </td>
                <td>
                  <Button
                    variant="primary"
                    onClick={() => handleView(application.id)}
                  >
                    View
                  </Button>{" "}
                  <Button
                    variant="secondary"
                    onClick={() => handleEdit(application.id)}
                  >
                    Edit
                  </Button>{" "}
                  <Button
                    variant="danger"
                    onClick={() => handleDelete(application.id)}
                  >
                    Delete
                  </Button>
                </td>
              </tr>
            </React.Fragment>
          ))}
        </tbody>
      </Table>
      <Row className="mt-3">
        <Col className="d-flex justify-content-start">
          <Button
            variant="outline-success"
            onClick={() => navigate("/applications/add")}
            className="me-3"
            size="lg"
          >
            Add New Application
          </Button>
          <Button
            variant="outline-dark"
            onClick={() => navigate("/")}
            size="lg"
          >
            Go to Home Page
          </Button>
        </Col>
      </Row>

      <ConfirmDeleteModal
        show={showDeleteModal}
        onHide={() => setShowDeleteModal(false)}
        onConfirm={confirmDelete}
      />
    </Container>
  );
};

export default SchoolApplicationsList;
