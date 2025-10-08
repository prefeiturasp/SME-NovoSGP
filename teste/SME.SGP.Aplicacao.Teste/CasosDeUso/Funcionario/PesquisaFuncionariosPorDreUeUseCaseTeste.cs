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
        public async Task Executar_Quando_Limite_Informado_E_Resultados_Maiores_Deve_Retornar_Paginado_Corretamente()
        {
            var request = new FiltroPesquisaFuncionarioDto { Limite = 3 };
            var funcionarios = new List<UsuarioEolRetornoDto>
        {
            new UsuarioEolRetornoDto { NomeServidor = "Zelia" },
            new UsuarioEolRetornoDto { NomeServidor = "Ana" },
            new UsuarioEolRetornoDto { NomeServidor = "Carlos" },
            new UsuarioEolRetornoDto { NomeServidor = "Beatriz" },
        };

            _mediatorMock.Setup(m => m.Send(It.IsAny<PesquisaFuncionariosPorDreUeQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(funcionarios);

            var resultado = await _useCase.Executar(request);

            Assert.NotNull(resultado);
            Assert.Equal(3, resultado.Items.Count());
            Assert.Equal(3, resultado.TotalRegistros);
            Assert.Equal(1, resultado.TotalPaginas);
            Assert.Equal("Ana", resultado.Items.ElementAt(0).NomeServidor);
            Assert.Equal("Beatriz", resultado.Items.ElementAt(1).NomeServidor);
            Assert.Equal("Carlos", resultado.Items.ElementAt(2).NomeServidor);
        }

        [Fact]
        public async Task Executar_Quando_Limite_Nao_Informado_E_Resultados_Menores_Deve_Retornar_Paginado_Corretamente()
        {
            var request = new FiltroPesquisaFuncionarioDto { Limite = 0 }; 
            var funcionarios = new List<UsuarioEolRetornoDto>
            {
                new UsuarioEolRetornoDto { NomeServidor = "Marcos" },
                new UsuarioEolRetornoDto { NomeServidor = "Fabio" },
                new UsuarioEolRetornoDto { NomeServidor = "Julia" }
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<PesquisaFuncionariosPorDreUeQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(funcionarios);

            var resultado = await _useCase.Executar(request);

            Assert.NotNull(resultado);
            Assert.Equal(3, resultado.Items.Count());
            Assert.Equal(3, resultado.TotalRegistros);
            Assert.Equal(1, resultado.TotalPaginas);
            Assert.Equal("Fabio", resultado.Items.ElementAt(0).NomeServidor);
            Assert.Equal("Julia", resultado.Items.ElementAt(1).NomeServidor);
            Assert.Equal("Marcos", resultado.Items.ElementAt(2).NomeServidor);
        }
    }
}
