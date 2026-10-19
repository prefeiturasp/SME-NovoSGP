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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.NotaFechamentoBimestre
{
    /// <summary>
    /// O fake de alunos precisa ser registrado por classe de teste, por isso este cenário fica
    /// separado dos demais testes de frequência do fechamento.
    /// </summary>
    public class Ao_obter_fechamento_final_sem_alunos_validos : NotaFechamentoBimestreTesteBase
    {
        private const long FECHAMENTO_TURMA_ID_1 = 1;
        private const long FECHAMENTO_TURMA_DISCIPLINA_ID_1 = 1;

        public Ao_obter_fechamento_final_sem_alunos_validos(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunosPorTurmaEAnoLetivoQuery, IEnumerable<AlunoPorTurmaResposta>>),
                typeof(ObterAlunosPorTurmaEAnoLetivoQueryHandlerFakeAlunosInativos), ServiceLifetime.Scoped));

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
        }

        [Fact]
        public async Task Deve_retornar_listagem_vazia_quando_nenhum_aluno_e_valido_no_fechamento_final()
        {
            var filtroNotaFechamento = new FiltroFechamentoNotaDto
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

            await InserirPeriodoEscolarCustomizado();
            await CriarDadosBase(filtroNotaFechamento);
            await CriarTipoAvaliacao(TipoAvaliacaoCodigo.AvaliacaoBimestral, AVALIACAO_NOME_1);

            var ultimoPeriodoEscolar = ObterTodos<PeriodoEscolar>().OrderByDescending(p => p.Bimestre).First();
            await CriarFechamentoUltimoBimestre(ultimoPeriodoEscolar);

            var useCase = ServiceProvider.GetService<IListarFechamentoTurmaBimestreUseCase>();

            var retorno = await useCase.Executar(TURMA_CODIGO_1, COMPONENTE_CURRICULAR_PORTUGUES_ID_138, BIMESTRE_FINAL, SEMESTRE_0);

            retorno.ShouldNotBeNull();
            retorno.Alunos.ShouldBeEmpty();
        }

        private async Task CriarFechamentoUltimoBimestre(PeriodoEscolar ultimoPeriodoEscolar)
        {
            await InserirNaBase(new FechamentoTurma
            {
                TurmaId = TURMA_ID_1,
                PeriodoEscolarId = ultimoPeriodoEscolar.Id,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new FechamentoTurmaDisciplina
            {
                DisciplinaId = COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                FechamentoTurmaId = FECHAMENTO_TURMA_ID_1,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            // A consulta de fechamento faz inner join em fechamento_aluno: sem esses registros
            // VerificaSePodeFazerFechamentoFinal não encontra o fechamento do último bimestre.
            await InserirNaBase(new FechamentoAluno
            {
                FechamentoTurmaDisciplinaId = FECHAMENTO_TURMA_DISCIPLINA_ID_1,
                AlunoCodigo = CODIGO_ALUNO_1,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
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
