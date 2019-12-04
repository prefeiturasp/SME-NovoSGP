using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Infra
{
    public class NotaConceitoDto
    {
        [Required(ErrorMessage = "Deve ser informado o codigo do Aluno")]
        public string AlunoId { get; set; }

        [Required(ErrorMessage = "Deve ser informado o codigo da atividade avaliativa")]
        public long AtividadeAvaliativaID { get; set; }

        public long Conceito { get; set; }
        public double Nota { get; set; }
    }
}