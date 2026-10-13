using Dapper;
using MediatR;
using Moq;
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

namespace SME.SGP.TesteIntegracao.ConselhoDeClasse.US155461
{
    [Collection("NotasUS155461")]
    public class NotasUS155461CaracterizacaoTeste
    {
        private readonly NotasUS155461Fixture fixture;
        private readonly ITestOutputHelper output;
        public NotasUS155461CaracterizacaoTeste(NotasUS155461Fixture fixture, ITestOutputHelper output)
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
            var fechamento = new ObterNotasConceitosFechamentoPorTurmasCodigosEBimestreQueryHandler(fixture.Conselho, cache);
            var conselho = new ObterNotasConceitosConselhoClassePorTurmasCodigosEBimestreQueryHandler(fixture.Conselho, cache);
            var mediator = new Mock<IMediator>();
            mediator.Setup(m => m.Send(It.IsAny<ObterNotasConceitosFechamentoPorTurmasCodigosEBimestreQuery>(), It.IsAny<CancellationToken>()))
                .Returns((ObterNotasConceitosFechamentoPorTurmasCodigosEBimestreQuery q, CancellationToken ct) => fechamento.Handle(q, ct));
            mediator.Setup(m => m.Send(It.IsAny<ObterNotasConceitosConselhoClassePorTurmasCodigosEBimestreQuery>(), It.IsAny<CancellationToken>()))
                .Returns((ObterNotasConceitosConselhoClassePorTurmasCodigosEBimestreQuery q, CancellationToken ct) => conselho.Handle(q, ct));
            var handler = new ObterNotasFinaisBimestresAlunoQueryHandler(mediator.Object);
            var retorno = await handler.Handle(new ObterNotasFinaisBimestresAlunoQuery(
                new[] { "TESTE-A", "TESTE-B" }, "ALUNO-A", tipoCalendario: 10), CancellationToken.None);
            Comparar(new[] { Nota(89,1,9,id:101,conselho:1), Nota(138,1,null,3,103,1),
                Nota(89,1,9,id:110,conselho:1), Nota(89,2,null,3,106,2),
                Nota(89,null,10,id:107,conselho:3), Nota(89,1,0,id:108,conselho:4,turma:"TESTE-B"),
                Nota(139,1,2), Nota(7,1,3) }, retorno);
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
