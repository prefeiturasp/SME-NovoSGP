namespace SME.SGP.Infra
{
    public static class RotasRabbitSgp
    {
        public const string RotaExcluirAulaRecorrencia = "sgp.aula.excluir.recorrencia";
        public const string RotaInserirAulaRecorrencia = "sgp.aula.cadastrar.recorrencia";
        public const string RotaAlterarAulaRecorrencia = "sgp.aula.alterar.recorrencia";
        public const string RotaAlterarAulaFrequenciaTratar = "sgp.aula.alterar.frequencia.tratar";
        public const string RotaNotificacaoUsuario = "sgp.notificacao.usuario";
        public const string RotaNotificacaoExclusaoAulasComFrequencia = "sgp.notificacao.aulas.exclusao.frequencia";
        public const string RotaCriarAulasInfatilAutomaticamente = "sgp.aulas.infantil.criar";
        public const string RotaSincronizarAulasInfatil = "sgp.aulas.infantil.sincronizar";
        public const string RotaRelatoriosProntos = "relatorios.prontos";
        public const string RotaRelatorioComErro = "relatorios.erro";
        public const string RotaRelatorioCorrelacaoCopiar = "relatorios.correlacao.copiar";
        public const string RotaRelatorioCorrelacaoInserir = "relatorios.correlacao.inserir";

        public const string RotaInserirPendenciaAula = "sgp.aulas.pendencias.inserir";
        public const string RotaNotificacaoNovaObservacaoCartaIntencoes = "sgp.notificacao.nova.observacao.cartaintencoes";
        public const string RotaNotificacaoNovaObservacaoDiarioBordo = "sgp.notificacao.nova.observacao.diariobordo";
        public const string RotaNovaNotificacaoObservacaoCartaIntencoes = "sgp.notificacao.nova.observacao.cartaintencoes";
        public const string RotaNotificacaoAlterarObservacaoDiarioBordo = "sgp.notificacao.alterar.observacao.diariobordo";
        public const string RotaExcluirNotificacaoObservacaoCartaIntencoes = "sgp.notificacao.excluir.observacao.cartaintencoes";
        public const string RotaNovaNotificacaoDevolutiva = "sgp.notificacao.nova.devolutiva";
        public const string RotaExcluirNotificacaoDevolutiva = "sgp.notificacao.excluir.devolutiva";
        public const string RotaExcluirNotificacaoDiarioBordo = "sgp.notificacao.excluir.diariobordo";
        public const string RotaExecutaPendenciasAula = "sgp.pendencias.aulas.executa";
        public const string RotaSincronizaComponetesCurricularesEol = "sgp.componentes.curriculares.eol.sincronizar";
        public const string RotaCalculoFrequenciaPorTurmaComponente = "sgp.frequencia.turma.componente";
        public const string RotaExecutaVerificacaoPendenciasGerais = "sgp.pendencias.gerais.executa.verificacao";

        public const string RotaExecutaExclusaoPendenciasAula = "sgp.pendencias.gerais.pendencias.aula.excluir";
        public const string RotaExecutaExclusaoPendenciasDiasLetivosInsuficientes = "sgp.pendencias.gerais.pendencias.calendario.excluir";
        public const string RotaExecutaExclusaoPendenciaParametroEvento = "sgp.pendencias.gerais.pendencias.evento.excluir";
        public const string RotaTrataNotificacoesNiveis = "sgp.notificacao.tratamento.niveiscargos";

        public const string RotaExecutaVerificacaoPendenciasProfessor = "sgp.pendencias.professor.executa.verificacao";

        public const string RotaExecutaVerificacaoPendenciasAusenciaFechamento = "sgp.pendencias.bimestre.ausencia.fechamento.verificacao";
        public const string RotaExecutaExclusaoPendenciasAusenciaAvaliacao = "sgp.pendencias.professor.avaliacao.excluir";

        public const string RotaExecutaExclusaoPendenciasAusenciaFechamento = "sgp.pendencias.bimestre.ausencia.fechamento.excluir";

        public const string RotaExecutaAtualizacaoSituacaoConselhoClasse = "sgp.conselho.classe.situacao.atualizar";
        public const string RotaNotificacaoAndamentoFechamento = "sgp.fechamento.andamento.notificar";

        public const string RotaNotificacaoInicioFimPeriodoFechamento = "sgp.fechamento.iniciofim.periodo.notificar";
               
        public const string RotaNotificacaoResultadoInsatisfatorio = "sgp.notificacao.nova.resultado.insatisfatorio";
        public const string RotaNotificacaoUeFechamentosInsuficientes = "sgp.fechamento.insuficiente.notificar";

        public const string RotaNotificacaoReuniaoPedagogica = "sgp.evento.reuniao.pedagogica.notificar";

        public const string RotaNotificacaoPeriodoFechamento = "sgp.periodo.fechamento.notificar";
        public const string RotaNotificacaoFrequenciaUe = "sgp.frequencia.ue.notificar";

        public const string RotaPendenciaAusenciaRegistroIndividual = "sgp.pendencias.professor.ausencia.registro.individual";
        public const string RotaAtualizarPendenciaAusenciaRegistroIndividual = "sgp.pendencias.professor.ausencia.registro.individual.atualizar";

        public const string RotaValidacaoAusenciaConciliacaoFrequenciaTurma = "sgp.frequencia.turma.conciliacao.validar";        

        public static string RotaNotificacaoRegistroConclusaoEncaminhamentoAEE => "notificacao.registro.conclusao.encaminhamentoaee";
        public static string RotaNotificacaoEncerramentoEncaminhamentoAEE => "notificacao.encerramento.encaminhamentoaee";
        public static string RotaNotificacaoDevolucaoEncaminhamentoAEE => "notificacao.devolucao.encaminhamentoaee";

        public static string EncerrarPlanoAEEEstudantesInativos => "plano.aee.encerrar.inativos";
        public static string GerarPendenciaValidadePlanoAEE => "plano.aee.pendencia.validade";

        public static string NotificarPlanoAEEExpirado => "plano.aee.notificar.expirados";
        public static string NotificarPlanoAEEEmAberto => "plano.aee.notificar.emaberto";
        public static string NotificarPlanoAEEReestruturado => "plano.aee.notificar.reestruturado";
        public static string NotificarCriacaoPlanoAEE => "plano.aee.notificar.criacao";
        public static string NotificarPlanoAEEEncerrado => "plano.aee.notificar.encerramento";
        public static string RotaNotificacaoRegistroItineranciaInseridoUseCase => "notificacao.registro.itinerancia.inserido";
        public static string SincronizaEstruturaInstitucionalUes => "sincroniza.estrtura.institucional.ues";
        public static string SincronizaEstruturaInstitucionalDreSync => "sgp.sincronizacao.institucional.dre.sync";
        public static string SincronizaEstruturaInstitucionalDreTratar => "sgp.sincronizacao.institucional.dre.tratar";
        public static string SincronizaEstruturaInstitucionalUeTratar => "sgp.sincronizacao.institucional.ue.tratar";
        public static string SincronizaEstruturaInstitucionalTurmasSync => "sgp.sincronizacao.institucional.turmas.sync";
        public static string SincronizaEstruturaInstitucionalTurmaTratar => "sgp.sincronizacao.institucional.turma.tratar";

        public static string SincronizaEstruturaInstitucionalTipoEscolaSync => "sgp.sincronizacao.institucional.tipoescola.sync";
        public static string SincronizaEstruturaInstitucionalTipoEscolaTratar => "sgp.sincronizacao.institucional.tipoescola.tratar";

        public static string SincronizaEstruturaInstitucionalCicloSync => "sgp.sincronizacao.institucional.ciclo.sync";
        public static string SincronizaEstruturaInstitucionalCicloTratar => "sgp.sincronizacao.institucional.ciclo.tratar";

        public static string ConsolidacaoFrequenciasTurmasCarregar => "sgp.frequencia.turma.carregar";
        public static string ConsolidarFrequenciasTurmasNoAno => "sgp.frequencia.turma.ano.consolidar";
        public static string ConsolidarFrequenciasPorTurma => "sgp.frequencia.turma.consolidar";

        public static string ConsolidacaoMatriculasTurmasDreCarregar => "sgp.matricula.turma.consolidar.dre.carregar";
        public static string SincronizarDresMatriculasTurmas => "sgp.matricula.turma.consolidar.dre.sync";
        public static string ConsolidacaoMatriculasTurmasUeCarregar => "sgp.matricula.turma.ue.carregar";
        public static string ConsolidacaoMatriculasTurmasCarregar => "sgp.matricula.turma.carregar";
        public static string ConsolidacaoMatriculasTurmasSync => "sgp.matricula.turma.sync";
    }
}
