using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Infra
{
    public class PlanoAnualTerritorioSaberDto
    {
        public PlanoAnualTerritorioSaberDto()
        { }

        public PlanoAnualTerritorioSaberDto(
            int? anoLetivo,
            IEnumerable<BimestrePlanoAnualTerritorioSaberDto> bimestres,
            string escolaId,
            long id,
            long? turmaId,
            long territorioExperienciaId)
        {
            AnoLetivo = anoLetivo;
            Bimestres = bimestres;
            EscolaId = escolaId;
            Id = id;
            TurmaId = turmaId;
            TerritorioExperienciaId = territorioExperienciaId;
        }

        [Required(ErrorMessage = "O ano deve ser informado")]
        public int? AnoLetivo { get; set; }

        [ListaTemElementos(ErrorMessage = "Os bimestres devem ser informados")]
        public IEnumerable<BimestrePlanoAnualTerritorioSaberDto> Bimestres { get; set; }

        [Range(1, long.MaxValue, ErrorMessage = "O Território/Experiência Pedagógica deve ser informada")]
        public long TerritorioExperienciaId { get; set; }

        [Required(ErrorMessage = "A escola deve ser informada")]
        public string EscolaId { get; set; }

        public long Id { get; set; }

        [Required(ErrorMessage = "A turma deve ser informada")]
        public long? TurmaId { get; set; }
    }
}
