using Hangfire;
using SME.Background.Core.Enumerados;
using SME.Background.Core.Interfaces;
using System;
using System.Linq.Expressions;

namespace SME.Background.Hangfire
{
    public class Processador : IProcessador
    {
        readonly IRegistrador registrador;

        public Processador(IRegistrador registrador)
        {
            this.registrador = registrador;
        }

        public string Executar(Expression<Action> metodo)
        {
            return BackgroundJob.Enqueue(metodo);
        }

        public void ExecutarPeriodicamente(Expression<Action> metodo, string cron)
        {
            RecurringJob.AddOrUpdate(metodo, cron);
        }

        public EstadoProcessamento ObterEstadoProcessamento(string idCorrelato)
        {
            throw new NotImplementedException();
        }

        public void Registrar()
        {
            registrador.Registrar();
        }
    }
}
