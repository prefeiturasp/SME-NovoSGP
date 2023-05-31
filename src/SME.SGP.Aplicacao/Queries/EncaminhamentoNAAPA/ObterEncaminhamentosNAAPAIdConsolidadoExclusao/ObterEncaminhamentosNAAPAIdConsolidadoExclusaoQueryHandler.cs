using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interfaces;

namespace SME.SGP.Aplicacao
{
    public class ObterEncaminhamentosNAAPAIdConsolidadoExclusaoQueryHandler : ConsultasBase, IRequestHandler<ObterEncaminhamentosNAAPAIdConsolidadoExclusaoQuery, IEnumerable<long>>
    {
        public readonly IRepositorioConsolidadoEncaminhamentoNAAPA repositorioConsolidado;
        public ObterEncaminhamentosNAAPAIdConsolidadoExclusaoQueryHandler(IContextoAplicacao contextoAplicacao, IRepositorioConsolidadoEncaminhamentoNAAPA repositorioEncaminhamentoNAAPA) : base(contextoAplicacao)
        {
            this.repositorioConsolidado = repositorioEncaminhamentoNAAPA ?? throw new ArgumentNullException(nameof(repositorioEncaminhamentoNAAPA));
        }

        public async Task<IEnumerable<long>> Handle(ObterEncaminhamentosNAAPAIdConsolidadoExclusaoQuery request, CancellationToken cancellationToken)
        {
            return await repositorioConsolidado.ObterIds(request.UeId,request.AnoLetivo, request.SituacoesIgnoradas.Select(s => (int)s).ToArray());
        }
    }
}