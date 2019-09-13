using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dominio.Servicos
{
    public class ServicoNotificacao
    {
        private readonly IRepositorioNotificacao repositorioNotificacao;

        public ServicoNotificacao(IRepositorioNotificacao repositorioNotificacao)
        {
            this.repositorioNotificacao = repositorioNotificacao ?? throw new ArgumentNullException(nameof(repositorioNotificacao));
        }

        public long GeraNovoCodigo()
        {
            var anoAtual = System.DateTime.Now.Year;
            long codigo = 0;

            //repositorioNotificacao.



            return codigo;
        }
    }
}
