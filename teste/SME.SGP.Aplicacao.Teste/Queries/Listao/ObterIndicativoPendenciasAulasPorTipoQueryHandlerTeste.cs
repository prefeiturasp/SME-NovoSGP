using MediatR;
using Moq;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Queries
{
    public class ObterIndicativoPendenciasAulasPorTipoQueryHandlerTeste
    {

        private readonly ObterIndicativoPendenciasAulasPorTipoQueryHandler query;
        private readonly Mock<IRepositorioPendenciaAulaConsulta> repositorioPendenciaAula;
        private readonly Mock<IMediator> mediator;

        public ObterIndicativoPendenciasAulasPorTipoQueryHandlerTeste()
        {
            repositorioPendenciaAula = new Mock<IRepositorioPendenciaAulaConsulta>();
            mediator = new Mock<IMediator>();
            query = new ObterIndicativoPendenciasAulasPorTipoQueryHandler(repositorioPendenciaAula.Object);
        }

        [Fact]
        public async Task Deve_Verificar_Se_Nao_Ha_Pendencia_Diario_Bordo()
        {
            //Arrange
            repositorioPendenciaAula.Setup(x => x.PossuiPendenciaDiarioBordo("512", "2386241", 1, false, true, ""))
                .ReturnsAsync(false);

            mediator.Setup(x => x.Send(It.IsAny<ObterIndicativoPendenciasAulasPorTipoQuery>(), It.IsAny<CancellationToken>()))
               .ReturnsAsync(new Infra.PendenciaPaginaInicialListao() { PendenciaDiarioBordo = true }) ;

            // Act
            var retornoConsulta = await query.Handle(new ObterIndicativoPendenciasAulasPorTipoQuery("512", "2386241", 1, true), new CancellationToken());

            // Assert
            Assert.NotNull(retornoConsulta);
            Assert.False(retornoConsulta.PendenciaDiarioBordo,"O usuário não possui nenhuma pendência do diário a ser resolvida!");
        }
    }
}
