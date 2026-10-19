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
    /// Characterization: prova que a nota em aprovação (wf_aprovacao_nota_fechamento) continua
    /// funcionando corretamente quando ObterNotasBimestre é trocada pela versão em lote em
    /// RetornaListagemAlunosFechamentoBimestreEspecifico. ExigeAprovacaoDeNotaQuery só retorna
    /// true para turma de ano anterior com perfil não-gestor — por isso reaproveita o mesmo fake
    /// de alunos de ano anterior usado em Ao_lancar_nota_ano_anterior.cs.
    /// </summary>
    public class Ao_obter_notas_dos_alunos_com_aprovacao_pendente : NotasDosAlunosTesteBase
    {
        public Ao_obter_notas_dos_alunos_com_aprovacao_pendente(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            // Turma de ano anterior: datas de matrícula/situação relativas a HorarioBrasilia().AddYears(-1),
            // compatíveis com o período escolar de ano anterior criado por CriarDadosBase.
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunosPorTurmaEAnoLetivoQuery, IEnumerable<AlunoPorTurmaResposta>>),
                typeof(ObterAlunosPorTurmaEAnoLetivoQueryHandlerFakeValidarAlunosAnoAnterior), ServiceLifetime.Scoped));

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterComponentesCurricularesEolPorCodigoTurmaLoginEPerfilQuery, IEnumerable<ComponenteCurricularEol>>),
                typeof(ObterComponentesCurricularesEolPorCodigoTurmaLoginEPerfilQueryHandlerFakePortugues), ServiceLifetime.Scoped));

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterComponentesCurricularesPorCodigoTurmaLoginEPerfilParaPlanejamentoQuery, IEnumerable<ComponenteCurricularEol>>),
                typeof(ObterComponentesCurricularesPorCodigoTurmaLoginEPerfilParaPlanejamentoQueryHandlerFakePortugues), ServiceLifetime.Scoped));

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterDadosTurmaEolPorCodigoQuery, DadosTurmaEolDto>),
                typeof(ObterDadosTurmaEolPorCodigoQueryHandlerFakeRegular), ServiceLifetime.Scoped));

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterTodosAlunosNaTurmaQuery, IEnumerable<AlunoPorTurmaResposta>>),
                typeof(ObterTodosAlunosNaTurmaQueryHandlerAnoAnteriorFake), ServiceLifetime.Scoped));

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterMatriculasAlunoNaTurmaQuery, IEnumerable<AlunoPorTurmaResposta>>),
                typeof(ObterMatriculasAlunoNaTurmaQueryHandlerFakeAlunoCodigo1), ServiceLifetime.Scoped));

            services.Replace(new ServiceDescriptor(typeof(IServicoTelemetria), typeof(ContadorQueriesTelemetriaFake), ServiceLifetime.Singleton));
        }

        [Fact]
        public async Task Deve_refletir_nota_em_aprovacao_e_ignorar_workflow_excluido()
        {
            var periodoEscolar = await CriarDadosBaseNotaAnoAnterior();

            await CriarFechamentoTurmaDisciplina(periodoEscolar);

            // ALUNO_1: nota persistida 5.0, workflow pendente (não excluído) com nota 8.0 — deve
            // aparecer 8.0 e EmAprovacao = true.
            await CriarFechamentoNota(id: 1, fechamentoAlunoId: 1, nota: 5.0);
            await CriarWfAprovacaoNotaFechamento(id: 1, fechamentoNotaId: 1, nota: 8.0, excluido: false);

            // ALUNO_2: nota persistida 6.0, workflow existe mas está excluído — deve ignorar o
            // workflow, mostrar 6.0 e EmAprovacao = false.
            await CriarFechamentoNota(id: 2, fechamentoAlunoId: 2, nota: 6.0);
            await CriarWfAprovacaoNotaFechamento(id: 2, fechamentoNotaId: 2, nota: 9.0, excluido: true);

            // ALUNO_3: nota persistida 7.0, sem nenhum workflow — deve mostrar 7.0 e
            // EmAprovacao = false.
            await CriarFechamentoNota(id: 3, fechamentoAlunoId: 3, nota: 7.0);

            var retorno = await ExecutarTeste(BIMESTRE_3);

            var notaAluno1 = ObterUnicaNota(retorno, CODIGO_ALUNO_1);
            notaAluno1.NotaConceito.ShouldBe(8.0);
            notaAluno1.EmAprovacao.ShouldBeTrue();

            var notaAluno2 = ObterUnicaNota(retorno, CODIGO_ALUNO_2);
            notaAluno2.NotaConceito.ShouldBe(6.0);
            notaAluno2.EmAprovacao.ShouldBeFalse();

            var notaAluno3 = ObterUnicaNota(retorno, CODIGO_ALUNO_3);
            notaAluno3.NotaConceito.ShouldBe(7.0);
            notaAluno3.EmAprovacao.ShouldBeFalse();
        }

        [Fact]
        public async Task Deve_manter_uma_unica_consulta_de_fechamento_nota_com_aprovacao_ativa()
        {
            // Vermelho de propósito, mesma razão dos demais round-trip guards: aqui o cenário exige
            // aprovação ativa (exigeAprovacao = true), garantindo que a troca para o lote não
            // reintroduz uma consulta por aluno quando o workflow de aprovação também está em jogo.
            var periodoEscolar = await CriarDadosBaseNotaAnoAnterior();

            await CriarFechamentoTurmaDisciplina(periodoEscolar);

            await CriarFechamentoNota(id: 1, fechamentoAlunoId: 1, nota: 5.0);
            await CriarWfAprovacaoNotaFechamento(id: 1, fechamentoNotaId: 1, nota: 8.0, excluido: false);
            await CriarFechamentoNota(id: 2, fechamentoAlunoId: 2, nota: 6.0);

            ContadorQueriesTelemetriaFake.Limpar();
            await ExecutarTeste(BIMESTRE_3);
            var consultas = ContadorQueriesTelemetriaFake.ContarPorTrecho(TRECHO_SQL_FECHAMENTO_NOTA);

            consultas.ShouldBe(1);
        }

        private async Task CriarFechamentoNota(long id, long fechamentoAlunoId, double? nota)
        {
            await InserirNaBase(new FechamentoNota
            {
                Id = id,
                FechamentoAlunoId = fechamentoAlunoId,
                DisciplinaId = COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                Nota = nota,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }

        private async Task CriarWfAprovacaoNotaFechamento(long id, long fechamentoNotaId, double nota, bool excluido)
        {
            await InserirNaBase(new WfAprovacaoNotaFechamento
            {
                Id = id,
                FechamentoNotaId = fechamentoNotaId,
                Nota = nota,
                Excluido = excluido,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }

        private async Task<PeriodoEscolar> CriarDadosBaseNotaAnoAnterior()
        {
            var filtroNotaFechamento = new FiltroFechamentoNotaDto
            {
                Perfil = ObterPerfilProfessor(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                ConsiderarAnoAnterior = true,
                Modalidade = Modalidade.Fundamental,
                AnoTurma = ANO_7,
                TipoFrequenciaAluno = TipoFrequenciaAluno.PorDisciplina,
                ProfessorRf = USUARIO_PROFESSOR_LOGIN_2222222,
                ComponenteCurricular = COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(),
                CriarPeriodoEscolar = true,
                CriarPeriodoEscolarCustomizado = false
            };

            await CriarDadosBase(filtroNotaFechamento);
            await CriarTipoAvaliacao(TipoAvaliacaoCodigo.AvaliacaoBimestral, AVALIACAO_NOME_1);

            return ObterTodos<PeriodoEscolar>().FirstOrDefault(c => c.Bimestre == BIMESTRE_3);
        }
    }
}
