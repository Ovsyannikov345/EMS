import { useAuth0 } from "@auth0/auth0-react";
import axios from "axios";

export interface Notification {
    id: string;
    title: string;
    userId: string;
    isRead: boolean;
    createdAt: Date;
}

export interface ApiError {
    error: boolean;
    statusCode?: number;
    message: string;
}

type ApiResponse<T> = T | ApiError;

const useNotificationsApi = () => {
    const { getAccessTokenSilently } = useAuth0();

    const createAxiosInstance = async () => {
        const token = await getAccessTokenSilently();

        const instance = axios.create({
            baseURL: process.env.REACT_APP_NOTIFICATION_API_URL,
        });

        instance.interceptors.request.use(async (config) => {
            config.headers.Authorization = `Bearer ${token}`;

            return config;
        });

        return instance;
    };

    const getNotificationList = async (): Promise<ApiResponse<Notification[]>> => {
        const client = await createAxiosInstance();

        try {
            const response = await client.get("Notification");

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

    const readNotification = async (id: string): Promise<ApiResponse<Notification>> => {
        const client = await createAxiosInstance();

        try {
            const response = await client.post(`Notification/${id}/read`);

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

    return { getNotificationList, readNotification };
};

export default useNotificationsApi;
