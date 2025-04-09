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

        public static readonly List<EventStatus> EventStatuses = new List<EventStatus>() {
            new EventStatus { GID = new Guid("1B45017E-2781-4802-BB86-037C4A9811F9"), Name = "Створена" },
            new EventStatus { GID = new Guid("7A41DB19-06F8-4334-BB0C-B886D92EC4B4"), Name = "Погоджена" },
            new EventStatus { GID = new Guid("66F0C9E4-3571-4795-8E34-9A9849631DA2"), Name = "Відхилена" },
            new EventStatus { GID = new Guid("58446654-DFA1-4688-8C1C-75BF86EB2200"), Name = "Активна" },
            new EventStatus { GID = new Guid("4E8DC672-602C-4B0E-B1A8-782725558163"), Name = "Завершена" },
        };
    }
}
