//definindo a conexão com o banco de dados e a definição com namespace
using System.Data.SqlClient;

namespace TelaLogin
{
    class Conexao
    {
        //Método para realizar a conexão com banco
        public static SqlConnection getConnection()
        {
            //Criando um objeto (cnn) do tipo SqlConnection a configura a string de conecxão
            SqlConnection cnn = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=login;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

            //Retorna uma conexão com o banco quando esse método for chamado
            return cnn;

        }
    }
}
