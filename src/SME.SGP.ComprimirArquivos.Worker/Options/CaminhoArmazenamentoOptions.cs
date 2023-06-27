namespace SME.SGP.ComprimirArquivos.Worker
{
    public class CaminhoArmazenamentoOptions
    {
        public const string Secao = "CaminhoArmazenamento";
        public string CaminhoFisico { get; set; }
        public string CaminhoTemporario { get; set; }
    }
}