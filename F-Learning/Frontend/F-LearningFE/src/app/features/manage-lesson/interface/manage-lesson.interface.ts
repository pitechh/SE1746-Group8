export interface Lesson {
  id: number;
  title: string;
  content: string;
  videoUrl: string;
  order: number;
  courseId: number;
}

export interface LessonForm {
  title: string;
  content: string;
  videoUrl: string;
  order: number;
  courseId : number;
}
