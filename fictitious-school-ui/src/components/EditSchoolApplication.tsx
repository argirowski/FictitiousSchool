import React, { useState, useEffect } from "react";
import axios from "axios";
import { Button, Col, Container, Form, Row } from "react-bootstrap";
import {
  Formik,
  Field,
  FieldArray,
  Form as FormikForm,
  ErrorMessage,
} from "formik";
import { useNavigate, useParams } from "react-router-dom";
import ConfirmNavigationModal from "./modals/ConfirmNavigationModal";
import LoadingSpinner from "./loader/LoadingSpinner";
import {
  CourseDateDTO,
  CourseDTO,
  FictitiousSchoolApplicationDTO,
} from "../interfaces/interfaces";
import { validationSchema } from "../utils/formUtils";

const EditSchoolApplication: React.FC = () => {
  const [courses, setCourses] = useState<CourseDTO[]>([]);
  const [courseDates, setCourseDates] = useState<CourseDateDTO[]>([]);
  const [application, setApplication] =
    useState<FictitiousSchoolApplicationDTO | null>(null);
  const [showModal, setShowModal] = useState(false);
  const [isFormDirty, setIsFormDirty] = useState(false);
  const navigate = useNavigate();
  const { id } = useParams<{ id: string }>();

  useEffect(() => {
    // Fetch courses
    axios
      .get("https://localhost:7029/api/Course")
      .then((response) => {
        setCourses(response.data);
        console.log("Courses fetched:", response.data);
      })
      .catch((error) => {
        console.error("There was an error fetching the courses!", error);
      });

    // Fetch application data
    axios
      .get(`https://localhost:7029/api/FictitiousSchool/${id}`)
      .then((response) => {
        setApplication(response.data);
        console.log("Application fetched:", response.data);
        // Fetch course dates based on the course ID in the application
        return axios.get(
          `https://localhost:7029/api/CourseDate/course/${response.data.course.id}`
        );
      })
      .then((response) => {
        setCourseDates(response.data);
        console.log("Course dates fetched:", response.data);
      })
      .catch((error) => {
        console.error(
          "There was an error fetching the application data!",
          error
        );
      });
  }, [id]);

  const handleCourseChange = (event: React.ChangeEvent<HTMLSelectElement>) => {
    const courseId = parseInt(event.target.value, 10);
    setApplication((prev) =>
      prev
        ? {
            ...prev,
            course: { ...prev.course, id: courseId },
            courseDate: { id: "", date: "" },
          }
        : null
    );

    // Fetch course dates based on selected course ID
    axios
      .get(`https://localhost:7029/api/CourseDate/course/${courseId}`)
      .then((response) => {
        setCourseDates(response.data);
        console.log("Course dates fetched on change:", response.data);
      })
      .catch((error) => {
        console.error("There was an error fetching the course dates!", error);
      });
  };

  const handleSubmit = (values: any) => {
    axios
      .put(`https://localhost:7029/api/FictitiousSchool/${id}`, values)
      .then((response) => {
        console.log("Application updated successfully:", response.data);
        navigate("/applications"); // Navigate to the applications list
      })
      .catch((error) => {
        console.error("There was an error updating the application!", error);
      });
  };

  const handleGoBack = () => {
    if (isFormDirty) {
      setShowModal(true);
    } else {
      navigate(-1);
    }
  };

  const handleConfirmNavigation = () => {
    setShowModal(false);
    navigate(-1);
  };

  if (!application) {
    return <LoadingSpinner />;
  }

  return (
    <Container className="component-container">
      <Row>
        <Col md={{ span: 8, offset: 2 }}>
          <h2>Edit Application</h2>
        </Col>
      </Row>
      <Formik
        initialValues={application}
        validationSchema={validationSchema}
        onSubmit={handleSubmit}
      >
        {({ values, handleChange }) => {
          console.log("Formik values:", values);
          return (
            <FormikForm>
              <Row>
                <Col md={{ span: 4 }}>
                  <Form.Group controlId="formCourseId">
                    <Form.Label>Course Name</Form.Label>
                    <Field
                      as="select"
                      name="course.id"
                      className="form-control"
                      value={values.course.id}
                      onChange={(e: React.ChangeEvent<HTMLSelectElement>) => {
                        handleCourseChange(e);
                        handleChange(e);
                        setIsFormDirty(true);
                      }}
                    >
                      <option value="">Select a course</option>
                      {courses.map((course) => (
                        <option key={course.id} value={course.id}>
                          {course.name}
                        </option>
                      ))}
                    </Field>
                    <ErrorMessage
                      name="course.id"
                      component="div"
                      className="text-danger"
                    />
                  </Form.Group>
                </Col>
                <Col md={{ span: 4 }}>
                  <Form.Group controlId="formCourseDateId">
                    <Form.Label>Course Date</Form.Label>
                    <Field
                      as="select"
                      name="courseDate.id"
                      className="form-control"
                      value={values.courseDate.id}
                      onChange={(e: React.ChangeEvent<HTMLSelectElement>) => {
                        handleChange(e);
                        setIsFormDirty(true);
                      }}
                    >
                      <option value="">Select a course date</option>
                      {courseDates.map((courseDate) => (
                        <option key={courseDate.id} value={courseDate.id}>
                          {new Date(courseDate.date).toLocaleDateString()}
                        </option>
                      ))}
                    </Field>
                    <ErrorMessage
                      name="courseDate.id"
                      component="div"
                      className="text-danger"
                    />
                  </Form.Group>
                </Col>
              </Row>
              <Row>
                <Col md={{ span: 8 }}>
                  <Form.Group controlId="formCompanyName">
                    <Form.Label>Company Name</Form.Label>
                    <Field
                      type="text"
                      name="company.name"
                      className="form-control"
                      value={values.company.name}
                      onChange={(e: React.ChangeEvent<HTMLInputElement>) => {
                        handleChange(e);
                        setIsFormDirty(true);
                      }}
                    />
                    <ErrorMessage
                      name="company.name"
                      component="div"
                      className="text-danger"
                    />
                  </Form.Group>
                </Col>
              </Row>
              <Row>
                <Col md={{ span: 4 }}>
                  <Form.Group controlId="formCompanyPhone">
                    <Form.Label>Company Phone</Form.Label>
                    <Field
                      type="text"
                      name="company.phone"
                      className="form-control"
                      value={values.company.phone}
                      onChange={(e: React.ChangeEvent<HTMLInputElement>) => {
                        handleChange(e);
                        setIsFormDirty(true);
                      }}
                    />
                    <ErrorMessage
                      name="company.phone"
                      component="div"
                      className="text-danger"
                    />
                  </Form.Group>
                </Col>
                <Col md={{ span: 4 }}>
                  <Form.Group controlId="formCompanyEmail">
                    <Form.Label>Company Email</Form.Label>
                    <Field
                      type="email"
                      name="company.email"
                      className="form-control"
                      value={values.company.email}
                      onChange={(e: React.ChangeEvent<HTMLInputElement>) => {
                        handleChange(e);
                        setIsFormDirty(true);
                      }}
                    />
                    <ErrorMessage
                      name="company.email"
                      component="div"
                      className="text-danger"
                    />
                  </Form.Group>
                </Col>
              </Row>

              <FieldArray name="participants">
                {({ push, remove }) => (
                  <>
                    {values.participants.map((participant, index) => (
                      <div key={index}>
                        <Row className="mt-5">
                          <Col md={{ span: 8 }}>
                            <h5>Participant #{index + 1}</h5>
                          </Col>
                          <Row>
                            <Col md={{ span: 8, offset: 0 }}>
                              <Form.Group
                                controlId={`formParticipantName${index}`}
                              >
                                <Form.Label>Name</Form.Label>
                                <Field
                                  type="text"
                                  name={`participants.${index}.name`}
                                  className="form-control"
                                  value={participant.name}
                                  onChange={(
                                    e: React.ChangeEvent<HTMLInputElement>
                                  ) => {
                                    handleChange(e);
                                    setIsFormDirty(true);
                                  }}
                                />
                                <ErrorMessage
                                  name={`participants.${index}.name`}
                                  component="div"
                                  className="text-danger"
                                />
                              </Form.Group>
                            </Col>
                          </Row>
                          <Col md={{ span: 4 }}>
                            <Form.Group
                              controlId={`formParticipantPhone${index}`}
                            >
                              <Form.Label>Phone</Form.Label>
                              <Field
                                type="text"
                                name={`participants.${index}.phone`}
                                className="form-control"
                                value={participant.phone}
                                onChange={(
                                  e: React.ChangeEvent<HTMLInputElement>
                                ) => {
                                  handleChange(e);
                                  setIsFormDirty(true);
                                }}
                              />
                              <ErrorMessage
                                name={`participants.${index}.phone`}
                                component="div"
                                className="text-danger"
                              />
                            </Form.Group>
                          </Col>
                          <Col md={{ span: 4 }}>
                            <Form.Group
                              controlId={`formParticipantEmail${index}`}
                            >
                              <Form.Label>Email</Form.Label>
                              <Field
                                type="email"
                                name={`participants.${index}.email`}
                                className="form-control"
                                value={participant.email}
                                onChange={(
                                  e: React.ChangeEvent<HTMLInputElement>
                                ) => {
                                  handleChange(e);
                                  setIsFormDirty(true);
                                }}
                              />
                              <ErrorMessage
                                name={`participants.${index}.email`}
                                component="div"
                                className="text-danger"
                              />
                            </Form.Group>
                          </Col>
                        </Row>
                        <Button
                          variant="danger"
                          onClick={() => remove(index)}
                          className="mt-3"
                        >
                          Remove Participant
                        </Button>
                      </div>
                    ))}
                    <Row className="mt-3">
                      <Col md={{ span: 8 }}>
                        <Button
                          variant="dark"
                          onClick={() =>
                            push({ name: "", phone: "", email: "" })
                          }
                        >
                          Add Participant
                        </Button>
                      </Col>
                    </Row>
                  </>
                )}
              </FieldArray>

              <Row className="mt-3">
                <Col md={{ span: 8 }} className="d-flex justify-content-start">
                  <Button
                    variant="outline-success"
                    type="submit"
                    className="me-3"
                    size="lg"
                  >
                    Update Application
                  </Button>
                  <Button
                    variant="outline-dark"
                    onClick={handleGoBack}
                    size="lg"
                  >
                    Go Back
                  </Button>
                </Col>
              </Row>
            </FormikForm>
          );
        }}
      </Formik>
      <ConfirmNavigationModal
        show={showModal}
        onHide={() => setShowModal(false)}
        onConfirm={handleConfirmNavigation}
      />
    </Container>
  );
};

export default EditSchoolApplication;
