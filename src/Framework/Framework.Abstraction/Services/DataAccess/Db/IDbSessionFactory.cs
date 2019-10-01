namespace Framework.Contracts.Services.Db
{
  public interface IDbSessionFactory
  {
    IDbSession CreateSession();
  }
}
