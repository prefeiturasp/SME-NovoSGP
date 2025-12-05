using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra.Interfaces;

namespace SME.SGP.Aplicacao
{
    public class ObterProfissionaisAtendimentoAtendimentosNAAPAIdConsolidadoExclusaoQueryHandler : ConsultasBase, IRequestHandler<ObterProfissionaisAtendimentoAtendimentosNAAPAIdConsolidadoExclusaoQuery, IEnumerable<long>>
    {
        public readonly IRepositorioConsolidadoAtendimentoNAAPA repositorioConsolidado;
        public ObterProfissionaisAtendimentoAtendimentosNAAPAIdConsolidadoExclusaoQueryHandler(IContextoAplicacao contextoAplicacao, IRepositorioConsolidadoAtendimentoNAAPA repositorioEncaminhamentoNAAPA) : base(contextoAplicacao)
        {
            this.repositorioConsolidado = repositorioEncaminhamentoNAAPA ?? throw new ArgumentNullException(nameof(repositorioEncaminhamentoNAAPA));
        }

        public async Task<IEnumerable<long>> Handle(ObterProfissionaisAtendimentoAtendimentosNAAPAIdConsolidadoExclusaoQuery request, CancellationToken cancellationToken)
        {
            return await repositorioConsolidado.ObterIds(request.UeId, request.Mes, request.AnoLetivo, request.RfsProfissionaisIgnorados);
        }
    }
}