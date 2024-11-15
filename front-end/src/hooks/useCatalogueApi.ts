import axios from "axios";
import { useAuth0 } from "@auth0/auth0-react";

interface EstateShortData {
    id: string;
    userId: string;
    type: number;
    address: string;
    area: number;
    roomsCount: number;
    price: number;
}

interface ApiError {
    error: boolean;
    statusCode?: number;
    message: string;
}

type ApiResponse<T> = T | ApiError;

const useCatalogueApi = () => {
    const { getAccessTokenSilently } = useAuth0();

    const createAxiosInstance = async () => {
        const token = await getAccessTokenSilently();

        console.log(token);

        const instance = axios.create({
            baseURL: process.env.REACT_APP_CATALOGUE_API_URL,
        });

        instance.interceptors.request.use(async (config) => {
            config.headers.Authorization = `Bearer ${token}`;

            return config;
        });

        return instance;
    };

    const getEstateList = async (): Promise<ApiResponse<EstateShortData[]>> => {
        const client = await createAxiosInstance();

        console.log(client);

        try {
            const response = await client.get("Estate");

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

    return { getEstateList };
};

export default useCatalogueApi;
