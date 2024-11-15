import React from "react";
import { Routes, Route, Navigate } from "react-router-dom";
import authorizedRoutes from "./authorizedRoutes";
import { useAuth0 } from "@auth0/auth0-react";
import { CATALOGUE_ROUTE } from "../utils/consts";
import { Box, CircularProgress } from "@mui/material";

const AppRouter = () => {
    const { isAuthenticated, loginWithRedirect, isLoading } = useAuth0();

    if (isLoading) {
        return (
            <Box
                sx={{
                    display: "flex",
                    justifyContent: "center",
                    alignItems: "center",
                    height: "100vh",
                }}
            >
                <CircularProgress />
            </Box>
        );
    }

    if (!isAuthenticated) {
        loginWithRedirect();
    }

    return (
        <Routes>
            {authorizedRoutes.map(({ path, Component }) => (
                <Route key={path} path={path} element={<Component />} />
            ))}

            <Route path="*" element={<Navigate to={CATALOGUE_ROUTE} />} />
        </Routes>
    );
};

export default AppRouter;
