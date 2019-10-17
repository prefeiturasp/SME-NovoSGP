using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public interface IComandosTipoCalendario
    {
        void MarcarExcluidos(long[] ids);

        void Salvar(TipoCalendarioDto dto);
    }
}