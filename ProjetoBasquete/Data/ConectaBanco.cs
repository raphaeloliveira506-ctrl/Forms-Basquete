using System;
using System.Data;
using MySql.Data.MySqlClient;
using ProjetoBasquete.Models;

namespace ProjetoBasquete.Data
{
    /// <summary>
    /// Classe de conexão com o banco de dados MySQL.
    /// Segue o padrão da Primeira Parte do trabalho:
    /// constantes de conexão, campo conexão, campo mensagem e métodos
    /// com try/catch retornando bool.
    /// </summary>
    public class ConectaBanco
    {
        // String de conexão - ALTERE DE ACORDO COM SEU BANCO E SERVIDOR
        public const string SERVER = "localhost";
        public const int PORT = 3306;
        public const string DATABASE = "DB_JogadoresBasquete";
        public const string USER = "root";
        public const string PASSWORD = "SUA-SENHA";

        MySqlConnection conexao = new MySqlConnection(
            $"server={SERVER};" +
            $"port={PORT};" +
            $"database={DATABASE};" +
            $"user id={USER};" +
            $"password={PASSWORD};");

        public string mensagem;

        // ==================== MÉTODO insereJogador (padrão insereBanda) ====================

        public bool insereJogador(Jogador novoJogador)
        {
            try
            {
                conexao.Open();
                MySqlCommand cmd =
                    new MySqlCommand("sp_insereJogador", conexao);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("nome", novoJogador.Nome);
                cmd.Parameters.AddWithValue("altura", novoJogador.Altura);
                cmd.Parameters.AddWithValue("posicao", novoJogador.FkPosicao);
                cmd.Parameters.AddWithValue("camisa", novoJogador.NumeroCamisa);
                cmd.Parameters.AddWithValue("time", novoJogador.Time);
                cmd.Parameters.AddWithValue("nacionalidade", novoJogador.Nacionalidade);
                cmd.ExecuteNonQuery(); // executar no banco
                return true;
            }
            catch (MySqlException erro)
            {
                mensagem = erro.Message;
                return false;
            }
            finally
            {
                conexao.Close();
            }
        }// fim do insereJogador

        // ==================== MÉTODO listaJogadores (DataTable, padrão listaBandas) ====================

        public DataTable listaJogadores()
        {
            MySqlCommand cmd = new MySqlCommand("sp_listaJogadores", conexao);
            cmd.CommandType = CommandType.StoredProcedure;
            try
            {
                conexao.Open();
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable tabela = new DataTable();
                da.Fill(tabela);
                return tabela;
            }
            catch (MySqlException e)
            {
                mensagem = "Erro: " + e.Message;
                return null;
            }
            finally
            {
                conexao.Close();
            }
        }// fim listaJogadores

        // ==================== MÉTODO deletaJogador (padrão deletaBanda) ====================

        public bool deletaJogador(int idRemoveJogador)
        {
            MySqlCommand cmd = new MySqlCommand("sp_removeJogador", conexao);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("idjogador", idRemoveJogador);
            try
            {
                conexao.Open();
                cmd.ExecuteNonQuery(); // executa o comando
                return true;
            }
            catch (MySqlException e)
            {
                mensagem = "Erro: " + e.Message;
                return false;
            }
            finally
            {
                conexao.Close();
            }
        }// fim deletaJogador

        // ==================== MÉTODO atualizaJogador ====================

