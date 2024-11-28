import { ReactElement } from "react";
import { DialogsProvider, NotificationsProvider } from "@toolpad/core";

interface ToolpadProviderProps {
    children: ReactElement;
}
const ToolpadProvider = ({ children }: ToolpadProviderProps) => {
    return (
        <NotificationsProvider>
            <DialogsProvider>{children}</DialogsProvider>
        </NotificationsProvider>
    );
};

export default ToolpadProvider;
