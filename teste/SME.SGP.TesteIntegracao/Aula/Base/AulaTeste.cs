using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Infra;
using SME.SGP.Infra.Contexto;
using SME.SGP.Infra.Interfaces;
using SME.SGP.TesteIntegracao.ServicosFakes;
using SME.SGP.TesteIntegracao.ServicosFakes.Rabbit;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao
{
    public abstract class AulaTeste : TesteBase
    {
        private const string USUARIO_CHAVE = "NomeUsuario";
        private const string USUARIO_RF_CHAVE = "RF";
        private const string USUARIO_LOGIN_CHAVE = "login";

        private const string USUARIO_LOGADO_CHAVE = "UsuarioLogado";

        private const string USUARIO_CLAIMS_CHAVE = "Claims";

        private const string USUARIO_CLAIM_TIPO_RF = "rf";
        private const string USUARIO_CLAIM_TIPO_PERFIL = "perfil";

        private const string TURMA_CODIGO_1 = "1";
        private const string TURMA_NOME_1 = "Turma Nome 1";
        private const string TURMA_ANO_2 = "2";

        private const int ANO_LETIVO_2022_NUMERO = 2022;
        private const string ANO_LETIVO_2022_NOME = "Ano Letivo 2022";

        protected const int SEMESTRE_1 = 1;

        protected const long COMPONENTE_CURRICULAR_PORTUGUES_ID_138 = 138;
        private const string COMPONENTE_CURRICULAR_PORTUGUES_NOME = "Português";
        protected const long COMPONENTE_CURRICULAR_DESCONHECIDO_ID_999999 = 999999;
        private const string COMPONENTE_CURRICULAR_DESCONHECIDO_NOME = "Desconhecido";

        protected const long COMPONENTE_REG_CLASSE_SP_INTEGRAL_1A5_ANOS_ID_1213 = 1213;
        private const string COMPONENTE_REG_CLASSE_SP_INTEGRAL_1A5_ANOS_NOME = "'Regencia Classe SP Integral'";

        protected const long COMPONENTE_REG_CLASSE_EJA_ETAPA_ALFAB_ID_1113 = 1113;
        private const string COMPONENTE_REG_CLASSE_EJA_ETAPA_ALFAB_NOME = "'Regencia Classe EJA ALFAB'";

        private const string COMPONENTE_CURRICULAR = "componente_curricular";
        private const string COMPONENTE_CURRICULAR_AREA_CONHECIMENTO = "componente_curricular_area_conhecimento";
        private const string AREA_DE_CONHECIMENTO_1 = "'Área de conhecimento 1'";

        private const string COMPONENTE_CURRICULAR_GRUPO_MATRIZ = "componente_curricular_grupo_matriz";
        private const string GRUPO_MATRIZ_1 = "'Grupo matriz 1'";

        private const string CODIGO_1 = "1";

        private const string ED_INF_EMEI_4_HS = "'ED.INF. EMEI 4 HS'";
        private const string REGENCIA_CLASSE_INFANTIL = "'Regência de Classe Infantil'";
        private const string REGENCIA_INFATIL_EMEI_4H = "'REGÊNCIA INFANTIL EMEI 4H'";
        private const string FALSE = "false";
        private const string TRUE = "true";

        private const string UE_CODIGO_1 = "1";
        private const string UE_NOME_1 = "Nome da UE";

        private const string DRE_CODIGO_1 = "1";
        private const string DRE_NOME_1 = "DRE 1";

        private const string SISTEMA_NOME = "Sistema";
        private const string SISTEMA_CODIGO_RF = "1";

        private const string EVENTO_NOME_FESTA = "Festa";

        private const string USUARIO_PROFESSOR_LOGIN_2222222 = "2222222";
        private const string USUARIO_PROFESSOR_CODIGO_RF_2222222 = "2222222";
        private const string USUARIO_PROFESSOR_NOME_2222222 = "Nome do usuario 2222222";

        private const string USUARIO_PROFESSOR_LOGIN_1111111 = "1111111";
        private const string USUARIO_PROFESSOR_CODIGO_RF_1111111 = "1111111";
        private const string USUARIO_PROFESSOR_NOME_1111111 = "Nome do usuário 1111111";

        private const string PROFESSOR = "Professor";
        private const int ORDEM_290 = 290;

        private const string PROFESSOR_CJ = "Professor CJ";
        private const int ORDEM_320 = 320;

        private const int QUANTIDADE_3 = 3;

        protected const int BIMESTRE_1 = 1;
        protected const int BIMESTRE_2 = 2;
        protected const int BIMESTRE_3 = 3;
        protected const int BIMESTRE_4 = 4;
        protected AulaTeste(CollectionFixture collectionFixture) : base(collectionFixture)
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
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterComponentesCurricularesDoProfessorNaTurmaQuery, IEnumerable<ComponenteCurricularEol>>), typeof(ObterComponentesCurricularesDoProfessorNaTurmaQueryHandlerAulaFake), ServiceLifetime.Scoped));
        }

        protected async Task<RetornoBaseDto> InserirAulaUseCaseComValidacaoBasica(TipoAula tipoAula, RecorrenciaAula recorrenciaAula, long componentecurricularId, DateTime dataAula, bool ehRegente = false)
        {
             var retorno = await InserirAulaUseCaseSemValidacaoBasica(tipoAula, recorrenciaAula, componentecurricularId, dataAula, ehRegente);

            retorno.ShouldNotBeNull();

            var aulasCadastradas = ObterTodos<Aula>();

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

        protected async Task CriarDadosBasicosAula(string perfil, Modalidade modalidade, ModalidadeTipoCalendario tipoCalendario, DateTime dataInicio, DateTime dataFim, int bimestre, bool criarPeriodo = true, long tipoCalendarioId = 1)
        {
            await CriarTipoCalendario(tipoCalendario);
            await CriarItensComuns(criarPeriodo, dataInicio, dataFim, bimestre, tipoCalendarioId);
            CriarClaimUsuario(perfil);
            await CriarUsuarios();            
            await CriarTurma(modalidade);
        }

        protected async Task CriarDadosBasicosAula(string perfil, Modalidade modalidade, ModalidadeTipoCalendario tipoCalendario)
        {
            await CriarDadosBasicosAula(perfil, modalidade, tipoCalendario, new DateTime(2022, 05, 02), new DateTime(2022, 07, 08), SEMESTRE_1);
        }

        protected void CriarClaimUsuario(string perfil)
        {
            var contextoAplicacao = ServiceProvider.GetService<IContextoAplicacao>();
            var variaveis = new Dictionary<string, object>
            {
                { USUARIO_CHAVE, USUARIO_PROFESSOR_NOME_2222222 },
                { USUARIO_LOGADO_CHAVE, USUARIO_PROFESSOR_LOGIN_2222222 },
                { USUARIO_RF_CHAVE, USUARIO_PROFESSOR_LOGIN_2222222 },
                { USUARIO_LOGIN_CHAVE, USUARIO_PROFESSOR_LOGIN_2222222 },

                {
                   USUARIO_CLAIMS_CHAVE,
                    new List<InternalClaim> {
                        new InternalClaim { Value = USUARIO_PROFESSOR_LOGIN_2222222, Type = USUARIO_CLAIM_TIPO_RF },
                        new InternalClaim { Value = perfil, Type = USUARIO_CLAIM_TIPO_PERFIL }
                    }
                }
            };
            contextoAplicacao.AdicionarVariaveis(variaveis);
        }

        protected string ObterPerfilProfessor()
        {
            return Guid.Parse(PerfilUsuario.PROFESSOR.Name()).ToString();
        }

        protected string ObterPerfilCJ()
        {
            return Guid.Parse(PerfilUsuario.CJ.Name()).ToString();
        }

        protected string ObterPerfilCJInfantil()
        {
            return Guid.Parse(PerfilUsuario.CJ_INFANTIL.Name()).ToString();
        }

        protected string ObterPerfilCP()
        {
            return Guid.Parse(PerfilUsuario.CP.Name()).ToString();
        }

        protected string ObterPerfilAD()
        {
            return Guid.Parse(PerfilUsuario.AD.Name()).ToString();
        }

        protected string ObterPerfilDiretor()
        {
            return Guid.Parse(PerfilUsuario.DIRETOR.Name()).ToString();
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

        protected async Task CriarPeriodoEscolarEncerrado()
        {
            await InserirNaBase(new PeriodoEscolar
            {
                Id = 1,
                TipoCalendarioId = 1,
                Bimestre = BIMESTRE_2,
                PeriodoInicio = new DateTime(2022, 01, 10),
                PeriodoFim = new DateTime(2022, 02, 5),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now,
                Migrado = false
            });
        }

        protected async Task CrieEvento(EventoLetivo letivo, DateTime dataInicioEvento, DateTime dataFimEvento)
        {
            await InserirNaBase(new EventoTipo
            {
                Descricao = EVENTO_NOME_FESTA,
                Ativo = true,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });

            await InserirNaBase(new Evento
            {
                Nome = EVENTO_NOME_FESTA,
                TipoCalendarioId = 1,
                TipoEventoId = 1,
                UeId = UE_CODIGO_1,
                Letivo = letivo,
                DreId = DRE_CODIGO_1,
                DataInicio = dataInicioEvento,
                DataFim = dataFimEvento,
                Status = EntidadeStatus.Aprovado,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now,
                Migrado = false
            });
        }

        protected async Task CriarAtribuicaoEsporadica(DateTime dataInicio, DateTime dataFim)
        {
            await InserirNaBase(new AtribuicaoEsporadica
            {
                UeId = UE_CODIGO_1,
                ProfessorRf = USUARIO_PROFESSOR_LOGIN_2222222,
                AnoLetivo = ANO_LETIVO_2022_NUMERO,
                DreId = DRE_CODIGO_1,
                DataInicio = dataInicio,
                DataFim = dataFim,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now,
                Migrado = false
            });
        }

        protected async Task CriarAtribuicaoCJ(Modalidade modalidade, long componenteCurricularId, bool substituir = true)
        {
            await InserirNaBase(new AtribuicaoCJ
            {
                TurmaId = TURMA_CODIGO_1,
                DreId = DRE_CODIGO_1,
                UeId = UE_CODIGO_1,
                ProfessorRf = USUARIO_PROFESSOR_LOGIN_2222222,
                DisciplinaId = componenteCurricularId,
                Modalidade = modalidade,
                Substituir = substituir,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now,
                Migrado = false
            });
        }

        protected async Task CriarAula(string componenteCurricularCodigo, DateTime dataAula, RecorrenciaAula recorrencia, string rf = USUARIO_PROFESSOR_LOGIN_2222222)
        {
            await InserirNaBase(ObtenhaAula(componenteCurricularCodigo, dataAula, recorrencia, rf));
        }

        protected async Task CriaAulaRecorrentePortugues(RecorrenciaAula recorrencia)
        {
            var aula = ObtenhaAula(COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), new System.DateTime(2022, 02, 10), recorrencia, USUARIO_PROFESSOR_LOGIN_2222222);
            aula.AulaPaiId = 1;

            await InserirNaBase(aula);
        }

        protected async Task CriarUsuarios()
        {
            await InserirNaBase(new Usuario
            {
                Login = USUARIO_PROFESSOR_LOGIN_2222222,
                CodigoRf = USUARIO_PROFESSOR_CODIGO_RF_2222222,
                Nome = USUARIO_PROFESSOR_NOME_2222222,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new Usuario
            {
                Login = USUARIO_PROFESSOR_LOGIN_1111111,
                CodigoRf = USUARIO_PROFESSOR_CODIGO_RF_1111111,
                Nome = USUARIO_PROFESSOR_NOME_1111111,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }

        protected ExcluirAulaDto ObtenhaDto(RecorrenciaAula recorrencia)
        {
            return new ExcluirAulaDto()
            {
                AulaId = 1,
                RecorrenciaAula = recorrencia
            };
        }
        protected async Task CriarTurma(Modalidade modalidade)
        {
            await InserirNaBase(new Turma
            {
                UeId = 1,
                Ano = TURMA_ANO_2,
                CodigoTurma = TURMA_CODIGO_1,
                Historica = true,
                ModalidadeCodigo = modalidade,
                AnoLetivo = ANO_LETIVO_2022_NUMERO,
                Semestre = SEMESTRE_1,
                Nome = TURMA_NOME_1
            });
        }

        protected async Task CriarAtividadeAvaliativaFundamental(DateTime dataAvaliacao)
        {
            await InserirNaBase(new TipoAvaliacao
            {
                Id = 1,
                Nome = "Avaliação bimestral",
                Descricao = "Avaliação bimestral",
                Situacao = true,
                AvaliacoesNecessariasPorBimestre = 1,
                Codigo = TipoAvaliacaoCodigo.AvaliacaoBimestral,
                CriadoPor = "Sistema",
                CriadoRF = "1",
                CriadoEm = DateTime.Now
            });

            await InserirNaBase(new AtividadeAvaliativa
            {
                Id = 1,
                DreId = "1",
                UeId = "1",
                ProfessorRf = USUARIO_PROFESSOR_CODIGO_RF_2222222,
                TurmaId = TURMA_CODIGO_1,
                Categoria = CategoriaAtividadeAvaliativa.Normal,
                TipoAvaliacaoId = 1,
                NomeAvaliacao = "Avaliação 04",
                DescricaoAvaliacao = "Avaliação 04",
                DataAvaliacao = dataAvaliacao,
                CriadoPor = "Sistema",
                CriadoRF = "1",
                CriadoEm = DateTime.Now
            });

            await InserirNaBase(new AtividadeAvaliativaDisciplina
            {
                Id = 1,
                AtividadeAvaliativaId = 1,
                DisciplinaId = COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(),
                CriadoPor = "Sistema",
                CriadoRF = "1",
                CriadoEm = DateTime.Now
            });
        }

        protected async Task CriarTipoCalendario(ModalidadeTipoCalendario tipoCalendario)
        {
            await InserirNaBase(new TipoCalendario
            {
                AnoLetivo = ANO_LETIVO_2022_NUMERO,
                Nome = ANO_LETIVO_2022_NOME,
                Periodo = Periodo.Semestral,
                Modalidade = tipoCalendario,
                Situacao = true,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                Excluido = false,
                Migrado = false
            });
        }

        protected async Task CriarItensComuns(bool criarPeriodo, DateTime dataInicio, DateTime dataFim, int bimestre, long tipoCalendarioId = 1)
        {
            await CriarPadrao();
            if (criarPeriodo) await CriarPeriodoEscolar(dataInicio, dataFim, bimestre, tipoCalendarioId);
            await CriarComponenteCurricular();
        }

        public async Task CriarPadrao()
        {
            await InserirNaBase(new Dre
            {
                CodigoDre = DRE_CODIGO_1,
                Abreviacao = DRE_NOME_1,
                Nome = DRE_NOME_1
            });
            await InserirNaBase(new Ue
            {
                CodigoUe = UE_CODIGO_1,
                DreId = 1,
                Nome = UE_NOME_1,
            });

            await InserirNaBase(new PrioridadePerfil
            {
                CodigoPerfil = Guid.Parse(PerfilUsuario.PROFESSOR.Name()),
                NomePerfil = PROFESSOR,
                Ordem = ORDEM_290,
                Tipo = TipoPerfil.UE,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new PrioridadePerfil
            {
                CodigoPerfil = Guid.Parse(PerfilUsuario.CJ.Name()),
                NomePerfil = PROFESSOR_CJ,
                Ordem = ORDEM_320,
                Tipo = TipoPerfil.UE,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });
        }

        protected async Task CriarPeriodoEscolar(DateTime dataInicio, DateTime dataFim, int bimestre, long tipoCalendarioId = 1)
        {
            await InserirNaBase(new PeriodoEscolar
            {
                TipoCalendarioId = tipoCalendarioId,
                Bimestre = bimestre,
                PeriodoInicio = dataInicio,
                PeriodoFim = dataFim,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now,
                Migrado = false
            });
        }

        private async Task CriarComponenteCurricular()
        {
            await InserirNaBase(COMPONENTE_CURRICULAR_AREA_CONHECIMENTO, CODIGO_1, AREA_DE_CONHECIMENTO_1);

            await InserirNaBase(COMPONENTE_CURRICULAR_GRUPO_MATRIZ, CODIGO_1, GRUPO_MATRIZ_1);

            await InserirNaBase(COMPONENTE_CURRICULAR, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), CODIGO_1, CODIGO_1, ED_INF_EMEI_4_HS, FALSE, FALSE, TRUE, FALSE, FALSE, TRUE, REGENCIA_CLASSE_INFANTIL, REGENCIA_INFATIL_EMEI_4H);

            await InserirNaBase(COMPONENTE_CURRICULAR, COMPONENTE_REG_CLASSE_SP_INTEGRAL_1A5_ANOS_ID_1213.ToString(), COMPONENTE_REG_CLASSE_SP_INTEGRAL_1A5_ANOS_ID_1213.ToString(), CODIGO_1, CODIGO_1, COMPONENTE_REG_CLASSE_SP_INTEGRAL_1A5_ANOS_NOME, FALSE, FALSE, TRUE, FALSE, FALSE, TRUE, COMPONENTE_REG_CLASSE_SP_INTEGRAL_1A5_ANOS_NOME, COMPONENTE_REG_CLASSE_SP_INTEGRAL_1A5_ANOS_NOME);
        }

        private Aula ObtenhaAula(string componenteCurricularCodigo, DateTime dataAula, RecorrenciaAula recorrencia, string rf = USUARIO_PROFESSOR_LOGIN_2222222)
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
    }
}
