using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using Newtonsoft.Json;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dto;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.EncaminhamentoNAAPA
{
    public class AtualizarTurmasProgramaDoEncaminhamentoNAAPAUseCaseTeste
    {
        [Fact]
        public async Task Executar_DeveAtualizarTurmasPrograma_QuandoDiferenteDasExistentes()
        {
            // Arrange
            var mediatorMock = new Mock<IMediator>();
            var useCase = new AtualizarTurmasProgramaDoEncaminhamentoNAAPAUseCase(mediatorMock.Object);

            var encaminhamentoId = 123L;
            var alunoCodigo = "456";
            var turmaId = 789L;
            var anoLetivo = 2024;

            var mensagem = new MensagemRabbit
            {
                Mensagem = JsonConvert.SerializeObject(new EncaminhamentoNAAPADto
                {
                    Id = encaminhamentoId,
                    AlunoCodigo = alunoCodigo,
                    TurmaId = turmaId
                })
            };

            var turma = new Turma
            {
                Id = turmaId,
                AnoLetivo = anoLetivo
            };

            var turmasAtuais = new List<AlunoTurmaProgramaDto>
        {
            new AlunoTurmaProgramaDto { DreUe = "DRE1 UE1", Turma = "TURMA1", ComponenteCurricular = "MATEMÁTICA" },
            new AlunoTurmaProgramaDto { DreUe = "DRE2 UE2", Turma = "TURMA2", ComponenteCurricular = "PORTUGUÊS" }
        };

            var respostaExistente = new RespostaEncaminhamentoNAAPA
            {
                Id = 1,
                Texto = JsonConvert.SerializeObject(new List<RespostaTurmaProgramaEncaminhamentoNAAPADto>
            {
                new RespostaTurmaProgramaEncaminhamentoNAAPADto { dreUe = "DRE1 UE1", turma = "TURMA1", componenteCurricular = "MATEMÁTICA" }
            })
            };

            var questao = new QuestaoEncaminhamentoNAAPA
            {
                QuestaoId = 10,
                Respostas = new List<RespostaEncaminhamentoNAAPA> { respostaExistente }
            };

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaPorIdQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(turma);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmasProgramaAlunoQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(turmasAtuais);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterQuestaoTurmasProgramaEncaminhamentoNAAPAPorIdQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(questao);

            mediatorMock.Setup(m => m.Send(It.IsAny<AlterarEncaminhamentoNAAPASecaoQuestaoRespostaCommand>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(true);

            // Act
            var resultado = await useCase.Executar(mensagem);

            // Assert
            Assert.True(resultado);

            mediatorMock.Verify(m => m.Send(It.IsAny<AlterarEncaminhamentoNAAPASecaoQuestaoRespostaCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

    }
}
