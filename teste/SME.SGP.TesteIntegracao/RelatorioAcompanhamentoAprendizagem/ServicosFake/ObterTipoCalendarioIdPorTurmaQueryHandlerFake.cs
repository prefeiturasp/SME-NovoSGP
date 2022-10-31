using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Aplicacao;

namespace SME.SGP.TesteIntegracao.RelatorioAcompanhamentoAprendizagem.ServicosFake
{
    public class ObterTipoCalendarioIdPorTurmaQueryHandlerFake : IRequestHandler<ObterTipoCalendarioIdPorTurmaQuery, long>
    {
        public async Task<long> Handle(ObterTipoCalendarioIdPorTurmaQuery request, CancellationToken cancellationToken)
        {
            return 1;
        }
    }
}