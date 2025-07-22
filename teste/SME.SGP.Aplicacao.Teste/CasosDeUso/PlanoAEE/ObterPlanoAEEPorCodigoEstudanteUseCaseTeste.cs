using MediatR;
using Moq;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.PlanoAEE
{
    public class ObterPlanoAEEPorCodigoEstudanteUseCaseTeste
    {
        private readonly Mock<IMediator> mediator;
        private readonly Mock<IRepositorioPlanoAEE> repositorioPlanoAEE;
        private readonly ObterPlanoAEEPorCodigoEstudanteUseCase useCase;

        public ObterPlanoAEEPorCodigoEstudanteUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            repositorioPlanoAEE = new Mock<IRepositorioPlanoAEE>();
            useCase = new ObterPlanoAEEPorCodigoEstudanteUseCase(mediator.Object, repositorioPlanoAEE.Object);
        }

        [Fact]
        public async Task Executar_Com_CodigoEstudante_Valido_Deve_Retornar_Plano_Completo()
        {
            // Arrange
            var codigoEstudante = "123";
            var questionarioId = 1L;
            var versoes = new List<PlanoAEEVersaoDto>
            {
                new PlanoAEEVersaoDto { Id = 1, Numero = 1 },
                new PlanoAEEVersaoDto { Id = 2, Numero = 2 }
            };
            var respostas = new List<RespostaQuestaoDto>
            {
                new RespostaQuestaoDto { QuestaoId = 1},
                new RespostaQuestaoDto { QuestaoId = 2 }
            };
            var questoes = new List<QuestaoDto>
            {
                new QuestaoDto { Id = 1},
                new QuestaoDto { Id = 2 }
            };

            var planoAEE = new Dominio.PlanoAEE
            {
                Id = 1,
                CriadoEm = DateTime.Now,
                CriadoPor = "Sistema",
                AlteradoEm = DateTime.Now,
                AlteradoPor = "Sistema"
            };

            repositorioPlanoAEE.Setup(x => x.ObterPorIdAsync(It.IsAny<long>()))
                .ReturnsAsync(planoAEE);

            mediator.Setup(x => x.Send(ObterQuestionarioPlanoAEEIdQuery.Instance, It.IsAny<CancellationToken>()))
                .ReturnsAsync(questionarioId);

            mediator.Setup(x => x.Send(It.IsAny<ObterVersoesPlanoAEEPorCodigoEstudanteQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(versoes);

            mediator.Setup(x => x.Send(It.IsAny<ObterRespostasPlanoAEEPorVersaoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(respostas);

            mediator.Setup(x => x.Send(It.IsAny<ObterQuestoesPorQuestionarioPorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(questoes);

            // Act
            var resultado = await useCase.Executar(codigoEstudante);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(questionarioId, resultado.QuestionarioId);
            Assert.Equal(2, resultado.Versoes.Count());
            Assert.Equal(2, resultado.Questoes.Count());
            Assert.Equal(planoAEE.CriadoPor, resultado.Auditoria.CriadoPor);
            Assert.Equal(planoAEE.AlteradoPor, resultado.Auditoria.AlteradoPor);
        }

        [Fact]
        public async Task Executar_Deve_Usar_Ultima_Versao_Para_Obter_Respostas()
        {
            // Arrange
            var codigoEstudante = "123";
            var versoes = new List<PlanoAEEVersaoDto>
            {
                new PlanoAEEVersaoDto { Id = 1, Numero = 1 },
                new PlanoAEEVersaoDto { Id = 2, Numero = 2 },
                new PlanoAEEVersaoDto { Id = 3, Numero = 3 }
            };

            mediator.Setup(x => x.Send(It.IsAny<ObterVersoesPlanoAEEPorCodigoEstudanteQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(versoes);

            long versaoIdUsada = 0;
            mediator.Setup(x => x.Send(It.IsAny<ObterRespostasPlanoAEEPorVersaoQuery>(), It.IsAny<CancellationToken>()))
                .Callback<IRequest<IEnumerable<RespostaQuestaoDto>>, CancellationToken>((request, _) =>
                {
                    var query = (ObterRespostasPlanoAEEPorVersaoQuery)request;
                    versaoIdUsada = query.VersaoPlanoId;
                })
                .ReturnsAsync(new List<RespostaQuestaoDto>());

            // Act
            await useCase.Executar(codigoEstudante);

            // Assert
            Assert.Equal(3, versaoIdUsada); // Deve usar o ID da última versão (maior número)
        }
    }
}
