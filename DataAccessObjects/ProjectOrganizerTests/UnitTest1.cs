using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.SqlClient;
using System.IO;
using System.Transactions;

namespace ProjectOrganizerTests
{
    [TestClass]
    public class UnitTestBase
    {
        private TransactionScope transaction;

        protected string ConnectionString { get; } = "Server=.\\SQLEXPRESS;Database=Projects;Trusted_Connection=True;";


        [TestInitialize]
        public void Setup()
        {
            transaction = new TransactionScope(); // BEGIN TRANSACTION

            string sql = File.ReadAllText("setup.sql");

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                
                SqlCommand command = new SqlCommand(sql, conn);

                command.ExecuteNonQuery();
            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Dispose(); // ROLLBACK TRANSACTION
        }
    }
}
