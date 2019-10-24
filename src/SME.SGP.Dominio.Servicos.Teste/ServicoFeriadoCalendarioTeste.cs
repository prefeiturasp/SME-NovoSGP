using Xunit;

namespace SME.SGP.Dominio.Servicos.Teste
{
    public class ServicoFeriadoCalendarioTeste
    {
        [Fact]
        public void Deve_Calcular_Feriados()
        {
            var servicoFeriado = new ServicoFeriadoCalendario();
            var data = servicoFeriado.CalcularFeriado(2019, FeriadoEnum.SextaSanta);

            Assert.True(data.Year == 2019);
        }
    }
}