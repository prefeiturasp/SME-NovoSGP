using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Nota
{
    public class ObterNotasFinaisUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ObterNotasFinaisUseCases _useCase;

        public ObterNotasFinaisUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ObterNotasFinaisUseCases(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Deve_Retornar_Grafico_Agrupado_Por_Ano_E_Abaixo_Acima_Media()
        {
            var filtro = new FiltroDashboardFechamentoDto
            {
                UeId = 0,
                DreId = 0,
                AnoLetivo = 6,
                Modalidade = 1,
                Semestre = 1,
                Bimestre = 2
            };

            var retornoNotas = new List<FechamentoConselhoClasseNotaFinalDto>
    {
        new FechamentoConselhoClasseNotaFinalDto { Ano = "1º", Nota = 7.5 },
        new FechamentoConselhoClasseNotaFinalDto { Ano = "1º", Nota = 4.0 },
        new FechamentoConselhoClasseNotaFinalDto { Ano = "2º", Conceito = "NS" },
        new FechamentoConselhoClasseNotaFinalDto { Ano = "2º", Conceito = "BOM" }
    };

            var parametro = new ParametrosSistema
            {
                Valor = "6.0"
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterNotasFinaisFechamentoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(retornoNotas);

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(parametro);

            var resultado = await _useCase.Executar(filtro);

            Assert.Collection(resultado,
                item =>
                {
                    Assert.Equal("1º", item.Grupo);
                    Assert.Equal(2, item.Quantidade);
                    Assert.Equal("Abaixo do mínimo", item.Descricao);
                },
                item =>
                {
                    Assert.Equal("2º", item.Grupo);
                    Assert.Equal(1, item.Quantidade);
                    Assert.Equal("Abaixo do mínimo", item.Descricao);
                },
                item =>
                {
                    Assert.Equal("2º", item.Grupo);
                    Assert.Equal(1, item.Quantidade);
                    Assert.Equal("Acima do mínimo", item.Descricao);
                }
            );
        }
    }
}
