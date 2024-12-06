import {
    CATALOGUE_ROUTE,
    ESTATE_CREATION_ROUTE,
    ESTATE_DETAILS_ROUTE,
    OWN_PROFILE_ROUTE,
    PROFILE_ROUTE,
    USER_ESTATE_ROUTE,
} from "../utils/consts";
import CataloguePage from "../pages/CataloguePage";
import ProfilePage from "../pages/ProfilePage";
import EstateDetailsPage from "../pages/EstateDetailsPage";
import EstateCreationPage from "../pages/EstateCreationPage";

const authorizedRoutes = [
    {
        path: CATALOGUE_ROUTE,
        Component: CataloguePage,
    },
    {
        path: USER_ESTATE_ROUTE,
        Component: CataloguePage,
    },
    {
        path: PROFILE_ROUTE,
        Component: ProfilePage,
    },
    {
        path: OWN_PROFILE_ROUTE,
        Component: ProfilePage,
    },
    {
        path: ESTATE_DETAILS_ROUTE,
        Component: EstateDetailsPage,
    },
    {
        path: ESTATE_CREATION_ROUTE,
        Component: EstateCreationPage,
    },
];

export default authorizedRoutes;
