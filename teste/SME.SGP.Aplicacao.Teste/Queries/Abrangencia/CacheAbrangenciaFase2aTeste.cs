using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Queries.Abrangencia
{
    public class CacheAbrangenciaFase2aTeste
    {
        private const string LOGIN = "7924488";

        private readonly Mock<IMediator> mediator = new();
        private readonly Mock<IRepositorioAbrangencia> repositorioAbrangencia = new();
        private readonly Mock<IRepositorioCacheAbrangencia> repositorioCache = new();
        private readonly List<string> chavesLidas = new();

        public CacheAbrangenciaFase2aTeste()
        {
            repositorioCache.Setup(c => c.ObterAsync(It.IsAny<string>(), false))
                .Callback<string, bool>((chave, _) => chavesLidas.Add(chave))
                .ReturnsAsync(string.Empty);
            repositorioCache.Setup(c => c.SalvarAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<bool>()))
                .Returns(Task.CompletedTask);
        }

        private void CacheLigado(bool ligado = true)
            => mediator.Setup(m => m.Send(It.IsAny<ObterConfiguracaoCacheAbrangenciaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(ligado ? new ConfiguracaoCacheAbrangenciaDto { Minutos = 5, Cj = 1 } : null);

        // ---------- relatorios/filtros/dres/{dre}/ues ----------

        private ObterFiltroRelatoriosUesPorAbrangenciaQueryHandler HandlerUesRelatorio()
        {
            mediator.Setup(m => m.Send(It.IsAny<ObterNovosTiposUEPorAnoQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync("31,29");
            mediator.Setup(m => m.Send(It.IsAny<ObterUsuarioPossuiAbrangenciaEmTodasAsUesQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);
            repositorioAbrangencia.Setup(r => r.ObterUes(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Guid>(),
                    It.IsAny<FiltroModalidade>(), It.IsAny<FiltroPeriodoLetivo>(), It.IsAny<int[]>(), It.IsAny<FiltroTexto>()))
                .ReturnsAsync(new[]
                {
                    new AbrangenciaUeRetorno { Id = 1, Codigo = "019769", NomeSimples = "B", TipoEscola = TipoEscola.EMEF },
                    new AbrangenciaUeRetorno { Id = 2, Codigo = "019770", NomeSimples = "A", TipoEscola = TipoEscola.EMEF }
                });

            return new ObterFiltroRelatoriosUesPorAbrangenciaQueryHandler(mediator.Object, repositorioAbrangencia.Object, repositorioCache.Object);
        }

        private static ObterFiltroRelatoriosUesPorAbrangenciaQuery QueryUesRelatorio(bool usarCache)
            => new(new Usuario { Login = LOGIN, PerfilAtual = Perfis.PERFIL_ADMCOTIC }, "108100", 2026) { UsarCache = usarCache };

        [Fact]
        public async Task Deve_Usar_A_Mesma_Chave_Das_Ues_Da_Fase_1_No_Filtro_De_Relatorio()
        {
            //Arrange
            CacheLigado();
            var handler = HandlerUesRelatorio();

            //Act
            await handler.Handle(QueryUesRelatorio(usarCache: true), CancellationToken.None);

            //Assert
            Assert.Equal($"abrangencia-ues:{LOGIN}-{Perfis.PERFIL_ADMCOTIC}-108100-0-2026-False-0-29,31", chavesLidas.Single());
        }

        [Fact]
        public async Task Deve_Manter_Ordenacao_E_Opcao_Todas_Fora_Do_Cache_No_Filtro_De_Relatorio()
        {
            //Arrange
            CacheLigado();
            var handler = HandlerUesRelatorio();

            //Act
            var retorno = await handler.Handle(QueryUesRelatorio(usarCache: true), CancellationToken.None);

            //Assert
            Assert.Equal(new[] { "-99", "019770", "019769" }, retorno.Select(u => u.Codigo));
        }

        [Fact]
        public async Task Deve_Ir_Direto_Ao_Banco_No_Filtro_De_Relatorio_Quando_UsarCache_For_False()
        {
            //Arrange
            CacheLigado();
            var handler = HandlerUesRelatorio();

            //Act
            await handler.Handle(QueryUesRelatorio(usarCache: false), CancellationToken.None);

            //Assert
            Assert.Empty(chavesLidas);
        }

        // ---------- relatorios/filtros/ues/{ue}/modalidades/abrangencias ----------

        private ObterFiltroRelatoriosModalidadesPorUeAbrangenciaQueryHandler HandlerModalidadesUe()
        {
            repositorioAbrangencia.Setup(r => r.ObterModalidadesPorUeAbrangencia(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Guid>(),
                    It.IsAny<IEnumerable<Modalidade>>(), It.IsAny<bool>(), It.IsAny<int>()))
                .ReturnsAsync(new[] { Modalidade.Fundamental });

            return new ObterFiltroRelatoriosModalidadesPorUeAbrangenciaQueryHandler(repositorioAbrangencia.Object,
                new Mock<IRepositorioSupervisorEscolaDre>().Object, repositorioCache.Object, mediator.Object);
        }

        [Fact]
        public async Task Deve_Gerar_Chave_Com_Ue_E_Modalidades_Ignoradas_Ordenadas()
        {
            //Arrange
            CacheLigado();
            var handler = HandlerModalidadesUe();
            var query = new ObterFiltroRelatoriosModalidadesPorUeAbrangenciaQuery("019769", LOGIN, Perfis.PERFIL_ADMCOTIC,
                new[] { Modalidade.Medio, Modalidade.EJA }, false, 2026) { UsarCache = true };

            //Act
            await handler.Handle(query, CancellationToken.None);

            //Assert
            Assert.Equal($"abrangencia-modalidades-ue:{LOGIN}-{Perfis.PERFIL_ADMCOTIC}-019769-False-2026-3,6", chavesLidas.Single());
        }

        [Fact]
        public async Task Deve_Retornar_Modalidades_Do_Cache_Sem_Chamar_O_Banco()
        {
            //Arrange
            CacheLigado();
            var handler = HandlerModalidadesUe();
            repositorioCache.Setup(c => c.ObterAsync(It.IsAny<string>(), false))
                .ReturnsAsync(JsonConvert.SerializeObject(new[] { Modalidade.Fundamental }));
            var query = new ObterFiltroRelatoriosModalidadesPorUeAbrangenciaQuery("019769", LOGIN, Perfis.PERFIL_ADMCOTIC, null, false, 2026) { UsarCache = true };

            //Act
            var retorno = await handler.Handle(query, CancellationToken.None);

            //Assert
            repositorioAbrangencia.Verify(r => r.ObterModalidadesPorUeAbrangencia(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Guid>(),
                It.IsAny<IEnumerable<Modalidade>>(), It.IsAny<bool>(), It.IsAny<int>()), Times.Never);
            Assert.Equal(((int)Modalidade.Fundamental).ToString(), retorno.Single().Valor);
        }

        // ---------- ObterNovosTiposUEPorAnoQuery ----------

        private ObterNovosTiposUEPorAnoQueryHandler HandlerNovosTipos(Mock<IRepositorioParametrosSistemaConsulta> repositorioParametros, string valor)
        {
            repositorioParametros.Setup(r => r.ObterNovosTiposUEPorAno(It.IsAny<int>())).ReturnsAsync(valor);
            return new ObterNovosTiposUEPorAnoQueryHandler(repositorioParametros.Object, mediator.Object, new MemoryCache(new MemoryCacheOptions()));
        }

        [Fact]
        public async Task Deve_Ler_Novos_Tipos_De_Ue_Do_Banco_Toda_Vez_Quando_Cache_Desligado()
        {
            //Arrange
            CacheLigado(false);
            var repositorioParametros = new Mock<IRepositorioParametrosSistemaConsulta>();
            var handler = HandlerNovosTipos(repositorioParametros, "29,31");

            //Act
            await handler.Handle(new ObterNovosTiposUEPorAnoQuery(2026), CancellationToken.None);
            await handler.Handle(new ObterNovosTiposUEPorAnoQuery(2026), CancellationToken.None);

            //Assert
            repositorioParametros.Verify(r => r.ObterNovosTiposUEPorAno(2026), Times.Exactly(2));
        }

        [Theory]
        [InlineData("29,31")]
        [InlineData(null)]
        public async Task Deve_Ler_Novos_Tipos_De_Ue_Uma_Vez_Por_Ano_Quando_Cache_Ligado(string valor)
        {
            //Arrange
            CacheLigado();
            var repositorioParametros = new Mock<IRepositorioParametrosSistemaConsulta>();
            var handler = HandlerNovosTipos(repositorioParametros, valor);

            //Act
            var primeiro = await handler.Handle(new ObterNovosTiposUEPorAnoQuery(2026), CancellationToken.None);
            var segundo = await handler.Handle(new ObterNovosTiposUEPorAnoQuery(2026), CancellationToken.None);
            await handler.Handle(new ObterNovosTiposUEPorAnoQuery(2027), CancellationToken.None);

            //Assert
            Assert.Equal(valor, primeiro);
            Assert.Equal(valor, segundo);
            repositorioParametros.Verify(r => r.ObterNovosTiposUEPorAno(2026), Times.Once);
            repositorioParametros.Verify(r => r.ObterNovosTiposUEPorAno(2027), Times.Once);
        }

        // ---------- chave de texto ----------

        [Fact]
        public void Deve_Diferenciar_Texto_Nulo_De_Vazio_Na_Chave()
        {
            //Act & Assert
            Assert.Equal("null", ((string)null).ParaChaveCache());
            Assert.Equal(string.Empty, string.Empty.ParaChaveCache());
            Assert.Equal("108100", "108100".ParaChaveCache());
        }
    }
}
