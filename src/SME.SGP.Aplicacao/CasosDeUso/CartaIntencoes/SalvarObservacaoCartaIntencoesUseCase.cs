using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarObservacaoCartaIntencoesUseCase : ISalvarObservacaoCartaIntencoesUseCase
    {
        private readonly IMediator mediator;

        public SalvarObservacaoCartaIntencoesUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        public async Task<AuditoriaDto> Executar(long cartaIntencoesId, SalvarObservacaoCartaIntencoesDto salvarObservacaoCartaIntencoesDto)
        {
            var usuarioLogadoId = await mediator.Send(new ObterUsuarioLogadoIdQuery());
            return await mediator.Send(new SalvarObservacaoCartaIntencoesCommand(salvarObservacaoCartaIntencoesDto.Descricao, cartaIntencoesId, usuarioLogadoId));
        }
    }
}
