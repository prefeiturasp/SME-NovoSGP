using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.ConselhoClasse
{
    public class SalvarConselhoClasseAlunoNotaUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly SalvarConselhoClasseAlunoNotaUseCase useCase;

        public SalvarConselhoClasseAlunoNotaUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new SalvarConselhoClasseAlunoNotaUseCase(mediatorMock.Object);
        }

        [Fact]
        public async Task Deve_Executar_Com_Sucesso()
        {
            var dto = CriarDto();
            ConfigurarMocksParaSucesso();

            var resultado = await useCase.Executar(dto);

            Assert.NotNull(resultado);
            mediatorMock.Verify(m => m.Send(It.IsAny<IRequest<object>>(), default), Times.AtLeastOnce);
        }

        [Fact]
        public async Task Deve_Lancar_Excecao_Quando_Turma_Nao_Encontrada()
        {
            var dto = CriarDto();
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaPorCodigoQuery>(), default))
                        .ReturnsAsync((Turma)null);

            await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(dto));
        }

        [Fact]
        public async Task Deve_Lancar_Excecao_Quando_FechamentoTurma_Nao_Existe()
        {
            var dto = CriarDto();
            ConfigurarMocksParaSucesso();
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterFechamentoTurmaPorIdAlunoCodigoQuery>(), default))
                        .ReturnsAsync((FechamentoTurma)null);

            await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(dto));
        }

        [Fact]
        public async Task Deve_Lancar_Excecao_Quando_Aluno_Nao_Encontrado()
        {
            var dto = CriarDto();
            ConfigurarMocksParaSucesso();
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterTodosAlunosNaTurmaQuery>(), default))
                        .ReturnsAsync(new List<AlunoPorTurmaResposta>());

            await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(dto));
        }

        private SalvarConselhoClasseAlunoNotaDto CriarDto()
        {
            return new SalvarConselhoClasseAlunoNotaDto
            {
                CodigoAluno = "123",
                CodigoTurma = "456",
                ConselhoClasseId = 1,
                FechamentoTurmaId = 1,
                Bimestre = 2,
                ConselhoClasseNotaDto = new ConselhoClasseNotaDto
                {
                    CodigoComponenteCurricular = 789,
                    Nota = 9
                }
            };
        }

        private void ConfigurarMocksParaSucesso()
        {
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaPorCodigoQuery>(), default))
                        .ReturnsAsync(new Turma { AnoLetivo = DateTime.Now.Year });

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterFechamentoTurmaPorIdAlunoCodigoQuery>(), default))
                        .ReturnsAsync(new FechamentoTurma());

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterTodosAlunosNaTurmaQuery>(), default))
                        .ReturnsAsync(new List<AlunoPorTurmaResposta>
                        {
                            new AlunoPorTurmaResposta { DataMatricula = DateTime.Now.AddMonths(-1), DataSituacao = DateTime.Now }
                        });

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterPeriodoEscolarPorTurmaBimestreQuery>(), default))
                        .ReturnsAsync(new PeriodoEscolar { PeriodoInicio = DateTime.Now.AddMonths(-2), PeriodoFim = DateTime.Now.AddMonths(2) });

            mediatorMock.Setup(m => m.Send(It.IsAny<GravarFechamentoTurmaConselhoClasseCommand>(), default))
                        .ReturnsAsync(true);

            mediatorMock.Setup(m => m.Send(It.IsAny<GravarConselhoClasseCommad>(), default))
                        .ReturnsAsync(new ConselhoClasseNotaRetornoDto());

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterUsuarioLogadoQuery>(), default))
              .ReturnsAsync(new Usuario
              {
                  Nome = "Usuário Teste",
                  Login = "usuario.teste"
              });

            mediatorMock.Setup(m => m.Send(It.IsAny<PossuiAtribuicaoCJPorDreUeETurmaQuery>(), default))
                .ReturnsAsync(true);

            mediatorMock.Setup(m => m.Send(It.IsAny<ProfessorPodePersistirTurmaQuery>(), default))
                        .ReturnsAsync(true);

            mediatorMock.Setup(m => m.Send(It.IsAny<VerificaSePodeEditarNotaQuery>(), default))
              .ReturnsAsync(true);
        }
    }
}
