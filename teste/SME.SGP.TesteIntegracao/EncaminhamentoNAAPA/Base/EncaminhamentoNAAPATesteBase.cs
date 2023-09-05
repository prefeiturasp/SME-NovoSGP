using SME.SGP.Dominio;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Aplicacao;
using System.Text.RegularExpressions;

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
        protected const  long ID_QUESTAO_AGRUPAMENTO_PROMOCAO_CUIDADOS = 3;
        protected const long ID_QUESTAO_PRIORIDADE = 2;
        protected const long ID_QUESTAO_OBS_AGRUPAMENTO_PROMOCAO_CUIDADOS = 6;
        protected const long ID_QUESTAO_TIPO_ADOECE_COM_FREQUENCIA = 4;
        protected const long ID_QUESTAO_TIPO_DOENCA_CRONICA = 5;

        protected const long ID_QUESTAO_DATA_ATENDIMENTO = 7;
        protected const long ID_QUESTAO_TIPO_ATENDIMENTO = 8;
        protected const long ID_QUESTAO_PROCEDIMENTO_TRABALHO = 9;
        protected const long ID_QUESTAO_DESCRICAO_ATENDIMENTO = 10;
        protected const long ID_QUESTAO_DESCRICAO_PROCEDIMENTO_TRABALHO = 11;

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
        }


        private async Task CriarSecaoEncaminhamentoNAAPAQuestionario()
        {
            //Id 1
            await InserirNaBase(new SecaoEncaminhamentoNAAPA()
            {
                QuestionarioId = 1,
                Nome = "Informações do Estudante",
                NomeComponente = "INFORMACOES_ESTUDANTE",
                Etapa = 1,Ordem = 1,
                CriadoPor = SISTEMA_NOME,CriadoRF = SISTEMA_CODIGO_RF,CriadoEm = DateTime.Now
            });

            //Id 2
            await InserirNaBase(new SecaoEncaminhamentoNAAPA()
            {
                QuestionarioId = 2,
                Nome = "Questões apresentadas",
                NomeComponente = "QUESTOES_APRESENTADAS_INFANTIL",
                Etapa = 1,
                Ordem = 2,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });

            //Id 3
            await InserirNaBase(new SecaoEncaminhamentoNAAPA()
            {
                QuestionarioId = 3,
                Nome = "Itinerância",
                NomeComponente = "QUESTOES_ITINERACIA",
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
                await CriarTurma(filtro.Modalidade, filtro.AnoTurma, filtro.ConsiderarAnoAnterior, tipoTurno:2);
        }

        protected async Task CriarPeriodoEscolar(bool considerarAnoAnterior = false)
        {
            await CriarPeriodoEscolar(DATA_03_01_INICIO_BIMESTRE_1, DATA_29_04_FIM_BIMESTRE_1, BIMESTRE_1, TIPO_CALENDARIO_1, considerarAnoAnterior);

            await CriarPeriodoEscolar(DATA_02_05_INICIO_BIMESTRE_2, DATA_24_07_FIM_BIMESTRE_2, BIMESTRE_2, TIPO_CALENDARIO_1, considerarAnoAnterior);

            await CriarPeriodoEscolar(DATA_25_07_INICIO_BIMESTRE_3, DATA_02_10_FIM_BIMESTRE_3, BIMESTRE_3, TIPO_CALENDARIO_1, considerarAnoAnterior);

            await CriarPeriodoEscolar(DATA_03_10_INICIO_BIMESTRE_4, DATA_22_12_FIM_BIMESTRE_4, BIMESTRE_4, TIPO_CALENDARIO_1, considerarAnoAnterior);
        }

        protected IObterEncaminhamentoNAAPAUseCase ObterServicoListagemComFiltros()
        {
            return ServiceProvider.GetService<IObterEncaminhamentoNAAPAUseCase>();    
        }

        protected IObterObservacoesDeEncaminhamentoNAAPAUseCase ObterObservacoesDeEncaminhamentoNAAPA()
        {
            return ServiceProvider.GetService<IObterObservacoesDeEncaminhamentoNAAPAUseCase>();
        }
        protected IExcluirObservacoesDeEncaminhamentoNAAPAUseCase ExcluirObservacoesDeEncaminhamentoNAAPA()
        {
            return ServiceProvider.GetService<IExcluirObservacoesDeEncaminhamentoNAAPAUseCase>();
        }
        protected ISalvarObservacoesDeEncaminhamentoNAAPAUseCase SalvarObservacoesDeEncaminhamentoNAAPA()
        {
            return ServiceProvider.GetService<ISalvarObservacoesDeEncaminhamentoNAAPAUseCase>();
        }

        protected INotificarSobreTransferenciaUeDreAlunoTurmaDoEncaminhamentoNAAPAUseCase ObterServicoNotificacaoTransfAlunoDreUeDoEncaminhamentoNAAPA()
        {
            return ServiceProvider.GetService<INotificarSobreTransferenciaUeDreAlunoTurmaDoEncaminhamentoNAAPAUseCase>();
        }

        protected IRegistrarEncaminhamentoNAAPAUseCase ObterServicoRegistrarEncaminhamento()
        {
            return ServiceProvider.GetService<IRegistrarEncaminhamentoNAAPAUseCase>();    
        }

        protected IRegistrarEncaminhamentoItinerarioNAAPAUseCase ObterServicoRegistrarEncaminhamentoItinerario()
        {
            return ServiceProvider.GetService<IRegistrarEncaminhamentoItinerarioNAAPAUseCase>();
        }

        protected IObterSecoesItineranciaDeEncaminhamentoNAAPAUseCase ObterServicoListagemSecoesItineranciaEncaminhamentoNaapa()
        {
            return ServiceProvider.GetService<IObterSecoesItineranciaDeEncaminhamentoNAAPAUseCase>();
        }

        protected IExcluirSecaoItineranciaEncaminhamentoNAAPAUseCase ObterServicoExcluirSecaoItineranciaEncaminhamentoNAAPA()
        {
            return ServiceProvider.GetService<IExcluirSecaoItineranciaEncaminhamentoNAAPAUseCase>();
        }

        protected IExcluirEncaminhamentoNAAPAUseCase ObterServicoExcluirEncaminhamento()
        {
            return ServiceProvider.GetService<IExcluirEncaminhamentoNAAPAUseCase>();
        }
        
        protected IEncerrarEncaminhamentoNAAPAUseCase ObterServicoEncerrarEncaminhamento()
        {
            return ServiceProvider.GetService<IEncerrarEncaminhamentoNAAPAUseCase>();
        }

        protected IReabrirEncaminhamentoNAAPAUseCase ObterServicoReaberturaEncaminhamento()
        {
            return ServiceProvider.GetService<IReabrirEncaminhamentoNAAPAUseCase>();
        }

        protected IObterEncaminhamentoNAAPAPorIdUseCase ObterServicoObterEncaminhamentoNAAPAPorId()
        {
            return ServiceProvider.GetService<IObterEncaminhamentoNAAPAPorIdUseCase>();    
        }

        protected IObterQuestionarioItinerarioEncaminhamentoNAAPAUseCase ObterServicoObterQuestionarioItinerarioEncaminhamentoNAAPA()
        {
            return ServiceProvider.GetService<IObterQuestionarioItinerarioEncaminhamentoNAAPAUseCase>();
        }

        protected IAtualizarEnderecoDoEncaminhamentoNAAPAUseCase ObterServicoAtualizarEnderecoDoEncaminhamentoNAAPA()
        {
            return ServiceProvider.GetService<IAtualizarEnderecoDoEncaminhamentoNAAPAUseCase>();
        }

        protected IAtualizarTurmasProgramaDoEncaminhamentoNAAPAUseCase ObterServicoAtualizarTurmasProgramaDoEncaminhamentoNAAPA()
        {
            return ServiceProvider.GetService<IAtualizarTurmasProgramaDoEncaminhamentoNAAPAUseCase>();
        }

        protected IAtualizarTurmaDoEncaminhamentoNAAPAUseCase ObterServicoAtualizarTurmaDoEncaminhamentoNAAPA()
        {
            return ServiceProvider.GetService<IAtualizarTurmaDoEncaminhamentoNAAPAUseCase>();
        }

        protected INotificarSobreInativacaoAlunoTurmaDoEncaminhamentoNAAPAUseCase ObterServicoNotificacaoAtualizacaoMatriculaAlunoDoEncaminhamentoNAAPA()
        {
            return ServiceProvider.GetService<INotificarSobreInativacaoAlunoTurmaDoEncaminhamentoNAAPAUseCase>();
        }

        protected IExisteEncaminhamentoNAAPAAtivoParaAlunoUseCase ObterServicoExisteEncaminhamentoNAAPAAtivoParaAlunoUseCase()
        {
            return ServiceProvider.GetService<IExisteEncaminhamentoNAAPAAtivoParaAlunoUseCase>();
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
                Nome = "Atendimento não presencial",
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
                Nome = "Outro procedimento",
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });
        }

        private async Task CriarQuestionario()
        {
            await InserirNaBase(new Questionario()
            {
                Nome = "Questionário Encaminhamento NAAPA Etapa 1 Seção 1",
                Tipo = TipoQuestionario.EncaminhamentoNAAPA,
                CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
            });
            await InserirNaBase(new Questionario()
            {
                Nome = "Questionário Encaminhamento NAAPA Etapa 1 Seção 2 - Infantil",
                Tipo = TipoQuestionario.EncaminhamentoNAAPA,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });
            await InserirNaBase(new Questionario()
            {
                Nome = "Questionário Encaminhamento NAAPA Etapa 1 Seção 3 - Itinerância",
                Tipo = TipoQuestionario.EncaminhamentoNAAPA,
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
                QuestionarioId = 1,
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
                QuestionarioId = 1,
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
                QuestionarioId = 2,
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
                QuestionarioId = 2,
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
                QuestionarioId = 2,
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
                QuestionarioId = 2,
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
                QuestionarioId = 3,
                Ordem = 1,
                Nome = "Data do atendimento",
                Obrigatorio = true,
                Tipo = TipoQuestao.Combo,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now,
                NomeComponente = "DATA_DO_ATENDIMENTO"
            });

            //id 8
            await InserirNaBase(new Questao()
            {
                QuestionarioId = 3,
                Ordem = 2,
                Nome = "Tipo do atendimento",
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
                QuestionarioId = 3,
                Ordem = 3,
                Nome = "Procedimento de trabalho",
                Obrigatorio = true,
                Tipo = TipoQuestao.Combo,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now,
                NomeComponente = "PROCEDIMENTO_DE_TRABALHO"
            });

            //id 10
            await InserirNaBase(new Questao()
            {
                QuestionarioId = 3,
                Ordem = 4,
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
                QuestionarioId = 3,
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
                QuestionarioId = 1,
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
                QuestionarioId = 1,
                Ordem = 15,
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
                QuestionarioId = 1,
                Ordem = 2,
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
                QuestionarioId = 1,
                Ordem = 3,
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
                QuestionarioId = 1,
                Ordem = 4,
                Nome = "Descrição do encaminhamento",
                NomeComponente = "DESCRICAO_ENCAMINHAMENTO",
                Obrigatorio = false,
                Tipo = TipoQuestao.EditorTexto,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
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
