import * as Yup from "yup";
import { EstateType } from "../../hooks/useCatalogueApi";

const estateValidationSchema = Yup.object({
    type: Yup.number().oneOf([EstateType.None, EstateType.Apartment, EstateType.House], "Invalid estate type").required("Type is required"),
    address: Yup.string().max(100, "Address must not exceed 100 characters").required("Address is required"),
    area: Yup.number().moreThan(0, "Area must be greater than 0").required("Area is required"),
    roomsCount: Yup.number().moreThan(0, "Rooms count must be greater than 0").required("Rooms count is required"),
    price: Yup.number().moreThan(0, "Price must be greater than 0").required("Price is required"),
});

export default estateValidationSchema;
