namespace SME.SGP.Infra
{
    public static class RotasRabbitSgp
    {
        public const string RotaNotificacaoUsuario = "sgp.notificacao.usuario";
        public const string RotaNotificacaoNovaObservacaoCartaIntencoes = "sgp.notificacao.nova.observacao.cartaintencoes";
        public const string RotaNotificacaoNovaObservacaoDiarioBordo = "sgp.notificacao.nova.observacao.diariobordo";
        public const string RotaNovaNotificacaoObservacaoCartaIntencoes = "sgp.notificacao.nova.observacao.cartaintencoes";
        public const string RotaNotificacaoAlterarObservacaoDiarioBordo = "sgp.notificacao.alterar.observacao.diariobordo";
        public const string RotaExcluirNotificacaoObservacaoCartaIntencoes = "sgp.notificacao.excluir.observacao.cartaintencoes";
        public const string RotaNovaNotificacaoDevolutiva = "sgp.notificacao.nova.devolutiva";
        public const string RotaExcluirNotificacaoDevolutiva = "sgp.notificacao.excluir.devolutiva";
        public const string RotaExcluirNotificacaoDiarioBordo = "sgp.notificacao.excluir.diariobordo";
        public const string RotaSincronizaComponetesCurricularesEol = "sgp.componentes.curriculares.eol.sincronizar";
        public const string RotaExecutaExclusaoPendenciasDiasLetivosInsuficientes = "sgp.pendencias.gerais.pendencias.calendario.excluir";
        public const string RotaExecutaExclusaoPendenciaParametroEvento = "sgp.pendencias.gerais.pendencias.evento.excluir";
        public const string RotaTrataNotificacoesNiveis = "sgp.notificacao.tratamento.niveiscargos";
        public const string RotaNotificacaoReuniaoPedagogica = "sgp.evento.reuniao.pedagogica.notificar";
        public const string RotaPendenciaAusenciaRegistroIndividual = "sgp.pendencias.professor.ausencia.registro.individual";
        public const string RotaAtualizarPendenciaAusenciaRegistroIndividual = "sgp.pendencias.professor.ausencia.registro.individual.atualizar";
        public const string NotificarCompensacaoAusencia = "sgp.compensacao.ausencia.notificar";
        public const string ExclusaoCompensacaoAusenciaAlunoEAula = "sgp.compensacao.ausencia.aluno.e.aula.excluir";
        public const string ExclusaoCompensacaoAusenciaPorIds = "sgp.compensacao.ausencia.excluir";
        public const string ConsolidarDevolutivasPorTurma = "sgp.consolidacao.devolutivas.turma";
        public const string ConsolidarDevolutivasPorUE = "sgp.consolidacao.devolutivas.turma.infantil.ue";
        public const string ConsolidarDevolutivasPorTurmaInfantil = "sgp.consolidacao.devolutivas.turma.infantil";
        public const string ConsolidarDiariosBordoCarregar = "sgp.consolidacao.diarios.bordo.carregar";
        public const string ConsolidarDiariosBordoPorUeTratar = "sgp.consolidacao.diarios.bordo.ue.tratar";
        public const string ConsolidarRegistrosPedagogicosPorUeTratar = "sgp.consolidacao.registros.pedagogicos.ue.tratar";
        public const string ConsolidarRegistrosPedagogicosPorTurmaTratar = "sgp.consolidacao.registros.pedagogicos.ue.turma.tratar";
        public const string SincronizaMediaRegistrosIndividuaisSync = "sgp.sincronizacao.media.registros.individuais.sync";
        public const string ConsolidarMediaRegistrosIndividuaisTurma = "sgp.consolidacao.media.registros.individuais.turma";
        public const string ConsolidarMediaRegistrosIndividuais = "sgp.consolidacao.media.registros.individuais";
        public const string ConsolidarAcompanhamentoAprendizagemAluno = "sgp.sincronizacao.acompanhamento.aprendizado.aluno";
        public const string ConsolidarAcompanhamentoAprendizagemAlunoPorUE = "sgp.sincronizacao.acompanhamento.aprendizado.aluno.ue";
        public const string ConsolidarAcompanhamentoAprendizagemAlunoTratar = "sgp.sincronizacao.acompanhamento.aprendizado.aluno.tratar";
        public const string ConsolidarRegistrosPedagogicos = "sgp.consolidacao.registros.pedagogicos";
        public const string RotaAgendamentoTratar = "sgp.agendamento.tratar";
        public const string RemoverArquivoArmazenamento = "sgp.armazenamento.remover.arquivo";
        public const string ExecutarAtualizacaoDasInformacoesPlanoAEE = "sgp.atualizar.informacoes.plano.aee";
        public const string ExecutarAtualizacaoDaTurmaDoPlanoAEE = "sgp.atualizar.turma.plano.aee";
        public const string ExecutarMigracaoRelatorioSemestralPAP = "sgp.executar.migracao.relatorio.semestral.pap";
        public const string ExecutarMigracaoRelatorioSemestralPAPPorAnoLetivo = "sgp.executar.migracao.relatorio.semestral.pap.ano.letivo";
        public const string ExecutarMigracaoRelatorioSemestralPAPPorId = "sgp.executar.migracao.relatorio.semestral.pap.id";
        public const string RotaNotificacaoInformativo = "sgp.notificacao.informativo";
        public const string RotaNotificacaoInformativoUsuario = "sgp.notificacao.informativo.usuario";
        public const string RotaExcluirNotificacaoInformativo = "sgp.notificacao.informativo.excluir";
        public const string RotaExcluirNotificacaoInformativoUsuario = "sgp.notificacao.informativo.usuario.excluir";
        public const string RotaExecutarExclusaoDasNotificacoesPeriodicamente = "sgp.notificacao.excluir.periodicamente";
        public const string RotaExecutarExclusaoDaNotificacao = "sgp.notificacao.excluir";
        public const string ExecutarAtualizacaoMapeamentoEstudantes = "sgp.mapeamento.estudantes.atualizar";
        public const string ExecutarAtualizacaoMapeamentoEstudantesBimestre = "sgp.mapeamento.estudantes.bimestre.atualizar";
        public const string ConsolidarPainelEdcacionalIdeb = "sgp.consolidacao.ideb.painel.educacional";

