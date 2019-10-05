using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Dto
{
    public class PlanoAnualDto
    {
        public PlanoAnualDto()
        { }

        public PlanoAnualDto(
            int? anoLetivo,
            IEnumerable<BimestrePlanoAnualDto> bimestres,
            string escolaId,
            long id,
            long? turmaId)
        {
            AnoLetivo = anoLetivo;
            Bimestres = bimestres;
            EscolaId = escolaId;
            Id = id;
            TurmaId = turmaId;
        }

        [Required(ErrorMessage = "O ano deve ser informado")]
        public int? AnoLetivo { get; set; }

        [ListaTemElementos(ErrorMessage = "Os bimestres devem ser informados")]
        public IEnumerable<BimestrePlanoAnualDto> Bimestres { get; set; }

        [Required(ErrorMessage = "A disciplina deve ser informada")]
        public long ComponenteCurricularEolId { get; set; }

        [Required(ErrorMessage = "A escola deve ser informada")]
        public string EscolaId { get; set; }

        public long Id { get; set; }

        [Required(ErrorMessage = "A turma deve ser informada")]
        public long? TurmaId { get; set; }
    }
}