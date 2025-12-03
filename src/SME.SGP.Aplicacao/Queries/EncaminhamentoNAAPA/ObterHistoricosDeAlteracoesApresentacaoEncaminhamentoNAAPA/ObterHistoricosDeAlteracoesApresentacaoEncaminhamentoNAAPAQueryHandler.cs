using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterHistoricosDeAlteracoesApresentacaoEncaminhamentoNAAPAQueryHandler : ConsultasBase, IRequestHandler<ObterHistoricosDeAlteracoesApresentacaoEncaminhamentoNAAPAQuery, PaginacaoResultadoDto<AtendimentoNAAPAHistoricoDeAlteracaoDto>>
    {
        private readonly IRepositorioEncaminhamentoNAAPAHistoricoAlteracoes repositorio;
        public ObterHistoricosDeAlteracoesApresentacaoEncaminhamentoNAAPAQueryHandler(
                                                IContextoAplicacao contextoAplicacao,
                                                IRepositorioEncaminhamentoNAAPAHistoricoAlteracoes repositorio) : base(contextoAplicacao)
        {
            this.repositorio = repositorio ?? throw new System.ArgumentNullException(nameof(repositorio));
        }

        public async Task<PaginacaoResultadoDto<AtendimentoNAAPAHistoricoDeAlteracaoDto>> Handle(ObterHistoricosDeAlteracoesApresentacaoEncaminhamentoNAAPAQuery request, CancellationToken cancellationToken)
        {
            return await this.repositorio.ListarPaginadoPorEncaminhamentoNAAPAId(request.EncaminhamentoNAAPId, Paginacao);
        }
    }
}
