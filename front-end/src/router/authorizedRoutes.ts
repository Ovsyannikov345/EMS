import { CATALOGUE_ROUTE, OWN_PROFILE_ROUTE, PROFILE_ROUTE, USER_ESTATE_ROUTE } from "../utils/consts";
import CataloguePage from "../pages/CataloguePage";
import ProfilePage from "../pages/ProfilePage";

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
];

export default authorizedRoutes;
