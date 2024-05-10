using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class DadosUeCriacaoAulaAutomaticaDto
    {
        public DadosUeCriacaoAulaAutomaticaDto(string codigoUe, DadosCriacaoAulasAutomaticasDto dadosCriacaoAulasAutomaticas)
        {
            CodigoUe = codigoUe;
            DadosCriacaoAulasAutomaticas = dadosCriacaoAulasAutomaticas;
        }

        public string CodigoUe { get; set; }
        public DadosCriacaoAulasAutomaticasDto DadosCriacaoAulasAutomaticas { get; set; }
    }
}
