using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IComandosTipoCalendario
    {
        void MarcarExcluidos(long[] ids);
        Task Incluir(TipoCalendarioDto tipoCalendarioDto);
        Task Alterar(TipoCalendarioDto tipoCalendarioDto, long id);
        Task ExecutarReplicacao(TipoCalendarioDto dto, bool inclusao, TipoCalendario tipoCalendario);
    }
}