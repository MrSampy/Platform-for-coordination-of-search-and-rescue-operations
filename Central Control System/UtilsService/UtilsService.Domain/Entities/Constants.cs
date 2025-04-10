namespace UtilsService.Domain.Entities
{
    public static class Constants
    {
        public static readonly string NotFoundEntityException = "No {0} with such gid: {1}.";

        public static readonly string InvalidPaginationQueryParametersException = "Page size and page number can not be less or equal to zero.";

        public static readonly string AlreadyExistsSuchCombinationOfResourceUnitException = "Such combination of resource and unit already exists.";

        public static readonly string EmptyResponseException = "Response is empty.";

        public static readonly string AuthService = "AuthService";

        public static readonly List<District> Districts = new List<District>() {
            new District { GID = new Guid("B19E5E8C-44FB-4C7F-9314-8C7A32B04DB5"), Name = "Голосіївський" },
            new District { GID = new Guid("9D3F161A-637A-45F7-ACB3-FA4C13AB7716"), Name = "Дарницький" },
            new District { GID = new Guid("C8F39C4C-9B03-4A39-B11D-1E7D8C0C578D"), Name = "Деснянський" },
            new District { GID = new Guid("A1FD2F2C-3D25-43C3-B4AB-125EC0B91743"), Name = "Дніпровський" },
            new District { GID = new Guid("1DC1C087-B9C3-4B7F-B291-038B9C1C3ABF"), Name = "Оболонський" },
            new District { GID = new Guid("54BB2F52-93F7-46E4-8F93-3E2DF89B9F92"), Name = "Печерський" },
            new District { GID = new Guid("A9C7168A-E055-47EF-8E27-8C0E768BF82F"), Name = "Подільський" },
            new District { GID = new Guid("B1A0AB07-94C5-4766-8E57-6A11FBB7CFC1"), Name = "Святошинський" },
            new District { GID = new Guid("292E86D6-271A-4142-8C39-DCFFB1F3132A"), Name = "Солом’янський" },
            new District { GID = new Guid("F0B19920-BC84-4EAD-B2C1-083D6C78B6E9"), Name = "Шевченківський" }
        };

        public static readonly List<MeasurementUnit> MeasurementUnits = new List<MeasurementUnit>() {
            new MeasurementUnit { GID = new Guid("A0A519F4-39F7-4F0E-81D3-3A7859790211"), Name = "Літр" },
            new MeasurementUnit { GID = new Guid("15E9A0B5-1A7D-4EF3-AB9F-408A8BD12452"), Name = "Кілограм" },
            new MeasurementUnit { GID = new Guid("C49A0519-AD91-4525-9280-E6C39A02C4D2"), Name = "Штука" },
            new MeasurementUnit { GID = new Guid("295AAD36-82A4-4F91-B0F2-6B10A10C2DF1"), Name = "Пачка" }
        };

        public static readonly List<Resource> Resources = new List<Resource>() {
            new Resource { GID = new Guid("ED93F09B-6931-4B80-B99A-8E0842F5F123"), Name = "Вода" },
            new Resource { GID = new Guid("C63E49D1-631F-4E1E-9D45-F4A4FC8F65BB"), Name = "Їжа" },
            new Resource { GID = new Guid("43C6E61F-924D-41F0-86BA-FB8E1F2E5F8C"), Name = "Аптечка" },
            new Resource { GID = new Guid("7319D98B-0B36-456B-99B0-79D129FD1D64"), Name = "Ліхтарик" },
            new Resource { GID = new Guid("5D7F420F-FB8E-449E-BA8F-6254D6C2797A"), Name = "Паливо" }
        };

        public static readonly List<ResourceMeasurementUnit> ResourceMeasurementUnits = new List<ResourceMeasurementUnit>() {
            new ResourceMeasurementUnit { GID = new Guid("1399B2A9-A7C0-4963-BD9B-FF28D55A6DB3"), ResourceGID = new Guid("ED93F09B-6931-4B80-B99A-8E0842F5F123"), UnitGID = new Guid("A0A519F4-39F7-4F0E-81D3-3A7859790211") }, // Вода - Літр
            new ResourceMeasurementUnit { GID = new Guid("47C1C39C-04B1-4B4E-9F9A-AF1BE99C17AE"), ResourceGID = new Guid("C63E49D1-631F-4E1E-9D45-F4A4FC8F65BB"), UnitGID = new Guid("15E9A0B5-1A7D-4EF3-AB9F-408A8BD12452") }, // Їжа - Кілограм
            new ResourceMeasurementUnit { GID = new Guid("70DE858B-8717-4DE6-A158-03BD27BB48F2"), ResourceGID = new Guid("43C6E61F-924D-41F0-86BA-FB8E1F2E5F8C"), UnitGID = new Guid("C49A0519-AD91-4525-9280-E6C39A02C4D2") }, // Аптечка - Штука
            new ResourceMeasurementUnit { GID = new Guid("DA309A56-913E-4324-82DA-F582E50C8C11"), ResourceGID = new Guid("7319D98B-0B36-456B-99B0-79D129FD1D64"), UnitGID = new Guid("C49A0519-AD91-4525-9280-E6C39A02C4D2") }, // Ліхтарик - Штука
            new ResourceMeasurementUnit { GID = new Guid("5F7C8C23-3CB6-4D33-A51D-FBC7DFEAF819"), ResourceGID = new Guid("5D7F420F-FB8E-449E-BA8F-6254D6C2797A"), UnitGID = new Guid("A0A519F4-39F7-4F0E-81D3-3A7859790211") }  // Паливо - Літр
        };

    }
}
