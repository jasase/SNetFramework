using Framework.Abstraction.Extension;

namespace Plugin.DataAccess.InfluxDb
{
    public class InfluxDbSetting : ISetting
    {
        public string Server { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public InfluxDbSetting()
        {
            Username = "";
            Password = "";
            Server = "";
        }
    }
}
