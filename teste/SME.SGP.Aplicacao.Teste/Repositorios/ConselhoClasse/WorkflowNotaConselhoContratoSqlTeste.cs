using Moq;
using SME.SGP.Dados;
using SME.SGP.Dados.Repositorios;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using SME.SGP.Infra.Interfaces;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace SME.SGP.Aplicacao.Teste.Repositorios.ConselhoClasse
{
    // O interceptor de telemetria é estático: esta coleção não pode executar
    // simultaneamente com outras coleções que eventualmente usem o mesmo estado.
    [CollectionDefinition("Contrato SQL workflow conselho", DisableParallelization = true)]
    public class WorkflowNotaConselhoContratoSqlCollection { }

    [Collection("Contrato SQL workflow conselho")]
    public class WorkflowNotaConselhoContratoSqlTeste
    {
        private readonly ITestOutputHelper output;

        public WorkflowNotaConselhoContratoSqlTeste(ITestOutputHelper output) => this.output = output;

        [Fact]
        public async Task Consulta_deve_filtrar_excluidos_e_preservar_precedencia_nota_conceito_e_sentinela()
        {
            // Teste do SQL emitido, não da execução SQL: a ação de banco NÃO é invocada.
            // A exclusão real de linhas será validada na etapa de integração.
            string sqlCapturado = null;
            string parametrosCapturados = null;
            var telemetria = new Mock<IServicoTelemetria>(MockBehavior.Strict);
            telemetria.Setup(t => t.RegistrarComRetornoAsync<double?>(It.IsAny<Func<Task<object>>>(), "Postgres",
                    It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Callback<Func<Task<object>>, string, string, string, string>((acao, nome, operacao, sql, parametros) =>
                {
                    sqlCapturado = sql;
                    parametrosCapturados = parametros;
                })
                .ReturnsAsync((object)null);

            DapperExtensionMethods.Init(telemetria.Object);
            try
            {
                var repositorio = new RepositorioConselhoClasseNotaConsulta(Mock.Of<ISgpContextConsultas>(), Mock.Of<IServicoAuditoria>());
                var retorno = await repositorio.VerificaNotaConselhoEmAprovacao(1001);

                var sqlNormalizado = Regex.Replace(sqlCapturado, @"\s+", " ").Trim().ToLowerInvariant();
                Assert.Equal("select coalesce(coalesce(wf.nota, wf.conceito_id),-1) from wf_aprovacao_nota_conselho wf " +
                    "where wf.conselho_classe_nota_id = @conselhoclassenotaid and not wf.excluido", sqlNormalizado);
                Assert.Contains("conselhoClasseNotaId = 1001", parametrosCapturados);
                Assert.Null(retorno);
                telemetria.Verify(t => t.RegistrarComRetornoAsync<double?>(It.IsAny<Func<Task<object>>>(), "Postgres",
                    It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
                output.WriteLine("SQL escalar de referência: " + sqlCapturado);
                output.WriteLine("Filtro de excluídos e coalesce verificados no comando emitido; sem conexão ou execução SQL.");
            }
            finally
            {
                DapperExtensionMethods.Init(null);
            }
        }

        [Fact]
        public async Task Consulta_em_lote_deve_filtrar_excluidos_e_usar_array_de_ids()
        {
            string sqlCapturado = null;
            var telemetria = new Mock<IServicoTelemetria>(MockBehavior.Strict);
            telemetria.Setup(t => t.RegistrarComRetornoAsync<ConselhoClasseNotaAprovacaoDto>(
                    It.IsAny<Func<Task<object>>>(), "Postgres", It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Callback<Func<Task<object>>, string, string, string, string>((acao, nome, operacao, sql, parametros) => sqlCapturado = sql)
                .ReturnsAsync((object)Array.Empty<ConselhoClasseNotaAprovacaoDto>());

            DapperExtensionMethods.Init(telemetria.Object);
            try
            {
                var repositorio = new RepositorioConselhoClasseNotaConsulta(Mock.Of<ISgpContextConsultas>(), Mock.Of<IServicoAuditoria>());
                var retorno = await repositorio.ObterNotasConselhoEmAprovacaoPorIds(new long[] { 1001, 1002, 1002 });

                var sqlNormalizado = Regex.Replace(sqlCapturado, @"\s+", " ").Trim().ToLowerInvariant();
                Assert.Equal("select wf.conselho_classe_nota_id as id, coalesce(coalesce(wf.nota, wf.conceito_id), -1) as notaemaprovacao " +
                    "from wf_aprovacao_nota_conselho wf where wf.conselho_classe_nota_id = any(@idsconselhoclassenota) and not wf.excluido", sqlNormalizado);
                Assert.Empty(retorno);
                telemetria.Verify(t => t.RegistrarComRetornoAsync<ConselhoClasseNotaAprovacaoDto>(
                    It.IsAny<Func<Task<object>>>(), "Postgres", It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
                output.WriteLine("SQL em lote: " + sqlCapturado);
            }
            finally
            {
                DapperExtensionMethods.Init(null);
            }
        }
    }
}
