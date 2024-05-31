using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterIdsNotificacaoPorNAAPAIdQueryHandler : IRequestHandler<ObterIdsNotificacaoPorNAAPAIdQuery, IEnumerable<long>>
    {
        private readonly IRepositorioInatividadeAtendimentoNAAPANotificacao repositorio;

        public ObterIdsNotificacaoPorNAAPAIdQueryHandler(IRepositorioInatividadeAtendimentoNAAPANotificacao repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public Task<IEnumerable<long>> Handle(ObterIdsNotificacaoPorNAAPAIdQuery request, CancellationToken cancellationToken)
        {
            return repositorio.ObterIdsNotificacoesPorNAAPAIdAsync(request.EncaminhamentoNAAPAId);
        }
    }
}
