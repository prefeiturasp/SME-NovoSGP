using MediatR;
using Moq;
using SME.SGP.Aplicacao.CasosDeUso.PainelEducacional;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterIndicadoresAlfabetizacaoCritica;
using SME.SGP.Infra.Dtos.PainelEducacional;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.PainelEducacional
{
    public class ConsultasIndicadoresNivelAlfabetizacaoCriticaUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly ConsultasIndicadoresNivelAlfabetizacaoCriticaUseCase useCase;

        public ConsultasIndicadoresNivelAlfabetizacaoCriticaUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new ConsultasIndicadoresNivelAlfabetizacaoCriticaUseCase(mediatorMock.Object);
        }

        [Fact]
        public async Task Deve_Obter_Numero_Estudantes_Sem_Filtros()
        {
            var listaEsperada = new List<PainelEducacionalIndicadorAlfabetizacaoCriticaDto>
            {
                new PainelEducacionalIndicadorAlfabetizacaoCriticaDto
                {
                    Posicao = "1",
                    Ue = "UE1",
                    Dre = "DRE1",
                    TotalAlunosNaoAlfabetizados = 10,
                    PercentualTotalAlunos = 20.5m
                }
            };

            mediatorMock
                .Setup(m => m.Send(It.IsAny<PainelEducacionalIndicadoresNivelAlfabetizacaoCriticaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(listaEsperada);

            var resultado = await useCase.ObterNumeroEstudantes();

            Assert.NotNull(resultado);
            Assert.Single(resultado);
            Assert.Equal("UE1", ((List<PainelEducacionalIndicadorAlfabetizacaoCriticaDto>)resultado)[0].Ue);

            mediatorMock.Verify(m => m.Send(It.Is<PainelEducacionalIndicadoresNivelAlfabetizacaoCriticaQuery>(
                q => q.CodigoDre == null && q.CodigoUe == null), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Deve_Obter_Numero_Estudantes_Com_Filtros()
        {
            var listaEsperada = new List<PainelEducacionalIndicadorAlfabetizacaoCriticaDto>
            {
                new PainelEducacionalIndicadorAlfabetizacaoCriticaDto
                {
                    Posicao = "2",
                    Ue = "UE2",
                    Dre = "DRE2",
                    TotalAlunosNaoAlfabetizados = 5,
                    PercentualTotalAlunos = 10m
                }
            };

            mediatorMock
                .Setup(m => m.Send(It.IsAny<PainelEducacionalIndicadoresNivelAlfabetizacaoCriticaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(listaEsperada);

            var resultado = await useCase.ObterNumeroEstudantes("DRE2", "UE2");

            Assert.NotNull(resultado);
            Assert.Single(resultado);
            Assert.Equal("UE2", ((List<PainelEducacionalIndicadorAlfabetizacaoCriticaDto>)resultado)[0].Ue);

            mediatorMock.Verify(m => m.Send(It.Is<PainelEducacionalIndicadoresNivelAlfabetizacaoCriticaQuery>(
                q => q.CodigoDre == "DRE2" && q.CodigoUe == "UE2"), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}

