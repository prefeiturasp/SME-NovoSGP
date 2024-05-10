using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IAdicionarObservacaoDiarioBordoUseCase
    {
        Task<AuditoriaDto> Executar(string observacao, long diarioBordoId, IEnumerable<long> UsuarioIdsNotificacao);
    }
}