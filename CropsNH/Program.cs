using System.Data.SqlClient;
using MySql.Data.MySqlClient;

namespace CropsNH
{
    internal static class Program
    {
        public static MySqlConnection mConnection = new(@"server=localhost;uid=root;pwd=1234;database=CropsNH");
        public static MainMenu? mMenu;
        [STAThread]
        static void Main()
        {
            mConnection.Open();
            ApplicationConfiguration.Initialize();
            Application.Run(mMenu = new MainMenu());
        }
    }
}