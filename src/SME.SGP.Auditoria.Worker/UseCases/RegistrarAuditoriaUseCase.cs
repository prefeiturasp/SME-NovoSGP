using SME.SGP.Auditoria.Worker.Interfaces;
using System.Threading.Tasks;

namespace SME.SGP.Auditoria.Worker
{
    public class RegistrarAuditoriaUseCase : IRegistrarAuditoriaUseCase
    {
        private readonly IRepositorioAuditoria repositorioAuditoria;

        public RegistrarAuditoriaUseCase(IRepositorioAuditoria repositorioAuditoria)
        {
            this.repositorioAuditoria = repositorioAuditoria ?? throw new System.ArgumentNullException(nameof(repositorioAuditoria));
        }

        public Task Executar(MensagemRabbit mensagem)
        {
            var auditoria = mensagem.ObterObjetoMensagem<Entidade.Auditoria>();
            return repositorioAuditoria.Salvar(auditoria);
        }
    }
}
