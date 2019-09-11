using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Dto
{
    public class PlanoAnualDto
    {
        [Required(ErrorMessage = "O ano deve ser informado")]
        public int? AnoLetivo { get; set; }

        [ListaTemElementos(ErrorMessage = "Os bimestres devem ser informados")]
        public IEnumerable<BimestrePlanoAnualDto> Bimestres { get; set; }

        [Required(ErrorMessage = "A escola deve ser informada")]
        public string EscolaId { get; set; }

        public long Id { get; set; }

        [Required(ErrorMessage = "A turma deve ser informada")]
        public long? TurmaId { get; set; }
    }
}