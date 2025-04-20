using Gateway.Domain.Services.Interfaces;
using Gateway.DTO.Constants;
using Gateway.DTO.DTOs.Auth;
using Gateway.DTO.DTOs.Operations;
using Gateway.DTO.DTOs.Operations.Create;
using Gateway.DTO.DTOs.Volunteers;
using Gateway.DTO.DTOs.Volunteers.Create;

namespace Gateway.Integration.Api.Config.Utils
{
    public class DbSeeder
    {
        private List<UserDTO> _users = new List<UserDTO>();
        private string _token = string.Empty;
        private const int _userCount = 5;
        private IAuthGateway _authGateway;
        private IOperationsGateway _operationsGateway;
        private IVolunteersGateway _volunteersGateway;
        private List<OperationWorkerDTO> _coordinators = new List<OperationWorkerDTO>();
        private List<OperationWorkerDTO> _dispatchers = new List<OperationWorkerDTO>();
        private List<OperationWorkerDTO> _admins = new List<OperationWorkerDTO>();
        private List<VolunteerDTO> _volunteers = new List<VolunteerDTO>();
        private List<EventDTO> _events = new List<EventDTO>();
        private List<GroupDTO> _groups = new List<GroupDTO>();
        public List<OperationTaskDTO> _operationTaskDTOs = new List<OperationTaskDTO>();
        private readonly Random _random = new Random();

        public DbSeeder(IAuthGateway authGateway, IOperationsGateway operationsGateway, IVolunteersGateway volunteersGateway)
        {
            _users = new List<UserDTO>();
            _authGateway = authGateway;
            _operationsGateway = operationsGateway;
            _volunteersGateway = volunteersGateway;
        }

        public async Task SeedAsync()
        {
            Task.Delay(2000).Wait();

            await CreateUsersAsync();

            if (_users == null || _users.Count != _userCount * 4)
            {
                return;
            }

            await CreateOperationWorkersAndEvents();

            await CreateVolunteers();

            await CreateGroups();
        }

        public async Task CreateGroups()
        {
            for (int i = 0; i < _events.Count; i++)
            {
                var createEntity = new CreateGroupDTO
                {
                    Name = $"GroupN{i}",
                    LeaderGID = _volunteers[_random.Next(0, _volunteers.Count - 1)].GID,
                    EventGID = _events[i].GID
                };
                var group = await _operationsGateway.CreateGroup(createEntity, _token);
                _groups.Add(group);
            }

            for (int i = 0; i < _groups.Count; i++)
            {
                var groupInDistrict1 = new CreateVolunteersGroupsDTO
                {
                    GroupGID = _groups[i].GID,
                    VolunteerGID = _groups[i].LeaderGID
                };
                await _volunteersGateway.AddVolunteerToGroup(groupInDistrict1, _token);

                var volunttersWithoutLeader = _volunteers.Where(x => x.GID != _groups[i].LeaderGID).ToList();

                var groupInDistrict2 = new CreateVolunteersGroupsDTO
                {
                    GroupGID = _groups[i].GID,
                    VolunteerGID = volunttersWithoutLeader[_random.Next(0, volunttersWithoutLeader.Count - 1)].GID,
                };

                await _volunteersGateway.AddVolunteerToGroup(groupInDistrict2, _token);
            }

            for (int i = 0; i < _groups.Count; i++)
            {
                var operationTask = new CreateOperationTaskDTO
                {
                    Name = $"TaskN{i}",
                    TaskDescription = $"TaskD{i}",
                    GroupGID = _groups[i].GID,
                    TaskStatusGID = SharedConstants.TaskStatusDoing
                };

                var task = await _operationsGateway.CreateOperationTask(operationTask, _token);

                _operationTaskDTOs.Add(task);
            }
        }

        public async Task CreateVolunteers()
        {
            var volunteers = _users.Where(x => x.Roles.Any(r => r.Name == "Volunteer")).ToList();

            for (int i = 0; i < _userCount; i++)
            {
                var createEntity = new CreateVolunteerDTO
                {
                    Name = $"VolunteerN{i}",
                    Surname = $"VolunteerS{i}",
                    SecondName = $"VolunteerS{i}",
                    MobilePhone = $"+38098984211{i}",
                    BirthDate = DateTime.UtcNow.AddYears(-20 - i),
                    UserGID = volunteers[i].Id,
                    Email = volunteers[i].Email
                };
                var volunteer = await _volunteersGateway.CreateVolunteer(createEntity, _token);
                _volunteers.Add(volunteer);
            }

            for (int i = 0; i < _userCount; i++)
            {
                List<int> numbers = new List<int>();
                for (int k = 0; k < 3; k++)
                {
                    var number = GenRandomNumber(numbers, SharedConstants.Districts.Count - 1);
                    var volunteerInDistrict = new CreateVolunteersDistrictsDTO
                    {
                        VolunteerGID = _volunteers[i].GID,
                        DistrictGID = SharedConstants.Districts[number].GID
                    };
                    var volunteerDistrict = await _volunteersGateway.CreateVolunteersDistrict(volunteerInDistrict, _token);
                }
            }
        }

        public int GenRandomNumber(List<int> numbers, int maxNumber)
        {
            while (true)
            {
                int number = _random.Next(0, maxNumber + 1);
                if (!numbers.Contains(number))
                {
                    numbers.Add(number);
                    return number;
                }
            }
        }

