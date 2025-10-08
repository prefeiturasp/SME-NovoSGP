using MediatR;
using Moq;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.MigracaoDadosFrequencia
{
    public class ExecutarSincronizacaoRegistroFrequenciaAlunosUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ExecutarSincronizacaoRegistroFrequenciaAlunosUseCase _useCase;

        public ExecutarSincronizacaoRegistroFrequenciaAlunosUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ExecutarSincronizacaoRegistroFrequenciaAlunosUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Quando_Recebe_Mensagem_Deve_Enviar_Comando_De_Insercao()
        {
            var frequencias = new List<RegistroFrequenciaAluno>
            {
                new RegistroFrequenciaAluno { CodigoAluno = "123", AulaId = 1 },
                new RegistroFrequenciaAluno { CodigoAluno = "456", AulaId = 1 }
            };
            var parametroDto = new ParametroFrequenciasPersistirDto(frequencias);
            var mensagem = new MensagemRabbit { Mensagem = JsonConvert.SerializeObject(parametroDto) };

            _mediatorMock.Setup(m => m.Send(
                            It.Is<InserirVariosRegistrosFrequenciaAlunoCommand>(c => c.FrequenciasPersistir.Count == frequencias.Count),
                            It.IsAny<CancellationToken>()))
                         .ReturnsAsync(true);

            var resultado = await _useCase.Executar(mensagem);

            Assert.True(resultado);

            _mediatorMock.Verify(m => m.Send(
                            It.Is<InserirVariosRegistrosFrequenciaAlunoCommand>(c => c.FrequenciasPersistir.Count == frequencias.Count),
                            It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
