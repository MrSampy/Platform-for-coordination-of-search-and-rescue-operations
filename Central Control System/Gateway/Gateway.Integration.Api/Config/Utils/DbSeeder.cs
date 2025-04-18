using Gateway.Domain.Services.Interfaces;
using Gateway.DTO.Constants;
using Gateway.DTO.DTOs.Auth;
using Gateway.DTO.DTOs.Operations;
using Gateway.DTO.DTOs.Operations.Create;

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
        private List<EventDTO> _events = new List<EventDTO>();

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
            await CreateOperationServiceModels();
        }
        public async Task CreateOperationServiceModels()
        {
            if (_users == null || _users.Count != _userCount * 4)
            {
                return;
            }

            var dispatchers = _users.Where(x => x.Roles.Any(r => r.Name == "Dispatcher")).ToList();

            var coordinators = _users.Where(x => x.Roles.Any(r => r.Name == "Coordinator")).ToList();

            for (int i = 0; i < _userCount; i++)
            {
                var creteEntity = new CreateOperationWorkerDTO
                {
                    Name = $"DispatcherN{i}",
                    Surname = $"DispatcherS{i}",
                    SecondName = $"DispatcherS{i}",
                    IdentificationCode = $"123456789{i}",
                    BirthDate = DateTime.UtcNow.AddYears(-20 + i),
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
                    IdentificationCode = $"123456789{i}",
                    BirthDate = DateTime.UtcNow.AddYears(-20 + i),
                    UserGID = coordinators[i].Id,
                    Email = coordinators[i].Email
                };
                var coordinator = await _operationsGateway.CreateOperationWorker(creteEntity, _token);
                _coordinators.Add(coordinator);
            }

            for (int i = 0; i < _userCount; i++)
            {
                var activeEvent = new CreateEventDTO
                {
                    Name = $"Event2{i}",
                    Longitude = i,
                    Latitude = i,
                    EventTypeGID = SharedConstants.EventTypeEvacuation,
                    EventStatusGID = SharedConstants.EventStatusActive,
                    DistrictGID = SharedConstants.Districts[i].GID,
                    DispatcherGID = _dispatchers[i].GID,
                    CoordinatorGID = _coordinators[i].GID
                };


                var createdEvent = new CreateEventDTO
                {
                    Name = $"Event1{i}",
                    Longitude = i,
                    Latitude = i,
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
