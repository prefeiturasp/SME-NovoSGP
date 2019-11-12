using SME.Background.Core.Enumerados;
using System;
using System.Linq.Expressions;

namespace SME.Background.Core
{
    public static class Cliente
    {
        public static string Executar(Expression<Action> metodo, TipoProcessamento tipoProcessamento = TipoProcessamento.ExecucaoLonga)
        {
            return Orquestrador.ObterProcessador(tipoProcessamento).Executar(metodo);
        }

        public static void ExecutarPeriodicamente(Expression<Action> metodo, string cron)
        {
            Orquestrador.ObterProcessador(TipoProcessamento.ExecucaoRecorrente).ExecutarPeriodicamente(metodo, cron);
        }

        public static EstadoProcessamento ObterEstadoProcessamento(string idCorrelato, TipoProcessamento tipoProcessamento)
        {
            return Orquestrador.ObterProcessador(tipoProcessamento).ObterEstadoProcessamento(idCorrelato);
        }

    }
}
