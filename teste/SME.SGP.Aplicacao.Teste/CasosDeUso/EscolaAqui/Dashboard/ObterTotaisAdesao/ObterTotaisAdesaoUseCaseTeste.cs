using FluentAssertions;
using MediatR;
using Moq;
using SME.SGP.Infra.Dtos.EscolaAqui.DashboardAdesao;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.EscolaAqui.Dashboard.ObterTotaisAdesao
{
    public class ObterTotaisAdesaoUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly ObterTotaisAdesaoUseCase useCase;

        public ObterTotaisAdesaoUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new ObterTotaisAdesaoUseCase(mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Deve_Retornar_Totais_Adesao()
        {
            var codigoDre = "01";
            var codigoUe = "0001";

            var resultadoEsperado = new List<TotaisAdesaoResultado>
            {
                new TotaisAdesaoResultado
                {
                    NomeCompletoDre = "DRE SUL",
                    NomeCompletoUe = "EMEF JOSÉ",
                    Codigoturma = 123,
                    TotalUsuariosComCpfInvalidos = 2,
                    TotalUsuariosPrimeiroAcessoIncompleto = 1,
                    TotalUsuariosSemAppInstalado = 5,
                    TotalUsuariosValidos = 30
                }
            };

            mediatorMock
                .Setup(m => m.Send(It.Is<ObterTotaisAdesaoQuery>(q =>
                        q.CodigoDre == codigoDre &&
                        q.CodigoUe == codigoUe),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(resultadoEsperado);

            var resultado = await useCase.Executar(codigoDre, codigoUe);

            resultado.Should().NotBeNull();
            resultado.Should().HaveCount(1);
            resultado.Should().BeEquivalentTo(resultadoEsperado);
        }

        [Fact]
        public async Task Executar_Deve_Retornar_Lista_Vazia_Quando_Na_oHouver_Dados()
        {
            var codigoDre = "02";
            var codigoUe = "9999";

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterTotaisAdesaoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<TotaisAdesaoResultado>());

            var resultado = await useCase.Executar(codigoDre, codigoUe);

            resultado.Should().NotBeNull();
            resultado.Should().BeEmpty();
        }

        [Fact]
        public void Construtor_Deve_Lancar_Excecao_Se_Mediator_For_Nulo()
        {
            Action acao = () => new ObterTotaisAdesaoUseCase(null);

            acao.Should().Throw<System.ArgumentNullException>()
                .WithParameterName("mediator");
        }
    }
}
