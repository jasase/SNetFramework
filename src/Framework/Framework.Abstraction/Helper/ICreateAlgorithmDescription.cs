namespace Framework.Contracts.Helper
{
    public interface ICreateAlgorithmDescription
    { }

    public interface ICreateAlgorithmDescription<TAlgorithm> : ICreateAlgorithmDescription
    {
        TAlgorithm Create();
    }
}
