using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AlterarCartaIntencoesObservacaoUseCase : IAlterarCartaIntencoesObservacaoUseCase
    {
        private readonly IMediator mediator;

        public AlterarCartaIntencoesObservacaoUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<AuditoriaDto> Executar(long cartaIntencoesObservacaoId, AlterarCartaIntencoesObservacaoDto dto)
        {            
            var usuarioLogadoId = await mediator.Send(new ObterUsuarioLogadoIdQuery());
            return await mediator.Send(new AlterarCartaIntencoesObservacaoCommand(dto.Observacao, cartaIntencoesObservacaoId, usuarioLogadoId));
        }
    }
}
