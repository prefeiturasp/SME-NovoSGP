using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra.Dtos;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.ConselhoDeClasse
{
    public class Ao_obter_totais_aulas_por_turma_componente_bimestre : TesteBase
    {
        private static readonly DateTime Inicio = new DateTime(2026, 3, 1);
        private static readonly DateTime Fim = new DateTime(2026, 3, 31);

        public Ao_obter_totais_aulas_por_turma_componente_bimestre(CollectionFixture collectionFixture)
            : base(collectionFixture)
        {
        }

        [Fact]
        public async Task Deve_somar_quantidades_sem_duplicar_aula_com_mais_de_um_registro_de_frequencia()
        {
            var calendarioId = await CriarCalendario();
            await CriarPeriodo(calendarioId, 1, Inicio, Fim);
            var primeiraAula = await CriarAula(calendarioId, new DateTime(2026, 3, 5), 2);
            var segundaAula = await CriarAula(calendarioId, new DateTime(2026, 3, 6), 3);
            await CriarRegistroFrequencia(primeiraAula);
            await CriarRegistroFrequencia(primeiraAula);
            await CriarRegistroFrequencia(segundaAula);

            var resultado = await Consultar(calendarioId);

            resultado.Single().AulasQuantidade.ShouldBe(5);
            resultado.Single().ComponenteCurricularCodigo.ShouldBe("138");
            resultado.Single().TurmaCodigo.ShouldBe("111");
            resultado.Single().Bimestre.ShouldBe(1);
        }

        [Fact]
        public async Task Deve_contar_aula_uma_vez_quando_periodos_identicos_estao_duplicados()
        {
            var calendarioId = await CriarCalendario();
            await CriarPeriodo(calendarioId, 1, Inicio, Fim);
            await CriarPeriodo(calendarioId, 1, Inicio, Fim);
            await CriarRegistroFrequencia(await CriarAula(calendarioId, Inicio, 3));

            var resultado = await Consultar(calendarioId);

            resultado.Single().AulasQuantidade.ShouldBe(3);
        }

        [Fact]
        public async Task Deve_incluir_primeiro_e_ultimo_dia_do_periodo_mesmo_com_horario()
        {
            var calendarioId = await CriarCalendario();
            await CriarPeriodo(calendarioId, 1, Inicio, Fim);
            foreach (var data in new[]
            {
                Inicio.AddTicks(-1),
                Inicio.AddHours(8),
                Fim.AddHours(23).AddMinutes(59),
                Fim.AddDays(1)
            })
                await CriarRegistroFrequencia(await CriarAula(calendarioId, data, 1));

            var resultado = await Consultar(calendarioId);

            resultado.Single().AulasQuantidade.ShouldBe(2);
        }

        [Fact]
        public async Task Deve_preservar_limites_quando_o_periodo_tem_horario()
        {
            var calendarioId = await CriarCalendario();
            await CriarPeriodo(calendarioId, 1, Inicio.AddHours(12), Inicio.AddDays(2).AddHours(12));
            for (var dia = 1; dia <= 4; dia++)
                await CriarRegistroFrequencia(await CriarAula(calendarioId, new DateTime(2026, 3, dia, 23, 0, 0), 1));

            var resultado = await Consultar(calendarioId);

            resultado.Single().AulasQuantidade.ShouldBe(2);
        }

        [Fact]
        public async Task Deve_ignorar_aula_sem_frequencia_e_registros_excluidos()
        {
            var calendarioId = await CriarCalendario();
            await CriarPeriodo(calendarioId, 1, Inicio, Fim);
            await CriarRegistroFrequencia(await CriarAula(calendarioId, Inicio, 2));
            await CriarAula(calendarioId, Inicio.AddDays(1), 3);
            await CriarRegistroFrequencia(await CriarAula(calendarioId, Inicio.AddDays(2), 4), excluido: true);
            await CriarRegistroFrequencia(await CriarAula(calendarioId, Inicio.AddDays(3), 5, excluido: true));

            var resultado = await Consultar(calendarioId);

            resultado.Single().AulasQuantidade.ShouldBe(2);
        }

        [Fact]
        public async Task Deve_separar_agrupamentos_por_turma_componente_bimestre_e_professor()
        {
            var calendarioId = await CriarCalendario();
            await CriarPeriodo(calendarioId, 1, Inicio, Fim);
            await CriarPeriodo(calendarioId, 2, Fim.AddDays(1), Fim.AddMonths(1));
            await CriarRegistroFrequencia(await CriarAula(calendarioId, Inicio, 1));
            await CriarRegistroFrequencia(await CriarAula(calendarioId, Inicio, 2, turma: "222"));
            await CriarRegistroFrequencia(await CriarAula(calendarioId, Inicio, 3, componente: "139"));
            await CriarRegistroFrequencia(await CriarAula(calendarioId, Inicio, 4, professor: "RF2"));
            await CriarRegistroFrequencia(await CriarAula(calendarioId, Fim.AddDays(1), 5));

            var resultado = await Consultar(calendarioId, new[] { "111", "222" }, Array.Empty<string>(), new[] { 1, 2 });

            resultado.Count.ShouldBe(5);
            resultado.Single(x => x.TurmaCodigo == "111" && x.ComponenteCurricularCodigo == "138" && x.Bimestre == 1 && x.Professor == "RF1").AulasQuantidade.ShouldBe(1);
            resultado.Single(x => x.TurmaCodigo == "222").AulasQuantidade.ShouldBe(2);
            resultado.Single(x => x.ComponenteCurricularCodigo == "139").AulasQuantidade.ShouldBe(3);
            resultado.Single(x => x.Professor == "RF2").AulasQuantidade.ShouldBe(4);
            resultado.Single(x => x.Bimestre == 2).AulasQuantidade.ShouldBe(5);

            var filtrado = await Consultar(calendarioId, new[] { "111" }, new[] { "138" }, new[] { 1 });
            filtrado.Count.ShouldBe(2);
            filtrado.Sum(x => x.AulasQuantidade).ShouldBe(5);
        }

        [Fact]
        public async Task Deve_filtrar_pelo_tipo_de_calendario()
        {
            var calendarioId = await CriarCalendario();
            var outroCalendarioId = await CriarCalendario();
            await CriarPeriodo(calendarioId, 1, Inicio, Fim);
            await CriarPeriodo(outroCalendarioId, 1, Inicio, Fim);
            await CriarRegistroFrequencia(await CriarAula(calendarioId, Inicio, 2));
            await CriarRegistroFrequencia(await CriarAula(outroCalendarioId, Inicio, 7));

            var resultado = await Consultar(calendarioId);

            resultado.Single().AulasQuantidade.ShouldBe(2);
        }

        [Fact]
        public async Task Deve_preservar_os_limites_atuais_das_datas_de_matricula_e_situacao()
        {
            var calendarioId = await CriarCalendario();
            await CriarPeriodo(calendarioId, 1, Inicio, Fim);
            for (var dia = 1; dia <= 4; dia++)
                await CriarRegistroFrequencia(await CriarAula(calendarioId, new DateTime(2026, 3, dia, 12, 0, 0), 1 << (dia - 1)));

            var matricula = new DateTime(2026, 3, 2);
            var situacao = new DateTime(2026, 3, 3);
            (await Consultar(calendarioId, matricula: matricula, situacao: situacao)).Single().AulasQuantidade.ShouldBe(6);
            (await Consultar(calendarioId, matricula: situacao)).Single().AulasQuantidade.ShouldBe(12);
            (await Consultar(calendarioId, situacao: situacao)).Single().AulasQuantidade.ShouldBe(3);
        }

        [Fact]
        public async Task Deve_preservar_limites_quando_matricula_e_situacao_tem_horario()
        {
            var calendarioId = await CriarCalendario();
            await CriarPeriodo(calendarioId, 1, Inicio, Fim);
            for (var dia = 1; dia <= 4; dia++)
                await CriarRegistroFrequencia(await CriarAula(calendarioId, new DateTime(2026, 3, dia, 12, 0, 0), 1 << (dia - 1)));

            var matricula = new DateTime(2026, 3, 2, 12, 0, 0);
            var situacao = new DateTime(2026, 3, 3, 12, 0, 0);
            (await Consultar(calendarioId, matricula: matricula, situacao: situacao)).Single().AulasQuantidade.ShouldBe(4);
            (await Consultar(calendarioId, matricula: matricula)).Single().AulasQuantidade.ShouldBe(12);
            (await Consultar(calendarioId, situacao: situacao)).Single().AulasQuantidade.ShouldBe(7);
        }

        [Fact]
        public async Task Deve_retornar_colecao_vazia_quando_nao_ha_aulas_com_frequencia()
        {
            var calendarioId = await CriarCalendario();
            await CriarPeriodo(calendarioId, 1, Inicio, Fim);
            await CriarAula(calendarioId, Inicio, 2);

            var resultado = await Consultar(calendarioId);

            resultado.ShouldBeEmpty();
        }

        private async Task<long> CriarCalendario()
        {
            return await InserirNaBaseAsync(new SME.SGP.Dominio.TipoCalendario
            {
                AnoLetivo = 2026,
                Nome = "Regressao US 155481",
                Periodo = Periodo.Anual,
                Modalidade = ModalidadeTipoCalendario.FundamentalMedio,
                Situacao = true,
                CriadoPor = "Teste",
                CriadoRF = "0"
            });
        }

        private async Task CriarPeriodo(long calendarioId, int bimestre, DateTime inicio, DateTime fim)
        {
            await InserirNaBaseAsync(new PeriodoEscolar
            {
                TipoCalendarioId = calendarioId,
                Bimestre = bimestre,
                PeriodoInicio = inicio,
                PeriodoFim = fim,
                CriadoPor = "Teste",
                CriadoRF = "0"
            });
        }

        private async Task<long> CriarAula(long calendarioId, DateTime data, int quantidade, string turma = "111", string componente = "138", string professor = "RF1", bool excluido = false)
        {
            return await InserirNaBaseAsync(new SME.SGP.Dominio.Aula
            {
                UeId = "1",
                TurmaId = turma,
                DisciplinaId = componente,
                TipoCalendarioId = calendarioId,
                ProfessorRf = professor,
                Quantidade = quantidade,
                DataAula = data,
                Excluido = excluido,
                CriadoPor = "Teste",
                CriadoRF = "0"
            });
        }

        private async Task CriarRegistroFrequencia(long aulaId, bool excluido = false)
        {
            await InserirNaBaseAsync(new RegistroFrequencia
            {
                AulaId = aulaId,
                Excluido = excluido,
                CriadoPor = "Teste",
                CriadoRF = "0"
            });
        }

        private async Task<List<TurmaComponenteQntAulasDto>> Consultar(long calendarioId, string[] turmas = null, string[] componentes = null, int[] bimestres = null, DateTime? matricula = null, DateTime? situacao = null)
        {
            var repositorio = ServiceProvider.GetRequiredService<IRepositorioFrequenciaAlunoDisciplinaPeriodoConsulta>();
            var resultado = await repositorio.ObterTotalAulasPorDisciplinaETurmaEBimestre(
                turmas ?? new[] { "111" }, componentes ?? new[] { "138" }, calendarioId,
                bimestres ?? new[] { 1 }, matricula, situacao);
            return resultado.ToList();
        }
    }
}
