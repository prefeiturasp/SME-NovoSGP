using Moq;
using Newtonsoft.Json;
using SME.SGP.Infra;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.TipoCalendario
{
    public class ExecutarTipoCalendarioUseCaseTeste
    {
        private readonly Mock<IComandosTipoCalendario> _comandosTipoCalendarioMock;
        private readonly ExecutarTipoCalendarioUseCase _useCase;

        public ExecutarTipoCalendarioUseCaseTeste()
        {
            _comandosTipoCalendarioMock = new Mock<IComandosTipoCalendario>();
            _useCase = new ExecutarTipoCalendarioUseCase(_comandosTipoCalendarioMock.Object);
        }

        [Fact]
        public async Task Executar_Quando_Mensagem_Valida_Deve_Chamar_Comando_E_Retornar_Verdadeiro()
        {
            var tipoCalendarioDto = new TipoCalendarioDto
            {
                Id = 1,
                AnoLetivo = 2025,
                Nome = "Calendário Teste DTO"
            };

            var tipoCalendario = new Dominio.TipoCalendario
            {
                Id = 1,
                AnoLetivo = 2025,
                Nome = "Calendário Teste"
            };

            var parametro = new ExecutarTipoCalendarioUseCase.ExecutarTipoCalendarioParametro
            {
                Dto = tipoCalendarioDto,
                TipoCalendario = tipoCalendario
            };

            var mensagemJson = JsonConvert.SerializeObject(parametro);
            var mensagemRabbit = new MensagemRabbit(mensagemJson);

            _comandosTipoCalendarioMock.Setup(c => c.ExecutarReplicacao(
                It.IsAny<TipoCalendarioDto>(),
                It.IsAny<bool>(),
                It.IsAny<Dominio.TipoCalendario>()))
                .Returns(Task.CompletedTask);

            var resultado = await _useCase.Executar(mensagemRabbit);

            _comandosTipoCalendarioMock.Verify(c => c.ExecutarReplicacao(
                It.Is<TipoCalendarioDto>(d => d.Id == tipoCalendarioDto.Id),
                false,
                It.Is<Dominio.TipoCalendario>(t => t.Id == tipoCalendario.Id)), Times.Once);

            Assert.True(resultado);
        }
    }
}
