using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Setup;
using Xunit;

namespace SME.SGP.TesteIntegracao.Listao
{
    public class Ao_sugerir_frequencia_listao : ListaoTesteBase
    {
        public Ao_sugerir_frequencia_listao(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<VerificaPodePersistirTurmaDisciplinaEOLQuery, bool>),
                typeof(VerificaPodePersistirTurmaDisciplinaEOLQueryHandlerComPermissaoFake), ServiceLifetime.Scoped));
        }

        [Fact(DisplayName = "Frequência Listão - Sugestão de frequência não deve replicar para aulas de outros dias")]
        public async Task Nao_deve_replicar_sugestao_de_frequencia_para_aulas_de_outros_dias()
        {
            var filtroListao = new FiltroListao
            {
                Bimestre = BIMESTRE_3,
                Modalidade = Modalidade.Fundamental,
                Perfil = ObterPerfilProfessor(),
                AnoTurma = ANO_8,
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                TipoTurma = TipoTurma.Regular,
                TurmaHistorica = false,
                ComponenteCurricularId = COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                CriarAula = false
            };

            await CriarDadosBasicos(filtroListao);
            await DefinirFrequenciaPreDefinidaAluno(TipoFrequencia.C);

            var dataComFrequenciaSugerida = DATA_25_07_INICIO_BIMESTRE_3;
            var dataSemFrequenciaSugerida = dataComFrequenciaSugerida.AddDays(1);

            await CriarAula(dataComFrequenciaSugerida, RecorrenciaAula.AulaUnica, TipoAula.Normal, USUARIO_PROFESSOR_LOGIN_2222222,
                TURMA_CODIGO_1, UE_CODIGO_1, filtroListao.ComponenteCurricularId.ToString(), TIPO_CALENDARIO_1);
            await CriarAula(dataComFrequenciaSugerida, RecorrenciaAula.AulaUnica, TipoAula.Normal, USUARIO_PROFESSOR_LOGIN_2222222,
                TURMA_CODIGO_1, UE_CODIGO_1, filtroListao.ComponenteCurricularId.ToString(), TIPO_CALENDARIO_1);
            await CriarAula(dataSemFrequenciaSugerida, RecorrenciaAula.AulaUnica, TipoAula.Normal, USUARIO_PROFESSOR_LOGIN_2222222,
                TURMA_CODIGO_1, UE_CODIGO_1, filtroListao.ComponenteCurricularId.ToString(), TIPO_CALENDARIO_1);

            await SalvarFrequenciaAula(TipoFrequencia.F);
            await DefinirFrequenciaPreDefinidaAluno(TipoFrequencia.C);

            var useCaseObterFrequencia = ServiceProvider.GetService<IObterFrequenciasPorPeriodoUseCase>();
            useCaseObterFrequencia.ShouldNotBeNull();

            var frequencias = await useCaseObterFrequencia.Executar(new FiltroFrequenciaPorPeriodoDto
            {
                TurmaId = TURMA_CODIGO_1,
                DisciplinaId = filtroListao.ComponenteCurricularId.ToString(),
                ComponenteCurricularId = filtroListao.ComponenteCurricularId.ToString(),
                DataInicio = dataComFrequenciaSugerida,
                DataFim = dataSemFrequenciaSugerida
            });

            frequencias.ShouldNotBeNull();

            var aluno = frequencias.Alunos.First(aluno => aluno.CodigoAluno == CODIGO_ALUNO_1);
            var aulaComRegistro = aluno.Aulas.First(aula => aula.AulaId == AULA_ID_1);
            var aulaSemRegistroMesmoDia = aluno.Aulas.First(aula => aula.AulaId == AULA_ID_2);
            var aulaSemRegistroOutroDia = aluno.Aulas.First(aula => aula.AulaId == AULA_ID_3);

            aulaComRegistro.TipoFrequencia.ShouldBe(TipoFrequencia.F.ShortName());
            aulaComRegistro.TipoFrequenciaSugerida.ShouldBeNull();

            aulaSemRegistroMesmoDia.TipoFrequencia.ShouldBe(TipoFrequencia.F.ShortName());
            aulaSemRegistroMesmoDia.TipoFrequenciaSugerida.ShouldBe(TipoFrequencia.F.ShortName());

            aulaSemRegistroOutroDia.TipoFrequencia.ShouldBe(TipoFrequencia.C.ShortName());
            aulaSemRegistroOutroDia.TipoFrequenciaSugerida.ShouldBeNull();
        }

        [Fact(DisplayName = "Frequência Listão - Primeiro registro por data e turma deve ignorar excluídos e ser determinístico")]
        public async Task Deve_ignorar_registros_excluidos_e_obter_primeiro_registro_de_forma_deterministica()
        {
            var filtroListao = new FiltroListao
            {
                Bimestre = BIMESTRE_3,
                Modalidade = Modalidade.Fundamental,
                Perfil = ObterPerfilProfessor(),
                AnoTurma = ANO_8,
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                TipoTurma = TipoTurma.Regular,
                TurmaHistorica = false,
                ComponenteCurricularId = COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                CriarAula = false
            };

            await CriarDadosBasicos(filtroListao);

            var dataAula = DATA_25_07_INICIO_BIMESTRE_3;

            await CriarAula(dataAula, RecorrenciaAula.AulaUnica, TipoAula.Normal, USUARIO_PROFESSOR_LOGIN_2222222,
                TURMA_CODIGO_1, UE_CODIGO_1, filtroListao.ComponenteCurricularId.ToString(), TIPO_CALENDARIO_1);
            await CriarAula(dataAula, RecorrenciaAula.AulaUnica, TipoAula.Normal, USUARIO_PROFESSOR_LOGIN_2222222,
                TURMA_CODIGO_1, UE_CODIGO_1, filtroListao.ComponenteCurricularId.ToString(), TIPO_CALENDARIO_1);
            await CriarAula(dataAula, RecorrenciaAula.AulaUnica, TipoAula.Normal, USUARIO_PROFESSOR_LOGIN_2222222,
                TURMA_CODIGO_1, UE_CODIGO_1, filtroListao.ComponenteCurricularId.ToString(), TIPO_CALENDARIO_1);
            await CriarAula(dataAula, RecorrenciaAula.AulaUnica, TipoAula.Normal, USUARIO_PROFESSOR_LOGIN_2222222,
                TURMA_CODIGO_1, UE_CODIGO_1, filtroListao.ComponenteCurricularId.ToString(), TIPO_CALENDARIO_1);

            var aulaExcluida = ObterTodos<Dominio.Aula>().First(aula => aula.Id == AULA_ID_4);
            aulaExcluida.Excluido = true;
            await AtualizarNaBase(aulaExcluida);

            var criadoEm = DateTimeExtension.HorarioBrasilia();
            await CriarRegistroFrequencia(AULA_ID_1, criadoEm.AddMinutes(-2), true);
            await CriarRegistroFrequencia(AULA_ID_4, criadoEm.AddMinutes(-1), false);
            await CriarRegistroFrequencia(AULA_ID_2, criadoEm, false);
            await CriarRegistroFrequencia(AULA_ID_3, criadoEm, false);

            var mediator = ServiceProvider.GetService<IMediator>();
            mediator.ShouldNotBeNull();

            var primeiroRegistroDataTurma = await mediator.Send(
                new ObterPrimeiroRegistroFrequenciaPorDataETurmaQuery(TURMA_CODIGO_1, dataAula));

            primeiroRegistroDataTurma.ShouldNotBeNull();
            primeiroRegistroDataTurma.AulaId.ShouldBe(AULA_ID_2);
            primeiroRegistroDataTurma.QuantidadeAulas.ShouldBe(1);
            primeiroRegistroDataTurma.ComponenteCurricularSugerido.ShouldNotBeNullOrEmpty();
        }

        private async Task DefinirFrequenciaPreDefinidaAluno(TipoFrequencia tipoFrequencia)
        {
            var frequenciaPreDefinida = ObterTodos<FrequenciaPreDefinida>()
                .First(frequencia => frequencia.CodigoAluno == CODIGO_ALUNO_1
                                     && frequencia.TurmaId == TURMA_ID_1
                                     && frequencia.ComponenteCurricularId == COMPONENTE_CURRICULAR_PORTUGUES_ID_138);

            frequenciaPreDefinida.TipoFrequencia = tipoFrequencia;
            await AtualizarNaBase(frequenciaPreDefinida);
        }

        private async Task SalvarFrequenciaAula(TipoFrequencia tipoFrequencia)
        {
            var useCaseSalvar = ServiceProvider.GetService<IInserirFrequenciaListaoUseCase>();
            useCaseSalvar.ShouldNotBeNull();

            await useCaseSalvar.Executar(new[]
            {
                new FrequenciaSalvarAulaAlunosDto
                {
                    AulaId = AULA_ID_1,
                    Alunos = new[]
                    {
                        new FrequenciaSalvarAlunoDto
                        {
                            CodigoAluno = CODIGO_ALUNO_1,
                            Frequencias = new[]
                            {
                                new FrequenciaAulaDto
                                {
                                    NumeroAula = NUMERO_AULA_1,
                                    TipoFrequencia = tipoFrequencia.ShortName()
                                }
                            }
                        }
                    }
                }
            });
        }

        private async Task CriarRegistroFrequencia(long aulaId, DateTime criadoEm, bool excluido)
        {
            await InserirNaBase(new RegistroFrequencia
            {
                AulaId = aulaId,
                CriadoEm = criadoEm,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                Excluido = excluido
            });
        }
    }
}
