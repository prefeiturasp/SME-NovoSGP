using SME.SGP.Dominio;
using SME.SGP.Infra.Interface;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Infra
{
    public class ServicoAuditoria : IServicoAuditoria
    {
        private readonly IServicoMensageriaSGP servicoMensageria;

        public ServicoAuditoria(IServicoMensageriaSGP servicoMensageria)
        {
            this.servicoMensageria = servicoMensageria ?? throw new ArgumentNullException(nameof(servicoMensageria));
        }

        public Task<bool> Auditar(Auditoria auditoria)
            => servicoMensageria.Publicar(new MensagemRabbit(auditoria), RotasRabbitAuditoria.PersistirAuditoriaDB, ExchangeSgpRabbit.Sgp, "PublicarFilaAuditoriaDB");
    }
}
