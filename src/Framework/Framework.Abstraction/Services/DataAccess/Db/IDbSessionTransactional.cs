namespace Framework.Abstraction.Services.Db
{
    public interface IDbSessionTransactional : IDbSession
    {
        void BeginTransaction();

        void EndTransaction();
    }
}
