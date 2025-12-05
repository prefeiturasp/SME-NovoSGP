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
    public class ObterAtendimentosNAAPAConsolidadoCargaQueryHandler : ConsultasBase, IRequestHandler<ObterAtendimentosNAAPAConsolidadoCargaQuery,IEnumerable<AtendimentosNAAPAConsolidadoDto>>
    {
        public readonly IRepositorioAtendimentoNAAPA repositorioEncaminhamentoNAAPA;
        public ObterAtendimentosNAAPAConsolidadoCargaQueryHandler(IContextoAplicacao contextoAplicacao,IRepositorioAtendimentoNAAPA repositorioEncaminhamentoNAAPA) : base(contextoAplicacao)
        {
            this.repositorioEncaminhamentoNAAPA = repositorioEncaminhamentoNAAPA ?? throw new ArgumentNullException(nameof(repositorioEncaminhamentoNAAPA));
        }

        public async Task<IEnumerable<AtendimentosNAAPAConsolidadoDto>> Handle(ObterAtendimentosNAAPAConsolidadoCargaQuery request, CancellationToken cancellationToken)
        {
            return await repositorioEncaminhamentoNAAPA.ObterQuantidadeSituacaoEncaminhamentosPorUeAnoLetivo(request.UeId,request.AnoLetivo);
        }
    }
}