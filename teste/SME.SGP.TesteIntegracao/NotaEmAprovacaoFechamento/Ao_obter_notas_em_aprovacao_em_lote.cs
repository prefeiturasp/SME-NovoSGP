using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.NotaEmAprovacaoFechamento
{
    /// <summary>
    /// Characterization (card 155456): a consulta em lote ObterNotasEmAprovacao deve devolver,
    /// para cada (aluno, disciplina, fechamento_turma), exatamente o mesmo valor que a consulta
    /// unitária ObterNotaEmAprovacao devolvia quando executada uma vez por nota.
    /// </summary>
    public class Ao_obter_notas_em_aprovacao_em_lote : TesteBase
    {
        private const long FECHAMENTO_TURMA_ID = 1;
        private const long DISCIPLINA_1 = 1;
        private const long DISCIPLINA_2 = 2;
        private const long DISCIPLINA_INEXISTENTE = 99;
        private const string ALUNO_1 = "1111";
        private const string ALUNO_2 = "2222";

        private const string TRECHO_SQL_NOTA_EM_APROVACAO = "wf_aprovacao_nota_fechamento";

        public Ao_obter_notas_em_aprovacao_em_lote(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);
            services.Replace(new ServiceDescriptor(typeof(IServicoTelemetria), typeof(ContadorQueriesTelemetriaFake), ServiceLifetime.Singleton));
        }

        [Fact]
        public async Task Deve_disparar_uma_unica_consulta_para_toda_a_turma()
        {
            await CarregarDados();

            var repositorio = ServiceProvider.GetService<IRepositorioNotasConceitosConsulta>();

            var chavesNota = new[]
            {
                (ALUNO_1, DISCIPLINA_1),
                (ALUNO_1, DISCIPLINA_2),
                (ALUNO_2, DISCIPLINA_1),
                (ALUNO_2, DISCIPLINA_2),
            };

            // Abordagem antiga: 1 consulta por nota (N idas ao banco)
            ContadorQueriesTelemetriaFake.Limpar();
            foreach (var (aluno, disciplina) in chavesNota)
                await repositorio.ObterNotaEmAprovacao(aluno, disciplina, FECHAMENTO_TURMA_ID);
            var consultasAbordagemAntiga = ContadorQueriesTelemetriaFake.ContarPorTrecho(TRECHO_SQL_NOTA_EM_APROVACAO);

            // Abordagem nova: 1 consulta em lote para toda a turma
            ContadorQueriesTelemetriaFake.Limpar();
            await repositorio.ObterNotasEmAprovacao(new[] { ALUNO_1, ALUNO_2 }, new[] { FECHAMENTO_TURMA_ID });
            var consultasAbordagemNova = ContadorQueriesTelemetriaFake.ContarPorTrecho(TRECHO_SQL_NOTA_EM_APROVACAO);

            consultasAbordagemAntiga.ShouldBe(chavesNota.Length); // N
            consultasAbordagemNova.ShouldBe(1);                   // 1
        }

        [Fact]
        public async Task Deve_retornar_os_mesmos_valores_da_consulta_unitaria()
        {
            await CarregarDados();

            var repositorio = ServiceProvider.GetService<IRepositorioNotasConceitosConsulta>();
            repositorio.ShouldNotBeNull();

            var emLote = (await repositorio.ObterNotasEmAprovacao(
                new[] { ALUNO_1, ALUNO_2 },
                new[] { FECHAMENTO_TURMA_ID })).ToList();

            // 1 linha por (aluno, disciplina, fechamento_turma) - inclusive a nota sem workflow
            emLote.Count.ShouldBe(4);

            ObterNotaEmLote(emLote, ALUNO_1, DISCIPLINA_1).ShouldBe(7.5);   // workflow ativo mais recente
            ObterNotaEmLote(emLote, ALUNO_1, DISCIPLINA_2).ShouldBe(4.0);
            ObterNotaEmLote(emLote, ALUNO_2, DISCIPLINA_1).ShouldBe(-1);    // sem workflow => -1 (NULLS FIRST no w.id desc)
            ObterNotaEmLote(emLote, ALUNO_2, DISCIPLINA_2).ShouldBe(8.0);   // DISTINCT ON pega o w.id maior

            // paridade com a consulta unitária, tupla a tupla
            foreach (var (aluno, disciplina) in new[]
                     {
                         (ALUNO_1, DISCIPLINA_1),
                         (ALUNO_1, DISCIPLINA_2),
                         (ALUNO_2, DISCIPLINA_1),
                         (ALUNO_2, DISCIPLINA_2),
                     })
            {
                var unitaria = await repositorio.ObterNotaEmAprovacao(aluno, disciplina, FECHAMENTO_TURMA_ID);
                ObterNotaEmLote(emLote, aluno, disciplina).ShouldBe(unitaria);
            }

            // workflow excluído é ignorado (aluno 1 / disciplina 1 tem um wf excluido com nota 1.0)
            ObterNotaEmLote(emLote, ALUNO_1, DISCIPLINA_1).ShouldNotBe(1.0);

            // disciplina sem nota: consulta unitária devolve 0.0 (default do QueryFirstOrDefault<double>);
            // no lote a tupla simplesmente não existe, e o helper VerificaNotaEmAprovacao aplica o mesmo 0.0
            emLote.Any(x => x.DisciplinaId == DISCIPLINA_INEXISTENTE).ShouldBeFalse();
            (await repositorio.ObterNotaEmAprovacao(ALUNO_1, DISCIPLINA_INEXISTENTE, FECHAMENTO_TURMA_ID)).ShouldBe(0.0);
        }

        [Fact]
        public async Task Deve_retornar_vazio_para_parametros_vazios()
        {
            await CarregarDados();

            var repositorio = ServiceProvider.GetService<IRepositorioNotasConceitosConsulta>();

            (await repositorio.ObterNotasEmAprovacao(Array.Empty<string>(), new[] { FECHAMENTO_TURMA_ID })).ShouldBeEmpty();
            (await repositorio.ObterNotasEmAprovacao(new[] { ALUNO_1 }, Array.Empty<long>())).ShouldBeEmpty();
        }

        private static double ObterNotaEmLote(System.Collections.Generic.IEnumerable<SME.SGP.Infra.NotaEmAprovacaoFechamentoDto> emLote, string aluno, long disciplina)
        {
            var registro = emLote.FirstOrDefault(x => x.CodigoAluno == aluno && x.DisciplinaId == disciplina && x.TurmaFechamentoId == FECHAMENTO_TURMA_ID);
            registro.ShouldNotBeNull($"esperava linha para aluno {aluno} / disciplina {disciplina}");
            return registro.Nota;
        }

        private async Task CarregarDados()
        {
            var anoLetivo = DateTimeExtension.HorarioBrasilia().Year;

            await InserirNaBase(new Dominio.TipoCalendario
            {
                Id = 1,
                AnoLetivo = anoLetivo,
                Nome = "Calendário Teste",
                Modalidade = ModalidadeTipoCalendario.FundamentalMedio,
                Periodo = Periodo.Anual,
                CriadoEm = DateTime.Now,
                CriadoPor = "",
                CriadoRF = ""
            });

            await InserirNaBase(new PeriodoEscolar
            {
                Id = 1,
                Bimestre = 1,
                PeriodoInicio = new DateTime(anoLetivo, 1, 1),
                PeriodoFim = new DateTime(anoLetivo, 3, 31),
                TipoCalendarioId = 1,
                CriadoEm = DateTime.Now,
                CriadoPor = "",
                CriadoRF = ""
            });

            await InserirNaBase(new Dre { Id = 1, Nome = "Dre Teste", CodigoDre = "11", Abreviacao = "DT" });
            await InserirNaBase(new Ue { Id = 1, Nome = "Ue Teste", DreId = 1, TipoEscola = TipoEscola.EMEF, CodigoUe = "22" });

            await InserirNaBase(new Dominio.Turma
            {
                Id = 1,
                Nome = "1A",
                CodigoTurma = "1234",
                Ano = "1",
                AnoLetivo = anoLetivo,
                TipoTurma = Dominio.Enumerados.TipoTurma.Regular,
                ModalidadeCodigo = Modalidade.Fundamental,
                UeId = 1
            });

            await InserirNaBase(new FechamentoTurma
            {
                Id = FECHAMENTO_TURMA_ID,
                PeriodoEscolarId = 1,
                TurmaId = 1,
                CriadoEm = new DateTime(anoLetivo, 1, 1),
                CriadoPor = "",
                CriadoRF = ""
            });

            await InserirNaBase(new FechamentoTurmaDisciplina
            {
                Id = 1,
                FechamentoTurmaId = FECHAMENTO_TURMA_ID,
                DisciplinaId = DISCIPLINA_1,
                CriadoEm = new DateTime(anoLetivo, 1, 1),
                CriadoPor = "",
                CriadoRF = ""
            });

            await InserirNaBase(new FechamentoAluno
            {
                Id = 1,
                FechamentoTurmaDisciplinaId = 1,
                AlunoCodigo = ALUNO_1,
                CriadoEm = new DateTime(anoLetivo, 1, 1),
                CriadoPor = "",
                CriadoRF = ""
            });

            await InserirNaBase(new FechamentoAluno
            {
                Id = 2,
                FechamentoTurmaDisciplinaId = 1,
                AlunoCodigo = ALUNO_2,
                CriadoEm = new DateTime(anoLetivo, 1, 1),
                CriadoPor = "",
                CriadoRF = ""
            });

            // fechamento_nota: (fa, disciplina)
            await InserirFechamentoNota(1, fechamentoAlunoId: 1, disciplinaId: DISCIPLINA_1);
            await InserirFechamentoNota(2, fechamentoAlunoId: 1, disciplinaId: DISCIPLINA_2);
            await InserirFechamentoNota(3, fechamentoAlunoId: 2, disciplinaId: DISCIPLINA_1); // sem workflow
            await InserirFechamentoNota(4, fechamentoAlunoId: 2, disciplinaId: DISCIPLINA_2);

            // wf_aprovacao_nota_fechamento
            await InserirWfNota(1, fechamentoNotaId: 1, nota: 7.5, excluido: false);
            await InserirWfNota(2, fechamentoNotaId: 1, nota: 1.0, excluido: true);   // ignorado
            await InserirWfNota(3, fechamentoNotaId: 2, nota: 4.0, excluido: false);
            await InserirWfNota(4, fechamentoNotaId: 4, nota: 3.0, excluido: false);  // mais antigo
            await InserirWfNota(5, fechamentoNotaId: 4, nota: 8.0, excluido: false);  // mais recente => vence
        }

        private async Task InserirFechamentoNota(long id, long fechamentoAlunoId, long disciplinaId)
        {
            await InserirNaBase(new FechamentoNota
            {
                Id = id,
                FechamentoAlunoId = fechamentoAlunoId,
                DisciplinaId = disciplinaId,
                Nota = 5,
                CriadoEm = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 1, 1),
                CriadoPor = "",
                CriadoRF = ""
            });
        }

        private async Task InserirWfNota(long id, long fechamentoNotaId, double nota, bool excluido)
        {
            await InserirNaBase(new WfAprovacaoNotaFechamento
            {
                Id = id,
                FechamentoNotaId = fechamentoNotaId,
                Nota = nota,
                Excluido = excluido,
                CriadoEm = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 1, 1),
                CriadoPor = "",
                CriadoRF = ""
            });
        }
    }
}
