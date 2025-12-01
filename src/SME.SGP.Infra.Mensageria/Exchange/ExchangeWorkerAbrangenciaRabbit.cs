namespace SME.SGP.Infra.Mensageria.Exchange
{
    public static class ExchangeWorkerAbrangenciaRabbit
    {
        public static string WorkerAbrangencia => "sgp.abrangencia.workers";
        public static string WorkerAbrangenciaDeadLetter => "sgp.abrangencia.workers.deadletter";
    }
}
