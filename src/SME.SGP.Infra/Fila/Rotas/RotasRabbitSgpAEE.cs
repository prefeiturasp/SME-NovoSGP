﻿namespace SME.SGP.Infra
{
    public class RotasRabbitSgpAEE
    {
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
    }
}
