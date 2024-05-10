using MediatR;
using SME.SGP.Aplicacao;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.Ocorrencia.ServicosFakes
{
    public class ObterPerfilAtualQueryHandlerFake : IRequestHandler<ObterPerfilAtualQuery, Guid>
    {
        public async Task<Guid> Handle(ObterPerfilAtualQuery request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(Guid.Parse("48e1e074-37d6-e911-abd6-f81654fe895d"));
        }
    }
}
