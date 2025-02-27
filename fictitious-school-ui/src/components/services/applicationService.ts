import axios from "axios";
import {
  CourseDateDTO,
  CourseDTO,
  FictitiousSchoolApplicationDTO,
} from "../../interfaces/interfaces";

const API_BASE_URL = "https://localhost:7029/api";

export const fetchSchoolApplications = async (): Promise<
  FictitiousSchoolApplicationDTO[]
> => {
  const response = await axios.get(`${API_BASE_URL}/FictitiousSchool`);
  return response.data;
};

export const deleteSchoolApplication = async (
  applicationId: string
): Promise<void> => {
  await axios.delete(`${API_BASE_URL}/FictitiousSchool/${applicationId}`);
};

export const fetchSchoolApplicationById = async (
  id: string
): Promise<FictitiousSchoolApplicationDTO> => {
  const response = await axios.get(`${API_BASE_URL}/FictitiousSchool/${id}`);
  return response.data;
};

export const fetchCourses = async (): Promise<CourseDTO[]> => {
  const response = await axios.get(`${API_BASE_URL}/Course`);
  return response.data;
};

export const fetchCourseDates = async (
  courseId: number
): Promise<CourseDateDTO[]> => {
  const response = await axios.get(
    `${API_BASE_URL}/CourseDate/course/${courseId}`
  );
  return response.data;
};

export const submitSchoolApplication = async (values: any): Promise<void> => {
  await axios.post(`${API_BASE_URL}/FictitiousSchool`, values);
};

export const fetchApplicationById = async (
  id: string
): Promise<FictitiousSchoolApplicationDTO> => {
  const response = await axios.get(`${API_BASE_URL}/FictitiousSchool/${id}`);
  return response.data;
};

export const updateApplication = async (
  id: string,
  values: any
): Promise<void> => {
  await axios.put(`${API_BASE_URL}/FictitiousSchool/${id}`, values);
};
