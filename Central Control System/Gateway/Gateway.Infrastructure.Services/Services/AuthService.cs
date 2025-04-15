using Gateway.Domain.Services.Interfaces;
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

        public async Task<TokenInfoDTO> RegisterWorker(RegisterWorkerModel model)
        {
            var registerModel = new RegisterModel
            {
                Username = model.Username,
                Password = model.Password,
                Email = model.Email
            };

            var user = await _authGateway.RegisterDispatcher(registerModel);

            if (user == null)
            {
                throw new ServiceException("User registration failed");
            }

            var token = await _authGateway.Login(new LoginModel
            {
                Username = model.Username,
                Password = model.Password
            });

            if (token == null)
            {
                throw new ServiceException("User login failed");
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

            var createdWorker = await _operationsGateway.CreateOperationWorker(operationWorker, token.Token);

            if (createdWorker == null)
            {
                throw new ServiceException("Operation worker creation failed");
            }

            return token;
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
