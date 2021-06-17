using MediatR;
using Moq;
using SME.SGP.Aplicacao.CasosDeUso;
using SME.SGP.Aplicacao.Queries;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
            //Arrange
            mediator.Setup(a => a.Send(It.IsAny<ObterSecoesPorEtapaDeEncaminhamentoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<SecaoQuestionarioDto>()
                {
                    new SecaoQuestionarioDto() { Id = 1, Nome = "Informações escolares", QuestionarioId = 1 },
                    new SecaoQuestionarioDto() { Id = 2, Nome = "Descrição do encaminhamento", QuestionarioId = 2 },                    
                });

            mediator.Setup(a => a.Send(It.IsAny<ObterSituacaoEncaminhamentoAEEPorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(SituacaoAEE.Rascunho);

            var encaminhamentoId = 0;
            //Act
            var dto = await obterSecoesPorEtapaDeEncaminhamentoAEEUseCase.Executar(encaminhamentoId);

            //Asert
            Assert.True(dto.Count() == 2);
        }

        [Fact]
        public async Task Obter_Secoes_Primeira_E_Segunda_Etapa()
        {
            //Arrange
            //Arrange
            mediator.Setup(a => a.Send(It.IsAny<ObterSecoesPorEtapaDeEncaminhamentoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<SecaoQuestionarioDto>()
                {
                    new SecaoQuestionarioDto() { Id = 1, Nome = "Informações escolares", QuestionarioId = 1 },
                    new SecaoQuestionarioDto() { Id = 2, Nome = "Descrição do encaminhamento", QuestionarioId = 2 },
                    new SecaoQuestionarioDto() { Id = 1, Nome = "Parecer Coordenação", QuestionarioId = 3 },
                });

            mediator.Setup(a => a.Send(It.IsAny<ObterSituacaoEncaminhamentoAEEPorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(SituacaoAEE.Encaminhado);

            var encaminhamentoId = 6;
            //Act
            var dto = await obterSecoesPorEtapaDeEncaminhamentoAEEUseCase.Executar(encaminhamentoId);

            //Asert
            Assert.True(dto.Count() == 3);
        }
    }
}
