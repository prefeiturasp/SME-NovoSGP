using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Infra
{
    public class FechamentoFinalSalvarItemDto
    {
        [Required(ErrorMessage = "É necessário informar o código RF do aluno.")]
        public string AlunoRf { get; set; }

        [Required(ErrorMessage = "É necessário informar o componente curricular.")]
        public long ComponenteCurricularCodigo { get; set; }

        public long? ConceitoId { get; set; }
        public double? Nota { get; set; }
        public long? SinteseId { get; set; }

        public bool EhNota()
        {
            return !ConceitoId.HasValue;
        }
    }
}