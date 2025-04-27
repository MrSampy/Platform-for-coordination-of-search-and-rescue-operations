import { Nullable } from "./commonTypes";

export interface VolunteersEventsDTO {
    gid: string;
    volunteerGID: string;
    eventGID: string;
  } 

  export interface VolunteersGroupsDTO {
    gid: string;
    volunteerGID: string;
    groupGID: string;
  } 

  export interface CreateVolunteersGroupsDTO {
    volunteerGID: string;
    groupGID: string;
  } 

  export interface VolunteerDTO {
    gid: string;
    name: string;
    surname: string;
    secondName: string;
    email: string;
    ratingNumber:number;
    mobilePhone: string;
    userGID: string;
    birthDate: Date;
  }