using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IComandosTipoCalendario
    {
        void MarcarExcluidos(long[] ids);
        Task Salvar(TipoCalendarioDto dto);
        Task Incluir(TipoCalendarioDto dto);
        Task Alterar(TipoCalendarioDto dto, long id);
    }
}