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
    public class NotasConselhoUS155461RetornoCaracterizacaoTeste
    {
        private readonly Mock<IMediator> mediator = new Mock<IMediator>();
        private readonly ITestOutputHelper output;
        private readonly List<NotaConceitoBimestreComponenteDto> notasConselho = new List<NotaConceitoBimestreComponenteDto>();
        private readonly List<NotaConceitoBimestreComponenteDto> notasFechamento = new List<NotaConceitoBimestreComponenteDto>();

        public NotasConselhoUS155461RetornoCaracterizacaoTeste(ITestOutputHelper output) => this.output = output;

        public static IEnumerable<object[]> CenariosAprovacao()
        {
            foreach (var regencia in new[] { false, true })
                foreach (var final in new[] { false, true })
                {
                    yield return new object[] { regencia, final, false, 8.5, 8.5, true };
                    yield return new object[] { regencia, final, true, 3.0, 3.0, true };
                    yield return new object[] { regencia, final, false, 0.0, 0.0, true };
                    yield return new object[] { regencia, final, false, null, 6.0, false };
                    yield return new object[] { regencia, final, true, null, 2.0, false };
                    yield return new object[] { regencia, final, false, -1.0, 6.0, false };
                }
        }

        [Theory]
        [MemberData(nameof(CenariosAprovacao))]
        public async Task Deve_preservar_nota_id_edicao_e_estado_de_aprovacao_no_retorno_publico(
            bool regencia, bool final, bool conceito, double? workflow, double valorEsperado, bool emAprovacao)
        {
            Preparar(regencia, final, conceito);
            notasConselho.Add(Nota(1001, conceito ? null : 6, conceito ? 2 : null, final ? null : 1));
            mediator.Setup(m => m.Send(It.Is<ObterNotaConselhoEmAprovacaoQuery>(q => q.ConselhoClasseNotaId == 1001),
                It.IsAny<CancellationToken>())).ReturnsAsync(workflow);
            var retorno = await Executar(final);
            var pos = Assert.Single(NotasPos(retorno));
            Assert.Equal(1001L, pos.Id);
            Assert.Equal(valorEsperado, pos.Nota);
            Assert.Equal(emAprovacao, pos.EmAprovacao);
            Assert.True(pos.PodeEditar);
            Assert.True(retorno.TemConselhoClasseAluno);
            Assert.True(retorno.PodeEditarNota);
            var grupo = Assert.Single(retorno.NotasConceitos);
            Assert.Equal("Base nacional comum", grupo.GrupoMatriz);
            if (regencia)
            {
                Assert.Empty(grupo.ComponentesCurriculares);
                Assert.Equal(101L, Assert.Single(grupo.ComponenteRegencia.ComponentesCurriculares).CodigoComponenteCurricular);
            }
            else
            {
                Assert.Null(grupo.ComponenteRegencia);
                Assert.Equal(101L, Assert.Single(grupo.ComponentesCurriculares).CodigoComponenteCurricular);
            }
        }

        [Theory]
        [InlineData(false, false)]
        [InlineData(true, false)]
        [InlineData(false, true)]
        [InlineData(true, true)]
        public async Task Deve_exibir_fechamento_sem_atribuir_id_de_conselho_quando_nao_ha_conselho(bool regencia, bool final)
        {
            Preparar(regencia, final, false);
            notasFechamento.Add(Nota(0, 7, null, final ? null : 1));
            var retorno = await Executar(final);
            var pos = Assert.Single(NotasPos(retorno));
            Assert.Null(pos.Id);
            Assert.Equal(7, pos.Nota);
            Assert.False(pos.EmAprovacao);
            Assert.True(pos.PodeEditar);
        }

        private void Preparar(bool regencia, bool final, bool conceito)
        {
            var periodo = new PeriodoEscolar { Id = 1, Bimestre = 1, PeriodoInicio = new DateTime(2026, 2, 5), PeriodoFim = new DateTime(2026, 4, 30) };
            var turma = new Turma
            {
                Id = 1,
                CodigoTurma = "1",
                AnoLetivo = 2026,
                Ano = "5",
                TipoTurma = TipoTurma.Regular,
                ModalidadeCodigo = Modalidade.Fundamental,
                Ue = new Ue { TipoEscola = Dominio.TipoEscola.EMEF }
            };
            var componente = new DisciplinaDto
            {
                Id = 101,
                CodigoComponenteCurricular = 101,
                Nome = "Componente sintético",
                GrupoMatrizId = 1,
                GrupoMatrizNome = "Base nacional comum",
                LancaNota = true,
                TurmaCodigo = "1"
            };
            var disciplinas = regencia ? new[] { new DisciplinaDto { Id=999,CodigoComponenteCurricular=999,Nome="Regência",
                GrupoMatrizId=1,GrupoMatrizNome="Base nacional comum",LancaNota=true,Regencia=true,TurmaCodigo="1" } } : new[] { componente };
            var areas = new[] { new AreaDoConhecimentoDto { Id = 1, Nome = "Área sintética" } };
            Responder<ObterTurmaPorCodigoQuery, Turma>(turma);
            Responder<ObterFechamentoTurmaPorIdAlunoCodigoQuery, FechamentoTurma>(new FechamentoTurma
            {
                Id = 1,
                Turma = turma,
                TurmaId = 1,
                PeriodoEscolar = final ? null : periodo,
                PeriodoEscolarId = final ? null : 1
            });
            Responder<ObterUltimoPeriodoEscolarPorAnoModalidadeSemestreQuery, PeriodoEscolar>(periodo);
            Responder<ObterTipoNotaPorTurmaQuery, TipoNota>(conceito ? TipoNota.Conceito : TipoNota.Nota);
            Responder<ObterTipoCalendarioPorAnoLetivoEModalidadeQuery, Dominio.TipoCalendario>(new Dominio.TipoCalendario { Id = 1, AnoLetivo = 2026 });
            Responder<ObterTurmaItinerarioEnsinoMedioQuery, IEnumerable<TurmaItinerarioEnsinoMedioDto>>(Array.Empty<TurmaItinerarioEnsinoMedioDto>());
            Responder<ObterTodosAlunosNaTurmaQuery, IEnumerable<AlunoPorTurmaResposta>>(new[] { new AlunoPorTurmaResposta {
                CodigoAluno="1",CodigoTurma=1,CodigoSituacaoMatricula=SituacaoMatriculaAluno.Ativo,
                DataMatricula=periodo.PeriodoInicio,DataSituacao=periodo.PeriodoFim } });
            Responder<ObterTurmaCodigosAlunoPorAnoLetivoAlunoTipoTurmaQuery, string[]>(new[] { "1" });
            Responder<ObterConselhoClasseIdsPorTurmaEPeriodoQuery, long[]>(new long[] { 1 });
            Responder<ObterPeriodosEscolaresPorTipoCalendarioQuery, IEnumerable<PeriodoEscolar>>(new[] { periodo });
            Responder<ObterPeriodosEscolaresPorTipoCalendarioIdQuery, IEnumerable<PeriodoEscolar>>(new[] { periodo });
            mediator.Setup(m => m.Send(It.IsAny<ObterTurmasComMatriculasValidasPeriodoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((ObterTurmasComMatriculasValidasPeriodoQuery q, CancellationToken _) =>
                    q.ConsideraPeriodoFechamento ? new[] { "1" } : Array.Empty<string>());
            Responder<ObterFrequenciasRegistradasPorTurmasComponentesCurricularesQuery, IEnumerable<RegistroFrequenciaAlunoBimestreDto>>(Array.Empty<RegistroFrequenciaAlunoBimestreDto>());
            Responder<ObterPeriodoEscolarPorCalendarioEDataQuery, PeriodoEscolar>(periodo);
            Responder<ObterTurmasConsideradasNoConselhoQuery, List<string>>(new List<string> { "1" });
            Responder<ObterConselhoClasseNotasAlunoQuery, IEnumerable<NotaConceitoBimestreComponenteDto>>(notasConselho);
            Responder<ObterNotasFechamentosPorTurmasCodigosBimestreQuery, IEnumerable<NotaConceitoBimestreComponenteDto>>(notasFechamento);
            Responder<ObterNotasFinaisBimestresAlunoQuery, IEnumerable<NotaConceitoBimestreComponenteDto>>(notasFechamento);
            Responder<ObterDadosAlunosQuery, IEnumerable<AlunoDadosBasicosDto>>(new[] { new AlunoDadosBasicosDto {
                CodigoEOL="1",SituacaoCodigo=SituacaoMatriculaAluno.Ativo,DataMatricula=periodo.PeriodoInicio } });
            Responder<ObterUsuarioLogadoQuery, Usuario>(new Usuario());
            Responder<ObterComponentesCurricularesPorTurmasCodigoQuery, IEnumerable<DisciplinaDto>>(disciplinas);
            Responder<ObterComponentesCurricularesPorIdsUsuarioLogadoQuery, IEnumerable<DisciplinaDto>>(disciplinas);
            Responder<ObterAreasConhecimentoQuery, IEnumerable<AreaDoConhecimentoDto>>(areas);
            Responder<ObterOrdenacaoAreasConhecimentoQuery, IEnumerable<ComponenteCurricularGrupoAreaOrdenacaoDto>>(Array.Empty<ComponenteCurricularGrupoAreaOrdenacaoDto>());
            Responder<MapearAreasConhecimentoQuery, IEnumerable<IGrouping<(string Nome, int? Ordem, long Id), AreaDoConhecimentoDto>>>(areas.GroupBy(a => (a.Nome, a.Ordem, a.Id)));
            Responder<ObterComponentesAreasConhecimentoQuery, IEnumerable<DisciplinaDto>>(disciplinas);
            Responder<ObterComponentesRegenciaPorAnoQuery, IEnumerable<DisciplinaResposta>>(new[] { new DisciplinaResposta {
                CodigoComponenteCurricular=101,Nome=componente.Nome,LancaNota=true } });
            Responder<ObterComponenteRegistraFrequenciaQuery, bool>(true);
            Responder<ObterConselhoClasseAlunoIdQuery, long>(1);
            Responder<VerificaSePodeEditarNotaQuery, bool>(true);
            Responder<ObterParametrosArredondamentoNotaPorDataAvaliacaoQuery, NotaParametroDto>(new NotaParametroDto { Minima = 0, Maxima = 10, Incremento = 0.5 });
        }

        private void Responder<TQuery, TResponse>(TResponse resposta) where TQuery : IRequest<TResponse>
            => mediator.Setup(m => m.Send(It.IsAny<TQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(resposta);

        private static NotaConceitoBimestreComponenteDto Nota(long id, double? valor, long? conceito, int? bimestre)
            => new NotaConceitoBimestreComponenteDto
            {
                ConselhoClasseId = id > 0 ? 1 : 0,
                ConselhoClasseNotaId = id,
                ComponenteCurricularCodigo = 101,
                AlunoCodigo = "1",
                TurmaCodigo = "1",
                Bimestre = bimestre,
                Nota = valor,
                ConceitoId = conceito
            };

        private async Task<ConselhoClasseAlunoNotasConceitosRetornoDto> Executar(bool final)
        {
            var useCase = new ObterNotasFrequenciaUseCase(mediator.Object, Mock.Of<IConsultasPeriodoFechamento>());
            var retorno = await useCase.Executar(new ConselhoClasseNotasFrequenciaDto(1, 1, "1", "1", final ? 0 : 1, false));
            output.WriteLine("Resposta anterior à otimização, dados sintéticos: " + JsonConvert.SerializeObject(retorno));
            return retorno;
        }

        private static IEnumerable<NotaPosConselhoDto> NotasPos(ConselhoClasseAlunoNotasConceitosRetornoDto retorno)
            => retorno.NotasConceitos.SelectMany(g => g.ComponenteRegencia != null
                ? g.ComponenteRegencia.ComponentesCurriculares.Select(c => c.NotaPosConselho)
                : g.ComponentesCurriculares.Select(c => c.NotaPosConselho));
    }
}
