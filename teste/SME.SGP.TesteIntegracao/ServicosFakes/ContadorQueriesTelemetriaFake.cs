using Elastic.Apm.Api;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.ServicosFakes
{
    /// <summary>
    /// IServicoTelemetria de teste que apenas delega a ação (igual ao TelemetriaFake) e,
    /// além disso, registra o SQL de cada consulta executada — permitindo contar quantas
    /// vezes uma consulta foi disparada ao banco durante uma etapa.
    /// </summary>
    public class ContadorQueriesTelemetriaFake : IServicoTelemetria
    {
        private static readonly object Trava = new();
        private static readonly List<string> sqlsExecutados = new();

        public static void Limpar()
        {
            lock (Trava) sqlsExecutados.Clear();
        }

        public static int ContarPorTrecho(string trechoSql)
        {
            lock (Trava)
                return sqlsExecutados.Count(s => s != null && s.Contains(trechoSql, StringComparison.OrdinalIgnoreCase));
        }

        private static void Registrar(string telemetriaValor)
        {
            lock (Trava) sqlsExecutados.Add(telemetriaValor);
        }

        public void Registrar(Action acao, string acaoNome, string telemetriaNome, string telemetriaValor)
        {
            Registrar(telemetriaValor);
            acao();
        }

        public dynamic RegistrarComRetorno<T>(Func<object> acao, string acaoNome, string telemetriaNome, string telemetriaValor, string parametros = "")
        {
            Registrar(telemetriaValor);
            return acao();
        }

        public Task RegistrarAsync(Func<Task> acao, string acaoNome, string telemetriaNome, string telemetriaValor, string parametros = "")
        {
            Registrar(telemetriaValor);
            return acao();
        }

        public Task<dynamic> RegistrarComRetornoAsync<T>(Func<Task<object>> acao, string acaoNome, string telemetriaNome, string telemetriaValor, string parametros = "")
        {
            Registrar(telemetriaValor);
            return acao();
        }

        public ITransaction Iniciar(string nome, string tipo) => null;

        public void Finalizar(ITransaction transacao) { }
    }
}
