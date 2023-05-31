using SME.SGP.Infra.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface IObterUsuarioNotificarCartaIntencoesObservacaoUseCase
    {
        Task<IEnumerable<UsuarioNotificarCartaIntencoesObservacaoDto>> Executar(ObterUsuarioNotificarCartaIntencoesObservacaoDto dto);
    }
}
