using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using ProjetoBasquete.Models;

namespace ProjetoBasquete.Data
{
    /// <summary>
    /// Camada de acesso a dados (DAL) para MySQL.
    /// Recebe os dados mapeados da classe Jogador e executa as stored procedures
    /// no banco de dados (Insert, Update, Delete, Select).
    /// </summary>
    public class JogadorDAL
    {
        private readonly ConexaoBanco _conexaoBanco = new ConexaoBanco();

        /// <summary>
        /// Mensagem de erro ou status da última operação.
        /// </summary>
        public string mensagem { get; private set; } = string.Empty;

        // ==================== MÉTODOS COM List<Jogador> (mantidos) ====================

        /// <summary>
        /// Insere um novo jogador utilizando a procedure sp_InserirJogador.
        /// </summary>
        public int Inserir(Jogador jogador)
        {
            using var conexao = _conexaoBanco.ObterConexao();
            using var cmd = new MySqlCommand("sp_InserirJogador", conexao);
            cmd.CommandType = CommandType.StoredProcedure;

            MapearParametros(cmd, jogador);

            var resultado = cmd.ExecuteScalar();
            return resultado != null ? Convert.ToInt32(resultado) : 0;
        }

        /// <summary>
        /// Atualiza os dados de um jogador existente via sp_AtualizarJogador.
        /// </summary>
        public void Atualizar(Jogador jogador)
        {
            using var conexao = _conexaoBanco.ObterConexao();
            using var cmd = new MySqlCommand("sp_AtualizarJogador", conexao);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@p_Id", jogador.Id);
            MapearParametros(cmd, jogador);

            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Exclui um jogador pelo ID via sp_DeletarJogador.
        /// </summary>
        public void Deletar(int id)
        {
            using var conexao = _conexaoBanco.ObterConexao();
            using var cmd = new MySqlCommand("sp_DeletarJogador", conexao);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@p_Id", id);
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Lista todos os jogadores cadastrados via sp_ListarJogadores.
        /// </summary>
        public List<Jogador> Listar()
        {
            var lista = new List<Jogador>();

            using var conexao = _conexaoBanco.ObterConexao();
            using var cmd = new MySqlCommand("sp_ListarJogadores", conexao);
            cmd.CommandType = CommandType.StoredProcedure;

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                lista.Add(MapearJogador(reader));
            }

            return lista;
        }

        /// <summary>
        /// Busca um jogador específico pelo ID via sp_BuscarJogadorPorId.
        /// </summary>
        public Jogador BuscarPorId(int id)
        {
            using var conexao = _conexaoBanco.ObterConexao();
            using var cmd = new MySqlCommand("sp_BuscarJogadorPorId", conexao);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@p_Id", id);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return MapearJogador(reader);
            }

            return null;
        }

        /// <summary>
        /// Alterna o status de jogador preferido via sp_AlternarPreferido.
        /// </summary>
        public void AlternarPreferido(int id, bool preferido)
        {
            using var conexao = _conexaoBanco.ObterConexao();
            using var cmd = new MySqlCommand("sp_AlternarPreferido", conexao);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@p_Id", id);
            cmd.Parameters.AddWithValue("@p_Preferido", preferido ? 1 : 0);

            cmd.ExecuteNonQuery();
        }

        // ==================== MÉTODOS COM DataTable (segunda parte do trabalho) ====================

        /// <summary>
        /// Lista todas as posições utilizando a procedure sp_listaPosicoes.
        /// Retorna um DataTable (análogo a listaGeneros).
        /// </summary>
        public DataTable ListaPosicoes()
        {
            using var conexao = _conexaoBanco.ObterConexao();
            using var cmd = new MySqlCommand("sp_listaPosicoes", conexao);
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

        /// <summary>
        /// Lista jogadores com INNER JOIN nas posições utilizando a procedure sp_listaJogadoresDetalhado.
        /// Retorna um DataTable (análogo a listaBandas).
        /// </summary>
        public DataTable ListaJogadoresDetalhado()
        {
            using var conexao = _conexaoBanco.ObterConexao();
            using var cmd = new MySqlCommand("sp_listaJogadoresDetalhado", conexao);
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

        /// <summary>
        /// Deleta um jogador pelo ID utilizando a procedure sp_removeJogador.
        /// Retorna true se sucesso, false se erro (análogo a deletaBanda).
        /// </summary>
        public bool DeletarJogador(int idRemoveJogador)
        {
            using var conexao = _conexaoBanco.ObterConexao();
            using var cmd = new MySqlCommand("sp_removeJogador", conexao);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@p_Id", idRemoveJogador);

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
        }

        /// <summary>
        /// Mapeia as propriedades do objeto Jogador para os parâmetros da stored procedure MySQL.
        /// </summary>
        private static void MapearParametros(MySqlCommand cmd, Jogador jogador)
        {
            cmd.Parameters.AddWithValue("@p_Nome", string.IsNullOrWhiteSpace(jogador.Nome) ? (object)DBNull.Value : jogador.Nome);
            cmd.Parameters.AddWithValue("@p_Altura", jogador.Altura);
            cmd.Parameters.AddWithValue("@p_fk_posicao", jogador.FkPosicao);
            cmd.Parameters.AddWithValue("@p_NumeroCamisa", jogador.NumeroCamisa);
            cmd.Parameters.AddWithValue("@p_Time", string.IsNullOrWhiteSpace(jogador.Time) ? (object)DBNull.Value : jogador.Time);
            cmd.Parameters.AddWithValue("@p_Nacionalidade", string.IsNullOrWhiteSpace(jogador.Nacionalidade) ? (object)DBNull.Value : jogador.Nacionalidade);
        }

        /// <summary>
        /// Mapeia os dados retornados do banco (MySqlDataReader) para a classe Jogador.
        /// </summary>
        private static Jogador MapearJogador(MySqlDataReader reader)
        {
            return new Jogador
            {
                Id = Convert.ToInt32(reader["Id"]),
                Nome = reader["Nome"]?.ToString(),
                Altura = reader["Altura"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["Altura"]),
                Posicao = reader["Posicao"]?.ToString(),
                FkPosicao = reader["fk_posicao"] == DBNull.Value ? 0 : Convert.ToInt32(reader["fk_posicao"]),
                NumeroCamisa = reader["NumeroCamisa"] == DBNull.Value ? 0 : Convert.ToInt32(reader["NumeroCamisa"]),
                Time = reader["Time"]?.ToString(),
                Nacionalidade = reader["Nacionalidade"]?.ToString(),
                Preferido = reader["Preferido"] == DBNull.Value ? false : Convert.ToBoolean(reader["Preferido"]),
                DataCadastro = reader["DataCadastro"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(reader["DataCadastro"])
            };
        }
    }
}