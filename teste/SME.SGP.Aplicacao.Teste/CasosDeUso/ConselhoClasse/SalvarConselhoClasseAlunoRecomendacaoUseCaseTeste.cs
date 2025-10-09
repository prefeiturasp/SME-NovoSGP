using MediatR;
using Moq;
using SME.SGP.Aplicacao.Queries;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.ConselhoClasse
{
    public class SalvarConselhoClasseAlunoRecomendacaoUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly SalvarConselhoClasseAlunoRecomendacaoUseCase useCase;

        public SalvarConselhoClasseAlunoRecomendacaoUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new SalvarConselhoClasseAlunoRecomendacaoUseCase(mediatorMock.Object);
        }

        [Fact]
        public async Task Deve_Lancar_Excecao_Quando_FechamentoTurma_Nao_Encontrado()
        {
            var dto = new ConselhoClasseAlunoAnotacoesDto { FechamentoTurmaId = 1 };
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterFechamentoTurmaCompletoPorIdQuery>(), default))
                        .ReturnsAsync((FechamentoTurma)null);

            var exception = await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(dto));
            Assert.Equal(MensagemNegocioFechamentoNota.FECHAMENTO_TURMA_NAO_LOCALIZADO, exception.Message);
        }

        [Fact]
        public async Task Deve_Lancar_Excecao_Quando_Periodo_Nao_Esta_Aberto()
        {
            var fechamentoTurma = new FechamentoTurma { Turma = new Turma(), PeriodoEscolar = new PeriodoEscolar() };
            var dto = new ConselhoClasseAlunoAnotacoesDto { FechamentoTurmaId = 1 };

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterFechamentoTurmaCompletoPorIdQuery>(), default))
                        .ReturnsAsync(fechamentoTurma);
            mediatorMock.Setup(m => m.Send(It.IsAny<TurmaEmPeriodoAbertoQuery>(), default))
                        .ReturnsAsync(false);

            var exception = await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(dto));
            Assert.Equal(MensagemNegocioComuns.APENAS_EH_POSSIVEL_CONSULTAR_ESTE_REGISTRO_POIS_O_PERIODO_NAO_ESTA_EM_ABERTO, exception.Message);
        }

        [Fact]
        public async Task Deve_Lancar_Excecao_Quando_Aluno_Nao_Encontrado()
        {
            var fechamentoTurma = new FechamentoTurma { Turma = new Turma(), PeriodoEscolar = new PeriodoEscolar() };
            var dto = new ConselhoClasseAlunoAnotacoesDto { FechamentoTurmaId = 1, AlunoCodigo = "123" };

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterFechamentoTurmaCompletoPorIdQuery>(), default))
                        .ReturnsAsync(fechamentoTurma);
            mediatorMock.Setup(m => m.Send(It.IsAny<TurmaEmPeriodoAbertoQuery>(), default))
                        .ReturnsAsync(true);
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterAlunosPorTurmaEAnoLetivoQuery>(), default))
                        .ReturnsAsync(new List<AlunoPorTurmaResposta>());

            var exception = await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(dto));
            Assert.Equal(MensagemNegocioConselhoClasse.ALUNO_NAO_ENCONTRADO_PARA_SALVAR_CONSELHO_CLASSE, exception.Message);
        }

        [Fact]
        public async Task Deve_Salvar_ConselhoClasseAluno_Com_Sucesso()
        {
            var fechamentoTurma = new FechamentoTurma { Turma = new Turma(), PeriodoEscolar = new PeriodoEscolar() };
            var aluno = new AlunoPorTurmaResposta { CodigoAluno = "123" };
            var dto = new ConselhoClasseAlunoAnotacoesDto
            {
                FechamentoTurmaId = 1,
                AlunoCodigo = "123",
                RecomendacaoAlunoIds = new List<long> { 1 },
                RecomendacaoFamiliaIds = new List<long> { 2 }
            };

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterFechamentoTurmaCompletoPorIdQuery>(), default))
                        .ReturnsAsync(fechamentoTurma);
            mediatorMock.Setup(m => m.Send(It.IsAny<TurmaEmPeriodoAbertoQuery>(), default))
                        .ReturnsAsync(true);
            mediatorMock.Setup(m => m.Send(It.IsAny<ObterAlunosPorTurmaEAnoLetivoQuery>(), default))
                        .ReturnsAsync(new List<AlunoPorTurmaResposta> { aluno });
            mediatorMock.Setup(m => m.Send(It.IsAny<VerificaNotasTodosComponentesCurricularesQuery>(), default))
                        .ReturnsAsync(true); 
            mediatorMock.Setup(m => m.Send(It.IsAny<SalvarConselhoClasseAlunoCommand>(), default))
                        .ReturnsAsync(1L);

            var result = await useCase.Executar(dto);

            Assert.NotNull(result);
            Assert.Equal("123", result.AlunoCodigo);
            mediatorMock.Verify(m => m.Send(It.IsAny<SalvarConselhoClasseAlunoRecomendacaoCommand>(), default), Times.Once);
        }
    }
}