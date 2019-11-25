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
    public partial class Cadastro : Form
    {
        //String de conexao como banco de dado
        string connectionString = (@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=login;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

        string idUsuarioControle = "vazio";
        string senhaControle = "";

        public Cadastro()
        {
            InitializeComponent();
        }

         //Validando busca   
        private void validarBusca()
        {
            //Verifica se o campo estiver vazio, interrompe a sub-rotina
            if (idUsuarioControle.Equals("vazio"))
            {
                //Mensagem ao usuário para consultar 
                MessageBox.Show("Consulte o usuário que deseja alterar clicando no botão consultar");

                //Interrompe a sub-rotina
                return;
            }
        }

        //Validando os dados
        private bool validaDados()
        {
            //Verificar se txtNome está preenchido, se não estiver preenchido retorna falso
            if (string.IsNullOrEmpty(txtNome.Text))
            {
                //Mensagem ao usuário obrigatorio
                MessageBox.Show("Preenchimento de campo obrigatório", "Nome", MessageBoxButtons.OK, MessageBoxIcon.Information);

                //Coloca o cursor no txtNome
                txtNome.Focus();

                //Retorno
                return false;
            }

            //Verifica se o txtEmail está preenchido, Se for nulo ou vazio retorna falso
            if (string.IsNullOrEmpty(txtEmail.Text))
            {
                //Mensagem ao usuário obrogatorio
                MessageBox.Show("Preenchimento de campo obrigatório", "Email", MessageBoxButtons.OK, MessageBoxIcon.Information);

                //Coloca o cursor no txtEmail
                txtEmail.Focus();

                //Retorno
                return false;
            }

            //Verifica se o txtSenha está preenchido, Se for nulo ou vazio retorna falso
            if (string.IsNullOrEmpty(txtSenha.Text))
            {
                //Mensagem ao usuário obrigatorio
                MessageBox.Show("Preenchimento de campo obrigatório", "Senha", MessageBoxButtons.OK, MessageBoxIcon.Information);

                //Coloca o cursor no txtSenha
                txtSenha.Focus();

                //Retorno
                return false;
            }

            // Verifica se o idUsuario está preenchido, Se for nulo ou vazio retorna falso
             if (idUsuarioControle != "vazio" && senhaControle != txtSenha.Text)
            {
                //Mensagem ao usuário de erro de senha
                MessageBox.Show("A senha não confere com a senha anterior", "Senha", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            txtConfE.Clear();
            txtSenha.Clear();
            txtConfS.Clear();
        }

        //Botão incluir
        private void btnIncluir_Click(object sender, EventArgs e)
        {

            //Verificar se está preenchido, se não estiver preenchido retorna falso
            if (txtNome.Text == "" || txtEmail.Text == "" || txtConfE.Text == "" || txtSenha.Text == "" || txtConfS.Text == "")
            {
                //Mensagem ao usuário obrigatorio
                MessageBox.Show("Campos não pode ficar em branco");
            }
            else
            {
                //Inserindo informação ao banco de dados
                string sqlQuery = "INSERT INTO dbo.login (email, senha, nome)" + "VALUES('" + txtEmail.Text + "','" + txtSenha.Text + "','" + txtNome.Text + "')";

                SqlConnection conn = new SqlConnection(connectionString);
                SqlCommand cmd = new SqlCommand(sqlQuery, conn);

                cmd.CommandType = CommandType.Text;
                conn.Open();

                //Confirmando preenchimento de email
                if (txtEmail.Text == txtConfE.Text)
                {
                    //Mensagem ao usuário confirmado
                    MessageBox.Show("Confirmado");
                }
                else
                {
                    //Mensagem ao usuário incorreto
                    MessageBox.Show("Email Incorreto");
                }

                //Confirmando preenchimento de senha
                if (txtSenha.Text == txtConfS.Text)
                {
                    //Mensagem ao usuário confirmado
                    MessageBox.Show("Confirmado");
                }
                else
                {
                    //Mensagem ao usuário incorreto
                    MessageBox.Show("Senha Incorreto");
                }

                //Tratamento de exceções 
                try
                {
                    int i = cmd.ExecuteNonQuery();
                    if (i > 0)
                    {

                        //Mensagem ao usuário suscesso
                        MessageBox.Show("Cadastro realizado co sucesso");
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

        //Botão Consulta
        private void bntConsulta_Click(object sender, EventArgs e)
        {
            //Selecionando informação no banco de dados
            string sql = "(SELECT * FROM dbo.login WHERE email = '" + txtEmail.Text + "')";

            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.CommandType = CommandType.Text;
            SqlDataReader reader;
            conn.Open();

            //Execução comando para o banco
            reader = cmd.ExecuteReader();

            //Tratamento de exceções 
            try
            {
                //Caso não haja informção no bando de dados
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
                    txtConfE.Text = reader["email"].ToString();
                    idUsuarioControle = reader["id_usuario"].ToString();
                    senhaControle = reader["senha"].ToString();
                }
            }

            catch (Exception ex)
            {
                //Mensagem de erro
                MessageBox.Show("Erro" + ex.ToString());
            }

            //Finailzando conexão com banco
            finally
            {
                conn.Close();
            }
        }

        //Botão Alterar
        private void btnAlterar_Click(object sender, EventArgs e)
        {
            validarBusca();

            // Antes de alterar o registro é preciso validar os dados de preenchimento  chama o método para validar a entrada de dados se retornou falso, interrompe o processamento
            if (validaDados() == false)
            {
                return;
            }

            //Declaração da variável para guardar as instruções SQL
            string sqlQuery;

            //Criando conexão chamando o método getConnection da classe Conexao
            SqlConnection conAlt = Conexao.getConnection();

            //Criando a instrução sql, parametrizada erro de codigo ter que arrumar o codigo UPDATE
            sqlQuery = "UPDATE dbo.login SET email='" + txtEmail.Text
                + "', nome='" + txtNome.Text
                + "' WHERE id_usuario = '" + idUsuarioControle + "' ";

            //Tratamento de exceções 
            try
            {
                conAlt.Open();

                SqlCommand cmd = new SqlCommand(sqlQuery, conAlt);


                //Executa o comando para o banco
                cmd.ExecuteNonQuery();

                //Mensagem ao usuário alterado
                MessageBox.Show("Usuário alterado com sucesso", "Usuario", MessageBoxButtons.OK, MessageBoxIcon.Information);

                //Limpa os campos para nova entrada de dados
                limparControles();

            }

            catch (Exception ex)
            {
                //Mensagem ao usuário de erro de alteração
                MessageBox.Show("Problema ao alterar usuário " + ex, "Usuario", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            senhaControle = "";
        }
        
        //Botão Excluir
        private void btnExcluir_Click(object sender, EventArgs e)
        {
            validarBusca();
            validaDados();
            if (!validaDados())
            {
                return;
            }

            //Excluindo registro do banco de dados
            if (MessageBox.Show("Deseja excluir permanentemente o registro?", "Usuario", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) ;
            {
                string sqlQuery;

                //Criando conexão chamando o método getConnection da classe Conexao
                SqlConnection conAlt = Conexao.getConnection();

                //Deletando informação do banco
                sqlQuery = "DELETE FROM dbo.login WHERE id_usuario ='" + idUsuarioControle + "' ";

                //Tratamento de exceções
                try
                {
                    conAlt.Open();
                    SqlCommand cmd = new SqlCommand(sqlQuery, conAlt);


                    //Executa o commando no banco
                    cmd.ExecuteNonQuery();

                    //Mensagem ao usuário excluido
                    MessageBox.Show("Cliente excluído com sucesso", "Usuario", MessageBoxButtons.OK);
                }

                catch (Exception ex)
                {
                    //Mensagem ao usuário de erro de exclusão
                    MessageBox.Show("Problema ao incluir usuario" + ex, "Usuario", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                //Finalizando conexão com banco
                finally
                {
                    if (conAlt != null)
                    {
                        conAlt.Close();
                    }
                }
            }
        }


        //Botão Sair
        private void btnSair_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
