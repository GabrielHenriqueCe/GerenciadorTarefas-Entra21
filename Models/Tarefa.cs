using System.ComponentModel.DataAnnotations;

namespace GerenciadorTarefas.MVC.Models
{
    public class Tarefa
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O título é obrigatório.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "O título deve ter entre 3 e 100 caracteres.")]
        public string Titulo { get; set; } = string.Empty;

        [Required(ErrorMessage = "A descrição é obrigatória.")]
        [StringLength(500, ErrorMessage = "A descrição deve ter no máximo 500 caracteres.")]
        public string Descricao { get; set; } = string.Empty;

        [Required(ErrorMessage = "A data é obrigatória.")]
        [DataType(DataType.Date)]
        public DateTime? Data { get; set; }

        public bool Concluida { get; set; }

        public int UsuarioId { get; set; }
    }
}