import { Nullable } from "./commonTypes";

export interface EventDTO {
  gid: string;
  name: string;
  longitude: number;
  latitude: number;
  eventTypeGID: string;
  districtGID: string;
  coordinatorGID: string;
  dispatcherGID: string;
  eventStatusGID: string;
}

export interface CreateEventDTO {
  name: string;
  longitude: number;
  latitude: number;
  eventTypeGID: string;
  districtGID: string;
  coordinatorGID: string;
  userGID: string;
}

export interface DetailEvent {
  gid: string;
  name: string;
  longitude: number;
  latitude: number;
  eventType: string;
  district: string;
  coordinator: string;
  dispatcher: string;
  eventStatus: string;
}
export interface EventPaginationQuery {
  pageNumber:number;
  pageSize:number;
  eventStatusGID: Nullable<string>;
}

export interface EventStatusDTO {
  gid: string;
  name: string;
}
export interface EventTypeDTO {
  gid: string;
  name: string;
}

export interface EventStatusChangeRequest{
  eventGID: string;
  eventStatusGID: string;
}

export interface OperationWorkerDTO {
  gid: string;
  name: string;
  surname: string;
  secondName: string;
  email: string;
  identificationCode: string;
  userGID: string;
  birthDate: Date;
}