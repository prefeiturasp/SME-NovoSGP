using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.ConselhoDeClasse.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;

namespace SME.SGP.TesteIntegracao.Listao
{
    public class ListaoTesteBase : TesteBaseComuns
    {
        private const string ATESTADO_MEDICO_DO_ALUNO = "Atestado Médico do Aluno";
        private const string ATESTADO_MEDIDO_PESSOA_FAMILIA = "Atestado Médico de pessoa da Família";
        private const string ENCHENTE = "Enchente";
        private const string FALTA_TRANSPORTE = "Falta de transporte";
        private const int QTDE_AULAS_A_SEREM_LANCADAS = 10;
        
        private readonly string[] listaDescricaoMotivoAusencia =
        {
            ATESTADO_MEDICO_DO_ALUNO,
            ATESTADO_MEDIDO_PESSOA_FAMILIA,
            ENCHENTE,
            FALTA_TRANSPORTE
        };
        
        protected readonly string[] CODIGOS_ALUNOS =
        {
            CODIGO_ALUNO_1, CODIGO_ALUNO_2, CODIGO_ALUNO_3, CODIGO_ALUNO_4, CODIGO_ALUNO_5, CODIGO_ALUNO_6,
            CODIGO_ALUNO_7, CODIGO_ALUNO_8, CODIGO_ALUNO_9, CODIGO_ALUNO_10, CODIGO_ALUNO_11, CODIGO_ALUNO_12,
            CODIGO_ALUNO_13
        };

        protected readonly TipoFrequencia[] TIPOS_FREQUENCIAS = { TipoFrequencia.C, TipoFrequencia.F, TipoFrequencia.R };
        protected readonly int[] QUANTIDADES_AULAS = { QUANTIDADE_AULA, QUANTIDADE_AULA_2, QUANTIDADE_AULA_3, QUANTIDADE_AULA_4 };

        protected readonly string[] TIPOS_FREQUENCIAS_SIGLA =
        {
            TipoFrequencia.C.ObterNomeCurto(), TipoFrequencia.F.ObterNomeCurto(), TipoFrequencia.R.ObterNomeCurto()
        };

