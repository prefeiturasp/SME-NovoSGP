using SME.SGP.Dto;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public interface IComandosDiasLetivos
    {
        DiasLetivosDto CalcularDiasLetivos(FiltroDiasLetivosDTO filtro);
    }
}