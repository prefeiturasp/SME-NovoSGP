using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarObservacaoCartaIntencoesCommandHandler : IRequestHandler<SalvarObservacaoCartaIntencoesCommand, AuditoriaDto>
    {
        private readonly IRepositorioCartaIntencoesObservacao repositorioCartaIntencoesObservacao;

        public SalvarObservacaoCartaIntencoesCommandHandler(IRepositorioCartaIntencoesObservacao repositorioCartaIntencoesObservacao)
        {
            this.repositorioCartaIntencoesObservacao = repositorioCartaIntencoesObservacao ?? throw new System.ArgumentNullException(nameof(repositorioCartaIntencoesObservacao));
        }

        public async Task<AuditoriaDto> Handle(SalvarObservacaoCartaIntencoesCommand request, CancellationToken cancellationToken)
        {
            var cartaIntencoesObservacao = new CartaIntencoesObservacao(request.Observacao, request.CartaIntencoesId, 0, request.UsuarioId);
            await repositorioCartaIntencoesObservacao.SalvarAsync(cartaIntencoesObservacao);
            return (AuditoriaDto)cartaIntencoesObservacao;
        }
    }
}
