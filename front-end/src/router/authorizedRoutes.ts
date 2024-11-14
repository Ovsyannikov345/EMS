import { CATALOGUE_ROUTE } from "../utils/consts";
import CataloguePage from "../pages/CataloguePage";

const authorizedRoutes = [
    {
        path: CATALOGUE_ROUTE,
        Component: CataloguePage,
    },
];

export default authorizedRoutes;