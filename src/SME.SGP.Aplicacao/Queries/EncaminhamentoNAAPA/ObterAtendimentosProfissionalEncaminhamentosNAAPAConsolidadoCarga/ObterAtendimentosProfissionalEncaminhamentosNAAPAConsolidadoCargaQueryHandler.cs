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
    public class ObterAtendimentosProfissionalEncaminhamentosNAAPAConsolidadoCargaQueryHandler : ConsultasBase, IRequestHandler<ObterAtendimentosProfissionalEncaminhamentosNAAPAConsolidadoCargaQuery, IEnumerable<AtendimentosProfissionalAtendimentoNAAPAConsolidadoDto>>
    {
        public readonly IRepositorioSecaoEncaminhamentoNAAPA repositorioEncaminhamentoNAAPA;
        public ObterAtendimentosProfissionalEncaminhamentosNAAPAConsolidadoCargaQueryHandler(IContextoAplicacao contextoAplicacao, IRepositorioSecaoEncaminhamentoNAAPA repositorioEncaminhamentoNAAPA) : base(contextoAplicacao)
        {
            this.repositorioEncaminhamentoNAAPA = repositorioEncaminhamentoNAAPA ?? throw new ArgumentNullException(nameof(repositorioEncaminhamentoNAAPA));
        }

        public async Task<IEnumerable<AtendimentosProfissionalAtendimentoNAAPAConsolidadoDto>> Handle(ObterAtendimentosProfissionalEncaminhamentosNAAPAConsolidadoCargaQuery request, CancellationToken cancellationToken)
        {
            return await repositorioEncaminhamentoNAAPA.ObterQuantidadeAtendimentosProfissionalPorUeAnoLetivoMes(request.UeId, request.Mes, request.AnoLetivo);
        }
    }
}