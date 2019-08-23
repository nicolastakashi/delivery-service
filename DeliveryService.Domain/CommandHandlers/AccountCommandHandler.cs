using DeliveryService.Domain.Commands;
using DeliveryService.Domain.Entities;
using DeliveryService.Domain.Repositories.Write;
using DeliveryService.Domain.Service;
using MediatR;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace DeliveryService.Domain.CommandHandlers
{
    public class AccountCommandHandler
        : IRequestHandler<CreateUserSessionCommand, DomainResult<string>>
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IJwtAuthService _jwtAuthService;

        public AccountCommandHandler(IAccountRepository accountRepository, IJwtAuthService jwtAuthService)
        {
            _accountRepository = accountRepository;
            _jwtAuthService = jwtAuthService;
        }

        public async Task<DomainResult<string>> Handle(CreateUserSessionCommand command, CancellationToken cancellationToken)
        {
            var user = await _accountRepository.GetUserByEmailAndPassword(command.Email, command.Password);

            if (user is null)
            {
                return DomainResult.Failure<string>("User not found", HttpStatusCode.Unauthorized);
            }

            var token = _jwtAuthService.CreateJwtToken(user);

            return DomainResult.Ok(token);
        }
    }
}
