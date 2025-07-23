using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Pendencias
{
    public class RemoverAtribuicaoPendenciasUsuariosUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly RemoverAtribuicaoPendenciasUsuariosUseCase useCase;

        public RemoverAtribuicaoPendenciasUsuariosUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new RemoverAtribuicaoPendenciasUsuariosUseCase(mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Deve_Retornar_True_Quando_Sucesso()
        {
            var pendenciasComUe = new List<PendenciaPerfilUsuarioDto>
            {
                new() { PendenciaId = 1, UeId = 10 },
                new() { PendenciaId = 2, UeId = 20 },
                new() { PendenciaId = 3, UeId = 10 }
            };

            var pendenciasSemPerfilUsuario = new List<PendenciaPendenteDto>
            {
                new() { PendenciaId = 100, UeId = 10 }
            };

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterPendenciaPerfilUsuarioPorSituacaoPendenciaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(pendenciasComUe);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterPendenciaSemPendenciaPerfilUsuarioQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(pendenciasSemPerfilUsuario);

            var resultado = await useCase.Executar(new MensagemRabbit());

            Assert.True(resultado);

            mediatorMock.Verify(m => m.Send(It.Is<PublicarFilaSgpCommand>(cmd =>
                cmd.Rota == RotasRabbitSgpPendencias.RotaRemoverAtribuicaoPendenciaUsuariosUe &&
                (long)((FiltroRemoverAtribuicaoPendenciaDto)cmd.Filtros).UeId == 10), It.IsAny<CancellationToken>()), Times.Once);

            mediatorMock.Verify(m => m.Send(It.Is<PublicarFilaSgpCommand>(cmd =>
                cmd.Rota == RotasRabbitSgpPendencias.RotaRemoverAtribuicaoPendenciaUsuariosUe &&
                (long)((FiltroRemoverAtribuicaoPendenciaDto)cmd.Filtros).UeId == 20), It.IsAny<CancellationToken>()), Times.Once);

            mediatorMock.Verify(m => m.Send(It.Is<PublicarFilaSgpCommand>(cmd =>
                cmd.Rota == RotasRabbitSgpPendencias.RotaTratarAtribuicaoPendenciaUsuarios &&
                cmd.Filtros is FiltroTratamentoAtribuicaoPendenciaDto), It.IsAny<CancellationToken>()), Times.Exactly(pendenciasSemPerfilUsuario.Count));
        }

        [Fact]
        public async Task Executar_Deve_Lancar_NegocioException_Se_Houver_Pendencias_Sem_Ue()
        {
            var pendenciasComUe = new List<PendenciaPerfilUsuarioDto>
            {
                new() { PendenciaId = 1, UeId = 10 },
                new() { PendenciaId = 2, UeId = null }, 
            };

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterPendenciaPerfilUsuarioPorSituacaoPendenciaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(pendenciasComUe);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterPendenciaSemPendenciaPerfilUsuarioQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<PendenciaPendenteDto>());

            await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(new MensagemRabbit()));
        }

        [Fact]
        public async Task Executar_Deve_Lancar_NegocioException_Se_Qualquer_Excecao_Ocorrida()
        {
            mediatorMock.Setup(m => m.Send(It.IsAny<IRequest<IEnumerable<PendenciaPerfilUsuarioDto>>>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Erro simulado"));

            await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(new MensagemRabbit()));
        }

        [Fact]
        public async Task Executar_Deve_Chamar_Send_Corretamente_Para_Todas_As_Ues_Distintas()
        {
            var pendencias = new List<PendenciaPerfilUsuarioDto>
            {
                new() { PendenciaId = 1, UeId = 1 },
                new() { PendenciaId = 2, UeId = 2 },
                new() { PendenciaId = 3, UeId = 1 },
                new() { PendenciaId = 4, UeId = 3 }
            };

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterPendenciaPerfilUsuarioPorSituacaoPendenciaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(pendencias);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterPendenciaSemPendenciaPerfilUsuarioQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<PendenciaPendenteDto>());

            var result = await useCase.Executar(new MensagemRabbit());

            Assert.True(result);

            mediatorMock.Verify(m => m.Send(It.Is<PublicarFilaSgpCommand>(cmd =>
                cmd.Rota == RotasRabbitSgpPendencias.RotaRemoverAtribuicaoPendenciaUsuariosUe &&
                ((FiltroRemoverAtribuicaoPendenciaDto)cmd.Filtros).UeId == 1), It.IsAny<CancellationToken>()), Times.Once);

            mediatorMock.Verify(m => m.Send(It.Is<PublicarFilaSgpCommand>(cmd =>
                cmd.Rota == RotasRabbitSgpPendencias.RotaRemoverAtribuicaoPendenciaUsuariosUe &&
                ((FiltroRemoverAtribuicaoPendenciaDto)cmd.Filtros).UeId == 2), It.IsAny<CancellationToken>()), Times.Once);

            mediatorMock.Verify(m => m.Send(It.Is<PublicarFilaSgpCommand>(cmd =>
                cmd.Rota == RotasRabbitSgpPendencias.RotaRemoverAtribuicaoPendenciaUsuariosUe &&
                ((FiltroRemoverAtribuicaoPendenciaDto)cmd.Filtros).UeId == 3), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
