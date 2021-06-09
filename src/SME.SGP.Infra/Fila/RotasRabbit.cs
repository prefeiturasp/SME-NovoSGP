namespace SME.SGP.Infra
{
    public static class RotasRabbit
    {
        public static string ExchangeServidorRelatorios => "sme.sr.workers.relatorios";
        public static string ExchangeSgp => "sme.sgp.workers";

        public static string FilaSgp => "sme.sgp.clients";
        
        public static string WorkerRelatoriosSgp => "sme.sr.workers.sgp";

        public static string RotaRelatoriosSolicitados => "relatorios.solicitados";
        public static string RotaRelatoriosProntos => "relatorios.prontos";

        public static string RotaExcluirAulaRecorrencia => "aula.excluir.recorrencia";
        public static string RotaInserirAulaRecorrencia => "aula.cadastrar.recorrencia";
        public static string RotaAlterarAulaRecorrencia => "aula.alterar.recorrencia";
        public static string RotaNotificacaoUsuario => "notificacao.usuario";
        public static string RotaNotificacaoExclusaoAulasComFrequencia => "notificacao.aulas.exclusao.frequencia";
        public static string RotaCriarAulasInfatilAutomaticamente => "aulas.infantil.criar";
        public static string RotaSincronizarAulasInfatil => "aulas.infantil.sincronizar";
        public static string RotaRelatorioComErro => "relatorios.erro";
        public static string RotaRelatorioCorrelacaoCopiar => "relatorios.correlacao.copiar";
        public static string RotaRelatorioCorrelacaoInserir => "relatorios.correlacao.inserir";

        public static string RotaInserirPendenciaAula => "aulas.pendencias.inserir";
        public static string RotaNotificacaoNovaObservacaoCartaIntencoes => "notificacao.nova.observacao.cartaintencoes";
        public static string RotaNotificacaoNovaObservacaoDiarioBordo => "notificacao.nova.observacao.diariobordo";
        public static string RotaNovaNotificacaoObservacaoCartaIntencoes => "notificacao.nova.observacao.cartaintencoes";
        public static string RotaNotificacaoAlterarObservacaoDiarioBordo => "notificacao.alterar.observacao.diariobordo";
        public static string RotaExcluirNotificacaoObservacaoCartaIntencoes => "notificacao.excluir.observacao.cartaintencoes";
        public static string RotaNovaNotificacaoDevolutiva => "notificacao.nova.devolutiva";
        public static string RotaExcluirNotificacaoDevolutiva => "notificacao.excluir.devolutiva";
        public static string RotaExcluirNotificacaoDiarioBordo => "notificacao.excluir.diariobordo";
        public static string RotaExecutaPendenciasAula => "pendencias.aulas.executa";
        public static string RotaSincronizaComponetesCurricularesEol => "componentes.curriculares.eol.sincronizar";
        public static string RotaCalculoFrequenciaPorTurmaComponente => "frequencia.turma.componente";
        public static string RotaExecutaVerificacaoPendenciasGerais => "pendencias.gerais.executa.verificacao";

        public static string RotaExecutaExclusaoPendenciasAula => "pendencias.gerais.pendencias.aula.excluir";
        public static string RotaExecutaExclusaoPendenciasDiasLetivosInsuficientes => "pendencias.gerais.pendencias.calendario.excluir";
        public static string RotaExecutaExclusaoPendenciaParametroEvento => "pendencias.gerais.pendencias.evento.excluir";
        public static string RotaTrataNotificacoesNiveis => "notificacao.tratamento.niveiscargos";

        public static string RotaExecutaVerificacaoPendenciasProfessor => "pendencias.professor.executa.verificacao";

        public static string RotaExecutaVerificacaoPendenciasAusenciaFechamento => "pendencias.bimestre.ausencia.fechamento.verificacao";
        public static string RotaExecutaExclusaoPendenciasAusenciaAvaliacao => "pendencias.professor.avaliacao.excluir";

        public static string RotaExecutaExclusaoPendenciasAusenciaFechamento => "pendencias.bimestre.ausencia.fechamento.excluir";

        public static string RotaExecutaAtualizacaoSituacaoConselhoClasse => "conselho.classe.situacao.atualizar";
        public static string RotaNotificacaoAndamentoFechamento => "fechamento.andamento.notificar";

        public static string RotaNotificacaoInicioFimPeriodoFechamento => "fechamento.iniciofim.periodo.notificar";
               
        public static string RotaNotificacaoResultadoInsatisfatorio => "notificacao.nova.resultado.insatisfatorio";
        public static string RotaNotificacaoUeFechamentosInsuficientes => "fechamento.insuficiente.notificar";

        public static string RotaNotificacaoReuniaoPedagogica => "evento.reuniao.pedagogica.notificar";

        public static string RotaNotificacaoPeriodoFechamento => "periodo.fechamento.notificar";
        public static string RotaNotificacaoFrequenciaUe => "frequencia.ue.notificar";

        public static string RotaPendenciaAusenciaRegistroIndividual => "pendencias.professor.ausencia.registro.individual";
        public static string RotaAtualizarPendenciaAusenciaRegistroIndividual => "pendencias.professor.ausencia.registro.individual.atualizar";

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

        public static string SincronizaDevolutivasPorTurmaInfantilSync => "sgp.sincronizacao.devolutivas.turma.sync";
        public static string ConsolidarDevolutivasPorTurmaInfantil => "sgp.devolutivas.turma.infantil.consolidar";
        public static string ConsolidarDevolutivasPorTurma => "sgp.devolutivas.turma.consolidar";
    }
}
