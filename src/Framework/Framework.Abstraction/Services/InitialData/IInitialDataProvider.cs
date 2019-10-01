using Framework.Abstraction.Entities;
using System.Collections.Generic;

namespace Framework.Abstraction.Services.InitialData
{
    public interface IInitialDataProvider<T>
        where T : Entity
    {
        IEnumerable<T> ProvideData();
    }
}
