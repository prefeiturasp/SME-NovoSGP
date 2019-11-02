using SME.SGP.Dto;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface IComandosDiasLetivos
    {
        //mudar para async
        DiasLetivosDto CalcularDiasLetivos(long tipoCalendarioId);
    }
}