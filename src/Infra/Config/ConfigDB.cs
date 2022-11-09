using Microsoft.Data.SqlClient;

namespace IWantAPP.Infra.Config;

public class ConfigDb
{
    public string getStringCon()
    {
        var builderConn = new SqlConnectionStringBuilder
        {
            DataSource = "localhost",
            InitialCatalog = "IWantDb",
            UserID = "sa",
            Password = "@Sql2022"
        };

        return builderConn.ConnectionString;
    }
}
