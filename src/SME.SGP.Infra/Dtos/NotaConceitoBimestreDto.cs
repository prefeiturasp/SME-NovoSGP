using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Infra
{
    public class NotaConceitoBimestreDto
    {
        [Required(ErrorMessage = "Necessário informar o codigo do aluno")]
        public string CodigoAluno { get; set; }

        public long? ConceitoId { get; set; }

        [Required(ErrorMessage = "Necessário informar a disciplina para atribuição da nota/conceito ao aluno")]
        public long DisciplinaId { get; set; }

        public double? Nota { get; set; }
    }
}