import L from "leaflet";
import mapMarkerIcon from "../../../images/map-marker.svg";

let markerIcon = L.icon({
    iconUrl: mapMarkerIcon,
    iconRetinaUrl: mapMarkerIcon,
    iconAnchor: [15, 55],
    popupAnchor: [10, -44],
    iconSize: [35, 65],
});

export { markerIcon };
