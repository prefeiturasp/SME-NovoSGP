using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace SME.SGP.TesteIntegracao.WorkflowAprovacaoNotaConselho
{
    /// <summary>
    /// Matriz de comparação da US 155457, executada no PostgreSQL do Testcontainers.
    /// A mesma matriz é consultada pelos caminhos escalar e em lote para proteger
    /// a paridade da resposta da US 155457.
    /// </summary>
    public class Ao_consultar_workflow_nota_conselho : TesteBase
    {
        private readonly ITestOutputHelper output;

        private static readonly IReadOnlyDictionary<long, double?> ValoresEsperados =
            new Dictionary<long, double?>
            {
                [1001] = 8.5,  // workflow com nota numérica
                [1002] = 3,    // workflow com conceito
                [1003] = 0,    // zero é valor válido e não ausência
                [1004] = null, // há workflow, mas está excluído
                [1005] = null, // não há workflow
                [1006] = 7.5,  // nota precede conceito quando ambos existem
                [1007] = -1    // registro ativo sem nota/conceito usa a sentinela atual
            };

        public Ao_consultar_workflow_nota_conselho(CollectionFixture collectionFixture,
            ITestOutputHelper output) : base(collectionFixture)
        {
            this.output = output;
        }

        [Fact(DisplayName = "Workflow da nota do Conselho - Comparar consultas escalar e em lote no PostgreSQL")]
        public async Task Deve_retornar_os_mesmos_valores_nas_consultas_escalar_e_em_lote()
        {
            await InserirMatrizControlada();
            var mediator = ServiceProvider.GetRequiredService<IMediator>();
            var idsComRepeticao = ValoresEsperados.Keys.Concat(new long[] { 1001, 1002 }).ToArray();
            var retornoEmLote = (await mediator.Send(new ObterNotasConselhoEmAprovacaoPorIdsQuery(idsComRepeticao))).ToArray();

            retornoEmLote.Length.ShouldBe(5);
            retornoEmLote.Select(nota => nota.Id).Distinct().Count().ShouldBe(retornoEmLote.Length);

            foreach (var esperado in ValoresEsperados)
            {
                var retornoEscalar = await mediator.Send(new ObterNotaConselhoEmAprovacaoQuery(esperado.Key));
                var notaLote = retornoEmLote.FirstOrDefault(nota => nota.Id == esperado.Key);
                var valorLote = notaLote?.NotaEmAprovacao;

                retornoEscalar.ShouldBe(esperado.Value);
                valorLote.ShouldBe(retornoEscalar);
                output.WriteLine($"conselhoClasseNotaId={esperado.Key}; esperado={Formatar(esperado.Value)}; " +
                    $"escalar={Formatar(retornoEscalar)}; lote={Formatar(valorLote)}");
            }
        }

        [Fact(DisplayName = "Workflow da nota do Conselho - Evidenciar desempenho escalar versus lote no PostgreSQL")]
        public async Task Deve_evidenciar_reducao_de_round_trips_e_latencia_da_consulta_em_lote()
        {
            const int quantidadeMassa = 160;
            const int repeticoes = 15;
            var tamanhos = new[] { 1, 8, 16, 100 };
            var ids = Enumerable.Range(1, quantidadeMassa).Select(indice => 200000L + indice).ToArray();

            await InserirMassaDesempenho(ids);
            var mediator = ServiceProvider.GetRequiredService<IMediator>();

            foreach (var tamanho in tamanhos)
            {
                var idsCenario = ids.Take(tamanho).ToArray();
                await ConsultarEscalar(mediator, idsCenario);
                await ConsultarEmLote(mediator, idsCenario);

                var temposEscalar = new List<double>(repeticoes);
                var temposLote = new List<double>(repeticoes);

                for (var repeticao = 0; repeticao < repeticoes; repeticao++)
                {
                    double[] retornoEscalar;
                    IReadOnlyDictionary<long, double> retornoLote;

                    if (repeticao % 2 == 0)
                    {
                        (retornoEscalar, var tempoEscalar) = await MedirEscalar(mediator, idsCenario);
                        (retornoLote, var tempoLote) = await MedirLote(mediator, idsCenario);
                        temposEscalar.Add(tempoEscalar);
                        temposLote.Add(tempoLote);
                    }
                    else
                    {
                        (retornoLote, var tempoLote) = await MedirLote(mediator, idsCenario);
                        (retornoEscalar, var tempoEscalar) = await MedirEscalar(mediator, idsCenario);
                        temposLote.Add(tempoLote);
                        temposEscalar.Add(tempoEscalar);
                    }

                    retornoLote.Count.ShouldBe(tamanho);
                    for (var indice = 0; indice < idsCenario.Length; indice++)
                        retornoLote[idsCenario[indice]].ShouldBe(retornoEscalar[indice]);
                }

                var medianaEscalar = Percentil(temposEscalar, 0.50);
                var medianaLote = Percentil(temposLote, 0.50);
                var p95Escalar = Percentil(temposEscalar, 0.95);
                var p95Lote = Percentil(temposLote, 0.95);

                output.WriteLine(
                    $"quantidade={tamanho}; repeticoes={repeticoes}; roundTripsEscalar={tamanho}; roundTripsLote=1; " +
                    $"medianaEscalarMs={FormatarTempo(medianaEscalar)}; medianaLoteMs={FormatarTempo(medianaLote)}; " +
                    $"p95EscalarMs={FormatarTempo(p95Escalar)}; p95LoteMs={FormatarTempo(p95Lote)}; " +
                    $"fatorMediana={FormatarTempo(medianaEscalar / medianaLote)}");
            }
        }

        private async Task InserirMatrizControlada()
        {
            // A consulta sob teste lê apenas a tabela de workflow. As FKs são suspensas
            // somente nesta sessão do banco descartável para manter a fixture mínima;
            // o finally restaura o comportamento normal mesmo se a carga falhar.
            await using var comando = _collectionFixture.Database.Conexao.CreateCommand();
            try
            {
                comando.CommandText = @"
                    set session_replication_role = replica;

                    insert into wf_aprovacao_nota_conselho
                        (wf_aprovacao_id, conselho_classe_nota_id, usuario_solicitante_id, nota, conceito_id, excluido)
                    values
                        (null, 1001, 1, 8.5, null, false),
                        (null, 1002, 1, null, 3, false),
                        (null, 1003, 1, 0, null, false),
                        (null, 1004, 1, 9, null, true),
                        (null, 1006, 1, 7.5, 2, false),
                        (null, 1007, 1, null, null, false);

                    set session_replication_role = origin;";

                await comando.ExecuteNonQueryAsync();
            }
            finally
            {
                await using var restaurar = new NpgsqlCommand("set session_replication_role = origin;",
                    _collectionFixture.Database.Conexao);
                await restaurar.ExecuteNonQueryAsync();
            }
        }

        private async Task InserirMassaDesempenho(IEnumerable<long> ids)
        {
            await using var comando = _collectionFixture.Database.Conexao.CreateCommand();
            try
            {
                comando.CommandText = @"
                    set session_replication_role = replica;

                    insert into wf_aprovacao_nota_conselho
                        (wf_aprovacao_id, conselho_classe_nota_id, usuario_solicitante_id, nota, conceito_id, excluido)
                    select null, id, 1, (id % 20)::numeric / 2, null, false
                      from unnest(@ids) id;

                    set session_replication_role = origin;";
                comando.Parameters.AddWithValue("ids", ids.ToArray());

                await comando.ExecuteNonQueryAsync();
            }
            finally
            {
                await using var restaurar = new NpgsqlCommand("set session_replication_role = origin;",
                    _collectionFixture.Database.Conexao);
                await restaurar.ExecuteNonQueryAsync();
            }
        }

        private static async Task<(double[] Retorno, double TempoMs)> MedirEscalar(IMediator mediator,
            long[] ids)
        {
            var cronometro = Stopwatch.StartNew();
            var retorno = await ConsultarEscalar(mediator, ids);
            cronometro.Stop();
            return (retorno, cronometro.Elapsed.TotalMilliseconds);
        }

        private static async Task<(IReadOnlyDictionary<long, double> Retorno, double TempoMs)> MedirLote(
            IMediator mediator, long[] ids)
        {
            var cronometro = Stopwatch.StartNew();
            var retorno = await ConsultarEmLote(mediator, ids);
            cronometro.Stop();
            return (retorno, cronometro.Elapsed.TotalMilliseconds);
        }

        private static async Task<double[]> ConsultarEscalar(IMediator mediator, IEnumerable<long> ids)
        {
            var retorno = new List<double>();
            foreach (var id in ids)
                retorno.Add((await mediator.Send(new ObterNotaConselhoEmAprovacaoQuery(id))).Value);
            return retorno.ToArray();
        }

        private static async Task<IReadOnlyDictionary<long, double>> ConsultarEmLote(IMediator mediator,
            IEnumerable<long> ids)
            => (await mediator.Send(new ObterNotasConselhoEmAprovacaoPorIdsQuery(ids)))
                .ToDictionary(nota => nota.Id, nota => nota.NotaEmAprovacao);

        private static double Percentil(IEnumerable<double> valores, double percentil)
        {
            var ordenados = valores.OrderBy(valor => valor).ToArray();
            var indice = Math.Max(0, (int)Math.Ceiling(percentil * ordenados.Length) - 1);
            return ordenados[indice];
        }

        private static string Formatar(double? valor)
            => valor.HasValue ? valor.Value.ToString("0.##", System.Globalization.CultureInfo.InvariantCulture) : "null";

        private static string FormatarTempo(double valor)
            => valor.ToString("0.###", CultureInfo.InvariantCulture);
    }
}
