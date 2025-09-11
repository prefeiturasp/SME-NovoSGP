using MediatR;
using Moq;
using Newtonsoft.Json;
using SME.SGP.Infra;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Frequencia
{
    public class ConciliacaoFrequenciaTurmasAlunosBuscarUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly ConciliacaoFrequenciaTurmasAlunosBuscarUseCase useCase;

        public ConciliacaoFrequenciaTurmasAlunosBuscarUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new ConciliacaoFrequenciaTurmasAlunosBuscarUseCase(mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_DeveEnviarComandoParaCadaComponenteEData()
        {
            // Arrange
            var turmaCodigo = "T1";
            var componentes = new[] { "MAT", "PORT" };
            var datas = new[] { new DateTime(2025, 5, 1), new DateTime(2025, 5, 2) };
            var alunos = new[] { "1", "2" };

            var dto = new TurmaComponentesParaCalculoFrequenciaDto(turmaCodigo, componentes, datas);
            var mensagem = new MensagemRabbit
            {
                Mensagem = JsonConvert.SerializeObject(dto)
            };

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterAlunosPorTurmaMapochoQuery>(), default))
                .ReturnsAsync(alunos);

            mediatorMock
                .Setup(m => m.Send(It.IsAny<IncluirFilaCalcularFrequenciaPorTurmaCommand>(), default))
                .ReturnsAsync(true);

            // Act
            var resultado = await useCase.Executar(mensagem);

            // Assert
            Assert.True(resultado);

            mediatorMock.Verify(m => m.Send(
                It.IsAny<ObterAlunosPorTurmaMapochoQuery>(),
                It.IsAny<CancellationToken>()), Times.Once);

            mediatorMock.Verify(m => m.Send(
                It.Is<IncluirFilaCalcularFrequenciaPorTurmaCommand>(c =>
                    c.Alunos.SequenceEqual(alunos) &&
                    c.TurmaId == turmaCodigo &&
                    componentes.Contains(c.DisciplinaId) &&
                    datas.Contains(c.DataAula)
                ),
                It.IsAny<CancellationToken>()), Times.Exactly(componentes.Length * datas.Length));
        }
    }
}
