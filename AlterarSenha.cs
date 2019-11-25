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
    public partial class AlterarSenha : Form
    {
        //String de conexao como banco de dado
        string connectionString = (@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=login;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
    
        //Variaveis
        string idUsuarioControle = "vazio";
        string emailControle = "";

        public AlterarSenha()
        {
            InitializeComponent();
        }
        
        //Validando  busca
        private void validarBusca()
        {
            //Verifica se tem o nome txtEmail. Se o campo estiver vazio, interrompe a sub-rotina
            if (idUsuarioControle.Equals("vazio"))
            {
                
                //Mensagem ao usuário de alteração
                MessageBox.Show("Consulte o usuário que deseja alterar clicando no botão consultar");
                //Interrompe a sub-rotina
                return;
            }
        }

        //Validando dados
        private bool validaDados()
        {

            //Verifica se o txtEmail está preenchido, Se for nulo ou vazio retorna falso
            if (string.IsNullOrEmpty(txtEmail.Text))
            {
                //Mensagem ao usuário obrigatorio
                MessageBox.Show("Preenchimento de campo obrigatório", "Email", MessageBoxButtons.OK, MessageBoxIcon.Information);

                //Coloca o cursor no txtEmail
                txtEmail.Focus();

                //Retorno
                return false;
            }
            if (idUsuarioControle != "vazio" && emailControle != txtEmail.Text)
            {

                //Mensagem ao usuário de erro no email
                MessageBox.Show("O Email não confere com a email anterior", "Email", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            //Se todas as validações passaram no teste, retorna verdadeiro
            return true;
        }
        //Sub-rotina para limpar os controles do formulário
        private void limparControles()
        {
            //Limpa os textos dos TextBox
            txtNome.Clear();
            txtEmail.Clear();
            txtSenha.Clear();
            txtConfS.Clear();
        }

        //Botão Consulta
        private void bntConsulta_Click(object sender, EventArgs e)
        {
            //Selecionado informação do banco
            string sql = "(SELECT * FROM dbo.login WHERE email = '" + txtEmail.Text + "')";

            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.CommandType = CommandType.Text;
            SqlDataReader reader;
            conn.Open();


            //Execução comando para o banco
            reader = cmd.ExecuteReader();

            try
            {
                if (!reader.Read())
                {

                    //Mensagem ao usuário cadastro não encontrado
                    MessageBox.Show("Cadastro não encontrado");
                }
                else
                {

                    //Trazer as informações do banco para os seguintes campos nome, email, id e senha
                    txtNome.Text = reader["nome"].ToString();
                    txtEmail.Text = reader["email"].ToString();
                    idUsuarioControle = reader["id_usuario"].ToString();
                    emailControle = reader["email"].ToString();
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

        //botão Alterar
        private void btnAlterar_Click(object sender, EventArgs e)
        {
            validarBusca();

            // Antes de alterar o registro é preciso validar os dados de preenchimento obrigatório chama o método para validar a entrada de dados se retornou falso, interrompe o processamento
            if (validaDados() == false)
            {
                return;
            }

            //Declaração da variável para guardar as instruções SQL
            string sqlQuery;

            //cria conexão chamando o método getConnection da classe Conexao
            SqlConnection conAlt = Conexao.getConnection();


            //Criando a a instrução sql, parametrizada erro de codigo ter que arrumar o codigo UPDATE
            sqlQuery = "UPDATE dbo.login SET senha='" + txtSenha.Text + "' WHERE id_usuario = '" + idUsuarioControle + "' ";

            //Tratamento de exceções
            try
            {
                conAlt.Open();

                SqlCommand cmd = new SqlCommand(sqlQuery, conAlt);

                //executa o comando
                cmd.ExecuteNonQuery();

                //Mensagem ao usuário alterado
                MessageBox.Show("Senha alterado com sucesso", "Senha", MessageBoxButtons.OK, MessageBoxIcon.Information);

                //Limpa os campos para nova entrada de dados
                limparControles();
            }

            catch (Exception ex)
            {

                //Mensagem ao usuário erro ao alterar
                MessageBox.Show("Problema ao alterar senha " + ex, "Senha", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            //Finalizando conexão com banco
            finally
            {
                if (conAlt != null)
                {
                    conAlt.Close();
                }
            }
            idUsuarioControle = "vazio";
            emailControle = "";
        }

        //Botão Sair
        private void btnSair_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
