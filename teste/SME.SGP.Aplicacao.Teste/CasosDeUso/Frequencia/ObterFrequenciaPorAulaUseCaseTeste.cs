using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Frequencia
{
    public class ObterFrequenciaPorAulaUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly ObterFrequenciaPorAulaUseCase useCase;

        public ObterFrequenciaPorAulaUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new ObterFrequenciaPorAulaUseCase(mediatorMock.Object);
        }

        [Fact(DisplayName = "Executar deve retornar Frequencia preenchida quando aula, turma e alunos existem")]
        public async Task Deve_Retornar_Frequencia_Quando_Sucesso()
        {
            // Arrange
            var aula = new Dominio.Aula
            {
                Id = 1,
                TurmaId = "12345",
                DisciplinaId = "101",
                TipoCalendarioId = 10,
                DataAula = DateTime.Today,
                Quantidade = 1
            };

            var turma = new Turma
            {
                Id = 1,
                CodigoTurma = "12345",                
                AnoLetivo = DateTime.Today.Year,
                TipoTurma = TipoTurma.Regular
            };

            var aluno = new AlunoPorTurmaResposta
            {
                CodigoAluno = "123",
                NomeAluno = "João da Silva",
                CodigoSituacaoMatricula = SituacaoMatriculaAluno.Ativo
            };

            // ValidarParametros
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterAulaPorIdQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(aula);
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaComUeEDrePorCodigoQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(turma);
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterAlunosDentroPeriodoQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(new List<AlunoPorTurmaResposta> { aluno });

            // Registro de frequência
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterRegistroFrequenciaPorAulaIdQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync((RegistroFrequencia)null);

            // Frequências de alunos
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterRegistrosFrequenciasAlunosSimplificadoPorAulaIdQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(new List<FrequenciaAlunoSimplificadoDto>());

            // Compensações
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterCompensacaoAusenciaAlunoAulaSimplificadoPorAulaIdsQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(new List<CompensacaoAusenciaAlunoAulaSimplificadoDto>());

            // Período escolar
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterPeriodosEscolaresPorTipoCalendarioIdEDataQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(new PeriodoEscolar
                        {
                            Id = 1,
                            Bimestre = 1,
                            PeriodoInicio = DateTime.Today.AddDays(-1),
                            PeriodoFim = DateTime.Today.AddDays(1)
                        });

            // Periodo aberto
            mediatorMock.Setup(m => m.Send(It.IsAny<TurmaEmPeriodoAbertoQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(true);

            // Parâmetros do sistema
            mediatorMock.Setup(m => m.Send(It.Is<ObterParametroSistemaPorTipoEAnoQuery>(q => q.TipoParametroSistema == TipoParametroSistema.PercentualFrequenciaCritico), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(new ParametrosSistema { Valor = "50" });
            mediatorMock.Setup(m => m.Send(It.Is<ObterParametroSistemaPorTipoEAnoQuery>(q => q.TipoParametroSistema == TipoParametroSistema.PercentualFrequenciaAlerta), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(new ParametrosSistema { Valor = "70" });

            // Componentes curriculares
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterComponentesCurricularesPorIdsUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(new List<DisciplinaDto> { new DisciplinaDto { CodigoComponenteCurricular = 101, RegistraFrequencia = true } });

            // Alunos com anotação
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterAlunosComAnotacaoNaAulaQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(new List<string>());

            // Frequências já registradas
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterFrequenciaAlunosPorTurmaDisciplinaEPeriodoEscolarQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(new List<FrequenciaAluno>());
            mediatorMock.Setup(m => m.Send(It.IsAny<ExisteFrequenciaRegistradaPorTurmaComponenteCurricularQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(false);

            // PAP
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterAlunosAtivosTurmaProgramaPapEolQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(new List<AlunosTurmaProgramaPapDto>());

            // Frequência predefinida
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterFrequenciaPreDefinidaPorAlunoETurmaQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(TipoFrequencia.C);

            // Marcador
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterMarcadorFrequenciaAlunoQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(new MarcadorFrequenciaDto());

            var filtro = new FiltroFrequenciaDto { AulaId = 1 };

            // Act
            var resultado = await useCase.Executar(filtro);

            // Assert
            Assert.NotNull(resultado);
            Assert.False(resultado.Desabilitado);
            Assert.Single(resultado.ListaFrequencia);
            Assert.Equal(aluno.CodigoAluno, resultado.ListaFrequencia.First().CodigoAluno);
        }
    }
}
