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
using SME.SGP.TesteIntegracao.ServicosFakes;
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

        private readonly string[] codigosAlunos =
        {
            CODIGO_ALUNO_1, CODIGO_ALUNO_2, CODIGO_ALUNO_3, CODIGO_ALUNO_4, CODIGO_ALUNO_5, CODIGO_ALUNO_6,
            CODIGO_ALUNO_7, CODIGO_ALUNO_8, CODIGO_ALUNO_9, CODIGO_ALUNO_10, CODIGO_ALUNO_11, CODIGO_ALUNO_12,
            CODIGO_ALUNO_13, CODIGO_ALUNO_14
        };

        private readonly TipoFrequencia[] tiposFrequencias = { TipoFrequencia.C, TipoFrequencia.F, TipoFrequencia.R };
        private readonly int[] quantidadesAulas = { QUANTIDADE_AULA, QUANTIDADE_AULA_2, QUANTIDADE_AULA_3, QUANTIDADE_AULA_4 };

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
            
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterTodosAlunosNaTurmaQuery,IEnumerable<AlunoPorTurmaResposta>>),
                typeof(ObterTodosAlunosNaTurmaQueryHandlerFakeListao), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunosDentroPeriodoQuery, IEnumerable<AlunoPorTurmaResposta>>), typeof(ObterAlunosDentroPeriodoQueryHandlerFake), ServiceLifetime.Scoped));

        }

        protected async Task CriarDadosBasicos(FiltroListao filtroListao)
        {
            await CriarDreUePerfil();
            await CriarComponenteCurricular();
            
            await CriarUsuarios();
            CriarClaimUsuario(filtroListao.Perfil);

            await CriarTurma(filtroListao.Modalidade, filtroListao.AnoTurma, filtroListao.TurmaHistorica,
                filtroListao.TipoTurma, filtroListao.TipoTurno);
            
            await CriarTipoCalendario(filtroListao.TipoCalendario, filtroListao.TurmaHistorica);
            
            await CriarPeriodoEscolarTodosBimestres(filtroListao.TipoCalendario, filtroListao.TurmaHistorica);
            
            if (filtroListao.CriarAula)
                await CriarAulas(filtroListao.ComponenteCurricularId, filtroListao.Bimestre);
            
            if (filtroListao.CriarPeriodoReaberturaTodosBimestres)
                await CriarPeriodoReaberturaTodosBimestres(filtroListao.TipoCalendario, filtroListao.TurmaHistorica);
            
            await InserirParametroSistema();
            await CriarMotivoAusencia();
            await CriarFrequenciaPreDefinida(filtroListao.ComponenteCurricularId);
            await CriarParametrosSistema(DateTimeExtension.HorarioBrasilia().Year);
        }
        
        protected IEnumerable<FrequenciaSalvarAlunoDto> ObterListaFrequenciaSalvarAluno(bool desabilitado = false)
        {
            return codigosAlunos.Select(codigoAluno => new FrequenciaSalvarAlunoDto
                { CodigoAluno = codigoAluno, Frequencias = ObterFrequenciaAula(), Desabilitado = desabilitado}).ToList();
        }

        private IEnumerable<FrequenciaAulaDto> ObterFrequenciaAula()
        {
            return quantidadesAulas.Select(numeroAula => new FrequenciaAulaDto
            {
                NumeroAula = numeroAula,
                TipoFrequencia = tiposFrequencias[new Random().Next(tiposFrequencias.Length)].ObterNomeCurto()
            }).ToList();
        } 
        
        protected IEnumerable<FrequenciaSalvarAlunoDto> ObterListaFrequenciaSalvarAlunoComAusencia()
        {
            return codigosAlunos.Select(codigoAluno => new FrequenciaSalvarAlunoDto
                { CodigoAluno = codigoAluno, Frequencias = ObterFrequenciaAula(codigoAluno) }).ToList();
        }

        protected IEnumerable<FrequenciaAulaDto> ObterFrequenciaAula(string codigoAluno)
        {
            string[] codigosAlunosAusencia = { CODIGO_ALUNO_1, CODIGO_ALUNO_5 };
            string[] codigosAlunosPresenca = { CODIGO_ALUNO_2, CODIGO_ALUNO_4, CODIGO_ALUNO_6 };
            string[] codigosAlunosRemotos = { CODIGO_ALUNO_3, CODIGO_ALUNO_7 };

            return quantidadesAulas.Select(numeroAula => new FrequenciaAulaDto
            {
                NumeroAula = numeroAula,
                TipoFrequencia = codigosAlunosAusencia.Contains(codigoAluno) ? TipoFrequencia.F.ObterNomeCurto() :
                    codigosAlunosPresenca.Contains(codigoAluno) ? TipoFrequencia.C.ObterNomeCurto() :
                    codigosAlunosRemotos.Contains(codigoAluno) ? TipoFrequencia.R.ObterNomeCurto() :
                    tiposFrequencias[new Random().Next(tiposFrequencias.Length)].ObterNomeCurto()
            }).ToList();
        }

        private async Task CriarAulas(long componenteCurricularId, int bimestre)
        {
            var datasAulasIncluidas = new List<DateTime>();
            var periodoEscolar = ObterTodos<PeriodoEscolar>().FirstOrDefault(w=> w.Bimestre == bimestre);
            
            for (var i = 0; i < QTDE_AULAS_A_SEREM_LANCADAS; i++)
            {
                var dataAula = periodoEscolar.PeriodoInicio.AddDays(i);
                await CriarAula(dataAula, RecorrenciaAula.AulaUnica, TipoAula.Normal, USUARIO_PROFESSOR_LOGIN_2222222,
                    TURMA_CODIGO_1, UE_CODIGO_1, componenteCurricularId.ToString(), TIPO_CALENDARIO_1);

                datasAulasIncluidas.Add(dataAula);
            }
        }
        
        private async Task CriarPeriodoEscolarTodosBimestres(ModalidadeTipoCalendario modalidadeTipoCalendario, bool turmaHistorica)
        {
            var tipoCalendarioId = (ObterTodos<TipoCalendario>()
                .FirstOrDefault(c => c.Modalidade == modalidadeTipoCalendario)?.Id).GetValueOrDefault();
            
            tipoCalendarioId.ShouldBeGreaterThan(0);
            
            await CriarPeriodoEscolar(DATA_01_02_INICIO_BIMESTRE_1, DATA_25_04_FIM_BIMESTRE_1, BIMESTRE_1, tipoCalendarioId, turmaHistorica);
            await CriarPeriodoEscolar(DATA_02_05_INICIO_BIMESTRE_2, DATA_24_07_FIM_BIMESTRE_2, BIMESTRE_2, tipoCalendarioId, turmaHistorica);
            await CriarPeriodoEscolar(DATA_25_07_INICIO_BIMESTRE_3, DATA_02_10_FIM_BIMESTRE_3, BIMESTRE_3, tipoCalendarioId, turmaHistorica);
            await CriarPeriodoEscolar(DATA_03_10_INICIO_BIMESTRE_4, DATA_22_12_FIM_BIMESTRE_4, BIMESTRE_4, tipoCalendarioId, turmaHistorica);
        }

        private async Task CriarPeriodoReaberturaTodosBimestres(ModalidadeTipoCalendario modalidadeTipoCalendario, bool turmaHistoria)
        {
            var tipoCalendarioId = (ObterTodos<TipoCalendario>()
                .FirstOrDefault(c => c.Modalidade == modalidadeTipoCalendario)?.Id).GetValueOrDefault();
            
            tipoCalendarioId.ShouldBeGreaterThan(0);
            
            await InserirNaBase(new FechamentoReabertura
            {
                Descricao = REABERTURA_GERAL,
                Inicio = turmaHistoria ? DATA_01_01_ANO_ANTERIOR : DATA_01_01,
                Fim = turmaHistoria ? DATA_31_12_ANO_ANTERIOR : DATA_31_12,
                TipoCalendarioId = tipoCalendarioId,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
            });

            var fechamentoAberturaId = (ObterTodos<FechamentoReabertura>()
                .FirstOrDefault(c => c.TipoCalendarioId == tipoCalendarioId)?.Id).GetValueOrDefault();
            
            fechamentoAberturaId.ShouldBeGreaterThan(0);

            await InserirNaBase(new FechamentoReaberturaBimestre
            {
                FechamentoAberturaId = fechamentoAberturaId,
                Bimestre = BIMESTRE_1,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
            });

            await InserirNaBase(new FechamentoReaberturaBimestre
            {
                FechamentoAberturaId = fechamentoAberturaId,
                Bimestre = BIMESTRE_2,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
            });

            await InserirNaBase(new FechamentoReaberturaBimestre
            {
                FechamentoAberturaId = fechamentoAberturaId,
                Bimestre = BIMESTRE_3,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
            });

            await InserirNaBase(new FechamentoReaberturaBimestre
            {
                FechamentoAberturaId = fechamentoAberturaId,
                Bimestre = BIMESTRE_4,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
            });
        }

        private async Task<(DateTime dataInicio, DateTime dataFim)> DefinirDataInicioFimBimestre(int bimestre, bool turmaHistorica)
        {
            DateTime dataInicio = default;
            DateTime dataFim = default;

            switch (bimestre)
            {
                case BIMESTRE_1:
                {
                    dataInicio = turmaHistorica ? DATA_03_01_INICIO_BIMESTRE_1_ANO_ANTERIOR : DATA_03_01_INICIO_BIMESTRE_1;
                    dataFim = turmaHistorica ? DATA_28_04_FIM_BIMESTRE_1_ANO_ANTERIOR : DATA_01_05_FIM_BIMESTRE_1;
                    break;
                }
                case BIMESTRE_2:
                {
                    dataInicio = turmaHistorica ? DATA_02_05_INICIO_BIMESTRE_2_ANO_ANTERIOR : DATA_02_05_INICIO_BIMESTRE_2;
                    dataFim = turmaHistorica ? DATA_08_07_FIM_BIMESTRE_2_ANO_ANTERIOR : DATA_24_07_FIM_BIMESTRE_2;
                    break;
                }
                case BIMESTRE_3:
                {
                    dataInicio = turmaHistorica ? DATA_25_07_INICIO_BIMESTRE_3_ANO_ANTERIOR : DATA_25_07_INICIO_BIMESTRE_3;
                    dataFim = turmaHistorica ? DATA_30_09_FIM_BIMESTRE_3_ANO_ANTERIOR : DATA_02_10_FIM_BIMESTRE_3;
                    break;
                }
                case BIMESTRE_4:
                {
                    dataInicio = turmaHistorica ? DATA_03_10_INICIO_BIMESTRE_4_ANO_ANTERIOR : DATA_03_10_INICIO_BIMESTRE_4;
                    dataFim = turmaHistorica ? DATA_22_12_FIM_BIMESTRE_4_ANO_ANTERIOR : DATA_22_12_FIM_BIMESTRE_4;
                    break;
                }
            }   
            
            return await Task.FromResult(new ValueTuple<DateTime, DateTime>(dataInicio, dataFim));
        }

        protected async Task InserirParametroSistema()
        {
            await InserirNaBase(new ParametrosSistema
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

            await InserirNaBase(new ParametrosSistema
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

            foreach (var codigoAluno in codigosAlunos)
            {
                var rand = new Random();
                var index = rand.Next(quantidadesAulas.Length);
                
                await CriarRegistroFrequenciaAluno(registroFrequenciaId, codigoAluno, quantidadesAulas[index], aulaId);
                await CriarFrequenciaAluno(periodoEscolar, codigoAluno, componenteCurricularId.ToString(), quantidadesAulas[index]);

                if (codigosAlunosAnotacaoFrequencia.Contains(codigoAluno))
                    await CriarAnotacaoFrequencia(aulaId, codigoAluno);
            }
        }
        
        protected async Task CriarRegistroFrenquenciaTodasAulas(int bimestre, long componenteCurricularId)
        {
            var aulas = ObterTodos<Dominio.Aula>();

            foreach (var aula in aulas)
            {
                await InserirNaBase(new RegistroFrequencia
                {
                    AulaId = aula.Id,
                    CriadoPor = SISTEMA_NOME,
                    CriadoRF = SISTEMA_CODIGO_RF
                });
            }

            var registroFrequenciaId = (ObterTodos<RegistroFrequencia>().FirstOrDefault()?.Id).GetValueOrDefault();
            registroFrequenciaId.ShouldBeGreaterThan(0);

            var periodoEscolar = ObterTodos<PeriodoEscolar>().FirstOrDefault(c => c.Bimestre == bimestre);
            periodoEscolar.ShouldNotBeNull();
            
            string[] codigosAlunosAnotacaoFrequencia = { CODIGO_ALUNO_2, CODIGO_ALUNO_4 };
            var codigoAlunos = new List<string>();
            foreach (var aula in aulas)
            {
                foreach (var codigoAluno in codigosAlunos)
                {
                    var rand = new Random();
                    var index = rand.Next(quantidadesAulas.Length);
                
                    await CriarRegistroFrequenciaAluno(registroFrequenciaId, codigoAluno, quantidadesAulas[index], aula.Id);

                    if (!codigoAlunos.Contains(codigoAluno))
                    {
                        await CriarFrequenciaAluno(periodoEscolar, codigoAluno, componenteCurricularId.ToString(), quantidadesAulas[index]);
                        codigoAlunos.Add(codigoAluno); 
                    }

                    if (codigosAlunosAnotacaoFrequencia.Contains(codigoAluno))
                        await CriarAnotacaoFrequencia(aula.Id, codigoAluno);
                }
            }
        }
        
        private async Task CriarRegistroFrequenciaAluno(long registroFrequenciaId, string codigoAluno, int numeroAula,
            long aulaId)
        {
            var rand = new Random();
            var index = rand.Next(tiposFrequencias.Length);
            
            await InserirNaBase(new RegistroFrequenciaAluno
            {
                CodigoAluno = codigoAluno,
                RegistroFrequenciaId = registroFrequenciaId,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                Valor = (int)tiposFrequencias[index],
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

        protected async Task CriarMotivoAusencia()
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

        protected async Task CriarFrequenciaPreDefinida(long componenteCurricularId)
        {
            var turmaId = ObterTodos<Dominio.Turma>().Select(c => c.Id).FirstOrDefault();

            foreach (var codigoAluno in codigosAlunos)
            {
                await InserirNaBase(new FrequenciaPreDefinida()
                {
                    CodigoAluno = codigoAluno,
                    TipoFrequencia = tiposFrequencias[new Random().Next(tiposFrequencias.Length)],
                    ComponenteCurricularId = componenteCurricularId,
                    TurmaId = turmaId
                });                
            }
        }        
        
        protected IInserirFrequenciaListaoUseCase InserirFrequenciaListaoUseCase()
        {
            return ServiceProvider.GetService<IInserirFrequenciaListaoUseCase>();
        }
    }
}