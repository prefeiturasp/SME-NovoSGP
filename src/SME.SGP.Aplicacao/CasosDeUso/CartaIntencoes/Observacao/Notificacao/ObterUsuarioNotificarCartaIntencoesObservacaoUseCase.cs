using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterUsuarioNotificarCartaIntencoesObservacaoUseCase : IObterUsuarioNotificarCartaIntencoesObservacaoUseCase
    {
        private readonly IMediator mediator;

        public ObterUsuarioNotificarCartaIntencoesObservacaoUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<UsuarioNotificarCartaIntencoesObservacaoDto>> Executar(ObterUsuarioNotificarCartaIntencoesObservacaoDto dto)
        {
            var usuariosNotificados = await mediator.Send(new ObterCartaIntencoesNotificacaoQuery(dto.TurmaId, dto.ComponenteCurricular));

            return usuariosNotificados;
        }
    }
}
