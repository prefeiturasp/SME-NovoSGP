using Bogus;
using FluentAssertions;
using Moq;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterNumeroAlunos;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces.Repositorios;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Queries.PainelEducacional
{
    public class PainelEducacionalNumeroEstudantesAgrupamentoNivelAlfabetizacaoQueryHandlerTeste
    {
        private readonly Mock<IRepositorioConsolidacaoAlfabetizacaoNivelEscrita> _repositorioMock;
        private readonly PainelEducacionalNumeroEstudantesAgrupamentoNivelAlfabetizacaoQueryHandler _sut;
        private readonly Faker _faker;

        public PainelEducacionalNumeroEstudantesAgrupamentoNivelAlfabetizacaoQueryHandlerTeste()
        {
            _repositorioMock = new Mock<IRepositorioConsolidacaoAlfabetizacaoNivelEscrita>();
            _sut = new PainelEducacionalNumeroEstudantesAgrupamentoNivelAlfabetizacaoQueryHandler(_repositorioMock.Object);
            _faker = new Faker();
        }

        [Fact]
        public async Task Handle_QuandoRepositorioRetornaDados_DeveMapearParaDtoCorretamente()
        {
            var query = new PainelEducacionalNumeroEstudantesAgrupamentoNivelAlfabetizacaoQuery(2025, 1, "108300", "094002");

            var dadosDoRepositorio = new List<ConsolidacaoAlfabetizacaoNivelEscrita>
             {
                 new ConsolidacaoAlfabetizacaoNivelEscrita
                 {
                     NivelEscrita = "PS",
                     Quantidade = 10,
                     DreCodigo = query.CodigoDre,
                     UeCodigo = query.CodigoUe,
                     AnoLetivo = (short)query.AnoLetivo,
                     Periodo = (short) query.Periodo
                 },
                 new ConsolidacaoAlfabetizacaoNivelEscrita
                 {
                     NivelEscrita = "A",
                     Quantidade = 20,
                     DreCodigo = query.CodigoDre,
                     UeCodigo = query.CodigoUe,
                     AnoLetivo = (short) query.AnoLetivo,
                     Periodo = (short) query.Periodo
                 }
             };

            _repositorioMock
                .Setup(r => r.ObterNumeroAlunos(query.AnoLetivo, query.Periodo, query.CodigoDre, query.CodigoUe))
                .ReturnsAsync(dadosDoRepositorio);

            var resultado = await _sut.Handle(query, CancellationToken.None);

            resultado.Should().NotBeNull();
            resultado.Should().HaveCount(2);

            var resultadoPreSilabico = resultado.FirstOrDefault(r => r.CodigoNivelAlfabetizacao == (int)NivelAlfabetizacao.PreSilabico);
            resultadoPreSilabico.Should().NotBeNull();
            resultadoPreSilabico.TotalAlunos.Should().Be(10);
            resultadoPreSilabico.Dre.Should().Be(query.CodigoDre);
            resultadoPreSilabico.Ue.Should().Be(query.CodigoUe);
            resultadoPreSilabico.Ano.Should().Be(query.AnoLetivo);
            resultadoPreSilabico.Periodo.Should().Be(query.Periodo);

            var resultadoAlfabetico = resultado.FirstOrDefault(r => r.CodigoNivelAlfabetizacao == (int)NivelAlfabetizacao.Alfabetico);
            resultadoAlfabetico.Should().NotBeNull();
            resultadoAlfabetico.TotalAlunos.Should().Be(20);
            resultadoAlfabetico.Dre.Should().Be(query.CodigoDre);
            resultadoAlfabetico.Ue.Should().Be(query.CodigoUe);
            resultadoAlfabetico.Ano.Should().Be(query.AnoLetivo);
            resultadoAlfabetico.Periodo.Should().Be(query.Periodo);

            _repositorioMock.Verify(r => r.ObterNumeroAlunos(2025, 1, "108300", "094002"), Times.Once);
        }

        [Theory(DisplayName = "Deve mapear corretamente a string do nível de escrita para o enum correspondente")]
        [InlineData("PS", NivelAlfabetizacao.PreSilabico)]
        [InlineData("SSV", NivelAlfabetizacao.SilabicoSemValor)]
        [InlineData("SCV", NivelAlfabetizacao.SilabicoComValor)]
        [InlineData("SA", NivelAlfabetizacao.SilabicoAlfabetico)]
        [InlineData("A", NivelAlfabetizacao.Alfabetico)]
        [InlineData(" ps ", NivelAlfabetizacao.PreSilabico)]
        [InlineData("a", NivelAlfabetizacao.Alfabetico)]
        [InlineData("", NivelAlfabetizacao.PreSilabico)]   
        [InlineData(null, NivelAlfabetizacao.PreSilabico)] 
        public async Task Handle_DeveMapearNivelEscritaStringParaEnumCorretamente(string nivelEscritaString, NivelAlfabetizacao nivelEsperado)
        {
            var query = new PainelEducacionalNumeroEstudantesAgrupamentoNivelAlfabetizacaoQuery(2025, 1);
            var registroUnico = new List<ConsolidacaoAlfabetizacaoNivelEscrita>
             {
                 new ConsolidacaoAlfabetizacaoNivelEscrita
                 {
                     NivelEscrita = nivelEscritaString,
                     Quantidade = _faker.Random.Int(1, 50),
                     DreCodigo = "DRE001",
                     UeCodigo = "UE001",
                     AnoLetivo = 2025,
                     Periodo = 1
                 }
             };

            _repositorioMock.Setup(r => r.ObterNumeroAlunos(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()))
                            .ReturnsAsync(registroUnico);

            var displayAttribute = nivelEsperado.GetType()
                .GetField(nivelEsperado.ToString())
                ?.GetCustomAttribute<DisplayAttribute>();

            var nomeEsperado = displayAttribute?.Name ?? nivelEsperado.ToString();
            var descricaoEsperada = displayAttribute?.Description ?? nivelEsperado.ToString();

            var resultado = await _sut.Handle(query, CancellationToken.None);

            resultado.Should().NotBeEmpty();
            var dto = resultado.First();

            dto.CodigoNivelAlfabetizacao.Should().Be((int)nivelEsperado, "o código deve corresponder ao ID do enum");
            dto.NivelAlfabetizacao.Should().Be(nomeEsperado, "o nome deve corresponder ao valor Name do DisplayAttribute");
            dto.NivelAlfabetizacaoDescricao.Should().Be(descricaoEsperada, "a descrição deve corresponder ao valor Description do DisplayAttribute");
        }

        [Fact]
        public async Task Handle_QuandoRepositorioRetornaVazio_DeveRetornarVazio()
        {
            var query = new PainelEducacionalNumeroEstudantesAgrupamentoNivelAlfabetizacaoQuery(2025, 1);
            _repositorioMock.Setup(r => r.ObterNumeroAlunos(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()))
                            .ReturnsAsync(new List<ConsolidacaoAlfabetizacaoNivelEscrita>());
            var resultado = await _sut.Handle(query, CancellationToken.None);

            resultado.Should().NotBeNull();
            resultado.Should().BeEmpty();
        }
    }
}
