namespace OperationsService.Domain.Entities
{
    public static class Constants
    {
        public static readonly string NotFoundEntityException = "No {0} with such gid: {1}.";

        public static readonly string InvalidPaginationQueryParametersException = "Page size and page number can not be less or equal to zero.";

        public static readonly string EmptyResponseException = "Response is empty.";

        public static readonly string OperationWorkerWithSuchEmailException = "Operation Worker with such email already exists.";

        public static readonly string DefaultException = "An unexpected error occurred. Please try again later.";

        public static readonly string AuthService = "AuthService";

        public static readonly string UtilsService = "UtilsService";

        public static readonly string VolunteerService = "VolunteerService";

        public static readonly string AuthServiceInMemory = "AuthServiceInMemory";

        public static readonly string UtilsServiceInMemory = "UtilsServiceInMemory";

        public static readonly string VolunteerServiceInMemory = "VolunteerServiceInMemory";

        public static Guid EventStatusDeleted = new Guid("9095DB76-5C22-431D-BFBD-51849423F57B");

        public static readonly List<EventStatus> EventStatuses = new List<EventStatus>() {
            new EventStatus { GID = new Guid("1B45017E-2781-4802-BB86-037C4A9811F9"), Name = "Створена" },
            new EventStatus { GID = new Guid("7A41DB19-06F8-4334-BB0C-B886D92EC4B4"), Name = "Погоджена" },
            new EventStatus { GID = new Guid("66F0C9E4-3571-4795-8E34-9A9849631DA2"), Name = "Відхилена" },
            new EventStatus { GID = new Guid("58446654-DFA1-4688-8C1C-75BF86EB2200"), Name = "Активна" },
            new EventStatus { GID = new Guid("4E8DC672-602C-4B0E-B1A8-782725558163"), Name = "Завершена" },
            new EventStatus { GID = new Guid("9095DB76-5C22-431D-BFBD-51849423F57B"), Name = "Видалена" },
        };

        public static readonly List<EventType> EventTypes = new List<EventType>() {
            new EventType { GID = new Guid("0B8BD99E-5499-4C45-8130-C17E42E556F3"), Name = "Пошук" },
            new EventType { GID = new Guid("D8DBEC85-C723-4ACA-A055-4D1CE3908E5D"), Name = "Евакуація" },
            new EventType { GID = new Guid("914AC9E1-040A-4A54-90F8-34C6A354F53F"), Name = "Логістика" },
        };

        public static readonly List<OperationTaskStatus> OperationTaskStatuses = new List<OperationTaskStatus>() {
            new OperationTaskStatus { GID = new Guid("A1E5697D-5A9A-4D61-9E84-143802F25E45"), Name = "Очікує виконавця" },
            new OperationTaskStatus { GID = new Guid("C8DBA917-3F3A-4BD0-9D1A-3A52C3F4ACD2"), Name = "Виконується" },
            new OperationTaskStatus { GID = new Guid("AFEC6D5D-4C6A-4A3F-9A9C-E3C3EFC2A9D4"), Name = "Завершена" },
            new OperationTaskStatus { GID = new Guid("B9A6C44D-AB56-4AA2-8F8E-74D52268E278"), Name = "Не виконана" }
        };
    }
}
