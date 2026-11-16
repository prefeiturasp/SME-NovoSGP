using MediatR;
using Moq;
using Newtonsoft.Json;
using SME.SGP.Aplicacao.Queries.UE.ObterUEsPorDRE;
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
    public class ObterUEsPorDREQueryHandlerCacheTeste
    {
        private const string LOGIN = "7924488";
        private const string CODIGO_DRE = "108100";

        private readonly Mock<IMediator> mediator;
        private readonly Mock<IRepositorioAbrangencia> repositorioAbrangencia;
        private readonly Mock<IRepositorioCacheAbrangencia> repositorioCache;
        private readonly ObterUEsPorDREQueryHandler handler;
        private readonly List<string> chavesLidas = new();
        private readonly List<int> minutosGravados = new();

        private static readonly AbrangenciaUeRetorno[] UES_BANCO =
        {
            new AbrangenciaUeRetorno { Id = 1, Codigo = "019769", NomeSimples = "MARINA COLASANTI", TipoEscola = TipoEscola.EMEF }
        };

        public ObterUEsPorDREQueryHandlerCacheTeste()
        {
            mediator = new Mock<IMediator>();
            repositorioAbrangencia = new Mock<IRepositorioAbrangencia>();
            repositorioCache = new Mock<IRepositorioCacheAbrangencia>();

            mediator.Setup(m => m.Send(It.IsAny<ObterNovosTiposUEPorAnoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync("29,31");

            repositorioAbrangencia.Setup(r => r.ObterUes(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Guid>(),
                    It.IsAny<FiltroModalidade>(), It.IsAny<FiltroPeriodoLetivo>(), It.IsAny<int[]>(), It.IsAny<FiltroTexto>()))
                .ReturnsAsync(UES_BANCO);

            repositorioCache.Setup(c => c.ObterAsync(It.IsAny<string>(), false))
                .Callback<string, bool>((chave, _) => chavesLidas.Add(chave))
                .ReturnsAsync(string.Empty);

            repositorioCache.Setup(c => c.SalvarAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<bool>()))
                .Callback<string, string, int, bool>((_, _, minutos, _) => minutosGravados.Add(minutos))
                .Returns(Task.CompletedTask);

            handler = new ObterUEsPorDREQueryHandler(mediator.Object, repositorioAbrangencia.Object, repositorioCache.Object);
        }

        private void CacheLigado(ConfiguracaoCacheAbrangenciaDto configuracao = null)
            => mediator.Setup(m => m.Send(It.IsAny<ObterConfiguracaoCacheAbrangenciaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(configuracao ?? new ConfiguracaoCacheAbrangenciaDto { Minutos = 5, Cj = 1 });

        private static ObterUEsPorDREQuery Query(bool usarCache = true, int anoLetivo = 2026, string login = LOGIN, Guid? perfil = null,
            string filtro = "", string[] anosTurma = null)
            => new ObterUEsPorDREQuery(new UEsPorDreDto
            {
                CodigoDre = CODIGO_DRE,
                AnoLetivo = anoLetivo,
                Modalidade = Modalidade.Fundamental,
                Filtro = filtro,
                AnosTurma = anosTurma
            }, login, perfil ?? Perfis.PERFIL_ADMCOTIC, usarCache);

        private void VerificarRepositorioChamado(Times vezes)
            => repositorioAbrangencia.Verify(r => r.ObterUes(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Guid>(),
                It.IsAny<FiltroModalidade>(), It.IsAny<FiltroPeriodoLetivo>(), It.IsAny<int[]>(), It.IsAny<FiltroTexto>()), vezes);

        [Fact]
        public async Task Deve_Ir_Direto_Ao_Banco_Quando_UsarCache_For_False()
        {
            //Arrange
            CacheLigado();

            //Act
            await handler.Handle(Query(usarCache: false), CancellationToken.None);

            //Assert
            VerificarRepositorioChamado(Times.Once());
            mediator.Verify(m => m.Send(It.IsAny<ObterConfiguracaoCacheAbrangenciaQuery>(), It.IsAny<CancellationToken>()), Times.Never);
            Assert.Empty(chavesLidas);
        }

        [Fact]
        public async Task Deve_Ir_Direto_Ao_Banco_Quando_Parametro_Estiver_Desligado()
        {
            //Arrange
            mediator.Setup(m => m.Send(It.IsAny<ObterConfiguracaoCacheAbrangenciaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((ConfiguracaoCacheAbrangenciaDto)null);

            //Act
            await handler.Handle(Query(), CancellationToken.None);

            //Assert
            VerificarRepositorioChamado(Times.Once());
            Assert.Empty(chavesLidas);
        }

        [Fact]
        public async Task Deve_Nao_Usar_Cache_Quando_Houver_Filtro_De_Texto()
        {
            //Arrange
            CacheLigado();

            //Act
            await handler.Handle(Query(filtro: "MARINA"), CancellationToken.None);

            //Assert
            VerificarRepositorioChamado(Times.Once());
            Assert.Empty(chavesLidas);
        }

        [Fact]
        public async Task Deve_Nao_Usar_Cache_Quando_Houver_AnosTurma()
        {
            //Arrange
            CacheLigado();

            //Act
            await handler.Handle(Query(anosTurma: new[] { "1", "2" }), CancellationToken.None);

            //Assert
            VerificarRepositorioChamado(Times.Once());
            Assert.Empty(chavesLidas);
        }

        [Fact]
        public async Task Deve_Retornar_Do_Cache_Sem_Chamar_O_Banco_Quando_Houver_Hit()
        {
            //Arrange
            CacheLigado();
            repositorioCache.Setup(c => c.ObterAsync(It.IsAny<string>(), false))
                .ReturnsAsync(JsonConvert.SerializeObject(UES_BANCO));

            //Act
            var retorno = await handler.Handle(Query(), CancellationToken.None);

            //Assert
            VerificarRepositorioChamado(Times.Never());
            Assert.Equal("EMEF MARINA COLASANTI", retorno.Single().Nome);
        }

        [Fact]
        public async Task Deve_Buscar_No_Banco_E_Gravar_No_Cache_Quando_Houver_Miss()
        {
            //Arrange
            CacheLigado();

            //Act
            await handler.Handle(Query(), CancellationToken.None);

            //Assert
            VerificarRepositorioChamado(Times.Once());
            Assert.Equal(new[] { 5 }, minutosGravados);
            Assert.Equal($"abrangencia-ues:{LOGIN}-{Perfis.PERFIL_ADMCOTIC}-{CODIGO_DRE}-{(int)Modalidade.Fundamental}-2026-False-0-29,31", chavesLidas.Single());
        }

        [Fact]
        public async Task Deve_Usar_Ttl_Do_Cj_Para_Perfil_Cj()
        {
            //Arrange
            CacheLigado(new ConfiguracaoCacheAbrangenciaDto { Minutos = 5, Cj = 1 });

            //Act
            await handler.Handle(Query(perfil: Perfis.PERFIL_CJ), CancellationToken.None);

            //Assert
            Assert.Equal(new[] { 1 }, minutosGravados);
        }

        [Fact]
        public async Task Deve_Gerar_Chaves_Diferentes_Para_Login_E_Ano_Diferentes()
        {
            //Arrange
            CacheLigado();

            //Act
            await handler.Handle(Query(), CancellationToken.None);
            await handler.Handle(Query(login: "7285388"), CancellationToken.None);
            await handler.Handle(Query(anoLetivo: 2025), CancellationToken.None);

            //Assert
            Assert.Equal(3, chavesLidas.Distinct().Count());
        }
    }
}
