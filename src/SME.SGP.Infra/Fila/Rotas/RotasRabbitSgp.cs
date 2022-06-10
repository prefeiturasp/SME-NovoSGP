namespace SME.SGP.Infra
{
    public static class RotasRabbitSgp
    {
        //-> Notificação
        public const string RotaNotificacaoUsuario = "sgp.notificacao.usuario";        
        public const string RotaNotificacaoNovaObservacaoCartaIntencoes = "sgp.notificacao.nova.observacao.cartaintencoes";
        public const string RotaNotificacaoNovaObservacaoDiarioBordo = "sgp.notificacao.nova.observacao.diariobordo";
        public const string RotaNovaNotificacaoObservacaoCartaIntencoes = "sgp.notificacao.nova.observacao.cartaintencoes";
        public const string RotaNotificacaoAlterarObservacaoDiarioBordo = "sgp.notificacao.alterar.observacao.diariobordo";
        public const string RotaExcluirNotificacaoObservacaoCartaIntencoes = "sgp.notificacao.excluir.observacao.cartaintencoes";
        public const string RotaNovaNotificacaoDevolutiva = "sgp.notificacao.nova.devolutiva";
        public const string RotaExcluirNotificacaoDevolutiva = "sgp.notificacao.excluir.devolutiva";
        public const string RotaExcluirNotificacaoDiarioBordo = "sgp.notificacao.excluir.diariobordo";
        public const string RotaTrataNotificacoesNiveis = "sgp.notificacao.tratamento.niveiscargos";
        public const string RotaNotificacaoResultadoInsatisfatorio = "sgp.notificacao.nova.resultado.insatisfatorio";        
        public const string RotaNotificacaoReuniaoPedagogica = "sgp.evento.reuniao.pedagogica.notificar";
        public const string RotaNotificacaoFrequenciaUe = "sgp.frequencia.ue.notificar";

        public const string RotaSincronizaComponetesCurricularesEol = "sgp.componentes.curriculares.eol.sincronizar";

        public const string RotaAtualizarParecerConclusivoAlunoPorDre = "sgp.conselho.classe.aluno.parecer.atualizar.dre";
        public const string RotaAtualizarParecerConclusivoAlunoPorUe = "sgp.conselho.classe.aluno.parecer.atualizar.ue";
        public const string RotaAtualizarParecerConclusivoAlunoPorTurma = "sgp.conselho.classe.aluno.parecer.atualizar.turma";
        public const string RotaAtualizarParecerConclusivoAluno = "sgp.conselho.classe.aluno.parecer.atualizar";
        public const string RotaExecutaAtualizacaoSituacaoConselhoClasse = "sgp.conselho.classe.situacao.atualizar";

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

        public const string RotaRabbitSRDeadletterTratar = "sgp.sr.rabbit.deadletter.tratar";
        public const string RotaRabbitSRDeadletterSync = "sgp.sr.rabbit.deadletter.sync";

        public const string WorkflowAprovacaoExcluir = "sgp.workflow.aprovacao.excluir";

        public const string VarreduraFechamentosTurmaDisciplinaEmProcessamentoPendentes = "sgp.fechamento.turma.disciplina.processamento.varredura";
        
        public const string ExecutarTipoCalendario = "sgp.executar.tipo.calendario";
        public const string ExecutarGravarRecorrencia = "sgp.executar.gravar.recorrencia";
        public const string GerarNotificacaoAlteracaoLimiteDias = "sgp.gerar.notificacao.alteracao.limite.dias";        
        public const string AlterarPeriodosComHierarquiaInferiorFechamento = "sgp.alterar.periodo.hierarquia.inferior.fechamento";
        public const string AlterarRecorrenciaEventos = "sgp.alterar.recorrencia.eventos";
        
        public const string SincronizarObjetivosComJurema = "sgp.sincronizar.objetivos.com.jurema";
        public const string NotificarAlunosFaltososBimestre = "sgp.alunos.faltosos.bimestre.notificacao";
        public const string NotificacoesNiveisCargos = "sgp.notificacoes.nivel.cargos";
        public const string SincronizarComponentesCurriculares = "sgp.sincronizar.componentes.curriculares";
        public const string SincronizarComponentesCurricularesEol = "sgp.sincronizar.componentes.curriculares.eol";
        public const string SyncGeralGoogleClassroom = "sgp.sync.geral.google.classroom";
        public const string SyncGsaGoogleClassroom = "sgp.sync.gsa.google.classroom";
        public const string SyncSerapEstudantesProvas = "sgp.sync.serap.estudantes.provas";
        public const string TratarNotificacoesNiveisCargos = "sgp.tratar.notificacoes.niveis.cargos";

        public const string AjusteImagesAcompanhamentoAprendizagemAlunoCarregar = "sgp.acompanhamento.aprendizado.aluno.imagens.ajuste.carregar";
        public const string AjusteImagesAcompanhamentoAprendizagemAlunoSync = "sgp.acompanhamento.aprendizado.aluno.imagens.ajuste.sync";
    }
}
