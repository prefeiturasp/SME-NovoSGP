using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IComandosFechamentoReabertura
    {
        Task Alterar(FechamentoReaberturaAlteracaoDto fechamentoReaberturaPersistenciaDto, long id);

        Task Salvar(FechamentoReaberturaPersistenciaDto fechamentoReaberturaPersistenciaDto);
    }
}