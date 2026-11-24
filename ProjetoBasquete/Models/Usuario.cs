using System;

namespace ProjetoBasquete.Models
{
    /// <summary>
    /// Classe modelo que representa um usuário do sistema (login).
    /// </summary>
    public class Usuario
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Senha { get; set; }
        public string NomeCompleto { get; set; }
        public bool Ativo { get; set; }

        public Usuario()
        {
            Login = string.Empty;
            Senha = string.Empty;
            NomeCompleto = string.Empty;
            Ativo = true;
        }
    }
}