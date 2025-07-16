using Bogus;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SME.SGP.Api.Controllers;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Api.Testes.Controllers
{
    public class ABAEControllerTeste
    {
        private readonly ABAEController _controller;
        private readonly Faker _faker;

        public ABAEControllerTeste()
        {
            _controller = new ABAEController();
            _faker = new Faker("pt_BR");
        }

        [Fact(DisplayName = "Deve chamar o caso de uso para incluir um cadastro ABAE")]
        public async Task DeveChamarCasoDeUso_ParaIncluirCadastroABAE()
        {
            // Arrange
            var salvarCadastroUseCaseMock = new Mock<ISalvarCadastroAcessoABAEUseCase>();
            var cadastroDto = new CadastroAcessoABAEDto { Nome = _faker.Name.FullName() };
            var retornoDto = new CadastroAcessoABAEDto { Id = _faker.Random.Long(1), Nome = cadastroDto.Nome };

            salvarCadastroUseCaseMock.Setup(x => x.Executar(cadastroDto)).ReturnsAsync(retornoDto);

            // Act
            var resultado = await _controller.Incluir(cadastroDto, salvarCadastroUseCaseMock.Object);

            // Assert
            salvarCadastroUseCaseMock.Verify(x => x.Executar(cadastroDto), Times.Once);
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            var valorRetornado = Assert.IsType<CadastroAcessoABAEDto>(okResult.Value);
            Assert.Equal(retornoDto.Id, valorRetornado.Id);
        }

        [Fact(DisplayName = "Deve chamar o caso de uso para alterar um cadastro ABAE")]
        public async Task DeveChamarCasoDeUso_ParaAlterarCadastroABAE()
        {
            // Arrange
            var salvarCadastroUseCaseMock = new Mock<ISalvarCadastroAcessoABAEUseCase>();
            var cadastroDto = new CadastroAcessoABAEDto { Id = _faker.Random.Long(1), Nome = _faker.Name.FullName() };

            salvarCadastroUseCaseMock.Setup(x => x.Executar(cadastroDto)).ReturnsAsync(cadastroDto);

            // Act
            var resultado = await _controller.Alterar(cadastroDto, salvarCadastroUseCaseMock.Object);

            // Assert
            salvarCadastroUseCaseMock.Verify(x => x.Executar(cadastroDto), Times.Once);
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.NotNull(okResult.Value);
        }

        [Fact(DisplayName = "Deve chamar o caso de uso para buscar um cadastro ABAE por ID")]
        public async Task DeveChamarCasoDeUso_ParaBuscarPorId()
        {
            // Arrange
            var obterCadastroUseCaseMock = new Mock<IObterCadastroAcessoABAEUseCase>();
            var id = _faker.Random.Int(1, 100);
            var retornoDto = new CadastroAcessoABAEDto { Id = id, Nome = _faker.Name.FullName() };

            obterCadastroUseCaseMock.Setup(x => x.Executar(id)).ReturnsAsync(retornoDto);

            // Act
            var resultado = await _controller.BuscarPorId(id, obterCadastroUseCaseMock.Object);

            // Assert
            obterCadastroUseCaseMock.Verify(x => x.Executar(id), Times.Once);
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            var valorRetornado = Assert.IsType<CadastroAcessoABAEDto>(okResult.Value);
            Assert.Equal(id, valorRetornado.Id);
        }

        [Fact(DisplayName = "Deve chamar o caso de uso para excluir um cadastro ABAE")]
        public async Task DeveChamarCasoDeUso_ParaExcluirCadastroABAE()
        {
            // Arrange
            var excluirCadastroUseCaseMock = new Mock<IExcluirCadastroAcessoABAEUseCase>();
            var id = _faker.Random.Long(1, 100);

            excluirCadastroUseCaseMock.Setup(x => x.Executar(id)).ReturnsAsync(true);

            // Act
            var resultado = await _controller.Excluir(id, excluirCadastroUseCaseMock.Object);

            // Assert
            excluirCadastroUseCaseMock.Verify(x => x.Executar(id), Times.Once);
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Equal(true, okResult.Value);
        }

        [Fact(DisplayName = "Deve chamar o caso de uso para buscar cadastros ABAE de forma paginada")]
        public async Task DeveChamarCasoDeUso_ParaBuscarPaginada()
        {
            // Arrange
            var obterPaginadoUseCaseMock = new Mock<IObterPaginadoCadastroAcessoABAEUseCase>();
            var filtro = new FiltroDreIdUeIdNomeSituacaoABAEDto { Nome = _faker.Name.JobTitle() };
            var retornoPaginado = new PaginacaoResultadoDto<DreUeNomeSituacaoABAEDto>();

            obterPaginadoUseCaseMock.Setup(x => x.Executar(filtro)).ReturnsAsync(retornoPaginado);

            // Act
            var resultado = await _controller.BuscarPaginada(filtro, obterPaginadoUseCaseMock.Object);

            // Assert
            obterPaginadoUseCaseMock.Verify(x => x.Executar(filtro), Times.Once);
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Same(retornoPaginado, okResult.Value);
        }

        [Fact(DisplayName = "Deve chamar o caso de uso para obter funcionários ABAE")]
        public async Task DeveChamarCasoDeUso_ParaObterFuncionarios()
        {
            // Arrange
            var obterFuncionariosUseCaseMock = new Mock<IObterFuncionariosABAEUseCase>();
            var codigoDre = _faker.Random.AlphaNumeric(6);
            var codigoUe = _faker.Random.AlphaNumeric(6);
            var codigoRf = _faker.Random.AlphaNumeric(7);
            var nomeServidor = _faker.Name.FullName();
            var retornoFuncionarios = new List<NomeCpfABAEDto>();

            obterFuncionariosUseCaseMock.Setup(x => x.Executar(It.Is<FiltroFuncionarioDto>(f =>
                f.CodigoDRE == codigoDre &&
                f.CodigoUE == codigoUe &&
                f.CodigoRF == codigoRf &&
                f.NomeServidor == nomeServidor
            ))).ReturnsAsync(retornoFuncionarios);

            // Act
            var resultado = await _controller.ObterFuncionarios(codigoDre, codigoUe, codigoRf, nomeServidor, obterFuncionariosUseCaseMock.Object);

            // Assert
            obterFuncionariosUseCaseMock.Verify(x => x.Executar(It.Is<FiltroFuncionarioDto>(f =>
                f.CodigoDRE == codigoDre &&
                f.CodigoUE == codigoUe &&
                f.CodigoRF == codigoRf &&
                f.NomeServidor == nomeServidor
            )), Times.Once);

            var okResult = Assert.IsType<OkObjectResult>(resultado);
            Assert.Same(retornoFuncionarios, okResult.Value);
        }
    }
}