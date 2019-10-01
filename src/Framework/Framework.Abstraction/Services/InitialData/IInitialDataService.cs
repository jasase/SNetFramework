using Framework.Contracts.Entities;

namespace Framework.Contracts.Services.InitialData
{
    public interface IInitialDataService
    {
        void PassInitialData<T>(IInitialDataProvider<T> dataProvider)
            where T : Entity;
    }
}
