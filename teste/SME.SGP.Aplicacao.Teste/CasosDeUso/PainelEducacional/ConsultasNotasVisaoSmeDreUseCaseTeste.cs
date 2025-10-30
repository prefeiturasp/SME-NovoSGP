using MediatR;
using Moq;
using SME.SGP.Aplicacao.CasosDeUso.PainelEducacional;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterNotas.VisaoSmeDre;
using SME.SGP.Dominio;
using SME.SGP.Infra.Dtos.PainelEducacional.Notas.VisaoSmeDre;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.PainelEducacional
{
    public class ConsultasNotasVisaoSmeDreUseCaseTeste
    {
        private readonly Mock<IMediator> mockMediator;
        private readonly Mock<IContextoAplicacao> mockContextoAplicacao;
        private readonly ConsultasNotasVisaoSmeDreUseCase useCase;

        public ConsultasNotasVisaoSmeDreUseCaseTeste()
        {
            mockMediator = new Mock<IMediator>();
            mockContextoAplicacao = new Mock<IContextoAplicacao>();
            useCase = new ConsultasNotasVisaoSmeDreUseCase(mockContextoAplicacao.Object, mockMediator.Object);
        }

        [Fact]
        public void Executar_Quando_Instanciar_Mediator_Nulo_Deve_Lancar_Argument_Null_Exception()
        {
            Assert.Throws<ArgumentNullException>(() => new ConsultasNotasVisaoSmeDreUseCase(mockContextoAplicacao.Object, null));
        }

        [Fact]
        public void Executar_Quando_Instanciar_Contexto_Aplicacao_Nulo_Deve_Lancar_Argument_Null_Exception()
        {
            Assert.Throws<ArgumentNullException>(() => new ConsultasNotasVisaoSmeDreUseCase(null, mockMediator.Object));
        }

        [Fact]
        public async Task Executar_Quando_Obter_Notas_Visao_Sme_Dre_Retornar_Nulo_Deve_Retornar_Vazio()
        {
            mockMediator
                .Setup(m => m.Send(It.IsAny<ObterNotaVisaoSmeDreQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((IEnumerable<PainelEducacionalNotasVisaoSmeDreRetornoSelectDto>)null);

            var resultado = await useCase.ObterNotasVisaoSmeDre("dre", 2024, 1, "9");

            Assert.NotNull(resultado);
            Assert.Empty(resultado);
        }

        [Fact]
        public async Task Executar_Quando_Obter_Notas_Visao_Sme_Dre_Retornar_Lista_Vazia_Deve_Retornar_Vazio()
        {
            var listaVazia = new List<PainelEducacionalNotasVisaoSmeDreRetornoSelectDto>();

            mockMediator
                .Setup(m => m.Send(It.IsAny<ObterNotaVisaoSmeDreQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(listaVazia);

            var resultado = await useCase.ObterNotasVisaoSmeDre("dre", 2024, 1, "9");

            Assert.NotNull(resultado);
            Assert.Empty(resultado);
        }

        [Fact]
        public async Task Executar_Quando_Obter_Notas_Visao_Sme_Dre_Retornar_Dados_Deve_Agrupar_E_Retornar_Dto()
        {
            var dadosConsolidacao = new List<PainelEducacionalNotasVisaoSmeDreRetornoSelectDto>
            {
                new PainelEducacionalNotasVisaoSmeDreRetornoSelectDto
                {
                    AnoLetivo = 2024, Modalidade = Modalidade.Fundamental, AnoTurma = "9", Bimestre = 1,
                    QuantidadeAbaixoMediaPortugues = 10, QuantidadeAcimaMediaPortugues = 90,
                    QuantidadeAbaixoMediaMatematica = 20, QuantidadeAcimaMediaMatematica = 80,
                    QuantidadeAbaixoMediaCiencias = 5, QuantidadeAcimaMediaCiencias = 95
                },
                new PainelEducacionalNotasVisaoSmeDreRetornoSelectDto
                {
                    AnoLetivo = 2024, Modalidade = Modalidade.Fundamental, AnoTurma = "9", Bimestre = 1,
                    QuantidadeAbaixoMediaPortugues = 5, QuantidadeAcimaMediaPortugues = 45,
                    QuantidadeAbaixoMediaMatematica = 10, QuantidadeAcimaMediaMatematica = 40,
                    QuantidadeAbaixoMediaCiencias = 2, QuantidadeAcimaMediaCiencias = 48
                },
                new PainelEducacionalNotasVisaoSmeDreRetornoSelectDto
                {
                    AnoLetivo = 2024, Modalidade = Modalidade.Fundamental, AnoTurma = "8", Bimestre = 1,
                    QuantidadeAbaixoMediaPortugues = 1, QuantidadeAcimaMediaPortugues = 19,
                    QuantidadeAbaixoMediaMatematica = 2, QuantidadeAcimaMediaMatematica = 18,
                    QuantidadeAbaixoMediaCiencias = 3, QuantidadeAcimaMediaCiencias = 17
                },
                new PainelEducacionalNotasVisaoSmeDreRetornoSelectDto
                {
                    AnoLetivo = 2024, Modalidade = Modalidade.EJA, AnoTurma = "1", Bimestre = 1,
                    QuantidadeAbaixoMediaPortugues = 50, QuantidadeAcimaMediaPortugues = 50,
                    QuantidadeAbaixoMediaMatematica = 60, QuantidadeAcimaMediaMatematica = 40,
                    QuantidadeAbaixoMediaCiencias = 70, QuantidadeAcimaMediaCiencias = 30
                }
            };

            mockMediator
                .Setup(m => m.Send(It.Is<ObterNotaVisaoSmeDreQuery>(q =>
                    q.CodigoDre == "dre-id" &&
                    q.AnoLetivo == 2024 &&
                    q.Bimestre == 1 &&
                    q.AnoTurma == "todos"), It.IsAny<CancellationToken>()))
                .ReturnsAsync(dadosConsolidacao);

            var resultado = await useCase.ObterNotasVisaoSmeDre("dre-id", 2024, 1, "todos");

            Assert.NotNull(resultado);
            Assert.Single(resultado);

            var painelDto = resultado.First();
            Assert.NotNull(painelDto.Modalidades);
            Assert.Equal(2, painelDto.Modalidades.Count());

            var modalidadeFundamental = painelDto.Modalidades.FirstOrDefault(m => m.SerieAno.Count() == 2);
            Assert.NotNull(modalidadeFundamental);
            Assert.Equal(2, modalidadeFundamental.SerieAno.Count());

            var serie9 = modalidadeFundamental.SerieAno.FirstOrDefault(s => s.Nome == "9");
            Assert.NotNull(serie9);
            Assert.Equal(3, serie9.ComponentesCurriculares.Count());

            var port9 = serie9.ComponentesCurriculares.FirstOrDefault(c => c.Nome == "Português");
            Assert.Equal(15, port9.AbaixoDaMedia);
            Assert.Equal(135, port9.AcimaDaMedia);

            var mat9 = serie9.ComponentesCurriculares.FirstOrDefault(c => c.Nome == "Matemática");
            Assert.Equal(30, mat9.AbaixoDaMedia);
            Assert.Equal(120, mat9.AcimaDaMedia);

            var cie9 = serie9.ComponentesCurriculares.FirstOrDefault(c => c.Nome == "Ciências");
            Assert.Equal(7, cie9.AbaixoDaMedia);
            Assert.Equal(143, cie9.AcimaDaMedia);

            var serie8 = modalidadeFundamental.SerieAno.FirstOrDefault(s => s.Nome == "8");
            Assert.NotNull(serie8);
            var port8 = serie8.ComponentesCurriculares.FirstOrDefault(c => c.Nome == "Português");
            Assert.Equal(1, port8.AbaixoDaMedia);
            Assert.Equal(19, port8.AcimaDaMedia);

            var modalidadeEja = painelDto.Modalidades.FirstOrDefault(m => m.SerieAno.Count() == 1);
            Assert.NotNull(modalidadeEja);
            Assert.Single(modalidadeEja.SerieAno);

            var serie1Eja = modalidadeEja.SerieAno.FirstOrDefault(s => s.Nome == "1");
            Assert.NotNull(serie1Eja);
            var matEja = serie1Eja.ComponentesCurriculares.FirstOrDefault(c => c.Nome == "Matemática");
            Assert.Equal(60, matEja.AbaixoDaMedia);
            Assert.Equal(40, matEja.AcimaDaMedia);
        }
    }
}
