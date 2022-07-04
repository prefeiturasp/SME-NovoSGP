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

        protected const long AULA_ID_1 = 1;

        protected const int NUMERO_AULAS_1 = 1;
        protected const int NUMERO_AULAS_2 = 2;
        protected const int NUMERO_AULAS_3 = 3;

        protected const int QTDE_1 = 1;
        protected const int QTDE_2 = 2;
        protected const int QTDE_3 = 3;

        protected const string TIPO_FREQUENCIA_COMPARECEU = "C";
        protected const string TIPO_FREQUENCIA_FALTOU = "F";
        protected const string TIPO_FREQUENCIA_REMOTO = "R";

        protected const string CODIGO_ALUNO_99999 = "99999";        
        protected const string CODIGO_ALUNO_77777 = "77777";        

        private const string REABERTURA_GERAL = "Reabrir Geral";

        protected readonly DateTime DATA_01_01 = new(DateTimeExtension.HorarioBrasilia().Year, 01, 01);

        private readonly DateTime DATA_31_12 = new(DateTimeExtension.HorarioBrasilia().Year, 12, 31);

        protected const int ZERO = 0;

        protected const long TURMA_ID_1 = 1;

        protected const decimal PERCENTUAL_100 = 100.0M;
        protected const decimal PERCENTUAL_ZERO = 0.00M;

        protected readonly DateTime DATA_02_05 = new(DateTimeExtension.HorarioBrasilia().Year, 05, 02);
        protected readonly DateTime DATA_07_08 = new(DateTimeExtension.HorarioBrasilia().Year, 08, 07);
        protected readonly DateTime DATA_03_08 = new(DateTimeExtension.HorarioBrasilia().Year-6, 03, 07);

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

        protected async Task<AuditoriaDto> InserirFrequenciaUseCaseComValidacaoBasica(FrequenciaDto frequenciaDto)
        {
            var retorno = await InserirFrequenciaUseCaseSemValidacaoBasica(frequenciaDto);

            retorno.ShouldNotBeNull();

            var registroFrequencia = ObterTodos<RegistroFrequencia>();
            registroFrequencia.ShouldNotBeEmpty();
            registroFrequencia.Count().ShouldBeGreaterThanOrEqualTo(1);

            var registroFrequenciaAluno = ObterTodos<RegistroFrequenciaAluno>();
            registroFrequenciaAluno.ShouldNotBeEmpty();
            registroFrequenciaAluno.Count().ShouldBeGreaterThanOrEqualTo(1);

            var consolidacaoFrequenciaAlunoMensal = ObterTodos<Dominio.ConsolidacaoFrequenciaAlunoMensal>();
            consolidacaoFrequenciaAlunoMensal.ShouldNotBeEmpty();
            consolidacaoFrequenciaAlunoMensal.Count().ShouldBeGreaterThanOrEqualTo(1);

            var consolidacaoDashBoardFrequencias = ObterTodos<ConsolidacaoDashBoardFrequencia>();
            consolidacaoDashBoardFrequencias.ShouldNotBeEmpty();
            consolidacaoDashBoardFrequencias.Count().ShouldBeGreaterThanOrEqualTo(1);

            return retorno;
        }

        protected async Task<AuditoriaDto> InserirFrequenciaUseCaseSemValidacaoBasica(FrequenciaDto frequenciaDto)
        {
            var useCase = ServiceProvider.GetService<IInserirFrequenciaUseCase>();

            return await useCase.Executar(frequenciaDto);
        }

        protected async Task<AuditoriaDto> SalvarAnotacaoFrequenciaAlunoUseCase(SalvarAnotacaoFrequenciaAlunoDto salvarAnotacaoFrequenciaAlunoDto)
        {
            var useCase = ServiceProvider.GetService<ISalvarAnotacaoFrequenciaAlunoUseCase>();

            return await useCase.Executar(salvarAnotacaoFrequenciaAlunoDto);
        }

        protected  async Task<bool> ExcluirAnotacaoFrequenciaAlunoUseCase(long id)
        {
            var useCase = ServiceProvider.GetService<IExcluirAnotacaoFrequenciaAlunoUseCase>();
            return await useCase.Executar(id);
        }

        protected async Task<bool> AlterarAnotacaoFrequenciaAlunoUseCase(AlterarAnotacaoFrequenciaAlunoDto param)
        {
            var useCase = ServiceProvider.GetService<IAlterarAnotacaoFrequenciaAlunoUseCase>();
            return await useCase.Executar(param);
        }

        protected async Task CriarDadosBase(string perfil, Modalidade modalidade, ModalidadeTipoCalendario tipoCalendario, DateTime dataInicio, DateTime dataFim, int bimestre, long tipoCalendarioId = 1, bool criarPeriodo = true)
        {
            await CriarTipoCalendario(tipoCalendario);
            await CriarItensComuns(criarPeriodo, dataInicio, dataFim, bimestre, tipoCalendarioId);
            CriarClaimUsuario(perfil);
            await CriarUsuarios();
        }

        protected async Task CriarDadosBasicos(string perfil, Modalidade modalidade, ModalidadeTipoCalendario tipoCalendario, DateTime dataInicio, DateTime dataFim, int bimestre,DateTime dataAula,string componenteCurricular, int quantidadeAula = QUANTIDADE_3, bool criarPeriodo = true, long tipoCalendarioId = 1, bool criarPeriodoEscolarEAbertura = true)
        {
            await CriarTipoCalendario(tipoCalendario);
            await CriarItensComuns(criarPeriodo, dataInicio, dataFim, bimestre, tipoCalendarioId);
            CriarClaimUsuario(perfil);
            await CriarUsuarios();
            await CriarTurma(modalidade);
            await CriarAula(componenteCurricular, dataAula, RecorrenciaAula.AulaUnica, quantidadeAula);
            if (criarPeriodoEscolarEAbertura)
                await CriarPeriodoEscolarEAbertura();
        }

        protected async Task CriarPeriodoEscolarEAbertura()
        {
            await CriarPeriodoEscolar(DATA_01_02_INICIO_BIMESTRE_1, DATA_25_04_FIM_BIMESTRE_1, BIMESTRE_1);

            await CriarPeriodoEscolar(DATA_02_05_INICIO_BIMESTRE_2, DATA_08_07_FIM_BIMESTRE_2, BIMESTRE_2);

            await CriarPeriodoEscolar(DATA_25_07_INICIO_BIMESTRE_3, DATA_30_09_FIM_BIMESTRE_3, BIMESTRE_3);

            await CriarPeriodoEscolar(DATA_03_10_INICIO_BIMESTRE_4, DATA_22_12_FIM_BIMESTRE_4, BIMESTRE_4);

            await CriarPeriodoReabertura(TIPO_CALENDARIO_1);
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

        protected async Task CriarAula(string componenteCurricularCodigo, DateTime dataAula, RecorrenciaAula recorrencia, int quantidadeAula, string rf = USUARIO_PROFESSOR_LOGIN_2222222)
        {
            await InserirNaBase(ObterAula(componenteCurricularCodigo, dataAula, recorrencia, quantidadeAula, rf));
        }

        private Aula ObterAula(string componenteCurricularCodigo, DateTime dataAula, RecorrenciaAula recorrencia, int quantidadeAula, string rf = USUARIO_PROFESSOR_LOGIN_2222222)
        {
            return new Aula
            {
                UeId = UE_CODIGO_1,
                DisciplinaId = componenteCurricularCodigo,
                TurmaId = TURMA_CODIGO_1,
                TipoCalendarioId = 1,
                ProfessorRf = rf,
                Quantidade = quantidadeAula,
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

        protected async Task CriarPeriodoEscolarEAberturaPadrao()
        {
            await CriarPeriodoEscolar(DATA_01_02_INICIO_BIMESTRE_1, DATA_25_04_FIM_BIMESTRE_1, BIMESTRE_1);

            await CriarPeriodoEscolar(DATA_02_05_INICIO_BIMESTRE_2, DATA_08_07_FIM_BIMESTRE_2, BIMESTRE_2);

            await CriarPeriodoEscolar(DATA_25_07_INICIO_BIMESTRE_3, DATA_30_09_FIM_BIMESTRE_3, BIMESTRE_3);

            await CriarPeriodoEscolar(DATA_03_10_INICIO_BIMESTRE_4, DATA_22_12_FIM_BIMESTRE_4, BIMESTRE_4);

            await CriarPeriodoReabertura(TIPO_CALENDARIO_1);
        }
        protected async Task CriarMotivoAusencia(string descricao)
        {
            await InserirNaBase(new MotivoAusencia() {Descricao = descricao });
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

        protected FrequenciaDto ObtenhaFrenqueciaDto()
        {
            var frenquencia = new FrequenciaDto(AULA_ID_1);

            frenquencia.ListaFrequencia = ObtenhaListaDeFrequenciaAluno();

            return frenquencia;
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

        protected async Task InserirFrequenciaUseCaseComValidacaoCompleta(FrequenciaDto frequencia, TipoFrequencia tipoFrequenciaPreDefinida, TipoFrequencia tipoFrequenciaRegistrada, decimal percentualFrequencia, int numeroAulas, int qtdeAusencias, int qtdeCompensacoes)
        {
            await InserirFrequenciaUseCaseSemValidacaoBasica(frequencia);

            var frequenciaPreDefinida = ObterTodos<FrequenciaPreDefinida>();
            frequenciaPreDefinida.ShouldNotBeEmpty();
            frequenciaPreDefinida.FirstOrDefault().TipoFrequencia.Equals(tipoFrequenciaPreDefinida).ShouldBeTrue();

            var registroFrequenciaAluno = ObterTodos<RegistroFrequenciaAluno>();
            registroFrequenciaAluno.ShouldNotBeEmpty();
            (registroFrequenciaAluno.FirstOrDefault().Valor == (int)tipoFrequenciaRegistrada).ShouldBeTrue();

            var consolidacaoFrequenciaAlunoMensal = ObterTodos<Dominio.ConsolidacaoFrequenciaAlunoMensal>();
            consolidacaoFrequenciaAlunoMensal.ShouldNotBeEmpty();
            (consolidacaoFrequenciaAlunoMensal.FirstOrDefault().Percentual == percentualFrequencia).ShouldBeTrue();
            (consolidacaoFrequenciaAlunoMensal.FirstOrDefault().QuantidadeAulas == numeroAulas).ShouldBeTrue();
            (consolidacaoFrequenciaAlunoMensal.FirstOrDefault().QuantidadeAusencias == qtdeAusencias).ShouldBeTrue();
            (consolidacaoFrequenciaAlunoMensal.FirstOrDefault().QuantidadeCompensacoes == qtdeCompensacoes).ShouldBeTrue();
        }

        protected async Task CriarPredefinicaoAluno(string codigoAluno, TipoFrequencia tipoFrequencia, long componenteCurricularId, long turmaId)
        {
            await InserirNaBase(new FrequenciaPreDefinida()
            {
                CodigoAluno = codigoAluno,
                TipoFrequencia = tipoFrequencia,
                ComponenteCurricularId = componenteCurricularId,
                TurmaId = turmaId
            });
        }
    }
}
