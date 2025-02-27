import { NavigateFunction } from "react-router-dom";
import * as Yup from "yup";

export const handleModalClose = (setShowModal: (show: boolean) => void) => () =>
  setShowModal(false);

export const handleModalConfirm =
  (setShowModal: (show: boolean) => void, navigate: NavigateFunction) => () => {
    setShowModal(false);
    navigate(-1);
  };

export const handleGoBack =
  (
    isFormDirty: boolean,
    setShowModal: (show: boolean) => void,
    navigate: NavigateFunction
  ) =>
  () => {
    if (isFormDirty) {
      setShowModal(true);
    } else {
      navigate(-1);
    }
  };

export const validationSchema = Yup.object().shape({
  courseId: Yup.number()
    .moreThan(0, "Course is required")
    .required("Course is required"),
  courseDateId: Yup.string().required("Course date is required"),
  company: Yup.object().shape({
    name: Yup.string().required("Company name is required"),
    phone: Yup.string().required("Company phone is required"),
    email: Yup.string()
      .email("Invalid email")
      .required("Company email is required"),
  }),
  participants: Yup.array().of(
    Yup.object().shape({
      name: Yup.string().required("Participant name is required"),
      phone: Yup.string().required("Participant phone is required"),
      email: Yup.string()
        .email("Invalid email")
        .required("Participant email is required"),
    })
  ),
});
