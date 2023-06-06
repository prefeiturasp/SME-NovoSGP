using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Aplicacao;

namespace SME.SGP.TesteIntegracao.ConselhoDeClasse.ServicosFakes
{
    public class ObterPerfilAtualQueryHandlerFake : IRequestHandler<ObterPerfilAtualQuery, Guid>
    {
        public async Task<Guid> Handle(ObterPerfilAtualQuery request, CancellationToken cancellationToken)
        {
            return Guid.Parse("48e1e074-37d6-e911-abd6-f81654fe895d");
        }
    }
}