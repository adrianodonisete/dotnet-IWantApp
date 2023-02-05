using Microsoft.Data.SqlClient;

namespace IWantAPP.Infra.Config;

public class ConfigDb
{
    public string getStringCon()
    {
        //var builderConn = new SqlConnectionStringBuilder
        //{
        //    DataSource = "localhost",
        //    InitialCatalog = "IWantDb",
        //    UserID = "sa",
        //    Password = "@Sql2022",
        //    //TrustServerCertificate = true,
        //    IntegratedSecurity = true,
        //};

        ////private const string connString = "Server=.\\SQLEXPRESS;Database=Blog;Integrated Security=SSPI;TrustServerCertificate=True";

        //return builderConn.ConnectionString;

        return "Server=localhost; Database=IWantDb; User Id=sa; Password=@Sql2022; TrustServerCertificate=True";
    }
}
