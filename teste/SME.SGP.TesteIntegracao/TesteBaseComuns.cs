using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao
{
    public abstract class TesteBaseComuns : TesteBase
    {        
        protected readonly CollectionFixture collectionFixture;

        protected readonly TesteBaseUtils testeBaseUtils;

        protected TesteBaseComuns(CollectionFixture collectionFixture) : base(collectionFixture)
        {
            this.collectionFixture = collectionFixture ?? throw new ArgumentNullException(nameof(collectionFixture));
            testeBaseUtils = new TesteBaseUtils(collectionFixture);
        }

        protected void CriarClaimUsuario(string perfil, string pagina = "0", string registros = "10")
        {
            testeBaseUtils.CriarClaimUsuario(perfil, pagina, registros);
        }

        protected string ObterPerfilProfessor()
        {
            return testeBaseUtils.ObterPerfilProfessor();
        }
        
        protected string ObterPerfilCoordenadorNAAPA()
        {
            return testeBaseUtils.ObterPerfilCoordenadorNAAPA();
        }
        
        protected string ObterPerfilPsicologoEscolar()
        {
            return testeBaseUtils.ObterPerfilPsicologoEscolar();
        }
        
        protected string ObterPerfilPsicopedagogo()
        {
            return testeBaseUtils.ObterPerfilPsicopedagogo();
        }
        
        protected string ObterPerfilAssistenteSocial()
        {
            return testeBaseUtils.ObterPerfilAssistenteSocial();
        }

        protected string ObterPerfilCJ()
        {
            return testeBaseUtils.ObterPerfilCJ();
        }

        protected string ObterPerfilCoordenadorCefai()
        {
            return testeBaseUtils.ObterPerfilCoordenadorCefai();
        }
        protected string ObterPerfilPaai()
        {
            return testeBaseUtils.ObterPerfilPaai();
        }
        
        protected string ObterPerfilPaee()
        {
            return testeBaseUtils.ObterPerfilPaee();
        }

        protected string ObterPerfilAdmDre()
        {
            return testeBaseUtils.ObterPerfilAdmDre();
        }

        protected string ObterPerfilAdmSme()
        {
            return testeBaseUtils.ObterPerfilAdmSme();
        }

        protected string ObterPerfilCJInfantil()
        {
            return testeBaseUtils.ObterPerfilCJInfantil();
        }

        protected string ObterPerfilProfessorInfantil()
        {
            return testeBaseUtils.ObterPerfilProfessorInfantil();
        }

        protected string ObterPerfilCP()
        {
            return testeBaseUtils.ObterPerfilCP();
        }
        
        protected string ObterPerfilCEFAI()
        {
            return testeBaseUtils.ObterPerfilCEFAI();
        }

        protected string ObterPerfilAD()
        {
            return testeBaseUtils.ObterPerfilAD();
        }

        protected string ObterPerfilDiretor()
        {
            return testeBaseUtils.ObterPerfilDiretor();
        }
        protected string ObterPerfilPAP()
        {
            return testeBaseUtils.ObterPerfilPAP();
        }
        protected string ObterPerfilPOA_Portugues()
        {
            return testeBaseUtils.ObterPerfilPOA_Portugues();
        }

        protected async Task CriarPeriodoEscolarEncerrado()
        {
            await testeBaseUtils.CriarPeriodoEscolarEncerrado();
        }

        protected async Task CriarEvento(EventoLetivo letivo, DateTime dataInicioEvento, DateTime dataFimEvento, bool eventoEscolaAqui = false, long tipoEventoId = 1)
        {
            await testeBaseUtils.CriarEvento(letivo, dataInicioEvento, dataFimEvento, eventoEscolaAqui, tipoEventoId);
        }

        protected async Task CriarComunicadoAluno(string comunicado, int anoLetivo, string codigoDre, string codigoUe, string codigoTurma, string codigoAluno, long comunicadoId = 1)
        {
            await testeBaseUtils.CriarComunicadoAluno(comunicado, anoLetivo, codigoDre, codigoUe, codigoTurma, codigoAluno, comunicadoId);
        }

        protected async Task CriarEventoTipoResumido(string descricao, EventoLocalOcorrencia localOcorrencia, bool concomitancia, EventoTipoData tipoData, bool dependencia, EventoLetivo letivo, long codigo)
        {
            await CriarEventoTipo(descricao, localOcorrencia, concomitancia, tipoData, dependencia, letivo, true, false, codigo, false, false);
        }

        protected async Task CriarEventoTipo(string descricao, EventoLocalOcorrencia localOcorrencia, bool concomitancia, EventoTipoData tipoData, bool dependencia, EventoLetivo letivo, bool ativo, bool excluido, long codigo, bool somenteLeitura, bool eventoEscolaAqui)
        {
            await testeBaseUtils.CriarEventoTipo(descricao, localOcorrencia, concomitancia, tipoData, dependencia, letivo, ativo, excluido, codigo, somenteLeitura, eventoEscolaAqui);
        }

        protected async Task CriarEventoResumido(string nomeEvento, DateTime dataInicio, DateTime dataFim, EventoLetivo eventoLetivo, long tipoCalendarioId, long tipoEventoId, string dreId, string ueId, EntidadeStatus eventoStatus)
        {
            await CriarEvento(nomeEvento, dataInicio, dataFim, eventoLetivo, tipoCalendarioId, tipoEventoId, dreId, ueId, eventoStatus, null, null, null, null);
        }

        protected async Task CriarEvento(string nomeEvento, DateTime dataInicio, DateTime dataFim, EventoLetivo eventoLetivo, long tipoCalendarioId, long tipoEventoId, string dreId, string ueId, EntidadeStatus eventoStatus, long? workflowAprovacaoId, TipoPerfil? tipoPerfil, long? eventoPaiId, long? feriadoId, bool migrado = false)
        {
            await testeBaseUtils.CriarEvento(nomeEvento, dataInicio, dataFim, eventoLetivo, tipoCalendarioId, tipoEventoId, dreId, ueId, eventoStatus, workflowAprovacaoId, tipoPerfil, eventoPaiId, feriadoId, migrado);
        }

        protected async Task CriarAtribuicaoEsporadica(DateTime dataInicio, DateTime dataFim)
        {
            await testeBaseUtils.CriarAtribuicaoEsporadica(dataInicio, dataFim);
        }

        protected async Task CriarAtribuicaoCJ(Modalidade modalidade, long componenteCurricularId, bool substituir = true)
        {
            await testeBaseUtils.CriarAtribuicaoCJ(modalidade, componenteCurricularId, substituir);
        }

        protected async Task CriarAtribuicaoCJ(Modalidade modalidade, long componenteCurricularId, string professorRf, bool substituir = true)
        {
            await testeBaseUtils.CriarAtribuicaoCJ(modalidade, componenteCurricularId, professorRf, substituir);
        }

        protected async Task CriarUsuarios()
        {
            await testeBaseUtils.CriarUsuarios();
        }

        protected async Task CriarTurma(Modalidade modalidade, bool turmaHistorica = false, bool turmasMesmaUe = false, int tipoTurnoEol = 0)
        {
            await testeBaseUtils.CriarTurma(modalidade, turmaHistorica, turmasMesmaUe, tipoTurnoEol);
        }

        protected async Task CriarTurma(Modalidade modalidade, string anoTurma, bool turmaHistorica = false, 
            TipoTurma tipoTurma = TipoTurma.Regular, int tipoTurno = 0)
        {
            await testeBaseUtils.CriarTurma(modalidade, anoTurma, turmaHistorica, tipoTurma, tipoTurno);
        }

        protected async Task CriarTurma(Modalidade modalidade, string anoTurma, string codigoTurma, bool turmaHistorica = false)
        {
            await testeBaseUtils.CriarTurma(modalidade, anoTurma, codigoTurma, turmaHistorica);
        }
        protected async Task CriarTurma(Modalidade modalidade, string anoTurma, string codigoTurma, TipoTurma tipoTurma, bool turmaHistorica = false )
        {
            await testeBaseUtils.CriarTurma(modalidade, anoTurma, codigoTurma, tipoTurma, turmaHistorica);
        }

        protected async Task CriarTurma(Modalidade modalidade, string anoTurma, string codigoTurma, TipoTurma tipoTurma, long ueId,int anoLetivo,bool turmaHistorica = false, string nomeTurma = null)
        {
            await testeBaseUtils.CriarTurma(modalidade, anoTurma, codigoTurma, tipoTurma, ueId, anoLetivo, turmaHistorica, nomeTurma);
        }

        protected async Task CriarDreUe(string codigoDre,string codigoUe)
        {
            await testeBaseUtils.CriarDreUe(codigoDre, codigoUe);
        }

        protected async Task CriarDreUe(string codigoDre, string nomeDre, string codigoUe, string nomeUe)
        {
            await testeBaseUtils.CriarDreUe(codigoDre, nomeDre, codigoUe, nomeUe);
        }

        protected async Task CriarAtividadeAvaliativaFundamental(DateTime dataAvaliacao)
        {
            await testeBaseUtils.CriarAtividadeAvaliativaFundamental(dataAvaliacao);
        }

        protected async Task CrieTipoAtividade()
        {
            await testeBaseUtils.CrieTipoAtividade();
        }

        protected async Task CriarAtividadeAvaliativa(DateTime dataAvaliacao, string componente, string rf, bool ehCj, long idAtividade)
        {
            await testeBaseUtils.CriarAtividadeAvaliativa(dataAvaliacao, componente, rf, ehCj, idAtividade);
        }

        protected async Task CriarAtividadeAvaliativaFundamental(
                                    DateTime dataAvaliacao,
                                    string componente,
                                    TipoAvaliacaoCodigo tipoAvalicao = TipoAvaliacaoCodigo.AvaliacaoBimestral,
                                    bool ehRegencia = false,
                                    bool ehCj = false,
                                    string rf = USUARIO_PROFESSOR_CODIGO_RF_2222222)
        {
            await testeBaseUtils.CriarAtividadeAvaliativaFundamental(dataAvaliacao, componente, tipoAvalicao, ehRegencia, ehCj, rf);
        }

        protected async Task CriarAtividadeAvaliativaRegencia(string componente, string nomeComponente)
        {
            await testeBaseUtils.CriarAtividadeAvaliativaRegencia(componente, nomeComponente);
        }

        protected async Task CriarTipoCalendario(ModalidadeTipoCalendario tipoCalendario, bool considerarAnoAnterior = false, int semestre = SEMESTRE_1)
        {
            await testeBaseUtils.CriarTipoCalendario(tipoCalendario, considerarAnoAnterior, semestre);
        }

        protected async Task CriarItensComuns(bool criarPeriodo, DateTime dataInicio, DateTime dataFim, int bimestre, long tipoCalendarioId = 1)
        {
            await testeBaseUtils.CriarItensComuns(criarPeriodo, dataInicio, dataFim, bimestre, tipoCalendarioId);
        }

        protected async Task CriarDreUePerfilComponenteCurricular()
        {
            await testeBaseUtils.CriarDreUePerfilComponenteCurricular();
        }

        protected async Task CriaTipoAvaliacao(TipoAvaliacaoCodigo tipoAvalicao)
        {
            await testeBaseUtils.CriaTipoAvaliacao(tipoAvalicao);
        }

        protected async Task CriarAtividadeAvaliativa(DateTime dataAvaliacao, string componente, TipoAvaliacaoCodigo tipoAvalicao = TipoAvaliacaoCodigo.AvaliacaoBimestral, bool ehRegencia = false,
                                                      bool ehCj = false, string rf = USUARIO_PROFESSOR_CODIGO_RF_2222222)
        {
            await testeBaseUtils.CriarAtividadeAvaliativa(dataAvaliacao, componente, tipoAvalicao, ehRegencia, ehCj, rf);
        }

        protected async Task CriarDreUePerfil()
        {
            await testeBaseUtils.CriarDreUePerfil();
        }

        protected async Task CriarPeriodoEscolar(DateTime dataInicio, DateTime dataFim, int bimestre, long tipoCalendarioId = 1, bool considerarAnoAnterior = false)
        {
            await testeBaseUtils.CriarPeriodoEscolar(dataInicio, dataFim, bimestre, tipoCalendarioId, considerarAnoAnterior);
        }
        protected async Task CriarComponenteRegenciaInfantil()
        {
            await testeBaseUtils.CriarComponenteRegenciaInfantil();
        }

        protected async Task CriarComponenteCurricular()
        {
            await testeBaseUtils.CriarComponenteCurricular();
        }
        
        protected async Task CriarPeriodoEscolarCustomizadoQuartoBimestre(bool periodoEscolarValido = false)
        {
            await testeBaseUtils.CriarPeriodoEscolarCustomizadoQuartoBimestre(periodoEscolarValido);
        }

        protected async Task CriarAula(DateTime dataAula, RecorrenciaAula recorrenciaAula, TipoAula tipoAula, string professorRf, string turmaCodigo, string ueCodigo, string disciplinaCodigo, long tipoCalendarioId, bool aulaCJ = false)
        {
            await testeBaseUtils.CriarAula(dataAula, recorrenciaAula, tipoAula, professorRf, turmaCodigo, ueCodigo, disciplinaCodigo, tipoCalendarioId, aulaCJ);
        }

        protected async Task CriarPeriodoEscolarReabertura(long tipoCalendarioId)
        {
            await testeBaseUtils.CriarPeriodoEscolarReabertura(tipoCalendarioId);
        }

        protected async Task CriarPeriodoReabertura(long tipoCalendarioId, DateTime? dataInicio = null, DateTime? dataFim = null)
        {
            await testeBaseUtils.CriarPeriodoReabertura(tipoCalendarioId, dataInicio, dataFim);
        }

        protected async Task CrieConceitoValores()
        {
            await testeBaseUtils.CrieConceitoValores();
        }

        protected async Task CriarParametrosSistema(int ano)
        {
            await testeBaseUtils.CriarParametrosSistema(ano);
        }

        protected async Task CriarVariosTiposCalendariosEPeriodosEscolaresParaEjaCelpFundamental()
        {
            await testeBaseUtils.CriarVariosTiposCalendariosEPeriodosEscolaresParaEjaCelpFundamental();
        }
        protected async Task CriarConselhoClasseConsolidadoTurmaAlunos()
        {
            await testeBaseUtils.CriarConselhoClasseConsolidadoTurmaAlunos();
        }
        public DateTime ObterTerceiroSabadoDoMes(int ano, int mes)
        {
            return testeBaseUtils.ObterTerceiroSabadoDoMes(ano, mes);
        }
    }
}
