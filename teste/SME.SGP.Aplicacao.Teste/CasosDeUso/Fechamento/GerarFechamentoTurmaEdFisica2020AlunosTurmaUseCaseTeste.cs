using MediatR;
using Moq;
using Newtonsoft.Json;
using SME.SGP.Infra;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Fechamento
{
    public class GerarFechamentoTurmaEdFisica2020AlunosTurmaUseCaseTeste
    {
        [Fact]
        public async Task Deve_Executar_Comando_De_Fechamento_Turma_EdFisica_Com_Sucesso()
        {
            var comandoOriginal = new GerarFechamentoTurmaEdFisica2020Command(
                turmaId: 1234,
                codigoAlunos: new long[] { 1001, 1002, 1003 }
            );

            var mensagem = new MensagemRabbit
            {
                Mensagem = JsonConvert.SerializeObject(comandoOriginal)
            };

            var mediatorMock = new Mock<IMediator>();
            mediatorMock
                .Setup(m => m.Send(It.IsAny<GerarFechamentoTurmaEdFisica2020Command>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var useCase = new GerarFechamentoTurmaEdFisica2020AlunosTurmaUseCase(mediatorMock.Object);

            var resultado = await useCase.Executar(mensagem);

            Assert.True(resultado);
            mediatorMock.Verify(m => m.Send(
                It.Is<GerarFechamentoTurmaEdFisica2020Command>(cmd =>
                    cmd.TurmaId == comandoOriginal.TurmaId &&
                    cmd.CodigoAlunos.Length == comandoOriginal.CodigoAlunos.Length &&
                    cmd.CodigoAlunos[0] == comandoOriginal.CodigoAlunos[0]), // você pode validar os demais também se quiser
                It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}
