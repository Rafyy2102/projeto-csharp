using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace TelaLogin
{
    
    public partial class Login : Form
    {
        //String de conexao como banco de dado
        string connectionString = (@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=login;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

        public Login()
        {
            InitializeComponent();
        }
        
        //Botão Login
        private void btnLogin_Click(object sender, EventArgs e)
        {
            // Confirmando login e senha
            if (txtEmail.Text == "" || txtSenha.Text == "")
            {

                //Mensagem ao usuário obrigatorio
                MessageBox.Show("Obrigatorio o preenchimento dos campos Email e Senha");
            }
            else
            {
                //Selecionando informação do banco
                string sql = "(SELECT * FROM dbo.login WHERE email = '" + txtEmail.Text + "' and senha = '" + txtSenha.Text + "')";

                SqlConnection conn = new SqlConnection(connectionString);
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.CommandType = CommandType.Text;
                SqlDataReader reader;
                conn.Open();

                //Executa o comando no banco
                reader = cmd.ExecuteReader();

                //Tratamento de  exceções 
                try
                {
                    if (!reader.Read())
                    {

                        //Mensagem ao usuário incorreto
                        MessageBox.Show("Login e senha Incorretos");
                    }
                    else
                    {

                        //Mensagem ao usuário sucesso
                        MessageBox.Show("Login efetuado com sucesso");
                    }
                }

                catch (Exception ex)
                {

                    //Mensagem de erro
                    MessageBox.Show("Erro" + ex.ToString());
                }

                //Finalizando conexão com banco
                finally
                {
                    conn.Close();
                }
            }
        }


        //Botão registrar-se
        private void btnRegis_Click(object sender, EventArgs e)
        {
            //Novo formulario 
            Cadastro novo = new Cadastro();
            novo.Show();
            this.Hide();
        }


        //Botão esqueceu a senha
        private void btnEsquece_Click(object sender, EventArgs e)
        {
            //novo formulario 
            AlterarSenha novo = new AlterarSenha();
            novo.Show();
            this.Hide();
        }
    }
}
