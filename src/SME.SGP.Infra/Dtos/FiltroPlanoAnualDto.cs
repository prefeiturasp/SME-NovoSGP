using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Infra
{
    public class FiltroPlanoAnualDto
    {
        [Required(ErrorMessage = "O ano deve ser informado")]
        public int AnoLetivo { get; set; }

        [Required(ErrorMessage = "O bimestre deve ser informado")]
        public int Bimestre { get; set; }

        [Required(ErrorMessage = "A escola deve ser informada")]
        public string EscolaId { get; set; }

        [Required(ErrorMessage = "A turma deve ser informada")]
        public int TurmaId { get; set; }
    }
}