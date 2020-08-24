using MediatR;
using Moq;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;
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
            // arrange
            mediator.Setup(a => a.Send(It.IsAny<ObterAnotacaoFrequenciaAlunoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new AnotacaoFrequenciaAluno
                {
                    Id = 1,
                    Anotacao = "Teste",
                    CriadoEm = DateTime.Today,
                    CriadoPor = "SISTEMA",
                    AlteradoEm = DateTime.Today,
                    AlteradoPor = "SISTEMA"
                });

            // act
            var anotacao = await obterAnotacaoFrequenciaAlunoUseCase.Executar(new Infra.FiltroAnotacaoFrequenciaAlunoDto("123123", 100));

            // assert
            Assert.NotNull(anotacao);
            Assert.NotNull(anotacao.Auditoria);
            Assert.True(anotacao.Id == 1);
        }
    }
}