        protected ListaoTesteBase(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }
        
        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunosPorTurmaEDataMatriculaQuery, IEnumerable<AlunoPorTurmaResposta>>),
                typeof(ObterAlunosPorTurmaEDataMatriculaQueryHandlerFakeListao), ServiceLifetime.Scoped));
            
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunosAtivosPorTurmaCodigoQuery, IEnumerable<AlunoPorTurmaResposta>>),
                typeof(ObterAlunosAtivosPorTurmaCodigoQueryHandlerFakeListao), ServiceLifetime.Scoped));            
        }

        protected async Task CriarDadosBasicos(FiltroListao filtroListao)
        {
            await CriarPadrao();
            await CriarComponenteCurricular();
            
            await CriarUsuarios();
            CriarClaimUsuario(filtroListao.Perfil);

            await CriarTurma(filtroListao.Modalidade, filtroListao.AnoTurma, filtroListao.TurmaHistorica,
                filtroListao.TipoTurma);
            
            await CriarTipoCalendario(filtroListao.TipoCalendario);
            await CriarAulas(filtroListao.ComponenteCurricularId, filtroListao.Bimestre);
            await CriarPeriodoEscolarTodosBimestres();
            await InserirParametroSistema();
            await CriarMotivoAusencia();
            await CriarFrequenciaPreDefinida(filtroListao.ComponenteCurricularId);
        }

        private async Task CriarAulas(long componenteCurricularId, int bimestre)
        {
            var datasAulasIncluidas = Array.Empty<DateTime?>();
            
            var (dataInicio, dataFim) = await DefinirDataInicioFimBimestre(bimestre);
            var range = dataFim.Subtract(dataInicio).Days;
            
            for (var i = 0; i < QTDE_AULAS_A_SEREM_LANCADAS; i++)
            {
                var dataAula = dataInicio.AddDays(new Random().Next(0, range));
                
                while (datasAulasIncluidas.Contains(dataAula) || dataAula > DateTimeExtension.HorarioBrasilia())
                    dataAula = dataInicio.AddDays(new Random().Next(0, range));
                
                await CriarAula(dataAula, RecorrenciaAula.AulaUnica, TipoAula.Normal, USUARIO_PROFESSOR_LOGIN_2222222,
                    TURMA_CODIGO_1, UE_CODIGO_1, componenteCurricularId.ToString(), TIPO_CALENDARIO_1);                
            }
        }
        
        private async Task CriarPeriodoEscolarTodosBimestres()
        {
            await CriarPeriodoEscolar(DATA_01_02_INICIO_BIMESTRE_1, DATA_25_04_FIM_BIMESTRE_1, BIMESTRE_1);
            await CriarPeriodoEscolar(DATA_02_05_INICIO_BIMESTRE_2, DATA_08_07_FIM_BIMESTRE_2, BIMESTRE_2);
            await CriarPeriodoEscolar(DATA_25_07_INICIO_BIMESTRE_3, DATA_30_09_FIM_BIMESTRE_3, BIMESTRE_3);
            await CriarPeriodoEscolar(DATA_03_10_INICIO_BIMESTRE_4, DATA_22_12_FIM_BIMESTRE_4, BIMESTRE_4);
        }

        private async Task<(DateTime dataInicio, DateTime dataFim)> DefinirDataInicioFimBimestre(int bimestre)
        {
            DateTime dataInicio = default;
            DateTime dataFim = default;

            switch (bimestre)
            {
                case BIMESTRE_1:
                {
                    dataInicio = DATA_01_02_INICIO_BIMESTRE_1;
                    dataFim = DATA_25_04_FIM_BIMESTRE_1;
                    break;
                }
                case BIMESTRE_2:
                    dataInicio = DATA_02_05_INICIO_BIMESTRE_2;
                    dataFim = DATA_08_07_FIM_BIMESTRE_2;
                    break;
                case BIMESTRE_3:
                    dataInicio = DATA_25_07_INICIO_BIMESTRE_3;
                    dataFim = DATA_30_09_FIM_BIMESTRE_3;
                    break;
                case BIMESTRE_4:
                    dataInicio = DATA_03_10_INICIO_BIMESTRE_4;
                    dataFim = DATA_22_12_FIM_BIMESTRE_4;
                    break;
            }   
            
            return await Task.FromResult(new ValueTuple<DateTime, DateTime>(dataInicio, dataFim));
        }

        private async Task InserirParametroSistema()
        {
            await InserirNaBase(new ParametrosSistema()
            {
                Nome = "PercentualFrequenciaCritico",
                Tipo = TipoParametroSistema.PercentualFrequenciaCritico,
                Descricao = "Percentual de frequência para definir aluno em situação crítica",
                Valor = "75",
                Ano = DateTimeExtension.HorarioBrasilia().Year,
                Ativo = true,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new ParametrosSistema()
            {
                Nome = "PercentualFrequenciaAlerta",
                Tipo = TipoParametroSistema.PercentualFrequenciaAlerta,
                Descricao = "Percentual de frequência para definir aluno em situação de alerta",
                Valor = "80",
                Ano = DateTimeExtension.HorarioBrasilia().Year,
                Ativo = true,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }

        protected async Task CriarRegistroFrenquencia(int bimestre, long componenteCurricularId)
        {
            var aulaId = (ObterTodos<Dominio.Aula>().FirstOrDefault()?.Id).GetValueOrDefault();
            aulaId.ShouldBeGreaterThan(0);
            
            await InserirNaBase(new RegistroFrequencia
            {
                AulaId = aulaId,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            var registroFrequenciaId = (ObterTodos<RegistroFrequencia>().FirstOrDefault()?.Id).GetValueOrDefault();
            registroFrequenciaId.ShouldBeGreaterThan(0);

            var periodoEscolar = ObterTodos<PeriodoEscolar>().FirstOrDefault(c => c.Bimestre == bimestre);
            periodoEscolar.ShouldNotBeNull();
            
            string[] codigosAlunosAnotacaoFrequencia = { CODIGO_ALUNO_2, CODIGO_ALUNO_4 };

            foreach (var codigoAluno in CODIGOS_ALUNOS)
            {
                var rand = new Random();
                var index = rand.Next(QUANTIDADES_AULAS.Length);
                
                await CriarRegistroFrequenciaAluno(registroFrequenciaId, codigoAluno, QUANTIDADES_AULAS[index], aulaId);
                await CriarFrequenciaAluno(periodoEscolar, codigoAluno, componenteCurricularId.ToString(), QUANTIDADES_AULAS[index]);

                if (codigosAlunosAnotacaoFrequencia.Contains(codigoAluno))
                    await CriarAnotacaoFrequencia(aulaId, codigoAluno);
            }
        }        
        
        private async Task CriarRegistroFrequenciaAluno(long registroFrequenciaId, string codigoAluno, int numeroAula,
            long aulaId)
        {
            var rand = new Random();
            var index = rand.Next(TIPOS_FREQUENCIAS.Length);
            
            await InserirNaBase(new RegistroFrequenciaAluno
            {
                CodigoAluno = codigoAluno,
                RegistroFrequenciaId = registroFrequenciaId,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                Valor = (int)TIPOS_FREQUENCIAS[index],
                NumeroAula = numeroAula,
                AulaId = aulaId
            });
        }

        private async Task CriarFrequenciaAluno(PeriodoEscolar periodoEscolar, string codigoAluno, string disciplinaId, int totalAulas)
        {
            await InserirNaBase(new Dominio.FrequenciaAluno
            {
                Bimestre = periodoEscolar.Bimestre,
                Tipo = TipoFrequenciaAluno.PorDisciplina,
                CodigoAluno = codigoAluno,
                DisciplinaId = disciplinaId,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                PeriodoFim = periodoEscolar.PeriodoFim,
                PeriodoInicio = periodoEscolar.PeriodoInicio,
                TotalAulas = totalAulas,
                TotalAusencias = 0,
                TotalCompensacoes = 0,
                TotalPresencas = 0,
                TotalRemotos = 0,
                TurmaId = TURMA_CODIGO_1,
                PeriodoEscolarId = periodoEscolar.Id,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }

        private async Task CriarMotivoAusencia()
        {
            foreach (var descricaoMotivoAusencia in listaDescricaoMotivoAusencia)
            {
                await InserirNaBase(new MotivoAusencia
                {
                    Descricao = descricaoMotivoAusencia
                });
            }
        }
        
        private async Task CriarAnotacaoFrequencia(long aulaId, string codigoAluno)
        {
            var motivosAunsencias = ObterTodos<MotivoAusencia>();
            motivosAunsencias.ShouldNotBeNull();

            var idsMotivosAusencias = motivosAunsencias.Select(c => c.Id).ToArray();
            
            var rand = new Random();
            var index = rand.Next(idsMotivosAusencias.Length);
            
            await InserirNaBase(new AnotacaoFrequenciaAluno
            {
                AulaId = aulaId,
                CodigoAluno = codigoAluno,
                MotivoAusenciaId = idsMotivosAusencias[index],
                Anotacao = "Teste de integração do Listão.",
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }

        private async Task CriarFrequenciaPreDefinida(long componenteCurricularId)
        {
            var turmaId = ObterTodos<Turma>().Select(c => c.Id).FirstOrDefault();

            foreach (var codigoAluno in CODIGOS_ALUNOS)
            {
                var rand = new Random();
                var index = rand.Next(TIPOS_FREQUENCIAS.Length);
                
                await InserirNaBase(new FrequenciaPreDefinida()
                {
                    CodigoAluno = codigoAluno,
                    TipoFrequencia = TIPOS_FREQUENCIAS[index],
                    ComponenteCurricularId = componenteCurricularId,
                    TurmaId = turmaId
                });                
            }
        }        
    }
}