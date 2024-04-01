namespace Mediator.Example;

public class CreateUserCommand : IRequest<bool>
{
    public string UserName { get; set; }
    public string Password { get; set; }
}

public class CreateUserCommandHandler(IUserRepository userRepository) : IRequestHandler<CreateUserCommand, bool>
{
    public async ValueTask<bool> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        //Do some validations
        //Do some business logic

        return await userRepository.CreateUserAsync(default, cancellationToken);
    }
}

public class DeleteUserCommand : IRequest
{
    public int UserId { get; set; }
}

public class DeleteUserCommandHandler(IUserRepository userRepository) : IRequestHandler<DeleteUserCommand>
{
    public async ValueTask<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        //Do some validations
        //Do some business logic

        await userRepository.DeleteUserAsync(request.UserId, cancellationToken);
        return Unit.Value;
    }
}