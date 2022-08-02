using Newtonsoft.Json;
using Polly;
using Polly.Registry;
using SME.SGP.Dominio;
using SME.SGP.Infra.Interface;
using SME.SGP.Infra.Interfaces;
using System;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Infra
{
    public class ServicoAuditoria : IServicoAuditoria
    {
        private readonly IServicoMensageria servicoMensageria;

        public ServicoAuditoria(IServicoMensageria servicoMensageria)
        {
            this.servicoMensageria = servicoMensageria ?? throw new ArgumentNullException(nameof(servicoMensageria));
        }

        public Task<bool> Auditar(Auditoria auditoria)
            => servicoMensageria.Publicar(new MensagemRabbit(auditoria), RotasRabbitAuditoria.PersistirAuditoriaDB, ExchangeSgpRabbit.Sgp, "PublicarFilaAuditoriaDB");
    }
}
