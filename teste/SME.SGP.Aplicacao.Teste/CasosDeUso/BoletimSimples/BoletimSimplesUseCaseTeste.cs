using MediatR;
using Moq;
using SME.SGP.Aplicacao.CasosDeUso;
using SME.SGP.Aplicacao.Queries;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao
{
    public class BoletimSimplesUseCaseTeste
    {
        private readonly BoletimUseCase boletimSimplesUseCase;
        private readonly Mock<IMediator> mediator;
        private readonly Mock<IUnitOfWork> unitOfWork;

        public BoletimSimplesUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            unitOfWork = new Mock<IUnitOfWork>();
            boletimSimplesUseCase = new BoletimUseCase(mediator.Object, unitOfWork.Object);
        }

        [Fact]
        public async Task Deve_Obter_Boletim_Fundamental()
        {
            // arrange

            var filtroBoletim = new FiltroRelatorioBoletimDto()
            {
                DreCodigo = "108800",
                UeCodigo = "094765",
                Semestre = 0,
                TurmaCodigo = "2117443",
                AnoLetivo = 2020,
                Modalidade = Modalidade.Fundamental,
                Modelo = ModeloBoletim.Simples,
                ConsideraHistorico = true,
                QuantidadeBoletimPorPagina = 1
            };

            mediator.Setup(a => a.Send(It.IsAny<ValidaSeExisteUePorCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);
            mediator.Setup(a => a.Send(It.IsAny<ValidaSeExisteDrePorCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);
            mediator.Setup(a => a.Send(It.IsAny<ObterTurmaPorCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Turma()
                {
                    CodigoTurma = "2117443",
                    Id = 310712,
                    Nome = "7B",
                    Ano = "7",
                    AnoLetivo = 2020,
                    ModalidadeCodigo = Modalidade.Fundamental
                });

            var usuarioLogado = new Usuario()
            {
                CodigoRf = "6769195",
                Nome = "Adalgisa"
            };

            mediator.Setup(a => a.Send(It.IsAny<ObterUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(usuarioLogado);

            mediator.Setup(a => a.Send(It.IsAny<GerarRelatorioCommand>(), It.IsAny<CancellationToken>()))
                 .ReturnsAsync(true);

            // act
            var anotacao = await boletimSimplesUseCase.Executar(filtroBoletim);

            // assert
            Assert.True(anotacao && filtroBoletim.Modalidade == Modalidade.Fundamental);
        }

        [Fact]
        public async Task Deve_Obter_Boletim_EM()
        {
            // arrange

            var filtroBoletim = new FiltroRelatorioBoletimDto()
            {
                DreCodigo = "109000",
                UeCodigo = "094609",
                Semestre = 0,
                TurmaCodigo = "2112419",
                AnoLetivo = 2020,
                Modalidade = Modalidade.Medio,
                Modelo = ModeloBoletim.Simples,
                ConsideraHistorico = true,
                QuantidadeBoletimPorPagina = 1
            };

            mediator.Setup(a => a.Send(It.IsAny<ValidaSeExisteUePorCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);
            mediator.Setup(a => a.Send(It.IsAny<ValidaSeExisteDrePorCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);
            mediator.Setup(a => a.Send(It.IsAny<ObterTurmaPorCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Turma()
                {
                    CodigoTurma = "2112419",
                    Id = 312783,
                    Nome = "1A",
                    Ano = "1",
                    AnoLetivo = 2020,
                    ModalidadeCodigo = Modalidade.Medio
                });

            var usuarioLogado = new Usuario()
            {
                CodigoRf = "7924488",
                Nome = "Gabriella"
            };

            mediator.Setup(a => a.Send(It.IsAny<ObterUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(usuarioLogado);

            mediator.Setup(a => a.Send(It.IsAny<GerarRelatorioCommand>(), It.IsAny<CancellationToken>()))
                 .ReturnsAsync(true);

            // act
            var anotacao = await boletimSimplesUseCase.Executar(filtroBoletim);

            // assert
            Assert.True(anotacao && filtroBoletim.Modalidade == Modalidade.Medio);
        }

        [Fact]
        public async Task Deve_Obter_Boletim_EJA()
        {
            // arrange

            var filtroBoletim = new FiltroRelatorioBoletimDto()
            {
                DreCodigo = "108400",
                UeCodigo = "094340",
                Semestre = 0,
                TurmaCodigo = "2222401",
                AnoLetivo = 2020,
                Modalidade = Modalidade.EJA,
                Modelo = ModeloBoletim.Simples,
                ConsideraHistorico = true,
                QuantidadeBoletimPorPagina = 1
            };

            mediator.Setup(a => a.Send(It.IsAny<ValidaSeExisteUePorCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);
            mediator.Setup(a => a.Send(It.IsAny<ValidaSeExisteDrePorCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);
            mediator.Setup(a => a.Send(It.IsAny<ObterTurmaPorCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Turma()
                {
                    CodigoTurma = "2222401",
                    Id = 310712,
                    Nome = "3B",
                    Ano = "3",
                    AnoLetivo = 2020,
                    EtapaEJA = 2,
                    ModalidadeCodigo = Modalidade.EJA
                });

            var usuarioLogado = new Usuario()
            {
                CodigoRf = "7924488",
                Nome = "Gabriella"
            };

            mediator.Setup(a => a.Send(It.IsAny<ObterUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(usuarioLogado);

            mediator.Setup(a => a.Send(It.IsAny<GerarRelatorioCommand>(), It.IsAny<CancellationToken>()))
                 .ReturnsAsync(true);

            // act
            var anotacao = await boletimSimplesUseCase.Executar(filtroBoletim);

            // assert
            Assert.True(anotacao && filtroBoletim.Modalidade == Modalidade.EJA);
        }
    }
}
