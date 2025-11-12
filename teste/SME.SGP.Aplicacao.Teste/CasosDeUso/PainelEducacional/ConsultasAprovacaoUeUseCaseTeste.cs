using MediatR;
using Moq;
using SME.SGP.Aplicacao.CasosDeUso.PainelEducacional;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterAprovacaoUe;
using SME.SGP.Dominio;
using SME.SGP.Infra.Dtos.PainelEducacional;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Teste.Aplicacao.CasosDeUso.PainelEducacional
{
    public class ConsultasAprovacaoUeUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly ConsultasAprovacaoUeUseCase useCase;

        public ConsultasAprovacaoUeUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new ConsultasAprovacaoUeUseCase(mediatorMock.Object);
        }

        [Fact(DisplayName = "Deve retornar dados de aprovação por UE com paginação")]
        public async Task ObterAprovacaoUe_DeveRetornarDadosCorretosComPaginacao()
        {
            // Arrange
            var filtro = new FiltroAprovacaoUeDto
            {
                AnoLetivo = 2025,
                ModalidadeId = (int)Modalidade.Fundamental,
                CodigoUe = "123456"
            };

            var registrosEsperados = new PainelEducacionalAprovacaoUeRetorno
            {
                Modalidades = new List<PainelEducacionalAprovacaoUeDto>
                {
                    new PainelEducacionalAprovacaoUeDto
                    {
                        Modalidade = "Fundamental",
                        Turmas = new List<PainelEducacionalAprovacaoUeTurmaDto>
                        {
                            new PainelEducacionalAprovacaoUeTurmaDto
                            {
                                Turma = "5A",
                                TotalPromocoes = 20,
                                TotalRetencoesAusencias = 2,
                                TotalRetencoesNotas = 1
                            }
                        }
                    }
                },
                            TotalPaginas = 1,
                            TotalRegistros = 1
                        };

                        mediatorMock
                            .Setup(m => m.Send(It.IsAny<PainelEducacionalAprovacaoUeQuery>(), It.IsAny<CancellationToken>()))
                            .ReturnsAsync(registrosEsperados);

            // Act
            var resultado = await useCase.ObterAprovacao(filtro);

            // Assert
            Assert.NotNull(resultado);
            Assert.Single(resultado.Modalidades);
            Assert.Equal("Fundamental", resultado.Modalidades.First().Modalidade);
            Assert.Equal("5A", resultado.Modalidades.First().Turmas.First().Turma);
        }
    }
}
