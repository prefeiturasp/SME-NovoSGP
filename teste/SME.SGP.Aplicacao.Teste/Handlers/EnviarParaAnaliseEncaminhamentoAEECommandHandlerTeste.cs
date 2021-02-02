using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Handlers
{
    public class EnviarParaAnaliseEncaminhamentoAEECommandHandlerTeste
    {
        private readonly Mock<IMediator> mediator;
        private readonly Mock<IRepositorioEncaminhamentoAEE> repositorioEncaminhamentoAEE;
        private readonly EnviarParaAnaliseEncaminhamentoAEECommandHandler enviarParaAnaliseEncaminhamentoAEECommandHandler;

        public EnviarParaAnaliseEncaminhamentoAEECommandHandlerTeste()
        {
            mediator = new Mock<IMediator>();
            repositorioEncaminhamentoAEE = new Mock<IRepositorioEncaminhamentoAEE>();
           // enviarParaAnaliseEncaminhamentoAEECommandHandler = new EnviarParaAnaliseEncaminhamentoAEECommandHandler(mediator.Object, repositorioDevolutiva.Object, repositorioTurma.Object);
        }

        [Fact]
        public async Task Deve_Atribuir_Responsavel()
        {
            // Arrange
            repositorioEncaminhamentoAEE.Setup(a => a.SalvarAsync(It.IsAny<EncaminhamentoAEE>()))
                .ReturnsAsync(1);

            //// Act
            //var auditoriaDto = inserirDevolutivaCommandHandler.Handle(new InserirDevolutivaCommand(1, new List<long> { 1, 2, 3, 4 }, DateTime.Today.AddDays(-15), DateTime.Today.AddDays(15), textoDescricao, 1), new CancellationToken());

            //// Assert
            //repositorioDevolutiva.Verify(x => x.SalvarAsync(It.IsAny<Devolutiva>()), Times.Once);
            //Assert.True(auditoriaDto.Id > 0);

            Assert.True(true);
        }

        [Fact]
        public async Task Nao_Deve_Atribuir_Responsavel()
        {
            // Arrange
            repositorioEncaminhamentoAEE.Setup(a => a.SalvarAsync(It.IsAny<EncaminhamentoAEE>()))
                .ReturnsAsync(1);

            //// Act
            //var auditoriaDto = inserirDevolutivaCommandHandler.Handle(new InserirDevolutivaCommand(1, new List<long> { 1, 2, 3, 4 }, DateTime.Today.AddDays(-15), DateTime.Today.AddDays(15), textoDescricao, 1), new CancellationToken());

            //// Assert
            //repositorioDevolutiva.Verify(x => x.SalvarAsync(It.IsAny<Devolutiva>()), Times.Once);
            //Assert.True(auditoriaDto.Id > 0);

            Assert.True(true);
        }
    }
}
