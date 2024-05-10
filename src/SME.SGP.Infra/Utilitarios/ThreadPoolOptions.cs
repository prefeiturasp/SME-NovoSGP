namespace SME.SGP.Infra.Utilitarios
{
    public class ThreadPoolOptions
    {
        public const string Secao = "ThreadPool";

        public int WorkerThreads { get; set; }
        public int CompletionPortThreads { get; set; }
    }
}
