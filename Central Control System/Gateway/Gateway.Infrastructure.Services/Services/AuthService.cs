using Gateway.Domain.Services.Interfaces;
using Gateway.DTO.Constants;
using Gateway.DTO.DTOs.Auth;
using Gateway.DTO.DTOs.Common;
using Gateway.DTO.DTOs.Operations.Create;
using Gateway.DTO.Exceptions;

namespace Gateway.Infrastructure.Services.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthGateway _authGateway;
        private readonly IOperationsGateway _operationsGateway;

        public AuthService(IAuthGateway authGateway, IOperationsGateway operationsGateway)
        {
            _authGateway = authGateway;
            _operationsGateway = operationsGateway;
        }

        public async Task RegisterWorker(RegisterWorkerModel model, string token)
        {
            var registerModel = new RegisterModel
            {
                Username = model.Username,
                Password = model.Password,
                Email = model.Email
            };

            if (model.Role != SharedConstants.Dispatcher && model.Role != SharedConstants.Coordinator)
            {
                throw new ServiceException("Invalid role");
            }

            var user = model.Role == SharedConstants.Dispatcher ? await _authGateway.RegisterDispatcher(registerModel, token) : await _authGateway.RegisterCoordinator(registerModel, token);

            if (user == null)
            {
                throw new ServiceException("User registration failed");
            }

            var operationWorker = new CreateOperationWorkerDTO
            {
                Name = model.Name,
                Surname = model.Surname,
                SecondName = model.SecondName,
                IdentificationCode = model.IdentificationCode,
                BirthDate = model.BirthDate,
                UserGID = user.Id,
                Email = model.Email
            };

            var createdWorker = await _operationsGateway.CreateOperationWorker(operationWorker, token);

            if (createdWorker == null)
            {
                throw new ServiceException("Operation worker creation failed");
            }
        }

        public IsExistModel IsUserWithSuchName(string name, CancellationToken cancellationToken)
        {

            var user = _authGateway.GetByUserName(name, cancellationToken);

            return new IsExistModel
            {
                IsExist = user != null,
            };
        }
        public IsExistModel IsUserWithSuchEmail(string email, CancellationToken cancellationToken)
        {
            var user = _authGateway.GetByEmail(email, cancellationToken);

            return new IsExistModel
            {
                IsExist = user != null,
            };
        }
    }
}
