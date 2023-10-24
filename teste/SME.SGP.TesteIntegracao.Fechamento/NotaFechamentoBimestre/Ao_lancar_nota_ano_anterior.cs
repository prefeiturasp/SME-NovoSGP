using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Integracoes.Respostas;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using SME.SGP.TesteIntegracao.ConselhoDeClasse.ServicosFakes;
using SME.SGP.TesteIntegracao.NotaFechamentoBimestre.ServicosFakes;
using SME.SGP.TesteIntegracao.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.NotaFechamentoBimestre
{
    public class Ao_lancar_nota_ano_anterior : NotaFechamentoBimestreTesteBase
    {
        private readonly DateTime DATA_18_04_ANO_ANTERIOR = new(DateTimeExtension.HorarioBrasilia().AddYears(-1).Year, 04, 18);

        public Ao_lancar_nota_ano_anterior(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery, bool>),
                typeof(ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQueryHandlerComPermissaoFake), ServiceLifetime.Scoped));

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterComponentesCurricularesEolPorCodigoTurmaLoginEPerfilQuery, IEnumerable<ComponenteCurricularEol>>),
                typeof(ObterComponentesCurricularesEolPorCodigoTurmaLoginEPerfilQueryHandlerFakePortugues), ServiceLifetime.Scoped));

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterComponentesCurricularesPorCodigoTurmaLoginEPerfilParaPlanejamentoQuery, IEnumerable<ComponenteCurricularEol>>),
                typeof(ObterComponentesCurricularesPorCodigoTurmaLoginEPerfilParaPlanejamentoQueryHandlerFakePortugues), ServiceLifetime.Scoped));

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterDadosTurmaEolPorCodigoQuery, DadosTurmaEolDto>),
                typeof(ObterDadosTurmaEolPorCodigoQueryHandlerFakeRegular), ServiceLifetime.Scoped));

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunosAtivosPorTurmaCodigoQuery, IEnumerable<AlunoPorTurmaResposta>>),
                typeof(ObterAlunosAtivosPorTurmaCodigoQueryHandlerFakeValidarAlunosFrequencia), ServiceLifetime.Scoped));

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterTodosAlunosNaTurmaQuery, IEnumerable<AlunoPorTurmaResposta>>),
                typeof(ObterTodosAlunosNaTurmaQueryHandlerAnoAnteriorFake), ServiceLifetime.Scoped));

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunosPorTurmaEAnoLetivoQuery, IEnumerable<AlunoPorTurmaResposta>>),
                typeof(ObterAlunosPorTurmaEAnoLetivoQueryHandlerFake), ServiceLifetime.Scoped));

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterMatriculasAlunoNaTurmaQuery, IEnumerable<AlunoPorTurmaResposta>>),
                typeof(ObterMatriculasAlunoNaTurmaQueryHandlerFakeAlunoCodigo1), ServiceLifetime.Scoped));

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterComponentesRegenciaPorAnoQuery, IEnumerable<DisciplinaResposta>>),
                typeof(NotaFechamento.ServicosFakes.ObterComponentesRegenciaPorAnoQueryHandlerFake), ServiceLifetime.Scoped));
        }

        [Fact(DisplayName = "Fechamento Bimestre - Deve lançar nota numérica em ano anterior como Professor Titular e ir para aprovação de WF")]
        public async Task Deve_lancar_nota_ano_anterior_professor_titular()
        {
            var filtroFechamentoNota = await ObterFiltroFechamentoNota(ObterPerfilProfessor(),
                ModalidadeTipoCalendario.FundamentalMedio,
                Modalidade.Fundamental,
                ANO_7,
                TipoFrequenciaAluno.PorDisciplina,
                COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString());

            await CriarDadosBase(filtroFechamentoNota);
            await CriarAvaliacaoBimestral(filtroFechamentoNota.ProfessorRf, filtroFechamentoNota.ComponenteCurricular);

            var notasLancadas = await LancarNotasAlunosAtivos(COMPONENTE_CURRICULAR_PORTUGUES_ID_138);
            await ExecutarTeste(notasLancadas);

            var fechamentosNotas = ObterTodos<FechamentoNota>();
            fechamentosNotas.ShouldNotBeNull();
            fechamentosNotas.Count.ShouldBe(4);

            var fechamentoNotaBimestre = await ObterFechamentoNotaBimestre(filtroFechamentoNota);

            fechamentoNotaBimestre.ShouldNotBeNull();

            fechamentoNotaBimestre.Bimestres.FirstOrDefault(c => c.Numero == BIMESTRE_3)!
                .Alunos.Any(c => c.NotasBimestre.Any(b => b.EmAprovacao)).ShouldBeTrue();

            var historicoNotas = ObterTodos<HistoricoNota>();
            historicoNotas.Count.ShouldBe(0);

            var historicoNotasNotaFechamentos = ObterTodos<HistoricoNotaFechamento>();
            historicoNotasNotaFechamentos.Count.ShouldBe(0);

            var wfAprovacaoNotaFechamento = ObterTodos<WfAprovacaoNotaFechamento>();
            wfAprovacaoNotaFechamento.Count.ShouldBe(4);

            var workflowAprovacao = ObterTodos<WorkflowAprovacao>();
            workflowAprovacao.Count.ShouldBe(0);
        }

        [Fact(DisplayName = "Fechamento Bimestre - Deve lançar nota numérica em ano anterior como CP sem gerar WF")]
        public async Task Deve_lancar_nota_ano_anterior_cp()
        {
            var filtroFechamentoNota = await ObterFiltroFechamentoNota(ObterPerfilCP(),
                ModalidadeTipoCalendario.FundamentalMedio,
                Modalidade.Fundamental,
                ANO_8,
                TipoFrequenciaAluno.PorDisciplina,
                COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString());

            await CriarDadosBase(filtroFechamentoNota);
            await CriarAvaliacaoBimestral(filtroFechamentoNota.ProfessorRf, filtroFechamentoNota.ComponenteCurricular);

            var notasLancadas = await LancarNotasAlunosAtivos(COMPONENTE_CURRICULAR_PORTUGUES_ID_138);
            await ExecutarTeste(notasLancadas);

            var fechamentosNotas = ObterTodos<FechamentoNota>();
            fechamentosNotas.ShouldNotBeNull();
            fechamentosNotas.Count.ShouldBe(4);

            var fechamentoNota = await ObterFechamentoNotaBimestre(filtroFechamentoNota);

            fechamentoNota.ShouldNotBeNull();

            fechamentoNota.Bimestres.FirstOrDefault(c => c.Numero == BIMESTRE_3)!
                .Alunos.Any(c => c.NotasBimestre.Any(b => !b.EmAprovacao)).ShouldBeTrue();

            var historicoNotas = ObterTodos<HistoricoNota>();
            historicoNotas.Count.ShouldBe(4);

            var historicoNotasNotaFechamentos = ObterTodos<HistoricoNotaFechamento>();
            historicoNotasNotaFechamentos.Count.ShouldBe(4);
        }

        [Fact(DisplayName = "Fechamento Bimestre - Deve lançar nota numérica em ano anterior como DIRETOR sem gerar WF")]
        public async Task Deve_lancar_nota_ano_anterior_diretor()
        {
            var filtroFechamentoNota = await ObterFiltroFechamentoNota(ObterPerfilDiretor(),
                ModalidadeTipoCalendario.FundamentalMedio,
                Modalidade.Fundamental,
                ANO_9,
                TipoFrequenciaAluno.PorDisciplina,
                COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString());

            await CriarDadosBase(filtroFechamentoNota);
            await CriarAvaliacaoBimestral(filtroFechamentoNota.ProfessorRf, filtroFechamentoNota.ComponenteCurricular);

            var notasLancadas = await LancarNotasAlunosAtivos(COMPONENTE_CURRICULAR_PORTUGUES_ID_138);
            await ExecutarTeste(notasLancadas);

            var fechamentosNotas = ObterTodos<FechamentoNota>();
            fechamentosNotas.ShouldNotBeNull();
            fechamentosNotas.Count.ShouldBe(4);

            var fechamentoNota = await ObterFechamentoNotaBimestre(filtroFechamentoNota);

            fechamentoNota.ShouldNotBeNull();

            fechamentoNota.Bimestres.FirstOrDefault(c => c.Numero == BIMESTRE_3)!
                .Alunos.Any(c => c.NotasBimestre.Any(b => !b.EmAprovacao)).ShouldBeTrue();

            var historicoNotas = ObterTodos<HistoricoNota>();
            historicoNotas.Count.ShouldBe(4);

            var historicoNotasNotaFechamentos = ObterTodos<HistoricoNotaFechamento>();
            historicoNotasNotaFechamentos.Count.ShouldBe(4);
        }

        [Fact(DisplayName = "Fechamento Bimestre - Deve lançar nota numérica em ano anterior como DIRETOR sem gerar WF para estudante inativo")]
        public async Task Deve_lancar_nota_se_estudante_ficou_inativo_durante_periodo_fechamento()
        {
            var filtroFechamentoNota = await ObterFiltroFechamentoNota(ObterPerfilDiretor(),
                ModalidadeTipoCalendario.FundamentalMedio,
                Modalidade.Fundamental,
                ANO_7,
                TipoFrequenciaAluno.PorDisciplina,
                COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(),
                true,
                false);

            filtroFechamentoNota.PeriodoEscolarValido = true;

            await CriarDadosBase(filtroFechamentoNota);
            await CriarAvaliacaoBimestral(filtroFechamentoNota.ProfessorRf, filtroFechamentoNota.ComponenteCurricular);

            var notasLancadas = await LancarNotasAlunosInativosDurantePeriodoFechamento(COMPONENTE_CURRICULAR_PORTUGUES_ID_138);
            await ExecutarTeste(notasLancadas);

            var fechamentosNotas = ObterTodos<FechamentoNota>();
            fechamentosNotas.ShouldNotBeNull();
            fechamentosNotas.Count.ShouldBe(7);

            var fechamentoNota = await ObterFechamentoNotaBimestre(filtroFechamentoNota);

            fechamentoNota.ShouldNotBeNull();

            fechamentoNota.Bimestres.FirstOrDefault(c => c.Numero == BIMESTRE_3)!
                .Alunos.Any(c => c.NotasBimestre.Any(b => !b.EmAprovacao)).ShouldBeTrue();

            var historicoNotas = ObterTodos<HistoricoNota>();
            historicoNotas.Count.ShouldBe(7);

            var historicoNotasNotaFechamentos = ObterTodos<HistoricoNotaFechamento>();
            historicoNotasNotaFechamentos.Count.ShouldBe(7);
        }

        [Fact(DisplayName = "Fechamento Bimestre - não deve lançar nota numérica e sim apresentar exceção para aluno inativo")]
        public async Task Deve_retornar_aviso_aluno_inativo()
        {
            var filtroFechamentoNota = await ObterFiltroFechamentoNota(ObterPerfilDiretor(),
                ModalidadeTipoCalendario.FundamentalMedio,
                Modalidade.Fundamental,
                ANO_7,
                TipoFrequenciaAluno.PorDisciplina,
                COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(),
                false,
                true);

            await CriarDadosBase(filtroFechamentoNota);
            await CriarAvaliacaoBimestral(filtroFechamentoNota.ProfessorRf, filtroFechamentoNota.ComponenteCurricular);

            var notasLancadas = await LancarNotasAlunosInativosForaPeriodoFechamento(COMPONENTE_CURRICULAR_PORTUGUES_ID_138);
            await ExecutarTeste(notasLancadas, true);
        }

        private async Task CriarAvaliacaoBimestral(string professorRf, string componenteCurricular)
        {
            await CriarTipoAvaliacao(TipoAvaliacaoCodigo.AvaliacaoBimestral, AVALIACAO_NOME_1);
            await CriarAtividadeAvaliativa(DATA_18_04_ANO_ANTERIOR, TIPO_AVALIACAO_CODIGO_1, AVALIACAO_NOME_1, false, false, professorRf);
            await CriarAtividadeAvaliativaDisciplina(ATIVIDADE_AVALIATIVA_1, componenteCurricular);
        }

        private async Task<NotasConceitosRetornoDto> ObterFechamentoNotaBimestre(FiltroFechamentoNotaDto filtroFechamentoNota)
        {
            var resultado = new NotasConceitosRetornoDto();

            var periodosEscolares = ObterTodos<PeriodoEscolar>();
            var periodoEscolarId = periodosEscolares.FirstOrDefault(c => c.Bimestre == BIMESTRE_3)!.Id;
            var dataInicioTicks = periodosEscolares.FirstOrDefault((c => c.Bimestre == BIMESTRE_3))!.PeriodoInicio.Ticks;
            var dataFimTicks = periodosEscolares.FirstOrDefault((c => c.Bimestre == BIMESTRE_3))!.PeriodoFim.Ticks;

            var listaNotasConceitos = await ObterListaNotasConceitos(BIMESTRE_3,
                COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                filtroFechamentoNota.Modalidade,
                0,
                TURMA_CODIGO_1,
                TURMA_ID_1,
                dataInicioTicks,
                dataFimTicks,
                periodoEscolarId);

            var useCase = ServiceProvider.GetService<IObterNotasParaAvaliacoesUseCase>();

            if (useCase.NaoEhNulo())
                resultado = await useCase.Executar(listaNotasConceitos);

            return await Task.FromResult(resultado);
        }

        private static async Task<List<FechamentoTurmaDisciplinaDto>> LancarNotasAlunosAtivos(long disciplinaId)
        {
            var alunosCodigos = new[] { CODIGO_ALUNO_1, CODIGO_ALUNO_2, CODIGO_ALUNO_3, CODIGO_ALUNO_4 };
            return await LancarNotasAlunos(alunosCodigos, disciplinaId);
        }

        private static async Task<List<FechamentoTurmaDisciplinaDto>> LancarNotasAlunosInativosDurantePeriodoFechamento(long disciplinaId)
        {
            var alunosCodigos = new[] { CODIGO_ALUNO_5, CODIGO_ALUNO_6, CODIGO_ALUNO_7, CODIGO_ALUNO_8, CODIGO_ALUNO_9, CODIGO_ALUNO_10, CODIGO_ALUNO_11 };
            return await LancarNotasAlunos(alunosCodigos, disciplinaId);
        }

        private static async Task<List<FechamentoTurmaDisciplinaDto>> LancarNotasAlunosInativosForaPeriodoFechamento(long disciplinaId)
        {
            var alunosCodigos = new[] { CODIGO_ALUNO_12, CODIGO_ALUNO_13 };
            return await LancarNotasAlunos(alunosCodigos, disciplinaId);
        }

        private static async Task<List<FechamentoTurmaDisciplinaDto>> LancarNotasAlunos(string[] alunosCodigos,
            long disciplinaId)
        {
            var fechamentosNotas = new List<FechamentoNotaDto>();

            foreach (var alunoCodigo in alunosCodigos)
            {
                Random randomNota = new();

                var nota = randomNota.Next(0, 10);

                var fechamentoNota = new FechamentoNotaDto()
                {
                    CodigoAluno = alunoCodigo,
                    DisciplinaId = disciplinaId,
                    Nota = nota,
                    ConceitoId = null,
                    SinteseId = null,
                    Anotacao = $"Anotação fechamento teste de integração do aluno {alunoCodigo}.",
                    CriadoEm = DateTimeExtension.HorarioBrasilia(),
                    CriadoRf = SISTEMA_CODIGO_RF,
                    CriadoPor = SISTEMA_NOME,
                };

                fechamentosNotas.Add(fechamentoNota);
            }

            var fechamentoTurma = new List<FechamentoTurmaDisciplinaDto>()
            {
                new()
                {
                    Bimestre = BIMESTRE_3,
                    DisciplinaId = disciplinaId,
                    Justificativa = "Teste",
                    TurmaId = TURMA_CODIGO_1 ,
                    NotaConceitoAlunos = fechamentosNotas
                }
            };

            return await Task.FromResult(fechamentoTurma);
        }

        private static async Task<ListaNotasConceitosDto> ObterListaNotasConceitos(int bimestre, long disciplinaCodigo,
            Modalidade modalidade, int semestre, string turmaCodigo, long turmaId, long periodoInicioTicks,
            long periodoFimTicks, long periodoEscolarId)
        {
            return await Task.FromResult(new ListaNotasConceitosDto
            {
                AnoLetivo = DateTimeExtension.HorarioBrasilia().AddYears(-1).Year,
                Bimestre = bimestre,
                DisciplinaCodigo = disciplinaCodigo,
                Modalidade = modalidade,
                Semestre = semestre,
                TurmaCodigo = turmaCodigo,
                TurmaHistorico = true,
                TurmaId = turmaId,
                PeriodoInicioTicks = periodoInicioTicks,
                PeriodoFimTicks = periodoFimTicks,
                PeriodoEscolarId = periodoEscolarId
            });
        }

        private static async Task<FiltroFechamentoNotaDto> ObterFiltroFechamentoNota(string perfil,
            ModalidadeTipoCalendario tipoCalendario, Modalidade modalidade, string anoTurma,
            TipoFrequenciaAluno tipoFrequenciaAluno, string componenteCurricular,
            bool criarPeriodoEscolar = true,
            bool criarPeriodoEscolarCustomizado = false)
        {
            return await Task.FromResult(new FiltroFechamentoNotaDto
            {
                Perfil = perfil,
                TipoCalendario = tipoCalendario,
                ConsiderarAnoAnterior = true,
                Modalidade = modalidade,
                AnoTurma = anoTurma,
                TipoFrequenciaAluno = tipoFrequenciaAluno,
                ProfessorRf = USUARIO_PROFESSOR_LOGIN_2222222,
                ComponenteCurricular = componenteCurricular,
                CriarPeriodoEscolar = criarPeriodoEscolar,
                CriarPeriodoEscolarCustomizado = criarPeriodoEscolarCustomizado
            });
        }
    }
}