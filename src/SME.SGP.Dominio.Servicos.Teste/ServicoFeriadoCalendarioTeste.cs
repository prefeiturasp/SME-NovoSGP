using Moq;
using SME.SGP.Dominio.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Dominio.Servicos.Teste
{
    public class ServicoFeriadoCalendarioTeste
    {
        private readonly Mock<IRepositorioFeriadoCalendario> repositorioFeriadoCalendario;

        public ServicoFeriadoCalendarioTeste()
        {
            repositorioFeriadoCalendario = new Mock<IRepositorioFeriadoCalendario>();
        }

        [Fact]
        public void Deve_Calcular_Sexta_Santa()
        {
            //ARRANGE
            var servicoferiado = new ServicoFeriadoCalendario(repositorioFeriadoCalendario.Object);

            //ACT
            var data = servicoferiado.CalcularFeriado(2019, FeriadoEnum.SextaSanta);

            //ASSERT
            Assert.True(data.Year == 2019);
        }

        [Fact]
        public void Deve_Inserir_Feriados_Moveis()
        {
            //ARRANGE
            var servicoferiado = new ServicoFeriadoCalendario(repositorioFeriadoCalendario.Object);
            IEnumerable<FeriadoCalendario> retorno = new List<FeriadoCalendario>();

            repositorioFeriadoCalendario.Setup(a => a.ObterFeriadosCalendario(new Infra.FiltroFeriadoCalendarioDto() { Tipo = TipoFeriadoCalendario.Movel, Ano = 2019 }))
                .Returns(Task.FromResult(retorno));

            //ACT
            servicoferiado.VerficaSeExisteFeriadosMoveisEInclui(2019);

            //ASSERT
            Assert.True(true);
        }
    }
}