// DENTRO DE Models/AnaliseSalva.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuiosqueBI.API.Models
{
    public class AnaliseSalva
    {
        [Key] // Isso define a propriedade 'Id' como a chave primária da tabela
        public int Id { get; set; }

        [Required] // Garante que este campo não pode ser nulo
        public string Contexto { get; set; } = string.Empty;

        public DateTime DataCriacao { get; set; }

        // Armazena o JSON completo dos resultados. jsonb é otimizado para PostgreSQL.
        [Column(TypeName = "jsonb")]
        public string ResultadosJson { get; set; } = string.Empty;
    }
}