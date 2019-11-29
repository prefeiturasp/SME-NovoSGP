using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Infra
{
    public class FiltroAtribuicaoEsporadicaDto
    {
        [Range(0, double.MaxValue, ErrorMessage = "É necessario informar o ano letivo")]
        public int AnoLetivo { get; set; }

        public string CodigoRF { get; set; }

        [Required(ErrorMessage = "É necessario informar o Id da DRE")]
        public string DreId { get; set; }

        public string ProfessorRF { get; set; }

        [Required(ErrorMessage = "É necessario informar o Id da UE")]
        public string UeId { get; set; }
    }
}