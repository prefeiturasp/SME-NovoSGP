using FluentAssertions;
using MediatR;
using Moq;
using Newtonsoft.Json;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Pendencias
{
    public class ExcluirPendenciaCalendarioAnoAnteriorCalendarioPoUeUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly ExcluirPendenciaCalendarioAnoAnteriorCalendarioPoUeUseCase useCase;

        public ExcluirPendenciaCalendarioAnoAnteriorCalendarioPoUeUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new ExcluirPendenciaCalendarioAnoAnteriorCalendarioPoUeUseCase(mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Deve_Publicar_Pendencias_Na_Fila_Quando_Dados_Forem_Validos()
        {
            var mediatorMock = new Mock<IMediator>();
            var useCase = new ExcluirPendenciaCalendarioAnoAnteriorCalendarioPoUeUseCase(mediatorMock.Object);

            var pendenciasEsperadas = new long[] { 1, 2, 3 };
            var dto = new ExcluirPendenciaCalendarioAnoAnteriorPorUeDto(2024, 9999, pendenciasEsperadas);
            var mensagem = new MensagemRabbit { Mensagem = JsonConvert.SerializeObject(dto) };

            PublicarFilaSgpCommand comandoCapturado = null;

            mediatorMock
                .Setup(x => x.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()))
                .Callback<IRequest<bool>, CancellationToken>((cmd, _) =>
                {
                    comandoCapturado = (PublicarFilaSgpCommand)cmd;
                })
                .ReturnsAsync(true);

            var resultado = await useCase.Executar(mensagem);

            resultado.Should().BeTrue();
            comandoCapturado.Should().NotBeNull();
            comandoCapturado.Rota.Should().Be(RotasRabbitSgpPendencias.RotaExcluirPendenciaCalendarioAnoAnteriorCalendarioIdsPendencias);

            var filtros = comandoCapturado.Filtros as long[];
            filtros.Should().NotBeNull("Filtros deve ser long[]");
            filtros.Should().BeEquivalentTo(pendenciasEsperadas);
        }

        [Fact]
        public async Task Executar_Deve_Lancar_Excecao_E_Logar_Erro_Se_Falhar_Ao_Publicar()
        {
            var dto = new ExcluirPendenciaCalendarioAnoAnteriorPorUeDto(2024, 123, new long[] { 10, 20 });
            var mensagem = new MensagemRabbit { Mensagem = JsonConvert.SerializeObject(dto) };

            var innerException = new Exception("Erro interno");
            var exception = new Exception("Erro ao publicar", innerException);

            mediatorMock.Setup(x => x.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()))
                        .ThrowsAsync(exception);

            SalvarLogViaRabbitCommand logCapturado = null;

            mediatorMock.Setup(x => x.Send(It.IsAny<SalvarLogViaRabbitCommand>(), It.IsAny<CancellationToken>()))
                        .Callback<IRequest<bool>, CancellationToken>((req, _) => logCapturado = (SalvarLogViaRabbitCommand)req)
                        .ReturnsAsync(true);

            Func<Task> acao = async () => await useCase.Executar(mensagem);

            await acao.Should().ThrowAsync<Exception>().WithMessage("Erro ao publicar");

            logCapturado.Should().NotBeNull();
            logCapturado.Mensagem.Should().Contain("Não foi possível realizar a exclusão");
            logCapturado.Nivel.Should().Be(LogNivel.Critico);
            logCapturado.Contexto.Should().Be(LogContexto.Calendario);
            logCapturado.InnerException.Should().Contain("Erro interno");
            logCapturado.Observacao.Should().Contain("Erro ao publicar");
        }
    }
}
