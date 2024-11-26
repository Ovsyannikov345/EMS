import axios, { AxiosResponse } from "axios";
import { useAuth0 } from "@auth0/auth0-react";
import { SortOption } from "../pages/CataloguePage";
import QueryParamNames from "../utils/queryParamNames";
import { UserProfile } from "./useProfileApi";

export enum EstateType {
    None = 0,
    Apartment = 1 << 0,
    House = 1 << 1,
}

export interface EstateShortData {
    id: string;
    userId: string;
    type: EstateType;
    address: string;
    area: number;
    roomsCount: number;
    price: number;
    imageIds: string[];
    createdAt: Date;
    updatedAt: Date;
}

export interface EstateQueryFilter {
    types: EstateType;
    address?: string;
    minArea?: number;
    maxArea?: number;
    minRoomsCount?: number;
    maxRoomsCount?: number;
    minPrice?: number;
    maxPrice?: number;
    userId?: string;
}

export interface PagedResult<T> {
    results: T[];
    currentPage: number;
    pageSize: number;
    totalPages: number;
    totalCount: number;
}

export interface EstateFullData {
    id: string;
    userId: string;
    type: EstateType;
    address: string;
    area: number;
    roomsCount: number;
    price: number;
    createdAt: Date;
    updatedAt: Date;
    user: UserProfile;
    imageIds: string[];
}

export interface EstateToUpdateData {
    id: string;
    userId: string;
    type: EstateType;
    address: string;
    area: number;
    roomsCount: number;
    price: number;
}

export interface EstateImage {
    id: string;
}

export interface ApiError {
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

    const getEstateList = async (
        pageNumber: number,
        sortOption: SortOption,
        filter: EstateQueryFilter
    ): Promise<ApiResponse<PagedResult<EstateShortData>>> => {
        const client = await createAxiosInstance();

        try {
            const filterQuery = Object.entries(filter)
                .filter(([, value]) => value)
                .map(([key, value]) => `${key}=${value}`)
                .join("&");

            const response = await client.get(
                `Estate?${QueryParamNames.PAGE_NUMBER}=${pageNumber}&${QueryParamNames.SORT_OPTION}=${sortOption}&${filterQuery}`
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

    const getEstate = async (estateId: string): Promise<ApiResponse<EstateFullData>> => {
        const client = await createAxiosInstance();

        try {
            const response = await client.get(`Estate/${estateId}`);

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

    const getEstateImageNames = async (estateId: string): Promise<ApiResponse<string[]>> => {
        const client = await createAxiosInstance();

        try {
            const response = await client.get(`EstateImage/${estateId}`);

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

    const updateEstate = async (updatedEstate: EstateToUpdateData): Promise<ApiResponse<EstateShortData>> => {
        const client = await createAxiosInstance();

        try {
            const response = await client.put(`Estate`, updatedEstate);

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

    const uploadEstateImage = async (estateId: string, file: File): Promise<ApiResponse<EstateImage>> => {
        const client = await createAxiosInstance();

        const formData = new FormData();

        formData.append("file", file);

        try {
            const response = await client.post(`EstateImage/${estateId}`, formData, {
                headers: { "Content-Type": "multipart/form-data" },
            });

            return { id: response.data };
        } catch (error: any) {
            if (error.response) {
                const { status, data } = error.response;
                return { error: true, statusCode: status, message: data.message ?? "Unknown error" };
            } else {
                return { error: true, message: "An unexpected error occurred." };
            }
        }
    };

    const deleteEstateImage = async (estateId: string, imageId: string): Promise<ApiResponse<AxiosResponse>> => {
        const client = await createAxiosInstance();

        try {
            const response = await client.delete(`EstateImage/${estateId}/${imageId}`);

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

    return { getEstateList, getEstate, getEstateImageNames, updateEstate, uploadEstateImage, deleteEstateImage };
};

export default useCatalogueApi;
