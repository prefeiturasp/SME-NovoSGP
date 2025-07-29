using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.PlanoAEE
{
    public class ObterPlanoAEEPorIdUseCaseTeste
    {

        private readonly Mock<IMediator> mediator;
        private readonly Mock<IConsultasPeriodoEscolar> consultasPeriodoEscolar;
        private readonly ObterPlanoAEEPorIdUseCase useCase;

        public ObterPlanoAEEPorIdUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            consultasPeriodoEscolar = new Mock<IConsultasPeriodoEscolar>();
            useCase = new ObterPlanoAEEPorIdUseCase(mediator.Object, consultasPeriodoEscolar.Object);
        }

        [Fact]
        public async Task Deve_Lancar_Excecao_Quando_PlanoId_Invalido()
        {
            // Arrange
            var filtro = new FiltroPesquisaQuestoesPorPlanoAEEIdDto(1, "123", 1);

            // Act & Assert
            await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(filtro));
        }

        [Fact]
        public async Task Deve_Lancar_Excecao_Quando_Plano_Nao_Encontrado()
        {
            // Arrange
            var filtro = new FiltroPesquisaQuestoesPorPlanoAEEIdDto(1, "123", 1);

            mediator.Setup(x => x.Send(It.IsAny<ObterPlanoAEEComTurmaPorIdQuery>(), It.IsAny<CancellationToken>()))
                   .ReturnsAsync((Dominio.PlanoAEE)null);

            // Act & Assert
            await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(filtro));
        }

        [Fact]
        public async Task Deve_Retornar_Plano_Quando_Encontrado()
        {
            // Arrange
            var filtro = new FiltroPesquisaQuestoesPorPlanoAEEIdDto(1, "123", 1);
            var plano = new Dominio.PlanoAEE
            {
                Id = 1,
                AlunoCodigo = "123",
                Turma = new Turma { Id = 1, CodigoTurma = "1", AnoLetivo = DateTime.Now.Year }
            };

            var alunoTurma = new AlunoReduzidoDto
            {
                CodigoAluno = "123",
                Nome = "Aluno Teste"
            };

            mediator.Setup(x => x.Send(It.IsAny<ObterPlanoAEEComTurmaPorIdQuery>(), It.IsAny<CancellationToken>()))
                   .ReturnsAsync(plano);

            mediator.Setup(x => x.Send(It.IsAny<ObterAlunoPorCodigoEAnoPlanoAeeQuery>(), It.IsAny<CancellationToken>()))
                   .ReturnsAsync(alunoTurma);

            mediator.Setup(x => x.Send(It.IsAny<ObterVersoesPlanoAEEQuery>(), It.IsAny<CancellationToken>()))
                   .ReturnsAsync(new List<PlanoAEEVersaoDto>());

            // Act
            var resultado = await useCase.Executar(filtro);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(plano.Id, resultado.Id);
        }

        [Fact]
        public async Task Deve_Lancar_Excecao_Quando_Aluno_Nao_Encontrado()
        {
            // Arrange
            var filtro = new FiltroPesquisaQuestoesPorPlanoAEEIdDto(1, "123", 1);
            var plano = new Dominio.PlanoAEE
            {
                Id = 1,
                AlunoCodigo = "123",
                Turma = new Turma { Id = 1, CodigoTurma = "1", AnoLetivo = DateTime.Now.Year }
            };

            mediator.Setup(x => x.Send(It.IsAny<ObterPlanoAEEComTurmaPorIdQuery>(), It.IsAny<CancellationToken>()))
                   .ReturnsAsync(plano);

            mediator.Setup(x => x.Send(It.IsAny<ObterAlunoPorCodigoEAnoPlanoAeeQuery>(), It.IsAny<CancellationToken>()))
                   .ReturnsAsync((AlunoReduzidoDto)null);

            mediator.Setup(x => x.Send(It.IsAny<ObterAlunoPorCodigoEAnoQuery>(), It.IsAny<CancellationToken>()))
                   .ReturnsAsync((AlunoReduzidoDto)null);

            // Act & Assert
            await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(filtro));
        }

        [Fact]
        public async Task Deve_Retornar_Plano_Com_Questoes_Quando_Existirem()
        {
            // Arrange
            var filtro = new FiltroPesquisaQuestoesPorPlanoAEEIdDto(1, "1", 1);
            var plano = new Dominio.PlanoAEE
            {
                Id = 1,
                AlunoCodigo = "1",
                Turma = new Turma { Id = 1, CodigoTurma = "1", AnoLetivo = DateTime.Now.Year }
            };

            var alunoTurma = new AlunoReduzidoDto
            {
                CodigoAluno = "1",
                Nome = "Joao"
            };

            var questoes = new List<QuestaoDto>
            {
                new QuestaoDto { Id = 1, TipoQuestao = TipoQuestao.Texto }
            };

            var alunoPorTurmaResposta = new AlunoPorTurmaResposta() 
            { CodigoAluno = "1", NomeAluno = "Joao", Ano = DateTime.Now.Year, CodigoTurma = 1, TurmaEscola = "1" };

            mediator.Setup(x => x.Send(It.IsAny<ObterPlanoAEEComTurmaPorIdQuery>(), It.IsAny<CancellationToken>()))
                   .ReturnsAsync(plano);

            mediator.Setup(x => x.Send(It.IsAny<ObterAlunoPorCodigoEAnoPlanoAeeQuery>(), It.IsAny<CancellationToken>()))
                   .ReturnsAsync(alunoTurma);

            mediator.Setup(x => x.Send(It.IsAny<ObterVersoesPlanoAEEQuery>(), It.IsAny<CancellationToken>()))
                   .ReturnsAsync(new List<PlanoAEEVersaoDto>());

            mediator.Setup(x => x.Send(It.IsAny<ObterQuestoesPlanoAEEPorVersaoQuery>(), It.IsAny<CancellationToken>()))
                   .ReturnsAsync(questoes);

            mediator.Setup(x => x.Send(It.IsAny<ObterAlunoPorCodigoEolQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(alunoPorTurmaResposta);

            // Act
            var resultado = await useCase.Executar(filtro);

            // Assert
            Assert.NotNull(resultado.Questoes);
            Assert.NotEmpty(resultado.Questoes);
        }
    }
}
