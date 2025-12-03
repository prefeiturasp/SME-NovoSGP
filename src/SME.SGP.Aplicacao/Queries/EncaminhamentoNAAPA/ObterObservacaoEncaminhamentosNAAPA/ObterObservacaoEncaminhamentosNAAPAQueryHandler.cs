using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterObservacaoEncaminhamentosNAAPAQueryHandler : ConsultasBase, IRequestHandler<ObterObservacaoEncaminhamentosNAAPAQuery, PaginacaoResultadoDto<AtendimentoNAAPAObservacoesDto>>
    {
        private readonly IRepositorioObservacaoEncaminhamentoNAAPA repositorioObs;
        public ObterObservacaoEncaminhamentosNAAPAQueryHandler(IContextoAplicacao contextoAplicacao,
            IRepositorioObservacaoEncaminhamentoNAAPA repositorio) : base(contextoAplicacao)
        {
            this.repositorioObs = repositorio ?? throw new System.ArgumentNullException(nameof(repositorio));
        }

        public async Task<PaginacaoResultadoDto<AtendimentoNAAPAObservacoesDto>> Handle(ObterObservacaoEncaminhamentosNAAPAQuery request, CancellationToken cancellationToken)
        {
            return await repositorioObs.ListarPaginadoPorEncaminhamentoNAAPAId(request.EncaminhamentoNAAPId,request.UsuarioLogadoRf,Paginacao);
        }
    }
}
