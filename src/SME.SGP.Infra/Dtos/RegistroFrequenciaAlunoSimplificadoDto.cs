using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Infra
{
    public class RegistroFrequenciaAlunoSimplificadoDto
    {
        public RegistroFrequenciaAlunoSimplificadoDto()
        {}
        public string CodigoAluno { get; set; }
        public int TotalAulas { get; set; }
        public int TotalAusencias { get; set; }
        public int TotalCompensacoes { get; set; }
        public int TotalPresencas { get; set; }
        public int TotalRemotos { get; set; }
        
        public int NumeroFaltasNaoCompensadas
        {
            get => TotalAusencias - TotalCompensacoes;
        }
        
        public double PercentualFrequencia
        {
            get
            {
                if (TotalAulas == 0)
                    return 0;

                var porcentagem = 100 - (((double)NumeroFaltasNaoCompensadas / TotalAulas) * 100);

                return Math.Round(porcentagem > 100 ? 100 : porcentagem);
            }
        }
    }
}
