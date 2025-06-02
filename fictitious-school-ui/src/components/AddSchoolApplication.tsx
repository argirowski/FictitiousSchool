import React, { useState, useEffect } from "react";
import { Button, Col, Container, Form, Row } from "react-bootstrap";
import {
  Formik,
  Field,
  FieldArray,
  Form as FormikForm,
  ErrorMessage,
} from "formik";
import { useNavigate } from "react-router-dom";
import {
  handleGoBack,
  handleModalClose,
  handleModalConfirm,
  validationSchema,
} from "../utils/formUtils";
import ConfirmNavigationModal from "./modals/ConfirmNavigationModal";
import {
  CourseDTO,
  CourseDateDTO,
  SubmitApplicationDTO,
} from "../interfaces/interfaces";
import {
  fetchCourses,
  fetchCourseDates,
  submitSchoolApplication,
} from "./services/applicationService";

const AddSchoolApplication: React.FC = () => {
  const [courses, setCourses] = useState<CourseDTO[]>([]);
  const [courseDates, setCourseDates] = useState<CourseDateDTO[]>([]);
  const [selectedCourseId, setSelectedCourseId] = useState<number | null>(null);
  const [showModal, setShowModal] = useState(false);
  const [isFormDirty, setIsFormDirty] = useState(false);
  const navigate = useNavigate();

  useEffect(() => {
    const fetchData = async () => {
      try {
        const coursesData = await fetchCourses();
        setCourses(coursesData);
      } catch (error) {
        console.error("There was an error fetching the courses!", error);
      }
    };

    fetchData();
  }, []);

  const handleCourseChange = async (
    event: React.ChangeEvent<HTMLSelectElement>
  ) => {
    const courseId = parseInt(event.target.value, 10);
    setSelectedCourseId(courseId);

    try {
      const courseDatesData = await fetchCourseDates(courseId);
      setCourseDates(courseDatesData);
    } catch (error) {
      console.error("There was an error fetching the course dates!", error);
    }
  };

  const initialValues: SubmitApplicationDTO = {
    courseId: 0,
    courseDateId: "",
    company: {
      name: "",
      phone: "",
      email: "",
    },
    participants: [
      {
        name: "",
        phone: "",
        email: "",
      },
    ],
  };

  const handleSubmit = async (values: SubmitApplicationDTO) => {
    try {
      await submitSchoolApplication(values);
      console.log("Application submitted successfully");
      navigate("/applications");
    } catch (error) {
      console.error("There was an error submitting the application!", error);
    }
  };

  return (
    <Container className="component-container">
      <Row>
        <Col md={{ span: 8, offset: 2 }}>
          <h2>Add New Application</h2>
        </Col>
      </Row>
      <Formik
        initialValues={initialValues}
        validationSchema={validationSchema}
        onSubmit={handleSubmit}
      >
        {({ values, handleChange }) => (
          <FormikForm>
            <Row>
              <Col md={{ span: 4 }}>
                <Form.Group controlId="formCourseId">
                  <Form.Label>Course Name</Form.Label>
                  <Field
                    as="select"
                    name="courseId"
                    className="form-control"
                    onChange={(e: React.ChangeEvent<HTMLSelectElement>) => {
                      handleCourseChange(e);
                      values.courseId = parseInt(e.target.value, 10);
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
                    name="courseId"
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
                    name="courseDateId"
                    className="form-control"
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
                    name="courseDateId"
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
                        onClick={() => push({ name: "", phone: "", email: "" })}
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
                  Add Application
                </Button>
                <Button
                  variant="outline-dark"
                  onClick={handleGoBack(isFormDirty, setShowModal, navigate)}
                  size="lg"
                >
                  Go Back
                </Button>
              </Col>
            </Row>
          </FormikForm>
        )}
      </Formik>
      <ConfirmNavigationModal
        show={showModal}
        onHide={handleModalClose(setShowModal)}
        onConfirm={handleModalConfirm(setShowModal, navigate)}
      />
    </Container>
  );
};

export default AddSchoolApplication;
