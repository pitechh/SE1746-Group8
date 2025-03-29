// auth.interface.ts
export interface RegisterRequest {
    fullName: string;
    email: string;
    password: string;
    confirmPassword: string;
    phoneNumber: string;
  }
  
  export interface RegisterResponse {
    message: string;
    userId?: string;
  }