using SME.SGP.Infra.Dtos;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface IObterUsuarioNotificarDiarioBordoObservacaoUseCase : IUseCase<ObterUsuarioNotificarDiarioBordoObservacaoDto, IEnumerable<UsuarioNotificarDiarioBordoObservacaoDto>>
    {
    }
}