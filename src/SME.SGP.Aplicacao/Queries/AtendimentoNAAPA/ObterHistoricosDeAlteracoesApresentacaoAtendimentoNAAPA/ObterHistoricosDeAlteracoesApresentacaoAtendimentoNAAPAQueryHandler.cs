using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterHistoricosDeAlteracoesApresentacaoAtendimentoNAAPAQueryHandler : ConsultasBase, IRequestHandler<ObterHistoricosDeAlteracoesApresentacaoAtendimentoNAAPAQuery, PaginacaoResultadoDto<AtendimentoNAAPAHistoricoDeAlteracaoDto>>
    {
        private readonly IRepositorioAtendimentoNAAPAHistoricoAlteracoes repositorio;
        public ObterHistoricosDeAlteracoesApresentacaoAtendimentoNAAPAQueryHandler(
                                                IContextoAplicacao contextoAplicacao,
                                                IRepositorioAtendimentoNAAPAHistoricoAlteracoes repositorio) : base(contextoAplicacao)
        {
            this.repositorio = repositorio ?? throw new System.ArgumentNullException(nameof(repositorio));
        }

        public async Task<PaginacaoResultadoDto<AtendimentoNAAPAHistoricoDeAlteracaoDto>> Handle(ObterHistoricosDeAlteracoesApresentacaoAtendimentoNAAPAQuery request, CancellationToken cancellationToken)
        {
            return await this.repositorio.ListarPaginadoPorEncaminhamentoNAAPAId(request.EncaminhamentoNAAPId, Paginacao);
        }
    }
}
