import axios, { AxiosResponse } from "axios";
import { useAuth0 } from "@auth0/auth0-react";

export interface UserProfile {
    id: string;
    auth0Id: string;
    firstName: string;
    lastName: string;
    phoneNumber?: string;
    birthDate?: Date;
    estateCount: number;
    createdAt: Date;
    updatedAt: Date;
}

export interface ProfileImage {
    blob: Blob;
}

export interface InfoVisibilityOptions {
    id: string;
    userId: string;
    phoneNumberVisibility: InfoVisibility;
    birthDateVisibility: InfoVisibility;
    createdAt: Date;
    updatedAt: Date;
}

export enum InfoVisibility {
    Public = 0,
    Private = 1,
}

export interface ApiError {
    error: boolean;
    statusCode?: number;
    message: string;
}

type ApiResponse<T> = T | ApiError;

const useProfileApi = () => {
    const { getAccessTokenSilently } = useAuth0();

    const createAxiosInstance = async () => {
        const token = await getAccessTokenSilently();

        const instance = axios.create({
            baseURL: process.env.REACT_APP_PROFILE_API_URL,
        });

        instance.interceptors.request.use(async (config) => {
            config.headers.Authorization = `Bearer ${token}`;

            return config;
        });

        return instance;
    };

    const getOwnProfile = async (): Promise<ApiResponse<UserProfile>> => {
        const client = await createAxiosInstance();

        try {
            const response = await client.get("Profile/my");

            return response.data;
        } catch (error: any) {
            if (error.response) {
                const { status, data } = error.response;
                return { error: true, statusCode: status, message: data.message ?? "Unknown error" };
            } else {
                return { error: true, message: "An unexpected error occurred." };
            }
        }
    };

    const getProfile = async (id: string): Promise<ApiResponse<UserProfile>> => {
        const client = await createAxiosInstance();

        try {
            const response = await client.get(`Profile/${id}`);

            return response.data;
        } catch (error: any) {
            if (error.response) {
                const { status, data } = error.response;
                return { error: true, statusCode: status, message: data.message ?? "Unknown error" };
            } else {
                return { error: true, message: "An unexpected error occurred." };
            }
        }
    };

    const getProfileImage = async (id: string): Promise<ApiResponse<ProfileImage>> => {
        const client = await createAxiosInstance();

        try {
            const response = await client.get(`ProfileImage/${id}`, {
                responseType: "blob",
            });

            return { blob: response.data };
        } catch (error: any) {
            if (error.response) {
                const { status, data } = error.response;
                return { error: true, statusCode: status, message: data.message ?? "Unknown error" };
            } else {
                return { error: true, message: "An unexpected error occurred." };
            }
        }
    };

    const updateProfile = async (updatedProfile: UserProfile): Promise<ApiResponse<UserProfile>> => {
        const client = await createAxiosInstance();

        try {
            const response = await client.put(`Profile/${updatedProfile.id}`, updatedProfile);

            return response.data;
        } catch (error: any) {
            if (error.response) {
                const { status, data } = error.response;
                return { error: true, statusCode: status, message: data.message ?? "Unknown error" };
            } else {
                return { error: true, message: "An unexpected error occurred." };
            }
        }
    };

    const getProfileVisibility = async (userId: string): Promise<ApiResponse<InfoVisibilityOptions>> => {
        const client = await createAxiosInstance();

        try {
            const response = await client.get(`Profile/${userId}/visibility`);

            return response.data;
        } catch (error: any) {
            if (error.response) {
                const { status, data } = error.response;
                return { error: true, statusCode: status, message: data.message ?? "Unknown error" };
            } else {
                return { error: true, message: "An unexpected error occurred." };
            }
        }
    };

    const updateProfileVisibility = async (updatedOptions: InfoVisibilityOptions): Promise<ApiResponse<InfoVisibilityOptions>> => {
        const client = await createAxiosInstance();

        try {
            const response = await client.put(`Profile/${updatedOptions.userId}/visibility`, updatedOptions);

            return response.data;
        } catch (error: any) {
            if (error.response) {
                const { status, data } = error.response;
                return { error: true, statusCode: status, message: data.message ?? "Unknown error" };
            } else {
                return { error: true, message: "An unexpected error occurred." };
            }
        }
    };

    const updateProfileImage = async (file: File): Promise<ApiResponse<AxiosResponse>> => {
        const client = await createAxiosInstance();

        const formData = new FormData();

        formData.append("file", file);

        try {
            const response = await client.post("ProfileImage", formData, {
                headers: { "Content-Type": "multipart/form-data" },
            });

            return response;
        } catch (error: any) {
            if (error.response) {
                const { status, data } = error.response;
                return { error: true, statusCode: status, message: data.message ?? "Unknown error" };
            } else {
                return { error: true, message: "An unexpected error occurred." };
            }
        }
    };

    return { getOwnProfile, getProfile, getProfileImage, updateProfile, getProfileVisibility, updateProfileVisibility, updateProfileImage };
};

export default useProfileApi;
