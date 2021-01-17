using MediatR;
using Moq;
using SME.SGP.Aplicacao.CasosDeUso;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso
{
    public class ObterSecoesPorEtapaDeEncaminhamentoAEEUseCaseTeste
    {
        private readonly ObterSecoesPorEtapaDeEncaminhamentoAEEUseCase obterSecoesPorEtapaDeEncaminhamentoAEEUseCase;
        private readonly Mock<IMediator> mediator;

        public ObterSecoesPorEtapaDeEncaminhamentoAEEUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            obterSecoesPorEtapaDeEncaminhamentoAEEUseCase = new ObterSecoesPorEtapaDeEncaminhamentoAEEUseCase(mediator.Object);
        }

        [Fact]
        public async Task Obter_Secoes_Primeira_Etapa()
        {
            //Act
            var dto = await obterSecoesPorEtapaDeEncaminhamentoAEEUseCase.Executar(1);

            //Asert
            Assert.NotNull(dto);
        }        
    }
}
