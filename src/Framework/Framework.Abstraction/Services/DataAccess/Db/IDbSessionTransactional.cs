namespace Framework.Contracts.Services.Db
{
    public interface IDbSessionTransactional : IDbSession
    {
        void BeginTransaction();

        void EndTransaction();
    }
}
