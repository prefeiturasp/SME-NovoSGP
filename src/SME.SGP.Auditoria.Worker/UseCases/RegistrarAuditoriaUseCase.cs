using SME.SGP.Auditoria.Worker.Interfaces;
using SME.SGP.Auditoria.Worker.Repositorio.Interfaces;
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

        public async Task<bool> Executar(Infra.MensagemRabbit mensagem)
        {
            var auditoria = mensagem.ObterObjetoMensagem<Entidade.Auditoria>();
            await repositorioAuditoria.Salvar(auditoria);

            return true;
        }
    }
}
