using MediatR;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso
{
    public class ObterAnotacaoFrequenciaAlunoUseCaseTeste
    {
        private readonly ObterAnotacaoFrequenciaAlunoUseCase obterAnotacaoFrequenciaAlunoUseCase;
        private readonly Mock<IMediator> mediator;

        public ObterAnotacaoFrequenciaAlunoUseCaseTeste()
        {

            mediator = new Mock<IMediator>();
            obterAnotacaoFrequenciaAlunoUseCase = new ObterAnotacaoFrequenciaAlunoUseCase(mediator.Object);
        }

        [Fact]
        public async Task Deve_Obter_Anotacao_Frequencia_Aluno()
        {
            mediator.Setup(a => a.Send(It.IsAny<ObterAnotacaoFrequenciaAlunoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Dominio.AnotacaoFrequenciaAluno
                {
                    Id = 1,
                    Anotacao = "Teste",
                    CriadoEm = DateTime.Today,
                    CriadoPor = "SISTEMA",
                    AlteradoEm = DateTime.Today,
                    AlteradoPor = "SISTEMA"
                });

            var anotacao = await obterAnotacaoFrequenciaAlunoUseCase.Executar(new Infra.FiltroAnotacaoFrequenciaAlunoDto("123123", 100));

            Assert.NotNull(anotacao);
            Assert.NotNull(anotacao.Auditoria);
            Assert.True(anotacao.Id == 1);
        }

        [Fact]
        public async Task Deve_Retornar_Null_Quando_Anotacao_Nao_Encontrada()
        {
            mediator.Setup(a => a.Send(It.IsAny<ObterAnotacaoFrequenciaAlunoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Dominio.AnotacaoFrequenciaAluno)null);

            var anotacao = await obterAnotacaoFrequenciaAlunoUseCase.Executar(new Infra.FiltroAnotacaoFrequenciaAlunoDto("123123", 100));

            Assert.Null(anotacao);
        }

        [Fact]
        public async Task Deve_Mapear_Corretamente_Quando_MotivoAusenciaId_For_Null()
        {
            mediator.Setup(a => a.Send(It.IsAny<ObterAnotacaoFrequenciaAlunoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Dominio.AnotacaoFrequenciaAluno
                {
                    Id = 1,
                    Anotacao = "Teste sem motivo",
                    CodigoAluno = "123456",
                    AulaId = 100,
                    MotivoAusenciaId = null,
                    CriadoEm = DateTime.Today,
                    CriadoPor = "SISTEMA"
                });

            var anotacao = await obterAnotacaoFrequenciaAlunoUseCase.Executar(new Infra.FiltroAnotacaoFrequenciaAlunoDto("123456", 100));

            Assert.NotNull(anotacao);
            Assert.Equal(0, anotacao.MotivoAusenciaId);
            Assert.Equal("Teste sem motivo", anotacao.Anotacao);
            Assert.Equal("123456", anotacao.CodigoAluno);
            Assert.Equal(100, anotacao.AulaId);
        }

        [Fact]
        public async Task Deve_Mapear_Corretamente_Quando_MotivoAusenciaId_For_Preenchido()
        {
            var motivoAusenciaId = 5L;
            mediator.Setup(a => a.Send(It.IsAny<ObterAnotacaoFrequenciaAlunoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Dominio.AnotacaoFrequenciaAluno
                {
                    Id = 2,
                    Anotacao = "Teste com motivo",
                    CodigoAluno = "789012",
                    AulaId = 200,
                    MotivoAusenciaId = motivoAusenciaId,
                    CriadoEm = DateTime.Today,
                    CriadoPor = "USUARIO_TESTE"
                });

            var anotacao = await obterAnotacaoFrequenciaAlunoUseCase.Executar(new Infra.FiltroAnotacaoFrequenciaAlunoDto("789012", 200));

            Assert.NotNull(anotacao);
            Assert.Equal(motivoAusenciaId, anotacao.MotivoAusenciaId);
            Assert.Equal("Teste com motivo", anotacao.Anotacao);
            Assert.Equal("789012", anotacao.CodigoAluno);
            Assert.Equal(200, anotacao.AulaId);
        }

        [Fact]
        public async Task Deve_Passar_Parametros_Corretos_Para_Query()
        {
            var codigoAluno = "987654321";
            var aulaId = 150L;
            var filtro = new Infra.FiltroAnotacaoFrequenciaAlunoDto(codigoAluno, aulaId);

            mediator.Setup(a => a.Send(It.IsAny<ObterAnotacaoFrequenciaAlunoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Dominio.AnotacaoFrequenciaAluno
                {
                    Id = 3,
                    Anotacao = "Teste parametros",
                    CodigoAluno = codigoAluno,
                    AulaId = aulaId,
                    CriadoEm = DateTime.Today,
                    CriadoPor = "SISTEMA"
                });

            await obterAnotacaoFrequenciaAlunoUseCase.Executar(filtro);

            mediator.Verify(m => m.Send(
                It.Is<ObterAnotacaoFrequenciaAlunoQuery>(q => 
                    q.CodigoAluno == codigoAluno && 
                    q.AulaId == aulaId), 
                It.IsAny<CancellationToken>()), 
                Times.Once);
        }

        [Fact]
        public void Deve_Criar_UseCase_Com_Mediator_Valido()
        {
            var useCase = new ObterAnotacaoFrequenciaAlunoUseCase(mediator.Object);

            Assert.NotNull(useCase);
        }
    }
}
