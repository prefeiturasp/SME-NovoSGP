using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Dto
{
    public class AlunoDto
    {
        public string Email { get; set; }
        public long Id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório.")]
        public string Nome { get; set; }

        public long ProfessorId { get; set; }
    }
}