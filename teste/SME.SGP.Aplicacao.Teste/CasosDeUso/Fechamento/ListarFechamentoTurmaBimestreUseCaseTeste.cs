using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Fechamento
{
    public class ListarFechamentoTurmaBimestreUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly ListarFechamentoTurmaBimestreUseCase useCase;

        public ListarFechamentoTurmaBimestreUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new ListarFechamentoTurmaBimestreUseCase(mediatorMock.Object);

            ConfigurarMocksPadrao();
        }

        [Fact]
        public async Task Nao_Deve_Consultar_Notas_Em_Aprovacao_Quando_Turma_Nao_Exige_Aprovacao()
        {
            mediatorMock
                .Setup(m => m.Send(It.IsAny<ExigeAprovacaoDeNotaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            var (alunos, periodoAtual, dto) = MontarCenarioPadrao();

            var resultado = await useCase.RetornaListagemAlunosFechamentoBimestreEspecifico(alunos, periodoAtual, null, dto);

            mediatorMock.Verify(m => m.Send(It.IsAny<ObterNotasEmAprovacaoQuery>(), It.IsAny<CancellationToken>()), Times.Never);

            var notaAluno = resultado.Single().NotasConceitoBimestre.Single();
            Assert.False(notaAluno.EmAprovacao);
            Assert.Equal(7.5, notaAluno.NotaConceito);
        }

        [Fact]
        public async Task Deve_Consultar_Notas_Em_Aprovacao_Quando_Turma_Exige_Aprovacao()
        {
            mediatorMock
                .Setup(m => m.Send(It.IsAny<ExigeAprovacaoDeNotaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterNotasEmAprovacaoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<NotaEmAprovacaoFechamentoDto>
                {
                    new NotaEmAprovacaoFechamentoDto
                    {
                        CodigoAluno = "111",
                        TurmaFechamentoId = 9001,
                        DisciplinaId = 101,
                        Nota = 8.5
                    }
                });

            var (alunos, periodoAtual, dto) = MontarCenarioPadrao();

            var resultado = await useCase.RetornaListagemAlunosFechamentoBimestreEspecifico(alunos, periodoAtual, null, dto);

            mediatorMock.Verify(m => m.Send(It.IsAny<ObterNotasEmAprovacaoQuery>(), It.IsAny<CancellationToken>()), Times.Once);

            var notaAluno = resultado.Single().NotasConceitoBimestre.Single();
            Assert.True(notaAluno.EmAprovacao);
            Assert.Equal(8.5, notaAluno.NotaConceito);
        }

        [Fact]
        public async Task Deve_Consultar_Plano_Aee_Em_Lote_E_Preencher_EhAtendidoAEE_Corretamente_No_Fechamento_Final()
        {
            var turma = new Turma
            {
                Id = 1,
                CodigoTurma = "1234",
                AnoLetivo = 2020,
                ModalidadeCodigo = Modalidade.Fundamental,
                Ue = new Ue(),
                TipoTurno = 1
            };

            var disciplina = new DisciplinaDto
            {
                Id = 101,
                CodigoComponenteCurricular = 101,
                Nome = "Matemática",
                Regencia = false
            };

            var periodoEscolar = new PeriodoEscolar
            {
                Bimestre = 4,
                PeriodoInicio = new System.DateTime(2020, 10, 1),
                PeriodoFim = new System.DateTime(2020, 12, 20)
            };

            var alunos = new List<AlunoPorTurmaResposta>
            {
                new AlunoPorTurmaResposta { CodigoAluno = "111", NomeAluno = "Aluno Com AEE" },
                new AlunoPorTurmaResposta { CodigoAluno = "222", NomeAluno = "Aluno Sem AEE" }
            };

            var usuario = new Usuario { PerfilAtual = Perfis.PERFIL_DIRETOR };

            var dto = new ListagemAlunosFechamentoDto(
                Enumerable.Empty<FechamentoTurmaDisciplina>(),
                turma,
                "101",
                disciplina,
                new List<PeriodoEscolar> { periodoEscolar },
                usuario,
                Enumerable.Empty<string>());

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterTipoCalendarioIdPorTurmaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1L);

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterFechamentoTurmaDisciplinaPorTurmaIdDisciplinasIdBimestreQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Enumerable.Empty<FechamentoTurmaDisciplina>());

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterFrequenciaGeralIndexadaPorAlunosQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Dictionary<string, FrequenciaAluno>());

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterMarcadorAlunoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((MarcadorFrequenciaDto)null);

            mediatorMock
                .Setup(m => m.Send(It.IsAny<VerificaPlanosAEEPorCodigosAlunosEAnoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<PlanoAEEResumoDto> { new PlanoAEEResumoDto { CodigoAluno = "111" } });

            var resultado = await useCase.RetornaListagemAlunosFechamentoFinal(alunos, new List<DisciplinaDto> { disciplina }, new NotaTipoValor { TipoNota = TipoNota.Nota }, dto);

            mediatorMock.Verify(m => m.Send(It.IsAny<VerificaPlanosAEEPorCodigosAlunosEAnoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            mediatorMock.Verify(m => m.Send(It.IsAny<VerificaEstudantePossuiPlanoAEEPorCodigoEAnoQuery>(), It.IsAny<CancellationToken>()), Times.Never);

            Assert.True(resultado.First(a => a.CodigoAluno == "111").EhAtendidoAEE);
            Assert.False(resultado.First(a => a.CodigoAluno == "222").EhAtendidoAEE);
        }

        private (List<AlunoPorTurmaResposta> alunos, PeriodoEscolar periodoAtual, ListagemAlunosFechamentoDto dto) MontarCenarioPadrao()
        {
            var turma = new Turma
            {
                Id = 1,
                CodigoTurma = "1234",
                AnoLetivo = 2020,
                ModalidadeCodigo = Modalidade.Fundamental,
                Ue = new Ue(),
                TipoTurno = 1
            };

            var disciplina = new DisciplinaDto
            {
                Id = 101,
                CodigoComponenteCurricular = 101,
                Nome = "Matemática",
                Regencia = false
            };

            var periodoAtual = new PeriodoEscolar
            {
                Bimestre = 1,
                PeriodoInicio = new System.DateTime(2020, 2, 1),
                PeriodoFim = new System.DateTime(2020, 4, 30)
            };

            var alunos = new List<AlunoPorTurmaResposta>
            {
                new AlunoPorTurmaResposta { CodigoAluno = "111", NomeAluno = "Fulano de Tal" }
            };

            var fechamentoTurmaDisciplina = new FechamentoTurmaDisciplina
            {
                Id = 501,
                FechamentoTurmaId = 9001
            };
            fechamentoTurmaDisciplina.FechamentoAlunos.Add(new FechamentoAluno
            {
                FechamentoTurmaDisciplinaId = 501,
                AlunoCodigo = "111"
            });

            var usuario = new Usuario { PerfilAtual = Perfis.PERFIL_DIRETOR };

            var dto = new ListagemAlunosFechamentoDto(
                new List<FechamentoTurmaDisciplina> { fechamentoTurmaDisciplina },
                turma,
                "101",
                disciplina,
                new List<PeriodoEscolar> { periodoAtual },
                usuario,
                Enumerable.Empty<string>());

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterNotaBimestrePorCodigosAlunosIdsFechamentoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<FechamentoNotaDto>
                {
                    new FechamentoNotaDto
                    {
                        CodigoAluno = "111",
                        FechamentoId = 501,
                        DisciplinaId = 101,
                        Nota = 7.5
                    }
                });

            return (alunos, periodoAtual, dto);
        }

        private void ConfigurarMocksPadrao()
        {
            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterAlunosAtivosTurmaProgramaPapEolQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Enumerable.Empty<AlunosTurmaProgramaPapDto>());

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterFrequenciaAlunosPorTurmaDisciplinaEPeriodoEscolarQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Enumerable.Empty<FrequenciaAluno>());

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterPeriodoFechamentoAnoAnteriorPorTurmaBimestreQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new PeriodoFechamentoVigenteDto());

            mediatorMock
                .Setup(m => m.Send(It.IsAny<VerificaPlanosAEEPorCodigosAlunosEAnoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Enumerable.Empty<PlanoAEEResumoDto>());

            mediatorMock
                .Setup(m => m.Send(It.IsAny<TurmaEmPeriodoFechamentoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterComponenteLancaNotaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);
        }
    }
}
