using Framework.Contracts.Entities;
using System.Collections.Generic;

namespace Framework.Contracts.Services.InitialData
{
    public interface IInitialDataProvider<T>
        where T : Entity
    {
        IEnumerable<T> ProvideData();
    }
}
