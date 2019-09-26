using System;
using System.Collections.Generic;

namespace SME.SGP.Dominio
{
    public class Usuario : EntidadeBase
    {
        public string CodigoRf { get; set; }
        public IEnumerable<Notificacao> Notificacoes { get { return notificacoes; } }
        public DateTime UltimoLogin { get; set; }
        private IList<Notificacao> notificacoes { get; set; }

        public void Adicionar(Notificacao notificacao)
        {
            if (notificacao != null)
                notificacoes.Add(notificacao);
        }

        public void AtualizaUltimoLogin()
        {
            this.UltimoLogin = DateTime.Now;
        }
    }
}