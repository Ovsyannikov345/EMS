import axios from "axios";
import { useAuth0 } from "@auth0/auth0-react";
import { EstateFullData } from "./useCatalogueApi";
import { UserProfile } from "./useProfileApi";

export interface Message {
    id: string;
    text: string;
    createdAt: Date;
    chatId: string;
    userId: string;
}

export interface Chat {
    id: string;
    estate: EstateFullData;
    user: UserProfile;
    messages: Message[];
}

export interface ApiError {
    error: boolean;
    statusCode?: number;
    message: string;
}

type ApiResponse<T> = T | ApiError;

const useChatApi = () => {
    const { getAccessTokenSilently } = useAuth0();

    const createAxiosInstance = async () => {
        const token = await getAccessTokenSilently();

        const instance = axios.create({
            baseURL: process.env.REACT_APP_CHAT_API_URL,
        });

        instance.interceptors.request.use(async (config) => {
            config.headers.Authorization = `Bearer ${token}`;

            return config;
        });

        return instance;
    };

    const createChat = async (estateId: string): Promise<ApiResponse<Chat>> => {
        const client = await createAxiosInstance();

        try {
            const response = await client.post(`Chat/estate/${estateId}`);

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

    const getUserChats = async (): Promise<ApiResponse<Chat[]>> => {
        const client = await createAxiosInstance();

        try {
            const response = await client.get(`Chat/my`);

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

    const getEstateChats = async (): Promise<ApiResponse<Chat[]>> => {
        const client = await createAxiosInstance();

        try {
            const response = await client.get(`Chat/my-estate`);

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

    const getChatDetails = async (chatId: string): Promise<ApiResponse<Chat>> => {
        const client = await createAxiosInstance();

        try {
            const response = await client.get(`Chat/${chatId}`);

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

    return { createChat, getUserChats, getEstateChats, getChatDetails };
};

export default useChatApi;
