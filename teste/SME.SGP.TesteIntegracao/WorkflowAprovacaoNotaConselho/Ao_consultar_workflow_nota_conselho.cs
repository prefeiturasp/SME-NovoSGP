using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.TesteIntegracao.Setup;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace SME.SGP.TesteIntegracao.WorkflowAprovacaoNotaConselho
{
    /// <summary>
    /// Matriz de comparação da US 155457, executada no PostgreSQL do Testcontainers.
    /// Nesta etapa somente a consulta escalar atual é exercitada. Quando a consulta
    /// em lote existir, a mesma matriz deverá ser consultada pelos dois caminhos.
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

        [Fact(DisplayName = "Workflow da nota do Conselho - Caracterizar consulta escalar no PostgreSQL")]
        public async Task Deve_retornar_matriz_de_comparacao_da_consulta_atual()
        {
            await InserirMatrizControlada();
            var mediator = ServiceProvider.GetRequiredService<IMediator>();

            foreach (var esperado in ValoresEsperados)
            {
                var atual = await mediator.Send(new ObterNotaConselhoEmAprovacaoQuery(esperado.Key));

                atual.ShouldBe(esperado.Value);
                output.WriteLine($"conselhoClasseNotaId={esperado.Key}; esperado={Formatar(esperado.Value)}; atual={Formatar(atual)}");
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

        private static string Formatar(double? valor)
            => valor.HasValue ? valor.Value.ToString("0.##", System.Globalization.CultureInfo.InvariantCulture) : "null";
    }
}
