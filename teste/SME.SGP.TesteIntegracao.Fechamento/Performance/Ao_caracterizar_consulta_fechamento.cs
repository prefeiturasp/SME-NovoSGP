using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json;
using SME.SGP.Aplicacao;
using SME.SGP.Dados;
using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.NotaFechamentoFinal.Base;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.Fechamento.Performance
{
    public class Ao_caracterizar_consulta_fechamento : NotaFechamentoTesteBase
    {
        private static readonly DateTime AuditoriaFixa = new DateTime(2024, 1, 1);
        public Ao_caracterizar_consulta_fechamento(CollectionFixture fixture) : base(fixture) { }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);
            services.AddSingleton<CenarioFechamento>();
            services.AddSingleton<MedicaoRepositorios>();
            services.Replace(ServiceDescriptor.Singleton<IHttpClientFactory, HttpExternoProibido>());
            RegistrarResposta<ObterAlunosPorTurmaEAnoLetivoQuery, IEnumerable<AlunoPorTurmaResposta>>(services, c => c.Alunos);
            RegistrarResposta<ObterComponenteCurricularPorIdQuery, DisciplinaDto>(services, c => c.Disciplina);
            RegistrarResposta<ObterComponentesCurricularesPorCodigoTurmaLoginEPerfilParaPlanejamentoQuery, IEnumerable<ComponenteCurricularEol>>(services,
                c => c.Componentes.Select(id => new ComponenteCurricularEol { Codigo = id, Descricao = "Componente " + id }));
            // Exercita ambos os ramos da política; a política de perfis/ano não é o alvo desta medição.
            RegistrarResposta<ExigeAprovacaoDeNotaQuery, bool>(services, c => c.ExigeAprovacao);
            RegistrarResposta<ExisteFrequenciaRegistradaPorTurmaComponenteCurricularQuery, bool>(services, c => true);
            RegistrarResposta<ObterTurmaEmPeriodoDeFechamentoQuery, bool>(services, c => false);
            Observar<IRepositorioFrequenciaAlunoDisciplinaPeriodoConsulta, RepositorioFrequenciaAlunoDisciplinaPeriodoConsulta>(services);
            Observar<IRepositorioConceitoConsulta, RepositorioConceitoConsulta>(services);
            Observar<IRepositorioNotasConceitosConsulta, RepositorioNotasConceitosConsulta>(services);
        }

        private static void RegistrarResposta<TQuery, TResposta>(IServiceCollection services, Func<CenarioFechamento, TResposta> resposta)
            where TQuery : IRequest<TResposta>
            => services.Replace(ServiceDescriptor.Scoped<IRequestHandler<TQuery, TResposta>>(sp =>
                new RespostaFixa<TQuery, TResposta>(() => resposta(sp.GetRequiredService<CenarioFechamento>()))));

        private static void Observar<TInterface, TRepositorio>(IServiceCollection services)
            where TInterface : class where TRepositorio : class, TInterface
            => services.Replace(ServiceDescriptor.Scoped<TInterface>(sp => ObservadorRepositorio<TInterface>.Criar(
                ActivatorUtilities.CreateInstance<TRepositorio>(sp), sp.GetRequiredService<MedicaoRepositorios>())));

        [Theory]
        [InlineData("numerica", 1)]
        [InlineData("numerica", 10)]
        [InlineData("numerica", 30)]
        [InlineData("conceito", 30)]
        [InlineData("regencia", 1)]
        [InlineData("regencia", 10)]
        [InlineData("regencia", 30)]
        [InlineData("aprovacao", 30)]
        [InlineData("regencia-aprovacao", 30)]
        [InlineData("sem-frequencia", 2)]
        [InlineData("conceito-inexistente", 2)]
        [InlineData("sintese", 2)]
        [InlineData("nota-excluida", 2)]
        [InlineData("outra-turma", 2)]
        public async Task Deve_preservar_resposta_e_registrar_referencia(string modo, int quantidade)
        {
            await Preparar(modo, quantidade);
            var medicao = ServiceProvider.GetRequiredService<MedicaoRepositorios>();
            var servico = ServiceProvider.GetRequiredService<IConsultasFechamentoTurmaDisciplina>();
            var cenario = ServiceProvider.GetRequiredService<CenarioFechamento>();
            Func<Task<FechamentoTurmaDisciplinaBimestreDto>> executar = () => servico.ObterNotasFechamentoTurmaDisciplina(TURMA_CODIGO_1, cenario.Disciplina.Id, 1, 0);
            medicao.Chamadas.Clear();
            var resposta = await executar();
            Assert.Single(medicao.Chamadas.Where(c => c.Repositorio == nameof(IRepositorioFrequenciaAlunoDisciplinaPeriodoConsulta)));
            Assert.DoesNotContain(medicao.Chamadas, c => c.Metodo == "ObterPorAlunoDisciplinaData");
            Assert.Equal(cenario.EhConceito ? 1 : 0,
                medicao.Chamadas.Count(c => c.Repositorio == nameof(IRepositorioConceitoConsulta)));
            Assert.Equal(cenario.ExigeAprovacao ? 1 : 0,
                medicao.Chamadas.Count(c => c.Repositorio == nameof(IRepositorioNotasConceitosConsulta)));
            Assert.Equal(quantidade, resposta.Alunos.Count);
            foreach (var aluno in resposta.Alunos)
            {
                Assert.Equal(modo == "sem-frequencia" ? 0 : 3, aluno.QuantidadeFaltas);
                Assert.Equal(modo == "sem-frequencia" ? string.Empty : "90,00", aluno.PercentualFrequencia);
                if (modo == "sintese")
                    Assert.NotNull(aluno.SinteseId);
                else
                {
                    var notas = aluno.Notas?.ToArray() ?? Array.Empty<FechamentoNotaRetornoDto>();
                    Assert.Equal(modo == "nota-excluida" ? 0 : cenario.Componentes.Length, notas.Length);
                    foreach (var nota in notas)
                    {
                        var esperado = modo == "conceito-inexistente" || modo == "regencia-aprovacao" ? 0d
                            : modo == "aprovacao" ? 9d : cenario.EhConceito ? 1d : 7d;
                        Assert.Equal(esperado, nota.NotaConceito);
                        Assert.Equal(cenario.ExigeAprovacao, nota.EmAprovacao);
                        if (modo == "conceito-inexistente") Assert.Equal(string.Empty, nota.ConceitoDescricao);
                    }
                }
            }

            // Somente quando solicitado: medições não integram o tempo de preparação da massa.
            var destino = Environment.GetEnvironmentVariable("SGP_FECHAMENTO_BASELINE_DIR");
            if (string.IsNullOrWhiteSpace(destino)) return;
            Directory.CreateDirectory(destino);
            for (var i = 0; i < 3; i++) await executar();
            var amostras = new List<object>();
            var jsonReferencia = JsonConvert.SerializeObject(resposta);
            for (var i = 0; i < 20; i++)
            {
                medicao.Chamadas.Clear();
                var relogio = Stopwatch.StartNew();
                var atual = await executar();
                relogio.Stop();
                Assert.Equal(jsonReferencia, JsonConvert.SerializeObject(atual));
                amostras.Add(new { totalMs = relogio.Elapsed.TotalMilliseconds, chamadas = medicao.Chamadas.ToArray() });
            }
            var hash = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(jsonReferencia)));
            File.WriteAllText(Path.Combine(destino, $"{modo}-{quantidade}.json"), JsonConvert.SerializeObject(new
            {
                modo, quantidade, ano = DateTimeExtension.HorarioBrasilia().Year,
                escopo = "Servico de aplicacao real e PostgreSQL descartavel; sem HTTP, EOL ou frontend",
                medicao = "Chamadas e tempo dos tres repositorios; nao e contagem global de SQL",
                hashResposta = hash, resposta, amostras
            }, Formatting.Indented));
        }

        [Fact]
        public async Task Deve_buscar_conceitos_distintos_sem_filtrar_vigencia_ou_ativo()
        {
            await Preparar("conceito", 1);
            var conceito = new Conceito { Valor = "X", Descricao = "Historico", Ativo = false,
                InicioVigencia = new DateTime(2000, 1, 1), FimVigencia = new DateTime(2001, 1, 1) };
            Auditar(conceito);
            conceito.Id = await InserirNaBaseAsync(conceito);
            var repo = ServiceProvider.GetRequiredService<IRepositorioConceitoConsulta>();
            var resultado = (await repo.ObterPorIdsAsync(new[] { 1L, conceito.Id, conceito.Id, 999L })).ToArray();
            Assert.Equal(2, resultado.Length);
            foreach (var atual in resultado)
                Assert.Equal(JsonConvert.SerializeObject(repo.ObterPorId(atual.Id)), JsonConvert.SerializeObject(atual));
            Assert.Contains(resultado, c => c.Id == conceito.Id && !c.Ativo && c.Valor == "X");
            Assert.Empty(await repo.ObterPorIdsAsync(Array.Empty<long>()));
            Assert.Empty(await repo.ObterPorIdsAsync(null));
        }

        [Theory]
        [InlineData("nenhum")]
        [InlineData("turma")]
        [InlineData("disciplina")]
        [InlineData("aluno")]
        [InlineData("nota")]
        [InlineData("workflow")]
        [InlineData("conceito")]
        [InlineData("sem-workflow")]
        public async Task Deve_preservar_aprovacao_em_lote_com_chaves_distintas_e_exclusoes(string modo)
        {
            await Preparar("aprovacao", 2);
            if (modo == "turma")
            {
                var item = ObterTodos<FechamentoTurma>().Single();
                item.Excluido = true; await AtualizarNaBase(item);
            }
            if (modo == "disciplina")
            {
                var item = ObterTodos<FechamentoTurmaDisciplina>().Single();
                item.Excluido = true; await AtualizarNaBase(item);
            }
            if (modo == "aluno")
            {
                var item = ObterTodos<FechamentoAluno>().First();
                item.Excluido = true; await AtualizarNaBase(item);
            }
            if (modo == "nota")
            {
                var item = ObterTodos<FechamentoNota>().First();
                item.Excluido = true; await AtualizarNaBase(item);
            }
            if (modo == "workflow" || modo == "conceito")
            {
                var item = ObterTodos<WfAprovacaoNotaFechamento>().First(w => w.Nota == 9);
                if (modo == "workflow") item.Excluido = true;
                else { item.Nota = null; item.ConceitoId = 1; }
                await AtualizarNaBase(item);
            }
            if (modo == "sem-workflow")
            {
                var item = new FechamentoNota { FechamentoAlunoId = 1, DisciplinaId = 138, Nota = 6 };
                Auditar(item); await InserirNaBase(item);
            }
            var repo = ServiceProvider.GetRequiredService<IRepositorioNotasConceitosConsulta>();
            var filtros = new[]
            {
                new NotaEmAprovacaoFechamentoDto { CodigoAluno = "1", TurmaFechamentoId = 1, DisciplinaId = 138 },
                new NotaEmAprovacaoFechamentoDto { CodigoAluno = "2", TurmaFechamentoId = 1, DisciplinaId = 138 },
                new NotaEmAprovacaoFechamentoDto { CodigoAluno = "1", TurmaFechamentoId = 1, DisciplinaId = 1105 },
                new NotaEmAprovacaoFechamentoDto { CodigoAluno = "1", TurmaFechamentoId = 999, DisciplinaId = 138 },
                new NotaEmAprovacaoFechamentoDto { CodigoAluno = "ausente", TurmaFechamentoId = 1, DisciplinaId = 138 }
            };
            var resultados = (await repo.ObterNotasEmAprovacaoAsync(filtros.Concat(filtros).ToArray())).ToArray();
            Assert.Equal(filtros.Length, resultados.Length);
            foreach (var filtro in filtros)
            {
                var atual = Assert.Single(resultados.Where(r => r.CodigoAluno == filtro.CodigoAluno
                    && r.TurmaFechamentoId == filtro.TurmaFechamentoId && r.DisciplinaId == filtro.DisciplinaId));
                Assert.Equal(await repo.ObterNotaEmAprovacao(filtro.CodigoAluno, filtro.DisciplinaId, filtro.TurmaFechamentoId), atual.Nota);
            }
            Assert.Empty(await repo.ObterNotasEmAprovacaoAsync(Array.Empty<NotaEmAprovacaoFechamentoDto>()));
            Assert.Empty(await repo.ObterNotasEmAprovacaoAsync(null));
        }

        [Theory]
        [InlineData("numerica", null)]
        [InlineData("numerica", "professor-teste")]
        [InlineData("sintese", null)]
        [InlineData("sintese", "professor-teste")]
        public async Task Deve_usar_frequencia_da_turma_solicitada_sem_filtrar_professor(string modo, string professor)
        {
            await Preparar(modo, 1);
            var periodo = ObterTodos<PeriodoEscolar>().Single(p => p.Bimestre == 1);
            var frequencia = new SME.SGP.Dominio.FrequenciaAluno("1", TURMA_CODIGO_1, "138", periodo.Id,
                periodo.PeriodoInicio.AddDays(2), periodo.PeriodoFim, 1, 5, 20, 1,
                TipoFrequenciaAluno.PorDisciplina, 0, 15, professor);
            Auditar(frequencia); await InserirNaBase(frequencia);
            await InserirFrequencia("1", periodo, "138", "outra-turma", 19);

            var servico = ServiceProvider.GetRequiredService<IConsultasFechamentoTurmaDisciplina>();
            var resposta = await servico.ObterNotasFechamentoTurmaDisciplina(TURMA_CODIGO_1, 138, 1, 0);
            var aluno = Assert.Single(resposta.Alunos);
            Assert.Equal(5, aluno.QuantidadeFaltas);
            Assert.Equal(1, aluno.QuantidadeCompensacoes);
            Assert.Equal("80,00", aluno.PercentualFrequencia);
            if (modo == "sintese") Assert.Equal(SinteseEnum.Frequente, aluno.SinteseId);
        }

        [Fact]
        public async Task Nao_deve_usar_outra_turma_quando_a_turma_solicitada_nao_tem_frequencia()
        {
            await Preparar("sem-frequencia", 1);
            var periodo = ObterTodos<PeriodoEscolar>().Single(p => p.Bimestre == 1);
            await InserirFrequencia("1", periodo, "138", "outra-turma", 19);
            var servico = ServiceProvider.GetRequiredService<IConsultasFechamentoTurmaDisciplina>();
            var resposta = await servico.ObterNotasFechamentoTurmaDisciplina(TURMA_CODIGO_1, 138, 1, 0);
            var aluno = Assert.Single(resposta.Alunos);
            Assert.Equal(0, aluno.QuantidadeFaltas);
            Assert.Equal(0, aluno.QuantidadeCompensacoes);
            Assert.Equal(string.Empty, aluno.PercentualFrequencia);
        }

        private async Task Preparar(string modo, int quantidade)
        {
            await CriarDadosBase(ObterFiltroNotas(ObterPerfilProfessor(), ANO_3, "138", TipoNota.Nota,
                Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, false));
            if (modo == "sintese")
            {
                var parametro = new ParametrosSistema { Ano = DateTimeExtension.HorarioBrasilia().Year,
                    Nome = "Percentual sintetico", Descricao = "Massa isolada", Ativo = true,
                    Tipo = TipoParametroSistema.CompensacaoAusenciaPercentualRegenciaClasse, Valor = "75" };
                Auditar(parametro); await InserirNaBase(parametro);
            }
            var c = ServiceProvider.GetRequiredService<CenarioFechamento>();
            c.EhConceito = modo.Contains("conceito") || modo.StartsWith("regencia");
            c.ExigeAprovacao = modo.Contains("aprovacao");
            c.Componentes = modo.StartsWith("regencia") ? new long[] { 138, 139, 2, 7, 8 } : new long[] { 138 };
            c.Disciplina = new DisciplinaDto { Id = modo.StartsWith("regencia") ? 1105 : 138,
                Nome = "Componente sintetico", Regencia = modo.StartsWith("regencia"), LancaNota = modo != "sintese" };
            var periodo = ObterTodos<PeriodoEscolar>().Single(p => p.Bimestre == 1);
            var ft = new FechamentoTurma { TurmaId = TURMA_ID_1, PeriodoEscolarId = periodo.Id };
            Auditar(ft); ft.Id = await InserirNaBaseAsync(ft);
            var ftd = new FechamentoTurmaDisciplina { FechamentoTurmaId = ft.Id, DisciplinaId = c.Disciplina.Id };
            Auditar(ftd); ftd.Id = await InserirNaBaseAsync(ftd);
            for (var i = 1; i <= quantidade; i++)
            {
                var codigo = i.ToString();
                c.Alunos.Add(new AlunoPorTurmaResposta { CodigoAluno = codigo, NomeAluno = $"Aluno sintetico {i:D3}",
                    CodigoTurma = int.Parse(TURMA_CODIGO_1), NumeroAlunoChamada = i,
                    CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo, SituacaoMatricula = "Ativo",
                    DataMatricula = periodo.PeriodoInicio, DataSituacao = periodo.PeriodoInicio });
                if (modo != "sintese")
                {
                    var fa = new FechamentoAluno { FechamentoTurmaDisciplinaId = ftd.Id, AlunoCodigo = codigo };
                    Auditar(fa); fa.Id = await InserirNaBaseAsync(fa);
                    foreach (var componente in c.Componentes)
                    {
                        var fn = new FechamentoNota { FechamentoAlunoId = fa.Id, DisciplinaId = componente,
                            ConceitoId = c.EhConceito ? (modo == "conceito-inexistente" ? 999 : 1) : (long?)null,
                            Nota = c.EhConceito ? (double?)null : 7, Excluido = modo == "nota-excluida" };
                        Auditar(fn); fn.Id = await InserirNaBaseAsync(fn);
                        if (c.ExigeAprovacao)
                        {
                            foreach (var valor in new[] { 8d, 9d, 10d })
                            {
                                var wf = new WfAprovacaoNotaFechamento { FechamentoNotaId = fn.Id, Nota = valor, Excluido = valor == 10 };
                                Auditar(wf); await InserirNaBase(wf);
                            }
                        }
                    }
                }
                if (modo != "sem-frequencia")
                {
                    // Dois elegíveis: o maior id deve vencer. Outro aluno/turma/disciplina não deve interferir.
                    await InserirFrequencia(codigo, periodo, c.Disciplina.Id.ToString(), TURMA_CODIGO_1, 8);
                    await InserirFrequencia(codigo, periodo, c.Disciplina.Id.ToString(), TURMA_CODIGO_1, 3);
                    await InserirFrequencia(codigo, periodo, "99999", TURMA_CODIGO_1, 17);
                    if (modo == "outra-turma")
                        await InserirFrequencia(codigo, periodo, c.Disciplina.Id.ToString(), "99999", 19);
                }
            }
        }

        private async Task InserirFrequencia(string aluno, PeriodoEscolar periodo, string disciplina, string turma, int faltas)
        {
            var f = new SME.SGP.Dominio.FrequenciaAluno(aluno, turma, disciplina, periodo.Id, periodo.PeriodoInicio, periodo.PeriodoFim,
                1, faltas, 20, 1, TipoFrequenciaAluno.PorDisciplina, 0, 17, null);
            // Representa uma consolidação anterior à revisão de datas, sem violar a chave única.
            // A consulta atual filtra datas do periodo_escolar, e não as datas gravadas em fa.
            if (faltas == 8) f.PeriodoInicio = periodo.PeriodoInicio.AddDays(1);
            Auditar(f); await InserirNaBase(f);
        }

        private static void Auditar(EntidadeBase entidade)
        {
            entidade.CriadoEm = AuditoriaFixa; entidade.CriadoPor = "Teste sintetico"; entidade.CriadoRF = "0";
        }

        [Fact]
        public async Task Deve_caracterizar_ausencia_de_workflow_e_ausencia_de_linha_de_aprovacao()
        {
            await Preparar("numerica", 1);
            var repo = ServiceProvider.GetRequiredService<IRepositorioNotasConceitosConsulta>();
            // COALESCE na linha existente retorna -1; sem linha, Dapper devolve default(double), zero.
            Assert.Equal(-1d, await repo.ObterNotaEmAprovacao("1", 138, 1));
            Assert.Equal(0d, await repo.ObterNotaEmAprovacao("1", 1105, 1));
        }

        [Fact]
        public async Task Deve_selecionar_frequencia_por_maior_id_entre_disciplinas_e_respeitar_periodo()
        {
            await Preparar("numerica", 1);
            var repo = ServiceProvider.GetRequiredService<IRepositorioFrequenciaAlunoDisciplinaPeriodoConsulta>();
            var periodo = ObterTodos<PeriodoEscolar>().Single(p => p.Bimestre == 1);
            await InserirFrequencia("1", periodo, "138", "99999", 19);
            Assert.Equal(3, repo.ObterPorAlunoDisciplinaData("1", new[] { "138" }, periodo.PeriodoFim, TURMA_CODIGO_1).TotalAusencias);
            Assert.Equal(17, repo.ObterPorAlunoDisciplinaData("1", new[] { "138", "99999" }, periodo.PeriodoFim, TURMA_CODIGO_1).TotalAusencias);
            Assert.Null(repo.ObterPorAlunoDisciplinaData("1", new[] { "138" }, periodo.PeriodoFim.AddDays(1), TURMA_CODIGO_1));
            Assert.Null(repo.ObterPorAlunoDisciplinaData("ausente", new[] { "138" }, periodo.PeriodoFim, TURMA_CODIGO_1));
        }

        [Fact]
        public void Deve_documentar_sobrecarga_usada_pelo_fluxo_antigo_de_frequencia()
        {
            var query = new ObterPorAlunoDisciplinaDataQuery("1", new[] { "138" }, AuditoriaFixa, "turma");
            Assert.Null(query.TurmaId);
            Assert.Equal("turma", query.Professor);
        }

        [Fact]
        public async Task Deve_caracterizar_ordenacao_de_aprovacao_com_nota_sem_workflow()
        {
            await Preparar("aprovacao", 1);
            var repo = ServiceProvider.GetRequiredService<IRepositorioNotasConceitosConsulta>();
            Assert.Equal(9d, await repo.ObterNotaEmAprovacao("1", 138, 1));
            var notaSemWorkflow = new FechamentoNota { FechamentoAlunoId = 1, DisciplinaId = 138, Nota = 6 };
            Auditar(notaSemWorkflow); await InserirNaBase(notaSemWorkflow);
            // ORDER BY w.id DESC coloca NULL antes dos ids. Não corrigir regra durante otimização.
            Assert.Equal(-1d, await repo.ObterNotaEmAprovacao("1", 138, 1));
        }

        [Fact]
        public async Task Deve_obter_lote_com_ultimo_registro_por_aluno_e_ignorar_aluno_sem_frequencia()
        {
            await Preparar("numerica", 2);
            var repo = ServiceProvider.GetRequiredService<IRepositorioFrequenciaAlunoDisciplinaPeriodoConsulta>();
            var periodo = ObterTodos<PeriodoEscolar>().Single(p => p.Bimestre == 1);
            await InserirFrequencia("1", periodo, "138", "outra", 19);
            var retorno = (await repo.ObterUltimasFrequenciasPorAlunosDisciplinasDataAsync(
                new[] { "1", "2", "1", "ausente" }, new[] { "138", "99999" }, periodo.PeriodoFim, TURMA_CODIGO_1)).ToArray();
            Assert.Equal(2, retorno.Length);
            Assert.Equal(2, retorno.Select(f => f.CodigoAluno).Distinct().Count());
            Assert.All(retorno, f => { Assert.Equal(17, f.TotalAusencias); Assert.Equal("99999", f.DisciplinaId); });
        }

        [Theory]
        [InlineData(null, 9)]
        [InlineData("", 9)]
        [InlineData(" ", 9)]
        [InlineData("p1", 5)]
        [InlineData("p2", 9)]
        [InlineData("outro", 3)]
        public async Task Deve_preservar_filtro_de_professor_incluindo_registros_sem_professor(string professor, int faltas)
        {
            await Preparar("numerica", 1);
            var periodo = ObterTodos<PeriodoEscolar>().Single(p => p.Bimestre == 1);
            foreach (var item in new[] { ("p1", 5, 2), ("p2", 9, 3) })
            {
                var f = new SME.SGP.Dominio.FrequenciaAluno("1", TURMA_CODIGO_1, "138", periodo.Id,
                    periodo.PeriodoInicio.AddDays(item.Item3), periodo.PeriodoFim, 1, item.Item2, 20, 1,
                    TipoFrequenciaAluno.PorDisciplina, 0, 17, item.Item1);
                Auditar(f); await InserirNaBase(f);
            }
            var repo = ServiceProvider.GetRequiredService<IRepositorioFrequenciaAlunoDisciplinaPeriodoConsulta>();
            var retorno = await repo.ObterUltimasFrequenciasPorAlunosDisciplinasDataAsync(
                new[] { "1" }, new[] { "138" }, periodo.PeriodoFim, TURMA_CODIGO_1, professor);
            Assert.Equal(faltas, Assert.Single(retorno).TotalAusencias);
        }

        [Fact]
        public async Task Deve_respeitar_limites_inclusivos_do_periodo_e_tipo_de_frequencia_no_lote()
        {
            await Preparar("numerica", 1);
            var periodo = ObterTodos<PeriodoEscolar>().Single(p => p.Bimestre == 1);
            var geral = new SME.SGP.Dominio.FrequenciaAluno("1", TURMA_CODIGO_1, "138", periodo.Id,
                periodo.PeriodoInicio, periodo.PeriodoFim, 1, 19, 20, 1, TipoFrequenciaAluno.Geral, 0, 1, null);
            Auditar(geral); await InserirNaBase(geral);
            var repo = ServiceProvider.GetRequiredService<IRepositorioFrequenciaAlunoDisciplinaPeriodoConsulta>();
            foreach (var data in new[] { periodo.PeriodoInicio, periodo.PeriodoFim })
            {
                var retorno = await repo.ObterUltimasFrequenciasPorAlunosDisciplinasDataAsync(new[] { "1" }, new[] { "138" }, data, TURMA_CODIGO_1);
                Assert.Equal(3, Assert.Single(retorno).TotalAusencias);
            }
            foreach (var data in new[] { periodo.PeriodoInicio.AddTicks(-1), periodo.PeriodoFim.AddDays(1) })
                Assert.Empty(await repo.ObterUltimasFrequenciasPorAlunosDisciplinasDataAsync(new[] { "1" }, new[] { "138" }, data, TURMA_CODIGO_1));
        }

        [Fact]
        public async Task Nao_deve_acessar_banco_ao_consultar_lote_sem_alunos()
        {
            var contexto = new Moq.Mock<ISgpContext>(Moq.MockBehavior.Strict);
            var repo = new RepositorioFrequenciaAlunoDisciplinaPeriodoConsulta(contexto.Object);
            Assert.Empty(await repo.ObterUltimasFrequenciasPorAlunosDisciplinasDataAsync(Array.Empty<string>(), new[] { "138" }, AuditoriaFixa));
            Assert.Empty(await repo.ObterUltimasFrequenciasPorAlunosDisciplinasDataAsync(null, new[] { "138" }, AuditoriaFixa));
            contexto.VerifyNoOtherCalls();
        }
    }

    public class CenarioFechamento
    {
        public List<AlunoPorTurmaResposta> Alunos { get; } = new List<AlunoPorTurmaResposta>();
        public DisciplinaDto Disciplina { get; set; }
        public long[] Componentes { get; set; }
        public bool ExigeAprovacao { get; set; }
        public bool EhConceito { get; set; }
    }

    public class RespostaFixa<TQuery, TResposta> : IRequestHandler<TQuery, TResposta> where TQuery : IRequest<TResposta>
    {
        private readonly Func<TResposta> resposta;
        public RespostaFixa(Func<TResposta> resposta) { this.resposta = resposta; }
        public Task<TResposta> Handle(TQuery request, CancellationToken cancellationToken) => Task.FromResult(resposta());
    }

    public class HttpExternoProibido : IHttpClientFactory
    {
        public HttpClient CreateClient(string name) => throw new InvalidOperationException("HTTP externo proibido no baseline isolado: " + name);
    }
}
