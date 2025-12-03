using Microsoft.Extensions.DependencyInjection;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.EncaminhamentoNAAPA
{
    public class EncaminhamentoNAAPATesteBase : TesteBaseComuns
    {
        protected const string MOTIVO_ENCERRAMENTO = "Motivo do encerramento do encaminhamento NAAPA";
        protected const int NORMAL = 1;
        protected const int PRIORITARIA = 2;
        protected const string NOME_ALUNO_1 = "Nome do aluno 1";
        protected const long ID_QUESTAO_DATA_ENTRADA_QUEIXA = 1;
        protected const long ID_QUESTAO_ENDERECO_RESIDENCIAL = 12;
        protected const long ID_QUESTAO_TURMAS_PROGRAMA = 13;
        protected const long ID_QUESTAO_AGRUPAMENTO_PROMOCAO_CUIDADOS = 3;
        protected const long ID_QUESTAO_PRIORIDADE = 2;
        protected const long ID_QUESTAO_OBS_AGRUPAMENTO_PROMOCAO_CUIDADOS = 6;
        protected const long ID_QUESTAO_TIPO_ADOECE_COM_FREQUENCIA = 4;
        protected const long ID_QUESTAO_TIPO_DOENCA_CRONICA = 5;

        protected const long ID_QUESTAO_DATA_ATENDIMENTO = 7;
        protected const long ID_QUESTAO_TIPO_ATENDIMENTO = 8;
        protected const long ID_QUESTAO_PROCEDIMENTO_TRABALHO = 9;
        protected const long ID_QUESTAO_DESCRICAO_ATENDIMENTO = 10;
        protected const long ID_QUESTAO_DESCRICAO_PROCEDIMENTO_TRABALHO = 11;

        protected const long ID_QUESTAO_ESTUDANTE_MIGRANTE = 35;
        protected const long ID_QUESTAO_GENERO = 36;
        protected const long ID_QUESTAO_GRUPO_ETNICO = 37;
        protected const long ID_QUESTAO_PORTA_ENTRADA = 38;
        protected const long ID_QUESTAO_AGRUPAMENTO_DESENVOLVIMENTO = 39;
        protected const long ID_QUESTAO_AGRUPAMENTO_PROTECAO = 40;
        protected const long ID_QUESTAO_ENSINO_APRENDIZAGEM = 41;
        protected const long ID_QUESTAO_HIPOTESE_ESCRITA = 42;
        protected const long ID_QUESTAO_PERMANENCIA_ESCOLAR = 43;
        protected const long ID_QUESTAO_ANEXO = 47;
        protected const long ID_QUESTAO_ANEXOS_ITINERANCIA = 48;
        protected const long ID_QUESTAO_PROFISSIONAIS_ENVOLVIDOS = 49;
        protected const long ID_QUESTAO_ESTA_EM_CLASSE_HOSPITALAR = 50;

        protected const long ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_DATA_ENTRADA_QUEIXA = 17;
        protected const long ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_PRIORIDADE = 18;
        protected const long ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_PORTA_ENTRADA = 19;
        protected const long ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_GENERO = 20;
        protected const long ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_GRUPO_ETNICO = 21;
        protected const long ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_ESTUDANTE_MIGRANTE = 22;
        protected const long ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_RESPONSAVEL_MIGRANTE = 23;
        protected const long ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_FLUXO_ALERTA = 24;

        protected const long ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_INFANTIL_AGRUPAMENTO_DESENVOLVIMENTO = 25;
        protected const long ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_INFANTIL_AGRUPAMENTO_PROTECAO = 26;
        protected const long ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_INFANTIL_AGRUPAMENTO_PROMOCAO_CUIDADOS = 27;
        protected const long ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_INFANTIL_TIPO_ADOECE_COM_FREQUENCIA_SEM_CUIDADOS_MEDICOS = 28;
        protected const long ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_INFANTIL_TIPO_DOENCA_CRONICA_TRATAMENTO_LONGA_DURACAO = 29;

        protected const long ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_EF_E_OUTROS_HIPOTESE_ESCRITA = 30;
        protected const long ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_EF_E_OUTROS_ENSINO_APRENDIZAGEM = 31;
        protected const long ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_EF_E_OUTROS_PERMANENCIA_ESCOLAR = 32;
        protected const long ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_EF_E_OUTROS_SAUDE_MENTAL_DIFIC_INTERACOES_SOCIAIS = 33;
        protected const long ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_EF_E_OUTROS_VULNERABILIDADE_SOCIAL = 34;

        protected const long ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_MES = 44;
        protected const long ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_MODALIDADE_ATENCAO = 45;
        protected const long ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_PROCEDIMENTO_TRABALHO = 46;

        protected const long ID_OPCAO_RESPOSTA_NORMAL = 1;
        protected const long ID_OPCAO_RESPOSTA_CARTEIRA_VACINAS_ATRASADA = 3;
        protected const long ID_OPCAO_RESPOSTA_ADOECE_COM_FREQUENCIA = 4;
        protected const long ID_OPCAO_RESPOSTA_DOENCA_CRONICA = 5;
        protected const long ID_OPCAO_RESPOSTA_ASSADURA_QUESTAO_TIPO_ADOECE_COM_FREQUENCIA = 6;
        protected const long ID_OPCAO_RESPOSTA_OUTRAS_QUESTAO_TIPO_ADOECE_COM_FREQUENCIA = 8;
        protected const long ID_OPCAO_RESPOSTA_ANEMIA_FALCIFORME_QUESTAO_TIPO_DOENCA_CRONICA = 9;
        protected const long ID_OPCAO_RESPOSTA_OUTRAS_QUESTAO_TIPO_DOENCA_CRONICA = 11;
        protected const long ID_OPCAO_RESPOSTA_ATENDIMENTO_NAO_PRESENCIAL = 12;
        protected const long ID_OPCAO_RESPOSTA_GRUPO_TRAB_NAAPA = 13;
        protected const long ID_OPCAO_RESPOSTA_ACOES_LUDICAS = 14;
        protected const long ID_OPCAO_RESPOSTA_OUTRO_PROCEDIMENTO = 15;
        protected const long ID_OPCAO_RESPOSTA_SIM = 168; 
        protected const long ID_OPCAO_RESPOSTA_MASCULINO = 170;
        protected const long ID_OPCAO_RESPOSTA_BRANCO = 172;
        protected const long ID_OPCAO_RESPOSTA_AGITACAO_MOTORA = 176;
        protected const long ID_OPCAO_RESPOSTA_SITUACAO_RISCO_SOCIAL = 178;
        protected const long ID_OPCAO_RESPOSTA_ALFABETIZACAO = 179;
        protected const long ID_OPCAO_RESPOSTA_PRE_SILABICO = 181;
        protected const long ID_OPCAO_RESPOSTA_BAIXA_FREQUENCIA = 183;
        protected const long ID_GRUPO_FOCAL_TIPO_ATENDIMENTO_EXCLUIDO = 185;
        protected const long ID_OPCAO_RESPOSTA_REUNIAO_COMPARTILHDA = 186;
        protected const long ID_OPCAO_RESPOSTA_REUNIAO_REDE_MARCRO = 187;
        protected const long ID_OPCAO_RESPOSTA_REUNIAO_REDE_MICRO = 188;
        protected const long ID_OPCAO_RESPOSTA_REUNIAO_REDE_MICRO_UE = 189;
        protected const long ID_OPCAO_RESPOSTA_REUNIAO_HORARIOS_COLETIVOS = 190;
        protected const long ID_OPCAO_RESPOSTA_ITINERANCIA = 191;
        protected const long ID_OPCAO_RESPOSTA_ATENDIMENTO_PEDAGOGICO_DOMICILIAR = 192;
        protected const long ID_OPCAO_RESPOSTA_ATENDIMENTO_ATENDIMENTO_PRESENCIAL_DRE = 193;
        protected const long ID_OPCAO_RESPOSTA_ATENDIMENTO_REMOTO = 194;

        protected const long ID_OPCAO_RESPOSTA_ANALISE_DOCUMENTAL = 195;
        protected const long ID_OPCAO_RESPOSTA_ENTREVISTA = 196;
        protected const long ID_OPCAO_RESPOSTA_GRUPO_FOCAL = 197;
        protected const long ID_OPCAO_RESPOSTA_REFLEXIVO_INTERVENTIVO = 198;
        protected const long ID_OPCAO_RESPOSTA_OBSERVACAO = 199;
        protected const long ID_OPCAO_RESPOSTA_PROJETO_TECER = 200;
        protected const long ID_OPCAO_RESPOSTA_VISITA_TECNICA = 201;

        protected const long ID_OPCAO_RESPOSTA_SIM_ESTA_EM_SALA_HOSPITALAR = 207; 

        protected const long ID_SECAO_ENCAMINHAMENTO_NAAPA_INFORMACOES_ESTUDANTE = 1;
        protected const long ID_SECAO_ENCAMINHAMENTO_NAAPA_QUESTOES_APRESENTADAS_INFANTIL = 2;
        protected const long ID_SECAO_ENCAMINHAMENTO_NAAPA_ITINERANCIA = 3;

        protected const long ID_QUESTIONARIO_INFORMACOES_ESTUDANTE = 1;
        protected const long ID_QUESTIONARIO_QUESTOES_APRESENTADAS_INFANTIL = 2;
        protected const long ID_QUESTIONARIO_NAAPA_ITINERANCIA = 3;
        
        protected const long ID_ATENDIMENTO_NAO_PRESENCIAL = 12;
        protected const long ID_GRUPO_DE_TRABALHO_NAAPA = 13;
        protected const long ID_ACOES_LUDICAS = 14;

        protected const string NOME_COMPONENTE_QUESTAO_AGRUPAMENTO_PROMOCAO_CUIDADOS = "AGRUPAMENTO_PROMOCAO_CUIDADOS";
        protected const string NOME_COMPONENTE_QUESTAO_ENDERECO_RESIDENCIAL = "ENDERECO_RESIDENCIAL";
        protected const string NOME_COMPONENTE_QUESTAO_TURMAS_PROGRAMA = "TURMAS_PROGRAMA";

        protected const long QUESTIONARIO_RELATORIO_DINAMICO_ENCAMINHAMENTO_NAAPA_4 = 4;
        protected const long QUESTIONARIO_RELATORIO_DINAMICO_ENCAMINHAMENTO_NAAPA_INFANTIL_5 = 5;
        protected const long QUESTIONARIO_RELATORIO_DINAMICO_ENCAMINHAMENTO_NAAPA_EF_EJA_CIEJA_MOVA_CMCT_ETEC_6 = 6;
        protected const long QUESTIONARIO_RELATORIO_DINAMICO_ENCAMINHAMENTO_NAAPA_ATENDIMENTO_7 = 7;

        protected const string QUESTAO_NOME_DATA_DE_ENTRADA_DA_QUEIXA = "Data de entrada da queixa";
        protected const string QUESTAO_NOME_COMPONENTE_DATA_DE_ENTRADA_DA_QUEIXA = "DATA_ENTRADA_QUEIXA";

        protected const string QUESTAO_NOME_PRIORIDADE = "Prioridade";
        protected const string QUESTAO_NOME_COMPONENTE_PRIORIDADE = "PRIORIDADE";

        protected const string QUESTAO_NOME_PORTA_ENTRADA = "Porta de entrada";
        protected const string QUESTAO_NOME_COMPONENTE_PORTA_ENTRADA = "PORTA_ENTRADA";

        protected const string QUESTAO_NOME_GENERO = "Gênero";
        protected const string QUESTAO_NOME_COMPONENTE_GENERO = "GENERO";

        protected const string QUESTAO_NOME_GRUPO_ETNICO = "Grupo étnico (autodenominação)";
        protected const string QUESTAO_NOME_COMPONENTE_GRUPO_ETNICO = "GRUPO_ETNICO";

        protected const string QUESTAO_NOME_ESTUDANTE_MIGRANTE = "Criança/Estudante é imigrante (autodenominação)";
        protected const string QUESTAO_NOME_COMPONENTE_ESTUDANTE_MIGRANTE = "ESTUDANTE_MIGRANTE";

        protected const string QUESTAO_NOME_RESPONSAVEL_MIGRANTE = "Responsável/Cuidador é imigrante";
        protected const string QUESTAO_NOME_COMPONENTE_RESPONSAVEL_MIGRANTE = "RESPONSAVEL_MIGRANTE";

        protected const string QUESTAO_NOME_FLUXO_ALERTA = "Aplicação do fluxo de alerta";
        protected const string QUESTAO_NOME_COMPONENTE_FLUXO_ALERTA = "FLUXO_ALERTA";

        protected const string QUESTAO_NOME_AGRUPAMENTO_DESENVOLVIMENTO = "Questões no agrupamento desenvolvimento";
        protected const string QUESTAO_NOME_COMPONENTE_AGRUPAMENTO_DESENVOLVIMENTO = "AGRUPAMENTO_DESENVOLVIMENTO";

        protected const string QUESTAO_NOME_AGRUPAMENTO_PROTECAO = "Questões no agrupamento proteção";
        protected const string QUESTAO_NOME_COMPONENTE_AGRUPAMENTO_PROTECAO = "AGRUPAMENTO_PROTECAO";

        protected const string QUESTAO_NOME_AGRUPAMENTO_PROMOCAO_CUIDADOS = "Questões no agrupamento promoção de cuidados";
        protected const string QUESTAO_NOME_COMPONENTE_AGRUPAMENTO_PROMOCAO_CUIDADOS = "AGRUPAMENTO_PROMOCAO_CUIDADOS";

        protected const string QUESTAO_NOME_SELECIONE_UM_TIPO = "Selecione um tipo";
        protected const string QUESTAO_NOME_COMPONENTE_TIPO_ADOECE_COM_FREQUENCIA_SEM_CUIDADOS_MEDICOS = "TIPO_ADOECE_COM_FREQUENCIA_SEM_CUIDADOS_MEDICOS";
        protected const string QUESTAO_NOME_COMPONENTE_TIPO_DOENCA_CRONICA_TRATAMENTO_LONGA_DURACAO = "TIPO_DOENCA_CRONICA_TRATAMENTO_LONGA_DURACAO";

        protected const string QUESTAO_NOME_HIPOTESE_ESCRITA = "Hipótese de escrita";
        protected const string QUESTAO_NOME_COMPONENTE_HIPOTESE_ESCRITA = "HIPOTESE_ESCRITA";

        protected const string QUESTAO_NOME_ENSINO_APRENDIZAGEM = "Ensino e aprendizagem";
        protected const string QUESTAO_NOME_COMPONENTE_ENSINO_APRENDIZAGEM = "ENSINO_APRENDIZAGEM";

        protected const string QUESTAO_NOME_PERMANENCIA_ESCOLAR = "Permanência Escolar";
        protected const string QUESTAO_NOME_COMPONENTE_PERMANENCIA_ESCOLAR = "PERMANENCIA_ESCOLAR";

        protected const string QUESTAO_NOME_SAUDE_MENTAL_DIFIC_INTERACOES_SOCIAIS = "Saúde/Saúde Mental/Dificuldades nas interações sociais";
        protected const string QUESTAO_NOME_COMPONENTE_SAUDE_MENTAL_DIFIC_INTERACOES_SOCIAIS = "SAUDE_MENTAL_DIFIC_INTERACOES_SOCIAIS";

        protected const string QUESTAO_NOME_VULNERABILIDADE_SOCIAL = "Vulnerabilidade Social";
        protected const string QUESTAO_NOME_COMPONENTE_VULNERABILIDADE_SOCIAL = "VULNERABILIDADE_SOCIAL";

        protected const string QUESTAO_NOME_MES_ATENDIMENTO = "Mês";
        protected const string QUESTAO_NOME_COMPONENTE_DATA_DO_ATENDIMENTO = "DATA_DO_ATENDIMENTO";

        protected const string QUESTAO_NOME_MODALIDADE_ATENCAO = "Modalidade de atenção";
        protected const string QUESTAO_NOME_COMPONENTE_TIPO_DO_ATENDIMENTO = "TIPO_DO_ATENDIMENTO";

        protected const string QUESTAO_NOME_PROCEDIMENTO_TRABALHO = "Procedimento de trabalho";
        protected const string QUESTAO_NOME_COMPONENTE_PROCEDIMENTO_DE_TRABALHO = "PROCEDIMENTO_DE_TRABALHO";

        protected const string QUESTAO_NOME_COMPONENTE_ANEXOS = "ANEXOS";
        protected const string QUESTAO_NOME_COMPONENTE_ANEXO_ITINERANCIA = "ANEXO_ITINERANCIA";
        protected const string QUESTAO_NOME_COMPONENTE_PROFISSIONAIS_ENVOLVIDOS = "PROFISSIONAIS_ENVOLVIDOS_ATENDIMENTO";
        protected const string QUESTAO_NOME_COMPONENTE_ESTA_EM_CLASSE_HOSPÍTALAR = "ESTA_EM_CLASSE_HOSPITALAR";

        protected const long ID_OPCAO_RESPOSTA_DOENCA_CRONICA_OU_EM_TRATAMENTO_DE_LONGA_DURACAO_71_1099 = 71;
        protected const long ID_QUESTAO_COMPLEMENTAR_SELECIONE_UM_FILTRO_DOENCA_CRONICA_OU_EM_TRATAMENTO_DE_LONGA_DURACAO_28_303 = 28;

        protected const long ID_OPCAO_RESPOSTA_DOENCA_CRONICA_OU_EM_TRATAMENTO_DE_LONGA_DURACAO_75_1103 = 75;
        protected const long ID_QUESTAO_COMPLEMENTAR_SELECIONE_UM_FILTRO_DOENCA_CRONICA_OU_EM_TRATAMENTO_DE_LONGA_DURACAO_29_304 = 29;

        protected const string NOME_SECAO_ENCAMINHAMENTO_NAAPA_INFORMACOES_ESTUDANTE = "INFORMACOES_ESTUDANTE";
        protected const string NOME_SECAO_ENCAMINHAMENTO_NAAPA_QUESTOES_APRESENTADAS_INFANTIL = "QUESTOES_APRESENTADAS_INFANTIL";
        protected const string NOME_SECAO_ENCAMINHAMENTO_NAAPA_QUESTOES_APRESENTADAS_FUNDAMENTAL = "QUESTOES_APRESENTADAS_FUNDAMENTAL";

        public EncaminhamentoNAAPATesteBase(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected async Task CriarDadosBase(FiltroNAAPADto filtro)
        {
            await CriarDreUePerfilComponenteCurricular();

            CriarClaimUsuario(filtro.Perfil);

            await CriarUsuarios();

            await CriarTurmaTipoCalendario(filtro);

            if (filtro.CriarPeriodoEscolar)
                await CriarPeriodoEscolar(filtro.ConsiderarAnoAnterior);

            await CriarQuestionario();
            await CriarQuestoes();
            await CriarRespostas();
            await CriarRespostasComplementares();
            await CriarSecaoEncaminhamentoNAAPAQuestionario();
            await CriarSecaoEncaminhamentoNAAPAModalidade();
        }

        private async Task CriarSecaoEncaminhamentoNAAPAModalidade()
        {
            var questionarios = ObterTodos<Questionario>();
            questionarios = questionarios.Where(q => q.Tipo == TipoQuestionario.RelatorioDinamicoEncaminhamentoNAAPA).ToList();
            var secoes = ObterTodos<SecaoEncaminhamentoNAAPA>();
            secoes = secoes.Where(s => questionarios.Any(q => q.Id == s.QuestionarioId)).ToList(); 

            var secaoPublicacaoModalidade = secoes.FirstOrDefault(s => s.NomeComponente == NOME_SECAO_ENCAMINHAMENTO_NAAPA_QUESTOES_APRESENTADAS_FUNDAMENTAL);
            await InserirNaBase(new SecaoEncaminhamentoNAAPAModalidade()
            {
                Modalidade = Modalidade.Fundamental,
                SecaoEncaminhamentoNAAPAId = secaoPublicacaoModalidade.Id,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });
            await InserirNaBase(new SecaoEncaminhamentoNAAPAModalidade()
            {
                Modalidade = Modalidade.Medio,
                SecaoEncaminhamentoNAAPAId = secaoPublicacaoModalidade.Id,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });

            secaoPublicacaoModalidade = secoes.FirstOrDefault(s => s.NomeComponente == NOME_SECAO_ENCAMINHAMENTO_NAAPA_QUESTOES_APRESENTADAS_INFANTIL);
            await InserirNaBase(new SecaoEncaminhamentoNAAPAModalidade()
            {
                Modalidade = Modalidade.EducacaoInfantil,
                SecaoEncaminhamentoNAAPAId = secaoPublicacaoModalidade.Id,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });
        }

        private async Task CriarSecaoEncaminhamentoNAAPAQuestionario()
        {
            //Id 1
            await InserirNaBase(new SecaoEncaminhamentoNAAPA()
            {
                QuestionarioId = ID_QUESTIONARIO_INFORMACOES_ESTUDANTE,
                Nome = "Informações do Estudante",
                NomeComponente = NOME_SECAO_ENCAMINHAMENTO_NAAPA_INFORMACOES_ESTUDANTE,
                Etapa = 1, Ordem = 1,
                CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
            });

            //Id 2
            await InserirNaBase(new SecaoEncaminhamentoNAAPA()
            {
                QuestionarioId = ID_QUESTIONARIO_QUESTOES_APRESENTADAS_INFANTIL,
                Nome = "Questões apresentadas",
                NomeComponente = NOME_SECAO_ENCAMINHAMENTO_NAAPA_QUESTOES_APRESENTADAS_INFANTIL,
                Etapa = 1,
                Ordem = 2,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });
            
            //Id 3
            await InserirNaBase(new SecaoEncaminhamentoNAAPA()
            {
                QuestionarioId = ID_QUESTIONARIO_NAAPA_ITINERANCIA,
                Nome = "Apoio e Acompanhamento",
                NomeComponente = EncaminhamentoNAAPAConstants.SECAO_ITINERANCIA,
                Etapa = 1,
                Ordem = 3,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });

            //Id 4
            await InserirNaBase(new SecaoEncaminhamentoNAAPA()
            {
                QuestionarioId = QUESTIONARIO_RELATORIO_DINAMICO_ENCAMINHAMENTO_NAAPA_4,
                Nome = "Informações do Estudante",
                NomeComponente = NOME_SECAO_ENCAMINHAMENTO_NAAPA_INFORMACOES_ESTUDANTE,
                Etapa = 1, Ordem = 1,
                CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
            });

            //Id 5
            await InserirNaBase(new SecaoEncaminhamentoNAAPA()
            {
                QuestionarioId = QUESTIONARIO_RELATORIO_DINAMICO_ENCAMINHAMENTO_NAAPA_INFANTIL_5,
                Nome = "Questões apresentadas - Somente infantil",
                NomeComponente = NOME_SECAO_ENCAMINHAMENTO_NAAPA_QUESTOES_APRESENTADAS_INFANTIL,
                Etapa = 1, Ordem = 2,
                CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
            });

            //Id 6
            await InserirNaBase(new SecaoEncaminhamentoNAAPA()
            {
                QuestionarioId = QUESTIONARIO_RELATORIO_DINAMICO_ENCAMINHAMENTO_NAAPA_EF_EJA_CIEJA_MOVA_CMCT_ETEC_6,
                Nome = "Questões apresentadas - Todos exceto infantil",
                NomeComponente = NOME_SECAO_ENCAMINHAMENTO_NAAPA_QUESTOES_APRESENTADAS_FUNDAMENTAL,
                Etapa = 1, Ordem = 2,
                CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
            });

            //Id 7
            await InserirNaBase(new SecaoEncaminhamentoNAAPA()
            {
                QuestionarioId = QUESTIONARIO_RELATORIO_DINAMICO_ENCAMINHAMENTO_NAAPA_ATENDIMENTO_7,
                Nome = "Apoio e acompanhamento",
                NomeComponente = EncaminhamentoNAAPAConstants.SECAO_ITINERANCIA,
                Etapa = 1,
                Ordem = 3,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });
        }

        protected async Task CriarTurmaTipoCalendario(FiltroNAAPADto filtro)
        {
            await CriarTipoCalendario(filtro.TipoCalendario, filtro.ConsiderarAnoAnterior);
            if (filtro.CriarTurmaPadrao)
                await CriarTurma(filtro.Modalidade, filtro.AnoTurma, filtro.ConsiderarAnoAnterior, tipoTurno: 2);
        }

        protected async Task CriarPeriodoEscolar(bool considerarAnoAnterior = false)
        {
            await CriarPeriodoEscolar(DATA_01_01_INICIO_BIMESTRE_1, DATA_29_04_FIM_BIMESTRE_1, BIMESTRE_1, TIPO_CALENDARIO_1, considerarAnoAnterior);

            await CriarPeriodoEscolar(DATA_02_05_INICIO_BIMESTRE_2, DATA_24_07_FIM_BIMESTRE_2, BIMESTRE_2, TIPO_CALENDARIO_1, considerarAnoAnterior);

            await CriarPeriodoEscolar(DATA_25_07_INICIO_BIMESTRE_3, DATA_02_10_FIM_BIMESTRE_3, BIMESTRE_3, TIPO_CALENDARIO_1, considerarAnoAnterior);

            await CriarPeriodoEscolar(DATA_03_10_INICIO_BIMESTRE_4, DATA_22_12_FIM_BIMESTRE_4, BIMESTRE_4, TIPO_CALENDARIO_1, considerarAnoAnterior);
        }

        protected IObterAtendimentoNAAPAUseCase ObterServicoListagemComFiltros()
        {
            return ServiceProvider.GetService<IObterAtendimentoNAAPAUseCase>();
        }

        protected IObterObservacoesDeAtendimentoNAAPAUseCase ObterObservacoesDeEncaminhamentoNAAPA()
        {
            return ServiceProvider.GetService<IObterObservacoesDeAtendimentoNAAPAUseCase>();
        }
        protected IExcluirObservacoesDeAtendimentoNAAPAUseCase ExcluirObservacoesDeEncaminhamentoNAAPA()
        {
            return ServiceProvider.GetService<IExcluirObservacoesDeAtendimentoNAAPAUseCase>();
        }
        protected ISalvarObservacoesDeAtendimentoNAAPAUseCase SalvarObservacoesDeEncaminhamentoNAAPA()
        {
            return ServiceProvider.GetService<ISalvarObservacoesDeAtendimentoNAAPAUseCase>();
        }

        protected INotificarSobreTransferenciaUeDreAlunoTurmaDoAtendimentoNAAPAUseCase ObterServicoNotificacaoTransfAlunoDreUeDoEncaminhamentoNAAPA()
        {
            return ServiceProvider.GetService<INotificarSobreTransferenciaUeDreAlunoTurmaDoAtendimentoNAAPAUseCase>();
        }

        protected IRegistrarAtendimentoNAAPAUseCase ObterServicoRegistrarEncaminhamento()
        {
            return ServiceProvider.GetService<IRegistrarAtendimentoNAAPAUseCase>();
        }

        protected IRegistrarAtendimentoItinerarioNAAPAUseCase ObterServicoRegistrarEncaminhamentoItinerario()
        {
            return ServiceProvider.GetService<IRegistrarAtendimentoItinerarioNAAPAUseCase>();
        }

        protected IExcluirArquivoItineranciaNAAPAUseCase ObterServicoExcluirArquivoItineranciaNAAPAUseCase()
        {
            return ServiceProvider.GetService<IExcluirArquivoItineranciaNAAPAUseCase>();
        }

        protected IUploadDeArquivoUseCase ObterServicoUploadDeArquivoUseCase()
        {
            return ServiceProvider.GetService<IUploadDeArquivoUseCase>();
        }

        protected IObterSecoesItineranciaDeAtendimentoNAAPAUseCase ObterServicoListagemSecoesItineranciaEncaminhamentoNaapa()
        {
            return ServiceProvider.GetService<IObterSecoesItineranciaDeAtendimentoNAAPAUseCase>();
        }

        protected IExcluirSecaoItineranciaAtendimentoNAAPAUseCase ObterServicoExcluirSecaoItineranciaEncaminhamentoNAAPA()
        {
            return ServiceProvider.GetService<IExcluirSecaoItineranciaAtendimentoNAAPAUseCase>();
        }

        protected IExcluirAtendimentoNAAPAUseCase ObterServicoExcluirEncaminhamento()
        {
            return ServiceProvider.GetService<IExcluirAtendimentoNAAPAUseCase>();
        }

        protected IEncerrarAtendimentoNAAPAUseCase ObterServicoEncerrarEncaminhamento()
        {
            return ServiceProvider.GetService<IEncerrarAtendimentoNAAPAUseCase>();
        }

        protected IReabrirAtendimentoNAAPAUseCase ObterServicoReaberturaEncaminhamento()
        {
            return ServiceProvider.GetService<IReabrirAtendimentoNAAPAUseCase>();
        }

        protected IObterAtendimentoNAAPAPorIdUseCase ObterServicoObterEncaminhamentoNAAPAPorId()
        {
            return ServiceProvider.GetService<IObterAtendimentoNAAPAPorIdUseCase>();
        }

        protected IObterProfissionaisEnvolvidosAtendimentoNAAPANAAPAUseCase ObterServicoObtencaoProfissionaisEnvolvidosAtendimentoNAAPANAAPAUseCase()
        {
            return ServiceProvider.GetService<IObterProfissionaisEnvolvidosAtendimentoNAAPANAAPAUseCase>();
        }

        protected IObterQuestionarioItinerarioAtendimentoNAAPAUseCase ObterServicoObterQuestionarioItinerarioEncaminhamentoNAAPA()
        {
            return ServiceProvider.GetService<IObterQuestionarioItinerarioAtendimentoNAAPAUseCase>();
        }

        protected IAtualizarEnderecoDoAtendimentoNAAPAUseCase ObterServicoAtualizarEnderecoDoEncaminhamentoNAAPA()
        {
            return ServiceProvider.GetService<IAtualizarEnderecoDoAtendimentoNAAPAUseCase>();
        }

        protected IAtualizarTurmasProgramaDoAtendimentoNAAPAUseCase ObterServicoAtualizarTurmasProgramaDoEncaminhamentoNAAPA()
        {
            return ServiceProvider.GetService<IAtualizarTurmasProgramaDoAtendimentoNAAPAUseCase>();
        }

        protected IAtualizarTurmaDoAtendimentoNAAPAUseCase ObterServicoAtualizarTurmaDoEncaminhamentoNAAPA()
        {
            return ServiceProvider.GetService<IAtualizarTurmaDoAtendimentoNAAPAUseCase>();
        }

        protected INotificarSobreInativacaoAlunoTurmaDoAtendimentoNAAPAUseCase ObterServicoNotificacaoAtualizacaoMatriculaAlunoDoEncaminhamentoNAAPA()
        {
            return ServiceProvider.GetService<INotificarSobreInativacaoAlunoTurmaDoAtendimentoNAAPAUseCase>();
        }

        protected INotificarInatividadeDoAtendimentoNAAPAInformacaoUseCase ObterServicoNotificarInatividadeDoAtendimentoNAAPAInformacaoUseCase()
        {
            return ServiceProvider.GetService<INotificarInatividadeDoAtendimentoNAAPAInformacaoUseCase>();
        }

        protected IExisteAtendimentoNAAPAAtivoParaAlunoUseCase ObterServicoExisteEncaminhamentoNAAPAAtivoParaAlunoUseCase()
        {
            return ServiceProvider.GetService<IExisteAtendimentoNAAPAAtivoParaAlunoUseCase>();
        }

        private async Task CriarRespostasComplementares()
        {
            await InserirNaBase(new OpcaoQuestaoComplementar()
            {
                OpcaoRespostaId = ID_OPCAO_RESPOSTA_ADOECE_COM_FREQUENCIA,
                QuestaoComplementarId = ID_QUESTAO_TIPO_ADOECE_COM_FREQUENCIA,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });

            await InserirNaBase(new OpcaoQuestaoComplementar()
            {
                OpcaoRespostaId = ID_OPCAO_RESPOSTA_DOENCA_CRONICA,
                QuestaoComplementarId = ID_QUESTAO_TIPO_DOENCA_CRONICA,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });

            await InserirNaBase(new OpcaoQuestaoComplementar()
            {
                OpcaoRespostaId = ID_OPCAO_RESPOSTA_OUTRO_PROCEDIMENTO,
                QuestaoComplementarId = ID_QUESTAO_DESCRICAO_PROCEDIMENTO_TRABALHO,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });

            //53
            await InserirNaBase(new OpcaoQuestaoComplementar()
            {
                OpcaoRespostaId = ID_OPCAO_RESPOSTA_DOENCA_CRONICA_OU_EM_TRATAMENTO_DE_LONGA_DURACAO_71_1099,
                QuestaoComplementarId = ID_QUESTAO_COMPLEMENTAR_SELECIONE_UM_FILTRO_DOENCA_CRONICA_OU_EM_TRATAMENTO_DE_LONGA_DURACAO_28_303,
                CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
            });

            //54
            await InserirNaBase(new OpcaoQuestaoComplementar()
            {
                OpcaoRespostaId = ID_OPCAO_RESPOSTA_DOENCA_CRONICA_OU_EM_TRATAMENTO_DE_LONGA_DURACAO_75_1103,
                QuestaoComplementarId = ID_QUESTAO_COMPLEMENTAR_SELECIONE_UM_FILTRO_DOENCA_CRONICA_OU_EM_TRATAMENTO_DE_LONGA_DURACAO_29_304,
                CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
            });
        }

        private async Task CriarRespostas()
            {
                //id 1
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_PRIORIDADE,
                    Ordem = 1,
                    Nome = "Normal",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 2
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_PRIORIDADE,
                    Ordem = 2,
                    Nome = "Prioritária",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 3
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_AGRUPAMENTO_PROMOCAO_CUIDADOS,
                    Ordem = 1,
                    Nome = "Carteira de vacinas atrasada",
                    CriadoPor = SISTEMA_NOME,
                    CriadoRF = SISTEMA_CODIGO_RF,
                    CriadoEm = DateTime.Now
                });

                //id 4
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_AGRUPAMENTO_PROMOCAO_CUIDADOS,
                    Ordem = 2,
                    Nome = "Adoece com frequência sem receber cuidados médicos",
                    CriadoPor = SISTEMA_NOME,
                    CriadoRF = SISTEMA_CODIGO_RF,
                    CriadoEm = DateTime.Now
                });

                //id 5
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_AGRUPAMENTO_PROMOCAO_CUIDADOS,
                    Ordem = 6,
                    Nome = "Doença crônica ou em tratamento de longa duração",
                    CriadoPor = SISTEMA_NOME,
                    CriadoRF = SISTEMA_CODIGO_RF,
                    CriadoEm = DateTime.Now
                });

                //id 6
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_TIPO_ADOECE_COM_FREQUENCIA,
                    Ordem = 1,
                    Nome = "Assadura",
                    CriadoPor = SISTEMA_NOME,
                    CriadoRF = SISTEMA_CODIGO_RF,
                    CriadoEm = DateTime.Now
                });

                //id 7
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_TIPO_ADOECE_COM_FREQUENCIA,
                    Ordem = 2,
                    Nome = "Bronquite",
                    CriadoPor = SISTEMA_NOME,
                    CriadoRF = SISTEMA_CODIGO_RF,
                    CriadoEm = DateTime.Now
                });

                //id 8
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_TIPO_ADOECE_COM_FREQUENCIA,
                    Ordem = 13,
                    Nome = "Outras",
                    CriadoPor = SISTEMA_NOME,
                    CriadoRF = SISTEMA_CODIGO_RF,
                    CriadoEm = DateTime.Now
                });

                //id 9
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_TIPO_DOENCA_CRONICA,
                    Ordem = 1,
                    Nome = "Anemia falciforme",
                    CriadoPor = SISTEMA_NOME,
                    CriadoRF = SISTEMA_CODIGO_RF,
                    CriadoEm = DateTime.Now
                });

                //id 10
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_TIPO_DOENCA_CRONICA,
                    Ordem = 2,
                    Nome = "Asma",
                    CriadoPor = SISTEMA_NOME,
                    CriadoRF = SISTEMA_CODIGO_RF,
                    CriadoEm = DateTime.Now
                });

                //id 11
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_TIPO_DOENCA_CRONICA,
                    Ordem = 14,
                    Nome = "Outras",
                    CriadoPor = SISTEMA_NOME,
                    CriadoRF = SISTEMA_CODIGO_RF,
                    CriadoEm = DateTime.Now
                });

                //id 12
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_TIPO_ATENDIMENTO,
                    Ordem = 1,
                    Nome = "Itinerância",
                    Excluido = true,
                    CriadoPor = SISTEMA_NOME,
                    CriadoRF = SISTEMA_CODIGO_RF,
                    CriadoEm = DateTime.Now
                });

                //id 13
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_TIPO_ATENDIMENTO,
                    Ordem = 2,
                    Nome = "Grupo de Trabalho NAAPA",
                    CriadoPor = SISTEMA_NOME,
                    CriadoRF = SISTEMA_CODIGO_RF,
                    CriadoEm = DateTime.Now
                });

                //id 14
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_PROCEDIMENTO_TRABALHO,
                    Ordem = 1,
                    Nome = "Ações Lúdicas",
                    CriadoPor = SISTEMA_NOME,
                    CriadoRF = SISTEMA_CODIGO_RF,
                    CriadoEm = DateTime.Now
                });

                //id 15
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_PROCEDIMENTO_TRABALHO,
                    Ordem = 2,
                    Nome = "Análise Documental",
                    CriadoPor = SISTEMA_NOME,
                    CriadoRF = SISTEMA_CODIGO_RF,
                    CriadoEm = DateTime.Now
                });

                //id 16 (1043)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_PRIORIDADE,
                    Ordem = 1,
                    Nome = "Normal",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 17 (1044)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_PRIORIDADE,
                    Ordem = 2,
                    Nome = "Prioritária",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 18 (1045)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_PORTA_ENTRADA,
                    Ordem = 1,
                    Nome = "Contato Telefônico",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 19 (1046)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_PORTA_ENTRADA,
                    Ordem = 2,
                    Nome = "E-mail",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 20 (1047)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_PORTA_ENTRADA,
                    Ordem = 3,
                    Nome = "Família",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 21 (1048)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_PORTA_ENTRADA,
                    Ordem = 4,
                    Nome = "Grupo de Trabalho",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 22 (1049)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_PORTA_ENTRADA,
                    Ordem = 5,
                    Nome = "Memorando / Relatório Escolar",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 23 (1050)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_PORTA_ENTRADA,
                    Ordem = 6,
                    Nome = "Ofício / MP / Vara da Infância",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 24 (1051)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_PORTA_ENTRADA,
                    Ordem = 7,
                    Nome = "Rede de Proteção Social",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 25 (1052)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_PORTA_ENTRADA,
                    Ordem = 8,
                    Nome = "Supervisão Escolar / Outros Setores DRE",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 26 (1053)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_PORTA_ENTRADA,
                    Ordem = 9,
                    Nome = "Busca Ativa Escolar (NAAPA)",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 27 (1054)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_PORTA_ENTRADA,
                    Ordem = 10,
                    Nome = "Busca Ativa Escolar (Unicef)",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 28 (1055)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_GENERO,
                    Ordem = 1,
                    Nome = "Masculino",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 29 (1056)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_GENERO,
                    Ordem = 2,
                    Nome = "Feminino",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 30 (1057)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_GENERO,
                    Ordem = 3,
                    Nome = "Outro",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 31 (1058)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_GRUPO_ETNICO,
                    Ordem = 1,
                    Nome = "Branco",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 32 (1059)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_GRUPO_ETNICO,
                    Ordem = 2,
                    Nome = "Negro",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 33 (1060)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_GRUPO_ETNICO,
                    Ordem = 3,
                    Nome = "Pardo",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 34 (1061)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_GRUPO_ETNICO,
                    Ordem = 4,
                    Nome = "Amarelo",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 35 (1062)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_GRUPO_ETNICO,
                    Ordem = 5,
                    Nome = "Indígena",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 36 (1063)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_GRUPO_ETNICO,
                    Ordem = 6,
                    Nome = "Não declarado",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 37 (1064)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_ESTUDANTE_MIGRANTE,
                    Ordem = 1,
                    Nome = "Sim",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 38 (1065)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_ESTUDANTE_MIGRANTE,
                    Ordem = 2,
                    Nome = "Não",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 39 (1066)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_RESPONSAVEL_MIGRANTE,
                    Ordem = 1,
                    Nome = "Sim",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 40 (1067)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_RESPONSAVEL_MIGRANTE,
                    Ordem = 2,
                    Nome = "Não",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 41 (1068)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_FLUXO_ALERTA,
                    Ordem = 1,
                    Nome = "Fluxo de violência",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 42 (1069)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_FLUXO_ALERTA,
                    Ordem = 2,
                    Nome = "Notificação feita pelo NAAPA",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 43 (1070)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_FLUXO_ALERTA,
                    Ordem = 3,
                    Nome = "Notificação feita pela escola",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 44 (1071)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_FLUXO_ALERTA,
                    Ordem = 4,
                    Nome = "Busca ativa escolar",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 45 (1072)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_FLUXO_ALERTA,
                    Ordem = 5,
                    Nome = "Busca ativa escolar - Unicef",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 45 (1073)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_FLUXO_ALERTA,
                    Ordem = 6,
                    Nome = "Fluxo da Gravidez",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 46 (1074)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_INFANTIL_AGRUPAMENTO_DESENVOLVIMENTO,
                    Ordem = 1,
                    Nome = "Agitação motora",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 47 (1075)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_INFANTIL_AGRUPAMENTO_DESENVOLVIMENTO,
                    Ordem = 2,
                    Nome = "Aparente sofrimento diante das rotinas propostas para seu agrupamento",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 48 (1076)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_INFANTIL_AGRUPAMENTO_DESENVOLVIMENTO,
                    Ordem = 3,
                    Nome = "Dificuldades nas habilidades motoras",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 49 (1077)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_INFANTIL_AGRUPAMENTO_DESENVOLVIMENTO,
                    Ordem = 4,
                    Nome = "Dificuldades nas habilidades de comunicação",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 50 (1078)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_INFANTIL_AGRUPAMENTO_DESENVOLVIMENTO,
                    Ordem = 5,
                    Nome = "Dificuldades nas interações com os adultos",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 51 (1079)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_INFANTIL_AGRUPAMENTO_DESENVOLVIMENTO,
                    Ordem = 6,
                    Nome = "Dificuldades nas interações com outras crianças",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 52 (1080)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_INFANTIL_AGRUPAMENTO_DESENVOLVIMENTO,
                    Ordem = 7,
                    Nome = "Dificuldades no desenvolvimento da comunicação verbal",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 53 (1081)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_INFANTIL_AGRUPAMENTO_DESENVOLVIMENTO,
                    Ordem = 8,
                    Nome = "Embotamento social",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 54 (1082)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_INFANTIL_AGRUPAMENTO_DESENVOLVIMENTO,
                    Ordem = 9,
                    Nome = "Isolamento",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 55 (1083)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_INFANTIL_AGRUPAMENTO_DESENVOLVIMENTO,
                    Ordem = 10,
                    Nome = "Medo",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 56 (1084)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_INFANTIL_AGRUPAMENTO_DESENVOLVIMENTO,
                    Ordem = 11,
                    Nome = "Não brinca",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 57 (1085)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_INFANTIL_AGRUPAMENTO_DESENVOLVIMENTO,
                    Ordem = 12,
                    Nome = "Tristeza",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 58 (1086)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_INFANTIL_AGRUPAMENTO_PROTECAO,
                    Ordem = 1,
                    Nome = "Em situação de risco social",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 59 (1087)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_INFANTIL_AGRUPAMENTO_PROTECAO,
                    Ordem = 2,
                    Nome = "Em situação de rua ou na rua",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 60 (1088)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_INFANTIL_AGRUPAMENTO_PROTECAO,
                    Ordem = 3,
                    Nome = "Família em situação de extrema pobreza",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 61 (1089)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_INFANTIL_AGRUPAMENTO_PROTECAO,
                    Ordem = 4,
                    Nome = "Responsáveis com transtornos mentais",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 62 (1090)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_INFANTIL_AGRUPAMENTO_PROTECAO,
                    Ordem = 5,
                    Nome = "Suspeita de trabalho infantil",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 63 (1091)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_INFANTIL_AGRUPAMENTO_PROTECAO,
                    Ordem = 6,
                    Nome = "Suspeita de Violência Estrutural",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 64 (1092)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_INFANTIL_AGRUPAMENTO_PROTECAO,
                    Ordem = 7,
                    Nome = "Suspeita de violência física",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 65 (1093)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_INFANTIL_AGRUPAMENTO_PROTECAO,
                    Ordem = 8,
                    Nome = "Suspeita de violência institucional",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 66 (1094)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_INFANTIL_AGRUPAMENTO_PROTECAO,
                    Ordem = 9,
                    Nome = "Suspeita de violência negligencial",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 67 (1095)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_INFANTIL_AGRUPAMENTO_PROTECAO,
                    Ordem = 10,
                    Nome = "Suspeita de violência psicológica",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 68 (1096)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_INFANTIL_AGRUPAMENTO_PROTECAO,
                    Ordem = 11,
                    Nome = "Suspeita de Violência química",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 69 (1097)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_INFANTIL_AGRUPAMENTO_PROTECAO,
                    Ordem = 12,
                    Nome = "Suspeita de violência sexual",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 70 (1098)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_INFANTIL_AGRUPAMENTO_PROMOCAO_CUIDADOS,
                    Ordem = 1,
                    Nome = "Carteira de vacinas atrasada",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 71 (1099)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_INFANTIL_AGRUPAMENTO_PROMOCAO_CUIDADOS,
                    Ordem = 2,
                    Nome = "Adoece com frequência sem receber cuidados médicos",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 72 (1100)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_INFANTIL_AGRUPAMENTO_PROMOCAO_CUIDADOS,
                    Ordem = 3,
                    Nome = "Baixo peso",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 73 (1101)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_INFANTIL_AGRUPAMENTO_PROMOCAO_CUIDADOS,
                    Ordem = 4,
                    Nome = "Excesso de peso",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 74 (1102)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_INFANTIL_AGRUPAMENTO_PROMOCAO_CUIDADOS,
                    Ordem = 5,
                    Nome = "Rotina de sono alterada",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 75 (1103)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_INFANTIL_AGRUPAMENTO_PROMOCAO_CUIDADOS,
                    Ordem = 6,
                    Nome = "Doença crônica ou em tratamento de longa duração",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 76 (1104)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_INFANTIL_AGRUPAMENTO_PROMOCAO_CUIDADOS,
                    Ordem = 7,
                    Nome = "Frequência Irregular/excesso de faltas",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 77 (1105)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_INFANTIL_AGRUPAMENTO_PROMOCAO_CUIDADOS,
                    Ordem = 8,
                    Nome = "Enurese e Encoprese",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 78 (1106)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_INFANTIL_AGRUPAMENTO_PROMOCAO_CUIDADOS,
                    Ordem = 9,
                    Nome = "Saúde bucal comprometida",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 79 (1107)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_INFANTIL_TIPO_ADOECE_COM_FREQUENCIA_SEM_CUIDADOS_MEDICOS,
                    Ordem = 1,
                    Nome = "Assadura",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 80 (1108)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_INFANTIL_TIPO_ADOECE_COM_FREQUENCIA_SEM_CUIDADOS_MEDICOS,
                    Ordem = 2,
                    Nome = "Bronquite",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 81 (1109)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_INFANTIL_TIPO_ADOECE_COM_FREQUENCIA_SEM_CUIDADOS_MEDICOS,
                    Ordem = 3,
                    Nome = "Coriza",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 82 (1110)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_INFANTIL_TIPO_ADOECE_COM_FREQUENCIA_SEM_CUIDADOS_MEDICOS,
                    Ordem = 4,
                    Nome = "Diarreia",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 83 (1111)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_INFANTIL_TIPO_ADOECE_COM_FREQUENCIA_SEM_CUIDADOS_MEDICOS,
                    Ordem = 5,
                    Nome = "Doenças de pele",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 84 (1112)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_INFANTIL_TIPO_ADOECE_COM_FREQUENCIA_SEM_CUIDADOS_MEDICOS,
                    Ordem = 6,
                    Nome = "Dor de ouvido",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 85 (1113)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_INFANTIL_TIPO_ADOECE_COM_FREQUENCIA_SEM_CUIDADOS_MEDICOS,
                    Ordem = 7,
                    Nome = "Febre",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 86 (1114)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_INFANTIL_TIPO_ADOECE_COM_FREQUENCIA_SEM_CUIDADOS_MEDICOS,
                    Ordem = 8,
                    Nome = "Gripes",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 87 (1115)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_INFANTIL_TIPO_ADOECE_COM_FREQUENCIA_SEM_CUIDADOS_MEDICOS,
                    Ordem = 9,
                    Nome = "Manchas na pele",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 88 (1116)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_INFANTIL_TIPO_ADOECE_COM_FREQUENCIA_SEM_CUIDADOS_MEDICOS,
                    Ordem = 10,
                    Nome = "Piolho",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 89 (1117)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_INFANTIL_TIPO_ADOECE_COM_FREQUENCIA_SEM_CUIDADOS_MEDICOS,
                    Ordem = 11,
                    Nome = "Sarna",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 90 (1118)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_INFANTIL_TIPO_ADOECE_COM_FREQUENCIA_SEM_CUIDADOS_MEDICOS,
                    Ordem = 12,
                    Nome = "Vômito",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 91 (1119)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_INFANTIL_TIPO_ADOECE_COM_FREQUENCIA_SEM_CUIDADOS_MEDICOS,
                    Ordem = 13,
                    Nome = "Outras",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 92 (1120)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_INFANTIL_TIPO_DOENCA_CRONICA_TRATAMENTO_LONGA_DURACAO,
                    Ordem = 1,
                    Nome = "Anemia falciforme",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 93 (1121)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_INFANTIL_TIPO_DOENCA_CRONICA_TRATAMENTO_LONGA_DURACAO,
                    Ordem = 2,
                    Nome = "Asma",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 94 (1122)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_INFANTIL_TIPO_DOENCA_CRONICA_TRATAMENTO_LONGA_DURACAO,
                    Ordem = 3,
                    Nome = "Bronquite",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 95 (1123)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_INFANTIL_TIPO_DOENCA_CRONICA_TRATAMENTO_LONGA_DURACAO,
                    Ordem = 4,
                    Nome = "Câncer",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 96 (1124)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_INFANTIL_TIPO_DOENCA_CRONICA_TRATAMENTO_LONGA_DURACAO,
                    Ordem = 5,
                    Nome = "Diabetes",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 97 (1125)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_INFANTIL_TIPO_DOENCA_CRONICA_TRATAMENTO_LONGA_DURACAO,
                    Ordem = 6,
                    Nome = "Doença hepática",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 98 (1126)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_INFANTIL_TIPO_DOENCA_CRONICA_TRATAMENTO_LONGA_DURACAO,
                    Ordem = 7,
                    Nome = "Doença renal",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 99 (1127)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_INFANTIL_TIPO_DOENCA_CRONICA_TRATAMENTO_LONGA_DURACAO,
                    Ordem = 8,
                    Nome = "Doenças do aparelho digestivo",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 100 (1128)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_INFANTIL_TIPO_DOENCA_CRONICA_TRATAMENTO_LONGA_DURACAO,
                    Ordem = 9,
                    Nome = "Epilepsia",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 101 (1129)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_INFANTIL_TIPO_DOENCA_CRONICA_TRATAMENTO_LONGA_DURACAO,
                    Ordem = 10,
                    Nome = "Imunossuprimido",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 102 (1130)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_INFANTIL_TIPO_DOENCA_CRONICA_TRATAMENTO_LONGA_DURACAO,
                    Ordem = 11,
                    Nome = "Soropositivo",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 103 (1131)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_INFANTIL_TIPO_DOENCA_CRONICA_TRATAMENTO_LONGA_DURACAO,
                    Ordem = 12,
                    Nome = "Transplantados",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 104 (1132)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_INFANTIL_TIPO_DOENCA_CRONICA_TRATAMENTO_LONGA_DURACAO,
                    Ordem = 13,
                    Nome = "Tuberculose",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 105 (1133)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_INFANTIL_TIPO_DOENCA_CRONICA_TRATAMENTO_LONGA_DURACAO,
                    Ordem = 14,
                    Nome = "Outras",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 106 (1134)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_EF_E_OUTROS_HIPOTESE_ESCRITA,
                    Ordem = 1,
                    Nome = "Pré-silábico",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 107 (1135)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_EF_E_OUTROS_HIPOTESE_ESCRITA,
                    Ordem = 2,
                    Nome = "Silábico sem valor sonoro",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 108 (1136)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_EF_E_OUTROS_HIPOTESE_ESCRITA,
                    Ordem = 3,
                    Nome = "Silábico com valor sonoro",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 109 (1137)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_EF_E_OUTROS_HIPOTESE_ESCRITA,
                    Ordem = 4,
                    Nome = "Silábico alfabético",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 110 (1138)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_EF_E_OUTROS_HIPOTESE_ESCRITA,
                    Ordem = 5,
                    Nome = "Alfabético",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 111 (1139)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_EF_E_OUTROS_ENSINO_APRENDIZAGEM,
                    Ordem = 1,
                    Nome = "Alfabetização",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 112 (1140)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_EF_E_OUTROS_ENSINO_APRENDIZAGEM,
                    Ordem = 2,
                    Nome = "Suspeita de Dislexia",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 113 (1141)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_EF_E_OUTROS_ENSINO_APRENDIZAGEM,
                    Ordem = 3,
                    Nome = "Dificuldade de Produção Texto",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 114 (1142)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_EF_E_OUTROS_ENSINO_APRENDIZAGEM,
                    Ordem = 4,
                    Nome = "Raciocínio Lógico",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 115 (1143)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_EF_E_OUTROS_ENSINO_APRENDIZAGEM,
                    Ordem = 5,
                    Nome = "Dificuldade leitura/compreensão",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 116 (1144)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_EF_E_OUTROS_ENSINO_APRENDIZAGEM,
                    Ordem = 6,
                    Nome = "Desatenção",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 117 (1145)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_EF_E_OUTROS_ENSINO_APRENDIZAGEM,
                    Ordem = 7,
                    Nome = "Desorganização",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 118 (1146)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_EF_E_OUTROS_ENSINO_APRENDIZAGEM,
                    Ordem = 8,
                    Nome = "Resistência ao registro",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 119 (1147)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_EF_E_OUTROS_ENSINO_APRENDIZAGEM,
                    Ordem = 9,
                    Nome = "Problemas de memorização",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 120 (1148)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_EF_E_OUTROS_PERMANENCIA_ESCOLAR,
                    Ordem = 1,
                    Nome = "Baixa frequência",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 121 (1149)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_EF_E_OUTROS_PERMANENCIA_ESCOLAR,
                    Ordem = 2,
                    Nome = "Reprovação ano anterior",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 122 (1150)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_EF_E_OUTROS_PERMANENCIA_ESCOLAR,
                    Ordem = 3,
                    Nome = "Evasão escolar",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 123 (1151)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_EF_E_OUTROS_PERMANENCIA_ESCOLAR,
                    Ordem = 4,
                    Nome = "Defasagem idade escolar",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 124 (1152)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_EF_E_OUTROS_SAUDE_MENTAL_DIFIC_INTERACOES_SOCIAIS,
                    Ordem = 1,
                    Nome = "Doença Crônica",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 125 (1153)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_EF_E_OUTROS_SAUDE_MENTAL_DIFIC_INTERACOES_SOCIAIS,
                    Ordem = 2,
                    Nome = "Enurese",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 126 (1154)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_EF_E_OUTROS_SAUDE_MENTAL_DIFIC_INTERACOES_SOCIAIS,
                    Ordem = 3,
                    Nome = "Encoprese",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 127 (1155)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_EF_E_OUTROS_SAUDE_MENTAL_DIFIC_INTERACOES_SOCIAIS,
                    Ordem = 4,
                    Nome = "Questões fonoaudiológicas",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 128 (1156)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_EF_E_OUTROS_SAUDE_MENTAL_DIFIC_INTERACOES_SOCIAIS,
                    Ordem = 5,
                    Nome = "Sonolência",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 129 (1157)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_EF_E_OUTROS_SAUDE_MENTAL_DIFIC_INTERACOES_SOCIAIS,
                    Ordem = 6,
                    Nome = "Transtornos alimentares",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 130 (1158)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_EF_E_OUTROS_SAUDE_MENTAL_DIFIC_INTERACOES_SOCIAIS,
                    Ordem = 7,
                    Nome = "Saúde - outras questões",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 131 (1159)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_EF_E_OUTROS_SAUDE_MENTAL_DIFIC_INTERACOES_SOCIAIS,
                    Ordem = 8,
                    Nome = "Auto agressão/Auto mutilação",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 132 (1160)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_EF_E_OUTROS_SAUDE_MENTAL_DIFIC_INTERACOES_SOCIAIS,
                    Ordem = 9,
                    Nome = "Ideação suicida",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 133 (1161)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_EF_E_OUTROS_SAUDE_MENTAL_DIFIC_INTERACOES_SOCIAIS,
                    Ordem = 10,
                    Nome = "Mutismo seletivo",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 134 (1162)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_EF_E_OUTROS_SAUDE_MENTAL_DIFIC_INTERACOES_SOCIAIS,
                    Ordem = 11,
                    Nome = "TDA",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 135 (1163)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_EF_E_OUTROS_SAUDE_MENTAL_DIFIC_INTERACOES_SOCIAIS,
                    Ordem = 12,
                    Nome = "TDAH",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 136 (1164)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_EF_E_OUTROS_SAUDE_MENTAL_DIFIC_INTERACOES_SOCIAIS,
                    Ordem = 13,
                    Nome = "TOD",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 137 (1165)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_EF_E_OUTROS_SAUDE_MENTAL_DIFIC_INTERACOES_SOCIAIS,
                    Ordem = 14,
                    Nome = "Saúde mental - outras questões",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 138 (1166)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_EF_E_OUTROS_SAUDE_MENTAL_DIFIC_INTERACOES_SOCIAIS,
                    Ordem = 15,
                    Nome = "Agitação",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 139 (1167)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_EF_E_OUTROS_SAUDE_MENTAL_DIFIC_INTERACOES_SOCIAIS,
                    Ordem = 16,
                    Nome = "Agressividade",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 140 (1168)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_EF_E_OUTROS_SAUDE_MENTAL_DIFIC_INTERACOES_SOCIAIS,
                    Ordem = 17,
                    Nome = "Apatia",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 141 (1169)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_EF_E_OUTROS_SAUDE_MENTAL_DIFIC_INTERACOES_SOCIAIS,
                    Ordem = 18,
                    Nome = "Comportamento Infantilizado",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 142 (1170)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_EF_E_OUTROS_SAUDE_MENTAL_DIFIC_INTERACOES_SOCIAIS,
                    Ordem = 19,
                    Nome = "Dificuldade de Interação",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 143 (1171)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_EF_E_OUTROS_SAUDE_MENTAL_DIFIC_INTERACOES_SOCIAIS,
                    Ordem = 20,
                    Nome = "Luto",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 144 (1172)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_EF_E_OUTROS_SAUDE_MENTAL_DIFIC_INTERACOES_SOCIAIS,
                    Ordem = 21,
                    Nome = "Medo",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 145 (1173)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_EF_E_OUTROS_SAUDE_MENTAL_DIFIC_INTERACOES_SOCIAIS,
                    Ordem = 22,
                    Nome = "Resistência a regras",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 146 (1174)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_EF_E_OUTROS_SAUDE_MENTAL_DIFIC_INTERACOES_SOCIAIS,
                    Ordem = 23,
                    Nome = "Timidez",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 147 (1175)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_EF_E_OUTROS_SAUDE_MENTAL_DIFIC_INTERACOES_SOCIAIS,
                    Ordem = 24,
                    Nome = "Gravidez na adolescência",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 148 (1176)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_EF_E_OUTROS_SAUDE_MENTAL_DIFIC_INTERACOES_SOCIAIS,
                    Ordem = 25,
                    Nome = "Comportamento - outras questões",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 149 (1177)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_EF_E_OUTROS_VULNERABILIDADE_SOCIAL,
                    Ordem = 1,
                    Nome = "Acolhimento Institucional",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 150 (1178)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_EF_E_OUTROS_VULNERABILIDADE_SOCIAL,
                    Ordem = 2,
                    Nome = "Frequenta ambientes de risco social",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 151 (1179)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_EF_E_OUTROS_VULNERABILIDADE_SOCIAL,
                    Ordem = 3,
                    Nome = "Medidas socioeducativas",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 152 (1180)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_EF_E_OUTROS_VULNERABILIDADE_SOCIAL,
                    Ordem = 4,
                    Nome = "Pobreza extrema",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 153 (1181)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_EF_E_OUTROS_VULNERABILIDADE_SOCIAL,
                    Ordem = 5,
                    Nome = "Responsável com S. Mental, Drogadição ou Deficiência",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 154 (1182)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_EF_E_OUTROS_VULNERABILIDADE_SOCIAL,
                    Ordem = 6,
                    Nome = "Suspeita de negligência",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 155 (1183)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_EF_E_OUTROS_VULNERABILIDADE_SOCIAL,
                    Ordem = 7,
                    Nome = "Suspeita de violência física",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 156 (1184)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_EF_E_OUTROS_VULNERABILIDADE_SOCIAL,
                    Ordem = 8,
                    Nome = "Suspeita de violência sexual",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 157 (1185)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_EF_E_OUTROS_VULNERABILIDADE_SOCIAL,
                    Ordem = 9,
                    Nome = "Suspeita de violência psicológica",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 158 (1186)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_EF_E_OUTROS_VULNERABILIDADE_SOCIAL,
                    Ordem = 10,
                    Nome = "Suspeita de violência institucional",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 159 (1187)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_EF_E_OUTROS_VULNERABILIDADE_SOCIAL,
                    Ordem = 11,
                    Nome = "Suspeita/Uso de substâncias psicoativas",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 160 (1188)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_EF_E_OUTROS_VULNERABILIDADE_SOCIAL,
                    Ordem = 12,
                    Nome = "Trabalho infantil",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 161 (1188)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_EF_E_OUTROS_VULNERABILIDADE_SOCIAL,
                    Ordem = 12,
                    Nome = "Trabalho infantil",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 162 (1189)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_EF_E_OUTROS_VULNERABILIDADE_SOCIAL,
                    Ordem = 13,
                    Nome = "Exploração sexual",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 163 (1190)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_EF_E_OUTROS_VULNERABILIDADE_SOCIAL,
                    Ordem = 14,
                    Nome = "Envolvimento com o tráfico de drogas",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 165 (1191)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_EF_E_OUTROS_VULNERABILIDADE_SOCIAL,
                    Ordem = 15,
                    Nome = "Responsável com problemas de saúde",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 166 (1192)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_EF_E_OUTROS_VULNERABILIDADE_SOCIAL,
                    Ordem = 16,
                    Nome = "Responsável recluso",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 167 (1193)
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_EF_E_OUTROS_VULNERABILIDADE_SOCIAL,
                    Ordem = 17,
                    Nome = "Em situação de rua ou na rua",
                    CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
                });

                //id 168
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_ESTUDANTE_MIGRANTE,
                    Ordem = 1,
                    Nome = "Sim",
                    CriadoPor = SISTEMA_NOME,
                    CriadoRF = SISTEMA_CODIGO_RF,
                    CriadoEm = DateTime.Now
                });

                //id 169
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_ESTUDANTE_MIGRANTE,
                    Ordem = 2,
                    Nome = "Não",
                    CriadoPor = SISTEMA_NOME,
                    CriadoRF = SISTEMA_CODIGO_RF,
                    CriadoEm = DateTime.Now
                });

                //id 170
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_GENERO,
                    Ordem = 1,
                    Nome = "Masculino",
                    CriadoPor = SISTEMA_NOME,
                    CriadoRF = SISTEMA_CODIGO_RF,
                    CriadoEm = DateTime.Now
                });

                //id 171
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_GENERO,
                    Ordem = 2,
                    Nome = "Feminino",
                    CriadoPor = SISTEMA_NOME,
                    CriadoRF = SISTEMA_CODIGO_RF,
                    CriadoEm = DateTime.Now
                });

                //id 172
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_GRUPO_ETNICO,
                    Ordem = 1,
                    Nome = "Branco",
                    CriadoPor = SISTEMA_NOME,
                    CriadoRF = SISTEMA_CODIGO_RF,
                    CriadoEm = DateTime.Now
                });

                //id 173
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_GRUPO_ETNICO,
                    Ordem = 2,
                    Nome = "Negro",
                    CriadoPor = SISTEMA_NOME,
                    CriadoRF = SISTEMA_CODIGO_RF,
                    CriadoEm = DateTime.Now
                });

                //id 174
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_PORTA_ENTRADA,
                    Ordem = 1,
                    Nome = "Contato Telefônico",
                    CriadoPor = SISTEMA_NOME,
                    CriadoRF = SISTEMA_CODIGO_RF,
                    CriadoEm = DateTime.Now
                });

                //id 175
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_PORTA_ENTRADA,
                    Ordem = 1,
                    Nome = "E-mail",
                    CriadoPor = SISTEMA_NOME,
                    CriadoRF = SISTEMA_CODIGO_RF,
                    CriadoEm = DateTime.Now
                });

                //id 176
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_AGRUPAMENTO_DESENVOLVIMENTO,
                    Ordem = 1,
                    Nome = "Agitação motora",
                    CriadoPor = SISTEMA_NOME,
                    CriadoRF = SISTEMA_CODIGO_RF,
                    CriadoEm = DateTime.Now
                });

                //id 177
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_AGRUPAMENTO_DESENVOLVIMENTO,
                    Ordem = 2,
                    Nome = "Aparente sofrimento diante das rotinas propostas para seu agrupamento",
                    CriadoPor = SISTEMA_NOME,
                    CriadoRF = SISTEMA_CODIGO_RF,
                    CriadoEm = DateTime.Now
                });


                //id 178
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_AGRUPAMENTO_PROTECAO,
                    Ordem = 1,
                    Nome = "Em situação de risco social",
                    CriadoPor = SISTEMA_NOME,
                    CriadoRF = SISTEMA_CODIGO_RF,
                    CriadoEm = DateTime.Now
                });

                //id 179
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_ENSINO_APRENDIZAGEM,
                    Ordem = 1,
                    Nome = "Alfabetização",
                    CriadoPor = SISTEMA_NOME,
                    CriadoRF = SISTEMA_CODIGO_RF,
                    CriadoEm = DateTime.Now
                });

                //id 180
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_ENSINO_APRENDIZAGEM,
                    Ordem = 2,
                    Nome = "Suspeita de Dislexia",
                    CriadoPor = SISTEMA_NOME,
                    CriadoRF = SISTEMA_CODIGO_RF,
                    CriadoEm = DateTime.Now
                });

                //id 181
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_HIPOTESE_ESCRITA,
                    Ordem = 1,
                    Nome = "Pré-silábico",
                    CriadoPor = SISTEMA_NOME,
                    CriadoRF = SISTEMA_CODIGO_RF,
                    CriadoEm = DateTime.Now
                });

                //id 182
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_HIPOTESE_ESCRITA,
                    Ordem = 2,
                    Nome = "Silábico sem valor sonoro",
                    CriadoPor = SISTEMA_NOME,
                    CriadoRF = SISTEMA_CODIGO_RF,
                    CriadoEm = DateTime.Now
                });

                //id 183
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_PERMANENCIA_ESCOLAR,
                    Ordem = 1,
                    Nome = "Baixa frequência",
                    CriadoPor = SISTEMA_NOME,
                    CriadoRF = SISTEMA_CODIGO_RF,
                    CriadoEm = DateTime.Now
                });

                //id 184
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_PERMANENCIA_ESCOLAR,
                    Ordem = 2,
                    Nome = "Reprovação ano anterior",
                    CriadoPor = SISTEMA_NOME,
                    CriadoRF = SISTEMA_CODIGO_RF,
                    CriadoEm = DateTime.Now
                });

                //id 185
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_TIPO_ATENDIMENTO,
                    Ordem = 3,
                    Nome = "Grupo Focal",
                    Excluido = true,
                    CriadoPor = SISTEMA_NOME,
                    CriadoRF = SISTEMA_CODIGO_RF,
                    CriadoEm = DateTime.Now
                });

                //id 186
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_PROCEDIMENTO_TRABALHO,
                    Ordem = 3,
                    Nome = "Reunião compartilhada",
                    Excluido = true,
                    CriadoPor = SISTEMA_NOME,
                    CriadoRF = SISTEMA_CODIGO_RF,
                    CriadoEm = DateTime.Now
                });


                //id 187
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_PROCEDIMENTO_TRABALHO,
                    Ordem = 4,
                    Nome = "Reunião de Rede Macro (formada pelo território)",
                    Excluido = true,
                    CriadoPor = SISTEMA_NOME,
                    CriadoRF = SISTEMA_CODIGO_RF,
                    CriadoEm = DateTime.Now
                });


                //id 188
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_PROCEDIMENTO_TRABALHO,
                    Ordem = 5,
                    Nome = "Reunião de Rede Micro (formada pelo NAAPA)",
                    Excluido = true,
                    CriadoPor = SISTEMA_NOME,
                    CriadoRF = SISTEMA_CODIGO_RF,
                    CriadoEm = DateTime.Now
                });

                //id 189
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_PROCEDIMENTO_TRABALHO,
                    Ordem = 6,
                    Nome = "Reunião de Rede Micro na UE",
                    Excluido = true,
                    CriadoPor = SISTEMA_NOME,
                    CriadoRF = SISTEMA_CODIGO_RF,
                    CriadoEm = DateTime.Now
                });

                //id 190
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_PROCEDIMENTO_TRABALHO,
                    Ordem = 7,
                    Nome = "Reunião em Horários Coletivos",
                    Excluido = true,
                    CriadoPor = SISTEMA_NOME,
                    CriadoRF = SISTEMA_CODIGO_RF,
                    CriadoEm = DateTime.Now
                });

                //id 191
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_TIPO_ATENDIMENTO,
                    Ordem = 1,
                    Nome = "Itinerância",
                    CriadoPor = SISTEMA_NOME,
                    CriadoRF = SISTEMA_CODIGO_RF,
                    CriadoEm = DateTime.Now
                });

                //id 192
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_TIPO_ATENDIMENTO,
                    Ordem = 3,
                    Nome = "Atendimento Pedagógico Domiciliar",
                    CriadoPor = SISTEMA_NOME,
                    CriadoRF = SISTEMA_CODIGO_RF,
                    CriadoEm = DateTime.Now
                });

                //id 193
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_TIPO_ATENDIMENTO,
                    Ordem = 5,
                    Nome = "Atendimento presencial na DRE",
                    CriadoPor = SISTEMA_NOME,
                    CriadoRF = SISTEMA_CODIGO_RF,
                    CriadoEm = DateTime.Now
                });

                //id 194
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_TIPO_ATENDIMENTO,
                    Ordem = 6,
                    Nome = "Atendimento Remoto",
                    CriadoPor = SISTEMA_NOME,
                    CriadoRF = SISTEMA_CODIGO_RF,
                    CriadoEm = DateTime.Now
                });

                //id 195
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_PROCEDIMENTO_TRABALHO,
                    Ordem = 7,
                    Nome = "Análise documental",
                    CriadoPor = SISTEMA_NOME,
                    CriadoRF = SISTEMA_CODIGO_RF,
                    CriadoEm = DateTime.Now
                });

                //id 196
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_PROCEDIMENTO_TRABALHO,
                    Ordem = 8,
                    Nome = "Entrevista",
                    CriadoPor = SISTEMA_NOME,
                    CriadoRF = SISTEMA_CODIGO_RF,
                    CriadoEm = DateTime.Now
                });

                //id 197
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_PROCEDIMENTO_TRABALHO,
                    Ordem = 9,
                    Nome = "Grupo focal",
                    CriadoPor = SISTEMA_NOME,
                    CriadoRF = SISTEMA_CODIGO_RF,
                    CriadoEm = DateTime.Now
                });

                //id 198
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_PROCEDIMENTO_TRABALHO,
                    Ordem = 10,
                    Nome = "Grupo reflexivo interventivo",
                    CriadoPor = SISTEMA_NOME,
                    CriadoRF = SISTEMA_CODIGO_RF,
                    CriadoEm = DateTime.Now
                });

                //id 199
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_PROCEDIMENTO_TRABALHO,
                    Ordem = 11,
                    Nome = "Observação",
                    CriadoPor = SISTEMA_NOME,
                    CriadoRF = SISTEMA_CODIGO_RF,
                    CriadoEm = DateTime.Now
                });

                //id 200
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_PROCEDIMENTO_TRABALHO,
                    Ordem = 12,
                    Nome = "Projeto Tecer",
                    CriadoPor = SISTEMA_NOME,
                    CriadoRF = SISTEMA_CODIGO_RF,
                    CriadoEm = DateTime.Now
                });

                //id 201
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_PROCEDIMENTO_TRABALHO,
                    Ordem = 13,
                    Nome = "Visita técnica",
                    CriadoPor = SISTEMA_NOME,
                    CriadoRF = SISTEMA_CODIGO_RF,
                    CriadoEm = DateTime.Now
                });

                //id 202
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_MES,
                    Ordem = 1,
                    Nome = "Janeiro",
                    CriadoPor = SISTEMA_NOME,
                    CriadoRF = SISTEMA_CODIGO_RF,
                    CriadoEm = DateTime.Now
                });

                //id 202
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_MES,
                    Ordem = 2,
                    Nome = "Fevereiro",
                    CriadoPor = SISTEMA_NOME,
                    CriadoRF = SISTEMA_CODIGO_RF,
                    CriadoEm = DateTime.Now
                });

                //id 203
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_MODALIDADE_ATENCAO,
                    Ordem = 1,
                    Nome = "Itinerância",
                    CriadoPor = SISTEMA_NOME,
                    CriadoRF = SISTEMA_CODIGO_RF,
                    CriadoEm = DateTime.Now
                });

                //id 204
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_MODALIDADE_ATENCAO,
                    Ordem = 2,
                    Nome = "Grupo de Trabalho NAAPA",
                    CriadoPor = SISTEMA_NOME,
                    CriadoRF = SISTEMA_CODIGO_RF,
                    CriadoEm = DateTime.Now
                });

                //id 205
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_PROCEDIMENTO_TRABALHO,
                    Ordem = 1,
                    Nome = "Ações Lúdicas",
                    CriadoPor = SISTEMA_NOME,
                    CriadoRF = SISTEMA_CODIGO_RF,
                    CriadoEm = DateTime.Now
                });

                //id 206
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_RELATORIO_DINAMICO_NAAPA_PROCEDIMENTO_TRABALHO,
                    Ordem = 2,
                    Nome = "Análise Documental",
                    CriadoPor = SISTEMA_NOME,
                    CriadoRF = SISTEMA_CODIGO_RF,
                    CriadoEm = DateTime.Now
                });

                //id 207
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_ESTA_EM_CLASSE_HOSPITALAR,
                    Ordem = 1,
                    Nome = "Sim",
                    CriadoPor = SISTEMA_NOME,
                    CriadoRF = SISTEMA_CODIGO_RF,
                    CriadoEm = DateTime.Now
                });

                //id 208
                await InserirNaBase(new OpcaoResposta()
                {
                    QuestaoId = ID_QUESTAO_ESTA_EM_CLASSE_HOSPITALAR,
                    Ordem = 2,
                    Nome = "Não",
                    CriadoPor = SISTEMA_NOME,
                    CriadoRF = SISTEMA_CODIGO_RF,
                    CriadoEm = DateTime.Now
                });
        }


        private async Task CriarQuestionario()
        {
            //1
            await InserirNaBase(new Questionario()
            {
                Nome = "Questionário Encaminhamento NAAPA Etapa 1 Seção 1",
                Tipo = TipoQuestionario.EncaminhamentoNAAPA,
                CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
            });

            //2
            await InserirNaBase(new Questionario()
            {
                Nome = "Questionário Encaminhamento NAAPA Etapa 1 Seção 2 - Infantil",
                Tipo = TipoQuestionario.EncaminhamentoNAAPA,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });

            //3
            await InserirNaBase(new Questionario()
            {
                Nome = "Questionário Encaminhamento NAAPA Etapa 1 Seção 3 - Itinerância",
                Tipo = TipoQuestionario.EncaminhamentoNAAPA,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });

            //4
            await InserirNaBase(new Questionario()
            {
                Nome = "Questionário Relatório Dinâmico Encaminhamento NAAPA",
                Tipo = TipoQuestionario.RelatorioDinamicoEncaminhamentoNAAPA,
                CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
            });

            //5
            await InserirNaBase(new Questionario()
            {
                Nome = "Questionário Relatório Dinâmico Encaminhamento NAAPA - Infantil",
                Tipo = TipoQuestionario.RelatorioDinamicoEncaminhamentoNAAPA,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });

            //6
            await InserirNaBase(new Questionario()
            {
                Nome = "Questionário Relatório Dinâmico Encaminhamento NAAPA - Fundamental, Médio, EJA, CIEJA, MOVA, CMCT, ETEC",
                Tipo = TipoQuestionario.RelatorioDinamicoEncaminhamentoNAAPA,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });

            //7
            await InserirNaBase(new Questionario()
            {
                Nome = "Questionário Relatório Dinâmico Encaminhamento NAAPA - Atendimento",
                Tipo = TipoQuestionario.RelatorioDinamicoEncaminhamentoNAAPA,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });
        }

        private async Task CriarQuestoes()
        {
            //id 1
            await InserirNaBase(new Questao()
            {
                QuestionarioId = ID_QUESTIONARIO_INFORMACOES_ESTUDANTE,
                Ordem = 0,
                Nome = "Data de entrada da queixa",
                SomenteLeitura = true,
                Obrigatorio = true,
                Tipo = TipoQuestao.Data,
                NomeComponente = "DATA_ENTRADA_QUEIXA",
                CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
            });

            //id 2
            await InserirNaBase(new Questao()
            {
                QuestionarioId = ID_QUESTIONARIO_INFORMACOES_ESTUDANTE,
                Ordem = 1,
                Nome = "Prioridade",
                Obrigatorio = true,
                Tipo = TipoQuestao.Combo,
                NomeComponente = "PRIORIDADE",
                CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
            });

            //id 3
            await InserirNaBase(new Questao()
            {
                QuestionarioId = ID_QUESTIONARIO_QUESTOES_APRESENTADAS_INFANTIL,
                Ordem = 4,
                Nome = "Questões no agrupamento promoção de cuidados",
                SomenteLeitura = true,
                Obrigatorio = true,
                Tipo = TipoQuestao.ComboMultiplaEscolha,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now,
                NomeComponente = NOME_COMPONENTE_QUESTAO_AGRUPAMENTO_PROMOCAO_CUIDADOS
            });

            //id 4
            await InserirNaBase(new Questao()
            {
                QuestionarioId = ID_QUESTIONARIO_QUESTOES_APRESENTADAS_INFANTIL,
                Ordem = 1,
                Nome = "Selecione um tipo",
                Observacao = "Adoece com frequência sem receber cuidados médicos",
                SomenteLeitura = true,
                Obrigatorio = false,
                Tipo = TipoQuestao.ComboMultiplaEscolha,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now,
                NomeComponente = "TIPO_ADOECE_COM_FREQUENCIA_SEM_CUIDADOS_MEDICOS"
            });

            //id 5
            await InserirNaBase(new Questao()
            {
                QuestionarioId = ID_QUESTIONARIO_QUESTOES_APRESENTADAS_INFANTIL,
                Ordem = 1,
                Nome = "Selecione um tipo",
                Observacao = "Doença crônica ou em tratamento de longa duração",
                SomenteLeitura = true,
                Obrigatorio = false,
                Tipo = TipoQuestao.ComboMultiplaEscolha,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now,
                NomeComponente = "TIPO_DOENCA_CRONICA_TRATAMENTO_LONGA_DURACAO"
            });

            //id 6
            await InserirNaBase(new Questao()
            {
                QuestionarioId = ID_QUESTIONARIO_QUESTOES_APRESENTADAS_INFANTIL,
                Ordem = 5,
                Nome = "Observações",
                SomenteLeitura = true,
                Obrigatorio = false,
                Tipo = TipoQuestao.Texto,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now,
                NomeComponente = "OBS_AGRUPAMENTO_PROMOCAO_CUIDADOS"
            });

            //id 7
            await InserirNaBase(new Questao()
            {
                QuestionarioId = ID_QUESTIONARIO_NAAPA_ITINERANCIA,
                Ordem = 0,
                Nome = "Data do atendimento",
                Obrigatorio = true,
                Tipo = TipoQuestao.Data,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now,
                NomeComponente = "DATA_DO_ATENDIMENTO"
            });

            //id 8
            await InserirNaBase(new Questao()
            {
                QuestionarioId = ID_QUESTIONARIO_NAAPA_ITINERANCIA,
                Ordem = 1,
                Nome = "Modalidade de atenção",
                Obrigatorio = true,
                Tipo = TipoQuestao.Combo,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now,
                NomeComponente = "TIPO_DO_ATENDIMENTO"
            });

            //id 9
            await InserirNaBase(new Questao()
            {
                QuestionarioId = ID_QUESTIONARIO_NAAPA_ITINERANCIA,
                Ordem = 2,
                Nome = "Procedimento de trabalho",
                Obrigatorio = true,
                Tipo = TipoQuestao.ComboMultiplaEscolha,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now,
                NomeComponente = "PROCEDIMENTO_DE_TRABALHO"
            });

            //id 10
            await InserirNaBase(new Questao()
            {
                QuestionarioId = ID_QUESTIONARIO_NAAPA_ITINERANCIA,
                Ordem = 6,
                Nome = "Descrição do atendimento",
                Obrigatorio = true,
                Tipo = TipoQuestao.EditorTexto,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now,
                NomeComponente = "DESCRICAO_DO_ATENDIMENTO"
            });

            //id 11
            await InserirNaBase(new Questao()
            {
                QuestionarioId = ID_QUESTIONARIO_NAAPA_ITINERANCIA,
                Ordem = 1,
                Nome = "Descrição do procedimento de trabalho",
                Obrigatorio = true,
                Tipo = TipoQuestao.Texto,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now,
                NomeComponente = "DESCRICAO_PROCEDIMENTO_TRABALHO"
            });

            //id 12
            await InserirNaBase(new Questao()
            {
                QuestionarioId = ID_QUESTIONARIO_INFORMACOES_ESTUDANTE,
                Ordem = 6,
                Nome = "Endereço residencial",
                Obrigatorio = false,
                Tipo = TipoQuestao.Endereco,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now,
                NomeComponente = "ENDERECO_RESIDENCIAL"
            });

            //id 13
            await InserirNaBase(new Questao()
            {
                QuestionarioId = ID_QUESTIONARIO_INFORMACOES_ESTUDANTE,
                Ordem = 16,
                Nome = "Turmas de programa",
                Obrigatorio = false,
                Tipo = TipoQuestao.TurmasPrograma,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now,
                NomeComponente = "TURMAS_PROGRAMA"
            });

            //id 14
            await InserirNaBase(new Questao()
            {
                QuestionarioId = ID_QUESTIONARIO_INFORMACOES_ESTUDANTE,
                Ordem = 3,
                Nome = "NIS (Número de Identificação Social)",
                NomeComponente = "NIS",
                Obrigatorio = false,
                Tipo = TipoQuestao.Numerico,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });

            //id 15
            await InserirNaBase(new Questao()
            {
                QuestionarioId = ID_QUESTIONARIO_INFORMACOES_ESTUDANTE,
                Ordem = 13,
                Nome = "UBS de referência",
                NomeComponente = "UBS",
                Obrigatorio = false,
                Tipo = TipoQuestao.Texto,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });

            //id 16
            await InserirNaBase(new Questao()
            {
                QuestionarioId = ID_QUESTIONARIO_INFORMACOES_ESTUDANTE,
                Ordem = 17,
                Nome = "Descrição do encaminhamento",
                NomeComponente = "DESCRICAO_ENCAMINHAMENTO",
                Obrigatorio = false,
                Tipo = TipoQuestao.EditorTexto,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });

            //id 17 (292)
            await InserirNaBase(new Questao()
            {
                QuestionarioId = QUESTIONARIO_RELATORIO_DINAMICO_ENCAMINHAMENTO_NAAPA_4,
                Ordem = 0,
                Nome = QUESTAO_NOME_DATA_DE_ENTRADA_DA_QUEIXA,
                NomeComponente = QUESTAO_NOME_COMPONENTE_DATA_DE_ENTRADA_DA_QUEIXA,
                Tipo = TipoQuestao.Periodo,
                CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
            });

            //id 18 (293)
            await InserirNaBase(new Questao()
            {
                QuestionarioId = QUESTIONARIO_RELATORIO_DINAMICO_ENCAMINHAMENTO_NAAPA_4,
                Ordem = 1,
                Nome = QUESTAO_NOME_PRIORIDADE,
                NomeComponente = QUESTAO_NOME_COMPONENTE_PRIORIDADE,
                Tipo = TipoQuestao.ComboMultiplaEscolha,
                CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
            });

            //id 19 (294)
            await InserirNaBase(new Questao()
            {
                QuestionarioId = QUESTIONARIO_RELATORIO_DINAMICO_ENCAMINHAMENTO_NAAPA_4,
                Ordem = 2,
                Nome = QUESTAO_NOME_PORTA_ENTRADA,
                NomeComponente = QUESTAO_NOME_COMPONENTE_PORTA_ENTRADA,
                Tipo = TipoQuestao.ComboMultiplaEscolha,
                CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
            });

            //id 20 (295)
            await InserirNaBase(new Questao()
            {
                QuestionarioId = QUESTIONARIO_RELATORIO_DINAMICO_ENCAMINHAMENTO_NAAPA_4,
                Ordem = 3,
                Nome = QUESTAO_NOME_GENERO,
                NomeComponente = QUESTAO_NOME_COMPONENTE_GENERO,
                Tipo = TipoQuestao.ComboMultiplaEscolha,
                CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
            });

            //id 21 (296)
            await InserirNaBase(new Questao()
            {
                QuestionarioId = QUESTIONARIO_RELATORIO_DINAMICO_ENCAMINHAMENTO_NAAPA_4,
                Ordem = 4,
                Nome = QUESTAO_NOME_GRUPO_ETNICO,
                NomeComponente = QUESTAO_NOME_COMPONENTE_GRUPO_ETNICO,
                Tipo = TipoQuestao.ComboMultiplaEscolha,
                CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
            });

            //id 22 (297)
            await InserirNaBase(new Questao()
            {
                QuestionarioId = QUESTIONARIO_RELATORIO_DINAMICO_ENCAMINHAMENTO_NAAPA_4,
                Ordem = 5,
                Nome = QUESTAO_NOME_ESTUDANTE_MIGRANTE,
                NomeComponente = QUESTAO_NOME_COMPONENTE_ESTUDANTE_MIGRANTE,
                Tipo = TipoQuestao.ComboMultiplaEscolha,
                CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
            });

            //id 23 (298)
            await InserirNaBase(new Questao()
            {
                QuestionarioId = QUESTIONARIO_RELATORIO_DINAMICO_ENCAMINHAMENTO_NAAPA_4,
                Ordem = 6,
                Nome = QUESTAO_NOME_RESPONSAVEL_MIGRANTE,
                NomeComponente = QUESTAO_NOME_COMPONENTE_RESPONSAVEL_MIGRANTE,
                Tipo = TipoQuestao.ComboMultiplaEscolha,
                CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
            });

            //id 24 (299)
            await InserirNaBase(new Questao()
            {
                QuestionarioId = QUESTIONARIO_RELATORIO_DINAMICO_ENCAMINHAMENTO_NAAPA_4,
                Ordem = 7, 
                Nome = QUESTAO_NOME_FLUXO_ALERTA,
                NomeComponente = QUESTAO_NOME_COMPONENTE_FLUXO_ALERTA,
                Tipo = TipoQuestao.ComboMultiplaEscolha,
                CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
            });

            //id 25 (300)
            await InserirNaBase(new Questao()
            {
                QuestionarioId = QUESTIONARIO_RELATORIO_DINAMICO_ENCAMINHAMENTO_NAAPA_INFANTIL_5,
                Ordem = 9,
                Nome = QUESTAO_NOME_AGRUPAMENTO_DESENVOLVIMENTO,
                NomeComponente = QUESTAO_NOME_COMPONENTE_AGRUPAMENTO_DESENVOLVIMENTO,
                Tipo = TipoQuestao.ComboMultiplaEscolha,
                CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
            });

            //id 26 (301)
            await InserirNaBase(new Questao()
            {
                QuestionarioId = QUESTIONARIO_RELATORIO_DINAMICO_ENCAMINHAMENTO_NAAPA_INFANTIL_5,
                Ordem = 10,
                Nome = QUESTAO_NOME_AGRUPAMENTO_PROTECAO,
                NomeComponente = QUESTAO_NOME_COMPONENTE_AGRUPAMENTO_PROTECAO,
                Tipo = TipoQuestao.ComboMultiplaEscolha,
                CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
            });

            //id 27 (302)
            await InserirNaBase(new Questao()
            {
                QuestionarioId = QUESTIONARIO_RELATORIO_DINAMICO_ENCAMINHAMENTO_NAAPA_INFANTIL_5,
                Ordem = 11,
                Nome = QUESTAO_NOME_AGRUPAMENTO_PROMOCAO_CUIDADOS,
                NomeComponente = QUESTAO_NOME_COMPONENTE_AGRUPAMENTO_PROMOCAO_CUIDADOS,
                Tipo = TipoQuestao.ComboMultiplaEscolha,
                CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
            });

            //id 28 (303)
            await InserirNaBase(new Questao()
            {
                QuestionarioId = QUESTIONARIO_RELATORIO_DINAMICO_ENCAMINHAMENTO_NAAPA_INFANTIL_5,
                Ordem = 1, 
                Nome = QUESTAO_NOME_SELECIONE_UM_TIPO,
                NomeComponente = QUESTAO_NOME_COMPONENTE_TIPO_ADOECE_COM_FREQUENCIA_SEM_CUIDADOS_MEDICOS,
                Tipo = TipoQuestao.ComboMultiplaEscolha,
                CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
            });

            //id 29 (304)
            await InserirNaBase(new Questao()
            {
                QuestionarioId = QUESTIONARIO_RELATORIO_DINAMICO_ENCAMINHAMENTO_NAAPA_INFANTIL_5,
                Ordem = 1,
                Nome = QUESTAO_NOME_SELECIONE_UM_TIPO,
                NomeComponente = QUESTAO_NOME_COMPONENTE_TIPO_DOENCA_CRONICA_TRATAMENTO_LONGA_DURACAO,
                Tipo = TipoQuestao.ComboMultiplaEscolha,
                CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
            });

            //id 30 (305)
            await InserirNaBase(new Questao()
            {
                QuestionarioId = QUESTIONARIO_RELATORIO_DINAMICO_ENCAMINHAMENTO_NAAPA_EF_EJA_CIEJA_MOVA_CMCT_ETEC_6,
                Ordem = 12,
                Nome = QUESTAO_NOME_HIPOTESE_ESCRITA,
                NomeComponente = QUESTAO_NOME_COMPONENTE_HIPOTESE_ESCRITA,
                Tipo = TipoQuestao.ComboMultiplaEscolha,
                CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
            });

            //id 31 (305)
            await InserirNaBase(new Questao()
            {
                QuestionarioId = QUESTIONARIO_RELATORIO_DINAMICO_ENCAMINHAMENTO_NAAPA_EF_EJA_CIEJA_MOVA_CMCT_ETEC_6,
                Ordem = 13,
                Nome = QUESTAO_NOME_ENSINO_APRENDIZAGEM,
                NomeComponente = QUESTAO_NOME_COMPONENTE_ENSINO_APRENDIZAGEM,
                Tipo = TipoQuestao.ComboMultiplaEscolha,
                CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
            });

            //id 32 (306)
            await InserirNaBase(new Questao()
            {
                QuestionarioId = QUESTIONARIO_RELATORIO_DINAMICO_ENCAMINHAMENTO_NAAPA_EF_EJA_CIEJA_MOVA_CMCT_ETEC_6,
                Ordem = 14,
                Nome = QUESTAO_NOME_PERMANENCIA_ESCOLAR,
                NomeComponente = QUESTAO_NOME_COMPONENTE_PERMANENCIA_ESCOLAR,
                Tipo = TipoQuestao.ComboMultiplaEscolha,
                CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
            });

            //id 33 (307)
            await InserirNaBase(new Questao()
            {
                QuestionarioId = QUESTIONARIO_RELATORIO_DINAMICO_ENCAMINHAMENTO_NAAPA_EF_EJA_CIEJA_MOVA_CMCT_ETEC_6,
                Ordem = 15,
                Nome = QUESTAO_NOME_SAUDE_MENTAL_DIFIC_INTERACOES_SOCIAIS,
                NomeComponente = QUESTAO_NOME_COMPONENTE_SAUDE_MENTAL_DIFIC_INTERACOES_SOCIAIS,
                Tipo = TipoQuestao.ComboMultiplaEscolha,
                CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
            });

            //id 34 (308)
            await InserirNaBase(new Questao()
            {
                QuestionarioId = QUESTIONARIO_RELATORIO_DINAMICO_ENCAMINHAMENTO_NAAPA_EF_EJA_CIEJA_MOVA_CMCT_ETEC_6,
                Ordem = 16,
                Nome = QUESTAO_NOME_VULNERABILIDADE_SOCIAL,
                NomeComponente = QUESTAO_NOME_COMPONENTE_VULNERABILIDADE_SOCIAL,
                Tipo = TipoQuestao.ComboMultiplaEscolha,
                CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
            });

            //ID 35
            await InserirNaBase(new Questao()
            {
                QuestionarioId = ID_QUESTIONARIO_INFORMACOES_ESTUDANTE,
                Ordem = 10,
                Nome = QUESTAO_NOME_ESTUDANTE_MIGRANTE,
                NomeComponente = QUESTAO_NOME_COMPONENTE_ESTUDANTE_MIGRANTE,
                Tipo = TipoQuestao.Radio,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });

            //ID 36
            await InserirNaBase(new Questao()
            {
                QuestionarioId = ID_QUESTIONARIO_INFORMACOES_ESTUDANTE,
                Ordem = 8,
                Nome = QUESTAO_NOME_GENERO,
                NomeComponente = QUESTAO_NOME_COMPONENTE_GENERO,
                Tipo = TipoQuestao.Combo,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });

            //ID 37
            await InserirNaBase(new Questao()
            {
                QuestionarioId = ID_QUESTIONARIO_INFORMACOES_ESTUDANTE,
                Ordem = 9,
                Nome = QUESTAO_NOME_GRUPO_ETNICO,
                NomeComponente = QUESTAO_NOME_COMPONENTE_GRUPO_ETNICO,
                Tipo = TipoQuestao.Combo,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });

            //ID 38
            await InserirNaBase(new Questao()
            {
                QuestionarioId = ID_QUESTIONARIO_INFORMACOES_ESTUDANTE,
                Ordem = 2,
                Nome = QUESTAO_NOME_PORTA_ENTRADA,
                NomeComponente = QUESTAO_NOME_COMPONENTE_PORTA_ENTRADA,
                Tipo = TipoQuestao.Combo,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });

            //ID 39
            await InserirNaBase(new Questao()
            {
                QuestionarioId = ID_QUESTIONARIO_QUESTOES_APRESENTADAS_INFANTIL,
                Ordem = 1,
                Nome = QUESTAO_NOME_AGRUPAMENTO_DESENVOLVIMENTO,
                NomeComponente = QUESTAO_NOME_COMPONENTE_AGRUPAMENTO_DESENVOLVIMENTO,
                Tipo = TipoQuestao.Combo,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });

            //ID 40
            await InserirNaBase(new Questao()
            {
                QuestionarioId = ID_QUESTIONARIO_QUESTOES_APRESENTADAS_INFANTIL,
                Ordem = 2,
                Nome = QUESTAO_NOME_AGRUPAMENTO_PROTECAO,
                NomeComponente = QUESTAO_NOME_COMPONENTE_AGRUPAMENTO_PROTECAO,
                Tipo = TipoQuestao.Combo,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });

            //id 41
            await InserirNaBase(new Questao()
            {
                QuestionarioId = ID_QUESTIONARIO_QUESTOES_APRESENTADAS_INFANTIL,
                Ordem = 9,
                Nome = QUESTAO_NOME_ENSINO_APRENDIZAGEM,
                NomeComponente = QUESTAO_NOME_COMPONENTE_ENSINO_APRENDIZAGEM,
                Tipo = TipoQuestao.ComboMultiplaEscolha,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });

            //id 42
            await InserirNaBase(new Questao()
            {
                QuestionarioId = ID_QUESTIONARIO_INFORMACOES_ESTUDANTE,
                Ordem = 10,
                Nome = QUESTAO_NOME_HIPOTESE_ESCRITA,
                NomeComponente = QUESTAO_NOME_COMPONENTE_HIPOTESE_ESCRITA,
                Tipo = TipoQuestao.ComboMultiplaEscolha,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });

            //id 43
            await InserirNaBase(new Questao()
            {
                QuestionarioId = ID_QUESTIONARIO_QUESTOES_APRESENTADAS_INFANTIL,
                Ordem = 11,
                Nome = QUESTAO_NOME_PERMANENCIA_ESCOLAR,
                NomeComponente = QUESTAO_NOME_COMPONENTE_PERMANENCIA_ESCOLAR,
                Tipo = TipoQuestao.ComboMultiplaEscolha,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });

            //44
            await InserirNaBase(new Questao()
            {
                QuestionarioId = QUESTIONARIO_RELATORIO_DINAMICO_ENCAMINHAMENTO_NAAPA_ATENDIMENTO_7,
                Ordem = 1,
                Nome = QUESTAO_NOME_MES_ATENDIMENTO,
                NomeComponente = QUESTAO_NOME_COMPONENTE_DATA_DO_ATENDIMENTO,
                Tipo = TipoQuestao.ComboMultiplaEscolha,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });

            //id 45
            await InserirNaBase(new Questao()
            {
                QuestionarioId = QUESTIONARIO_RELATORIO_DINAMICO_ENCAMINHAMENTO_NAAPA_ATENDIMENTO_7,
                Ordem = 2,
                Nome = QUESTAO_NOME_MODALIDADE_ATENCAO,
                Obrigatorio = true,
                Tipo = TipoQuestao.Combo,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now,
                NomeComponente = QUESTAO_NOME_COMPONENTE_TIPO_DO_ATENDIMENTO
            });

            //id 46
            await InserirNaBase(new Questao()
            {
                QuestionarioId = QUESTIONARIO_RELATORIO_DINAMICO_ENCAMINHAMENTO_NAAPA_ATENDIMENTO_7,
                Ordem = 3,
                Nome = QUESTAO_NOME_PROCEDIMENTO_TRABALHO,
                Obrigatorio = true,
                Tipo = TipoQuestao.ComboMultiplaEscolha,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now,
                NomeComponente = QUESTAO_NOME_COMPONENTE_PROCEDIMENTO_DE_TRABALHO
            });

            //id 47
            await InserirNaBase(new Questao()
            {
                QuestionarioId = ID_QUESTIONARIO_INFORMACOES_ESTUDANTE,
                Ordem = 18,
                Nome = "Anexo",
                Obrigatorio = false,
                Tipo = TipoQuestao.Upload,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now,
                NomeComponente = QUESTAO_NOME_COMPONENTE_ANEXOS
            });

            //id 48
            await InserirNaBase(new Questao()
            {
                QuestionarioId = ID_QUESTIONARIO_NAAPA_ITINERANCIA,
                Ordem = 7,
                Nome = "Anexos",
                Obrigatorio = false,
                Tipo = TipoQuestao.Upload,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now,
                NomeComponente = QUESTAO_NOME_COMPONENTE_ANEXO_ITINERANCIA
            });

            //id 49
            await InserirNaBase(new Questao()
            {
                QuestionarioId = ID_QUESTIONARIO_NAAPA_ITINERANCIA,
                Ordem = 5,
                Nome = "Profissionais envolvidos",
                Obrigatorio = false,
                Tipo = TipoQuestao.ProfissionaisEnvolvidos,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now,
                NomeComponente = QUESTAO_NOME_COMPONENTE_PROFISSIONAIS_ENVOLVIDOS
            });

            //id 50
            await InserirNaBase(new Questao()
            {
                QuestionarioId = ID_QUESTIONARIO_INFORMACOES_ESTUDANTE,
                Ordem = 12,
                Nome = "Está em classe hospitalar",
                Obrigatorio = true,
                Tipo = TipoQuestao.Combo,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now,
                NomeComponente = QUESTAO_NOME_COMPONENTE_ESTA_EM_CLASSE_HOSPÍTALAR
            });


        }

        protected class FiltroNAAPADto
        {
            public FiltroNAAPADto()
            {
                TipoCalendarioId = TIPO_CALENDARIO_1;
                ConsiderarAnoAnterior = false;
                CriarPeriodoEscolar = true;
                CriarTurmaPadrao = true;
            }
            public string Perfil { get; set; }
            public Modalidade Modalidade { get; set; }
            public ModalidadeTipoCalendario TipoCalendario { get; set; }
            public long TipoCalendarioId { get; set; }
            public bool CriarPeriodoEscolar { get; set; }
            public string AnoTurma { get; set; }
            public bool ConsiderarAnoAnterior { get; set; }
            public long DreId { get; set; }
            public string CodigoUe { get; set; }
            public long TurmaId { get; set; }
            public int Situacao { get; set; }
            public int Prioridade { get; set; }
            public DateTime? DataAberturaQueixaInicio { get; set; }
            public DateTime? DataAberturaQueixaFim { get; set; }
            public bool CriarTurmaPadrao { get; set; }
        }
    }
}
