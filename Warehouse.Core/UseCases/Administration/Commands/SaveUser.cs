﻿using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Vayosoft.Core.Commands;
using Vayosoft.Core.Persistence;
using Vayosoft.Core.Persistence.Commands;
using Vayosoft.Core.SharedKernel.Exceptions;
using Warehouse.Core.Entities.Enums;
using Warehouse.Core.Entities.Models;
using Warehouse.Core.Entities.Models.Security;
using Warehouse.Core.Exceptions;
using Warehouse.Core.Persistence;
using Warehouse.Core.Services;
using Warehouse.Core.Utilities;

namespace Warehouse.Core.UseCases.Administration.Commands;

public class SaveUser : ICommand
{
    public long Id { get; set; }
    public string Username { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public string Type { get; set; }
    public DateTime? Registered { get; set; }
    public DateTime? Deregistered { get; set; }
    public string CultureId { get; set; }
    public long ProviderId { get; set; }
    public LogEventType? LogLevel { get; set; }
    public string Password { get; set; }
    public List<string> Roles { get; set; }

    public class SaveUserValidator : AbstractValidator<SaveUser>
    {
        public SaveUserValidator()
        {
            RuleFor(u => u.Username).NotEmpty();
            RuleFor(u => u.Email).NotEmpty().EmailAddress();
        }
    }
}

public class HandleSaveUser : ICommandHandler<SaveUser>
{
    private readonly IUserStore<UserEntity> _userStore;
    private readonly IUserContext _userContext;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ILogger<HandleSaveUser> _logger;

    public  HandleSaveUser(IUserStore<UserEntity> userStore, IUserContext userContext, IPasswordHasher passwordHasher, ILogger<HandleSaveUser> logger)
    {
        _userStore = userStore;
        _userContext = userContext;
        _passwordHasher = passwordHasher;
        _logger = logger;
    }

    public async Task<Unit> Handle(SaveUser command, CancellationToken cancellationToken)
    {
        try
        {
            var identity = _userContext.User.Identity ?? throw new ArgumentNullException(nameof(_userContext.User.Identity));
            var identityType = identity.GetUserType();
            var providerId = identity.GetProviderId();
            await _userContext.LoadSessionAsync();

            if (!_userContext.IsAdministrator)
                throw new NotEnoughPermissionsException();

            UserEntity entity;
            if (command.Id > 0)
            {
                entity = await _userStore.FindByIdAsync(command.Id, cancellationToken);
                if (entity == null)
                    throw new EntityNotFoundException(nameof(UserEntity), command.Id);

                if (!string.IsNullOrEmpty(command.Password))
                    entity.PasswordHash = _passwordHasher.HashPassword(command.Password);
            }
            else
            {
                if (string.IsNullOrEmpty(command.Password))
                    throw new ArgumentNullException(nameof(command.Password));

                entity = new UserEntity(command.Username)
                {
                    PasswordHash = _passwordHasher.HashPassword(command.Password),
                };
            }

            entity.Email = command.Email;
            entity.Phone = command.Phone;

            var userType = Enum.Parse<UserType>(command.Type);
            entity.Type = userType > identityType ? userType : identityType;
            entity.ProviderId = !_userContext.IsSupervisor ? providerId : command.ProviderId;

            entity.LogLevel = command.LogLevel;

            await _userStore.UpdateAsync(entity, cancellationToken);

            if (command.Roles.Any())
            {
                if (_userStore is IUserRoleStore store)
                {
                    var userRoles = new List<string>();
                    foreach (var commandRole in command.Roles)
                    {
                        var role = await store.FindRoleByIdAsync(commandRole, cancellationToken);
                        if (role != null)
                            userRoles.Add(role.Id);
                    }
                    await store.UpdateUserRolesAsync(entity.Id, userRoles, cancellationToken);
                }
            }
        }
        catch (Exception e)
        {
            _logger.LogError($"{e.Message}\r\n{e.StackTrace}");
        }

        return Unit.Value;
    }
}
