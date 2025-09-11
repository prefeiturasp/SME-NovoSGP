using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
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
        }

        [Fact]
        public async Task Executar_Turma_Na_Encontrada_Deve_Lancar_Negocio_Exception()
        {
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaComUeEDrePorCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Turma)null);

            var ex = await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar("turma123", 1, 1, null));
            Assert.Equal("Não foi possível localizar a turma.", ex.Message);
        }

        [Fact]
        public async Task Executar_Alunos_Nao_Encontrados_Deve_Lancar_Negocio_Exception()
        {
            var turma = new Turma
            {
                Id = 1,
                AnoLetivo = 2025,
                CodigoTurma = "turma123"
            };

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaComUeEDrePorCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(turma);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterAlunosPorTurmaEAnoLetivoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<AlunoPorTurmaResposta>());

            var ex = await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar("turma123", 1, 1, null));
            Assert.Equal("Não foi encontrado alunos para a turma informada", ex.Message);
        }

        [Fact]
        public async Task Executar_Tipo_Nota_Turma_Nao_Encontrada_Deve_Lancar_Negocio_Exception()
        {
            var turma = new Turma
            {
                Id = 1,
                AnoLetivo = 2025,
                CodigoTurma = "turma123"
            };

            var alunos = new List<AlunoPorTurmaResposta>
        {
            new AlunoPorTurmaResposta
            {
              Ano = 2025,
        CodigoAluno = "123",
        CodigoComponenteCurricular = 1,
        CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo,
        CodigoTurma = 1,
        DataNascimento = new DateTime(2010, 1, 1),
        DataSituacao = DateTime.Today,
        DataMatricula = DateTime.Today.AddMonths(-2),
        EscolaTransferencia = string.Empty,
        NomeAluno = "Aluno Teste",
        NomeSocialAluno = null, // ou string.Empty
        NumeroAlunoChamada = 1,
        ParecerConclusivo = null,
        PossuiDeficiencia = false,
        SituacaoMatricula = "Ativo",
        Transferencia_Interna = false,
        TurmaEscola = string.Empty,
        TurmaRemanejamento = null,
        TurmaTransferencia = null,
        NomeResponsavel = "Responsável Teste",
        TipoResponsavel = "Pai",
        CelularResponsavel = "11999999999",
        DataAtualizacaoContato = DateTime.Today,
        CodigoEscola = "1234567",
        CodigoTipoTurma = 1,
        DataAtualizacaoTabela = DateTime.Today
            }
        };

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaComUeEDrePorCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(turma);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterAlunosPorTurmaEAnoLetivoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(alunos);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterNotaTipoValorPorTurmaIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((NotaTipoValor)null);

            var ex = await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar("turma123", 1, 1, null));
            Assert.Equal("Não foi possível localizar o tipo de nota para esta turma.", ex.Message);
        }

        [Fact]
        public async Task Executar_Com_Dados_Validos_Deve_Retornar_Fechamento_Nota_Conceito_TurmaDto()
        {
            var turma = new Turma
            {
                Id = 1,
                AnoLetivo = 2025,
                CodigoTurma = "turma123",
                ModalidadeCodigo = Modalidade.Fundamental
            };

            var alunos = new List<AlunoPorTurmaResposta>
        {
        new AlunoPorTurmaResposta
            {
                Ano = 2025,
                CodigoAluno = "123",
                CodigoComponenteCurricular = 1,
                CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo,
                CodigoTurma = 1,
                DataNascimento = new DateTime(2010, 5, 10),
                DataSituacao = DateTime.Today,
                DataMatricula = DateTime.Today.AddMonths(-6),
                EscolaTransferencia = "Escola X",
                NomeAluno = "Aluno Teste",
                NomeSocialAluno = null,
                NumeroAlunoChamada = 1,
                ParecerConclusivo = null,
                PossuiDeficiencia = false,
                SituacaoMatricula = "Ativo",
                Transferencia_Interna = false,
                TurmaEscola = "Turma X",
                TurmaRemanejamento = null,
                TurmaTransferencia = null,
                NomeResponsavel = "Responsável Teste",
                TipoResponsavel = "Mãe",
                CelularResponsavel = "11999999999",
                DataAtualizacaoContato = DateTime.Today,
                CodigoEscola = "123456",
                CodigoTipoTurma = 1,
                DataAtualizacaoTabela = DateTime.Today
            }
        };

            var notaTipoValor = new NotaTipoValor
            {
                TipoNota = TipoNota.Nota
            };

            var tipoCalendario = new SME.SGP.Dominio.TipoCalendario
            {
                Id = 1
            };

            var periodosEscolares = new List<PeriodoEscolar>
        {
            new PeriodoEscolar
            {
                Id = 1,
                Bimestre = 1,
                PeriodoInicio = DateTime.Today.AddMonths(-1),
                PeriodoFim = DateTime.Today,
                TipoCalendarioId = 1
            }
        };

            var componentesCurriculares = new List<DisciplinaDto>
        {
            new DisciplinaDto
            {
                CodigoComponenteCurricular = 1,
                Nome = "Matemática",
                Regencia = false,
                Id = 1
            }
        };

            var usuario = new Usuario();
            typeof(Usuario)
                .GetProperty("Perfis")
                .SetValue(usuario, new List<PrioridadePerfil>
                {
            new PrioridadePerfil { Tipo = TipoPerfil.DRE }
                });

            var valorParametroPercentual = "30";
            var valorParametroMedia = "6";

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaComUeEDrePorCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(turma);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterAlunosPorTurmaEAnoLetivoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(alunos);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterNotaTipoValorPorTurmaIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(notaTipoValor);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterTipoCalendarioPorAnoLetivoEModalidadeQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(tipoCalendario);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterPeriodosEscolaresPorTipoCalendarioIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(periodosEscolares);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterComponentesCurricularesPorIdsUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(componentesCurriculares);

            mediatorMock.Setup(m => m.Send(It.Is<ObterValorParametroSistemaTipoEAnoQuery>(q => q.Tipo == TipoParametroSistema.PercentualAlunosInsuficientes), It.IsAny<CancellationToken>()))
                .ReturnsAsync(valorParametroPercentual);

            mediatorMock.Setup(m => m.Send(It.Is<ObterValorParametroSistemaTipoEAnoQuery>(q => q.Tipo == TipoParametroSistema.MediaBimestre), It.IsAny<CancellationToken>()))
                .ReturnsAsync(valorParametroMedia);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(usuario);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaPorCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(turma);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterFechamentosTurmaComponentesQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<FechamentoTurmaDisciplina>());

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterCodigosAlunosComAnotacaoNoFechamentoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<string>());

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterTipoAvaliacaoBimestralQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new TipoAvaliacao { AvaliacoesNecessariasPorBimestre = 1 });

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterPeriodoEscolarPorCalendarioEDataQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(periodosEscolares.First());

            mediatorMock.Setup(m => m.Send(It.IsAny<TurmaPossuiAvaliacaoNoPeriodoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterParametrosArredondamentoNotaPorDataAvaliacaoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new NotaParametroDto { Incremento = 3, Maxima = 7, Minima = 2 });

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterAvaliacoesBimestraisQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<AtividadeAvaliativaDisciplina> { new AtividadeAvaliativaDisciplina() });

            mediatorMock.Setup(m => m.Send(It.IsAny<VerificaEstudantePossuiPlanoAEEPorCodigoEAnoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterAlunosAtivosTurmaProgramaPapEolQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<AlunosTurmaProgramaPapDto>());

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterPeriodoFechamentoVigentePorTurmaDataBimestreQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new PeriodoFechamentoVigenteDto
                {
                    PeriodoFechamentoInicio = DateTime.Today.AddDays(-10),
                    PeriodoFechamentoFim = DateTime.Today.AddDays(10)
                });

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterMarcadorAlunoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new MarcadorFrequenciaDto());

            mediatorMock.Setup(m => m.Send(It.IsAny<PodePersistirTurmaDisciplinaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            mediatorMock.Setup(m => m.Send(It.IsAny<TurmaEmPeriodoFechamentoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterComponenteLancaNotaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterFrequenciaAlunosPorAlunoDisciplinaPeriodoEscolarTipoTurmaQuery>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(new FrequenciaAluno
               {
                   TotalAulas = 20,
                   TotalAusencias = 0,
                   TotalCompensacoes = 0
               });

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterNotasBimestrePorCodigoAlunoFechamentoIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<FechamentoNotaDto>());

            mediatorMock.Setup(m => m.Send(It.IsAny<ExigeAprovacaoDeNotaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            var resultado = await useCase.Executar("turma123", 1, 1, 1);

            Assert.NotNull(resultado);
            Assert.Equal(TipoNota.Nota, resultado.NotaTipo);
            Assert.Equal(6, resultado.MediaAprovacaoBimestre);
            Assert.Equal(30, resultado.PercentualAlunosInsuficientes);
            Assert.NotNull(resultado.Alunos);
            Assert.Single(resultado.Alunos);
            Assert.Equal("Aluno Teste", resultado.Alunos.First().Nome);
            Assert.Equal("123", resultado.Alunos.First().CodigoAluno);
            Assert.Equal(1, resultado.Alunos.First().NumeroChamada);
            Assert.Equal(SituacaoFechamento.NaoIniciado, resultado.Situacao);
            Assert.NotNull(resultado.DadosArredondamento);
            Assert.True(resultado.PossuiAvaliacao);
            Assert.NotNull(resultado.Observacoes);
        }
    }
}