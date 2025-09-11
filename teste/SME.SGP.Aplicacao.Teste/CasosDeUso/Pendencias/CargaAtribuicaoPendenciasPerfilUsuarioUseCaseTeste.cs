using MediatR;
using Moq;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Pendencias
{
    public class CargaAtribuicaoPendenciasPerfilUsuarioUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly CargaAtribuicaoPendenciasPerfilUsuarioUseCase useCase;

        public CargaAtribuicaoPendenciasPerfilUsuarioUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new CargaAtribuicaoPendenciasPerfilUsuarioUseCase(mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Deve_Obter_Pendencias_E_Publicar_Fila_Para_Cada_Pendencia()
        {
            var pendencias = new List<PendenciaPendenteDto>
            {
                new PendenciaPendenteDto { PendenciaId = 1, UeId = 10 },
                new PendenciaPendenteDto { PendenciaId = 2, UeId = 20 }
            };

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterPendenciaCargaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(pendencias);

            mediatorMock.Setup(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var mensagem = new MensagemRabbit();

            var resultado = await useCase.Executar(mensagem);

            Assert.True(resultado);

            mediatorMock.Verify(m => m.Send(It.IsAny<ObterPendenciaCargaQuery>(), It.IsAny<CancellationToken>()), Times.Once);

            foreach (var pendencia in pendencias)
            {
                mediatorMock.Verify(m => m.Send(It.Is<PublicarFilaSgpCommand>(cmd =>
                    cmd.Rota == RotasRabbitSgpPendencias.RotaTratarAtribuicaoPendenciaUsuarios &&
                    VerificaFiltro(cmd.Filtros, pendencia.PendenciaId, pendencia.UeId)
                ), It.IsAny<CancellationToken>()), Times.Once);
            }
        }

        [Fact]
        public async Task Executar_Quando_Mediator_Throw_Exception_Deve_Chamar_Salvar_Log()
        {
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterPendenciaCargaQuery>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Erro simulado"));

            mediatorMock.Setup(m => m.Send(It.IsAny<SalvarLogViaRabbitCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var mensagem = new MensagemRabbit();

            var resultado = await useCase.Executar(mensagem);

            Assert.True(resultado);

            mediatorMock.Verify(m => m.Send(It.IsAny<ObterPendenciaCargaQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            mediatorMock.Verify(m => m.Send(It.Is<SalvarLogViaRabbitCommand>(cmd =>
                cmd.Mensagem.Contains("Erro ao realizar a carga de atribuição de pendencia usuário")
            ), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_Retorna_True_Sempre()
        {
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterPendenciaCargaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<PendenciaPendenteDto>());

            var mensagem = new MensagemRabbit();

            var resultado = await useCase.Executar(mensagem);

            Assert.True(resultado);
        }

        bool VerificaFiltro(object filtros, long pendenciaId, long ueId)
        {
            if (filtros is FiltroTratamentoAtribuicaoPendenciaDto filtro)
                return filtro.PendenciaId == pendenciaId && filtro.UeId == ueId;
            return false;
        }
    }
}
