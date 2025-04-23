using Gateway.DTO.DTOs.Operations;
using Gateway.DTO.DTOs.Utils;

namespace Gateway.DTO.Constants
{
    public static class SharedConstants
    {
        #region RegExps
        public static readonly string MobilePhomeRegexp = @"^\+?[0-9\s\-]{7,15}$";

        #endregion

        #region Exception
        public static readonly string FieldIsRequierdException = "{0} is required.";

        public static readonly string InvalidBirthDateException = "Birth date must be in the past.";

        public static readonly string InvalidFieldFormatException = "Invalid {0} format.";

        public static readonly string NotFoundEntityException = "No {0} with such gid: {1}.";

        public static readonly string InvalidPaginationQueryParametersException = "Page size and page number can not be less or equal to zero.";

        public static readonly string AlreadyExistsSuchCombinationOfResourceUnitException = "Such combination of resource and unit already exists.";

        public static readonly string EmptyResponseException = "Response is empty.";

        public static readonly string DefaultException = "An unexpected error occurred. Please try again later.";
        #endregion

        #region ServiceNames

        public static readonly string AuthService = "AuthService";

        public static readonly string UtilsService = "UtilsService";

        public static readonly string VolunteerService = "VolunteerService";

        public static readonly string OperationsService = "OperationsService";

        public static readonly string AuthServiceInMemory = "AuthServiceInMemory";

        public static readonly string UtilsServiceInMemory = "UtilsServiceInMemory";

        public static readonly string VolunteerServiceInMemory = "VolunteerServiceInMemory";

        public static readonly string OperationsServiceInMemory = "OperationsServiceInMemory";
        #endregion

        public static readonly string DispatcherRoleName = "Dispatcher";

        public static readonly Guid EventStatusCreated = new Guid("1B45017E-2781-4802-BB86-037C4A9811F9");
        public static readonly Guid EventStatusActive = new Guid("58446654-DFA1-4688-8C1C-75BF86EB2200");
        public static readonly Guid EventStatusComplete = new Guid("4E8DC672-602C-4B0E-B1A8-782725558163");

        public static readonly Guid EventTypeSearch = new Guid("0B8BD99E-5499-4C45-8130-C17E42E556F3");
        public static readonly Guid EventTypeEvacuation = new Guid("D8DBEC85-C723-4ACA-A055-4D1CE3908E5D");
        public static readonly Guid EventTypeLogistic = new Guid("914AC9E1-040A-4A54-90F8-34C6A354F53F");

        public static readonly Guid TaskStatusDoing = new Guid("C8DBA917-3F3A-4BD0-9D1A-3A52C3F4ACD2");

        public static readonly List<EventStatusDTO> EventStatuses = new List<EventStatusDTO>() {
            new EventStatusDTO { GID = new Guid("1B45017E-2781-4802-BB86-037C4A9811F9"), Name = "Створена" },
            new EventStatusDTO { GID = new Guid("7A41DB19-06F8-4334-BB0C-B886D92EC4B4"), Name = "Погоджена" },
            new EventStatusDTO { GID = new Guid("66F0C9E4-3571-4795-8E34-9A9849631DA2"), Name = "Відхилена" },
            new EventStatusDTO { GID = new Guid("58446654-DFA1-4688-8C1C-75BF86EB2200"), Name = "Активна" },
            new EventStatusDTO { GID = new Guid("4E8DC672-602C-4B0E-B1A8-782725558163"), Name = "Завершена" },
            new EventStatusDTO { GID = new Guid("9095DB76-5C22-431D-BFBD-51849423F57B"), Name = "Видалена" },
        };

        public static readonly List<EventTypeDTO> EventTypes = new List<EventTypeDTO>() {
            new EventTypeDTO { GID = new Guid("0B8BD99E-5499-4C45-8130-C17E42E556F3"), Name = "Пошук" },
            new EventTypeDTO { GID = new Guid("D8DBEC85-C723-4ACA-A055-4D1CE3908E5D"), Name = "Евакуація" },
            new EventTypeDTO { GID = new Guid("914AC9E1-040A-4A54-90F8-34C6A354F53F"), Name = "Логістика" },
        };

        public static readonly List<OperationTaskStatusDTO> OperationTaskStatuses = new List<OperationTaskStatusDTO>() {
            new OperationTaskStatusDTO { GID = new Guid("A1E5697D-5A9A-4D61-9E84-143802F25E45"), Name = "Очікує виконавця" },
            new OperationTaskStatusDTO { GID = new Guid("C8DBA917-3F3A-4BD0-9D1A-3A52C3F4ACD2"), Name = "Виконується" },
            new OperationTaskStatusDTO { GID = new Guid("AFEC6D5D-4C6A-4A3F-9A9C-E3C3EFC2A9D4"), Name = "Завершена" },
            new OperationTaskStatusDTO { GID = new Guid("B9A6C44D-AB56-4AA2-8F8E-74D52268E278"), Name = "Не виконана" }
        };


