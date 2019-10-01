using Framework.Abstraction.Entities;

namespace Framework.Abstraction.Services.InitialData
{
    public interface IInitialDataService
    {
        void PassInitialData<T>(IInitialDataProvider<T> dataProvider)
            where T : Entity;
    }
}
