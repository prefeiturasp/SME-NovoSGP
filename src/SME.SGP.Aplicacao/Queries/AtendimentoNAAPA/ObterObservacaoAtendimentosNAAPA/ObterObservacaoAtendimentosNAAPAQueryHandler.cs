using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterObservacaoAtendimentosNAAPAQueryHandler : ConsultasBase, IRequestHandler<ObterObservacaoAtendimentosNAAPAQuery, PaginacaoResultadoDto<AtendimentoNAAPAObservacoesDto>>
    {
        private readonly IRepositorioObservacaoAtendimentoNAAPA repositorioObs;
        public ObterObservacaoAtendimentosNAAPAQueryHandler(IContextoAplicacao contextoAplicacao,
            IRepositorioObservacaoAtendimentoNAAPA repositorio) : base(contextoAplicacao)
        {
            this.repositorioObs = repositorio ?? throw new System.ArgumentNullException(nameof(repositorio));
        }

        public async Task<PaginacaoResultadoDto<AtendimentoNAAPAObservacoesDto>> Handle(ObterObservacaoAtendimentosNAAPAQuery request, CancellationToken cancellationToken)
        {
            return await repositorioObs.ListarPaginadoPorEncaminhamentoNAAPAId(request.EncaminhamentoNAAPId,request.UsuarioLogadoRf,Paginacao);
        }
    }
}
