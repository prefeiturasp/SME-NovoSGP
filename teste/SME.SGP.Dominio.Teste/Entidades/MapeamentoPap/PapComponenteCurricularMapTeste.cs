using SME.SGP.Dominio.Constantes;
using SME.SGP.Dominio.Entidades.MapeamentoPap;
using System.Linq;
using Xunit;

namespace SME.SGP.Dominio.Teste
{
    public class PapComponenteCurricularMapTeste
    {
        [Theory(DisplayName = "Deve retornar o TipoPap correto para cada componente curricular")]
        [InlineData(ComponentesCurricularesConstants.CODIGO_PAP_PROJETO_COLABORATIVO, TipoPap.PapColaborativo)]
        [InlineData(ComponentesCurricularesConstants.CODIGO_PAP_RECUPERACAO_APRENDIZAGENS, TipoPap.RecuperacaoAprendizagens)]
        [InlineData(ComponentesCurricularesConstants.CODIGO_PAP_2_ANO_ALFABETIZACAO, TipoPap.Pap2Ano)]
        [InlineData(ComponentesCurricularesConstants.CODIGO_PAP_2_ANO_COLABORATIVO_ALFABETIZACAO, TipoPap.Pap2Ano)]
        public void ObterTipoPapPorComponente_DeveRetornarTipoPapCorreto(long componenteId, TipoPap tipoPapEsperado)
        {
            // Act
            var resultado = PapComponenteCurricularMap.ObterTipoPapPorComponente(componenteId);

            // Assert
            Assert.Equal(tipoPapEsperado, resultado);
        }

        [Fact(DisplayName = "Deve retornar o tipo default (0) para componente não mapeado")]
        public void ObterTipoPapPorComponente_QuandoComponenteNaoMapeado_DeveRetornarTipoDefault()
        {
            // Arrange
            const long componenteInexistente = 99999;

            // Act
            var resultado = PapComponenteCurricularMap.ObterTipoPapPorComponente(componenteInexistente);

            // Assert
            Assert.Equal((TipoPap)0, resultado);
        }

        [Fact(DisplayName = "Deve retornar os dois componentes para o TipoPap Pap2Ano")]
        public void ObterComponentesPorTipoPap_QuandoPap2Ano_DeveRetornarDoisComponentes()
        {
            // Arrange
            var tipoPap = TipoPap.Pap2Ano;
            var componentesEsperados = new[]
            {
                ComponentesCurricularesConstants.CODIGO_PAP_2_ANO_ALFABETIZACAO,
                ComponentesCurricularesConstants.CODIGO_PAP_2_ANO_COLABORATIVO_ALFABETIZACAO
            };

            // Act
            var resultado = PapComponenteCurricularMap.ObterComponentesPorTipoPap(tipoPap);

            // Assert
            Assert.NotEmpty(resultado);
            Assert.Equal(2, resultado.Count());
            Assert.All(componentesEsperados, componente => Assert.Contains(componente, resultado));
        }

        [Fact(DisplayName = "Deve retornar o componente único para PapColaborativo")]
        public void ObterComponentesPorTipoPap_QuandoPapColaborativo_DeveRetornarUmComponente()
        {
            // Arrange
            var tipoPap = TipoPap.PapColaborativo;

            // Act
            var resultado = PapComponenteCurricularMap.ObterComponentesPorTipoPap(tipoPap);

            // Assert
            Assert.NotEmpty(resultado);
            Assert.Equal(ComponentesCurricularesConstants.CODIGO_PAP_PROJETO_COLABORATIVO, resultado.SingleOrDefault());
        }

        [Fact(DisplayName = "Deve retornar lista vazia para TipoPap não mapeado")]
        public void ObterComponentesPorTipoPap_QuandoTipoPapNaoMapeado_DeveRetornarListaVazia()
        {
            // Arrange
            var tipoPapInexistente = (TipoPap)99;

            // Act
            var resultado = PapComponenteCurricularMap.ObterComponentesPorTipoPap(tipoPapInexistente);

            // Assert
            Assert.Empty(resultado);
        }
    }
}
