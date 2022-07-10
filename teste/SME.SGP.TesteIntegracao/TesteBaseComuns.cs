﻿using Microsoft.Extensions.DependencyInjection;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Infra;
using SME.SGP.Infra.Contexto;
using SME.SGP.Infra.Interfaces;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao
{
    public abstract class TesteBaseComuns : TesteBase
    {
        private const string USUARIO_CHAVE = "NomeUsuario";
        private const string USUARIO_RF_CHAVE = "RF";
        private const string USUARIO_LOGIN_CHAVE = "login";

        private const string USUARIO_LOGADO_CHAVE = "UsuarioLogado";

        private const string USUARIO_CLAIMS_CHAVE = "Claims";

        private const string USUARIO_CLAIM_TIPO_RF = "rf";
        private const string USUARIO_CLAIM_TIPO_PERFIL = "perfil";

        protected const string TURMA_CODIGO_1 = "1";
        private const string TURMA_NOME_1 = "Turma Nome 1";

        protected const string TURMA_CODIGO_2 = "2";
        protected const string TURMA_CODIGO_3 = "3";
        private const string TURMA_NOME_2 = "Turma Nome 2";

        protected const string TURMA_ANO_2 = "2";
        protected const string TURMA_ANO_3 = "3";

        private int ANO_LETIVO_Ano_Atual_NUMERO = DateTimeExtension.HorarioBrasilia().Year;
        private const string ANO_LETIVO_Ano_Atual_NOME = "Ano Letivo Ano Atual";
        protected const string FALSE = "false";
        protected const string TRUE = "true";

        protected const int SEMESTRE_1 = 1;
        protected const long COMPONENTE_CURRICULAR_PORTUGUES_ID_139 = 139;
        protected const long COMPONENTE_CURRICULAR_PORTUGUES_ID_138 = 138;
        protected const string COMPONENTE_CURRICULAR_LINGUA_PORTUGUESA_NOME = "'Língua Portuguesa'";
        protected const string COMPONENTE_CURRICULAR_PORTUGUES_NOME = "Língua Portuguesa";
        protected const long COMPONENTE_CURRICULAR_DESCONHECIDO_ID_999999 = 999999;
        protected const string COMPONENTE_CURRICULAR_DESCONHECIDO_NOME = "Desconhecido";

        protected const string COMPONENTE_CURRICULAR_MATEMATICA_NOME = "'MATEMATICA'";

        protected const long COMPONENTE_REGENCIA_CLASSE_FUND_I_5H_ID_1105 = 1105;
        protected const string COMPONENTE_REGENCIA_CLASSE_FUND_I_5H_NOME_1105 = "'Regência de Classe Fund I - 5H'";
        protected const string COMPONENTE_REG_CLASSE_CICLO_ALFAB_INTERD_5HRS_EOL_1105 = "'REG CLASSE CICLO ALFAB / INTERD 5HRS'";


        protected const long COMPONENTE_REGENCIA_CLASSE_EJA_BASICA_ID_1114 = 1114;
        protected const string COMPONENTE_REGENCIA_CLASSE_EJA_BASICA_NOME_1114 = "'Regência de Classe EJA - Básica'";
        protected const string COMPONENTE_REG_CLASSE_EJA_ETAPA_BASICA_EOL_1114 = "'REG CLASSE EJA ETAPA BASICA'";

        protected const long COMPONENTE_REG_CLASSE_SP_INTEGRAL_1A5_ANOS_ID_1213 = 1213;
        protected const string COMPONENTE_REG_CLASSE_SP_INTEGRAL_1A5_ANOS_NOME = "'Regencia Classe SP Integral'";
        protected const string COMPONENTE_REG_CLASSE_SP_INTEGRAL_1A5_ANOS_EOL = "'REG CLASSE SP INTEGRAL 1A5 ANOS'";

        protected const long COMPONENTE_REG_CLASSE_EJA_ETAPA_ALFAB_ID_1113 = 1113;
        protected const string COMPONENTE_REG_CLASSE_EJA_ETAPA_ALFAB_NOME = "'Regencia Classe EJA ALFAB'";

        protected const long COMPONENTE_CURRICULAR_AULA_COMPARTILHADA = 1123;
        protected const string COMPONENTE_CURRICULAR_AULA_COMPARTILHADA_NOME = "'AULA COMPARTILHADA'";

        protected const long COMPONENTE_CURRICULAR_AEE_COLABORATIVO = 1103;
        protected const string COMPONENTE_CURRICULAR_AEE_COLABORATIVO_NOME = "'AEE COLABORATIVO'";

        private const string COMPONENTE_CURRICULAR = "componente_curricular";
        private const string COMPONENTE_CURRICULAR_AREA_CONHECIMENTO = "componente_curricular_area_conhecimento";
        private const string AREA_DE_CONHECIMENTO_1 = "'Área de conhecimento 1'";
        private const string AREA_DE_CONHECIMENTO_8 = "'Área de conhecimento 8'";
        private const string AREA_DE_CONHECIMENTO_2 = "'Área de conhecimento 2'";

        protected const string COMPONENTE_CIENCIAS_ID_89 = "89";
        protected const string COMPONENTE_GEOGRAFIA_ID_8 = "8";
        protected const string COMPONENTE_GEOGRAFIA_NOME = "'Geografia'";
        protected const string COMPONENTE_HISTORIA_ID_7 = "7";
        protected const string COMPONENTE_LINGUA_PORTUGUESA_ID_138 = "138";
        protected const string COMPONENTE_MATEMATICA_ID_2 = "2";

        private const string COMPONENTE_CURRICULAR_GRUPO_MATRIZ = "componente_curricular_grupo_matriz";
        private const string GRUPO_MATRIZ_1 = "'Grupo matriz 1'";
        private const string GRUPO_MATRIZ_8 = "'Grupo matriz 8'";

        private const string CODIGO_1 = "1";
        private const string CODIGO_2 = "2";
        private const string CODIGO_8 = "8";
        private const string NULO = "null";

        protected const string PROVA = "Prova";
        protected const string TESTE = "Teste";

        private const string ED_INF_EMEI_4_HS = "'ED.INF. EMEI 4 HS'";
        private const string REGENCIA_CLASSE_INFANTIL = "'Regência de Classe Infantil'";
        private const string REGENCIA_INFATIL_EMEI_4H = "'REGÊNCIA INFANTIL EMEI 4H'";

        protected const string UE_CODIGO_1 = "1";
        private const string UE_NOME_1 = "Nome da UE";

        protected const string DRE_CODIGO_1 = "1";
        protected const string DRE_NOME_1 = "DRE 1";

        protected const string SISTEMA_NOME = "Sistema";
        protected const string SISTEMA_CODIGO_RF = "1";

        private const string EVENTO_NOME_FESTA = "Festa";

        protected const string USUARIO_PROFESSOR_LOGIN_2222222 = "2222222";
        protected const string USUARIO_PROFESSOR_CODIGO_RF_2222222 = "2222222";
        private const string USUARIO_PROFESSOR_NOME_2222222 = "Nome do usuario 2222222";

        protected const string USUARIO_PROFESSOR_LOGIN_1111111 = "1111111";
        protected const string USUARIO_PROFESSOR_CODIGO_RF_1111111 = "1111111";
        private const string USUARIO_PROFESSOR_NOME_1111111 = "Nome do usuário 1111111";

        private const string PROFESSOR = "Professor";
        private const int ORDEM_290 = 290;

        private const string PROFESSOR_CJ = "Professor CJ";
        private const int ORDEM_320 = 320;

        protected const int BIMESTRE_1 = 1;
        protected const int BIMESTRE_2 = 2;
        protected const int BIMESTRE_3 = 3;
        protected const int BIMESTRE_4 = 4;

        protected const string EVENTO_NAO_LETIVO = "Evento não letivo";
        protected const long TIPO_EVENTO_21 = 21;
        protected const long TIPO_EVENTO_1 = 1;
        protected const long TIPO_EVENTO_2 = 2;
        protected const long TIPO_EVENTO_13 = 13;
        protected const long TIPO_EVENTO_14 = 14;
        protected const string SUSPENSAO_DE_ATIVIDADES = "Suspensão de Atividades";
        protected const string REPOSICAO_AULA = "Reposição de Aula";
        protected const string REPOSICAO_DIA = "Reposição Dia";
        protected const string REPOSICAO_AULA_DE_GREVE = "Reposição de Aula de Greve";
        protected const string LIBERACAO_EXCEPCIONAL = "Liberação excepcional";
        protected const int TIPO_CALENDARIO_ID = 1;

        protected readonly DateTime DATA_03_01 = new(DateTimeExtension.HorarioBrasilia().Year, 01, 03);
        protected readonly DateTime DATA_29_04 = new(DateTimeExtension.HorarioBrasilia().Year, 04, 29);

        protected readonly DateTime DATA_02_05 = new(DateTimeExtension.HorarioBrasilia().Year, 05, 02);
        protected readonly DateTime DATA_08_07 = new(DateTimeExtension.HorarioBrasilia().Year, 07, 08);

        protected readonly DateTime DATA_25_07 = new(DateTimeExtension.HorarioBrasilia().Year, 07, 25);
        protected readonly DateTime DATA_30_09 = new(DateTimeExtension.HorarioBrasilia().Year, 09, 30);

        protected readonly DateTime DATA_03_10 = new(DateTimeExtension.HorarioBrasilia().Year, 10, 03);
        protected readonly DateTime DATA_22_12 = new(DateTimeExtension.HorarioBrasilia().Year, 12, 22);

        protected readonly DateTime DATA_01_01 = new(DateTimeExtension.HorarioBrasilia().Year, 01, 01);

        protected readonly DateTime DATA_31_12 = new(DateTimeExtension.HorarioBrasilia().Year, 12, 31);

        protected readonly DateTime DATA_10_01 = new(DateTimeExtension.HorarioBrasilia().Year, 01, 10);

        protected const int AULA_ID = 1;
        protected const int QUANTIDADE_AULA = 1;
        protected const int QUANTIDADE_AULA_2 = 2;
        protected const int QUANTIDADE_AULA_3 = 3;
        protected const int QUANTIDADE_AULA_4 = 4;
        protected const string CODIGO_ALUNO_1 = "1";
        protected const string CODIGO_ALUNO_2 = "2";
        protected const string CODIGO_ALUNO_3 = "3";
        protected const string CODIGO_ALUNO_4 = "4";

        protected DateTime DATA_01_02_INICIO_BIMESTRE_1 = new(DateTimeExtension.HorarioBrasilia().Year, 02, 01);
        protected DateTime DATA_25_04_FIM_BIMESTRE_1 = new(DateTimeExtension.HorarioBrasilia().Year, 04, 25);
        protected DateTime DATA_02_05_INICIO_BIMESTRE_2 = new(DateTimeExtension.HorarioBrasilia().Year, 05, 02);
        protected DateTime DATA_08_07_FIM_BIMESTRE_2 = new(DateTimeExtension.HorarioBrasilia().Year, 07, 08);
        protected DateTime DATA_25_07_INICIO_BIMESTRE_3 = new(DateTimeExtension.HorarioBrasilia().Year, 07, 25);
        protected DateTime DATA_30_09_FIM_BIMESTRE_3 = new(DateTimeExtension.HorarioBrasilia().Year, 09, 30);
        protected DateTime DATA_03_10_INICIO_BIMESTRE_4 = new(DateTimeExtension.HorarioBrasilia().Year, 10, 03);
        protected DateTime DATA_22_12_FIM_BIMESTRE_4 = new(DateTimeExtension.HorarioBrasilia().Year, 12, 22);
        protected const string REABERTURA_GERAL = "Reabrir Geral";
        protected DateTime DATA_INICIO_BIMESTRE_1 = new(DateTimeExtension.HorarioBrasilia().Year, 05, 02);
        protected DateTime DATA_FIM_BIMESTRE_1 = new(DateTimeExtension.HorarioBrasilia().Year, 07, 08);
        protected DateTime DATA_INICIO_BIMESTRE_2 = new(DateTimeExtension.HorarioBrasilia().Year, 05, 02);
        protected DateTime DATA_FIM_BIMESTRE_2 = new(DateTimeExtension.HorarioBrasilia().Year, 07, 08);
        protected DateTime DATA_INICIO_BIMESTRE_3 = new(DateTimeExtension.HorarioBrasilia().Year, 07, 25);
        protected DateTime DATA_FIM_BIMESTRE_3 = new(DateTimeExtension.HorarioBrasilia().Year, 09, 30);
        protected DateTime DATA_INICIO_BIMESTRE_4 = new(DateTimeExtension.HorarioBrasilia().Year, 10, 03);
        protected DateTime DATA_FIM_BIMESTRE_4 = new(DateTimeExtension.HorarioBrasilia().Year, 12, 22);
        protected long TIPO_CALENDARIO_1 = 1;

        protected const long ATESTADO_MEDICO_DO_ALUNO_1 = 1;
        protected const long ATESTADO_MEDICO_DE_PESSOA_DA_FAMILIA_2 = 2;
        protected const long DOENCA_NA_FAMILIA_SEM_ATESTADO_3 = 3;
        protected const long OBITO_DE_PESSOA_DA_FAMILIA_4 = 4;
        protected const long INEXISTENCIA_DE_PESSOA_PARA_LEVAR_A_ESCOLA_5 = 5;
        protected const long ENCHENTE_6 = 6;
        protected const long FALTA_DE_TRANSPORTE_7 = 7;
        protected const long VIOLENCIA_NA_AREA_ONDE_MORA_8 = 8;
        protected const long CALAMIDADE_PUBLICA_QUE_ATINGIU_A_ESCOLA_OU_EXIGIU_O_USO_DO_ESPAÇO_COMO_ABRIGAMENTO_9 = 9;
        protected const long ESCOLA_FECHADA_POR_SITUACAO_DE_VIOLENCIA_10 = 10;

        protected const string DESCRICAO_FREQUENCIA_ALUNO_1 = "Lorem Ipsum";
        protected const string DESCRICAO_FREQUENCIA_ALUNO_2 = "é um texto bastante conhecido";

        protected readonly CollectionFixture collectionFixture;

        protected TesteBaseComuns(CollectionFixture collectionFixture) : base(collectionFixture)
        {
            this.collectionFixture = collectionFixture ?? throw new ArgumentNullException(nameof(collectionFixture));
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

        protected string ObterPerfilProfessorInfantil()
        {
            return Guid.Parse(PerfilUsuario.PROFESSOR_INFANTIL.Name()).ToString();
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
        protected async Task CriarPeriodoEscolarEncerrado()
        {
            await InserirNaBase(new PeriodoEscolar
            {
                Id = 1,
                TipoCalendarioId = 1,
                Bimestre = BIMESTRE_2,
                PeriodoInicio = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 01, 10),
                PeriodoFim = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 02, 5),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now,
                Migrado = false
            });
        }

        protected async Task CriarEvento(EventoLetivo letivo, DateTime dataInicioEvento, DateTime dataFimEvento)
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

        protected async Task CriarEventoTipoResumido(string descricao, EventoLocalOcorrencia localOcorrencia, bool concomitancia, EventoTipoData tipoData, bool dependencia, EventoLetivo letivo, long codigo)
        {
            await CriarEventoTipo(descricao, localOcorrencia, concomitancia, tipoData, dependencia, letivo, true, false, codigo, false, false);
        }

        protected async Task CriarEventoTipo(string descricao, EventoLocalOcorrencia localOcorrencia, bool concomitancia, EventoTipoData tipoData, bool dependencia, EventoLetivo letivo, bool ativo, bool excluido, long codigo, bool somenteLeitura, bool eventoEscolaAqui)
        {
            await InserirNaBase(new EventoTipo()
            {
                Descricao = descricao,
                LocalOcorrencia = localOcorrencia,
                Concomitancia = concomitancia,
                TipoData = tipoData,
                Dependencia = dependencia,
                Letivo = letivo,
                Ativo = ativo,
                Excluido = excluido,
                Codigo = codigo,
                SomenteLeitura = somenteLeitura,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME
            });
        }

        protected async Task CriarEventoResumido(string nomeEvento, DateTime dataInicio, DateTime dataFim, EventoLetivo eventoLetivo, long tipoCalendarioId, long tipoEventoId, string dreId, string ueId, EntidadeStatus eventoStatus)
        {
            await CriarEvento(nomeEvento, dataInicio, dataFim, eventoLetivo, tipoCalendarioId, tipoEventoId, dreId, ueId, eventoStatus, null, null, null, null);
        }

        protected async Task CriarEvento(string nomeEvento, DateTime dataInicio, DateTime dataFim, EventoLetivo eventoLetivo, long tipoCalendarioId, long tipoEventoId, string dreId, string ueId, EntidadeStatus eventoStatus, long? workflowAprovacaoId, TipoPerfil? tipoPerfil, long? eventoPaiId, long? feriadoId, bool migrado = false)
        {
            await InserirNaBase(new Evento
            {
                Nome = nomeEvento,
                DataInicio = dataInicio,
                DataFim = dataFim,
                Letivo = eventoLetivo,
                TipoCalendarioId = tipoCalendarioId,
                TipoEventoId = tipoEventoId,
                DreId = dreId,
                UeId = ueId,
                CriadoEm = DateTime.Now,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                Excluido = false,
                Status = eventoStatus,
                WorkflowAprovacaoId = workflowAprovacaoId,
                Migrado = migrado,
                TipoPerfilCadastro = tipoPerfil,
                EventoPaiId = eventoPaiId,
                FeriadoId = feriadoId
            });
        }

        protected async Task CriarAtribuicaoEsporadica(DateTime dataInicio, DateTime dataFim)
        {
            await InserirNaBase(new AtribuicaoEsporadica
            {
                UeId = UE_CODIGO_1,
                ProfessorRf = USUARIO_PROFESSOR_LOGIN_2222222,
                AnoLetivo = ANO_LETIVO_Ano_Atual_NUMERO,
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


        protected async Task CriarTurma(Modalidade modalidade)
        {
            await InserirNaBase(new Turma
            {
                UeId = 1,
                Ano = TURMA_ANO_2,
                CodigoTurma = TURMA_CODIGO_1,
                Historica = true,
                ModalidadeCodigo = modalidade,
                AnoLetivo = ANO_LETIVO_Ano_Atual_NUMERO,
                Semestre = SEMESTRE_1,
                Nome = TURMA_NOME_1
            });
        }


        protected async Task CriarTurma_Exclusao(Modalidade modalidade)
        {
            await InserirNaBase(new Turma
            {
                UeId = 1,
                Ano = TURMA_ANO_3,
                CodigoTurma = TURMA_CODIGO_3,
                Historica = true,
                ModalidadeCodigo = modalidade,
                AnoLetivo = ANO_LETIVO_Ano_Atual_NUMERO,
                Semestre = SEMESTRE_1,
                Nome = TURMA_NOME_1
            });
        }
        protected async Task CriarTurma(Modalidade modalidade, string turmaCodigo)
        {
            await InserirNaBase(new Turma
            {
                UeId = 1,
                Ano = TURMA_ANO_2,
                CodigoTurma = turmaCodigo,
                Historica = true,
                ModalidadeCodigo = modalidade,
                AnoLetivo = ANO_LETIVO_Ano_Atual_NUMERO,
                Semestre = SEMESTRE_1,
                Nome = TURMA_NOME_1
            });
        }

        protected async Task CriaTipoAvaliacao(TipoAvaliacaoCodigo tipoAvalicao)
        {
            await InserirNaBase(new TipoAvaliacao
            {
                Id = 1,
                Nome = "Avaliação bimestral",
                Descricao = "Avaliação bimestral",
                Situacao = true,
                AvaliacoesNecessariasPorBimestre = 1,
                Codigo = tipoAvalicao,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });
        }

        protected async Task CriarAtividadeAvaliativaFundamental(
                                    DateTime dataAvaliacao,
                                    string componente,
                                    TipoAvaliacaoCodigo tipoAvalicao = TipoAvaliacaoCodigo.AvaliacaoBimestral,
                                    bool ehRegencia = false,
                                    bool ehCj = false,
                                    string rf = USUARIO_PROFESSOR_CODIGO_RF_2222222)
        {
            await CriaTipoAvaliacao(tipoAvalicao);

            await InserirNaBase(new AtividadeAvaliativa
            {
                Id = 1,
                DreId = "1",
                UeId = "1",
                ProfessorRf = rf,
                TurmaId = TURMA_CODIGO_1,
                Categoria = CategoriaAtividadeAvaliativa.Normal,
                TipoAvaliacaoId = 1,
                NomeAvaliacao = "Avaliação 04",
                DescricaoAvaliacao = "Avaliação 04",
                DataAvaliacao = dataAvaliacao,
                EhRegencia = ehRegencia,
                EhCj = ehCj,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });

            await InserirNaBase(new AtividadeAvaliativaDisciplina
            {
                Id = 1,
                AtividadeAvaliativaId = 1,
                DisciplinaId = componente,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });
        }

        protected async Task CriarAtividadeAvaliativaRegencia(string componente, string nomeComponente)
        {

            await InserirNaBase(new AtividadeAvaliativaRegencia
            {
                Id = 1,
                AtividadeAvaliativaId = 1,
                DisciplinaContidaRegenciaId = componente,
                DisciplinaContidaRegenciaNome = nomeComponente,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });
        }

        protected async Task CriarTipoCalendario(ModalidadeTipoCalendario tipoCalendario)
        {
            await InserirNaBase(new TipoCalendario
            {
                AnoLetivo = ANO_LETIVO_Ano_Atual_NUMERO,
                Nome = ANO_LETIVO_Ano_Atual_NOME,
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

        protected async Task CriarPadrao()
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

        protected async Task CriarComponenteCurricular()
        { 
            await InserirNaBase(COMPONENTE_CURRICULAR_AREA_CONHECIMENTO, CODIGO_1, AREA_DE_CONHECIMENTO_1);

            await InserirNaBase(COMPONENTE_CURRICULAR_GRUPO_MATRIZ, CODIGO_1, GRUPO_MATRIZ_1);

            await InserirNaBase(COMPONENTE_CURRICULAR_AREA_CONHECIMENTO, CODIGO_8, AREA_DE_CONHECIMENTO_8);

            await InserirNaBase(COMPONENTE_CURRICULAR_AREA_CONHECIMENTO, CODIGO_2, AREA_DE_CONHECIMENTO_2);

            await InserirNaBase(COMPONENTE_CURRICULAR_GRUPO_MATRIZ, CODIGO_8, GRUPO_MATRIZ_8);

            await InserirNaBase(COMPONENTE_CURRICULAR, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), NULO, CODIGO_1, CODIGO_1, COMPONENTE_CURRICULAR_LINGUA_PORTUGUESA_NOME, FALSE, FALSE, FALSE, TRUE, TRUE, TRUE, COMPONENTE_CURRICULAR_LINGUA_PORTUGUESA_NOME, NULO);

            await InserirNaBase(COMPONENTE_CURRICULAR, COMPONENTE_MATEMATICA_ID_2.ToString(), NULO, CODIGO_1, CODIGO_2, COMPONENTE_CURRICULAR_MATEMATICA_NOME, FALSE, FALSE, FALSE, TRUE, TRUE, TRUE, COMPONENTE_CURRICULAR_MATEMATICA_NOME, NULO);

            await InserirNaBase(COMPONENTE_CURRICULAR, COMPONENTE_GEOGRAFIA_ID_8.ToString(), NULO, CODIGO_1, CODIGO_1, COMPONENTE_GEOGRAFIA_NOME, FALSE, FALSE, FALSE, TRUE, TRUE, TRUE, COMPONENTE_GEOGRAFIA_NOME, NULO);

            await InserirNaBase(COMPONENTE_CURRICULAR, COMPONENTE_REG_CLASSE_SP_INTEGRAL_1A5_ANOS_ID_1213.ToString(), NULO, CODIGO_1, NULO, COMPONENTE_REG_CLASSE_SP_INTEGRAL_1A5_ANOS_EOL, TRUE, FALSE, FALSE, FALSE, TRUE, TRUE, COMPONENTE_REG_CLASSE_SP_INTEGRAL_1A5_ANOS_NOME, NULO);

            await InserirNaBase(COMPONENTE_CURRICULAR, COMPONENTE_REGENCIA_CLASSE_FUND_I_5H_ID_1105.ToString(), NULO, CODIGO_1, NULO, COMPONENTE_REG_CLASSE_CICLO_ALFAB_INTERD_5HRS_EOL_1105, TRUE, FALSE, FALSE, FALSE, TRUE, TRUE, COMPONENTE_REGENCIA_CLASSE_FUND_I_5H_NOME_1105, NULO);

            await InserirNaBase(COMPONENTE_CURRICULAR, COMPONENTE_REGENCIA_CLASSE_EJA_BASICA_ID_1114.ToString(), NULO, CODIGO_1, CODIGO_8, COMPONENTE_REG_CLASSE_EJA_ETAPA_BASICA_EOL_1114, TRUE, FALSE, FALSE, FALSE, TRUE, TRUE, COMPONENTE_REGENCIA_CLASSE_EJA_BASICA_NOME_1114, NULO);

            await InserirNaBase(COMPONENTE_CURRICULAR, COMPONENTE_CURRICULAR_AULA_COMPARTILHADA.ToString(), COMPONENTE_CURRICULAR_AULA_COMPARTILHADA.ToString(), CODIGO_1, CODIGO_1, COMPONENTE_CURRICULAR_AULA_COMPARTILHADA_NOME, FALSE, TRUE, FALSE, FALSE, FALSE, TRUE, COMPONENTE_CURRICULAR_AULA_COMPARTILHADA_NOME, COMPONENTE_CURRICULAR_AULA_COMPARTILHADA_NOME);

            await InserirNaBase(COMPONENTE_CURRICULAR, COMPONENTE_CURRICULAR_AEE_COLABORATIVO.ToString(), COMPONENTE_CURRICULAR_AEE_COLABORATIVO.ToString(), CODIGO_1, CODIGO_1, COMPONENTE_CURRICULAR_AEE_COLABORATIVO_NOME, FALSE, TRUE, FALSE, FALSE, FALSE, TRUE, COMPONENTE_CURRICULAR_AEE_COLABORATIVO_NOME, COMPONENTE_CURRICULAR_AEE_COLABORATIVO_NOME);

        }

        protected async Task CriarAula(DateTime dataAula, RecorrenciaAula recorrenciaAula, TipoAula tipoAula, string professorRf, string turmaCodigo, string ueCodigo, string disciplinaCodigo, long tipoCalendarioId, bool aulaCJ = false)
        {
            await InserirNaBase(new Aula()
            {
                UeId = ueCodigo,
                DisciplinaId = disciplinaCodigo,
                TurmaId = turmaCodigo,
                TipoCalendarioId = tipoCalendarioId,
                ProfessorRf = professorRf,
                Quantidade = 1,
                DataAula = dataAula,
                RecorrenciaAula = recorrenciaAula,
                TipoAula = tipoAula,
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                Excluido = false,
                AulaCJ = aulaCJ
            });
        }

        protected async Task CriarPeriodoEscolarReabertura(long tipoCalendarioId)
        {
            await CriarPeriodoEscolar(DATA_03_01, DATA_29_04, BIMESTRE_1, tipoCalendarioId);
            await CriarPeriodoEscolar(DATA_02_05, DATA_08_07, BIMESTRE_2, tipoCalendarioId);
            await CriarPeriodoEscolar(DATA_25_07, DATA_30_09, BIMESTRE_3, tipoCalendarioId);
            await CriarPeriodoEscolar(DATA_03_10, DATA_22_12, BIMESTRE_4, tipoCalendarioId);

            await CriarPeriodoReabertura(tipoCalendarioId);
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
