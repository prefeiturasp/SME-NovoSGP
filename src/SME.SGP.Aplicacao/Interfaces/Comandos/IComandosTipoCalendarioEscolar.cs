using SME.SGP.Dto;

namespace SME.SGP.Aplicacao
{
    public interface IComandosTipoCalendarioEscolar
    {
        void Salvar(TipoCalendarioEscolarDto dto);
        void MarcarExcluidos(long[] ids);
    }
}
