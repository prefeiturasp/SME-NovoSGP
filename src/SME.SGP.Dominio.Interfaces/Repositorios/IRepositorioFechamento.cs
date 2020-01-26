using System;
using System.Collections.Generic;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioFechamento : IRepositorioBase<Fechamento>
    {
        void AlterarPeriodosComHierarquiaInferior(DateTime inicioDoFechamento, DateTime finalDoFechamento, long periodoEscolarId, long? dreId);

        Fechamento ObterPorTipoCalendarioDreEUE(long tipoCalendarioId, long? dreId, long? ueId);

        void SalvarBimestres(IEnumerable<FechamentoBimestre> fechamentosBimestre, long fechamentoId);

        bool ValidaRegistrosForaDoPeriodo(DateTime inicioDoFechamento, DateTime finalDoFechamento, long fechamentoId, long periodoEscolarId, long? dreId);
    }
}