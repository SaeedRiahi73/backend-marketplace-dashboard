using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task_Application.Common.Responses;
using Task_Application.Contracts.Interfaces;
using Task_Application.Contracts.Interfaces.Security;
using Task_Application.Contracts.Interfaces.Users;
using Task_Application.Dtos.User;
using Task_Application.Enums;
using Task_Application.Features.Users.Requests.Commands;
using Task_Domain.Common;
using Task_Domain.Entities;
using Task_Domain.Enums;



namespace Task_Application.Features.Users.Handler.Command
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommandRequest, ResultInfo<UserDto>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public CreateUserCommandHandler(
            IUserRepository userRepository,
            IPasswordHasher passwordHasher,
            ICurrentUserService currentUserService,
            IMapper mapper,
            IUnitOfWork unitOfWork)
        {
            _passwordHasher = passwordHasher;
            _currentUserService = currentUserService;
            _mapper = mapper;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<ResultInfo<UserDto>> Handle(
            CreateUserCommandRequest request,
            CancellationToken cancellationToken)
        {
            Guid? currentUserId = _currentUserService.UserId;
            UserRole? currentRole = _currentUserService.Role;

            if (currentUserId is null || currentRole is null)
            {
                return ResultInfo<UserDto>.Failure(
                    ["The user is not authenticated."],
                    status: ResultStatus.Unauthorized);
            }

            if (currentRole != UserRole.Admin)
            {
                return ResultInfo<UserDto>.Failure(
                    ["Only administrators can create users."],
                    status: ResultStatus.Forbidden);
            }

            CreateUserDto dto = request.CreateUser;
            UserRole role = dto.Role ?? UserRole.ProductManager;

            if (role != UserRole.ProductManager)
            {
                return ResultInfo<UserDto>.Failure(
                    ["The selected role is not allowed for user creation."],
                    status: ResultStatus.BadRequest);
            }

            if (await _userRepository.ExistsByUsernameAsync(
                    dto.Username,
                    cancellationToken))
            {
                return ResultInfo<UserDto>.Failure(
                    ["Username already exists."],
                    status: ResultStatus.Conflict);
            }

            if (await _userRepository.ExistsByEmailAsync(
                    dto.Email,
                    cancellationToken))
            {
                return ResultInfo<UserDto>.Failure(
                    ["Email already exists."],
                    status: ResultStatus.Conflict);
            }

            string passwordHash = _passwordHasher.GenerateHash(dto.Password);

            User user = new User(
                dto.Username,
                dto.Email,
                passwordHash,
                role);

            await _userRepository.AddAsync(
                user,
                cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            UserDto response = _mapper.Map<UserDto>(user);
            response.CanChangeStatus =
                role == UserRole.ProductManager &&
                !user.IsSystemUser &&
                user.Id != currentUserId.Value;

            return ResultInfo<UserDto>.Success(
                response,
                "User created successfully.",
                ResultStatus.Created);
        }
    }
}
