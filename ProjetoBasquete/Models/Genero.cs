namespace ProjetoBasquete.Models
{
    /// <summary>
    /// Classe modelo que representa um gênero.
    /// </summary>
    public class Genero
    {
        public int Id { get; set; }
        public string Nome { get; set; }

        public Genero()
        {
            Nome = string.Empty;
        }
    }
}