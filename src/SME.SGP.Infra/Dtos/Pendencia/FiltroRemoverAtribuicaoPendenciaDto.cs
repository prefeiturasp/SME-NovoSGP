using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class FiltroRemoverAtribuicaoPendenciaDto
    {
        public FiltroRemoverAtribuicaoPendenciaDto(long ueId, IEnumerable<PendenciaPerfilUsuarioDto> pendenciasFuncionarios)
        {
            UeId = ueId;
            PendenciasFuncionarios = pendenciasFuncionarios;
        }

        public IEnumerable<PendenciaPerfilUsuarioDto> PendenciasFuncionarios { get; set; }
        public long UeId { get; set; }
    }
}
