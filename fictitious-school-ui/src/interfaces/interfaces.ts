export interface CourseDTO {
  id: number;
  name: string;
}

export interface CompanyDTO {
  name: string;
  phone: string;
  email: string;
}

export interface ParticipantDTO {
  name: string;
  phone: string;
  email: string;
}

export interface FictitiousSchoolApplicationDTO {
  id: string;
  course: CourseDTO;
  courseDate: CourseDateDTO;
  company: CompanyDTO;
  participants: ParticipantDTO[];
}

export interface CourseDateDTO {
  id: string;
  date: string;
}

export interface ConfirmDeleteModalProps {
  show: boolean;
  onHide: () => void;
  onConfirm: () => void;
}

export interface ConfirmNavigationModalProps {
  show: boolean;
  onHide: () => void;
  onConfirm: () => void;
}

export interface SubmitApplicationDTO {
  courseId: number;
  courseDateId: string;
  company: CompanyDTO;
  participants: ParticipantDTO[];
}

export interface UpdateApplicationDTO {
  id: string;
  courseId: number;
  courseDateId: string;
  company: CompanyDTO;
  participants: ParticipantDTO[];
}
