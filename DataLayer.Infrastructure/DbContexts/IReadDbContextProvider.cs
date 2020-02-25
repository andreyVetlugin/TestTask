namespace DataLayer.Infrastructure.DbContexts
{
    public interface IReadDbContextProvider<TIEntity> where TIEntity : class
    {
        IReadDbContext<TIEntity> GetReadContext();
    }
}
