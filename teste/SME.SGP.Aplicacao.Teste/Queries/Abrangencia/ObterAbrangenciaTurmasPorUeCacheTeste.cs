using MediatR;
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
    public class ObterAbrangenciaTurmasPorUeCacheTeste
    {
        private const string LOGIN = "7924488";
        private const string CODIGO_UE = "019262";

        private readonly Mock<IMediator> mediator;
        private readonly Mock<IRepositorioAbrangencia> repositorioAbrangencia;
        private readonly Mock<IServicoUsuario> servicoUsuario;
        private readonly Mock<IRepositorioCacheAbrangencia> repositorioCache;
        private readonly ObterAbrangenciaTurmasPorUeModalidadePeriodoHistoricoAnoLetivoTiposQueryHandler handler;
        private readonly List<string> chavesLidas = new();
        private readonly Dictionary<string, string> gravados = new();
        private readonly List<int> minutosGravados = new();

        private Guid perfilAtual = Perfis.PERFIL_ADMCOTIC;

        public ObterAbrangenciaTurmasPorUeCacheTeste()
        {
            mediator = new Mock<IMediator>();
            repositorioAbrangencia = new Mock<IRepositorioAbrangencia>();
            servicoUsuario = new Mock<IServicoUsuario>();
            repositorioCache = new Mock<IRepositorioCacheAbrangencia>();

            servicoUsuario.Setup(s => s.ObterLoginAtual()).Returns(LOGIN);
            servicoUsuario.Setup(s => s.ObterPerfilAtual()).Returns(() => perfilAtual);

            mediator.Setup(m => m.Send(It.IsAny<ObterParametroTurmaFiltroPorAnoLetivoEModalidadeQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((string[])null);

            repositorioAbrangencia.Setup(r => r.ObterTurmasPorTipos(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Guid>(),
                    It.IsAny<FiltroModalidade>(), It.IsAny<int[]>(), It.IsAny<FiltroPeriodoLetivo>(), It.IsAny<string[]>()))
                .ReturnsAsync(() => TurmasBanco());

            mediator.Setup(m => m.Send(It.IsAny<ObterTurmasApiEolQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<TurmaApiEolDto> { new() { Codigo = 3019147 } });

            repositorioCache.Setup(c => c.ObterAsync(It.IsAny<string>(), false))
                .Callback<string, bool>((chave, _) => chavesLidas.Add(chave))
                .ReturnsAsync((string chave, bool _) => gravados.TryGetValue(chave, out var json) ? json : string.Empty);

            repositorioCache.Setup(c => c.SalvarAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<bool>()))
                .Callback<string, string, int, bool>((chave, json, minutos, _) =>
                {
                    gravados[chave] = json;
                    minutosGravados.Add(minutos);
                })
                .Returns(Task.CompletedTask);

            handler = new ObterAbrangenciaTurmasPorUeModalidadePeriodoHistoricoAnoLetivoTiposQueryHandler(
                repositorioAbrangencia.Object, servicoUsuario.Object, mediator.Object, repositorioCache.Object);
        }

        private static IEnumerable<AbrangenciaTurmaRetorno> TurmasBanco() => new[]
        {
            new AbrangenciaTurmaRetorno
            {
                Ano = "1", AnoLetivo = 2026, Codigo = "3019147", CodigoModalidade = (int)Modalidade.Fundamental,
                Nome = "1A", NomeFiltro = "1A - MANHÃ", Semestre = 0, Id = 10, TipoTurma = 1
            }
        };

        private void CacheLigado()
            => mediator.Setup(m => m.Send(It.IsAny<ObterConfiguracaoCacheAbrangenciaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ConfiguracaoCacheAbrangenciaDto { Minutos = 5, Cj = 1 });

        private static ObterAbrangenciaTurmasPorUeModalidadePeriodoHistoricoAnoLetivoTiposQuery Query(bool usarCache = true,
            int[] tipos = null, string[] anosTurma = null)
            => new(CODIGO_UE, new FiltroModalidade(Modalidade.Fundamental, anosTurma), new FiltroPeriodoLetivo(2026), tipos, true)
            {
                UsarCache = usarCache
            };

        private void VerificarBancoEEolChamados(Times vezes)
        {
            repositorioAbrangencia.Verify(r => r.ObterTurmasPorTipos(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Guid>(),
                It.IsAny<FiltroModalidade>(), It.IsAny<int[]>(), It.IsAny<FiltroPeriodoLetivo>(), It.IsAny<string[]>()), vezes);
            mediator.Verify(m => m.Send(It.IsAny<ObterTurmasApiEolQuery>(), It.IsAny<CancellationToken>()), vezes);
        }

        [Fact]
        public async Task Deve_Ir_Direto_Ao_Banco_Quando_UsarCache_For_False()
        {
            //Arrange
            CacheLigado();

            //Act
            await handler.Handle(Query(usarCache: false), CancellationToken.None);

            //Assert
            VerificarBancoEEolChamados(Times.Once());
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
            VerificarBancoEEolChamados(Times.Once());
            Assert.Empty(chavesLidas);
        }

        [Fact]
        public async Task Deve_Usar_Cache_Na_Segunda_Chamada_Sem_Ir_Ao_Banco_Nem_Ao_Eol()
        {
            //Arrange
            CacheLigado();

            //Act
            var primeira = (await handler.Handle(Query(), CancellationToken.None)).ToList();
            var segunda = (await handler.Handle(Query(), CancellationToken.None)).ToList();

            //Assert
            VerificarBancoEEolChamados(Times.Once());
            Assert.Equal(JsonConvert.SerializeObject(primeira), JsonConvert.SerializeObject(segunda));
            Assert.Equal(new[] { 5 }, minutosGravados);
        }

        [Fact]
        public async Task Deve_Manter_NomeFiltro_Sem_Duplicar_Prefixo_Ao_Ler_Do_Cache()
        {
            //Arrange
            CacheLigado();

            //Act
            await handler.Handle(Query(), CancellationToken.None);
            var doCache = (await handler.Handle(Query(), CancellationToken.None)).Single();

            //Assert
            Assert.Equal(TurmasBanco().Single().NomeFiltro, doCache.NomeFiltro);
            Assert.Equal(TurmasBanco().Single().ModalidadeTurmaNome, doCache.ModalidadeTurmaNome);
        }

        [Fact]
        public async Task Deve_Usar_Ttl_Do_Cj_Para_Perfil_Cj()
        {
            //Arrange
            CacheLigado();
            perfilAtual = Perfis.PERFIL_CJ;

            //Act
            await handler.Handle(Query(), CancellationToken.None);

            //Assert
            Assert.Equal(new[] { 1 }, minutosGravados);
        }

        [Fact]
        public async Task Deve_Nao_Gravar_Cache_Quando_Eol_Nao_Devolver_Turmas()
        {
            //Arrange
            CacheLigado();
            mediator.Setup(m => m.Send(It.IsAny<ObterTurmasApiEolQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<TurmaApiEolDto>());
            repositorioCache.Setup(c => c.SalvarAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<bool>()))
                .Callback<string, string, int, bool>((chave, json, _, _) =>
                {
                    if (json != "[]")
                        gravados[chave] = json;
                })
                .Returns(Task.CompletedTask);

            //Act
            var retorno = await handler.Handle(Query(), CancellationToken.None);

            //Assert
            Assert.Empty(retorno);
            Assert.Empty(gravados);
        }

        [Fact]
        public async Task Deve_Propagar_Erro_Sem_Gravar_Cache()
        {
            //Arrange
            CacheLigado();
            repositorioAbrangencia.Setup(r => r.ObterTurmasPorTipos(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Guid>(),
                    It.IsAny<FiltroModalidade>(), It.IsAny<int[]>(), It.IsAny<FiltroPeriodoLetivo>(), It.IsAny<string[]>()))
                .ThrowsAsync(new TimeoutException());

            //Act + Assert
            await Assert.ThrowsAsync<TimeoutException>(() => handler.Handle(Query(), CancellationToken.None));
            repositorioCache.Verify(c => c.SalvarAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<bool>()), Times.Never);
        }

        [Fact]
        public void Chave_Deve_Ter_Todas_As_Variaveis_Na_Ordem()
        {
            //Arrange
            var query = new ObterAbrangenciaTurmasPorUeModalidadePeriodoHistoricoAnoLetivoTiposQuery(CODIGO_UE,
                new FiltroModalidade(Modalidade.Fundamental, new[] { "2", "1" }), new FiltroPeriodoLetivo(2026, true, 1), new[] { 3, 1 }, false);

            //Act
            var chave = ObterAbrangenciaTurmasPorUeModalidadePeriodoHistoricoAnoLetivoTiposQueryHandler.MontarChaveCache(query, LOGIN, perfilAtual);

            //Assert
            Assert.Equal($"abrangencia-turmas-ue:{LOGIN}-{perfilAtual}-{CODIGO_UE}-{(int)Modalidade.Fundamental}-1,2-2026-True-1-1,3-False", chave);
        }

        [Fact]
        public void Chave_Deve_Diferenciar_AnosTurma_Nulo_De_Vazio()
        {
            //Act
            var chave = ObterAbrangenciaTurmasPorUeModalidadePeriodoHistoricoAnoLetivoTiposQueryHandler.MontarChaveCache;

            //Assert
            Assert.NotEqual(chave(Query(anosTurma: null), LOGIN, perfilAtual), chave(Query(anosTurma: Array.Empty<string>()), LOGIN, perfilAtual));
        }

        [Fact]
        public void Chave_Deve_Ignorar_Ordem_Dos_Tipos_E_Tratar_Vazio_Como_Null()
        {
            //Act
            var chave = ObterAbrangenciaTurmasPorUeModalidadePeriodoHistoricoAnoLetivoTiposQueryHandler.MontarChaveCache;

            //Assert
            Assert.Equal(chave(Query(tipos: new[] { 3, 1 }), LOGIN, perfilAtual), chave(Query(tipos: new[] { 1, 3 }), LOGIN, perfilAtual));
            Assert.Equal(chave(Query(tipos: Array.Empty<int>()), LOGIN, perfilAtual), chave(Query(tipos: null), LOGIN, perfilAtual));
            Assert.NotEqual(chave(Query(), LOGIN, perfilAtual), chave(Query(), "outro.login", perfilAtual));
        }
    }
}
