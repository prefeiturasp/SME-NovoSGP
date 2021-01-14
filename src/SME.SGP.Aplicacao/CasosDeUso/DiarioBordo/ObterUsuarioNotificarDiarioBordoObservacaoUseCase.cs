using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterUsuarioNotificarDiarioBordoObservacaoUseCase : AbstractUseCase, IObterUsuarioNotificarDiarioBordoObservacaoUseCase
    {
        public ObterUsuarioNotificarDiarioBordoObservacaoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<IEnumerable<UsuarioNotificarDiarioBordoObservacaoDto>> Executar(ObterUsuarioNotificarDiarioBordoObservacaoDto dto) 
            => await mediator.Send(new ObterUsuarioNotificarDiarioBordoObservacaoQuery(dto.TurmaId, dto.ObservacaoId));
    }
}