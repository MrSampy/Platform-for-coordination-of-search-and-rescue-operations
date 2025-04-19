export type ErrorModel = {
    message: string;
    details: string;
    statuscode:number;
  };
  export type Nullable<T> = T | null | undefined;

  export interface PaginationQuery {
    pageNumber:number;
    pageSize:number;
  }