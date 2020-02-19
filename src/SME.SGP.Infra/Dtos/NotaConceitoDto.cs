using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Infra
{
    public class NotaConceitoDto
    {
        [Required(ErrorMessage = "Deve ser informado o código do aluno")]
        public string AlunoId { get; set; }

        [Required(ErrorMessage = "Deve ser informado o código da avaliação")]
        public long AtividadeAvaliativaId { get; set; }

        public long? Conceito { get; set; }
        public double? Nota { get; set; }
    }
}