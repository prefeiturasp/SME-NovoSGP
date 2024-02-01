using System.Collections.Generic;

namespace SME.SGP.Infra.Dtos
{
    public class InformesDto
    {
        public long Id { get; set; }
        public long? DreId { get; set; }
        public long? UeId { get; set; }
        public IEnumerable<GruposDeUsuariosDto> Perfis { get; set; }
        public string Titulo { get; set; }
        public string Texto { get; set; }
    }
}
