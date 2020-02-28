namespace DataLayer.Infrastructure.DbContexts
{
    public interface IWriteDbContextProvider<TIEntity> where TIEntity: class
    {
        IWriteDbContext<TIEntity> GetWriteContext();
    }
}
