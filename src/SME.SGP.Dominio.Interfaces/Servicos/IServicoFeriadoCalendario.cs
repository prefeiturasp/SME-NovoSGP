using System;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IServicoFeriadoCalendario
    {
        DateTime CalcularFeriado(int ano, FeriadoEnum feriado);

        void VerficaSeExisteFeriadosMoveisEInclui(int ano);
    }
}