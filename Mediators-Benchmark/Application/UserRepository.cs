public interface IUserRepository
{
    Task<bool> CreateUserAsync(object user, CancellationToken cancellationToken = default);
    Task DeleteUserAsync(int id, CancellationToken cancellationToken = default);
}

public class UserRepository : IUserRepository
{
    public Task<bool> CreateUserAsync(object user, CancellationToken cancellationToken = default)
    {
        //Insert user into database

        return Task.FromResult(true);
    }

    public Task DeleteUserAsync(int id, CancellationToken cancellationToken = default)
    {
        //Delete user from database

        return Task.CompletedTask;
    }
}