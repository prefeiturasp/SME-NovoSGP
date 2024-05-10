using MediatR;
using SME.SGP.Aplicacao;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.Aula.DiarioBordo.ServicosFakes
{
    public class SolicitaRelatorioDevolutivasCommandFake : IRequestHandler<SolicitaRelatorioDevolutivasCommand, Guid>
    {
        public Task<Guid> Handle(SolicitaRelatorioDevolutivasCommand request, CancellationToken cancellationToken)
        {
            return Task.FromResult(Guid.NewGuid());
        }
    }
}
