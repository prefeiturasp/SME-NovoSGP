using MediatR;
using Moq;
using Newtonsoft.Json;
using SME.SGP.Aplicacao.Integracoes.Respostas;
using SME.SGP.Aplicacao.Queries;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.ConselhoClasse
{
    // Referência funcional da US 155457. Os cenários preservam a resposta registrada
    // antes da melhoria e agora também protegem a quantidade de consultas do lote.
    public class ObterNotasFrequenciaWorkflowCaracterizacaoTeste
    {
        private readonly Mock<IMediator> mediator = new Mock<IMediator>();
        private readonly ITestOutputHelper output;
        private readonly List<NotaConceitoBimestreComponenteDto> notasConselho = new List<NotaConceitoBimestreComponenteDto>();
        private readonly List<NotaConceitoBimestreComponenteDto> notasFechamento = new List<NotaConceitoBimestreComponenteDto>();
        private readonly Dictionary<long, double?> workflows = new Dictionary<long, double?>();
        private readonly ObterNotasFrequenciaUseCase useCase;
        private const long NotaId = 1001;
        private const long ComponenteId = 101;

        public ObterNotasFrequenciaWorkflowCaracterizacaoTeste(ITestOutputHelper output)
        {
            this.output = output;
            useCase = new ObterNotasFrequenciaUseCase(mediator.Object, Mock.Of<IConsultasPeriodoFechamento>());
        }

        [Theory]
        [InlineData(false, false, 8.5, 8.5, true)]
        [InlineData(true, false, 8.5, 8.5, true)]
        [InlineData(false, true, 3.0, 3.0, true)]
        [InlineData(true, true, 3.0, 3.0, true)]
        [InlineData(false, false, 0.0, 0.0, true)]
        [InlineData(true, false, 0.0, 0.0, true)]
        [InlineData(false, false, null, 6.0, false)]
        [InlineData(true, false, null, 6.0, false)]
        [InlineData(false, true, null, 2.0, false)]
        [InlineData(true, true, null, 2.0, false)]
        [InlineData(false, false, -1.0, 6.0, false)]
        [InlineData(true, false, -1.0, 6.0, false)]
        public async Task Deve_preservar_nota_id_e_sinalizacao_do_workflow(bool regencia, bool conceito,
            double? valorWorkflow, double notaEsperada, bool emAprovacao)
        {
            Preparar(regencia, conceito: conceito);
            notasConselho.Add(Nota(NotaId, ComponenteId, conceito ? null : 6, conceito ? 2 : null));
            workflows[NotaId] = valorWorkflow;

            var retorno = await Executar();

            var nota = Assert.Single(NotasPosConselho(retorno));
            Assert.Equal(NotaId, nota.Id);
            Assert.Equal(notaEsperada, nota.Nota);
            Assert.Equal(emAprovacao, nota.EmAprovacao);
            Assert.True(nota.PodeEditar);
            ValidarEstrutura(retorno, regencia);
            VerificarConsultas(1);
        }

        [Theory]
        [InlineData(false, false)]
        [InlineData(true, false)]
        [InlineData(false, true)]
        [InlineData(true, true)]
        public async Task Deve_usar_fechamento_sem_inventar_id_ou_consultar_workflow(bool regencia, bool possuiFechamento)
        {
            Preparar(regencia);
            if (possuiFechamento)
                notasFechamento.Add(Nota(9001, ComponenteId, 7));

            var retorno = await Executar();

            var nota = Assert.Single(NotasPosConselho(retorno));
            Assert.Null(nota.Id);
            Assert.Equal(possuiFechamento ? (double?)7 : null, nota.Nota);
            Assert.False(nota.EmAprovacao);
            VerificarConsultas(0);
        }

        [Theory]
        [InlineData(false, null, 7.0, false)]
        [InlineData(true, null, 7.0, false)]
        [InlineData(false, 0.0, 0.0, true)]
        [InlineData(true, 0.0, 0.0, true)]
        public async Task Deve_preservar_id_da_nota_sem_valor_ao_usar_fechamento_e_workflow(bool regencia,
            double? valorWorkflow, double notaEsperada, bool emAprovacao)
        {
            Preparar(regencia);
            notasConselho.Add(Nota(NotaId, ComponenteId, null));
            notasFechamento.Add(Nota(9001, ComponenteId, 7));
            workflows[NotaId] = valorWorkflow;

            var retorno = await Executar();

            var nota = Assert.Single(NotasPosConselho(retorno));
            Assert.Equal(NotaId, nota.Id);
            Assert.Equal(notaEsperada, nota.Nota);
            Assert.Equal(emAprovacao, nota.EmAprovacao);
            VerificarConsultas(1);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public async Task Deve_consultar_somente_nota_mais_recente_da_turma_considerada(bool regencia)
        {
            Preparar(regencia);
            notasConselho.Add(Nota(NotaId - 1, ComponenteId, 4));
            notasConselho.Add(Nota(NotaId, ComponenteId, 6));
            var outraTurma = Nota(NotaId + 1, ComponenteId, 9);
            outraTurma.TurmaCodigo = "2";
            notasConselho.Add(outraTurma);
            workflows[NotaId] = 8;

            var retorno = await Executar();

            var nota = Assert.Single(NotasPosConselho(retorno));
            Assert.Equal(NotaId, nota.Id);
            Assert.Equal(8, nota.Nota);
            mediator.Verify(m => m.Send(It.Is<ObterNotasConselhoEmAprovacaoPorIdsQuery>(q => q.IdsConselhoClasseNota.SequenceEqual(new[] { NotaId })),
                It.IsAny<CancellationToken>()), Times.Once);
            VerificarConsultas(1);
        }

        [Theory]
        [InlineData(false, 1)]
        [InlineData(false, 8)]
        [InlineData(false, 16)]
        [InlineData(true, 1)]
        [InlineData(true, 8)]
        [InlineData(true, 16)]
        public async Task Consulta_em_lote_realiza_uma_consulta_para_todas_as_notas(bool regencia, int quantidade)
        {
            Preparar(regencia, quantidade);
            for (var i = 0; i < quantidade; i++)
            {
                notasConselho.Add(Nota(NotaId + i, ComponenteId + i, 6));
                workflows[NotaId + i] = i % 2 == 0 ? (double?)0 : null;
            }

            var retorno = await Executar();

            var notas = NotasPosConselho(retorno).ToArray();
            Assert.Equal(quantidade, notas.Length);
            for (var i = 0; i < quantidade; i++)
            {
                Assert.Equal(NotaId + i, notas[i].Id);
                Assert.Equal(i % 2 == 0 ? 0 : 6, notas[i].Nota);
                Assert.Equal(i % 2 == 0, notas[i].EmAprovacao);
            }
            mediator.Verify(m => m.Send(It.Is<ObterNotasConselhoEmAprovacaoPorIdsQuery>(q =>
                    q.IdsConselhoClasseNota.SequenceEqual(Enumerable.Range(0, quantidade).Select(i => NotaId + i))),
                It.IsAny<CancellationToken>()), Times.Once);
            VerificarConsultas(quantidade);
        }

        [Fact]
        public async Task Referencia_sem_componentes_nao_consulta_workflow()
        {
            Preparar(false, 0);
            var retorno = await Executar();
            Assert.Empty(retorno.NotasConceitos);
            VerificarConsultas(0);
        }

        private void Preparar(bool regencia, int quantidade = 1, bool conceito = false)
        {
            var periodo = new PeriodoEscolar { Id = 1, Bimestre = 1, PeriodoInicio = new DateTime(2026, 2, 5), PeriodoFim = new DateTime(2026, 4, 30) };
            var turma = new Turma { Id = 1, CodigoTurma = "1", AnoLetivo = 2026, Ano = "5", TipoTurma = TipoTurma.Regular,
                ModalidadeCodigo = Modalidade.Fundamental, Ue = new Ue { TipoEscola = Dominio.TipoEscola.EMEF } };
            var componentes = Enumerable.Range(0, quantidade).Select(i => new DisciplinaDto
            {
                Id = ComponenteId + i, CodigoComponenteCurricular = ComponenteId + i, Nome = $"Componente {i:D2}",
                GrupoMatrizId = 1, GrupoMatrizNome = "Base nacional comum", LancaNota = true, TurmaCodigo = "1"
            }).Reverse().ToArray(); // Entrada invertida para também caracterizar a ordenação da resposta.
            var disciplinas = regencia ? new[] { new DisciplinaDto { Id = 999, CodigoComponenteCurricular = 999,
                Nome = "Regência", GrupoMatrizId = 1, GrupoMatrizNome = "Base nacional comum", LancaNota = true,
                Regencia = true, TurmaCodigo = "1" } } : componentes;
            var areas = new[] { new AreaDoConhecimentoDto { Id = 1, Nome = "Área" } };

            Responder<ObterTurmaPorCodigoQuery, Turma>(turma);
            Responder<ObterFechamentoTurmaPorIdAlunoCodigoQuery, FechamentoTurma>(new FechamentoTurma(turma, periodo));
            Responder<ObterUltimoPeriodoEscolarPorAnoModalidadeSemestreQuery, PeriodoEscolar>(periodo);
            Responder<ObterTipoNotaPorTurmaQuery, TipoNota>(conceito ? TipoNota.Conceito : TipoNota.Nota);
            Responder<ObterTipoCalendarioPorAnoLetivoEModalidadeQuery, Dominio.TipoCalendario>(new Dominio.TipoCalendario { Id = 1, AnoLetivo = 2026 });
            Responder<ObterTurmaItinerarioEnsinoMedioQuery, IEnumerable<TurmaItinerarioEnsinoMedioDto>>(Array.Empty<TurmaItinerarioEnsinoMedioDto>());
            Responder<ObterTodosAlunosNaTurmaQuery, IEnumerable<AlunoPorTurmaResposta>>(new[] { new AlunoPorTurmaResposta {
                CodigoAluno = "1", CodigoTurma = 1, CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo,
                DataMatricula = periodo.PeriodoInicio, DataSituacao = periodo.PeriodoFim } });
            Responder<ObterTurmaCodigosAlunoPorAnoLetivoAlunoTipoTurmaQuery, string[]>(new[] { "1" });
            Responder<ObterConselhoClasseIdsPorTurmaEPeriodoQuery, long[]>(new long[] { 1 });
            Responder<ObterPeriodosEscolaresPorTipoCalendarioQuery, IEnumerable<PeriodoEscolar>>(new[] { periodo });
            Responder<ObterPeriodosEscolaresPorTipoCalendarioIdQuery, IEnumerable<PeriodoEscolar>>(new[] { periodo });
            // Frequência sem registros nesta fixture: o foco é a composição das notas e do workflow.
            mediator.Setup(m => m.Send(It.IsAny<ObterTurmasComMatriculasValidasPeriodoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((ObterTurmasComMatriculasValidasPeriodoQuery q, CancellationToken _) => q.ConsideraPeriodoFechamento ? new[] { "1" } : Array.Empty<string>());
            Responder<ObterFrequenciasRegistradasPorTurmasComponentesCurricularesQuery, IEnumerable<RegistroFrequenciaAlunoBimestreDto>>(Array.Empty<RegistroFrequenciaAlunoBimestreDto>());
            Responder<ObterPeriodoEscolarPorCalendarioEDataQuery, PeriodoEscolar>(periodo);
            Responder<ObterTurmasConsideradasNoConselhoQuery, List<string>>(new List<string> { "1" });
            Responder<ObterConselhoClasseNotasAlunoQuery, IEnumerable<NotaConceitoBimestreComponenteDto>>(notasConselho);
            Responder<ObterNotasFechamentosPorTurmasCodigosBimestreQuery, IEnumerable<NotaConceitoBimestreComponenteDto>>(notasFechamento);
            Responder<ObterDadosAlunosQuery, IEnumerable<AlunoDadosBasicosDto>>(new[] { new AlunoDadosBasicosDto {
                CodigoEOL = "1", SituacaoCodigo = SituacaoMatriculaAluno.Ativo, DataMatricula = periodo.PeriodoInicio } });
            Responder<ObterUsuarioLogadoQuery, Usuario>(new Usuario());
            Responder<ObterComponentesCurricularesPorTurmasCodigoQuery, IEnumerable<DisciplinaDto>>(disciplinas);
            Responder<ObterComponentesCurricularesPorIdsUsuarioLogadoQuery, IEnumerable<DisciplinaDto>>(disciplinas);
            Responder<ObterAreasConhecimentoQuery, IEnumerable<AreaDoConhecimentoDto>>(areas);
            Responder<ObterOrdenacaoAreasConhecimentoQuery, IEnumerable<ComponenteCurricularGrupoAreaOrdenacaoDto>>(Array.Empty<ComponenteCurricularGrupoAreaOrdenacaoDto>());
            Responder<MapearAreasConhecimentoQuery, IEnumerable<IGrouping<(string Nome, int? Ordem, long Id), AreaDoConhecimentoDto>>>(areas.GroupBy(a => (a.Nome, a.Ordem, a.Id)));
            Responder<ObterComponentesAreasConhecimentoQuery, IEnumerable<DisciplinaDto>>(disciplinas);
            Responder<ObterComponentesRegenciaPorAnoQuery, IEnumerable<DisciplinaResposta>>(componentes.Select(c => new DisciplinaResposta {
                CodigoComponenteCurricular = c.CodigoComponenteCurricular, Nome = c.Nome, LancaNota = true }));
            Responder<ObterComponenteRegistraFrequenciaQuery, bool>(true);
            Responder<ObterConselhoClasseAlunoIdQuery, long>(1);
            Responder<VerificaSePodeEditarNotaQuery, bool>(true);
            Responder<ObterParametrosArredondamentoNotaPorDataAvaliacaoQuery, NotaParametroDto>(new NotaParametroDto { Minima = 0, Maxima = 10, Incremento = 0.5 });
            mediator.Setup(m => m.Send(It.IsAny<ObterNotasConselhoEmAprovacaoPorIdsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((ObterNotasConselhoEmAprovacaoPorIdsQuery q, CancellationToken _) =>
                    q.IdsConselhoClasseNota
                        .Where(id => workflows.TryGetValue(id, out var valor) && valor.HasValue)
                        .Select(id => new ConselhoClasseNotaAprovacaoDto { Id = id, NotaEmAprovacao = workflows[id].Value }));
        }

        private void Responder<TQuery, TResponse>(TResponse resposta) where TQuery : IRequest<TResponse>
            => mediator.Setup(m => m.Send(It.IsAny<TQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(resposta);

        private static NotaConceitoBimestreComponenteDto Nota(long id, long componente, double? nota, long? conceito = null)
            => new NotaConceitoBimestreComponenteDto { ConselhoClasseId = 1, ConselhoClasseNotaId = id,
                ComponenteCurricularCodigo = componente, AlunoCodigo = "1", TurmaCodigo = "1", Bimestre = 1, Nota = nota, ConceitoId = conceito };

        private async Task<ConselhoClasseAlunoNotasConceitosRetornoDto> Executar()
        {
            var retorno = await useCase.Executar(new ConselhoClasseNotasFrequenciaDto(1, 1, "1", "1", 1, false));
            output.WriteLine("Resposta de referência (dados sintéticos): " + JsonConvert.SerializeObject(retorno));
            return retorno;
        }

        private static IEnumerable<NotaPosConselhoDto> NotasPosConselho(ConselhoClasseAlunoNotasConceitosRetornoDto retorno)
            => retorno.NotasConceitos.SelectMany(g => g.ComponenteRegencia != null
                ? g.ComponenteRegencia.ComponentesCurriculares.Select(c => c.NotaPosConselho)
                : g.ComponentesCurriculares.Select(c => c.NotaPosConselho));

        private static void ValidarEstrutura(ConselhoClasseAlunoNotasConceitosRetornoDto retorno, bool regencia)
        {
            Assert.True(retorno.TemConselhoClasseAluno);
            Assert.True(retorno.PodeEditarNota);
            Assert.Equal(0, retorno.DadosArredondamento.Minima);
            Assert.Equal(10, retorno.DadosArredondamento.Maxima);
            Assert.Equal(0.5, retorno.DadosArredondamento.Incremento);
            var grupo = Assert.Single(retorno.NotasConceitos);
            Assert.Equal("Base nacional comum", grupo.GrupoMatriz);
            Assert.True(grupo.DesabilitarCampos);
            List<NotaBimestreDto> notas;
            if (regencia)
            {
                Assert.Empty(grupo.ComponentesCurriculares);
                Assert.Equal(0, grupo.ComponenteRegencia.QuantidadeAulas);
                Assert.Equal(0, grupo.ComponenteRegencia.Faltas);
                Assert.Equal(0, grupo.ComponenteRegencia.AusenciasCompensadas);
                Assert.Equal("", grupo.ComponenteRegencia.Frequencia);
                var componente = Assert.Single(grupo.ComponenteRegencia.ComponentesCurriculares);
                Assert.Equal(ComponenteId, componente.CodigoComponenteCurricular);
                Assert.Equal("Componente 00", componente.Nome);
                notas = componente.NotasFechamentos;
            }
            else
            {
                Assert.Null(grupo.ComponenteRegencia);
                var componente = Assert.Single(grupo.ComponentesCurriculares);
                Assert.Equal(ComponenteId, componente.CodigoComponenteCurricular);
                Assert.Equal("Componente 00", componente.Nome);
                Assert.Equal("0", componente.Aulas);
                Assert.Equal(0, componente.QuantidadeAulas);
                Assert.Equal(0, componente.Faltas);
                Assert.Equal(0, componente.AusenciasCompensadas);
                Assert.Null(componente.Frequencia);
                notas = componente.NotasFechamentos;
            }
            var fechamento = Assert.Single(notas);
            Assert.Equal(1, fechamento.Bimestre);
            Assert.Null(fechamento.NotaConceito);
        }

        private void VerificarConsultas(int quantidade)
        {
            var consultasEsperadas = quantidade > 0 ? 1 : 0;
            mediator.Verify(m => m.Send(It.IsAny<ObterNotasConselhoEmAprovacaoPorIdsQuery>(), It.IsAny<CancellationToken>()), Times.Exactly(consultasEsperadas));
            mediator.Verify(m => m.Send(It.IsAny<ObterNotaConselhoEmAprovacaoQuery>(), It.IsAny<CancellationToken>()), Times.Never);
            output.WriteLine($"Após a melhoria: {quantidade} nota(s), {consultasEsperadas} consulta(s) em lote e zero consulta escalar pelo MediatR; sem acesso ao banco.");
        }
    }
}
