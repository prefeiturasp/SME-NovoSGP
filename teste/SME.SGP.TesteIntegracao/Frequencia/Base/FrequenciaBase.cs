using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.ServicosFakes;
using SME.SGP.TesteIntegracao.ServicosFakes.AulaRecorrenteFake;
using SME.SGP.TesteIntegracao.ServicosFakes.Query;
using SME.SGP.TesteIntegracao.ServicosFakes.Rabbit;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao
{
    public abstract class FrequenciaBase : TesteBaseComuns
    {
        private const int QUANTIDADE_3 = 3;

        private const string REABERTURA_GERAL = "Reabrir Geral";

        private readonly DateTime DATA_01_01 = new(DateTimeExtension.HorarioBrasilia().Year, 01, 01);

        private readonly DateTime DATA_31_12 = new(DateTimeExtension.HorarioBrasilia().Year, 12, 31);

        protected FrequenciaBase(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterFuncionarioCoreSSOPorPerfilDreQuery, IEnumerable<UsuarioEolRetornoDto>>), typeof(ObterFuncionarioCoreSSOPorPerfilDreQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterFuncionariosPorPerfilDreQuery, IEnumerable<UsuarioEolRetornoDto>>), typeof(ObterFuncionariosPorPerfilDreQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterSupervisorPorCodigoQuery, IEnumerable<SupervisoresRetornoDto>>), typeof(ObterSupervisorPorCodigoQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunosPorTurmaQuery, IEnumerable<AlunoPorTurmaResposta>>), typeof(ObterAlunosPorTurmaQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<IncluirFilaInserirAulaRecorrenteCommand, bool>), typeof(IncluirFilaInserirAulaRecorrenteCommandHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<SalvarLogViaRabbitCommand, bool>), typeof(SalvarLogViaRabbitCommandHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<IncluirFilaExclusaoAulaRecorrenteCommand, bool>), typeof(IncluirFilaExclusaoAulaRecorrenteCommandHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<IncluirFilaAlteracaoAulaRecorrenteCommand, bool>), typeof(IncluirFilaAlteracaoAulaRecorrenteCommandHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterComponentesCurricularesDoProfessorNaTurmaQuery, IEnumerable<ComponenteCurricularEol>>), typeof(ObterComponentesCurricularesDoProfessorNaTurmaQueryHandlerAulaFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterValidacaoPodePersistirTurmaNasDatasQuery, List<PodePersistirNaDataRetornoEolDto>>), typeof(ObterValidacaoPodePersistirTurmaNasDatasQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<IncluirFilaCalcularFrequenciaPorTurmaCommand, bool>), typeof(IncluirFilaCalcularFrequenciaPorTurmaCommandHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<IncluirFilaConsolidarDashBoardFrequenciaCommand, bool>), typeof(IncluirFilaConsolidarDashBoardFrequenciaCommandHandlerFake), ServiceLifetime.Scoped));

        }

        protected async Task<AuditoriaDto> InserirFrequenciaUseCaseBasica()
        {
            var useCase = ServiceProvider.GetService<IInserirFrequenciaUseCase>();
            var frequencia = ObterFrequencia();

            var retorno = await useCase.Executar(frequencia);

            retorno.ShouldNotBeNull();

            var aulasCadastradas = ObterTodos<RegistroFrequencia>();

            aulasCadastradas.ShouldNotBeEmpty();
            aulasCadastradas.Count().ShouldBeGreaterThanOrEqualTo(1);

            return retorno;
        }

        protected async Task<RetornoBaseDto> InserirAulaUseCaseSemValidacaoBasica(TipoAula tipoAula, RecorrenciaAula recorrenciaAula, long componenteCurricularId, DateTime dataAula, long tipoCalendarioId, string turmaCodigo, string ueCodigo, bool ehRegente = false)
        {
            var useCase = ServiceProvider.GetService<IInserirAulaUseCase>();
            var aula = ObterAula(tipoAula, recorrenciaAula, componenteCurricularId, dataAula, tipoCalendarioId, turmaCodigo, ueCodigo);
            if (ehRegente) aula.EhRegencia = true;

            return await useCase.Executar(aula);
        }

        protected async Task<CadastroAulaDto> PodeCadastrarAulaUseCase(TipoAula tipoAula, string turmaCodigo, long componenteCurricularId, DateTime dataAula, bool ehRegente = false)
        {
            var useCase = ServiceProvider.GetService<IPodeCadastrarAulaUseCase>();

            var filtroPodeCadastrarAulaDto = ObterFiltroPodeCadastrarAulaDto(tipoAula, turmaCodigo, componenteCurricularId, dataAula, ehRegente);

            return await useCase.Executar(filtroPodeCadastrarAulaDto);
        }

        private FiltroPodeCadastrarAulaDto ObterFiltroPodeCadastrarAulaDto(TipoAula tipoAula, string turmaCodigo, long componenteCurricular, DateTime dataAula, bool ehRegencia, long aulaId = 0)
        {
            return new FiltroPodeCadastrarAulaDto()
            {
                AulaId = aulaId,
                ComponenteCurricular = componenteCurricular,
                DataAula = dataAula,
                EhRegencia = ehRegencia,
                TipoAula = tipoAula,
                TurmaCodigo = turmaCodigo
            };

        }

        protected async Task CriarDadosBasicosAula(string perfil, Modalidade modalidade, ModalidadeTipoCalendario tipoCalendario, DateTime dataInicio, DateTime dataFim, int bimestre, bool criarPeriodo = true, long tipoCalendarioId = 1)
        {
            await CriarTipoCalendario(tipoCalendario);
            await CriarItensComuns(criarPeriodo, dataInicio, dataFim, bimestre, tipoCalendarioId);
            CriarClaimUsuario(perfil);
            await CriarUsuarios();
            await CriarTurma(modalidade);
        }

        protected async Task CriarDadosBasicosAula(string perfil, Modalidade modalidade, ModalidadeTipoCalendario tipoCalendario, bool criarPeriodo = true)
        {
            await CriarDadosBasicosAula(perfil, modalidade, tipoCalendario, new DateTime(DateTimeExtension.HorarioBrasilia().Year, 05, 02), new DateTime(DateTimeExtension.HorarioBrasilia().Year, 07, 08), BIMESTRE_1, criarPeriodo);
        }

        protected FrequenciaDto ObterFrequencia()
        {
            var frenquencia = new FrequenciaDto(AULA_ID);

            frenquencia.ListaFrequencia = ObtenhaListaDeFrequenciaAluno();

            return frenquencia;
        }
        protected PersistirAulaDto ObterAula(TipoAula tipoAula, RecorrenciaAula recorrenciaAula, long componenteCurricularId, DateTime dataAula, long tipoCalendarioId, string turmaCodigo, string ueCodigo)
        {
            var componenteCurricular = ObterComponenteCurricular(componenteCurricularId);
            return new PersistirAulaDto()
            {
                CodigoTurma = turmaCodigo,
                Quantidade = 1,
                TipoAula = tipoAula,
                DataAula = dataAula,
                DisciplinaCompartilhadaId = componenteCurricularId,
                CodigoUe = ueCodigo,
                RecorrenciaAula = recorrenciaAula,
                TipoCalendarioId = tipoCalendarioId,
                CodigoComponenteCurricular = long.Parse(componenteCurricular.Codigo),
                NomeComponenteCurricular = componenteCurricular.Descricao
            };
        }

        private ComponenteCurricularDto ObterComponenteCurricular(long componenteCurricularId)
        {
            if (componenteCurricularId == COMPONENTE_CURRICULAR_PORTUGUES_ID_138)
                return new ComponenteCurricularDto()
                {
                    Codigo = COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(),
                    Descricao = COMPONENTE_CURRICULAR_PORTUGUES_NOME
                };
            else if (componenteCurricularId == COMPONENTE_CURRICULAR_DESCONHECIDO_ID_999999)
                return new ComponenteCurricularDto()
                {
                    Codigo = COMPONENTE_CURRICULAR_DESCONHECIDO_ID_999999.ToString(),
                    Descricao = COMPONENTE_CURRICULAR_DESCONHECIDO_NOME
                };
            else if (componenteCurricularId == COMPONENTE_REG_CLASSE_SP_INTEGRAL_1A5_ANOS_ID_1213)
                return new ComponenteCurricularDto()
                {
                    Codigo = COMPONENTE_REG_CLASSE_SP_INTEGRAL_1A5_ANOS_ID_1213.ToString(),
                    Descricao = COMPONENTE_REG_CLASSE_SP_INTEGRAL_1A5_ANOS_NOME
                };
            else if (componenteCurricularId == COMPONENTE_REG_CLASSE_EJA_ETAPA_ALFAB_ID_1113)
                return new ComponenteCurricularDto()
                {
                    Codigo = COMPONENTE_REG_CLASSE_EJA_ETAPA_ALFAB_ID_1113.ToString(),
                    Descricao = COMPONENTE_REG_CLASSE_EJA_ETAPA_ALFAB_NOME
                };

            return null;
        }

        protected async Task CriarAula(string componenteCurricularCodigo, DateTime dataAula, RecorrenciaAula recorrencia, string rf = USUARIO_PROFESSOR_LOGIN_2222222)
        {
            await InserirNaBase(ObterAula(componenteCurricularCodigo, dataAula, recorrencia, rf));
        }

        protected async Task CriaAulaRecorrentePortugues(RecorrenciaAula recorrencia)
        {
            var aula = ObterAula(COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), new System.DateTime(DateTimeExtension.HorarioBrasilia().Year, 02, 10), recorrencia, USUARIO_PROFESSOR_LOGIN_2222222);
            aula.AulaPaiId = 1;

            await InserirNaBase(aula);
        }
        private Aula ObterAula(string componenteCurricularCodigo, DateTime dataAula, RecorrenciaAula recorrencia, string rf = USUARIO_PROFESSOR_LOGIN_2222222)
        {
            return new Aula
            {
                UeId = UE_CODIGO_1,
                DisciplinaId = componenteCurricularCodigo,
                TurmaId = TURMA_CODIGO_1,
                TipoCalendarioId = 1,
                ProfessorRf = rf,
                Quantidade = QUANTIDADE_3,
                DataAula = dataAula,
                RecorrenciaAula = recorrencia,
                TipoAula = TipoAula.Normal,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                Excluido = false,
                Migrado = false,
                AulaCJ = false
            };
        }
        protected ExcluirAulaDto ObterExcluirAulaDto(RecorrenciaAula recorrencia)
        {
            return new ExcluirAulaDto()
            {
                AulaId = 1,
                RecorrenciaAula = recorrencia
            };
        }

        protected async Task CriarPeriodoEscolarEAberturaPadrao()
        {
            await CriarPeriodoEscolar(DATA_INICIO_BIMESTRE_1, DATA_FIM_BIMESTRE_1, BIMESTRE_1);

            await CriarPeriodoEscolar(DATA_INICIO_BIMESTRE_2, DATA_FIM_BIMESTRE_2, BIMESTRE_2);

            await CriarPeriodoEscolar(DATA_INICIO_BIMESTRE_3, DATA_FIM_BIMESTRE_3, BIMESTRE_3);

            await CriarPeriodoEscolar(DATA_INICIO_BIMESTRE_4, DATA_FIM_BIMESTRE_4, BIMESTRE_4);

            await CriarPeriodoReabertura(TIPO_CALENDARIO_1);
        }

        protected async Task CriarPeriodoReabertura(long tipoCalendarioId)
        {
            await InserirNaBase(new FechamentoReabertura()
            {
                Descricao = REABERTURA_GERAL,
                Inicio = DATA_01_01,
                Fim = DATA_31_12,
                TipoCalendarioId = tipoCalendarioId,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
            });

            await InserirNaBase(new FechamentoReaberturaBimestre()
            {
                FechamentoAberturaId = 1,
                Bimestre = BIMESTRE_1,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
            });

            await InserirNaBase(new FechamentoReaberturaBimestre()
            {
                FechamentoAberturaId = 1,
                Bimestre = BIMESTRE_2,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
            });

            await InserirNaBase(new FechamentoReaberturaBimestre()
            {
                FechamentoAberturaId = 1,
                Bimestre = BIMESTRE_3,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
            });

            await InserirNaBase(new FechamentoReaberturaBimestre()
            {
                FechamentoAberturaId = 1,
                Bimestre = BIMESTRE_4,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
            });
        }

        private List<RegistroFrequenciaAlunoDto> ObtenhaListaDeFrequenciaAluno()
        {
            var lista = new List<RegistroFrequenciaAlunoDto>();
            var aulas = ObtenhaFrenquenciaAula();

            lista.Add(new RegistroFrequenciaAlunoDto() { CodigoAluno = "1", Aulas = aulas, TipoFrequenciaPreDefinido = TipoFrequencia.C.ShortName() });
            lista.Add(new RegistroFrequenciaAlunoDto() { CodigoAluno = "2", Aulas = aulas, TipoFrequenciaPreDefinido = TipoFrequencia.C.ShortName() });
            lista.Add(new RegistroFrequenciaAlunoDto() { CodigoAluno = "3", Aulas = aulas, TipoFrequenciaPreDefinido = TipoFrequencia.C.ShortName() });
            lista.Add(new RegistroFrequenciaAlunoDto() { CodigoAluno = "4", Aulas = aulas, TipoFrequenciaPreDefinido = TipoFrequencia.C.ShortName() });

            return lista;
        }

        private List<FrequenciaAulaDto> ObtenhaFrenquenciaAula()
        {
            var lista = new List<FrequenciaAulaDto>();

            lista.Add(new FrequenciaAulaDto() { NumeroAula = QUANTIDADE_AULA, TipoFrequencia = TipoFrequencia.C.ShortName() });

            return lista;
        }
    }
}
