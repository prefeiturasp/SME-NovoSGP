using SME.Background.Core.Enumerados;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace SME.Background.Core.Interfaces
{
    public interface IProcessadorCliente
    {
        void Registrar();
        string Executar(System.Linq.Expressions.Expression<Action> metodo);
        string Executar<T>(Expression<Action<T>> metodo);
        void ExecutarPeriodicamente(System.Linq.Expressions.Expression<Action> metodo, string cron);
        EstadoProcessamento ObterEstadoProcessamento(string idCorrelato);
    }
}
