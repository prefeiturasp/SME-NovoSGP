using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
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
    /// <summary>
    /// Characterization + guarda de round-trips para o N+1 de notas em
    /// RetornaListagemAlunosFechamentoBimestreEspecifico (ListarFechamentoTurmaBimestreUseCase.cs:216),
    /// onde a nota do bimestre ainda é buscada aluno a aluno dentro do foreach via ObterNotasBimestre,
    /// em vez de usar ObterNotasBimestrePorCodigosAlunosIdsFechamentos (já existe, já usada pelo
    /// endpoint irmão fechamento/turma).
    ///
    /// Segue exatamente o mesmo padrão de dados de Ao_obter_frequencia_dos_alunos.cs.
    /// </summary>
    public class Ao_obter_notas_dos_alunos : NotasDosAlunosTesteBase
    {
        public Ao_obter_notas_dos_alunos(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunosPorTurmaEAnoLetivoQuery, IEnumerable<AlunoPorTurmaResposta>>),
                typeof(ObterAlunosPorTurmaEAnoLetivoQueryHandlerFakeValidarAlunos), ServiceLifetime.Scoped));

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterComponentesCurricularesEolPorCodigoTurmaLoginEPerfilQuery, IEnumerable<ComponenteCurricularEol>>),
                typeof(ObterComponentesCurricularesEolPorCodigoTurmaLoginEPerfilQueryHandlerFakePortugues), ServiceLifetime.Scoped));

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterComponentesCurricularesPorCodigoTurmaLoginEPerfilParaPlanejamentoQuery, IEnumerable<ComponenteCurricularEol>>),
                typeof(ObterComponentesCurricularesPorCodigoTurmaLoginEPerfilParaPlanejamentoQueryHandlerFakePortugues), ServiceLifetime.Scoped));

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterDadosTurmaEolPorCodigoQuery, DadosTurmaEolDto>),
                typeof(ObterDadosTurmaEolPorCodigoQueryHandlerFakeRegular), ServiceLifetime.Scoped));

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterTodosAlunosNaTurmaQuery, IEnumerable<AlunoPorTurmaResposta>>),
                typeof(ObterTodosAlunosNaTurmaQueryHandlerFake), ServiceLifetime.Scoped));

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterMatriculasAlunoNaTurmaQuery, IEnumerable<AlunoPorTurmaResposta>>),
                typeof(ObterMatriculasAlunoNaTurmaQueryHandlerFakeAlunoCodigo1), ServiceLifetime.Scoped));

            services.Replace(new ServiceDescriptor(typeof(IServicoTelemetria), typeof(ContadorQueriesTelemetriaFake), ServiceLifetime.Singleton));
        }

        [Fact]
        public async Task Deve_retornar_a_nota_correspondente_a_cada_aluno()
        {
            var periodoEscolar = await CriarDadosBaseNota();
            var conceitoSatisfatorio = ObterConceito(SATISFATORIO);

            await CriarFechamentoTurmaDisciplina(periodoEscolar);

            await CriarFechamentoNota(fechamentoAlunoId: 1, nota: 7.5, conceitoId: null);
            await CriarFechamentoNota(fechamentoAlunoId: 2, nota: null, conceitoId: conceitoSatisfatorio.Id);
            // ALUNO_3 (fechamentoAlunoId: 3) propositalmente sem nota lançada.

            var retorno = await ExecutarTeste();

            var notaAluno1 = ObterUnicaNota(retorno, CODIGO_ALUNO_1);
            notaAluno1.DisciplinaCodigo.ShouldBe(COMPONENTE_CURRICULAR_PORTUGUES_ID_138);
            notaAluno1.NotaConceito.ShouldBe(7.5);
            notaAluno1.EmAprovacao.ShouldBeFalse();

            var notaAluno2 = ObterUnicaNota(retorno, CODIGO_ALUNO_2);
            notaAluno2.NotaConceito.ShouldBe((double)conceitoSatisfatorio.Id);

            // Aluno sem FechamentoNota, não-regência, com FechamentoAluno existente: o código monta
            // um placeholder (ListarFechamentoTurmaBimestreUseCase.cs:294-307) e chama
            // VerificaNotaEmAprovacao SEM checar exigeAprovacao (diferente do ramo "com nota", que
            // checa em :350). Sem nenhum registro em notasEmAprovacao, VerificaNotaEmAprovacao cai
            // no default (nota = 0d, 0d >= 0), então grava NotaConceito = 0 e EmAprovacao = true —
            // comportamento atual, não o que "deveria" ser.
            var notaAluno3 = ObterUnicaNota(retorno, CODIGO_ALUNO_3);
            notaAluno3.DisciplinaCodigo.ShouldBe(COMPONENTE_CURRICULAR_PORTUGUES_ID_138);
            notaAluno3.NotaConceito.ShouldBe(0d);
            notaAluno3.EmAprovacao.ShouldBeTrue();
        }

        [Fact]
        public async Task Nao_deve_disparar_uma_consulta_de_fechamento_nota_por_aluno()
        {
            // ListarFechamentoTurmaBimestreUseCase.cs:216 buscava a nota do bimestre uma vez por
            // aluno dentro do foreach; agora usa ObterNotasBimestrePorCodigosAlunosIdsFechamentos
            // (já existente, já usada pelo endpoint irmão fechamento/turma), carregada uma única vez
            // antes do foreach.
            var periodoEscolar = await CriarDadosBaseNota();

            await CriarFechamentoTurmaDisciplina(periodoEscolar);

            await CriarFechamentoNota(fechamentoAlunoId: 1, nota: 7.5, conceitoId: null);
            await CriarFechamentoNota(fechamentoAlunoId: 2, nota: 5.0, conceitoId: null);

            ContadorQueriesTelemetriaFake.Limpar();
            await ExecutarTeste();
            var consultas = ContadorQueriesTelemetriaFake.ContarPorTrecho(TRECHO_SQL_LOTE_NOTAS_BIMESTRE);

            consultas.ShouldBe(1);
        }

        [Fact]
        public async Task Deve_retornar_todas_as_notas_de_um_aluno_com_mais_de_uma_disciplina_lancada()
        {
            // O foreach de RetornaListagemAlunosFechamentoBimestreEspecifico itera TODAS as linhas
            // devolvidas por ObterNotasBimestre para o aluno, sem filtrar por regência — então um
            // aluno com nota em 2 disciplinas no mesmo fechamento (cenário real de regência, mas
            // reproduzível aqui sem precisar da maquinaria de componentes EOL de regência) já
            // caracteriza o caso "mais de uma nota por aluno".
            var periodoEscolar = await CriarDadosBaseNota();

            await CriarFechamentoTurmaDisciplina(periodoEscolar);

            await CriarFechamentoNota(fechamentoAlunoId: 1, nota: 7.5, conceitoId: null, disciplinaId: COMPONENTE_CURRICULAR_PORTUGUES_ID_138);
            await CriarFechamentoNota(fechamentoAlunoId: 1, nota: 5.0, conceitoId: null, disciplinaId: COMPONENTE_CURRICULAR_ARTES_ID_139);

            var retorno = await ExecutarTeste();

            var aluno1 = ObterAlunoDoFechamento(retorno, CODIGO_ALUNO_1);
            aluno1.NotasConceitoBimestre.ShouldNotBeNull();
            aluno1.NotasConceitoBimestre.Count.ShouldBe(2);

            aluno1.NotasConceitoBimestre.FirstOrDefault(n => n.DisciplinaCodigo == COMPONENTE_CURRICULAR_PORTUGUES_ID_138)?.NotaConceito.ShouldBe(7.5);
            aluno1.NotasConceitoBimestre.FirstOrDefault(n => n.DisciplinaCodigo == COMPONENTE_CURRICULAR_ARTES_ID_139)?.NotaConceito.ShouldBe(5.0);
        }

        [Fact]
        public async Task Nao_deve_crescer_consultas_de_fechamento_nota_por_quantidade_de_notas()
        {
            // Aqui o aluno tem 2 notas (2 disciplinas) no mesmo fechamento — a consulta em lote deve
            // continuar sendo UMA só, não uma por nota.
            var periodoEscolar = await CriarDadosBaseNota();

            await CriarFechamentoTurmaDisciplina(periodoEscolar);

            await CriarFechamentoNota(fechamentoAlunoId: 1, nota: 7.5, conceitoId: null, disciplinaId: COMPONENTE_CURRICULAR_PORTUGUES_ID_138);
            await CriarFechamentoNota(fechamentoAlunoId: 1, nota: 5.0, conceitoId: null, disciplinaId: COMPONENTE_CURRICULAR_ARTES_ID_139);

            ContadorQueriesTelemetriaFake.Limpar();
            await ExecutarTeste();
            var consultas = ContadorQueriesTelemetriaFake.ContarPorTrecho(TRECHO_SQL_LOTE_NOTAS_BIMESTRE);

            consultas.ShouldBe(1);
        }

        private Conceito ObterConceito(string valor)
            => ObterTodos<Conceito>().FirstOrDefault(c => c.Valor == valor);

        private async Task CriarFechamentoNota(long fechamentoAlunoId, double? nota, long? conceitoId, long disciplinaId = COMPONENTE_CURRICULAR_PORTUGUES_ID_138)
        {
            await InserirNaBase(new FechamentoNota
            {
                FechamentoAlunoId = fechamentoAlunoId,
                DisciplinaId = disciplinaId,
                Nota = nota,
                ConceitoId = conceitoId,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }

        private async Task<PeriodoEscolar> CriarDadosBaseNota()
        {
            var filtroNotaFechamento = ObterFiltroFechamentoNota();

            await InserirPeriodoEscolarCustomizado();
            await CriarDadosBase(filtroNotaFechamento);
            await CriarTipoAvaliacao(TipoAvaliacaoCodigo.AvaliacaoBimestral, AVALIACAO_NOME_1);
            await CrieConceitoValores();

            return ObterTodos<PeriodoEscolar>().FirstOrDefault(c => c.Bimestre == BIMESTRE_1);
        }

        private FiltroFechamentoNotaDto ObterFiltroFechamentoNota()
        {
            return new FiltroFechamentoNotaDto
            {
                Perfil = ObterPerfilProfessor(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                ConsiderarAnoAnterior = false,
                Modalidade = Modalidade.Fundamental,
                AnoTurma = ANO_7,
                TipoFrequenciaAluno = TipoFrequenciaAluno.PorDisciplina,
                ProfessorRf = USUARIO_PROFESSOR_LOGIN_2222222,
                ComponenteCurricular = COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(),
                CriarPeriodoEscolar = false,
                CriarPeriodoEscolarCustomizado = false
            };
        }

        private async Task InserirPeriodoEscolarCustomizado()
        {
            var dataReferencia = DateTimeExtension.HorarioBrasilia().Date;

            await CriarPeriodoEscolar(dataReferencia.AddDays(-45), dataReferencia.AddDays(+30), BIMESTRE_1);
            await CriarPeriodoEscolar(dataReferencia.AddDays(40), dataReferencia.AddDays(115), BIMESTRE_2);
            await CriarPeriodoEscolar(dataReferencia.AddDays(125), dataReferencia.AddDays(200), BIMESTRE_3);
            await CriarPeriodoEscolar(dataReferencia.AddDays(210), dataReferencia.AddDays(285), BIMESTRE_4);
        }
    }
}
