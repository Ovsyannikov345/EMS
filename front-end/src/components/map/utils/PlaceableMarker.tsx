import { LatLngExpression } from "leaflet";
import { useState } from "react";
import { Marker, useMapEvents } from "react-leaflet";

const PlaceableMarker = ({ onMoveCallback, icon }: { onMoveCallback: Function; icon: L.Icon }) => {
    const [position, setPosition] = useState<LatLngExpression>();

    useMapEvents({
        click: (e: { latlng: LatLngExpression }) => {
            setPosition(e.latlng);
            onMoveCallback?.(e.latlng);
        },
    });

    return position ? <Marker position={position} icon={icon} /> : null;
};

export default PlaceableMarker;
