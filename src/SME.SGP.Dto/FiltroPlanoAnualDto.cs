using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Dto
{
    public class FiltroPlanoAnualDto
    {
        [Required(ErrorMessage = "O ano deve ser informado")]
        public int Ano { get; set; }

        [Required(ErrorMessage = "O bimestre deve ser informado")]
        public int Bimestre { get; set; }

        [Required(ErrorMessage = "A escola deve ser informada")]
        public string EscolaId { get; set; }

        [Required(ErrorMessage = "A escola deve ser informada")]
        public int TurmaId { get; set; }
    }
}