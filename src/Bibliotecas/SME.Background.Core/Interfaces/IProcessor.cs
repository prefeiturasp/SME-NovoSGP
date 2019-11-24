using System;
using System.Linq.Expressions;

namespace SME.Background.Core.Interfaces
{
    public interface IProcessor
    {
        string Executar(Expression<Action> metodo);

        string Executar<T>(Expression<Action<T>> metodo);

        void ExecutarPeriodicamente(Expression<Action> metodo, string cron);

        void ExecutarPeriodicamente<T>(Expression<Action<T>> metodo, string cron);

        void Registrar();
    }
}