        #region Relatórios

        public const string RotaRelatoriosProntos = "sgp.relatorios.prontos.notificar";
        public const string RotaRelatoriosProntosApp = "sgp.relatorio.app.pronto.notificar";
        public const string RotaRelatorioCorrelacaoCopiar = "sgp.relatorios.correlacao.copiar";
        public const string RotaRelatorioCorrelacaoInserir = "sgp.relatorios.correlacao.inserir";
        public const string RotaRelatorioComErro = "sgp.relatorios.erro.notificar";
        public const string RotaRelatoriosComErroBoletim = "sgp.relatorios.erro.notificar.boletim";
        public const string RotaRelatoriosComErroBoletimDetalhado = "sgp.relatorios.erro.notificar.boletimdetalhado";
        public const string RotaRelatoriosComErroBoletimDetalhadoApp = "sgp.relatorios.app.erro.notificar.boletimdetalhado";
        public const string RotaRelatoriosComErroPlanoDeAula = "sgp.relatorios.erro.notificar.planodeaula";
        public const string RotaRelatoriosComErroRegistroIndividual = "sgp.relatorios.erro.notificar.registroindividual";
        public const string RotaRelatoriosComErroConselhoDeClasse = "sgp.relatorios.erro.notificar.conselhodeclasse";
        public const string RotaRelatoriosComErroRelatorioAcompanhamentoAprendizagem = "sgp.relatorios.erro.notificar.relatorioacompanhamentoaprendizagem";
        public const string RotaRelatoriosComErroAcompanhamentoFrequencia = "sgp.relatorios.erro.notificar.acompanhamentofrequencia";
        public const string RotaRelatoriosComErroCalendarioEscolar = "sgp.relatorios.erro.notificar.calendárioescolar";
        public const string RotaRelatoriosComErroRegistroItinerancia = "sgp.relatorios.erro.notificar.registroitinerancia";
        public const string RotaRelatoriosComErroPendencias = "sgp.relatorios.erro.notificar.pendencias";
        public const string RotaRelatoriosComErroAcompanhamentoFechamento = "sgp.relatorios.erro.notificar.acompanhamentofechamento";
        public const string RotaRelatoriosComErroParecerConclusivo = "sgp.relatorios.erro.notificar.parecerconclusivo";
        public const string RotaRelatoriosComErroNotasConceitosFinais = "sgp.relatorios.erro.notificar.notasconceitosfinais";
        public const string RotaRelatoriosComErroRelatorioAlteracaoNotas = "sgp.relatorios.erro.notificar.relatorioalteracaonotas";
        public const string RotaRelatoriosComErroPapResumos = "sgp.relatorios.erro.notificar.papresumos";
        public const string RotaRelatoriosComErroPapGraficos = "sgp.relatorios.erro.notificar.papgraficos";
        public const string RotaRelatoriosComErroPapRelatorioSemestral = "sgp.relatorios.erro.notificar.paprelatoriosemestral";
        public const string RotaRelatoriosComErroFrequencia = "sgp.relatorios.erro.notificar.frequencia";
        public const string RotaRelatoriosComErroCompensacaoAusencia = "sgp.relatorios.erro.notificar.compensacaoausencia";
        public const string RotaRelatoriosComErroControleGrade = "sgp.relatorios.erro.notificar.controlegrade";
        public const string RotaRelatoriosComErroControlePlanejamentoDiario = "sgp.relatorios.erro.notificar.controleplanejamentodiario";
        public const string RotaRelatoriosComErroDevolutivas = "sgp.relatorios.erro.notificar.devolutivas";
        public const string RotaRelatoriosComErroUsuarios = "sgp.relatorios.erro.notificar.usuarios";
        public const string RotaRelatoriosComErroAtribuicoes = "sgp.relatorios.erro.notificar.atribuicoes";
        public const string RotaRelatoriosComErroNotificacoes = "sgp.relatorios.erro.notificar.notificacoes";
        public const string RotaRelatoriosComErroEscolaAquiLeitura = "sgp.relatorios.erro.notificar.escolaaquileitura";
        public const string RotaRelatoriosComErroEscolaAquiAdesao = "sgp.relatorios.erro.notificar.escolaaquiadesao";
        public const string RotaRelatoriosComErroAtaFinalResultados = "sgp.relatorios.erro.notificar.atafinalresultados";
        public const string RotaRelatoriosComErroHistoricoEscolar = "sgp.relatorios.erro.notificar.historicoescolar";
        public const string RotaRelatoriosComErroAtaBimestral = "sgp.relatorios.erro.notificar.atabimestralresultados";
        public const string RotaRelatoriosComErroRelatorioAcompanhamentoRegistrosPedagogicos = "sgp.relatorios.erro.notificar.relatorioacompanhamentoregistrospedagogicos";

