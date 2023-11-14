using MediatR;
using Moq;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.ConsolidacaoTurmaConselhoClasse
{
    public class ExecutarConsolidacaoTurmaConselhoClasseUseCaseTeste
    {

        [Fact]
        public async Task Deve_Evitar_ConsolidacaoErrada_Para_Alunos_Rematriculados_No_Ano_Seguinte()
        {
            // Arrange
            var mediatorMock = new Mock<IMediator>();
            var consultasDisciplinaMock = new Mock<IConsultasDisciplina>();

            var useCase = new ExecutarConsolidacaoTurmaConselhoClasseUseCase(mediatorMock.Object);

            var turma = new Turma
            {
                Id = 1,
                CodigoTurma = "1",
                ModalidadeCodigo = Modalidade.Fundamental,
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year

            };

            var mensagemPorTurma = new ConsolidacaoTurmaDto(turma.Id, (int)Bimestre.Primeiro);
            var mensagem = JsonConvert.SerializeObject(mensagemPorTurma);
            var mensagemRabbit = new MensagemRabbit() { Mensagem = mensagem };

            List<AlunoPorTurmaResposta> alunos = CriarAlunos();



            var tipoCalendarioId = 1;
            List<PeriodoEscolar> periodosEscolares = CriarPeriodosEscolares(tipoCalendarioId);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaComUeEDrePorIdQuery>(), default))
                .ReturnsAsync(turma);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterAlunosAtivosPorTurmaCodigoQuery>(), default))
                .ReturnsAsync(alunos);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterMatriculasAlunoNaTurmaQuery>(), default))
                .ReturnsAsync(alunos);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterIdTipoCalendarioPorAnoLetivoEModalidadeQuery>(), default))
                .ReturnsAsync(tipoCalendarioId);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterPeriodosEscolaresPorTipoCalendarioIdQuery>(), default))
                .ReturnsAsync(periodosEscolares);

            mediatorMock.Setup(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), default))
                .ReturnsAsync(true);

            // Act
            var resultado = await useCase.Executar(mensagemRabbit);

            // Assert
            Assert.True(resultado);
            mediatorMock.Verify(m => m.Send(It.IsAny<ObterConsolidacoesConselhoClasseAtivasIdPorAlunoETurmaQuery>(), default), Times.Never);
            mediatorMock.Verify(m => m.Send(It.IsAny<ObterConsolidacoesConselhoClasseNotaPorConsolidacaoAlunoIdsBimestreQuery>(), default), Times.Never);
            mediatorMock.Verify(m => m.Send(It.IsAny<ExcluirConsolidacaoConselhoPorIdBimestreCommand>(), default), Times.Never);
        }

        private static List<PeriodoEscolar> CriarPeriodosEscolares(int tipoCalendarioId)
        {
            return new List<PeriodoEscolar>()
            { 
                new PeriodoEscolar() {
                    Id = 3,
                    Bimestre = (int)Bimestre.Terceiro,
                    PeriodoInicio = new DateTime(2023,07,24),
                    PeriodoFim = new DateTime(2023,09,30),
                    TipoCalendarioId = tipoCalendarioId
                },
                new PeriodoEscolar() {
                    Id = 4,
                    Bimestre = (int)Bimestre.Quarto,
                    PeriodoInicio = new DateTime(2023,10,02),
                    PeriodoFim = new DateTime(2023,12,21),
                    TipoCalendarioId = tipoCalendarioId
                },
                new PeriodoEscolar() {
                    Id = 1,
                    Bimestre = (int)Bimestre.Primeiro,
                    PeriodoInicio = new DateTime(2023,02,06),
                    PeriodoFim = new DateTime(2023,04,29),
                    TipoCalendarioId = tipoCalendarioId

                },
                new PeriodoEscolar() {
                    Id = 2,
                    Bimestre = (int)Bimestre.Segundo,
                    PeriodoInicio = new DateTime(2023,05,02),
                    PeriodoFim = new DateTime(2023,07,08),
                    TipoCalendarioId = tipoCalendarioId
                }
              
            };
        }

        private static List<AlunoPorTurmaResposta> CriarAlunos()
        {
            var dataMatriculaAluno = new DateTime(2022, 10, 26);
            var dataSituacaoAluno = new DateTime(2023, 10, 31);

            var alunoAtivo = new AlunoPorTurmaResposta
            {
                CodigoAluno = "1",
                SituacaoMatricula = "Rematriculado",
                DataSituacao = dataSituacaoAluno,
                DataMatricula = dataMatriculaAluno,
                Ano = 2023,
                CelularResponsavel = "1111111111111",
                CodigoComponenteCurricular = 0,
                CodigoEscola = "1",
                CodigoSituacaoMatricula = SituacaoMatriculaAluno.Rematriculado,
                CodigoTipoTurma = 0,
                CodigoTurma = 1,
                EscolaTransferencia = null,
                NomeAluno = "1",
                NumeroAlunoChamada = 1,
                ParecerConclusivo = null,
                TipoResponsavel = "1"
            };
            var alunoInativo = new AlunoPorTurmaResposta
            {
                CodigoAluno = "2",
                SituacaoMatricula = "Transferido",
                DataSituacao = dataSituacaoAluno,
                DataMatricula = dataMatriculaAluno,
                Ano = 2023,
                CelularResponsavel = "1111111111111",
                CodigoComponenteCurricular = 0,
                CodigoEscola = "1",
                CodigoSituacaoMatricula = SituacaoMatriculaAluno.Transferido,
                CodigoTipoTurma = 0,
                CodigoTurma = 1,
                EscolaTransferencia = null,
                NomeAluno = "2",
                NumeroAlunoChamada = 2,
                ParecerConclusivo = null,
                TipoResponsavel = "1"
            };

            var alunos = new List<AlunoPorTurmaResposta> { alunoAtivo, alunoInativo };
            return alunos;
        }
    }
}
