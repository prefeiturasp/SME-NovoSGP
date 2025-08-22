using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Infra.Enumerados
{
    public enum SituacaoArquivoImportacao
    {
        [Display(Name = "Carregamento inicial")]
        CarregamentoInicial = 1,

        [Display(Name = "Processando")]
        Processando = 2,

        [Display(Name = "Processado com falhas")]
        Processado = 3,

        [Display(Name = "Processado com sucesso")]
        Cancelado = 4,
    }
}
