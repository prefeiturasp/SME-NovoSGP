using MediatR;
using Moq;
using SME.SGP.Aplicacao.Queries.ComponentesCurriculares.ObterComponentesCurricularesPorAnosEModalidade;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Relatorios
{
    public class ObterComponentesCurricularesPorUeAnosModalidadeUseCaseCacheTeste
    {
        private const int ANO_LETIVO = 2026;
        private const string UE = "019262";

        private readonly Mock<IMediator> mediator;
        private readonly Mock<IRepositorioCacheAbrangencia> repositorioCache;
        private readonly ObterComponentesCurricularesPorUeAnosModalidadeUseCase useCase;
        private readonly List<string> chavesLidas = new();
        private readonly Dictionary<string, string> gravados = new();
        private readonly List<int> minutosGravados = new();

        private IEnumerable<long> turmasBanco = new long[] { 3019147, 3019152 };
        private IEnumerable<ComponenteCurricularEol> componentesEol = ComponentesEol();

        public ObterComponentesCurricularesPorUeAnosModalidadeUseCaseCacheTeste()
        {
            mediator = new Mock<IMediator>();
            repositorioCache = new Mock<IRepositorioCacheAbrangencia>();

            mediator.Setup(m => m.Send(It.IsAny<ObterTurmasPorUeAnosModalidadeQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => turmasBanco);

            mediator.Setup(m => m.Send(It.IsAny<ObterComponentesCurricularesEOLPorTurmasCodigoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => componentesEol);

            mediator.Setup(m => m.Send(It.IsAny<ObterComponentesCurricularesPorAnosEModalidadeQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => componentesEol);

            mediator.Setup(m => m.Send(It.IsAny<ObterComponentesCurricularesSimplesPorIdsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new[]
                {
                    new ComponenteCurricularSimplesDto { Id = 139, Descricao = "LINGUA PORTUGUESA" },
                    new ComponenteCurricularSimplesDto { Id = 1213, Descricao = "MATEMATICA" }
                });

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

            useCase = new ObterComponentesCurricularesPorUeAnosModalidadeUseCase(mediator.Object, repositorioCache.Object);
        }

        private static IEnumerable<ComponenteCurricularEol> ComponentesEol() => new[]
        {
            new ComponenteCurricularEol { Codigo = 1213, Descricao = "MAT", LancaNota = true, TurmaCodigo = "3019147" },
            new ComponenteCurricularEol { Codigo = 139, Descricao = "PORT", LancaNota = true, TurmaCodigo = "3019147" },
            new ComponenteCurricularEol { Codigo = 139, Descricao = "PORT", LancaNota = true, TurmaCodigo = "3019152" }
        };

        private void CacheLigado()
            => mediator.Setup(m => m.Send(It.IsAny<ObterConfiguracaoCacheAbrangenciaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ConfiguracaoCacheAbrangenciaDto { Minutos = 5, Cj = 1 });

        private void CacheDesligado()
            => mediator.Setup(m => m.Send(It.IsAny<ObterConfiguracaoCacheAbrangenciaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((ConfiguracaoCacheAbrangenciaDto)null);

        private void VerificarTurmasBuscadas(Times vezes)
            => mediator.Verify(m => m.Send(It.IsAny<ObterTurmasPorUeAnosModalidadeQuery>(), It.IsAny<CancellationToken>()), vezes);

        [Fact]
        public async Task Cache_Desligado_Busca_Sem_Usar_Cache()
        {
            CacheDesligado();

            var retorno = (await useCase.Executar(new[] { "1" }, ANO_LETIVO, UE, Modalidade.Fundamental)).ToList();

            Assert.Equal(new long[] { -99, 1213, 139 }, retorno.Select(c => c.Codigo));
            VerificarTurmasBuscadas(Times.Once());
            repositorioCache.Verify(c => c.ObterAsync(It.IsAny<string>(), It.IsAny<bool>()), Times.Never);
            repositorioCache.Verify(c => c.SalvarAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<bool>()), Times.Never);
        }

        [Fact]
        public async Task Miss_Busca_E_Grava_Com_Chave_E_Ttl_Do_Parametro()
        {
            CacheLigado();

            await useCase.Executar(new[] { "2", "1" }, ANO_LETIVO, UE, Modalidade.Fundamental);

            Assert.Equal("componentes-curriculares-relatorio:2026-019262-5-1,2", Assert.Single(gravados.Keys));
            Assert.Equal(5, Assert.Single(minutosGravados));
            VerificarTurmasBuscadas(Times.Once());
        }

        [Fact]
        public async Task Hit_Nao_Busca_De_Novo_E_Retorna_O_Mesmo_Conteudo()
        {
            CacheLigado();

            var primeira = (await useCase.Executar(new[] { "1" }, ANO_LETIVO, UE, Modalidade.Fundamental)).ToList();
            var segunda = (await useCase.Executar(new[] { "1" }, ANO_LETIVO, UE, Modalidade.Fundamental)).ToList();

            VerificarTurmasBuscadas(Times.Once());
            Assert.Equal(primeira.Select(c => (c.Codigo, c.Descricao, c.LancaNota, c.TurmaCodigo)),
                         segunda.Select(c => (c.Codigo, c.Descricao, c.LancaNota, c.TurmaCodigo)));
            Assert.Equal("Todos", segunda[0].Descricao);
            Assert.Equal("MATEMATICA", segunda.Single(c => c.Codigo == 1213).Descricao);
        }

        [Fact]
        public async Task Ordem_E_Repeticao_Dos_Anos_Nao_Mudam_A_Chave()
        {
            CacheLigado();

            await useCase.Executar(new[] { "3", "1", "2" }, ANO_LETIVO, UE, Modalidade.Fundamental);
            await useCase.Executar(new[] { "1", "2", "3", "1" }, ANO_LETIVO, UE, Modalidade.Fundamental);

            Assert.Single(chavesLidas.Distinct());
            VerificarTurmasBuscadas(Times.Once());
        }

        [Fact]
        public async Task Todos_Os_Anos_Gera_A_Mesma_Chave_Com_Ou_Sem_Outros_Anos()
        {
            CacheLigado();

            await useCase.Executar(new[] { "-99" }, ANO_LETIVO, UE, Modalidade.Fundamental);
            await useCase.Executar(new[] { "-99", "1", "2" }, ANO_LETIVO, UE, Modalidade.Fundamental);

            Assert.Equal("componentes-curriculares-relatorio:2026-019262-5-", Assert.Single(chavesLidas.Distinct()));
            VerificarTurmasBuscadas(Times.Once());
        }

        [Fact]
        public async Task Filtros_Diferentes_Geram_Chaves_Diferentes()
        {
            CacheLigado();

            await useCase.Executar(new[] { "1" }, ANO_LETIVO, UE, Modalidade.Fundamental);
            await useCase.Executar(new[] { "1" }, ANO_LETIVO, "092843", Modalidade.Fundamental);
            await useCase.Executar(new[] { "1" }, ANO_LETIVO, UE, Modalidade.Medio);
            await useCase.Executar(new[] { "1" }, 2025, UE, Modalidade.Fundamental);
            await useCase.Executar(new[] { "2" }, ANO_LETIVO, UE, Modalidade.Fundamental);

            Assert.Equal(5, chavesLidas.Distinct().Count());
            VerificarTurmasBuscadas(Times.Exactly(5));
        }

        [Fact]
        public async Task Todas_As_Ues_Usa_Chave_Com_Ue_Vazia_E_Busca_Por_Anos_E_Modalidade()
        {
            CacheLigado();

            await useCase.Executar(new[] { "-99" }, ANO_LETIVO, "-99", Modalidade.Fundamental);

            Assert.Equal("componentes-curriculares-relatorio:2026--5-", Assert.Single(gravados.Keys));
            mediator.Verify(m => m.Send(It.IsAny<ObterComponentesCurricularesPorAnosEModalidadeQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            mediator.Verify(m => m.Send(It.IsAny<ObterComponentesCurricularesEOLPorTurmasCodigoQuery>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Sem_Turmas_Lanca_Excecao_E_Nao_Grava()
        {
            CacheLigado();
            turmasBanco = Enumerable.Empty<long>();

            await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(new[] { "1" }, ANO_LETIVO, UE, Modalidade.Fundamental));

            repositorioCache.Verify(c => c.SalvarAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<bool>()), Times.Never);
        }

        [Fact]
        public async Task Retorno_Vazio_Nao_Fica_No_Cache()
        {
            CacheLigado();
            componentesEol = Enumerable.Empty<ComponenteCurricularEol>();

            await useCase.Executar(new[] { "1" }, ANO_LETIVO, UE, Modalidade.Fundamental);
            await useCase.Executar(new[] { "1" }, ANO_LETIVO, UE, Modalidade.Fundamental);

            Assert.Empty(gravados);
            VerificarTurmasBuscadas(Times.Exactly(2));
        }
    }
}
