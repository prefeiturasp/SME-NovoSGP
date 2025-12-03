using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interfaces;

namespace SME.SGP.Aplicacao
{
    public class ObterEncaminhamentosNAAPAConsolidadoCargaQueryHandler : ConsultasBase, IRequestHandler<ObterEncaminhamentosNAAPAConsolidadoCargaQuery,IEnumerable<AtendimentosNAAPAConsolidadoDto>>
    {
        public readonly IRepositorioAtendimentoNAAPA repositorioEncaminhamentoNAAPA;
        public ObterEncaminhamentosNAAPAConsolidadoCargaQueryHandler(IContextoAplicacao contextoAplicacao,IRepositorioAtendimentoNAAPA repositorioEncaminhamentoNAAPA) : base(contextoAplicacao)
        {
            this.repositorioEncaminhamentoNAAPA = repositorioEncaminhamentoNAAPA ?? throw new ArgumentNullException(nameof(repositorioEncaminhamentoNAAPA));
        }

        public async Task<IEnumerable<AtendimentosNAAPAConsolidadoDto>> Handle(ObterEncaminhamentosNAAPAConsolidadoCargaQuery request, CancellationToken cancellationToken)
        {
            return await repositorioEncaminhamentoNAAPA.ObterQuantidadeSituacaoEncaminhamentosPorUeAnoLetivo(request.UeId,request.AnoLetivo);
        }
    }
}