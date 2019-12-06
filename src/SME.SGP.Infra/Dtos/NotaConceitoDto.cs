using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Infra
{
    public class NotaConceitoDto
    {
        [Required(ErrorMessage = "Deve ser informado o codigo do aluno")]
        public string AlunoId { get; set; }

        [Required(ErrorMessage = "Deve ser informado o codigo da avaliação")]
        public long AtividadeAvaliativaID { get; set; }

        public long Conceito { get; set; }
        public double Nota { get; set; }
    }
}