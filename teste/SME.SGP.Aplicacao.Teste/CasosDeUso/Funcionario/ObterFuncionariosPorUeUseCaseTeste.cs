using MediatR;
using Moq;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Funcionario
{
    public class ObterFuncionariosPorUeUseCaseTeste
    {
        [Fact]
        public void Construtor_Quando_Mediator_Nulo_Deve_Lancar_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new ObterFuncionariosPorUeUseCase(null));
        }

        [Fact]
        public async Task Executar_Quando_Valido_Deve_Chamar_Query_E_Retornar_Funcionarios()
        {
            var mediatorMock = new Mock<IMediator>();
            var useCase = new ObterFuncionariosPorUeUseCase(mediatorMock.Object);

            var codigoUe = "012345";
            var filtro = "Teste";
            var funcionariosRetorno = new List<UsuarioEolRetornoDto>
            {
                new UsuarioEolRetornoDto { CodigoRf = "111", NomeServidor = "Servidor Teste 1" },
                new UsuarioEolRetornoDto { CodigoRf = "222", NomeServidor = "Servidor Teste 2" }
            };

            mediatorMock.Setup(m => m.Send(It.Is<ObterFuncionariosPorUeQuery>(q => q.CodigoUe == codigoUe && q.Filtro == filtro), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(funcionariosRetorno);

            var resultado = await useCase.Executar(codigoUe, filtro);

            Assert.NotNull(resultado);
            Assert.Equal(funcionariosRetorno, resultado);
            mediatorMock.Verify(m => m.Send(It.Is<ObterFuncionariosPorUeQuery>(q => q.CodigoUe == codigoUe && q.Filtro == filtro), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
