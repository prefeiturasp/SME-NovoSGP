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
        public BoletimSimplesUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            boletimSimplesUseCase = new BoletimUseCase(mediator.Object);
        }

        [Fact]
        public async Task Deve_Obter_Boletim_Fundamental()
        {
            // arrange

            var filtroBoletim = new FiltroRelatorioBoletimDto()
            {
                DreCodigo = "111",
                UeCodigo = "222",
                Semestre = 0,
                TurmaCodigo = "333",
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
                    CodigoTurma = "333",
                    Id = 123123,
                    Nome = "X",
                    Ano = "7",
                    AnoLetivo = 2020,
                    ModalidadeCodigo = Modalidade.Fundamental
                });

            var usuarioLogado = new Usuario()
            {
                CodigoRf = "11111",
                Nome = "Professor"
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
                DreCodigo = "111",
                UeCodigo = "222",
                Semestre = 0,
                TurmaCodigo = "333",
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
                    CodigoTurma = "333",
                    Id = 123123,
                    Nome = "A",
                    Ano = "1",
                    AnoLetivo = 2020,
                    ModalidadeCodigo = Modalidade.Medio
                });

            var usuarioLogado = new Usuario()
            {
                CodigoRf = "222222",
                Nome = "Admin"
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
                DreCodigo = "111",
                UeCodigo = "222",
                Semestre = 0,
                TurmaCodigo = "333",
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
                    CodigoTurma = "333",
                    Id = 123123,
                    Nome = "B",
                    Ano = "3",
                    AnoLetivo = 2020,
                    EtapaEJA = 2,
                    ModalidadeCodigo = Modalidade.EJA
                });

            var usuarioLogado = new Usuario()
            {
                CodigoRf = "222222",
                Nome = "Admin"
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
