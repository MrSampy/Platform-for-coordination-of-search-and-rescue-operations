import { MapContainer, TileLayer, Marker, useMapEvents } from 'react-leaflet';
import { useState } from 'react';
import L from 'leaflet';

interface LocationPickerMapProps {
  lat: number;
  lng: number;
  onLocationChange: (lat: number, lng: number) => void;
}

const markerIcon = L.icon({
  iconUrl: 'https://raw.githubusercontent.com/pointhi/leaflet-color-markers/master/img/marker-icon-red.png',
  shadowUrl: 'https://unpkg.com/leaflet@1.7.1/dist/images/marker-shadow.png',
  iconSize: [25, 41],
  iconAnchor: [12, 41],
  popupAnchor: [1, -34],
  shadowSize: [41, 41]
});

const LocationPickerMap = ({ lat, lng, onLocationChange }: LocationPickerMapProps) => {
  const [position, setPosition] = useState({ lat, lng });

  const MapClickHandler = () => {
    useMapEvents({
      click(e) {
        const { lat, lng } = e.latlng;
        setPosition({ lat, lng });
        onLocationChange(lat, lng);
      },
    });
    return null;
  };

  return (
<MapContainer center={[lat || 50.4501, lng || 30.5234]} zoom={10} style={{ height: '300px', width: '100%' }}>
    <TileLayer
        attribution='&copy; OpenStreetMap contributors'
        url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
      />
      <MapClickHandler />
      <Marker position={[position.lat, position.lng]} icon={markerIcon} />
    </MapContainer>
  );
};

export default LocationPickerMap; // ✅ Proper export
