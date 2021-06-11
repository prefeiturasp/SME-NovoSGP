using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dto;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Dtos
{
    public class AbrangenciaTurmaRetornoEolDtoTeste
    {
        [Theory]
        [InlineData(TipoTurma.ClasseBilingueII, "1A", "2", "2")]
        [InlineData(TipoTurma.EdFisica, "1A", "2", "1")]
        [InlineData(TipoTurma.EdFisica, "3A", "7", "3")]
        [InlineData(TipoTurma.EdFisica, "6Z", "1", "6")]
        [InlineData(TipoTurma.EvenParaAtribuicao, "3A", "4", "4")]
        [InlineData(TipoTurma.ItinerarioEnsMedio, "6A", "6", "6")]
        [InlineData(TipoTurma.Itinerarios2AAno, "7A", "2", "2")]
        [InlineData(TipoTurma.Itinerarios2AAno, "3B", "7", "2")]
        [InlineData(TipoTurma.Programa, "3B", "3", "3")]
        [InlineData(TipoTurma.Regular, "1B", "1", "1")]
        public void TestaAnoTipoTurma(TipoTurma tipoTurma, string turmaNome, string ano, string anoEsperado)
        {
            //Arrange 
            var dtoParaValidar = new AbrangenciaTurmaRetornoEolDto() { Ano = ano, TipoTurma = tipoTurma, NomeTurma = turmaNome };

            //Act
            var anoParaValidar = dtoParaValidar.Ano;

            //Assert
            Assert.Equal(anoEsperado, anoParaValidar);
        }
    }
}
