using FluentAssertions;
using MediatR;
using Moq;
using SME.SGP.Infra.Dtos.EscolaAqui.DashboardAdesao;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.EscolaAqui.Dashboard.ObterTotaisAdesaoAgrupadosPorDre
{
    public class ObterTotaisAdesaoAgrupadosPorDreUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly ObterTotaisAdesaoAgrupadosPorDreUseCase useCase;

        public ObterTotaisAdesaoAgrupadosPorDreUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new ObterTotaisAdesaoAgrupadosPorDreUseCase(mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Deve_Retornar_Totais_Adesao_Agrupados_Por_Dre()
        {
            var resultadoEsperado = new List<TotaisAdesaoAgrupadoProDreResultado>
            {
                new TotaisAdesaoAgrupadoProDreResultado
                {
                    NomeCompletoDre = "DRE Penha",
                    TotalUsuariosComCpfInvalidos = 2,
                    TotalUsuariosPrimeiroAcessoIncompleto = 5,
                    TotalUsuariosSemAppInstalado = 10,
                    TotalUsuariosValidos = 50
                },
                new TotaisAdesaoAgrupadoProDreResultado
                {
                    NomeCompletoDre = "DRE Ipiranga",
                    TotalUsuariosComCpfInvalidos = 1,
                    TotalUsuariosPrimeiroAcessoIncompleto = 3,
                    TotalUsuariosSemAppInstalado = 7,
                    TotalUsuariosValidos = 40
                }
            };

            mediatorMock
                .Setup(m => m.Send(It.Is<ObterTotaisAdesaoAgrupadosPorDreQuery>(q => q == ObterTotaisAdesaoAgrupadosPorDreQuery.Instance), It.IsAny<CancellationToken>()))
                .ReturnsAsync(resultadoEsperado);

            var resultado = await useCase.Executar();

            resultado.Should().NotBeNull();
            resultado.Should().HaveCount(2);
            resultado.Should().BeEquivalentTo(resultadoEsperado);
        }

        [Fact]
        public async Task Executar_Deve_Retornar_Lista_Vazia_Quando_Nao_Houver_Dados()
        {
            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterTotaisAdesaoAgrupadosPorDreQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<TotaisAdesaoAgrupadoProDreResultado>());

            var resultado = await useCase.Executar();

            resultado.Should().NotBeNull();
            resultado.Should().BeEmpty();
        }
    }
}
