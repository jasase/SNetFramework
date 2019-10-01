namespace Framework.Abstraction.Services.Db
{
  public interface IDbSessionFactory
  {
    IDbSession CreateSession();
  }
}
