using System;

namespace ProjetoBasquete.Models
{
    /// <summary>
    /// Classe modelo que representa um jogador de basquete.
    /// </summary>
    public class Jogador
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public decimal Altura { get; set; }
        public string Posicao { get; set; }
        public int FkPosicao { get; set; }
        public int NumeroCamisa { get; set; }
        public string Time { get; set; }
        public string Nacionalidade { get; set; }
        public bool Preferido { get; set; }
        public DateTime DataCadastro { get; set; }

        public Jogador()
        {
            Nome = string.Empty;
            Posicao = string.Empty;
            Time = string.Empty;
            Nacionalidade = string.Empty;
            DataCadastro = DateTime.Now;
            Preferido = false;
        }
    }
}