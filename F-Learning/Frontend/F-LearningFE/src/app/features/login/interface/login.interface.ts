export interface LoginResponse {
    message: string;
    token: string;
    user: {
      id: string;
      email: string;
      fullName: string;
      phoneNumber: string;
    };
  }
  
  export interface LoginRequest {
    email: string;
    password: string;
  }