using MediatR;
using Moq;
using Newtonsoft.Json;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Fechamento
{
    public class GerarPendenciasFechamentoUseCaseTeste
    {
        [Fact]
        public async Task Deve_Executar_Comando_De_Gerar_Pendencias_Com_Sucesso()
        {
            var comandoOriginal = new GerarPendenciasFechamentoCommand(
                componenteCurricularId: 123,
                turmaCodigo: "T123",
                turmaNome: "Turma Teste",
                periodoEscolarInicio: new DateTime(2025, 2, 1),
                periodoEscolarFim: new DateTime(2025, 6, 30),
                bimestre: 2,
                usuarioId: 456,
                usuarioRF: "1234567",
                fechamentoTurmaDisciplinaId: 789,
                justificativa: "Justificativa teste",
                criadoRF: "1234567",
                turmaId: 9999,
                perfilUsuario: null, 
                componenteSemNota: false,
                registraFrequencia: true
            );

            var mensagem = new MensagemRabbit
            {
                Mensagem = JsonConvert.SerializeObject(comandoOriginal),
                PerfilUsuario = "Professor"
            };

            var mediatorMock = new Mock<IMediator>();
            mediatorMock
                .Setup(m => m.Send(It.IsAny<GerarPendenciasFechamentoCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var useCase = new GerarPendenciasFechamentoUseCase(mediatorMock.Object);

            var resultado = await useCase.Executar(mensagem);

            Assert.True(resultado);
            mediatorMock.Verify(m => m.Send(
                It.Is<GerarPendenciasFechamentoCommand>(cmd =>
                    cmd.ComponenteCurricularId == comandoOriginal.ComponenteCurricularId &&
                    cmd.TurmaCodigo == comandoOriginal.TurmaCodigo &&
                    cmd.Bimestre == comandoOriginal.Bimestre &&
                    cmd.PerfilUsuario == mensagem.PerfilUsuario),
                It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}
