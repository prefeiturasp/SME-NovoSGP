using MediatR;
using Moq;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.DiarioBordoPendenciaDevolutiva
{
    public class ReprocessarDiarioBordoPendenciaDevolutivaPorUeUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly Mock<IRepositorioUeConsulta> repositorioUeConsultaMock;
        private readonly ReprocessarDiarioBordoPendenciaDevolutivaPorUeUseCase useCase;

        public ReprocessarDiarioBordoPendenciaDevolutivaPorUeUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            repositorioUeConsultaMock = new Mock<IRepositorioUeConsulta>();
            useCase = new ReprocessarDiarioBordoPendenciaDevolutivaPorUeUseCase(mediatorMock.Object, repositorioUeConsultaMock.Object);
        }

        [Fact]
        public async Task Executar_Deve_Publicar_Comandos_Quando_Nao_Ignorar_Geracao_Pendencia()
        {
            var filtroDto = new FiltroDiarioBordoPendenciaDevolutivaDto(anoLetivo: 2025, dreId: 1);
            var mensagem = new MensagemRabbit(JsonConvert.SerializeObject(filtroDto));
            var ues = new List<Ue>
            {
                new Ue { Id = 10, CodigoUe = "UE1", TipoEscola = TipoEscola.EMEF },
                new Ue { Id = 20, CodigoUe = "UE2", TipoEscola = TipoEscola.EMEF },
                new Ue { Id = 30, CodigoUe = "UE3", TipoEscola = TipoEscola.EMEI }
            };

            repositorioUeConsultaMock.Setup(r => r.ObterPorDre(filtroDto.DreId)).ReturnsAsync(ues);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterTipoUeIgnoraGeracaoPendenciasQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            var resultado = await useCase.Executar(mensagem);

            Assert.True(resultado);

            mediatorMock.Verify(m => m.Send(It.Is<PublicarFilaSgpCommand>(cmd =>
                cmd.Rota == RotasRabbitSgp.RotaReprocessarDiarioBordoPendenciaDevolutivaPorTurma &&
                ((FiltroDiarioBordoPendenciaDevolutivaDto)cmd.Filtros).DreId == filtroDto.DreId &&
                new[] { "UE1", "UE2", "UE3" }.Contains(((FiltroDiarioBordoPendenciaDevolutivaDto)cmd.Filtros).UeCodigo)
            ), It.IsAny<CancellationToken>()), Times.Exactly(3));
        }

        [Fact]
        public async Task Executar_Nao_Deve_Publicar_Quando_Ignorar_Geracao_Pendencia()
        {
            var filtroDto = new FiltroDiarioBordoPendenciaDevolutivaDto(2025, 1);
            var mensagem = new MensagemRabbit(JsonConvert.SerializeObject(filtroDto));
            var ues = new List<Ue>
            {
                new Ue { Id = 10, CodigoUe = "UE1", TipoEscola = TipoEscola.EMEF },
            };

            repositorioUeConsultaMock.Setup(r => r.ObterPorDre(filtroDto.DreId)).ReturnsAsync(ues);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterTipoUeIgnoraGeracaoPendenciasQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var resultado = await useCase.Executar(mensagem);

            Assert.True(resultado);

            mediatorMock.Verify(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Executar_Deve_Retornar_False_E_Logar_Quando_Exception()
        {
            var filtroDto = new FiltroDiarioBordoPendenciaDevolutivaDto(2025, 1);
            var mensagem = new MensagemRabbit(JsonConvert.SerializeObject(filtroDto));

            repositorioUeConsultaMock.Setup(r => r.ObterPorDre(filtroDto.DreId))
                .ThrowsAsync(new Exception("Erro inesperado"));

            var resultado = await useCase.Executar(mensagem);

            Assert.False(resultado);

            mediatorMock.Verify(m => m.Send(It.Is<SalvarLogViaRabbitCommand>(cmd =>
                cmd.Mensagem.Contains("Não foi possível executar") &&
                cmd.Nivel == LogNivel.Critico &&
                cmd.Contexto == LogContexto.Devolutivas
            ), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
