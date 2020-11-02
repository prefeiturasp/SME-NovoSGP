using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IComandosTipoCalendario
    {
        void MarcarExcluidos(long[] ids);
        Task Incluir(TipoCalendarioDto dto);
        Task Alterar(TipoCalendarioDto dto, long id);
        Task ExecutarMetodosAsync(TipoCalendarioDto dto, bool inclusao, TipoCalendario tipoCalendario);
    }
}