        public bool atualizaJogador(Jogador jogador)
        {
            try
            {
                conexao.Open();
                MySqlCommand cmd =
                    new MySqlCommand("sp_atualizaJogador", conexao);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("id", jogador.Id);
                cmd.Parameters.AddWithValue("nome", jogador.Nome);
                cmd.Parameters.AddWithValue("altura", jogador.Altura);
                cmd.Parameters.AddWithValue("posicao", jogador.FkPosicao);
                cmd.Parameters.AddWithValue("camisa", jogador.NumeroCamisa);
                cmd.Parameters.AddWithValue("time", jogador.Time);
                cmd.Parameters.AddWithValue("nacionalidade", jogador.Nacionalidade);
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (MySqlException erro)
            {
                mensagem = erro.Message;
                return false;
            }
            finally
            {
                conexao.Close();
            }
        }

        // ==================== MÉTODO listaPosicoes (para ComboBox) ====================

        public DataTable listaPosicoes()
        {
            MySqlCommand cmd = new MySqlCommand("sp_listaPosicoes", conexao);
            cmd.CommandType = CommandType.StoredProcedure;
            try
            {
                conexao.Open();
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable tabela = new DataTable();
                da.Fill(tabela);
                return tabela;
            }
            catch (MySqlException e)
            {
                mensagem = "Erro: " + e.Message;
                return null;
            }
            finally
            {
                conexao.Close();
            }
        }

        // ==================== MÉTODO buscaJogadorPorId ====================

        public DataTable buscaJogadorPorId(int id)
        {
            MySqlCommand cmd = new MySqlCommand("sp_buscaJogadorPorId", conexao);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("id", id);
            try
            {
                conexao.Open();
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable tabela = new DataTable();
                da.Fill(tabela);
                return tabela;
            }
            catch (MySqlException e)
            {
                mensagem = "Erro: " + e.Message;
                return null;
            }
            finally
            {
                conexao.Close();
            }
        }

        // ==================== MÉTODO alternarPreferido ====================

        public bool alternarPreferido(int id, bool preferido)
        {
            try
            {
                conexao.Open();
                MySqlCommand cmd =
                    new MySqlCommand("sp_alternarPreferido", conexao);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("id", id);
                cmd.Parameters.AddWithValue("preferido", preferido ? 1 : 0);
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (MySqlException erro)
            {
                mensagem = erro.Message;
                return false;
            }
            finally
            {
                conexao.Close();
            }
        }

        // ==================== MÉTODO validarLogin ====================

        public Usuario validarLogin(string login, string senha)
        {
            try
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("sp_validarLogin", conexao);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("usuario", login);
                cmd.Parameters.AddWithValue("senha", senha);

                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    return new Usuario
                    {
                        Id = Convert.ToInt32(reader["idUsuario"]),
                        Login = reader["Usuario"]?.ToString(),
                        NomeCompleto = reader["NomeCompleto"]?.ToString(),
                        Ativo = Convert.ToBoolean(reader["Ativo"])
                    };
                }

                return null;
            }
            catch (MySqlException erro)
            {
                mensagem = erro.Message;
                return null;
            }
            finally
            {
                conexao.Close();
            }
        }

        // ==================== MÉTODOS CRUD DE GÊNEROS ====================

        public bool insereGenero(Genero novoGenero)
        {
            try
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("sp_insereGenero", conexao);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("nome", novoGenero.Nome);
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (MySqlException erro)
            {
                mensagem = erro.Message;
                return false;
            }
            finally
            {
                conexao.Close();
            }
        }

        public bool atualizaGenero(Genero genero)
        {
            try
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("sp_atualizaGenero", conexao);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("id", genero.Id);
                cmd.Parameters.AddWithValue("nome", genero.Nome);
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (MySqlException erro)
            {
                mensagem = erro.Message;
                return false;
            }
            finally
            {
                conexao.Close();
            }
        }

        public bool deletaGenero(int idRemoveGenero)
        {
            MySqlCommand cmd = new MySqlCommand("sp_removeGenero", conexao);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("id", idRemoveGenero);
            try
            {
                conexao.Open();
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (MySqlException e)
            {
                mensagem = "Erro: " + e.Message;
                return false;
            }
            finally
            {
                conexao.Close();
            }
        }

        public DataTable listaGeneros()
        {
            MySqlCommand cmd = new MySqlCommand("sp_listaGeneros", conexao);
            cmd.CommandType = CommandType.StoredProcedure;
            try
            {
                conexao.Open();
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable tabela = new DataTable();
                da.Fill(tabela);
                return tabela;
            }
            catch (MySqlException e)
            {
                mensagem = "Erro: " + e.Message;
                return null;
            }
            finally
            {
                conexao.Close();
            }
        }

        public DataTable buscaGeneroPorId(int id)
        {
            MySqlCommand cmd = new MySqlCommand("sp_buscaGeneroPorId", conexao);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("id", id);
            try
            {
                conexao.Open();
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable tabela = new DataTable();
                da.Fill(tabela);
                return tabela;
            }
            catch (MySqlException e)
            {
                mensagem = "Erro: " + e.Message;
                return null;
            }
            finally
            {
                conexao.Close();
            }
        }
    }
}