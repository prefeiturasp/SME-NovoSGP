using System;

namespace SME.SGP.Notificacoes.Worker
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ActionAttribute : Attribute
    {
        public ActionAttribute(string rota, string nome)
        {
            Rota = rota;
            Nome = nome;
        }

        public string Rota { get; }
        public string Nome { get; }
    }
}
