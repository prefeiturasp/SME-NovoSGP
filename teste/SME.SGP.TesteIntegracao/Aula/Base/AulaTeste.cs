using Elastic.Apm.Api;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Nota.ServicosFakes;
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
    public abstract class AulaTeste : TesteBaseComuns
    {
        private const int QUANTIDADE_3 = 3;

        protected AulaTeste(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterFuncionarioCoreSSOPorPerfilDreQuery, IEnumerable<UsuarioEolRetornoDto>>), typeof(ObterFuncionarioCoreSSOPorPerfilDreQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterFuncionariosPorPerfilDreQuery, IEnumerable<UsuarioEolRetornoDto>>), typeof(ObterFuncionariosPorPerfilDreQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterSupervisorPorCodigoDreQuery, IEnumerable<SupervisoresRetornoDto>>), typeof(ObterSupervisorPorCodigoQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunosPorTurmaQuery, IEnumerable<AlunoPorTurmaResposta>>), typeof(ObterAlunosPorTurmaQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<IncluirFilaInserirAulaRecorrenteCommand, bool>), typeof(IncluirFilaInserirAulaRecorrenteCommandHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<IncluirFilaExclusaoAulaRecorrenteCommand, bool>), typeof(IncluirFilaExclusaoAulaRecorrenteCommandHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<IncluirFilaAlteracaoAulaRecorrenteCommand, bool>), typeof(IncluirFilaAlteracaoAulaRecorrenteCommandHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterComponentesCurricularesDoProfessorNaTurmaQuery, IEnumerable<ComponenteCurricularEol>>), typeof(ObterComponentesCurricularesDoProfessorNaTurmaQueryHandlerAulaFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterValidacaoPodePersistirTurmaNasDatasQuery, List<PodePersistirNaDataRetornoEolDto>>), typeof(ObterValidacaoPodePersistirTurmaNasDatasQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunosEolPorTurmaQuery, IEnumerable<AlunoPorTurmaResposta>>), typeof(Nota.ServicosFakes.ObterAlunosEolPorTurmaQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterTodosAlunosNaTurmaQuery, IEnumerable<AlunoPorTurmaResposta>>), typeof(Nota.ServicosFakes.ObterAlunosEolPorTurmaQueryHandlerFake), ServiceLifetime.Scoped));
        }

        protected async Task<RetornoBaseDto> InserirAulaUseCaseComValidacaoBasica(TipoAula tipoAula, RecorrenciaAula recorrenciaAula, long componentecurricularId, DateTime dataAula, bool ehRegente = false)
        {
            var retorno = await InserirAulaUseCaseSemValidacaoBasica(tipoAula, recorrenciaAula, componentecurricularId, dataAula, ehRegente);

            retorno.ShouldNotBeNull();

            var aulasCadastradas = ObterTodos<Dominio.Aula>();

            aulasCadastradas.ShouldNotBeEmpty();
            aulasCadastradas.Count().ShouldBeGreaterThanOrEqualTo(1);

            return retorno;
        }

        protected async Task<RetornoBaseDto> InserirAulaUseCaseSemValidacaoBasica(TipoAula tipoAula, RecorrenciaAula recorrenciaAula, long componenteCurricularId, DateTime dataAula, bool ehRegente = false, long tipoCalendarioId = 1)
        {
            var useCase = ServiceProvider.GetService<IInserirAulaUseCase>();
            var aula = ObterAula(tipoAula, recorrenciaAula, componenteCurricularId, dataAula, tipoCalendarioId);
            if (ehRegente) aula.EhRegencia = true;

            return await useCase.Executar(aula);
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
                ComponentesCurriculares = new long[] { componenteCurricular },
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
            await CriarParametrosSistema(dataInicio.Year);
        }
        
        protected async Task CriarDadosBasicosAulaSemPeriodoEscolar(string perfil, Modalidade modalidade, ModalidadeTipoCalendario tipoCalendario)
        {
            await CriarTipoCalendario(tipoCalendario);
            await CriarDreUePerfil();
            await CriarComponenteCurricular();
            CriarClaimUsuario(perfil);
            await CriarUsuarios();
            await CriarTurma(modalidade);
        }

        protected async Task CriarDadosBasicosAula(string perfil, Modalidade modalidade, ModalidadeTipoCalendario tipoCalendario, bool criarPeriodo = true)
        {
            await CriarDadosBasicosAula(perfil, modalidade, tipoCalendario, new DateTime(DateTimeExtension.HorarioBrasilia().Year, 05, 02), new DateTime(DateTimeExtension.HorarioBrasilia().Year, 07, 08), BIMESTRE_1, criarPeriodo);
        }

        protected PersistirAulaDto ObterAula(TipoAula tipoAula, RecorrenciaAula recorrenciaAula, long componenteCurricularId, DateTime dataAula, long tipoCalendarioId = 1)
        {
            var componenteCurricular = ObterComponenteCurricular(componenteCurricularId);
            return new PersistirAulaDto()
            {
                CodigoTurma = TURMA_CODIGO_1,
                Quantidade = 1,
                TipoAula = tipoAula,
                DataAula = dataAula,
                DisciplinaCompartilhadaId = componenteCurricularId,
                CodigoUe = UE_CODIGO_1,
                RecorrenciaAula = recorrenciaAula,
                TipoCalendarioId = tipoCalendarioId,
                CodigoComponenteCurricular = long.Parse(componenteCurricular.Codigo),
                NomeComponenteCurricular = componenteCurricular.Descricao
            };
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
            else if (componenteCurricularId == COMPONENTE_CURRICULAR_ARTES_ID_139)
                return new ComponenteCurricularDto()
                {
                    Codigo = COMPONENTE_CURRICULAR_ARTES_ID_139.ToString(),
                    Descricao = COMPONENTE_CURRICULAR_ARTES_NOME
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

        protected async Task CriaAulaRecorrentePortugues(RecorrenciaAula recorrencia, DateTime dataAula)
        {
            var aula = ObterAula(COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), dataAula, recorrencia, USUARIO_PROFESSOR_LOGIN_2222222);
            aula.AulaPaiId = 1;

            await InserirNaBase(aula);
        }
        private Dominio.Aula ObterAula(string componenteCurricularCodigo, DateTime dataAula, RecorrenciaAula recorrencia, string rf = USUARIO_PROFESSOR_LOGIN_2222222)
        {
            return new Dominio.Aula
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

        protected ExcluirAulasRecorrentesComponenteTerritorioSaberDisponibilizadoFiltro ObterExcluirAularFuturaTerritorioSaberDto(DateTime dataReferencia, string codigoTurma, string codigoComponenteCurricular)
        {
            return new ExcluirAulasRecorrentesComponenteTerritorioSaberDisponibilizadoFiltro()
            {
                DataReferenciaAtribuicao = dataReferencia,
                CodigosComponentesCurricularesDisponibilizados = new string[] {codigoComponenteCurricular},
                CodigoTurma = codigoTurma
            };
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
    }
}
