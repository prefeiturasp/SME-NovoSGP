using System;

namespace SME.SGP.Infra
{
    public class FiltroFuncionarioDto
    {
        public string CodigoRF { get; set; }
        public string CodigoDRE { get; set; }
        public string CodigoUE { get; set; }
        public string NomeServidor { get; set; }

        public FiltroFuncionarioDto()
        {}

        public FiltroFuncionarioDto(string codigoDre, string codigoUe, string codigoRf, string nomeServidor)
        {
            CodigoDRE = codigoDre;
            CodigoUE = codigoUe;
            CodigoRF = codigoRf;
            NomeServidor = nomeServidor;
        }

        public void AtualizaCodigoDre(string codigoDre)
        {
            CodigoDRE = codigoDre;
        }

        public void AtualizaCodigoUe(string codigoUe)
        {
            CodigoUE = codigoUe;
        }
    }
    
}