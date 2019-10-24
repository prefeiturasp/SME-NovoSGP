using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public interface IComandosFeriadoCalendario
    {
        void MarcarExcluidos(long[] ids);

        void Salvar(FeriadoCalendarioDto dto);
    }
}