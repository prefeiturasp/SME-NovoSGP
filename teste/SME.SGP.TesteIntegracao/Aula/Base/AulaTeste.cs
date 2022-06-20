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

        private const int SEMESTRE_2 = 2;
        
        private const long COMPONENTE_CURRICULAR_PORTUGUES_ID_1106 = 1106;
        private const string COMPONENTE_CURRICULAR_PORTUGUES_1106_CODIGO = "1106";
        private const string COMPONENTE_CURRICULAR_PORTUGUES_NOME = "Português";

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

        private const int BIMESTRE_2 = 2;

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
        }

        protected async Task<RetornoBaseDto> ValidarInserirAulaUseCaseBasico(TipoAula tipoAula, RecorrenciaAula recorrenciaAula, bool ehRegente = false)
        {
            var useCase = ServiceProvider.GetService<IInserirAulaUseCase>();
            var aula = ObterAula(tipoAula, recorrenciaAula);
            if (ehRegente) aula.EhRegencia = true;

            var retorno = await useCase.Executar(aula);

            retorno.ShouldNotBeNull();

            var aulasCadastradas = ObterTodos<Aula>();

            aulasCadastradas.ShouldNotBeEmpty();
            aulasCadastradas.Count().ShouldBeGreaterThanOrEqualTo(1);

            return retorno;
        }

        protected async Task CriarDadosBasicosAula(string perfil, Modalidade modalidade, ModalidadeTipoCalendario tipoCalendario, bool criarPeriodo = true)
        {
            await CriarItensComuns(criarPeriodo);
            CriarClaimUsuario(perfil);
            await CriarUsuarios();
            await CriarTipoCalendario(tipoCalendario);
            await CriarTurma(modalidade);
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

        protected PersistirAulaDto ObterAula(TipoAula tipoAula, RecorrenciaAula recorrenciaAula)
        {
            return new PersistirAulaDto()
            {
                CodigoTurma = TURMA_CODIGO_1,
                Quantidade = 1,
                TipoAula = tipoAula,
                DataAula = new DateTime(2022, 02, 10),
                DisciplinaCompartilhadaId = 1106,
                CodigoUe = UE_CODIGO_1,
                RecorrenciaAula = recorrenciaAula,
                CodigoComponenteCurricular = COMPONENTE_CURRICULAR_PORTUGUES_ID_1106,
                NomeComponenteCurricular = COMPONENTE_CURRICULAR_PORTUGUES_NOME,
                TipoCalendarioId = 1
            };
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

        protected async Task CrieEvento(EventoLetivo letivo)
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
                DataInicio = new DateTime(2022, 02, 10),
                DataFim = new DateTime(2022, 02, 10),
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

        protected async Task CriarAtribuicaoCJ(Modalidade modalidade)
        {
            await InserirNaBase(new AtribuicaoCJ
            {
                TurmaId = TURMA_CODIGO_1,
                DreId =DRE_CODIGO_1,
                UeId = UE_CODIGO_1,
                ProfessorRf = USUARIO_PROFESSOR_LOGIN_2222222,
                DisciplinaId = COMPONENTE_CURRICULAR_PORTUGUES_ID_1106,
                Modalidade = modalidade,
                Substituir = true,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now,
                Migrado = false
            });
        }

        protected async Task CriarAula(string rf = USUARIO_PROFESSOR_LOGIN_2222222)
        {
            await InserirNaBase(new Aula
            {
                UeId = UE_CODIGO_1,
                DisciplinaId = COMPONENTE_CURRICULAR_PORTUGUES_1106_CODIGO.ToString(),
                TurmaId = TURMA_CODIGO_1,
                TipoCalendarioId = 1,
                ProfessorRf = rf,
                Quantidade = QUANTIDADE_3,
                DataAula = new DateTime(2022, 02, 10),
                RecorrenciaAula = 0,
                TipoAula = TipoAula.Normal,
                CriadoEm = new DateTime(2022, 02, 10),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                Excluido = false,
                Migrado = false,
                AulaCJ = false
            });
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

        private async Task CriarTurma(Modalidade modalidade)
        {
            await InserirNaBase(new Turma
            {
                UeId = 1,
                Ano = TURMA_ANO_2,
                CodigoTurma = TURMA_CODIGO_1,
                Historica = true,
                ModalidadeCodigo = modalidade,
                AnoLetivo = ANO_LETIVO_2022_NUMERO,
                Semestre = SEMESTRE_2,
                Nome = TURMA_NOME_1
            });
        }

        private async Task CriarTipoCalendario(ModalidadeTipoCalendario tipoCalendario)
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

        private async Task CriarItensComuns(bool criarPeriodo)
        {
            await CriarPadrao();
            if (criarPeriodo) await CriarPeriodoEscolar();
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

        private async Task CriarPeriodoEscolar()
        {
            await InserirNaBase(new PeriodoEscolar
            {
                TipoCalendarioId = 1,
                Bimestre = SEMESTRE_2,
                PeriodoInicio = new DateTime(2022, 01, 10),
                PeriodoFim = DateTime.Now.AddYears(1),
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

            await InserirNaBase(COMPONENTE_CURRICULAR, COMPONENTE_CURRICULAR_PORTUGUES_1106_CODIGO, COMPONENTE_CURRICULAR_PORTUGUES_1106_CODIGO, CODIGO_1, CODIGO_1, ED_INF_EMEI_4_HS, FALSE, FALSE, TRUE, FALSE, FALSE, TRUE,REGENCIA_CLASSE_INFANTIL,REGENCIA_INFATIL_EMEI_4H);
        }
    }
}
