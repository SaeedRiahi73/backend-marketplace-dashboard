using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task_Application.Common.Responses;
using Task_Application.Contracts.Interfaces.Security;
using Task_Application.Contracts.Interfaces.Users;
using Task_Application.Features.Users.Requests.Commands;
using Task_Domain.Common;
using Task_Domain.Entities;



namespace Task_Application.Features.Users.Handler.Command
{
    public class RegisterUserHandler : IRequestHandler<RegisterUserRequest, ResultInfo<Guid>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IMapper _mapper;

        public RegisterUserHandler(IUserRepository userRepository, IPasswordHasher passwordHasher, IMapper mapper)
        {
            _passwordHasher = passwordHasher;
            _mapper = mapper;
            _userRepository = userRepository;
        }
        public async Task<ResultInfo<Guid>> Handle(RegisterUserRequest request, CancellationToken cancellationToken)
        {

            if (string.IsNullOrEmpty(request.CreateUser.Username))
                return ResultInfo<Guid>.Failure(["Username is empty"]);

            if (string.IsNullOrEmpty(request.CreateUser.Password))
                return ResultInfo<Guid>.Failure(["Password is empty"]);

            if (await _userRepository.ExistsByUsernameAsync(request.CreateUser.Username))
                return ResultInfo<Guid>.Failure(["Username already exists"]);

            string passwordHash = _passwordHasher.GenerateHash(request.CreateUser.Password);

            User user = new User(request.CreateUser.Username, request.CreateUser.Email, passwordHash);

            await _userRepository.AddAsync(user);

            return ResultInfo<Guid>.Success(user.Id, "User created successfully");
        }
    }
}
