using SME.SGP.Dominio.Interfaces;
using System;

namespace SME.SGP.Dominio.Servicos
{
    public class ServicoNotificacao : IServicoNotificacao
    {
        private readonly IRepositorioNotificacao repositorioNotificacao;

        public ServicoNotificacao(IRepositorioNotificacao repositorioNotificacao)
        {
            this.repositorioNotificacao = repositorioNotificacao ?? throw new ArgumentNullException(nameof(repositorioNotificacao));
        }

        public void GeraNovoCodigo(Notificacao notificacao)
        {
            notificacao.Codigo = ObtemNovoCodigo();
        }

        public long ObtemNovoCodigo()
        {
            return repositorioNotificacao.ObterUltimoCodigoPorAno(DateTime.Now.Year) + 1;
        }
    }
}