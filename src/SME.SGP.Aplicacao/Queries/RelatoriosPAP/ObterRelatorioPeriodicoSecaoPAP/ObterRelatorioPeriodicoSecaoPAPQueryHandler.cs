using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterRelatorioPeriodicoSecaoPAPQueryHandler : IRequestHandler<ObterRelatorioPeriodicoSecaoPAPQuery, RelatorioPeriodicoPAPSecao>
    {
        private readonly IRepositorioRelatorioPeriodicoPAPSecao repositorio;

        public ObterRelatorioPeriodicoSecaoPAPQueryHandler(IRepositorioRelatorioPeriodicoPAPSecao repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public Task<RelatorioPeriodicoPAPSecao> Handle(ObterRelatorioPeriodicoSecaoPAPQuery request, CancellationToken cancellationToken)
        {
            return repositorio.ObterSecoesComQuestoes(request.RelatorioSecaoId);
        }
    }
}
