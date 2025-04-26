
import { Nullable } from "./commonTypes";

export interface DetailGroup {
    gid: string;
    name: string;
    eventGID: string;
    eventName: string;
    leaderGID: Nullable<string>;
    leaderName: Nullable<string>;
  }
  
  export interface GroupPaginationQuery {
    pageNumber:number;
    pageSize:number;
    eventGID: Nullable<string>;
    leaderGID: Nullable<string>;
  }

  
export interface GroupDTO {
    gid: string;
    name: string;
    eventGID: string;
    leaderGID: Nullable<string>;
  }
  
  
  export interface CreateGroupDTO {
    gid: Nullable<string>;
    name: string;
    eventGID: string;
    leaderGID: Nullable<string>;
    deleted: Nullable<boolean>;
    edited: Nullable<boolean>;
  }
  