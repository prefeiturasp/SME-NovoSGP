using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAtendimentoProfissionalEncaminhamentosNAAPAConsolidadoQueryHandler : ConsultasBase, IRequestHandler<ObterAtendimentoProfissionalEncaminhamentosNAAPAConsolidadoQuery, ConsolidadoAtendimentoNAAPA>
    {
        public readonly IRepositorioConsolidadoAtendimentoNAAPA repositorioConsolidado;
        public ObterAtendimentoProfissionalEncaminhamentosNAAPAConsolidadoQueryHandler(IContextoAplicacao contextoAplicacao, IRepositorioConsolidadoAtendimentoNAAPA repositorioEncaminhamentoNAAPA) : base(contextoAplicacao)
        {
            this.repositorioConsolidado = repositorioEncaminhamentoNAAPA ?? throw new ArgumentNullException(nameof(repositorioEncaminhamentoNAAPA));
        }

        public async Task<ConsolidadoAtendimentoNAAPA> Handle(ObterAtendimentoProfissionalEncaminhamentosNAAPAConsolidadoQuery request, CancellationToken cancellationToken)
        {
            return await repositorioConsolidado.ObterPorUeIdMesAnoLetivoProfissional(request.UeId, request.Mes, request.AnoLetivo, request.RfProfissional, (int)request.Modalidade);
        }
    }
}