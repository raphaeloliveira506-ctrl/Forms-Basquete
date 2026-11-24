using System;
using MySql.Data.MySqlClient;

namespace ProjetoBasquete.Data
{
    /// <summary>
    /// Camada de conexão com o banco de dados MySQL.
    /// Aqui ficam: nome do banco, credenciais de acesso e o método que abre a conexão.
    /// </summary>
    public class ConexaoBanco
    {
        // Altere conforme o seu ambiente MySQL
        private readonly string _stringConexao =
            "Server=localhost;Database=DB_JogadoresBasquete;Uid=root;Pwd=sua_senha;";

        /// <summary>
        /// Retorna uma instância de conexão aberta com o banco MySQL.
        /// </summary>
        public MySqlConnection ObterConexao()
        {
            var conexao = new MySqlConnection(_stringConexao);
            conexao.Open();
            return conexao;
        }
    }
}