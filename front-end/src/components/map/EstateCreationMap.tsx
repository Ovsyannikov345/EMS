import { MapContainer, TileLayer } from "react-leaflet";
import MapLocationFinder from "./utils/MapLocationFinder";
import PlaceableMarker from "./utils/PlaceableMarker";
import "leaflet/dist/leaflet.css";
import { LatLng } from "leaflet";
import { useNotifications } from "@toolpad/core";
import useOSMApi from "../../hooks/useOSMApi";
import { markerIcon } from "./utils/mapIcons";

interface EstateCreationMapProps {
    onLocationChange: (address: any) => void;
}

const EstateCreationMap = ({ onLocationChange }: EstateCreationMapProps) => {
    const notifications = useNotifications();

    const { getAddressFromCoordinates } = useOSMApi();

    const changeLocation = async (newLocation: LatLng) => {
        const response = await getAddressFromCoordinates(newLocation.lat, newLocation.lng);

        if ("error" in response) {
            notifications.show(response.message, { severity: "error", autoHideDuration: 3000 });
        } else {
            const address = response.address;

            const addressString = [address.country, address.city, address.road, address.house_number].join(", ");

            onLocationChange(addressString);
        }
    };

    return (
        <div style={{ width: "100%", aspectRatio: 16 / 9 }}>
            <MapContainer
                center={[51.505, -0.09]}
                zoom={window.innerWidth < 720 ? 10 : 12}
                scrollWheelZoom={true}
                style={{ height: "100%", width: "100%" }}
            >
                <TileLayer
                    attribution='&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
                    url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
                />
                <MapLocationFinder />
                <PlaceableMarker onMoveCallback={changeLocation} icon={markerIcon} />
            </MapContainer>
        </div>
    );
};

export default EstateCreationMap;
