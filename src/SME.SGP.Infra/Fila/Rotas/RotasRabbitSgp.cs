﻿namespace SME.SGP.Infra
{
    public static class RotasRabbitSgp
    {
        public const string RotaNotificacaoUsuario = "sgp.notificacao.usuario";
        public const string RotaNotificacaoExclusaoAulasComFrequencia = "sgp.notificacao.aulas.exclusao.frequencia";

        public const string RotaInserirPendenciaAula = "sgp.aulas.pendencias.inserir";
        public const string RotaNotificacaoNovaObservacaoCartaIntencoes = "sgp.notificacao.nova.observacao.cartaintencoes";
        public const string RotaNotificacaoNovaObservacaoDiarioBordo = "sgp.notificacao.nova.observacao.diariobordo";
        public const string RotaNovaNotificacaoObservacaoCartaIntencoes = "sgp.notificacao.nova.observacao.cartaintencoes";
        public const string RotaNotificacaoAlterarObservacaoDiarioBordo = "sgp.notificacao.alterar.observacao.diariobordo";
        public const string RotaExcluirNotificacaoObservacaoCartaIntencoes = "sgp.notificacao.excluir.observacao.cartaintencoes";
        public const string RotaNovaNotificacaoDevolutiva = "sgp.notificacao.nova.devolutiva";
        public const string RotaExcluirNotificacaoDevolutiva = "sgp.notificacao.excluir.devolutiva";
        public const string RotaExcluirNotificacaoDiarioBordo = "sgp.notificacao.excluir.diariobordo";
        public const string RotaSincronizaComponetesCurricularesEol = "sgp.componentes.curriculares.eol.sincronizar";

        public const string RotaExecutaPendenciasAula = "sgp.pendencias.aulas.executa";
        public const string RotaExecutaPendenciasAulaDiarioBordo = "sgp.pendencias.aulas.diario.bordo.executa";
        public const string RotaExecutaPendenciasAulaDiarioBordoTurma = "sgp.pendencias.aulas.diario.bordo.turma.executa";
        public const string RotaExecutaPendenciasAulaDiarioBordoTurmaAulaComponente = "sgp.pendencias.aulas.diario.bordo.turma.aula.componente.executa";
        public const string RotaExecutaPendenciasAulaAvaliacao = "sgp.pendencias.aulas.avaliacao.executa";
        public const string RotaExecutaPendenciasAulaFrequencia = "sgp.pendencias.aulas.frequencia.executa";
        public const string RotaExecutaPendenciasAulaPlanoAula = "sgp.pendencias.aulas.plano.aula.executa";
        public const string RotaExecutaVerificacaoPendenciasGerais = "sgp.pendencias.gerais.executa.verificacao";
        public const string RotaExecutaExclusaoPendenciasAula = "sgp.pendencias.gerais.pendencias.aula.excluir";
        public const string RotaExecutaExclusaoPendenciaDiarioBordoAula = "sgp.pendencias.diario.bordo.aula.excluir";
        public const string RotaExecutaExclusaoPendenciasDiasLetivosInsuficientes = "sgp.pendencias.gerais.pendencias.calendario.excluir";
        public const string RotaExecutaExclusaoPendenciaParametroEvento = "sgp.pendencias.gerais.pendencias.evento.excluir";
        public const string RotaExecutaVerificacaoPendenciasProfessor = "sgp.pendencias.professor.executa.verificacao";
        public const string RotaExecutaVerificacaoPendenciasAusenciaFechamento = "sgp.pendencias.bimestre.ausencia.fechamento.verificacao";
        public const string RotaExecutaExclusaoPendenciasAusenciaAvaliacao = "sgp.pendencias.professor.avaliacao.excluir";
        public const string RotaExecutaExclusaoPendenciasAusenciaFechamento = "sgp.pendencias.bimestre.ausencia.fechamento.excluir";

        public const string RotaTratarAtribuicaoPendenciaUsuarios = "sgp.pendencias.atribuicao.tratar";
        public const string RotaCargaAtribuicaoPendenciaPerfilUsuario = "sgp.pendencia.perfil.usuario.atribuicao.carga";
        public const string RotaRemoverAtribuicaoPendenciaUsuarios = "sgp.pendencias.perfil.usuario.remover.atribuicao";
        public const string RotaRemoverAtribuicaoPendenciaUsuariosUe = "sgp.pendencias.perfil.usuario.remover.atribuicao.ue";
        public const string RotaRemoverAtribuicaoPendenciaUsuariosUeFuncionario = "sgp.pendencias.perfil.usuario.remover.atribuicao.ue.funcionario";

        public const string RotaTrataNotificacoesNiveis = "sgp.notificacao.tratamento.niveiscargos";

        public const string RotaAtualizarParecerConclusivoAlunoPorDre = "sgp.conselho.classe.aluno.parecer.atualizar.dre";
        public const string RotaAtualizarParecerConclusivoAlunoPorUe = "sgp.conselho.classe.aluno.parecer.atualizar.ue";
        public const string RotaAtualizarParecerConclusivoAlunoPorTurma = "sgp.conselho.classe.aluno.parecer.atualizar.turma";
        public const string RotaAtualizarParecerConclusivoAluno = "sgp.conselho.classe.aluno.parecer.atualizar";

        public const string RotaExecutaAtualizacaoSituacaoConselhoClasse = "sgp.conselho.classe.situacao.atualizar";
        public const string RotaNotificacaoAndamentoFechamento = "sgp.fechamento.andamento.notificar";
        public const string RotaNotificacaoAndamentoFechamentoPorUe = "sgp.fechamento.andamento.notificar.ue";

        public const string RotaNotificacaoInicioFimPeriodoFechamento = "sgp.fechamento.abertura.iniciofim.periodo.notificar";
        public const string RotaNotificacaoInicioPeriodoFechamentoUE = "sgp.fechamento.abertura.inicio.periodo.notificar.ue";
        public const string RotaNotificacaoFimPeriodoFechamentoUE = "sgp.fechamento.abertura.fim.periodo.notificar.ue";
        public const string RotaGeracaoPendenciasFechamento = "sgp.fechamento.pendencias.gerar";
               
        public const string RotaNotificacaoResultadoInsatisfatorio = "sgp.notificacao.nova.resultado.insatisfatorio";
        public const string RotaNotificacaoUeFechamentosInsuficientes = "sgp.fechamento.insuficiente.notificar";

        public const string RotaNotificacaoReuniaoPedagogica = "sgp.evento.reuniao.pedagogica.notificar";

        public const string RotaNotificacaoPeriodoFechamento = "sgp.periodo.fechamento.notificar";

        public const string RotaNotificacaoFrequenciaUe = "sgp.frequencia.ue.notificar";

        public const string RotaPendenciaAusenciaRegistroIndividual = "sgp.pendencias.professor.ausencia.registro.individual";
        public const string RotaAtualizarPendenciaAusenciaRegistroIndividual = "sgp.pendencias.professor.ausencia.registro.individual.atualizar";

        public const string RotaNotificacaoRegistroConclusaoEncaminhamentoAEE = "notificacao.registro.conclusao.encaminhamentoaee";
        public const string RotaNotificacaoEncerramentoEncaminhamentoAEE = "notificacao.encerramento.encaminhamentoaee";
        public const string RotaNotificacaoDevolucaoEncaminhamentoAEE = "notificacao.devolucao.encaminhamentoaee";
        public const string RotaEncerrarEncaminhamentoAEEAutomaticoSync = "sgp.encaminhamento.aee.encerrar.automatico.sync";
        public const string RotaValidarEncerrarEncaminhamentoAEEAutomatico = "sgp.encaminhamento.aee.encerrar.automatico.validar";
        public const string RotaEncerrarEncaminhamentoAEEEncerrarAutomatico = "sgp.encaminhamento.aee.encerrar.automatico.encerrar";

        public const string EncerrarPlanoAEEEstudantesInativos = "plano.aee.encerrar.inativos";
        public const string GerarPendenciaValidadePlanoAEE = "plano.aee.pendencia.validade";

        public const string NotificarPlanoAEEExpirado = "plano.aee.notificar.expirados";
        public const string NotificarPlanoAEEEmAberto = "plano.aee.notificar.emaberto";
        public const string NotificarPlanoAEEReestruturado = "plano.aee.notificar.reestruturado";
        public const string NotificarCriacaoPlanoAEE = "plano.aee.notificar.criacao";
        public const string NotificarPlanoAEEEncerrado = "plano.aee.notificar.encerramento";
        public const string RotaNotificacaoRegistroItineranciaInseridoUseCase = "notificacao.registro.itinerancia.inserido";

        public const string NotificarCompensacaoAusencia = "sgp.compensacao.ausencia.notificar";

        public const string ConsolidacaoMatriculasTurmasDreCarregar = "sgp.matricula.turma.consolidar.dre.carregar";
        public const string SincronizarDresMatriculasTurmas = "sgp.matricula.turma.consolidar.dre.sync";
        public const string ConsolidacaoMatriculasTurmasCarregar = "sgp.matricula.turma.carregar";
        public const string ConsolidacaoMatriculasTurmasSync = "sgp.matricula.turma.sync";

        public const string ConsolidarTurmaSync = "sgp.consolidacao.turma.sync";
        public const string ConsolidarTurmaTratar = "sgp.consolidacao.turma.tratar";

        public const string RotaNotificacaoAlunosFaltosos = "sgp.aulas.alunos.faltosos.notificar";

        public const string RotaNotificacaoFechamentoReabertura = "sgp.fechamento.reabertura.notificacao";
        public const string RotaNotificacaoFechamentoReaberturaDRE = "sgp.fechamento.reabertura.notificacao.dre";
        public const string RotaNotificacaoFechamentoReaberturaUE = "sgp.fechamento.reabertura.notificacao.ue";

        public const string RotaNotificacaoAulasPrevistasSync = "sgp.aulas.previstas.notificacao.sync";
        public const string RotaNotificacaoAulasPrevistas = "sgp.aulas.previstas.notificacao";

        public const string RotaRabbitDeadletterSync = "sgp.rabbit.deadletter.sync";
        public const string RotaRabbitDeadletterTratar = "sgp.rabbit.deadletter.tratar";

        public const string CarregarDadosAlunosFrequenciaMigracao = "sgp.migracao.frequencia.alunos.carregar";
        public const string SincronizarDadosAlunosFrequenciaMigracao = "sgp.migracao.frequencia.alunos.sync";
      
        public const string SincronizaDevolutivasPorTurmaInfantilSync = "sgp.consolidacao.devolutivas.turma.sync";
        public const string ConsolidarDevolutivasPorTurma = "sgp.consolidacao.devolutivas.turma";
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

        public const string RotaMuralAvisosSync = "sgp.mural.avisos.sync";
        public const string RotaAtividadesSync = "sgp.atividade.avaliativa.sync";
        public const string RotaAtividadesNotasSync = "sgp.atividade.avaliativa.notas.sync";

        public const string RotaAgendamentoTratar = "sgp.agendamento.tratar";

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

        public const string RotaRabbitSRDeadletterTratar = "sgp.sr.rabbit.deadletter.tratar";
        public const string RotaRabbitSRDeadletterSync = "sgp.sr.rabbit.deadletter.sync";

        public const string WorkflowAprovacaoExcluir = "sgp.workflow.aprovacao.excluir";
        public const string NotificacoesDaAulaExcluir = "sgp.notificacoes.aula.excluir";
        public const string FrequenciaDaAulaExcluir = "sgp.frequencia.aula.excluir";
        public const string PlanoAulaDaAulaExcluir = "sgp.plano.aula.excluir";
        public const string AnotacoesFrequenciaDaAulaExcluir = "sgp.anotacoes.frequencia.aula.excluir";
        public const string DiarioBordoDaAulaExcluir = "sgp.diarios.bordo.aula.excluir";
        

        public const string RotaConsolidacaoDiariaDashBoardFrequencia = "sgp.consolidacao.diaria.frequencia.dashboard";
        public const string RotaConsolidacaoSemanalDashBoardFrequencia = "sgp.consolidacao.semanal.frequencia.dashboard";
        public const string RotaConsolidacaoMensalDashBoardFrequencia = "sgp.consolidacao.mensal.frequencia.dashboard";

        public const string VarreduraFechamentosTurmaDisciplinaEmProcessamentoPendentes = "sgp.fechamento.turma.disciplina.processamento.varredura";
        public const string NotificacaoFrequencia = "sgp.notificacoes.frequencia";
        public const string ExecutarTipoCalendario = "sgp.executar.tipo.calendario";
        public const string ExecutarGravarRecorrencia = "sgp.executar.gravar.recorrencia";
        public const string GerarNotificacaoAlteracaoLimiteDias = "sgp.gerar.notificacao.alteracao.limite.dias";
        public const string VerificarPendenciasFechamentoTurmaDisciplina = "sgp.verificar.pendencias.fechamento.turma.disciplina";
        public const string AlterarPeriodosComHierarquiaInferiorFechamento = "sgp.alterar.periodo.hierarquia.inferior.fechamento";
        public const string AlterarRecorrenciaEventos = "sgp.alterar.recorrencia.eventos";
        

        public const string NotifificarRegistroFrequencia = "sgp.registro.frequencia.notificacao";
        public const string SincronizarObjetivosComJurema = "sgp.sincronizar.objetivos.com.jurema";
        public const string NotificarAlunosFaltososBimestre = "sgp.alunos.faltosos.bimestre.notificacao";
        public const string NotificacoesNiveisCargos = "sgp.notificacoes.nivel.cargos";
        public const string SincronizarComponentesCurriculares = "sgp.sincronizar.componentes.curriculares";
        public const string SincronizarComponentesCurricularesEol = "sgp.sincronizar.componentes.curriculares.eol";
        public const string SyncGeralGoogleClassroom = "sgp.sync.geral.google.classroom";
        public const string SyncGsaGoogleClassroom = "sgp.sync.gsa.google.classroom";
        public const string SyncSerapEstudantesProvas = "sgp.sync.serap.estudantes.provas";
        public const string TratarNotificacoesNiveisCargos = "sgp.tratar.notificacoes.niveis.cargos";
        public const string PendenciasGerais = "sgp.pendencias.gerais";
        public const string PendenciasGeraisAulas = "sgp.pendencias.gerais.aula";
        public const string PendenciasGeraisCalendario = "sgp.pendencias.gerais.calendario";
        public const string PendenciasGeraisEventos = "sgp.pendencias.gerais.evento";

        public const string NotificacaoPeriodoFechamentoReaberturaIniciando = "sgp.periodo.fechamento.reabertura.iniciando";
        public const string NotificacaoPeriodoFechamentoReaberturaEncerrando = "sgp.periodo.fechamento.reabertura.encerrando";
        public const string NotificacaoPeriodoFechamentoReaberturaUE = "sgp.periodo.fechamento.reabertura.ue";
        public const string VerificaPendenciasFechamentoTurma = "sgp.fechamento.turma.pendencia.verificar";

        public const string AjusteImagesAcompanhamentoAprendizagemAlunoCarregar = "sgp.acompanhamento.aprendizado.aluno.imagens.ajuste.carregar";
        public const string AjusteImagesAcompanhamentoAprendizagemAlunoSync = "sgp.acompanhamento.aprendizado.aluno.imagens.ajuste.sync";
    }
}
