using Moq;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterIdepPorAnoEtapa;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra.Dtos.PainelEducacional;
using System;
using System.Collections.Generic;
using System.Linq;
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
                new PainelEducacionalIdepDto
                {
                    AnoLetivo = 2023,
                    Etapa = PainelEducacionalIdepEtapa.AnosIniciais,
                    Faixa = "5.0-6.9",
                    CodigoDre = "123456",
                    Quantidade = 10,
                    MediaGeral = 5.5m,
                    UltimaAtualizacao = new DateTime(2023, 10, 1)
                }
            };

            var query = new ObterIdepPorAnoEtapaQuery(2023, PainelEducacionalIdepEtapa.AnosIniciais.ToString(), "123456");

            _repositorio.Setup(r => r.ObterIdepPorAnoEtapa(2023, PainelEducacionalIdepEtapa.AnosIniciais.ToString(), "123456"))
                       .ReturnsAsync(dados);

            var resultado = await _handler.Handle(query, CancellationToken.None);
            var resultadoLista = resultado.ToList();

            Assert.Single(resultadoLista);
            var item = resultadoLista.First();
            Assert.Equal(2023, item.AnoLetivo);
            Assert.Equal(PainelEducacionalIdepEtapa.AnosIniciais, item.Etapa);
            Assert.Equal("5.0-6.9", item.Faixa);
            Assert.Equal("123456", item.CodigoDre);
            Assert.Equal(10, item.Quantidade);
            Assert.Equal(5.5m, item.MediaGeral);
            Assert.Equal(new DateTime(2023, 10, 1), item.UltimaAtualizacao);
        }

        [Fact]
        public async Task Deve_Retornar_Dados_Agrupados_E_Ordenados()
        {
            var dadosRepositorio = new List<PainelEducacionalIdepDto>
            {
                new PainelEducacionalIdepDto 
                { 
                    AnoLetivo = 2023, 
                    Etapa = PainelEducacionalIdepEtapa.AnosIniciais,
                    Faixa = "0-4.9",
                    CodigoDre = "123456",
                    Quantidade = 10,
                    MediaGeral = 4.5m,
                    UltimaAtualizacao = new DateTime(2023, 10, 1)
                },
                new PainelEducacionalIdepDto 
                { 
                    AnoLetivo = 2023, 
                    Etapa = PainelEducacionalIdepEtapa.AnosIniciais,
                    Faixa = "0-4.9",
                    CodigoDre = "123456",
                    Quantidade = 5,
                    MediaGeral = 3.5m,
                    UltimaAtualizacao = new DateTime(2023, 10, 15)
                },
                new PainelEducacionalIdepDto 
                { 
                    AnoLetivo = 2023, 
                    Etapa = PainelEducacionalIdepEtapa.AnosIniciais,
                    Faixa = "5.0-6.9",
                    CodigoDre = "123456",
                    Quantidade = 8,
                    MediaGeral = 6.0m,
                    UltimaAtualizacao = new DateTime(2023, 10, 10)
                }
            };

            var query = new ObterIdepPorAnoEtapaQuery(2023, PainelEducacionalIdepEtapa.AnosIniciais.ToString(), "123456");

            _repositorio.Setup(r => r.ObterIdepPorAnoEtapa(2023, PainelEducacionalIdepEtapa.AnosIniciais.ToString(), "123456"))
                       .ReturnsAsync(dadosRepositorio);

            var resultado = await _handler.Handle(query, CancellationToken.None);
            var resultadoLista = resultado.ToList();

            Assert.Equal(2, resultadoLista.Count);

            var primeiroGrupo = resultadoLista.First(x => x.Faixa == "0-4.9");
            Assert.Equal(2023, primeiroGrupo.AnoLetivo);
            Assert.Equal(PainelEducacionalIdepEtapa.AnosIniciais, primeiroGrupo.Etapa);
            Assert.Equal("123456", primeiroGrupo.CodigoDre);
            Assert.Equal(15, primeiroGrupo.Quantidade); 
            Assert.Equal(4.0m, primeiroGrupo.MediaGeral); 
            Assert.Equal(new DateTime(2023, 10, 15), primeiroGrupo.UltimaAtualizacao); 

            var segundoGrupo = resultadoLista.First(x => x.Faixa == "5.0-6.9");
            Assert.Equal(2023, segundoGrupo.AnoLetivo);
            Assert.Equal(PainelEducacionalIdepEtapa.AnosIniciais, segundoGrupo.Etapa);
            Assert.Equal("123456", segundoGrupo.CodigoDre);
            Assert.Equal(8, segundoGrupo.Quantidade);
            Assert.Equal(6.0m, segundoGrupo.MediaGeral);
            Assert.Equal(new DateTime(2023, 10, 10), segundoGrupo.UltimaAtualizacao);

            Assert.Equal("0-4.9", resultadoLista[0].Faixa);
            Assert.Equal("5.0-6.9", resultadoLista[1].Faixa);
        }

        [Fact]
        public async Task Deve_Retornar_Lista_Vazia_Quando_Repositorio_Nao_Retorna_Dados()
        {
            var query = new ObterIdepPorAnoEtapaQuery(2023, PainelEducacionalIdepEtapa.AnosIniciais.ToString(), "123456");

            _repositorio.Setup(r => r.ObterIdepPorAnoEtapa(2023, PainelEducacionalIdepEtapa.AnosIniciais.ToString(), "123456"))
                       .ReturnsAsync(new List<PainelEducacionalIdepDto>());

            var resultado = await _handler.Handle(query, CancellationToken.None);

            Assert.Empty(resultado);
        }

        [Fact]
        public void Deve_Lancar_ArgumentNullException_Quando_Repositorio_For_Null()
        {
            Assert.Throws<ArgumentNullException>(() => new ObterIdepPorAnoEtapaQueryHandler(null));
        }
    }
}