        #endregion

        public const string WorkflowAprovacaoExcluir = "sgp.workflow.aprovacao.excluir";
        public const string DiarioBordoDaAulaExcluir = "sgp.diarios.bordo.aula.excluir";
        public const string ExecutarTipoCalendario = "sgp.executar.tipo.calendario";
        public const string ExecutarGravarRecorrencia = "sgp.executar.gravar.recorrencia";
        public const string AlterarRecorrenciaEventos = "sgp.alterar.recorrencia.eventos";
        public const string SincronizarObjetivosComJurema = "sgp.sincronizar.objetivos.com.jurema";
        public const string NotificarAlunosFaltososBimestre = "sgp.alunos.faltosos.bimestre.notificacao";
        public const string SincronizarComponentesCurriculares = "sgp.sincronizar.componentes.curriculares";
        public const string SincronizarComponentesCurricularesEol = "sgp.sincronizar.componentes.curriculares.eol";
        public const string SyncGeralGoogleClassroom = "sgp.sync.geral.google.classroom";
        public const string SyncGsaGoogleClassroom = "sgp.sync.gsa.google.classroom";
        public const string SyncSerapEstudantesProvas = "sgp.sync.serap.estudantes.provas";
        public const string TratarNotificacoesNiveisCargos = "sgp.tratar.notificacoes.niveis.cargos";
        public const string TratarNotificacoesNiveisCargosDre = "sgp.tratar.notificacoes.niveis.cargos.dre";
        public const string TratarNotificacoesNiveisCargosUe = "sgp.tratar.notificacoes.niveis.cargos.ue";
        public const string AjusteImagesAcompanhamentoAprendizagemAlunoCarregar = "sgp.acompanhamento.aprendizado.aluno.imagens.ajuste.carregar";
        public const string AjusteImagesAcompanhamentoAprendizagemAlunoSync = "sgp.acompanhamento.aprendizado.aluno.imagens.ajuste.sync";
        public const string AtualizaUltimoLoginUsuario = "sgp.usuario.login.atualizar";

        #region Agendamento

        public const string RotaMuralAvisosSyncAgendado = "sgp.mural.avisos.sync.agendado";
        public const string RotaAtividadesSyncAgendado = "sgp.atividade.avaliativa.sync.agendado";
        public const string RotaNotaAtividadesSyncAgendado = "sgp.nota.atividade.avaliativa.sync.agendado";

        #endregion

        #region Diário de bordo

        public const string RotaReprocessarDiarioBordoPendenciaDevolutivaPorDre = "sgp.diario.bordo.pendencia.devolutiva.dre";
        public const string RotaReprocessarDiarioBordoPendenciaDevolutivaPorUe = "sgp.diario.bordo.pendencia.devolutiva.ue";
        public const string RotaReprocessarDiarioBordoPendenciaDevolutivaPorTurma = "sgp.diario.bordo.pendencia.devolutiva.turma";
        public const string RotaReprocessarDiarioBordoPendenciaDevolutivaPorComponente = "sgp.diario.bordo.pendencia.devolutiva.componente";

        #endregion

        #region Atribuição de responsáveis

        public const string RemoverAtribuicaoResponsaveis = "sgp.remover.atribuicao.responsaveis";
        public const string RemoverAtribuicaoResponsaveisSupervisorPorDre = "sgp.remover.atribuicao.responsaveis.supervisor.dre";
        public const string RemoverAtribuicaoResponsaveisPAAIPorDre = "sgp.remover.atribuicao.responsaveis.paai.dre";
        public const string RemoverAtribuicaoResponsaveisASPPorDre = "sgp.remover.atribuicao.responsaveis.aspp.dre";
        public const string GerarCacheAtribuicaoResponsaveis = "sgp.gerar.atribuicao.responsaveis.cache";

        #endregion

        public const string ExecutarGravarObservacaoHistorioEscolar = "sgp.executar.gravar.observacao.historico.escolar";

        public const string SincronizarAbrangencia = "sgp.abrangencia.worker.sync";
        //public const string SincronizarAbrangenciaTratar = "sgp.abrangencia.tratar.worker";
    }
}
