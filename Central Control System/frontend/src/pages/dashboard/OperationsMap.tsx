import React, { useEffect, useState } from 'react';
import { MapContainer, TileLayer, Marker, Popup } from 'react-leaflet';
import L from 'leaflet';
import axios from 'axios';
import { TokenInfoDTO } from '../../types/authTypes';
import { DetailEvent, EventPaginationQuery } from '../../types/eventTypes';
import { EventStatusActive } from '../../types/constants';

const markerIcon = L.icon({
  iconUrl: 'https://unpkg.com/leaflet@1.7.1/dist/images/marker-icon.png',
  iconSize: [25, 41],
  iconAnchor: [12, 41]
});

export default function OperationsMap() {
  const [events, setEvents] = useState<DetailEvent[]>([]);

  useEffect(() => {
    const fetchEvents = async () => {
      const tokenStr = localStorage.getItem('token');
      if (!tokenStr) return;

      const token = JSON.parse(tokenStr) as TokenInfoDTO;
      const paginationQuery: EventPaginationQuery = {
        pageNumber: 1,
        pageSize: 1000,
        eventStatusGID: EventStatusActive.gid
      };

      try {
        const response = await axios.post<{ items: DetailEvent[] }>(
          `${process.env.REACT_APP_API_BASE_URL}/event/sort`,
          paginationQuery,
          {
            headers: {
              Authorization: `Bearer ${token.token}`,
            },
          }
        );
        setEvents(response.data.items);
      } catch (err) {
        console.error('Failed to load active events', err);
      }
    };

    fetchEvents();
  }, []);

  return (
    <div className="border-round-xl shadow-2 p-4" style={{ backgroundColor: 'white' }}>
      <h2>Мапа активних операцій</h2>
      <MapContainer
        center={[50.4501, 30.5234]} // center on Kyiv
        zoom={10}
        scrollWheelZoom={true}
        style={{ height: '600px', width: '100%' }}
      >
        <TileLayer
          attribution='&copy; OpenStreetMap contributors'
          url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
        />

        {events.map(event => (
          <Marker
            key={event.gid}
            position={[event.latitude, event.longitude]}
            icon={markerIcon}
          >
            <Popup>
              <strong>{event.name}</strong><br />
              Тип: {event.eventType}<br />
              Район: {event.district}<br />
              Дипетчер: {event.dispatcher}<br />
              Координатор: {event.coordinator}
            </Popup>
          </Marker>
        ))}
      </MapContainer>
    </div>
  );
}
