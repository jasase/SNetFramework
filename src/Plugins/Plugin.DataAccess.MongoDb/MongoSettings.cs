using Framework.Abstraction.Extension;

namespace Plugin.DataAccess.MongoDb
{
    public class MongoSettings : ISetting
    {
        public string ConnectionString { get; set; }
        public string DbName { get; set; }

        public MongoSettings()
        {
            ConnectionString = "mongodb://10.111.111.35/";
            DbName = "wallpaper";
        }
    }
}