        public static readonly List<DistrictDTO> Districts = new List<DistrictDTO>() {
            new DistrictDTO { GID = new Guid("B19E5E8C-44FB-4C7F-9314-8C7A32B04DB5"), Name = "Голосіївський" },
            new DistrictDTO { GID = new Guid("9D3F161A-637A-45F7-ACB3-FA4C13AB7716"), Name = "Дарницький" },
            new DistrictDTO { GID = new Guid("C8F39C4C-9B03-4A39-B11D-1E7D8C0C578D"), Name = "Деснянський" },
            new DistrictDTO { GID = new Guid("A1FD2F2C-3D25-43C3-B4AB-125EC0B91743"), Name = "Дніпровський" },
            new DistrictDTO { GID = new Guid("1DC1C087-B9C3-4B7F-B291-038B9C1C3ABF"), Name = "Оболонський" },
            new DistrictDTO { GID = new Guid("54BB2F52-93F7-46E4-8F93-3E2DF89B9F92"), Name = "Печерський" },
            new DistrictDTO { GID = new Guid("A9C7168A-E055-47EF-8E27-8C0E768BF82F"), Name = "Подільський" },
            new DistrictDTO { GID = new Guid("B1A0AB07-94C5-4766-8E57-6A11FBB7CFC1"), Name = "Святошинський" },
            new DistrictDTO { GID = new Guid("292E86D6-271A-4142-8C39-DCFFB1F3132A"), Name = "Солом’янський" },
            new DistrictDTO { GID = new Guid("F0B19920-BC84-4EAD-B2C1-083D6C78B6E9"), Name = "Шевченківський" }
        };

        public static readonly List<MeasurementUnitDTO> MeasurementUnits = new List<MeasurementUnitDTO>() {
            new MeasurementUnitDTO { GID = new Guid("A0A519F4-39F7-4F0E-81D3-3A7859790211"), Name = "Літр" },
            new MeasurementUnitDTO { GID = new Guid("15E9A0B5-1A7D-4EF3-AB9F-408A8BD12452"), Name = "Кілограм" },
            new MeasurementUnitDTO { GID = new Guid("C49A0519-AD91-4525-9280-E6C39A02C4D2"), Name = "Штука" },
            new MeasurementUnitDTO { GID = new Guid("295AAD36-82A4-4F91-B0F2-6B10A10C2DF1"), Name = "Пачка" }
        };

        public static readonly List<ResourceDTO> Resources = new List<ResourceDTO>() {
            new ResourceDTO { GID = new Guid("ED93F09B-6931-4B80-B99A-8E0842F5F123"), Name = "Вода" },
            new ResourceDTO { GID = new Guid("C63E49D1-631F-4E1E-9D45-F4A4FC8F65BB"), Name = "Їжа" },
            new ResourceDTO { GID = new Guid("43C6E61F-924D-41F0-86BA-FB8E1F2E5F8C"), Name = "Аптечка" },
            new ResourceDTO { GID = new Guid("7319D98B-0B36-456B-99B0-79D129FD1D64"), Name = "Ліхтарик" },
            new ResourceDTO { GID = new Guid("5D7F420F-FB8E-449E-BA8F-6254D6C2797A"), Name = "Паливо" }
        };

        public static readonly List<ResourceMeasurementUnitDTO> ResourceMeasurementUnits = new List<ResourceMeasurementUnitDTO>() {
            new ResourceMeasurementUnitDTO { GID = new Guid("1399B2A9-A7C0-4963-BD9B-FF28D55A6DB3"), ResourceGID = new Guid("ED93F09B-6931-4B80-B99A-8E0842F5F123"), UnitGID = new Guid("A0A519F4-39F7-4F0E-81D3-3A7859790211") }, // Вода - Літр
            new ResourceMeasurementUnitDTO { GID = new Guid("47C1C39C-04B1-4B4E-9F9A-AF1BE99C17AE"), ResourceGID = new Guid("C63E49D1-631F-4E1E-9D45-F4A4FC8F65BB"), UnitGID = new Guid("15E9A0B5-1A7D-4EF3-AB9F-408A8BD12452") }, // Їжа - Кілограм
            new ResourceMeasurementUnitDTO { GID = new Guid("70DE858B-8717-4DE6-A158-03BD27BB48F2"), ResourceGID = new Guid("43C6E61F-924D-41F0-86BA-FB8E1F2E5F8C"), UnitGID = new Guid("C49A0519-AD91-4525-9280-E6C39A02C4D2") }, // Аптечка - Штука
            new ResourceMeasurementUnitDTO { GID = new Guid("DA309A56-913E-4324-82DA-F582E50C8C11"), ResourceGID = new Guid("7319D98B-0B36-456B-99B0-79D129FD1D64"), UnitGID = new Guid("C49A0519-AD91-4525-9280-E6C39A02C4D2") }, // Ліхтарик - Штука
            new ResourceMeasurementUnitDTO { GID = new Guid("5F7C8C23-3CB6-4D33-A51D-FBC7DFEAF819"), ResourceGID = new Guid("5D7F420F-FB8E-449E-BA8F-6254D6C2797A"), UnitGID = new Guid("A0A519F4-39F7-4F0E-81D3-3A7859790211") }  // Паливо - Літр
        };
    }
}
