using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Polly.Registry;
using SME.SGP.Dados.Repositorios;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using SME.SGP.Infra.Interfaces;

namespace SME.SGP.TesteIntegracao.ServicosFakes
{
    public class ServicoMensageriaLogsFake : ServicoMensageria<LogMensagem>, IServicoMensageriaLogs
    {
        public override string ObterParametrosMensagem(LogMensagem mensagemLog)
        {
            var json = JsonConvert.SerializeObject(mensagemLog);
            var mensagem = JsonConvert.DeserializeObject<LogMensagem>(json);
            return mensagem!.Mensagem + ", ExcecaoInterna:" + mensagem.ExcecaoInterna;
        }

        public ServicoMensageriaLogsFake(IConexoesRabbitFilasLog conexaoRabbit, IServicoTelemetria servicoTelemetria,
            IReadOnlyPolicyRegistry<string> registry)
            : base(conexaoRabbit, servicoTelemetria, registry)
        {
        }
    }
}