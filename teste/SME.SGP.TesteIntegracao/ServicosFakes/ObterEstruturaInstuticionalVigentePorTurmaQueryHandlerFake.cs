using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Dto;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.ServicosFakes
{
    public class ObterEstruturaInstuticionalVigentePorTurmaQueryHandlerFake : IRequestHandler<ObterEstruturaInstuticionalVigentePorTurmaQuery, EstruturaInstitucionalRetornoEolDTO>
    {
        public async Task<EstruturaInstitucionalRetornoEolDTO> Handle(ObterEstruturaInstuticionalVigentePorTurmaQuery request, CancellationToken cancellationToken)
        {
            return await Task.FromResult((EstruturaInstitucionalRetornoEolDTO)null);
        }
    }
}
