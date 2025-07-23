using MediatR;
using Moq;
using SME.SGP.Dto;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.EscolaAqui.Comunicados.ObterComunicadosAnoAtual
{
    public class ObterComunicadosAnoAtualUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly ObterComunicadosAnoAtualUseCase useCase;

        public ObterComunicadosAnoAtualUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new ObterComunicadosAnoAtualUseCase(mediatorMock.Object);
        }

        [Fact]
        public async Task Deve_Executar_Mediator_Send_E_Retornar_Comunicados()
        {
            var comunicadosEsperados = new List<ComunicadoTurmaAlunoDto>
            {
                new ComunicadoTurmaAlunoDto
                {
                    Id = 1,
                    AnoLetivo = 2025,
                    CodigoDre = "DRE01",
                    CodigoUe = "UE01",
                    TurmaCodigo = "TURMA01",
                    AlunoCodigo = "ALUNO01",
                    Modalidade = 1,
                    SeriesResumidas = "1º Ano",
                    TipoComunicado = 2,
                    TipoEscola = "Municipal"
                }
            };

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterComunicadosAnoAtualQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(comunicadosEsperados);

            var resultado = await useCase.Executar();

            Assert.NotNull(resultado);
            Assert.Single(resultado);
            Assert.Equal("ALUNO01", ((List<ComunicadoTurmaAlunoDto>)resultado)[0].AlunoCodigo);

            mediatorMock.Verify(m => m.Send(ObterComunicadosAnoAtualQuery.Instance, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
