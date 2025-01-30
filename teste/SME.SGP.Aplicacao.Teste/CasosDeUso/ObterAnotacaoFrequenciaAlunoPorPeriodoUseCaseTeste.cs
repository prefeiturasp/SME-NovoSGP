using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Infra.Dtos.AnotacaoFrequenciaAluno;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using System.Linq;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso
{
    public class ObterAnotacaoFrequenciaAlunoPorPeriodoUseCaseTeste
    {
        private readonly ObterAnotacaoFrequenciaAlunoPorPeriodoUseCase obterAnotacaoFrequenciaAlunoPorPeriodoUseCase;
        private readonly Mock<IMediator> mediator;

        public ObterAnotacaoFrequenciaAlunoPorPeriodoUseCaseTeste()
        {

            mediator = new Mock<IMediator>();
            obterAnotacaoFrequenciaAlunoPorPeriodoUseCase = new ObterAnotacaoFrequenciaAlunoPorPeriodoUseCase(mediator.Object);
        }

        [Fact]
        public async Task Deve_Obter_Anotacao_Frequencia_Aluno_Por_Periodo()
        {
           
            var anotacoes = new List<AnotacaoAlunoAulaPorPeriodoDto>
            {
                new AnotacaoAlunoAulaPorPeriodoDto
                {
                    AulaId = 101,
                    Anotacao = "Presença confirmada",
                    CodigoAluno = "12345",
                    MotivoAusenciaId = 5
                }
            };

            mediator.Setup(a => a.Send(It.IsAny<ObterAnotacaoFrequenciaAlunoPorPeriodoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(anotacoes);

            var resultado = await obterAnotacaoFrequenciaAlunoPorPeriodoUseCase.Executar(new Infra.FiltroAnotacaoFrequenciaAlunoPorPeriodoDto(
            "12345",
            new DateTime(2025, 1, 1),
            new DateTime(2025, 12, 31)));
            
            Assert.NotNull(resultado);
            Assert.NotEmpty(resultado);
            Assert.Equal(101, resultado.First().AulaId);
        }

        [Fact]
        public async Task Nao_Deve_Obter_Anotacao_Se_Data_Final_Menor_Que_Data_Inicial()
        {

            var filtroInvalido = new Infra.FiltroAnotacaoFrequenciaAlunoPorPeriodoDto(
                "12345",
                new DateTime(2025, 12, 31),
                new DateTime(2025, 1, 1)
            );

            mediator.Setup(a => a.Send(It.IsAny<ObterAnotacaoFrequenciaAlunoPorPeriodoQuery>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new ArgumentException("A data final não pode ser menor que a data inicial."));

            var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
                obterAnotacaoFrequenciaAlunoPorPeriodoUseCase.Executar(filtroInvalido));

            Assert.Equal("A data final não pode ser menor que a data inicial.", exception.Message);
        }


    }
}
