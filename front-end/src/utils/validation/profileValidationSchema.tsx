import * as Yup from "yup";

const profileValidationSchema = Yup.object({
    firstName: Yup.string().required("First Name is required").max(50, "First Name must not exceed 50 characters"),
    lastName: Yup.string().required("Last Name is required").max(50, "Last Name must not exceed 50 characters"),
    phoneNumber: Yup.string().required("Phone Number is required").max(30, "Phone Number must not exceed 30 characters"),
    birthDate: Yup.date().required("Birth Date is required").max(new Date(), "Birth Date cannot be in the future"),
});

export default profileValidationSchema;
