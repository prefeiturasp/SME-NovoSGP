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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.NotaBimestreFechamento
{
    /// <summary>
    /// Characterization: a consulta em lote ObterNotasBimestrePorCodigosAlunosIdsFechamentos deve
    /// devolver, para cada (aluno, fechamento_turma_disciplina), exatamente o mesmo resultado que a
    /// consulta unitária ObterNotasBimestre devolvia quando executada uma vez por aluno dentro do
    /// foreach de RetornaListagemAlunosFechamentoBimestreEspecifico
    /// (ListarFechamentoTurmaBimestreUseCase.cs:216).
    /// </summary>
    public class Ao_obter_notas_bimestre_em_lote : TesteBase
    {
        private const long FECHAMENTO_TURMA_ID = 1;
        private const long FECHAMENTO_TURMA_DISCIPLINA_ID = 1;
        private const long DISCIPLINA_1 = 1;
        private const long DISCIPLINA_2 = 2;
        private const long CONCEITO_1 = 10;
        private const long SINTESE_1 = 1;
        private const string ALUNO_1 = "1111";
        private const string ALUNO_2 = "2222";
        private const string ALUNO_3 = "3333"; // sem nota lançada

        private const string TRECHO_SQL_FECHAMENTO_NOTA = "fechamento_nota";

        public Ao_obter_notas_bimestre_em_lote(CollectionFixture collectionFixture) : base(collectionFixture)
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

            var repositorio = ServiceProvider.GetService<IRepositorioFechamentoTurmaDisciplinaConsulta>();

            var alunos = new[] { ALUNO_1, ALUNO_2, ALUNO_3 };

            // Abordagem antiga: 1 consulta por aluno, dentro do foreach (N idas ao banco)
            ContadorQueriesTelemetriaFake.Limpar();
            foreach (var aluno in alunos)
                await repositorio.ObterNotasBimestre(aluno, FECHAMENTO_TURMA_DISCIPLINA_ID);
            var consultasAbordagemAntiga = ContadorQueriesTelemetriaFake.ContarPorTrecho(TRECHO_SQL_FECHAMENTO_NOTA);

            // Abordagem nova: 1 consulta em lote para toda a turma
            ContadorQueriesTelemetriaFake.Limpar();
            await repositorio.ObterNotasBimestrePorCodigosAlunosIdsFechamentos(alunos, new[] { FECHAMENTO_TURMA_DISCIPLINA_ID });
            var consultasAbordagemNova = ContadorQueriesTelemetriaFake.ContarPorTrecho(TRECHO_SQL_FECHAMENTO_NOTA);

            consultasAbordagemAntiga.ShouldBe(alunos.Length); // N
            consultasAbordagemNova.ShouldBe(1);                // 1
        }

        [Fact]
        public async Task Deve_retornar_os_mesmos_valores_da_consulta_unitaria()
        {
            await CarregarDados();

            var repositorio = ServiceProvider.GetService<IRepositorioFechamentoTurmaDisciplinaConsulta>();
            repositorio.ShouldNotBeNull();

            var alunos = new[] { ALUNO_1, ALUNO_2, ALUNO_3 };

            var emLote = (await repositorio.ObterNotasBimestrePorCodigosAlunosIdsFechamentos(alunos, new[] { FECHAMENTO_TURMA_DISCIPLINA_ID })).ToList();

            // ALUNO_1 tem 2 notas (2 disciplinas), ALUNO_2 tem 1, ALUNO_3 não tem nenhuma
            emLote.Count.ShouldBe(3);

            foreach (var aluno in alunos)
            {
                var unitaria = (await repositorio.ObterNotasBimestre(aluno, FECHAMENTO_TURMA_DISCIPLINA_ID)).ToList();
                var doLote = emLote.Where(x => x.CodigoAluno == aluno).ToList();

                doLote.Count.ShouldBe(unitaria.Count);

                foreach (var notaUnitaria in unitaria)
                {
                    var notaLote = doLote.FirstOrDefault(x => x.DisciplinaId == notaUnitaria.DisciplinaId);
                    notaLote.ShouldNotBeNull($"esperava nota da disciplina {notaUnitaria.DisciplinaId} para o aluno {aluno} no lote");

                    notaLote.CodigoAluno.ShouldBe(notaUnitaria.CodigoAluno);
                    notaLote.Nota.ShouldBe(notaUnitaria.Nota);
                    notaLote.ConceitoId.ShouldBe(notaUnitaria.ConceitoId);
                    notaLote.SinteseId.ShouldBe(notaUnitaria.SinteseId);
                    notaLote.CriadoPor.ShouldBe(notaUnitaria.CriadoPor);
                    notaLote.CriadoRf.ShouldBe(notaUnitaria.CriadoRf);
                    notaLote.AlteradoPor.ShouldBe(notaUnitaria.AlteradoPor);
                    notaLote.AlteradoRf.ShouldBe(notaUnitaria.AlteradoRf);
                }
            }

            // ALUNO_3 não tem nota: as duas abordagens devem devolver vazio
            (await repositorio.ObterNotasBimestre(ALUNO_3, FECHAMENTO_TURMA_DISCIPLINA_ID)).ShouldBeEmpty();
            emLote.Any(x => x.CodigoAluno == ALUNO_3).ShouldBeFalse();
        }

        [Fact]
        public async Task Deve_retornar_vazio_para_parametros_vazios()
        {
            await CarregarDados();

            var repositorio = ServiceProvider.GetService<IRepositorioFechamentoTurmaDisciplinaConsulta>();

            (await repositorio.ObterNotasBimestrePorCodigosAlunosIdsFechamentos(Array.Empty<string>(), new[] { FECHAMENTO_TURMA_DISCIPLINA_ID })).ShouldBeEmpty();
            (await repositorio.ObterNotasBimestrePorCodigosAlunosIdsFechamentos(new[] { ALUNO_1 }, Array.Empty<long>())).ShouldBeEmpty();
        }

        [Fact]
        public async Task Deve_respeitar_fechamentoTurmaDisciplinaId_zero_como_a_consulta_unitaria()
        {
            await CarregarDados();

            var repositorio = ServiceProvider.GetService<IRepositorioFechamentoTurmaDisciplinaConsulta>();

            // Reproduz o sentinela usado em ListarFechamentoTurmaBimestreUseCase.cs:216
            // (fechamentoTurma.NaoEhNulo() ? fechamentoTurma.Id : 0) quando o aluno não tem fechamento.
            var unitariaComZero = await repositorio.ObterNotasBimestre(ALUNO_1, 0);
            var loteComZero = await repositorio.ObterNotasBimestrePorCodigosAlunosIdsFechamentos(new[] { ALUNO_1 }, new[] { 0L });

            unitariaComZero.ShouldBeEmpty();
            loteComZero.ShouldBeEmpty();
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
                Id = FECHAMENTO_TURMA_DISCIPLINA_ID,
                FechamentoTurmaId = FECHAMENTO_TURMA_ID,
                DisciplinaId = DISCIPLINA_1,
                CriadoEm = new DateTime(anoLetivo, 1, 1),
                CriadoPor = "",
                CriadoRF = ""
            });

            await InserirNaBase(new Sintese
            {
                Id = SINTESE_1,
                Valor = "Frequente",
                Descricao = "Frequente",
                Ativo = true,
                Aprovado = true,
                InicioVigencia = new DateTime(anoLetivo, 1, 1),
                FimVigencia = new DateTime(anoLetivo, 12, 31),
                CriadoEm = new DateTime(anoLetivo, 1, 1),
                CriadoPor = "",
                CriadoRF = ""
            });

            await InserirNaBase(new FechamentoAluno
            {
                Id = 1,
                FechamentoTurmaDisciplinaId = FECHAMENTO_TURMA_DISCIPLINA_ID,
                AlunoCodigo = ALUNO_1,
                CriadoEm = new DateTime(anoLetivo, 1, 1),
                CriadoPor = "",
                CriadoRF = ""
            });

            await InserirNaBase(new FechamentoAluno
            {
                Id = 2,
                FechamentoTurmaDisciplinaId = FECHAMENTO_TURMA_DISCIPLINA_ID,
                AlunoCodigo = ALUNO_2,
                CriadoEm = new DateTime(anoLetivo, 1, 1),
                CriadoPor = "",
                CriadoRF = ""
            });

            await InserirNaBase(new FechamentoAluno
            {
                Id = 3,
                FechamentoTurmaDisciplinaId = FECHAMENTO_TURMA_DISCIPLINA_ID,
                AlunoCodigo = ALUNO_3,
                CriadoEm = new DateTime(anoLetivo, 1, 1),
                CriadoPor = "",
                CriadoRF = ""
            });

            // ALUNO_1: duas notas — uma numérica (disciplina 1), uma por conceito (disciplina 2)
            await InserirNaBase(new FechamentoNota
            {
                Id = 1,
                FechamentoAlunoId = 1,
                DisciplinaId = DISCIPLINA_1,
                Nota = 7.5,
                CriadoEm = new DateTime(anoLetivo, 1, 1),
                CriadoPor = "sistema",
                CriadoRF = "0000000"
            });

            await InserirNaBase(new FechamentoNota
            {
                Id = 2,
                FechamentoAlunoId = 1,
                DisciplinaId = DISCIPLINA_2,
                ConceitoId = CONCEITO_1,
                CriadoEm = new DateTime(anoLetivo, 1, 1),
                CriadoPor = "sistema",
                CriadoRF = "0000000"
            });

            // ALUNO_2: uma nota com síntese preenchida
            await InserirNaBase(new FechamentoNota
            {
                Id = 3,
                FechamentoAlunoId = 2,
                DisciplinaId = DISCIPLINA_1,
                Nota = 5.0,
                SinteseId = SINTESE_1,
                CriadoEm = new DateTime(anoLetivo, 1, 1),
                CriadoPor = "sistema",
                CriadoRF = "0000000"
            });

            // ALUNO_3: nenhuma nota lançada (propositalmente sem InserirFechamentoNota)
        }
    }
}
