using System;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IServicoFeriadoCalendario
    {
        DateTime CalcularFeriado(int ano, FeriadoEnum feriado);

        Task VerficaSeExisteFeriadosMoveisEInclui(int ano);
    }
}