        public async Task CreateOperationWorkersAndEvents()
        {
            var dispatchers = _users.Where(x => x.Roles.Any(r => r.Name == "Dispatcher") && x.Roles.Count == 1).ToList();

            var coordinators = _users.Where(x => x.Roles.Any(r => r.Name == "Coordinator")).ToList();

            var admins = _users.Where(x => x.Roles.Any(r => r.Name == "Admin")).ToList();

            for (int i = 0; i < _userCount; i++)
            {
                var creteEntity = new CreateOperationWorkerDTO
                {
                    Name = $"DispatcherN{i}",
                    Surname = $"DispatcherS{i}",
                    SecondName = $"DispatcherS{i}",
                    IdentificationCode = $"143456789{i}",
                    BirthDate = DateTime.UtcNow.AddYears(-20 - i),
                    UserGID = dispatchers[i].Id,
                    Email = dispatchers[i].Email
                };
                var dispatcher = await _operationsGateway.CreateOperationWorker(creteEntity, _token);
                _dispatchers.Add(dispatcher);
            }

            for (int i = 0; i < _userCount; i++)
            {
                var creteEntity = new CreateOperationWorkerDTO
                {
                    Name = $"CoordinatorN{i}",
                    Surname = $"CoordinatorS{i}",
                    SecondName = $"CoordinatorS{i}",
                    IdentificationCode = $"133456789{i}",
                    BirthDate = DateTime.UtcNow.AddYears(-30 - i),
                    UserGID = coordinators[i].Id,
                    Email = coordinators[i].Email
                };
                var coordinator = await _operationsGateway.CreateOperationWorker(creteEntity, _token);
                _coordinators.Add(coordinator);
            }

            for (int i = 0; i < _userCount; i++)
            {
                var creteEntity = new CreateOperationWorkerDTO
                {
                    Name = $"AdminN{i}",
                    Surname = $"AdminS{i}",
                    SecondName = $"AdminS{i}",
                    IdentificationCode = $"123456789{i}",
                    BirthDate = DateTime.UtcNow.AddYears(-40 - i),
                    UserGID = admins[i].Id,
                    Email = admins[i].Email
                };
                var admin = await _operationsGateway.CreateOperationWorker(creteEntity, _token);
                _admins.Add(admin);
            }

            for (int i = 0; i < _userCount; i++)
            {
                var activeEvent = new CreateEventDTO
                {
                    Name = $"Event2{i}",
                    Latitude = 50.443481M - i * 0.01M,
                    Longitude = 30.501528M - i * 0.01M,
                    EventTypeGID = SharedConstants.EventTypeEvacuation,
                    EventStatusGID = SharedConstants.EventStatusActive,
                    DistrictGID = SharedConstants.Districts[i].GID,
                    DispatcherGID = _dispatchers[i].GID,
                    CoordinatorGID = _coordinators[i].GID
                };


                var createdEvent = new CreateEventDTO
                {
                    Name = $"Event1{i}",
                    Latitude = 50.443481M + i * 0.01M,
                    Longitude = 30.501528M + i * 0.01M,
                    EventTypeGID = SharedConstants.EventTypeSearch,
                    EventStatusGID = SharedConstants.EventStatusCreated,
                    DistrictGID = SharedConstants.Districts[i].GID,
                    DispatcherGID = _dispatchers[i].GID,
                    CoordinatorGID = _coordinators[i].GID
                };

                var createdEvent1 = await _operationsGateway.CreateEvent(activeEvent, _token);
                var createdEvent2 = await _operationsGateway.CreateEvent(createdEvent, _token);
                _events.Add(createdEvent1);
                _events.Add(createdEvent2);
            }

            for (int i = 0; i < _events.Count; i++)
            {
                var resourceEvent = new CreateResourcesEventDTO
                {
                    EventGID = _events[i].GID,
                    ResourceGID = SharedConstants.Resources[_random.Next(0, SharedConstants.Resources.Count - 1)].GID,
                    RequiredQuantity = i + 1,
                    AvailableQuantity = 1,
                };

                await _operationsGateway.CreateResourcesEvent(resourceEvent, _token);
            }
        }
        public async Task CreateUsersAsync()
        {
            var defaultPassword = "*Password1Password";

            for (int i = 1; i <= _userCount; i++)
            {
                var registerModel = new RegisterModel { Username = $"volunteer{i}", Email = $"volunteer{i}@example.com", Password = defaultPassword };
                var existingUser = _authGateway.GetByUserName(registerModel.Username, CancellationToken.None);
                if (existingUser != null)
                {
                    continue;
                }
                var user = await _authGateway.Register(registerModel);
                _users.Add(user);
            }

            _token = _authGateway.Login(new LoginModel { Username = "volunteer1", Password = defaultPassword }).Result.Token;

            for (int i = 1; i <= _userCount; i++)
            {
                var registerModel = new RegisterModel { Username = $"admin{i}", Email = $"admin{i}@example.com", Password = defaultPassword };
                var existingUser = _authGateway.GetByUserName(registerModel.Username, CancellationToken.None);
                if (existingUser != null)
                {
                    continue;
                }
                var user = await _authGateway.RegisterAdmin(registerModel, _token);
                _users.Add(user);
            }


            for (int i = 1; i <= _userCount; i++)
            {
                var registerModel = new RegisterModel { Username = $"coordinator{i}", Email = $"coordinator{i}@example.com", Password = defaultPassword };
                var existingUser = _authGateway.GetByUserName(registerModel.Username, CancellationToken.None);
                if (existingUser != null)
                {
                    continue;
                }
                var user = await _authGateway.RegisterCoordinator(registerModel, _token);
                _users.Add(user);
            }

            for (int i = 1; i <= _userCount; i++)
            {
                var registerModel = new RegisterModel { Username = $"dispatcher{i}", Email = $"dispatcher{i}@example.com", Password = defaultPassword };
                var existingUser = _authGateway.GetByUserName(registerModel.Username, CancellationToken.None);
                if (existingUser != null)
                {
                    continue;
                }
                var user = await _authGateway.RegisterDispatcher(registerModel);
                _users.Add(user);
            }
        }
    }
}
