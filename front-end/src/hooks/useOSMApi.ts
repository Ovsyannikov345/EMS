import axios from "axios";
import OsmQueryParamNames from "../utils/osmQueryParamNames";

export interface Address {
    country: string;
    city: string;
    road: string;
    house_number: string;
}

export interface AddressResponse {
    address: Address;
}

export interface ApiError {
    error: boolean;
    statusCode?: number;
    message: string;
}

type ApiResponse<T> = T | ApiError;

const useOSMApi = () => {
    const createAxiosInstance = async () => {
        const instance = axios.create({
            baseURL: process.env.REACT_APP_OSM_API_URL,
        });

        return instance;
    };

    const getAddressFromCoordinates = async (lat: number, lng: number): Promise<ApiResponse<AddressResponse>> => {
        const client = await createAxiosInstance();

        try {
            const response = await client.get(
                "/reverse?" +
                    [
                        `${OsmQueryParamNames.LATITUDE}=${lat}`,
                        `${OsmQueryParamNames.LONGITUDE}=${lng}`,
                        `${OsmQueryParamNames.RESPONSE_FORMAT}=json`,
                        `${OsmQueryParamNames.RESPONSE_LANGUAGE}=en-us`,
                    ].join("&")
            );

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

    return { getAddressFromCoordinates };
};

export default useOSMApi;
