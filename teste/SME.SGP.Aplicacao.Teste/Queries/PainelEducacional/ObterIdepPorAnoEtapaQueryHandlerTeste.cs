using Moq;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterIdepPorAnoEtapa;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra.Dtos.PainelEducacional;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Queries.PainelEducacional.ObterIdepPorAnoEtapa
{
    public class ObterIdepPorAnoEtapaQueryHandlerTeste
    {
        private readonly Mock<IRepositorioIdepPainelEducacionalConsulta> _repositorio;
        private readonly ObterIdepPorAnoEtapaQueryHandler _handler;

        public ObterIdepPorAnoEtapaQueryHandlerTeste()
        {
            _repositorio = new Mock<IRepositorioIdepPainelEducacionalConsulta>();
            _handler = new ObterIdepPorAnoEtapaQueryHandler(_repositorio.Object);
        }

        [Fact]
        public async Task Deve_Chamar_Repositorio_Com_Parametros_Corretos()
        {
            var query = new ObterIdepPorAnoEtapaQuery(2023, PainelEducacionalIdepEtapa.AnosIniciais.ToString(), "123456");

            await _handler.Handle(query, CancellationToken.None);

            _repositorio.Verify(r => r.ObterIdepPorAnoEtapa(2023, PainelEducacionalIdepEtapa.AnosIniciais.ToString(), "123456"), Times.Once);
        }

        [Fact]
        public async Task Deve_Retornar_Dados_Do_Repositorio()
        {
            var dados = new List<PainelEducacionalIdepDto>
            {
                new PainelEducacionalIdepDto { AnoLetivo = 2023, Etapa = PainelEducacionalIdepEtapa.AnosIniciais }
            };

            var query = new ObterIdepPorAnoEtapaQuery(2023, PainelEducacionalIdepEtapa.AnosIniciais.ToString(), "123456");

            _repositorio.Setup(r => r.ObterIdepPorAnoEtapa(2023, PainelEducacionalIdepEtapa.AnosIniciais.ToString(), "123456"))
                       .ReturnsAsync(dados);

            var resultado = await _handler.Handle(query, CancellationToken.None);

            Assert.Equal(dados, resultado);
        }
    }
}