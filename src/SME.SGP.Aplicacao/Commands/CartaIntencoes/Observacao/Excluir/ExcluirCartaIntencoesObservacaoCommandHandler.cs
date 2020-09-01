using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirCartaIntencoesObservacaoCommandHandler : IRequestHandler<ExcluirCartaIntencoesObservacaoCommand, bool>
    {
        private readonly IRepositorioCartaIntencoesObservacao repositorioCartaIntencoesObservacao;

        public ExcluirCartaIntencoesObservacaoCommandHandler(IRepositorioCartaIntencoesObservacao repositorioCartaIntencoesObservacao)
        {
            this.repositorioCartaIntencoesObservacao = repositorioCartaIntencoesObservacao ?? throw new System.ArgumentNullException(nameof(repositorioCartaIntencoesObservacao));
        }

        public async Task<bool> Handle(ExcluirCartaIntencoesObservacaoCommand request, CancellationToken cancellationToken)
        {
            var cartaIntencoesObservacao = await repositorioCartaIntencoesObservacao.ObterPorIdAsync(request.ObservacaoId);
            if (cartaIntencoesObservacao == null)
                throw new NegocioException("Observação da carta de intenções não encontrada.");

            cartaIntencoesObservacao.ValidarUsuarioAlteracao(request.UsuarioId);
            cartaIntencoesObservacao.Remover();

            await repositorioCartaIntencoesObservacao.SalvarAsync(cartaIntencoesObservacao);
            return true;
        }
    }
}
