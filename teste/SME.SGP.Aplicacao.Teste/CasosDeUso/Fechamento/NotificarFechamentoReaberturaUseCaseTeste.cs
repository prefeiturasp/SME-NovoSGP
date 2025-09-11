using MediatR;
using Moq;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Fechamento
{
    public class NotificarFechamentoReaberturaUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly NotificarFechamentoReaberturaUseCase _useCase;

        public NotificarFechamentoReaberturaUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new NotificarFechamentoReaberturaUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Deve_Retornar_False_Quando_Filtro_For_Nulo()
        {
            var mensagem = new MensagemRabbit("null");

            var resultado = await _useCase.Executar(mensagem);

            Assert.False(resultado);
            _mediatorMock.Verify(m => m.Send(It.Is<SalvarLogViaRabbitCommand>(
                cmd => cmd.Mensagem.Contains("não possui dados") &&
                       cmd.Nivel == LogNivel.Informacao &&
                       cmd.Contexto == LogContexto.Fechamento
            ), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Deve_Publicar_Para_UE_Quando_Eh_Para_Ue()
        {
            var filtro = CriarFiltro(ehParaUe: true);

            var mensagem = new MensagemRabbit(JsonConvert.SerializeObject(filtro));

            var resultado = await _useCase.Executar(mensagem);

            Assert.True(resultado);
            _mediatorMock.Verify(m => m.Send(It.Is<PublicarFilaSgpCommand>(
                cmd => cmd.Rota == RotasRabbitSgpFechamento.RotaNotificacaoFechamentoReaberturaUE
            ), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Deve_Publicar_Para_DRE_Quando_Eh_Para_Ue_For_Falso()
        {
            var filtro = CriarFiltro(ehParaUe: false);

            var mensagem = new MensagemRabbit(JsonConvert.SerializeObject(filtro));

            var retornoQuery = new List<Ue>
            {
                new Ue { CodigoUe = "0001", Dre = new Dre { CodigoDre = "DRE1" } },
                new Ue { CodigoUe = "0002", Dre = new Dre { CodigoDre = "DRE1" } },
                new Ue { CodigoUe = "0003", Dre = new Dre { CodigoDre = "DRE2" } },
            };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterUEsComDREsPorModalidadeTipoCalendarioQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(retornoQuery);

            var resultado = await _useCase.Executar(mensagem);

            Assert.True(resultado);
            _mediatorMock.Verify(m => m.Send(It.Is<PublicarFilaSgpCommand>(
                cmd => cmd.Rota == RotasRabbitSgpFechamento.RotaNotificacaoFechamentoReaberturaDRE
            ), It.IsAny<CancellationToken>()), Times.Exactly(2)); 
        }

        private FiltroFechamentoReaberturaNotificacaoDto CriarFiltro(bool ehParaUe)
        {
            return new FiltroFechamentoReaberturaNotificacaoDto(
                dreCodigo: "DRE123",
                ueCodigo: "UE456",
                id: 1,
                codigoRf: "RF789",
                tipoCalendarioNome: "Regular",
                ueNome: "UE Teste",
                dreAbreviacao: "DRE Teste",
                inicio: DateTime.Now,
                fim: DateTime.Now.AddDays(1),
                bimestreNome: "1º Bimestre",
                ehParaUe: ehParaUe,
                anoLetivo: 2025,
                modalidades: new int[] { 1, 2 }
            );
        }
    }  
}
