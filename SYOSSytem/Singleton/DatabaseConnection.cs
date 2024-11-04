namespace SYOSSytem.Singleton;

public class DatabaseConnection
{
    private static DatabaseConnection instance;
    private static readonly object padlock = new();

    private DatabaseConnection()
    {
        // Initialize the connection string
        ConnectionString =
            "Data Source=SHAMMIKA-LAPTOP\\SQLEXPRESS;Initial Catalog=SYOS_DB;Integrated Security=True;TrustServerCertificate=True";
    }

    public static DatabaseConnection Instance
    {
        get
        {
            lock (padlock)
            {
                if (instance == null) instance = new DatabaseConnection();
                return instance;
            }
        }
    }

    public string ConnectionString { get; }
}