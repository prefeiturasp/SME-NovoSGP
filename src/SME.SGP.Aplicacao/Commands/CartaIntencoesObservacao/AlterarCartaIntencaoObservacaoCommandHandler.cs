using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AlterarCartaIntencaoObservacaoCommandHandler : IRequestHandler<AlterarCartaIntencaoObservacaoCommand, AuditoriaDto>
    {
        private readonly IRepositorioCartaIntencoesObservacao repositorioCartaIntencoesObservacao;

        public AlterarCartaIntencaoObservacaoCommandHandler(IRepositorioCartaIntencoesObservacao repositorioCartaIntencoesObservacao)
        {
            this.repositorioCartaIntencoesObservacao = repositorioCartaIntencoesObservacao ?? throw new System.ArgumentNullException(nameof(repositorioCartaIntencoesObservacao));
        }

        public async Task<AuditoriaDto> Handle(AlterarCartaIntencaoObservacaoCommand request, CancellationToken cancellationToken)
        {
            var cartaIntencoesObservacao = await repositorioCartaIntencoesObservacao.ObterPorIdAsync(request.ObservacaoId);
            if (cartaIntencoesObservacao == null)
                throw new NegocioException("Observação da carta de intenção não encontrada.");

            cartaIntencoesObservacao.ValidarUsuarioAlteracao(request.UsuarioId);

            cartaIntencoesObservacao.Observacao = request.Observacao;

            await repositorioCartaIntencoesObservacao.SalvarAsync(cartaIntencoesObservacao);
            return (AuditoriaDto)cartaIntencoesObservacao;
        }

    }
}
