using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterRespostaPorSecaoPAPQueryHandler : IRequestHandler<ObterRespostaPorSecaoPAPQuery, IEnumerable<RelatorioPeriodicoPAPResposta>>
    {
        private readonly IRepositorioRelatorioPeriodicoPAPResposta repositorio;

        public ObterRespostaPorSecaoPAPQueryHandler(IRepositorioRelatorioPeriodicoPAPResposta repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }
        public Task<IEnumerable<RelatorioPeriodicoPAPResposta>> Handle(ObterRespostaPorSecaoPAPQuery request, CancellationToken cancellationToken)
        {
            return repositorio.ObterRespostas(request.SecaoId);
        }
    }
}
