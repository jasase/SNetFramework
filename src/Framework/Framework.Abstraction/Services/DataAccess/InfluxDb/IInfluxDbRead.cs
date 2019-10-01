using System.Collections.Generic;

namespace Framework.Abstraction.Services.DataAccess.InfluxDb
{
    public interface IInfluxDbRead
    {
        IEnumerable<InfluxDbQueryResult> Read(string database, string query);
    }
}
