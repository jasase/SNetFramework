using System.Collections.Generic;

namespace Framework.Contracts.Services.DataAccess.InfluxDb
{
    public interface IInfluxDbRead
    {
        IEnumerable<InfluxDbQueryResult> Read(string database, string query);
    }
}
