using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IAlterarObservacaoDiarioBordoUseCase
    {
        Task<AuditoriaDto> Executar(string observacao, long observacaoId, IEnumerable<long> usuariosIdNotificacao);
    }
}