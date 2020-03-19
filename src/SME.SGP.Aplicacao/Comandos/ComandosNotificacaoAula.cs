using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ComandosNotificacaoAula : IComandosNotificacaoAula
    {
        private readonly IRepositorioNotificacaoAula repositorioNotificacaoAula;
        public ComandosNotificacaoAula(IRepositorioNotificacaoAula repositorioNotificacaoAula)
        {
            this.repositorioNotificacaoAula = repositorioNotificacaoAula ?? throw new ArgumentNullException(nameof(repositorioNotificacaoAula));
        }

        public async Task Excluir(long aulaId)
            => await repositorioNotificacaoAula.Excluir(aulaId);

        public async Task Inserir(long notificacaoId, long aulaId)
            => await repositorioNotificacaoAula.Inserir(notificacaoId, aulaId);
    }
}
