using SME.SGP.Dto;

namespace SME.SGP.Aplicacao
{
    public interface IComandosTipoCalendario
    {
        void Salvar(TipoCalendarioDto dto);
        void MarcarExcluidos(long[] ids);
    }
}
