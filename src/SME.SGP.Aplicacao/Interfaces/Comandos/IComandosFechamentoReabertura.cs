using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IComandosFechamentoReabertura
    {
        Task Salvar(FechamentoReaberturaPersistenciaDto fechamentoReaberturaPersistenciaDto);
        Task Alterar(FechamentoReaberturaPersistenciaDto fechamentoReaberturaPersistenciaDto, long id);
    }
}