
export interface DistrictDTO {
    gid: string;
    name: string;
  }
  

export interface ResourceDTO {
  gid: string;
  name: string;
}
export interface MeasurementUnitDTO {
  gid: string;
  name: string;
}
export interface ResourceMeasurementUnitDTO {
  gid: string;
  unitGID: string;
  resourceGID: string;
}