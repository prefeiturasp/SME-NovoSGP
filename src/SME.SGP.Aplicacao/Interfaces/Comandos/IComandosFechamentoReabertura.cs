using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IComandosFechamentoReabertura
    {
        Task<string> Alterar(FechamentoReaberturaAlteracaoDto fechamentoReaberturaPersistenciaDto, long id, bool alteracaoHierarquicaConfirmacao);

        Task<string> Excluir(long[] ids);

        Task<string> Salvar(FechamentoReaberturaPersistenciaDto fechamentoReaberturaPersistenciaDto);
    }
}