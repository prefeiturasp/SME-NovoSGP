using System;

namespace SME.SGP.Infra
{
    public class FiltroComponentesSemAtribuicaoCodigoDataMetricasDto
    {
        public FiltroComponentesSemAtribuicaoCodigoDataMetricasDto(string[] codigosComponentesSemAtribuicao, DateTime data, string codigoTurma)
        {
            CodigosComponentesSemAtribuicao = codigosComponentesSemAtribuicao;
            Data = data;
            CodigoTurma = codigoTurma;
        }
        public string[] CodigosComponentesSemAtribuicao { get; set; }
        public DateTime Data { get; set; }
        public string CodigoTurma { get; set; }
    }
}
