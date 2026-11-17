using MediatR;
using Moq;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Abrangencia
{
    public class ObterTurmasNaoHistoricasUseCaseCacheTeste
    {
        private const long USUARIO_ID = 123;

        private readonly Mock<IMediator> mediator;
        private readonly Mock<IRepositorioCacheAbrangencia> repositorioCache;
        private readonly ObterTurmasNaoHistoricasUseCase useCase;
        private readonly List<string> chavesLidas = new();
        private readonly Dictionary<string, string> gravados = new();
        private readonly List<int> minutosGravados = new();

        private Usuario usuario;

        public ObterTurmasNaoHistoricasUseCaseCacheTeste()
        {
            mediator = new Mock<IMediator>();
            repositorioCache = new Mock<IRepositorioCacheAbrangencia>();

            usuario = UsuarioUe(Perfis.PERFIL_PROFESSOR);

            mediator.Setup(m => m.Send(It.IsAny<ObterUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => usuario);

            mediator.Setup(m => m.Send(It.IsAny<ObterTurmasPorAnoEUsuarioIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => TurmasBanco());

            repositorioCache.Setup(c => c.ObterAsync(It.IsAny<string>(), false))
                .Callback<string, bool>((chave, _) => chavesLidas.Add(chave))
                .ReturnsAsync((string chave, bool _) => gravados.TryGetValue(chave, out var json) ? json : string.Empty);

            repositorioCache.Setup(c => c.SalvarAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<bool>()))
                .Callback<string, string, int, bool>((chave, json, minutos, _) =>
                {
                    if (json != "[]")
                        gravados[chave] = json;
                    minutosGravados.Add(minutos);
                })
                .Returns(Task.CompletedTask);

            useCase = new ObterTurmasNaoHistoricasUseCase(mediator.Object, repositorioCache.Object);
        }

        private static Usuario UsuarioUe(Guid perfil, TipoPerfil tipo = TipoPerfil.UE)
        {
            var usuarioUe = new Usuario { Id = USUARIO_ID, Login = "7769903", CodigoRf = "7769903" };
            usuarioUe.DefinirPerfis(new[] { new PrioridadePerfil { CodigoPerfil = perfil, Tipo = tipo } });
            usuarioUe.DefinirPerfilAtual(perfil);
            return usuarioUe;
        }

        private static IEnumerable<TurmaNaoHistoricaDto> TurmasBanco() => new[]
        {
            new TurmaNaoHistoricaDto
            {
                Id = 10, Codigo = "3019147", CodigoModalidade = (int)Modalidade.Fundamental, AnoTurma = "1",
                AnoLetivo = DateTime.Now.Year, Nome = "1A", NomeFiltro = "1A - MANHÃ"
            }
        };

        private void CacheLigado()
            => mediator.Setup(m => m.Send(It.IsAny<ObterConfiguracaoCacheAbrangenciaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ConfiguracaoCacheAbrangenciaDto { Minutos = 5, Cj = 1 });

        private void VerificarBancoChamado(Times vezes)
            => mediator.Verify(m => m.Send(It.IsAny<ObterTurmasPorAnoEUsuarioIdQuery>(), It.IsAny<CancellationToken>()), vezes);

        [Fact]
        public async Task Deve_Retornar_Vazio_Sem_Banco_E_Sem_Cache_Quando_Usuario_Nao_For_Somente_Ue()
        {
            CacheLigado();
            usuario = UsuarioUe(Perfis.PERFIL_ADMCOTIC, TipoPerfil.SME);

            var retorno = await useCase.Executar();

            Assert.Empty(retorno);
            VerificarBancoChamado(Times.Never());
            Assert.Empty(chavesLidas);
        }

        [Fact]
        public async Task Deve_Ir_Direto_Ao_Banco_Quando_Parametro_Estiver_Desligado()
        {
            mediator.Setup(m => m.Send(It.IsAny<ObterConfiguracaoCacheAbrangenciaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((ConfiguracaoCacheAbrangenciaDto)null);

            await useCase.Executar();
            await useCase.Executar();

            VerificarBancoChamado(Times.Exactly(2));
            Assert.Empty(chavesLidas);
        }

        [Fact]
        public async Task Deve_Usar_Cache_Na_Segunda_Chamada()
        {
            CacheLigado();

            var primeira = (await useCase.Executar()).ToList();
            var segunda = (await useCase.Executar()).ToList();

            VerificarBancoChamado(Times.Once());
            Assert.Equal(JsonConvert.SerializeObject(primeira), JsonConvert.SerializeObject(segunda));
            Assert.Equal(new[] { 5 }, minutosGravados);
        }

        [Fact]
        public async Task Deve_Usar_Id_Do_Usuario_Logado_Sem_Nova_Consulta()
        {
            CacheLigado();

            await useCase.Executar();

            mediator.Verify(m => m.Send(It.IsAny<ObterUsuarioLogadoIdQuery>(), It.IsAny<CancellationToken>()), Times.Never);
            mediator.Verify(m => m.Send(It.Is<ObterTurmasPorAnoEUsuarioIdQuery>(q => q.UsuarioId == USUARIO_ID), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Deve_Manter_NomeFiltro_Sem_Duplicar_Prefixo_Ao_Ler_Do_Cache()
        {
            CacheLigado();

            await useCase.Executar();
            var doCache = (await useCase.Executar()).Single();

            Assert.Equal(TurmasBanco().Single().NomeFiltro, doCache.NomeFiltro);
            Assert.Equal(TurmasBanco().Single().ModalidadeTurmaNome, doCache.ModalidadeTurmaNome);
        }

        [Fact]
        public async Task Deve_Usar_Chave_Por_Usuario_Perfil_E_Ano()
        {
            CacheLigado();

            await useCase.Executar();

            Assert.Equal($"abrangencia-turmas-vigentes:{USUARIO_ID}-{Perfis.PERFIL_PROFESSOR}-{DateTime.Now.Year}", chavesLidas.Single());
        }

        [Fact]
        public async Task Deve_Usar_Ttl_Do_Cj_Para_Perfil_Cj()
        {
            CacheLigado();
            usuario = UsuarioUe(Perfis.PERFIL_CJ);

            await useCase.Executar();

            Assert.Equal(new[] { 1 }, minutosGravados);
        }

        [Fact]
        public async Task Deve_Nao_Gravar_Cache_Quando_Nao_Houver_Turmas()
        {
            CacheLigado();
            mediator.Setup(m => m.Send(It.IsAny<ObterTurmasPorAnoEUsuarioIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Enumerable.Empty<TurmaNaoHistoricaDto>());

            var retorno = await useCase.Executar();

            Assert.Empty(retorno);
            Assert.Empty(gravados);
        }

        [Fact]
        public async Task Deve_Propagar_Erro_Sem_Gravar_Cache()
        {
            CacheLigado();
            mediator.Setup(m => m.Send(It.IsAny<ObterTurmasPorAnoEUsuarioIdQuery>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new TimeoutException());

            await Assert.ThrowsAsync<TimeoutException>(() => useCase.Executar());
            repositorioCache.Verify(c => c.SalvarAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<bool>()), Times.Never);
        }
    }
}
