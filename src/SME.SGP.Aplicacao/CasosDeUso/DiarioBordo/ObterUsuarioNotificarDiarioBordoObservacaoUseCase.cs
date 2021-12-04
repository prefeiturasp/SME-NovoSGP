using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterUsuarioNotificarDiarioBordoObservacaoUseCase : IObterUsuarioNotificarDiarioBordoObservacaoUseCase
    {
        private readonly IMediator mediator;

        public ObterUsuarioNotificarDiarioBordoObservacaoUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<UsuarioNotificarDiarioBordoObservacaoDto>> Executar(ObterUsuarioNotificarDiarioBordoObservacaoDto dto)
        {
            var usuariosNotificados = await mediator.Send(new ObterDiarioBordoNotificacaoQuery(dto.TurmaId, dto.ObservacaoId, dto.DiarioBordoId));

            return usuariosNotificados;
        }        
    }
}