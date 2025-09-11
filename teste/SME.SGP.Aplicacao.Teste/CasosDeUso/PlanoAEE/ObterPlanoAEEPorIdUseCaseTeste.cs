using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
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

            // Plano do domínio
            var planoDominio = new Dominio.PlanoAEE
            {
                Id = 1,
                AlunoCodigo = "123",
                Turma = new Turma
                {
                    Id = 1,
                    CodigoTurma = "1",
                    AnoLetivo = DateTime.Now.Year,
                    ModalidadeCodigo = Modalidade.Fundamental,
                    UeId = 10,
                    TipoTurma = Dominio.Enumerados.TipoTurma.Regular,
                    Historica = false,
                    Ue = new Ue { CodigoUe = "UE01" }
                },
                Situacao = SituacaoPlanoAEE.ParecerCP
            };

            // Aluno reduzido
            var alunoTurma = new AlunoReduzidoDto
            {
                CodigoAluno = "123",
                Nome = "Aluno Teste"
            };

            // Matrícula ativa do aluno
            var matriculaAtiva = new AlunoPorTurmaResposta
            {
                CodigoAluno = "123",
                NomeAluno = "Aluno Teste",
                CodigoTurma = 1,
                Ano = DateTime.Now.Year,
                TurmaEscola = "1",
                DataSituacao = DateTime.Now,
                SituacaoMatricula = "Ativo"
            };

            // Versões
            var versoes = new List<PlanoAEEVersaoDto>
            {
                new PlanoAEEVersaoDto { Id = 100, Numero = 1 }
            };

                    // Questões
                    var questoes = new List<QuestaoDto>
            {
                new QuestaoDto { Id = 1, TipoQuestao = TipoQuestao.Texto }
            };

            // Período escolar
            var periodoAtual = new PeriodoEscolar { Id = 123 };

            // Usuário logado
            var usuarioLogado = new Usuario
            {
                Login = "teste.usuario",
                PerfilAtual = Guid.NewGuid(),
            };

            // Mock de mediator
            mediator.Setup(x => x.Send(It.IsAny<ObterPlanoAEEComTurmaPorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(planoDominio);

            mediator.Setup(x => x.Send(It.IsAny<ObterAlunoPorCodigoEAnoPlanoAeeQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(alunoTurma);

            mediator.Setup(x => x.Send(It.IsAny<ObterAlunoPorCodigoEolQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((AlunoPorTurmaResposta)null); // força o fluxo para buscar matrícula

            mediator.Setup(x => x.Send(It.IsAny<ObterMatriculasAlunoNaTurmaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<AlunoPorTurmaResposta> { matriculaAtiva });

            mediator.Setup(x => x.Send(It.IsAny<ObterVersoesPlanoAEEQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(versoes);

            mediator.Setup(x => x.Send(It.IsAny<ObterQuestionarioPlanoAEEIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(999);

            mediator.Setup(x => x.Send(It.IsAny<ObterQuestoesPlanoAEEPorVersaoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(questoes);

            mediator.Setup(x => x.Send(It.IsAny<ObterTurmaPorCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(planoDominio.Turma);

            mediator.Setup(x => x.Send(It.IsAny<ObterUeComDrePorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(planoDominio.Turma.Ue);

            mediator.Setup(x => x.Send(It.IsAny<ObterUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(usuarioLogado);

            consultasPeriodoEscolar
                .Setup(x => x.ObterPeriodoAtualPorModalidade((Modalidade)It.IsAny<int>()))
                .ReturnsAsync(periodoAtual);

            // Act
            var resultado = await useCase.Executar(filtro);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(planoDominio.Id, resultado.Id);
            Assert.NotNull(resultado.Aluno);
            Assert.NotNull(resultado.Turma);
            Assert.NotNull(resultado.Questoes);
            Assert.NotEmpty(resultado.Questoes);
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

            // Plano do domínio
            var planoDominio = new Dominio.PlanoAEE
            {
                Id = 1,
                AlunoCodigo = "1",
                Turma = new Turma
                {
                    Id = 1,
                    CodigoTurma = "1",
                    AnoLetivo = DateTime.Now.Year,
                    TipoTurma = Dominio.Enumerados.TipoTurma.Programa,
                    UeId = 10,
                    ModalidadeCodigo = Modalidade.Fundamental
                },
                Situacao = SituacaoPlanoAEE.ParecerCP
            };

            // Aluno reduzido
            var alunoTurma = new AlunoReduzidoDto
            {
                CodigoAluno = "1",
                Nome = "Joao"
            };

            // Aluno por turma
            var alunoPorTurmaResposta = new AlunoPorTurmaResposta
            {
                CodigoAluno = "1",
                NomeAluno = "Joao",
                Ano = DateTime.Now.Year,
                CodigoTurma = 1,
                TurmaEscola = "1",
                DataNascimento = DateTime.Now.AddYears(-10),
                SituacaoMatricula = "Ativo",
                DataSituacao = DateTime.Now
            };

            // Turma
            var turma = new Turma
            {
                Id = 1,
                CodigoTurma = "1",
                AnoLetivo = DateTime.Now.Year,
                ModalidadeCodigo = Modalidade.Fundamental,
                UeId = 10,
                TipoTurma = Dominio.Enumerados.TipoTurma.Regular,
                Ue = new Ue
                {
                    CodigoUe = "UE01"
                }
            };

            // UE
            var ue = new Ue
            {
                CodigoUe = "UE01"
            };

            // Versões
            var versoes = new List<PlanoAEEVersaoDto>
    {
        new PlanoAEEVersaoDto { Id = 100, Numero = 1 }
    };

            // Questões
            var questoes = new List<QuestaoDto>
    {
        new QuestaoDto { Id = 1, TipoQuestao = TipoQuestao.Texto }
    };

            // Período escolar
            var periodoAtual = new PeriodoEscolar
            {
                Id = 123
            };

            // Usuário logado
            var usuarioLogado = new Usuario
            {
                Nome = "Usuário Teste",
                CodigoRf = "123456"
            };

            // Configurações de mocks
            mediator.Setup(x => x.Send(It.IsAny<ObterPlanoAEEComTurmaPorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(planoDominio);

            mediator.Setup(x => x.Send(It.IsAny<ObterAlunoPorCodigoEAnoPlanoAeeQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(alunoTurma);

            mediator.Setup(x => x.Send(It.IsAny<ObterAlunoPorCodigoEolQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(alunoPorTurmaResposta);

            mediator.Setup(x => x.Send(It.IsAny<ObterTurmaPorCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(turma);

            mediator.Setup(x => x.Send(It.IsAny<ObterUeComDrePorIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(ue);

            mediator.Setup(x => x.Send(It.IsAny<ObterVersoesPlanoAEEQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(versoes);

            mediator.Setup(x => x.Send(It.IsAny<ObterQuestionarioPlanoAEEIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(999);

            mediator.Setup(x => x.Send(It.IsAny<ObterQuestoesPlanoAEEPorVersaoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(questoes);

            consultasPeriodoEscolar
                .Setup(x => x.ObterPeriodoAtualPorModalidade((Modalidade)It.IsAny<int>()))
                .ReturnsAsync(periodoAtual);

            mediator.Setup(x => x.Send(It.IsAny<ObterUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(usuarioLogado);

            // Act
            var resultado = await useCase.Executar(filtro);

            // Assert
            Assert.NotNull(resultado.Questoes);
            Assert.NotEmpty(resultado.Questoes);
        }
    }
}
