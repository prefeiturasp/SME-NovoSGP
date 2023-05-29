using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterCartaIntencoesObservacaoPorObservacaoIdQueryHandler : IRequestHandler<ObterCartaIntencoesObservacaoPorObservacaoIdQuery, CartaIntencoesObservacaoDto>
    {
        private readonly IRepositorioCartaIntencoesObservacao repositorioCartaIntencoesObservacao;

        public ObterCartaIntencoesObservacaoPorObservacaoIdQueryHandler(IRepositorioCartaIntencoesObservacao repositorioCartaIntencoesObservacao)
        {
            this.repositorioCartaIntencoesObservacao = repositorioCartaIntencoesObservacao;
        }

        public async Task<CartaIntencoesObservacaoDto> Handle(ObterCartaIntencoesObservacaoPorObservacaoIdQuery request, CancellationToken cancellationToken)
            => await repositorioCartaIntencoesObservacao.ObterCartaIntencoesObservacaoPorObservacaoId(request.ObservacaoId);
    }
}
