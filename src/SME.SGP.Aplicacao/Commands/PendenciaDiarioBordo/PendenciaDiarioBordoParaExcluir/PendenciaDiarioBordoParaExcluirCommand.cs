using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class PendenciaDiarioBordoParaExcluirCommand : IRequest<bool>
    {
        public IEnumerable<PendenciaDiarioBordoParaExcluirDto> PendenciaDiariosBordoParaExcluirDto { get; set; }

        public PendenciaDiarioBordoParaExcluirCommand(IEnumerable<PendenciaDiarioBordoParaExcluirDto> pendenciaDiariosBordoParaExcluirDto)
        {
            PendenciaDiariosBordoParaExcluirDto = pendenciaDiariosBordoParaExcluirDto;
        }
    }
}