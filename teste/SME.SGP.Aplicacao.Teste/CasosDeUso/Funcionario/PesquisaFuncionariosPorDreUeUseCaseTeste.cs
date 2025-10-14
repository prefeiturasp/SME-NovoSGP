using MediatR;
using Moq;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Funcionario
{
    public class PesquisaFuncionariosPorDreUeUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly PesquisaFuncionariosPorDreUeUseCase _useCase;

        public PesquisaFuncionariosPorDreUeUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new PesquisaFuncionariosPorDreUeUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Quando_Limite_Zerado_Deve_Aplicar_Limite_Padrao_E_Retornar_Paginado()
        {
            var request = new FiltroPesquisaFuncionarioDto { Limite = 0 };
            var funcionariosMock = new List<UsuarioEolRetornoDto>();
            for (int i = 1; i <= 15; i++)
            {
                funcionariosMock.Add(new UsuarioEolRetornoDto { NomeServidor = $"Funcionario Z{15 - i}" });
            }

            _mediatorMock.Setup(m => m.Send(It.IsAny<PesquisaFuncionariosPorDreUeQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(funcionariosMock);

            var resultado = await _useCase.Executar(request);

            Assert.NotNull(resultado);
            Assert.Equal(10, resultado.Items.Count());
            Assert.Equal(10, resultado.TotalRegistros);
            Assert.Equal("Funcionario Z0", resultado.Items.First().NomeServidor);
        }

        [Fact]
        public async Task Executar_Quando_Limite_Informado_Deve_Aplicar_Limite_Correto_E_Retornar_Paginado()
        {
            var limiteInformado = 5;
            var request = new FiltroPesquisaFuncionarioDto { Limite = limiteInformado };
            var funcionariosMock = new List<UsuarioEolRetornoDto>();
            for (int i = 1; i <= 8; i++)
            {
                funcionariosMock.Add(new UsuarioEolRetornoDto { NomeServidor = $"Servidor B{8 - i}" });
            }

            _mediatorMock.Setup(m => m.Send(It.IsAny<PesquisaFuncionariosPorDreUeQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(funcionariosMock);

            var resultado = await _useCase.Executar(request);

            Assert.NotNull(resultado);
            Assert.Equal(limiteInformado, resultado.Items.Count());
            Assert.Equal(limiteInformado, resultado.TotalRegistros);
            Assert.Equal("Servidor B0", resultado.Items.First().NomeServidor);
        }

        [Fact]
        public async Task Executar_Quando_Total_Funcionarios_Menor_Que_Limite_Deve_Retornar_Total_Correto()
        {
            var request = new FiltroPesquisaFuncionarioDto { Limite = 20 };
            var funcionariosMock = new List<UsuarioEolRetornoDto>
            {
                new UsuarioEolRetornoDto { NomeServidor = "Maria" },
                new UsuarioEolRetornoDto { NomeServidor = "Ana" },
                new UsuarioEolRetornoDto { NomeServidor = "Zelia" }
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<PesquisaFuncionariosPorDreUeQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(funcionariosMock);

            var resultado = await _useCase.Executar(request);

            Assert.NotNull(resultado);
            Assert.Equal(funcionariosMock.Count, resultado.Items.Count());
            Assert.Equal(funcionariosMock.Count, resultado.TotalRegistros);
            Assert.Equal("Ana", resultado.Items.First().NomeServidor);
        }
    }
}
