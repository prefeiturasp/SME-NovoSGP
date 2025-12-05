using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAtendimentosNAAPAIdConsolidadoExclusaoQueryHandler : ConsultasBase, IRequestHandler<ObterAtendimentosNAAPAIdConsolidadoExclusaoQuery, IEnumerable<long>>
    {
        public readonly IRepositorioConsolidadoEncaminhamentoNAAPA repositorioConsolidado;
        public ObterAtendimentosNAAPAIdConsolidadoExclusaoQueryHandler(IContextoAplicacao contextoAplicacao, IRepositorioConsolidadoEncaminhamentoNAAPA repositorioEncaminhamentoNAAPA) : base(contextoAplicacao)
        {
            this.repositorioConsolidado = repositorioEncaminhamentoNAAPA ?? throw new ArgumentNullException(nameof(repositorioEncaminhamentoNAAPA));
        }

        public async Task<IEnumerable<long>> Handle(ObterAtendimentosNAAPAIdConsolidadoExclusaoQuery request, CancellationToken cancellationToken)
        {
            return await repositorioConsolidado.ObterIds(request.UeId,request.AnoLetivo, request.SituacoesIgnoradas.Select(s => (int)s).ToArray());
        }
    }
}