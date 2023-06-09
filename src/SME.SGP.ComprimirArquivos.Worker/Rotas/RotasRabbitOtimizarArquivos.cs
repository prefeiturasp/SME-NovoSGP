namespace SME.SGP.ComprimirArquivos.Worker
{
    public static class RotasRabbitOtimizarArquivos
    {
        public static string ExchangeSgp => "sme.sgp.workers";
        public static string ExchangeSgpDeadLetter => "sme.sgp.workers.deadletter";

        public const string OtimizarArquivoImagem = "sgp.otimizar.arquivo.imagem";
        
        public const string OtimizarArquivoVideo = "sgp.otimizar.arquivo.video";
        
        public static int DeadLetterTTL => 10 * 60 * 1000; /*10 Min * 60 Seg * 1000 milisegundos = 10 minutos em milisegundos*/
        
        public static ulong QuantidadeReprocessamentoDeadLetter = 3;
    }
}