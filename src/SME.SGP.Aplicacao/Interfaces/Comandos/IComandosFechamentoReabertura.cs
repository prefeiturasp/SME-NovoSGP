using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IComandosFechamentoReabertura
    {
        Task<string> Alterar(FechamentoReaberturaAlteracaoDto fechamentoReaberturaPersistenciaDto, long id);

        Task<string> Salvar(FechamentoReaberturaPersistenciaDto fechamentoReaberturaPersistenciaDto);
    }
}