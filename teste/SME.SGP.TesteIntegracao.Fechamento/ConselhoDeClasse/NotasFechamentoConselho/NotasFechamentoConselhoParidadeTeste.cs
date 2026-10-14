using Dapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dominio.Constantes;
using Newtonsoft.Json;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.ServicosFakes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace SME.SGP.TesteIntegracao.ConselhoDeClasse.NotasFechamentoConselho
{
    [Collection("NotasFechamentoConselho")]
    public class NotasFechamentoConselhoParidadeTeste
    {
        private readonly NotasFechamentoConselhoFixture fixture;
        private readonly ITestOutputHelper output;
        public NotasFechamentoConselhoParidadeTeste(NotasFechamentoConselhoFixture fixture, ITestOutputHelper output)
        {
            this.fixture = fixture;
            this.output = output;
        }

        [Fact]
        public async Task Deve_caracterizar_ultima_nota_e_filtros_diferentes_das_fontes_de_fechamento()
        {
            await fixture.PrepararAsync();
            var fechamentoConselho = await fixture.Conselho.ObterNotasConceitosFechamentoPorTurmaCodigoEBimestreAsync(
                "TESTE-A", 1, tipoCalendario: 10, alunoCodigo: "ALUNO-A");
            Comparar(new[] { Nota(89, 1, 7), Nota(139, 1, 2), Nota(138, 1, 6), Nota(7, 1, 3) }, fechamentoConselho);
            var fechamentoBimestral = await fixture.Fechamento.ObterNotasPorTurmaCodigoEBimestreAsync("TESTE-A", 1, tipoCalendario: 10);
            Comparar(new[] { Nota(89, 1, 7), Nota(139, 1, 0), Nota(7, 1, 3), Nota(89, 1, 5, aluno: "ALUNO-B") }, fechamentoBimestral);
        }

        [Fact]
        public async Task Deve_preservar_ids_distintos_nulos_conceitos_e_exclusao_da_nota_de_conselho()
        {
            await fixture.PrepararAsync();
            var retorno = await fixture.Conselho.ObterNotasConceitosConselhoClassePorTurmaCodigoEBimestreAsync(
                "TESTE-A", 1, tipoCalendario: 10, alunoCodigo: "ALUNO-A");
            Comparar(new[] { Nota(89,1,9,id:101,conselho:1), Nota(139,1,null,id:102,conselho:1),
                Nota(138,1,null,3,103,1), Nota(89,1,9,id:110,conselho:1) }, retorno);
        }

        [Fact]
        public async Task Deve_distinguir_bimestre_nulo_zero_e_calendarios()
        {
            await fixture.PrepararAsync();
            var final = await fixture.Conselho.ObterNotasConceitosConselhoClassePorTurmaCodigoEBimestreAsync(
                "TESTE-A", 0, tipoCalendario: 10, alunoCodigo: "ALUNO-A");
            Comparar(new[] { Nota(89, null, 10, id: 107, conselho: 3) }, final);
            var todos = await fixture.Conselho.ObterNotasConceitosConselhoClassePorTurmaCodigoEBimestreAsync(
                "TESTE-A", null, tipoCalendario: 10, alunoCodigo: "ALUNO-A");
            Comparar(new[] { Nota(89,1,9,id:101,conselho:1), Nota(139,1,null,id:102,conselho:1),
                Nota(138,1,null,3,103,1), Nota(89,1,9,id:110,conselho:1),
                Nota(89,2,null,3,106,2), Nota(89,null,10,id:107,conselho:3) }, todos);
            var outroCalendario = await fixture.Conselho.ObterNotasConceitosFechamentoPorTurmaCodigoEBimestreAsync(
                "TESTE-A", 1, tipoCalendario: 20, alunoCodigo: "ALUNO-A");
            Comparar(new[] { Nota(89, 1, 9) }, outroCalendario);
        }

        [Theory]
        [InlineData(-1, true)]
        [InlineData(0, true)]
        [InlineData(1, false)]
        public async Task Deve_preservar_inclusao_da_data_limite_da_matricula(int dias, bool inclui)
        {
            await fixture.PrepararAsync();
            var data = new DateTime(2025, 3, 31).AddDays(dias);
            var retorno = (await fixture.Conselho.ObterNotasConceitosFechamentoPorTurmaCodigoEBimestreAsync(
                "TESTE-A", 1, dataMatricula: data, tipoCalendario: 10, alunoCodigo: "ALUNO-A")).ToArray();
            if (inclui) Comparar(new[] { Nota(89, 1, 7), Nota(139, 1, 2), Nota(138, 1, 6), Nota(7, 1, 3) }, retorno);
            else Assert.Empty(retorno);
        }

        [Fact]
        public async Task Deve_preservar_periodo_final_sem_datas_e_o_filtro_de_situacao()
        {
            await fixture.PrepararAsync();
            var retorno = await fixture.Conselho.ObterNotasConceitosFechamentoPorTurmaCodigoEBimestreAsync(
                "TESTE-A", 0, new DateTime(2025, 3, 31), new DateTime(2025, 3, 31), 10, "ALUNO-A");
            Comparar(new[] { Nota(89, 1, 7), Nota(139, 1, 2), Nota(138, 1, 6), Nota(7, 1, 3), Nota(89, null, 6) }, retorno);
        }

        [Fact]
        public async Task Deve_compor_notas_finais_pelos_handlers_reais_em_multiplas_turmas()
        {
            await fixture.PrepararAsync();
            var cache = new RepositorioCacheFake();
            var services = new ServiceCollection();
            services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(ObterNotasFinaisBimestresAlunoQueryHandler).Assembly));
            services.AddSingleton<IRepositorioConselhoClasseNotaConsulta>(fixture.Conselho);
            services.AddSingleton<IRepositorioCache>(cache);
            using var provider = services.BuildServiceProvider();
            var retorno = await provider.GetRequiredService<IMediator>().Send(new ObterNotasFinaisBimestresAlunoQuery(
                new[] { "TESTE-A", "TESTE-B" }, "ALUNO-A", tipoCalendario: 10), CancellationToken.None);
            Comparar(new[] { Nota(89,1,9,id:101,conselho:1), Nota(138,1,null,3,103,1),
                Nota(89,1,9,id:110,conselho:1), Nota(89,2,null,3,106,2),
                Nota(89,null,10,id:107,conselho:3), Nota(89,1,0,id:108,conselho:4,turma:"TESTE-B"),
                Nota(139,1,2), Nota(7,1,3) }, retorno);
        }

        [Theory]
        [InlineData(0, null)]
        [InlineData(0, 10L)]
        [InlineData(0, 20L)]
        [InlineData(1, null)]
        [InlineData(1, 10L)]
        [InlineData(1, 20L)]
        [InlineData(2, null)]
        [InlineData(2, 10L)]
        [InlineData(2, 20L)]
        [InlineData(-1, null)]
        public async Task Deve_preservar_fontes_completas_na_consulta_unificada(int bimestre, long? calendario)
        {
            await fixture.PrepararAsync();
            foreach (var aluno in new[] { "ALUNO-A", "ALUNO-B", null, "" })
            {
                var fontes = await fixture.Conselho.ObterFontesNotasFechamentoConselhoAsync("TESTE-A", bimestre, calendario, aluno);
                var referencia = await FontesNotasFechamentoConselhoReferenciaAnterior.ObterAsync(fixture.Conexao, "TESTE-A", bimestre, calendario, aluno);
                Comparar(referencia.Fechamento, fontes.Fechamento);
                Comparar(referencia.Conselho, fontes.Conselho);
            }
        }

        [Theory]
        [InlineData(1)]
        [InlineData(30)]
        [InlineData(300)]
        public async Task Deve_preservar_todos_os_registros_na_massa_controlada_sem_conselho(int alunos)
        {
            await fixture.PrepararAsync();
            await SqlMapper.ExecuteAsync(fixture.Conexao, @"
                insert into fechamento_turma values (100,1,1);
                insert into fechamento_turma_disciplina values (100,100,89,false);
                insert into componente_curricular select 1000+c,true from generate_series(1,6) c;
                insert into fechamento_aluno select 1000+a,100,'VOLUME-'||a from generate_series(1,@alunos) a;
                insert into fechamento_nota
                    select 100000+a*12+c*2+v,1000+a,1000+c,null,case when v=0 then 4 else 7 end,false
                    from generate_series(1,@alunos) a cross join generate_series(1,6) c cross join generate_series(0,1) v;
            ", new { alunos });
            var retorno = (await fixture.Fechamento.ObterNotasPorTurmaCodigoEBimestreAsync("TESTE-A", 1, tipoCalendario: 10))
                .Where(n => n.AlunoCodigo.StartsWith("VOLUME-")).ToArray();
            var esperado = Enumerable.Range(1, alunos).SelectMany(a => Enumerable.Range(1, 6)
                .Select(c => Nota(1000 + c, 1, 7, aluno: $"VOLUME-{a}"))).ToArray();
            Comparar(esperado, retorno);
            output.WriteLine($"Massa sintética: {alunos} estudantes, {alunos * 12} notas persistidas, {retorno.Length} notas retornadas.");
        }

        [Theory]
        [InlineData(30, false, 0)]
        [InlineData(30, true, 0)]
        [InlineData(30, false, 2)]
        [InlineData(30, true, 2)]
        [InlineData(300, false, 0)]
        [InlineData(300, true, 0)]
        [InlineData(300, false, 2)]
        [InlineData(300, true, 2)]
        [InlineData(1000, false, 0)]
        [InlineData(1000, true, 0)]
        [InlineData(1000, false, 2)]
        [InlineData(1000, true, 2)]
        public async Task Deve_preservar_volume_das_fontes_unificadas_e_finais_contra_referencia_anterior(int alunos, bool comConselho, int bimestre)
        {
            await fixture.PrepararAsync();
            await SqlMapper.ExecuteAsync(fixture.Conexao, @"
                insert into periodo_escolar
                    select 100+p,10,p,timestamp '2026-02-01',timestamp '2026-12-31' from generate_series(1,4) p;
                insert into fechamento_turma select 100+p,1,case when p=5 then null else 100+p end from generate_series(1,5) p;
                insert into fechamento_turma_disciplina
                    select 1000+p*10+c,100+p,1000+c,false from generate_series(1,5) p cross join generate_series(1,6) c;
                insert into componente_curricular select 1000+c,true from generate_series(1,6) c;
                insert into fechamento_aluno
                    select 100000+p*100000+a*10+c,1000+p*10+c,'VOLUME-'||a from generate_series(1,5) p
                    cross join generate_series(1,@alunos) a cross join generate_series(1,6) c;
                insert into fechamento_nota
                    select 1000000+p*1000000+a*100+c*10+v,100000+p*100000+a*10+c,1000+c,
                           case when c=6 then 2 else null end,case when c=6 then null else 5+v end,false
                    from generate_series(1,5) p cross join generate_series(1,@alunos) a
                    cross join generate_series(1,6) c cross join generate_series(0,2) v;
                insert into conselho_classe select 100+p,100+p from generate_series(1,5) p where @comConselho;
                insert into conselho_classe_aluno
                    select 100000+p*10000+a,100+p,'VOLUME-'||a from generate_series(1,5) p
                    cross join generate_series(1,@alunos) a where @comConselho and a%2=0;
                insert into conselho_classe_nota
                    select 20000000+p*1000000+a*100+c*10+v,100000+p*10000+a,1000+c,
                           case when c=6 then 3 else null end,
                           case when c in (5,6) then null when c=4 then 0 else 9 end,false
                    from generate_series(1,5) p cross join generate_series(1,@alunos) a
                    cross join generate_series(1,6) c cross join generate_series(0,1) v
                    where @comConselho and a%2=0;
            ", new { alunos, comConselho });

            var referencia = await FontesNotasFechamentoConselhoReferenciaAnterior.ObterAsync(fixture.Conexao, "TESTE-A", bimestre, 10, null);
            var fontes = await fixture.Conselho.ObterFontesNotasFechamentoConselhoAsync("TESTE-A", bimestre, 10, null);
            Comparar(referencia.Fechamento, fontes.Fechamento);
            Comparar(referencia.Conselho, fontes.Conselho);

            // Conselho presente/ausente por estudante, em todos os quatro estados das fontes no cache.
            foreach (var aluno in new[] { "VOLUME-1", "VOLUME-2", $"VOLUME-{alunos}" }.Distinct())
            {
                var anterior = await FontesNotasFechamentoConselhoReferenciaAnterior.ObterAsync(fixture.Conexao, "TESTE-A", bimestre, 10, aluno);
                var conselhoValido = anterior.Conselho.Where(n => n.NotaConceito.HasValue).ToArray();
                var esperado = conselhoValido.Concat(anterior.Fechamento.Where(f =>
                    !conselhoValido.Any(c => c.ComponenteCurricularCodigo == f.ComponenteCurricularCodigo && c.Bimestre == f.Bimestre))).ToArray();
                foreach (var estado in Enumerable.Range(0, 4))
                {
                    var cache = new RepositorioCacheNotasTeste();
                    if ((estado & 1) != 0)
                        await cache.SalvarAsync(string.Format(NomeChaveCache.NOTA_CONCEITO_FECHAMENTO_TURMA_ALUNO_BIMESTRES_E_FINAL, "TESTE-A", aluno),
                            JsonConvert.SerializeObject(anterior.Fechamento));
                    if ((estado & 2) != 0)
                        await cache.SalvarAsync(string.Format(NomeChaveCache.NOTA_CONCEITO_CONSELHO_CLASSE_TURMA_BIMESTRE_ALUNO, "TESTE-A", bimestre, aluno),
                            JsonConvert.SerializeObject(anterior.Conselho));
                    var handler = new ObterNotasFinaisBimestresAlunoQueryHandler(fixture.Conselho, cache);
                    var query = new ObterNotasFinaisBimestresAlunoQuery(new[] { "TESTE-A" }, aluno, bimestre: bimestre, tipoCalendario: 10);
                    Comparar(esperado, await handler.Handle(query, CancellationToken.None));
                    Comparar(esperado, await handler.Handle(query, CancellationToken.None));
                }
            }
            output.WriteLine($"Massa controlada: {alunos} estudantes, 5 períodos, 6 componentes, {alunos * 90} notas de fechamento; conselho={comConselho}. Validação de paridade, não medição de desempenho.");
        }

        [Theory]
        [InlineData("nota")]
        [InlineData("conceito")]
        [InlineData("aluno")]
        [InlineData("turma")]
        [InlineData("componente")]
        [InlineData("bimestre")]
        [InlineData("id")]
        [InlineData("duplicacao")]
        [InlineData("ausencia")]
        public void Comparacao_de_paridade_deve_detectar_divergencias_e_nao_ocultar_duplicatas(string mudanca)
        {
            var esperado = new[] { Nota(89, 1, 7, id: 101, conselho: 1) };
            var alterado = Nota(89, 1, 7, id: 101, conselho: 1);
            var atual = new List<NotaConceitoBimestreComponenteDto> { alterado };
            switch (mudanca)
            {
                case "nota": alterado.Nota = 8; break;
                case "conceito": alterado.ConceitoId = 2; break;
                case "aluno": alterado.AlunoCodigo = "OUTRO"; break;
                case "turma": alterado.TurmaCodigo = "OUTRA"; break;
                case "componente": alterado.ComponenteCurricularCodigo = 139; break;
                case "bimestre": alterado.Bimestre = 2; break;
                case "id": alterado.ConselhoClasseNotaId = 102; break;
                case "duplicacao": atual.Add(alterado); break;
                case "ausencia": atual.Clear(); break;
            }
            Assert.False(Assinar(esperado).SequenceEqual(Assinar(atual)));
        }

        private void Comparar(IEnumerable<NotaConceitoBimestreComponenteDto> esperado, IEnumerable<NotaConceitoBimestreComponenteDto> atual)
        {
            // Ordena as assinaturas, mas preserva multiplicidade. A consulta não promete ORDER BY.
            Assert.Equal(Assinar(esperado), Assinar(atual));
        }

        private static string[] Assinar(IEnumerable<NotaConceitoBimestreComponenteDto> notas)
            => notas.Select(n => JsonConvert.SerializeObject(n)).OrderBy(n => n, StringComparer.Ordinal).ToArray();

        private static NotaConceitoBimestreComponenteDto Nota(long componente, int? bimestre, double? nota,
            long? conceito = null, long id = 0, long conselho = 0, string aluno = "ALUNO-A", string turma = "TESTE-A")
            => new NotaConceitoBimestreComponenteDto
            {
                ComponenteCurricularCodigo = componente,
                Bimestre = bimestre,
                Nota = nota,
                ConceitoId = conceito,
                ConselhoClasseNotaId = id,
                ConselhoClasseId = conselho,
                AlunoCodigo = aluno,
                TurmaCodigo = turma
            };
    }
}
