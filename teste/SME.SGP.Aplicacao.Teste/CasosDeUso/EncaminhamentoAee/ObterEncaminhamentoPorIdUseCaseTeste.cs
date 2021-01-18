using MediatR;
using Moq;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso
{
    public class ObterEncaminhamentoPorIdUseCaseTeste
    {
        private readonly Mock<IMediator> mediator;
        private readonly ObterEncaminhamentoPorIdUseCase useCase;

        public ObterEncaminhamentoPorIdUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            useCase = new ObterEncaminhamentoPorIdUseCase(mediator.Object);
        }

        
        public async Task Deve_Listar_Encaminhamento()
        {
            //Arrange
            mediator.Setup(a => a.Send(It.IsAny<ObterAlunoPorCodigoEAnoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new AlunoReduzidoDto() { CodigoAluno = "" });

            var encaminhamentoId = 6;

            //Act
            var retorno = await useCase.Executar(encaminhamentoId);

            //Asert
            Assert.True(retorno != null);
        }

        public async Task Nao_Deve_Listar_Encaminhamento()
        {
            
            var encaminhamentoId = -99;

            //Act
            var retorno = await useCase.Executar(encaminhamentoId);

            //Asert
            Assert.Null(retorno);            
        }
    }
}
