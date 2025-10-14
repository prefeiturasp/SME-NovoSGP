using MediatR;
using Moq;
using SME.SGP.Aplicacao.CasosDeUso.PainelEducacional;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterIdepPorAnoEtapa;
using SME.SGP.Dominio;
using SME.SGP.Infra.Dtos.PainelEducacional;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.PainelEducacional
{
    public class ConsultasIdepPainelEducacionalUseCaseTeste
    {
        private readonly Mock<IMediator> _mediator;
        private readonly ConsultasIdepPainelEducacionalUseCase _useCase;

        public ConsultasIdepPainelEducacionalUseCaseTeste()
        {
            _mediator = new Mock<IMediator>();
            _useCase = new ConsultasIdepPainelEducacionalUseCase(_mediator.Object);
        }

        [Fact]
        public async Task Deve_Retornar_Dados_Quando_Encontrar_No_Ano_Solicitado()
        {
            var dados = new List<PainelEducacionalIdepDto>
            {
                new() { AnoLetivo = 2023, Etapa = PainelEducacionalIdepEtapa.AnosIniciais, MediaGeral = 6.5M, Faixa = "5-6", Quantidade = 10 }
            };

            _mediator.Setup(m => m.Send(It.IsAny<ObterIdepPorAnoEtapaQuery>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(dados);

            var resultado = await _useCase.ObterIdepPorAnoEtapa(2023, "1", "123456");

            Assert.Equal(2023, resultado.AnoSolicitado);
            Assert.Equal(2023, resultado.AnoUtilizado);
            Assert.False(resultado.AnoSolicitadoSemDados);
            Assert.Equal(PainelEducacionalIdepEtapa.AnosIniciais.ToString(), resultado.Etapa);
            Assert.Equal(6.5, resultado.MediaGeral);
        }

        [Fact]
        public async Task Deve_Retornar_Dados_Do_Ano_Anterior_Se_Ano_Solicitado_Sem_Dados()
        {
            var dados2022 = new List<PainelEducacionalIdepDto>
            {
                new() { AnoLetivo = 2022, Etapa = PainelEducacionalIdepEtapa.AnosIniciais, MediaGeral = 6.0M, Faixa = "5-6", Quantidade = 8 }
            };

            _mediator.SetupSequence(m => m.Send(It.IsAny<ObterIdepPorAnoEtapaQuery>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync((IEnumerable<PainelEducacionalIdepDto>)null)
                    .ReturnsAsync(dados2022);

            var resultado = await _useCase.ObterIdepPorAnoEtapa(2023, "1", "123456");

            Assert.Equal(2023, resultado.AnoSolicitado);
            Assert.Equal(2022, resultado.AnoUtilizado);
            Assert.True(resultado.AnoSolicitadoSemDados);
        }

        [Fact]
        public async Task Deve_Retornar_Vazio_Se_Nao_Encontrar_Nenhum_Ano()
        {
            _mediator.Setup(m => m.Send(It.IsAny<ObterIdepPorAnoEtapaQuery>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync((IEnumerable<PainelEducacionalIdepDto>)null);

            var resultado = await _useCase.ObterIdepPorAnoEtapa(2023, "1", "123456");

            Assert.Equal(2023, resultado.AnoSolicitado);
            Assert.True(resultado.AnoSolicitadoSemDados);
            Assert.Equal(0, resultado.MediaGeral);
            Assert.Empty(resultado.Distribuicao);
        }

        [Fact]
        public async Task Deve_Normalizar_Parametros_Com_Valores_Default()
        {
            var dados = new List<PainelEducacionalIdepDto>
            {
                new() { AnoLetivo = DateTime.Now.Year, Etapa = PainelEducacionalIdepEtapa.AnosIniciais, MediaGeral = 6.5M }
            };

            _mediator.Setup(m => m.Send(It.IsAny<ObterIdepPorAnoEtapaQuery>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(dados);

            var resultado = await _useCase.ObterIdepPorAnoEtapa(0, null, null);

            Assert.Equal(DateTime.Now.Year, resultado.AnoSolicitado);
            Assert.Equal(PainelEducacionalIdepEtapa.AnosIniciais.ToString(), resultado.Etapa);
        }
    }
}