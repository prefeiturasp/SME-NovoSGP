using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioDiarioBordoConsulta
    {
        Task<IEnumerable<DiarioBordo>> ObterIdDiarioBordoAulasExcluidas(string codigoTurma, string[] codigosDisciplinas, long tipoCalendarioId, DateTime[] datasConsideradas);
    }
}
