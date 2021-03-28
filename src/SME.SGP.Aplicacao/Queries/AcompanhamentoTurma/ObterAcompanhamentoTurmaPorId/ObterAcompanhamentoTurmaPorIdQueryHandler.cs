using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAcompanhamentoTurmaPorIdQueryHandler : IRequestHandler<ObterAcompanhamentoTurmaPorIdQuery, AcompanhamentoTurma>
    {
        private readonly IRepositorioAcompanhamentoTurma repositorioAcompanhamentoTurma;

        public ObterAcompanhamentoTurmaPorIdQueryHandler(IRepositorioAcompanhamentoTurma repositorioAcompanhamentoTurma)
        {
            this.repositorioAcompanhamentoTurma = repositorioAcompanhamentoTurma ?? throw new ArgumentNullException(nameof(repositorioAcompanhamentoTurma));
        }

        public async Task<AcompanhamentoTurma> Handle(ObterAcompanhamentoTurmaPorIdQuery request, CancellationToken cancellationToken)
            => await repositorioAcompanhamentoTurma.ObterPorIdAsync(request.Id);
    }

}
