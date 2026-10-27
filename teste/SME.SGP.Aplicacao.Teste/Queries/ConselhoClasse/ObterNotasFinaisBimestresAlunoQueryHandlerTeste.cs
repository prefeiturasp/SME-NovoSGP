using Moq;
using Newtonsoft.Json;
using SME.SGP.Infra;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dominio.Constantes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Queries.ConselhoClasse
{
    // Caracteriza o Handle atual. Não estabelece novas regras de precedência ou de turma.
    public class ObterNotasFinaisBimestresAlunoQueryHandlerTeste
    {
        private readonly Mock<IRepositorioConselhoClasseNotaConsulta> repositorio = new Mock<IRepositorioConselhoClasseNotaConsulta>();

        [Fact]
        public async Task Deve_reconsultar_fontes_apos_falha_transitoria_com_cache_real()
        {
            var fontes = new NotasFechamentoConselhoDto { Fechamento = new[] { Nota(89, 1, 7) },
                Conselho = new[] { Nota(89, 1, 9, id: 101) } };
            repositorio.SetupSequence(r => r.ObterFontesNotasFechamentoConselhoAsync("TESTE-A", 0, null, "ALUNO-A"))
                .ThrowsAsync(new InvalidOperationException("Falha transitória sintética"))
                .ReturnsAsync(fontes);
            var cache = new RepositorioCacheNotasTeste();
            var handler = new ObterNotasFinaisBimestresAlunoQueryHandler(repositorio.Object, cache);
            Comparar(fontes.Conselho, await handler.Handle(
                new ObterNotasFinaisBimestresAlunoQuery(new[] { "TESTE-A" }, "ALUNO-A"), CancellationToken.None));
            repositorio.Verify(r => r.ObterFontesNotasFechamentoConselhoAsync("TESTE-A", 0, null, "ALUNO-A"), Times.Exactly(2));
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public async Task Deve_compartilhar_fontes_com_sucesso_mesmo_se_gravacao_do_cache_falhar(bool falharGravacao)
        {
            var fontes = new NotasFechamentoConselhoDto { Fechamento = new[] { Nota(89, 1, 7) },
                Conselho = new[] { Nota(89, 1, 9, id: 101) } };
            repositorio.Setup(r => r.ObterFontesNotasFechamentoConselhoAsync("TESTE-A", 0, null, "ALUNO-A"))
                .ReturnsAsync(fontes);
            var handler = new ObterNotasFinaisBimestresAlunoQueryHandler(repositorio.Object,
                new RepositorioCacheNotasTeste(falharGravacao));
            Comparar(fontes.Conselho, await handler.Handle(
                new ObterNotasFinaisBimestresAlunoQuery(new[] { "TESTE-A" }, "ALUNO-A"), CancellationToken.None));
            repositorio.Verify(r => r.ObterFontesNotasFechamentoConselhoAsync("TESTE-A", 0, null, "ALUNO-A"), Times.Once);
        }

        [Fact]
        public async Task Deve_propagar_falha_persistente_sem_cachear_dados_parciais()
        {
            repositorio.Setup(r => r.ObterFontesNotasFechamentoConselhoAsync("TESTE-A", 0, null, "ALUNO-A"))
                .ThrowsAsync(new InvalidOperationException("Falha persistente sintética"));
            var cache = new RepositorioCacheNotasTeste();
            var handler = new ObterNotasFinaisBimestresAlunoQueryHandler(repositorio.Object, cache);
            await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(
                new ObterNotasFinaisBimestresAlunoQuery(new[] { "TESTE-A" }, "ALUNO-A"), CancellationToken.None));
            Assert.Empty(cache.Dados);
        }

        [Fact]
        public async Task Deve_preservar_fechamento_quando_nao_ha_conselho()
        {
            var fechamento = new[] { Nota(89, 1, 7), Nota(89, 2, null, 2), Nota(139, null, 0) };
            var retorno = await Executar(fechamento, Array.Empty<NotaConceitoBimestreComponenteDto>());
            Comparar(fechamento, retorno);
        }

        [Fact]
        public async Task Deve_preferir_conselho_preenchido_e_preservar_outros_componentes_e_periodos()
        {
            var fechamento = new[] { Nota(89, 1, 7), Nota(89, 2, 6), Nota(139, 1, 5) };
            var conselho = Nota(89, 1, 9, id: 101);
            var retorno = await Executar(fechamento, new[] { conselho });
            Comparar(new[] { conselho, fechamento[1], fechamento[2] }, retorno);
        }

        [Theory]
        [InlineData(0.0, null)]
        [InlineData(null, 3L)]
        public async Task Deve_considerar_zero_e_conceito_como_valores_preenchidos(double? valor, long? conceito)
        {
            var conselho = Nota(89, 1, valor, conceito, 101);
            Comparar(new[] { conselho }, await Executar(new[] { Nota(89, 1, 7) }, new[] { conselho }));
        }

        [Fact]
        public async Task Deve_usar_fechamento_quando_conselho_nao_tem_nota_nem_conceito()
        {
            var fechamento = Nota(89, 1, 7);
            Comparar(new[] { fechamento }, await Executar(new[] { fechamento }, new[] { Nota(89, 1, null, id: 101) }));
        }

        [Fact]
        public async Task Deve_preservar_nota_existente_somente_no_conselho()
        {
            var conselho = Nota(139, null, 8, id: 101);
            Comparar(new[] { conselho }, await Executar(Array.Empty<NotaConceitoBimestreComponenteDto>(), new[] { conselho }));
        }

        [Fact]
        public async Task Deve_preservar_registros_distintos_do_conselho_sem_deduplicar_valores()
        {
            var conselho = new[] { Nota(89, 1, 9, id: 101), Nota(89, 1, 9, id: 102) };
            Comparar(conselho, await Executar(new[] { Nota(89, 1, 7) }, conselho));
        }

        [Fact]
        public async Task Deve_caracterizar_comparacao_por_componente_e_bimestre_entre_turmas()
        {
            var fechamento = Nota(89, 1, 7);
            var conselho = Nota(89, 1, 9, id: 101);
            conselho.TurmaCodigo = "TESTE-B";
            Comparar(new[] { conselho }, await Executar(new[] { fechamento }, new[] { conselho }));
        }

        [Fact]
        public async Task Deve_tratar_periodo_final_nulo_separadamente_do_bimestre()
        {
            var fechamento = new[] { Nota(89, 1, 7), Nota(89, null, 6) };
            var conselho = Nota(89, null, 10, id: 101);
            Comparar(new[] { conselho, fechamento[0] }, await Executar(fechamento, new[] { conselho }));
        }

        private static void Comparar(IEnumerable<NotaConceitoBimestreComponenteDto> esperado,
            IEnumerable<NotaConceitoBimestreComponenteDto> atual)
        {
            // Verifica dados e multiplicidade, sem exigir identidade dos objetos ou ordem não contratada.
            Assert.Equal(esperado.Select(n => JsonConvert.SerializeObject(n)).OrderBy(n => n, StringComparer.Ordinal),
                atual.Select(n => JsonConvert.SerializeObject(n)).OrderBy(n => n, StringComparer.Ordinal));
        }

        private async Task<NotaConceitoBimestreComponenteDto[]> Executar(
            IEnumerable<NotaConceitoBimestreComponenteDto> fechamento, IEnumerable<NotaConceitoBimestreComponenteDto> conselho)
        {
            repositorio.Setup(r => r.ObterFontesNotasFechamentoConselhoAsync(It.IsAny<string>(), 0, null, "ALUNO-A"))
                .Returns((string turma, int bimestre, long? calendario, string aluno) => Task.FromResult(
                    new NotasFechamentoConselhoDto { Fechamento = fechamento.Where(n => n.TurmaCodigo == turma).ToArray(),
                        Conselho = conselho.Where(n => n.TurmaCodigo == turma).ToArray() }));
            var handler = new ObterNotasFinaisBimestresAlunoQueryHandler(repositorio.Object, CriarCache(new Dictionary<string, IEnumerable<NotaConceitoBimestreComponenteDto>>()).Object);
            return (await handler.Handle(new ObterNotasFinaisBimestresAlunoQuery(new[] { "TESTE-A", "TESTE-B" }, "ALUNO-A"), CancellationToken.None)).ToArray();
        }

        private static NotaConceitoBimestreComponenteDto Nota(long componente, int? bimestre, double? nota,
            long? conceito = null, long id = 0)
            => new NotaConceitoBimestreComponenteDto
            {
                AlunoCodigo = "ALUNO-A",
                TurmaCodigo = "TESTE-A",
                ComponenteCurricularCodigo = componente,
                Bimestre = bimestre,
                Nota = nota,
                ConceitoId = conceito,
                ConselhoClasseNotaId = id,
                ConselhoClasseId = id > 0 ? 1 : 0
            };

        [Theory]
        [InlineData(false, false)]
        [InlineData(true, false)]
        [InlineData(false, true)]
        [InlineData(true, true)]
        public async Task Deve_preservar_caches_completos_e_carregar_apenas_fontes_ausentes(bool fechamentoEmCache, bool conselhoEmCache)
        {
            var fechamento = new[] { Nota(89, 1, 7), Nota(139, 1, 5) };
            var conselho = new[] { Nota(89, 1, 9, id: 101), Nota(139, 1, null, id: 102) };
            var chaveF = string.Format(NomeChaveCache.NOTA_CONCEITO_FECHAMENTO_TURMA_ALUNO_BIMESTRES_E_FINAL, "TESTE-A", "ALUNO-A");
            var chaveC = string.Format(NomeChaveCache.NOTA_CONCEITO_CONSELHO_CLASSE_TURMA_BIMESTRE_ALUNO, "TESTE-A", 0, "ALUNO-A");
            var dados = new Dictionary<string, IEnumerable<NotaConceitoBimestreComponenteDto>>();
            if (fechamentoEmCache) dados[chaveF] = fechamento;
            if (conselhoEmCache) dados[chaveC] = conselho;
            // Valores diferentes na fonte confirmam que uma entrada quente não é sobrescrita.
            var fBanco = fechamentoEmCache ? new[] { Nota(89, 1, 1) } : fechamento;
            var cBanco = conselhoEmCache ? new[] { Nota(89, 1, 2, id: 103) } : conselho;
            repositorio.Setup(r => r.ObterFontesNotasFechamentoConselhoAsync("TESTE-A", 0, null, "ALUNO-A"))
                .ReturnsAsync(new NotasFechamentoConselhoDto { Fechamento = fBanco, Conselho = cBanco });
            repositorio.Setup(r => r.ObterNotasConceitosFechamentoPorTurmaCodigoEBimestreAsync("TESTE-A", 0, null, null, null, "ALUNO-A"))
                .ReturnsAsync(fBanco);
            repositorio.Setup(r => r.ObterNotasConceitosConselhoClassePorTurmaCodigoEBimestreAsync("TESTE-A", null, null, null, null, "ALUNO-A"))
                .ReturnsAsync(cBanco);
            var handler = new ObterNotasFinaisBimestresAlunoQueryHandler(repositorio.Object, CriarCache(dados).Object);
            var q = new ObterNotasFinaisBimestresAlunoQuery(new[] { "TESTE-A" }, "ALUNO-A");
            Comparar(new[] { conselho[0], fechamento[1] }, await handler.Handle(q, CancellationToken.None));
            Comparar(fechamento, dados[chaveF]);
            Comparar(conselho, dados[chaveC]);
            Comparar(new[] { conselho[0], fechamento[1] }, await handler.Handle(q, CancellationToken.None));
            repositorio.Verify(r => r.ObterFontesNotasFechamentoConselhoAsync("TESTE-A", 0, null, "ALUNO-A"),
                Times.Exactly(!fechamentoEmCache && !conselhoEmCache ? 1 : 0));
            repositorio.Verify(r => r.ObterNotasConceitosFechamentoPorTurmaCodigoEBimestreAsync("TESTE-A", 0, null, null, null, "ALUNO-A"),
                Times.Exactly(!fechamentoEmCache && conselhoEmCache ? 1 : 0));
            repositorio.Verify(r => r.ObterNotasConceitosConselhoClassePorTurmaCodigoEBimestreAsync("TESTE-A", null, null, null, null, "ALUNO-A"),
                Times.Exactly(fechamentoEmCache && !conselhoEmCache ? 1 : 0));
        }

        [Fact]
        public async Task Deve_preservar_multiplicidade_de_turmas_repetidas_e_lista_vazia()
        {
            var nota = Nota(89, 1, 7);
            repositorio.Setup(r => r.ObterFontesNotasFechamentoConselhoAsync("TESTE-A", 0, null, "ALUNO-A"))
                .ReturnsAsync(new NotasFechamentoConselhoDto { Fechamento = new[] { nota }, Conselho = Array.Empty<NotaConceitoBimestreComponenteDto>() });
            var handler = new ObterNotasFinaisBimestresAlunoQueryHandler(repositorio.Object, CriarCache(new Dictionary<string, IEnumerable<NotaConceitoBimestreComponenteDto>>()).Object);
            Comparar(new[] { nota, nota }, await handler.Handle(new ObterNotasFinaisBimestresAlunoQuery(new[] { "TESTE-A", "TESTE-A" }, "ALUNO-A"), CancellationToken.None));
            Assert.Empty(await handler.Handle(new ObterNotasFinaisBimestresAlunoQuery(Array.Empty<string>(), "ALUNO-A"), CancellationToken.None));
            repositorio.Verify(r => r.ObterFontesNotasFechamentoConselhoAsync("TESTE-A", 0, null, "ALUNO-A"), Times.Once);
        }

        private static Mock<IRepositorioCache> CriarCache(Dictionary<string, IEnumerable<NotaConceitoBimestreComponenteDto>> dados)
        {
            var cache = new Mock<IRepositorioCache>();
            cache.Setup(c => c.ObterAsync(It.IsAny<string>(), It.IsAny<Func<Task<IEnumerable<NotaConceitoBimestreComponenteDto>>>>(), It.IsAny<string>(), 720, false))
                .Returns(async (string chave, Func<Task<IEnumerable<NotaConceitoBimestreComponenteDto>>> buscar, string telemetria, int minutos, bool gzip) =>
                {
                    if (dados.TryGetValue(chave, out var valor)) return valor;
                    valor = (await buscar()).ToArray();
                    dados[chave] = valor;
                    return valor;
                });
            return cache;
        }

        [Fact]
        public async Task Deve_refletir_nova_nota_apos_invalidacao_da_chave_existente_do_conselho()
        {
            var dados = new Dictionary<string, IEnumerable<NotaConceitoBimestreComponenteDto>>();
            repositorio.Setup(r => r.ObterFontesNotasFechamentoConselhoAsync("TESTE-A", 0, null, "ALUNO-A"))
                .ReturnsAsync(new NotasFechamentoConselhoDto { Fechamento = new[] { Nota(89, 1, 7) },
                    Conselho = new[] { Nota(89, 1, 9, id: 101) } });
            var handler = new ObterNotasFinaisBimestresAlunoQueryHandler(repositorio.Object, CriarCache(dados).Object);
            var q = new ObterNotasFinaisBimestresAlunoQuery(new[] { "TESTE-A" }, "ALUNO-A");
            Comparar(new[] { Nota(89, 1, 9, id: 101) }, await handler.Handle(q, CancellationToken.None));
            dados.Remove(string.Format(NomeChaveCache.NOTA_CONCEITO_CONSELHO_CLASSE_TURMA_BIMESTRE_ALUNO, "TESTE-A", 0, "ALUNO-A"));
            repositorio.Setup(r => r.ObterNotasConceitosConselhoClassePorTurmaCodigoEBimestreAsync("TESTE-A", null, null, null, null, "ALUNO-A"))
                .ReturnsAsync(new[] { Nota(89, 1, 6, id: 101) });
            Comparar(new[] { Nota(89, 1, 6, id: 101) }, await handler.Handle(q, CancellationToken.None));
            repositorio.Verify(r => r.ObterFontesNotasFechamentoConselhoAsync("TESTE-A", 0, null, "ALUNO-A"), Times.Once);
        }
    }
}
