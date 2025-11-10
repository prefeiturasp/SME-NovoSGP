using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class FiltroListaAulaIdComponenteCurricularIdDto
    {
        public IEnumerable<PendenciaDiarioBordoParaExcluirDto> PendenciaDiariosBordoParaExcluirDto { get; set; }

        public FiltroListaAulaIdComponenteCurricularIdDto(IEnumerable<PendenciaDiarioBordoParaExcluirDto> pendenciaDiariosBordoParaExcluirDto)
        {
            PendenciaDiariosBordoParaExcluirDto = pendenciaDiariosBordoParaExcluirDto;
        }
    }
}
