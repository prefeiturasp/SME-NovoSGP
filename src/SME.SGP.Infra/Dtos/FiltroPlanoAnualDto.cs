using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Infra
{
    public class FiltroPlanoAnualDto
    {
        [Required(ErrorMessage = "O ano deve ser informado")]
        public int AnoLetivo { get; set; }

        [Required(ErrorMessage = "O bimestre deve ser informado")]
        public int Bimestre { get; set; }

        [Range(1, long.MaxValue, ErrorMessage = "O componente curricular deve ser informado")]
        public long ComponenteCurricularEolId { get; set; }

        [Required(ErrorMessage = "A escola deve ser informada")]
        public string EscolaId { get; set; }

        [Required(ErrorMessage = "A turma deve ser informada")]
        public string TurmaId { get; set; }
    }
}