using Microsoft.Data.SqlClient;

namespace IWantAPP.Infra.Config;

public class Connection
{
    public SqlConnection GetConnection()
    {
        var strCon = new ConfigDb().getStringCon();
        return new SqlConnection(strCon);
    }
}
