using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;

namespace SME.SGP.Aplicacao
{
    public class ObterCompensacaoAusenciaPorIdQueryHandler : IRequestHandler<ObterCompensacaoAusenciaPorIdQuery,CompensacaoAusencia>
    {
        private readonly IRepositorioCompensacaoAusencia repositorioCompensacaoAusencia;

        public ObterCompensacaoAusenciaPorIdQueryHandler(IRepositorioCompensacaoAusencia compensacaoAusencia)
        {
            repositorioCompensacaoAusencia = compensacaoAusencia ?? throw new ArgumentNullException(nameof(compensacaoAusencia));
        }

        public async Task<CompensacaoAusencia> Handle(ObterCompensacaoAusenciaPorIdQuery request, CancellationToken cancellationToken)
        {
            return await repositorioCompensacaoAusencia.ObterPorIdAsync(request.CompensacaoAusenciaId);
        }
    }
}