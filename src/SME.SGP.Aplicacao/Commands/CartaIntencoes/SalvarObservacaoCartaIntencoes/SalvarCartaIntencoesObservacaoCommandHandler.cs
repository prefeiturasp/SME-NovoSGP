using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarCartaIntencoesObservacaoCommandHandler : IRequestHandler<SalvarCartaIntencoesObservacaoCommand, AuditoriaDto>
    {
        private readonly IRepositorioCartaIntencoesObservacao repositorioCartaIntencoesObservacao;

        public SalvarCartaIntencoesObservacaoCommandHandler(IRepositorioCartaIntencoesObservacao repositorioCartaIntencoesObservacao)
        {
            this.repositorioCartaIntencoesObservacao = repositorioCartaIntencoesObservacao ?? throw new System.ArgumentNullException(nameof(repositorioCartaIntencoesObservacao));
        }

        public async Task<AuditoriaDto> Handle(SalvarCartaIntencoesObservacaoCommand request, CancellationToken cancellationToken)
        {
            var cartaIntencoesObservacao = new CartaIntencoesObservacao(request.Observacao, request.TurmaId, request.ComponenteCurricularId, request.UsuarioId); ;
            await repositorioCartaIntencoesObservacao.SalvarAsync(cartaIntencoesObservacao);
            return (AuditoriaDto)cartaIntencoesObservacao;
        }
    }
}
