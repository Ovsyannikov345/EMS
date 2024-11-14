import React from "react";
import { BrowserRouter } from "react-router-dom";
import { LocalizationProvider } from "@mui/x-date-pickers";
import { AdapterMoment } from "@mui/x-date-pickers/AdapterMoment";
import Header from "./components/headers/Header";
import AppRouter from "./router/AppRouter";
import { Auth0Provider } from "@auth0/auth0-react";

function App() {
    return (
        <Auth0Provider
            domain={process.env.REACT_APP_AUTH0_DOMAIN!}
            clientId={process.env.REACT_APP_AUTH0_CLIENT_ID!}
            authorizationParams={{ redirect_uri: process.env.REACT_APP_AUTH0_REDIRECT_URL!, audience: process.env.REACT_APP_AUTH0_AUDIENCE }}
        >
            <LocalizationProvider dateAdapter={AdapterMoment} adapterLocale="en-us">
                <BrowserRouter>
                    <Header />
                </BrowserRouter>
            </LocalizationProvider>
        </Auth0Provider>
    );
}

export default App;
