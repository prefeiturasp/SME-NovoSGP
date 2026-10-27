using Moq;
using SME.SGP.Dados.Cache;
using SME.SGP.Dados.Repositorios;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.ConselhoDeClasse.NotasFechamentoConselho
{
    // Executa ObterAsync real; isola somente transporte, telemetria e mensageria.
    internal class RepositorioCacheNotasTeste : RepositorioCache
    {
        private readonly bool falharGravacao;
        internal Dictionary<string, string> Dados { get; } = new Dictionary<string, string>();

        internal RepositorioCacheNotasTeste(bool falharGravacao = false)
            : base(CriarTelemetria(), Mock.Of<IServicoMensageriaLogs>(), Mock.Of<IMetricasCache>())
        {
            this.falharGravacao = falharGravacao;
            NomeServicoCache = "Cache de notas em memoria para testes";
        }

        private static IServicoTelemetria CriarTelemetria()
        {
            var telemetria = new Mock<IServicoTelemetria>();
            telemetria.Setup(t => t.RegistrarComRetorno<string>(It.IsAny<Func<object>>(), It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns((Func<object> acao, string nome, string evento, string valor, string parametros) => acao());
            telemetria.Setup(t => t.RegistrarAsync(It.IsAny<Func<Task>>(), It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns((Func<Task> acao, string nome, string evento, string valor, string parametros) => acao());
            return telemetria.Object;
        }

        protected override string ObterValor(string nomeChave)
            => Dados.TryGetValue(nomeChave, out var valor) ? valor : string.Empty;

        protected override Task SalvarValor(string nomeChave, string valor, int minutosParaExpirar)
        {
            if (falharGravacao) return Task.FromException(new InvalidOperationException("Falha sintética ao salvar cache"));
            Dados[nomeChave] = valor;
            return Task.CompletedTask;
        }

        protected override Task RemoverValor(string nomeChave)
        {
            Dados.Remove(nomeChave);
            return Task.CompletedTask;
        }
    }
}
