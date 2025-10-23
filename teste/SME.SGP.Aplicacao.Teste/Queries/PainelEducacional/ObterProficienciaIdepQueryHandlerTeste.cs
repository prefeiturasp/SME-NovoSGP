using Moq;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterProficienciaIdep;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra.Dtos.PainelEducacional;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Queries.PainelEducacional.ObterProficienciaIdep
{
    public class ObterProficienciaIdepQueryHandlerTeste
    {
        [Fact]
        public void Construtor_DeveLancarArgumentNullException_QuandoRepositorioEhNulo()
        {
            Assert.Throws<ArgumentNullException>(() => new ObterProficienciaIdepQueryHandler(null));
        }

        [Fact]
        public async Task Handle_DeveRetornarProficienciaIdepAgrupadaDto_QuandoRepositorioRetornaDados()
        {
            var mockRepositorio = new Mock<IRepositorioPainelEducacionalProficienciaIdep>();
            var handler = new ObterProficienciaIdepQueryHandler(mockRepositorio.Object);
            var query = new ObterProficienciaIdepQuery(2023, "123456");
            var retornoEsperado = new List<ProficienciaIdepAgrupadaDto>
            {
                new ProficienciaIdepAgrupadaDto { AnoLetivo = 2023, ComponenteCurricular = 1, EtapaEnsino = 2, ProficienciaMedia = 7.5m, Boletim = "A" }
            };

            mockRepositorio.Setup(r => r.ObterProficienciaIdep(query.AnoLetivo, query.CodigoUe))
                .ReturnsAsync(retornoEsperado);

            var resultado = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.Single(resultado);
            Assert.Equal(2023, ((List<ProficienciaIdepAgrupadaDto>)resultado)[0].AnoLetivo);
        }
    }
}
