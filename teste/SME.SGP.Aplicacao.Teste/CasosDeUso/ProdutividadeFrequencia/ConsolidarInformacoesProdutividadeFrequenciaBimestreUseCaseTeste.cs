using MediatR;
using Moq;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.ProdutividadeFrequencia
{
    public class ConsolidarInformacoesProdutividadeFrequenciaBimestreUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly Mock<IRepositorioFrequenciaConsulta> repositorioFrequenciaMock;
        private readonly ConsolidarInformacoesProdutividadeFrequenciaBimestreUseCase useCase;

        public ConsolidarInformacoesProdutividadeFrequenciaBimestreUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            repositorioFrequenciaMock = new Mock<IRepositorioFrequenciaConsulta>();
            useCase = new ConsolidarInformacoesProdutividadeFrequenciaBimestreUseCase(mediatorMock.Object, repositorioFrequenciaMock.Object);
        }

        [Fact]
        public void Construtor_Quando_Mediator_Nulo_Deve_Lancar_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>("mediator",
                () => new ConsolidarInformacoesProdutividadeFrequenciaBimestreUseCase(null, repositorioFrequenciaMock.Object));
        }

        [Fact]
        public void Construtor_Quando_RepositorioFrequencia_Nulo_Deve_Lancar_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>("repositorioFrequencia",
                () => new ConsolidarInformacoesProdutividadeFrequenciaBimestreUseCase(mediatorMock.Object, null));
        }

        [Fact]
        public async Task Executar_Quando_Repositorio_Retorna_Registros_Deve_Enviar_Comandos_E_Retornar_True()
        {
            var filtro = new FiltroConsolicacaoProdutividadeFrequenciaUeBimestreDTO
            {
                AnoLetivo = 2023,
                CodigoUe = "123",
                Bimestre = 1
            };
            var mensagem = new MensagemRabbit(JsonConvert.SerializeObject(filtro));

            var registros = new List<RegistroFrequenciaProdutividadeDto>
            {
                new RegistroFrequenciaProdutividadeDto { ProfRf = "A", Modalidade = Modalidade.EducacaoInfantil },
                new RegistroFrequenciaProdutividadeDto { ProfRf = "B", Modalidade = Modalidade.Fundamental }
            };

            repositorioFrequenciaMock.Setup(r => r.ObterInformacoesProdutividadeFrequencia(
                filtro.AnoLetivo, filtro.CodigoUe, filtro.Bimestre))
                .ReturnsAsync(registros);

            mediatorMock.Setup(m => m.Send(It.IsAny<SalvarConsolidacaoProdutividadeFrequenciaCommand>(), default))
                .ReturnsAsync(1L);

            var resultado = await useCase.Executar(mensagem);

            Assert.True(resultado);

            repositorioFrequenciaMock.Verify(r => r.ObterInformacoesProdutividadeFrequencia(
                filtro.AnoLetivo, filtro.CodigoUe, filtro.Bimestre), Times.Once);

            mediatorMock.Verify(m => m.Send(
                It.IsAny<SalvarConsolidacaoProdutividadeFrequenciaCommand>(),
                It.IsAny<CancellationToken>()), Times.Exactly(registros.Count));
        }

        [Fact]
        public async Task Executar_Quando_Repositorio_Retorna_Vazio_Deve_Nao_Enviar_Comandos_E_Retornar_True()
        {
            var filtro = new FiltroConsolicacaoProdutividadeFrequenciaUeBimestreDTO
            {
                AnoLetivo = 2023,
                CodigoUe = "123",
                Bimestre = 1
            };
            var mensagem = new MensagemRabbit(JsonConvert.SerializeObject(filtro));

            var registros = Enumerable.Empty<RegistroFrequenciaProdutividadeDto>();

            repositorioFrequenciaMock.Setup(r => r.ObterInformacoesProdutividadeFrequencia(
                filtro.AnoLetivo, filtro.CodigoUe, filtro.Bimestre))
                .ReturnsAsync(registros);

            var resultado = await useCase.Executar(mensagem);

            Assert.True(resultado);

            repositorioFrequenciaMock.Verify(r => r.ObterInformacoesProdutividadeFrequencia(
                filtro.AnoLetivo, filtro.CodigoUe, filtro.Bimestre), Times.Once);

            mediatorMock.Verify(m => m.Send(
                It.IsAny<SalvarConsolidacaoProdutividadeFrequenciaCommand>(),
                It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
