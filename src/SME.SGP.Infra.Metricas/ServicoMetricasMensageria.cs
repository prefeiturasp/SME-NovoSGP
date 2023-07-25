using SME.SGP.Infra.Interface;
using SME.SGP.Infra.Interfaces;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Infra
{
    public class ServicoMetricasMensageria : IServicoMetricasMensageria
    {
        private readonly IServicoMensageriaLogs servicoMensageria;

        public ServicoMetricasMensageria(IServicoMensageriaLogs servicoMensageria)
        {
            this.servicoMensageria = servicoMensageria ?? throw new ArgumentNullException(nameof(servicoMensageria));
        }

        public Task Concluido(string rota)
            => PublicarMetrica(TipoAcaoMensageria.Ack, rota);

        public Task Erro(string rota)
            => PublicarMetrica(TipoAcaoMensageria.Rej, rota);

        public Task Obtido(string rota)
            => PublicarMetrica(TipoAcaoMensageria.Get, rota);

        public Task Publicado(string rota)
            => PublicarMetrica(TipoAcaoMensageria.Pub, rota);

        private Task PublicarMetrica(TipoAcaoMensageria tipoAcao, string rota)
        {

        }
    }
}
