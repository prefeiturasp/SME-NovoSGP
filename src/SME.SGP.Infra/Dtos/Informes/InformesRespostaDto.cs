using System.Collections.Generic;

namespace SME.SGP.Infra.Dtos
{
    public class InformesRespostaDto
    {
        public InformesRespostaDto()
        {
            Anexos = new List<ArquivoResumidoDto>();
        }
        public long Id { get; set; }
        public long? DreId { get; set; }
        public string DreNome { get; set; }
        public long? UeId { get; set; }
        public string UeNome { get; set; }
        public IEnumerable<GruposDeUsuariosDto> Perfis { get; set; }
        public string Titulo { get; set; }
        public string Texto { get; set; }
        public AuditoriaDto Auditoria { get; set; }
        public List<ArquivoResumidoDto> Anexos { get; set; } 
    }
}
