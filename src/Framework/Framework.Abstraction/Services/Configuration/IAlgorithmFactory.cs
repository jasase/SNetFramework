using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Abstraction.Services.Configuration
{
    public interface IAlgorithmFactory
    {
        string AlgorithName { get; }
        T GetAlgorithm<T>();
    }

    /// <summary>
    /// Factory Interface für ein Setting Objekt.
    /// Implementierung soll einen ALgorithmus vom Typ T erstellen
    /// </summary>
    /// <typeparam name="T">Typ des Algorithmuses</typeparam>
    public interface IAlgorithmFactory<T> : IAlgorithmFactory
        where T : class
    {
        T GetAlgorithm();        
    }

    /// <summary>
    /// Factory Interface für ein Setting Objekt.
    /// Implementierung soll einen ALgorithmus vom Typ T erstellen
    /// </summary>
    /// <typeparam name="T">Typ des Algorithmuses</typeparam>
    /// <typeparam name="TSetting">Typ des Algorithmuses</typeparam>
    public interface IAlgorithmFactory<T, TSetting> : IAlgorithmFactory<T>
        where T : class
        where TSetting : IAlgorithmSetting
        //TODO where TSetting : without Constructor parameter
    {
        T GetAlgorithm();
    }
}
