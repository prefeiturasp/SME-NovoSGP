using SME.SGP.Dto;

namespace SME.SGP.Infra
{
    public class ComunicadoResultadoDto : ComunicadoCompletoDto
    {
        public string Grupo { get; set; }
        public int GrupoId { get; set; }
    }
}