// manage-user.ts
export interface ManageUser {
    id: string;
    email: string;
    fullName: string;
    phoneNumber: string;
    createdAt: string;
    isActive: boolean;
    roles?: string[]; // optional, chỉ xuất hiện trong JSON đầu tiên
    userName?: string; // optional, chỉ xuất hiện trong JSON thứ ba
    normalizedUserName?: string; // optional
    normalizedEmail?: string; // optional
    emailConfirmed?: boolean; // optional
    passwordHash?: string; // optional
    securityStamp?: string; // optional
    concurrencyStamp?: string; // optional
    phoneNumberConfirmed?: boolean; // optional
    twoFactorEnabled?: boolean; // optional
    lockoutEnd?: string; // optional
    lockoutEnabled?: boolean; // optional
    accessFailedCount?: number; // optional
}