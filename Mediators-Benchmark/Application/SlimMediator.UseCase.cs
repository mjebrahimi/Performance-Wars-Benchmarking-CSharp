namespace SlimMediator.Example;

public class CreateUserCommand : IRequest<bool>
{
    public string UserName { get; set; }
    public string Password { get; set; }
}

public class CreateUserCommandHandler(IUserRepository userRepository) : IRequestHandler<CreateUserCommand, bool>
{
    public Task<bool> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        //Do some validations
        //Do some business logic

        return userRepository.CreateUserAsync(default, cancellationToken);
    }
}

public class DeleteUserCommand : IRequest
{
    public int UserId { get; set; }
}

public class DeleteUserCommandHandler(IUserRepository userRepository) : IRequestHandler<DeleteUserCommand>
{
    public Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        //Do some validations
        //Do some business logic

        return userRepository.DeleteUserAsync(request.UserId, cancellationToken);
    }
}