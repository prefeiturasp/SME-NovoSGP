using MediatR;
using Moq;
using Newtonsoft.Json;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.ProdutividadeFrequencia
{
    public class ConsolidarInformacoesProdutividadeFrequenciaUeUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly Mock<IRepositorioRegistroAcaoBuscaAtiva> repositorioBuscaAtivaMock;
        private readonly ConsolidarInformacoesProdutividadeFrequenciaUeUseCase useCase;

        public ConsolidarInformacoesProdutividadeFrequenciaUeUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            repositorioBuscaAtivaMock = new Mock<IRepositorioRegistroAcaoBuscaAtiva>();
            useCase = new ConsolidarInformacoesProdutividadeFrequenciaUeUseCase(mediatorMock.Object, repositorioBuscaAtivaMock.Object);
        }

        [Fact]
        public void Construtor_Quando_Mediator_Nulo_Deve_Lancar_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>("mediator",
                () => new ConsolidarInformacoesProdutividadeFrequenciaUeUseCase(null, repositorioBuscaAtivaMock.Object));
        }

        [Fact]
        public void Construtor_Quando_RepositorioBuscaAtiva_Nulo_Deve_Lancar_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>("repositorioBuscaAtiva",
                () => new ConsolidarInformacoesProdutividadeFrequenciaUeUseCase(mediatorMock.Object, null));
        }

        [Fact]
        public async Task Executar_Quando_Chamado_Deve_Obter_Ue_Excluir_Consolidacao_E_Publicar_Para_Bimestres()
        {
            var ano = 2023;
            var filtro = new FiltroIdAnoLetivoDto(123, new DateTime(ano, 1, 1));
            var mensagem = new MensagemRabbit(JsonConvert.SerializeObject(filtro));
            var ueDto = new DreUeCodigoDto { UeCodigo = "UE123", DreCodigo = "DRE456" };

            mediatorMock.Setup(m => m.Send(
                It.Is<ObterCodigoUEDREPorIdQuery>(q => q.UeId == filtro.Id),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(ueDto);

            mediatorMock.Setup(m => m.Send(
                It.Is<ExcluirConsolidacoesProdutividadeFrequenciaUeAnoLetivoCommand>(c =>
                    c.UeCodigo == ueDto.UeCodigo &&
                    c.AnoLetivo == ano),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            mediatorMock.Setup(m => m.Send(
                It.Is<PublicarFilaSgpCommand>(c => c.Rota == RotasRabbitSgpFrequencia.ConsolidarInformacoesProdutividadeFrequenciaBimestre),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var resultado = await useCase.Executar(mensagem);

            Assert.True(resultado);

            mediatorMock.Verify(m => m.Send(
                It.Is<ObterCodigoUEDREPorIdQuery>(q => q.UeId == filtro.Id),
                It.IsAny<CancellationToken>()), Times.Once);

            mediatorMock.Verify(m => m.Send(
                It.Is<ExcluirConsolidacoesProdutividadeFrequenciaUeAnoLetivoCommand>(c =>
                    c.UeCodigo == ueDto.UeCodigo &&
                    c.AnoLetivo == ano),
                It.IsAny<CancellationToken>()), Times.Once);

            mediatorMock.Verify(m => m.Send(
                It.Is<PublicarFilaSgpCommand>(c =>
                    c.Rota == RotasRabbitSgpFrequencia.ConsolidarInformacoesProdutividadeFrequenciaBimestre &&
                    ((FiltroConsolicacaoProdutividadeFrequenciaUeBimestreDTO)c.Filtros).Bimestre == 1 &&
                    ((FiltroConsolicacaoProdutividadeFrequenciaUeBimestreDTO)c.Filtros).AnoLetivo == ano &&
                    ((FiltroConsolicacaoProdutividadeFrequenciaUeBimestreDTO)c.Filtros).CodigoUe == ueDto.UeCodigo),
                It.IsAny<CancellationToken>()), Times.Once);

            mediatorMock.Verify(m => m.Send(
                It.Is<PublicarFilaSgpCommand>(c =>
                    c.Rota == RotasRabbitSgpFrequencia.ConsolidarInformacoesProdutividadeFrequenciaBimestre &&
                    ((FiltroConsolicacaoProdutividadeFrequenciaUeBimestreDTO)c.Filtros).Bimestre == 2),
                It.IsAny<CancellationToken>()), Times.Once);

            mediatorMock.Verify(m => m.Send(
                It.Is<PublicarFilaSgpCommand>(c =>
                    c.Rota == RotasRabbitSgpFrequencia.ConsolidarInformacoesProdutividadeFrequenciaBimestre &&
                    ((FiltroConsolicacaoProdutividadeFrequenciaUeBimestreDTO)c.Filtros).Bimestre == 3),
                It.IsAny<CancellationToken>()), Times.Once);

            mediatorMock.Verify(m => m.Send(
                It.Is<PublicarFilaSgpCommand>(c =>
                    c.Rota == RotasRabbitSgpFrequencia.ConsolidarInformacoesProdutividadeFrequenciaBimestre &&
                    ((FiltroConsolicacaoProdutividadeFrequenciaUeBimestreDTO)c.Filtros).Bimestre == 4),
                It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
