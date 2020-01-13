using Moq;
using SME.SGP.Dominio.Interfaces;
using System;
using Xunit;

namespace SME.SGP.Dominio.Servicos.Teste
{
    public class ServicoDiaLetivoTeste
    {
        private readonly Mock<IRepositorioEvento> repositorioEvento;
        private readonly Mock<IRepositorioPeriodoEscolar> repositorioPeriodoEscolar;
        private readonly IServicoDiaLetivo servicoDiaLetivo;
        private readonly Mock<IUnitOfWork> unitOfWork;

        public ServicoDiaLetivoTeste()
        {
            repositorioPeriodoEscolar = new Mock<IRepositorioPeriodoEscolar>();
            repositorioEvento = new Mock<IRepositorioEvento>();
            unitOfWork = new Mock<IUnitOfWork>();
            servicoDiaLetivo = new ServicoDiaLetivo(repositorioEvento.Object, repositorioPeriodoEscolar.Object);
        }

        [Fact]
        public void Deve_Validar_Se_Eh_Dia_Letivo()
        {
            var data = new DateTime(2019, 02, 02);
            repositorioPeriodoEscolar.Setup(x => x.ObterPorTipoCalendarioData(1, data)).Returns(new PeriodoEscolar
            {
                Bimestre = 1,
                PeriodoInicio = new DateTime(2019, 02, 01),
                PeriodoFim = new DateTime(2019, 04, 30),
                TipoCalendarioId = 1,
                Id = 1
            });

            repositorioEvento.Setup(x => x.EhEventoLetivoPorTipoDeCalendarioDataDreUe(1, data, null, null)).Returns(true);
            var retorno = servicoDiaLetivo.ValidarSeEhDiaLetivo(data, 1, null, null);
            Assert.False(retorno);
        }
    }
}