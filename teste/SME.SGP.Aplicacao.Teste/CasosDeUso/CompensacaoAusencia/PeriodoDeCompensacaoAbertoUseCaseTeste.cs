using MediatR;
using Moq;
using SME.SGP.Dominio;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.CompensacaoAusencia
{
    public class PeriodoDeCompensacaoAbertoUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly PeriodoDeCompensacaoAbertoUseCase useCase;

        public PeriodoDeCompensacaoAbertoUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new PeriodoDeCompensacaoAbertoUseCase(mediatorMock.Object);
        }

        [Fact]
        public async Task Verificar_Periodo_Aberto_Deve_Retornar_True_Quando_Parametro_Ativo_E_Turma_No_Ano_Atual_E_Periodo_Aberto()
        {
            var turma = new Turma { AnoLetivo = DateTime.Now.Year, CodigoTurma = "T1" };
            var parametro = new ParametrosSistema { Ativo = true };

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaComUeEDrePorCodigoQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(turma);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(parametro);

            mediatorMock.Setup(m => m.Send(It.IsAny<TurmaEmPeriodoAbertoQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(true);

            var resultado = await useCase.VerificarPeriodoAberto("T1", 1);

            Assert.True(resultado);
        }

        [Fact]
        public async Task Verificar_Periodo_Aberto_Deve_Retornar_False_Quando_Parametro_Sistema_Nulo()
        {
            var turma = new Turma { AnoLetivo = DateTime.Now.Year, CodigoTurma = "T2" };

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaComUeEDrePorCodigoQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(turma);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync((ParametrosSistema)null);

            var resultado = await useCase.VerificarPeriodoAberto("T2", 1);

            Assert.False(resultado);
        }

        [Fact]
        public async Task Verificar_Periodo_Aberto_Deve_Retornar_False_Quando_Parametro_Sistema_Inativo()
        {
            var turma = new Turma { AnoLetivo = DateTime.Now.Year, CodigoTurma = "T3" };
            var parametro = new ParametrosSistema { Ativo = false };

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaComUeEDrePorCodigoQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(turma);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(parametro);

            var resultado = await useCase.VerificarPeriodoAberto("T3", 1);

            Assert.False(resultado);
        }

        [Fact]
        public async Task Verificar_Periodo_Aberto_Deve_Chamar_Periodo_Aberto_Query_Quando_Ano_Letivo_Diferente_Do_Ano_Atual()
        {
            var turma = new Turma { AnoLetivo = DateTime.Now.Year - 1, CodigoTurma = "T4" };
            var parametro = new ParametrosSistema { Ativo = true };

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaComUeEDrePorCodigoQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(turma);

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(parametro);

            mediatorMock.Setup(m => m.Send(It.IsAny<TurmaEmPeriodoAbertoQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(true);

            var resultado = await useCase.VerificarPeriodoAberto("T4", 1);

            Assert.True(resultado);
            mediatorMock.Verify(m => m.Send(It.IsAny<TurmaEmPeriodoAbertoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public void Construtor_Deve_Lancar_Argument_Null_Exception_Quando_Mediator_Nulo()
        {
            Assert.Throws<ArgumentNullException>(() => new PeriodoDeCompensacaoAbertoUseCase(null));
        }
    }
}
