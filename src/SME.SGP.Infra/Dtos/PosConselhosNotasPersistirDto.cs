using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Infra
{
    public class PosConselhosNotasPersistirDto
    {
        [Required(ErrorMessage = "É necessário informar o Aluno.")]
        public string AlunoCodigo { get; set; }

        [Required(ErrorMessage = "É necessário informar o Bimestre.")]
        public int Bimestre { get; set; }

        [Required(ErrorMessage = "É necessário informar o Componente curricular.")]
        public string ComponenteCurricularCodigo { get; set; }

        public int? ConceitoCodigo { get; set; }

        [Required(ErrorMessage = "É necessário informar a Justificativa.")]
        public string Justificativa { get; set; }

        public decimal? Nota { get; set; }

        [Required(ErrorMessage = "É necessário informar a Turma.")]
        public string TurmaCodigo { get; set; }
    }
}