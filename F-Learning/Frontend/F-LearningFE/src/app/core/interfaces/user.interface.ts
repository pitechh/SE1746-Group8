export interface AuthenticatedUser {
  id: string;
  email: string;
  fullName: string;
  phoneNumber: string;
  roles: string[];
}