using MediatR;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.NovoEncaminhamentoNAAPA;
using SME.SGP.Infra.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterHistoricosDeAlteracoesApresentacaoNovoEncaminhamentoNAAPAQueryHandler : ConsultasBase, IRequestHandler<ObterHistoricosDeAlteracoesApresentacaoNovoEncaminhamentoNAAPAQuery, PaginacaoResultadoDto<NovoEncaminhamentoNAAPAHistoricoDeAlteracaoDto>>
    {
        private readonly IRepositorioNovoEncaminhamentoNAAPAHistoricoAlteracoes repositorio;
        public ObterHistoricosDeAlteracoesApresentacaoNovoEncaminhamentoNAAPAQueryHandler(
                                                IContextoAplicacao contextoAplicacao,
                                                IRepositorioNovoEncaminhamentoNAAPAHistoricoAlteracoes repositorio) : base(contextoAplicacao)
        {
            this.repositorio = repositorio ?? throw new System.ArgumentNullException(nameof(repositorio));
        }

        public async Task<PaginacaoResultadoDto<NovoEncaminhamentoNAAPAHistoricoDeAlteracaoDto>> Handle(ObterHistoricosDeAlteracoesApresentacaoNovoEncaminhamentoNAAPAQuery request, CancellationToken cancellationToken)
        {
            return await this.repositorio.ListarPaginadoPorEncaminhamentoNAAPAId(request.EncaminhamentoNAAPId, Paginacao);
        }
    }
}
