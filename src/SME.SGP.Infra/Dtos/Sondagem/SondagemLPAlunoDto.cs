﻿using System.Diagnostics;

namespace SME.SGP.Infra.Dtos.Sondagem
{
    public class SondagemLPAlunoDto
    {
        public string Escrita1Bim { get; set; }
        public string Escrita2Bim { get; set; }
        public string Escrita3Bim { get; set; }
        public string Escrita4Bim { get; set; }
        public string Leitura1Bim { get; set; }
        public string Leitura2Bim { get; set; }
        public string Leitura3Bim { get; set; }
        public string Leitura4Bim { get; set; }

        public string ObterHipoteseEscrita(int bimestre)
        {
            switch (bimestre)
            {
                case 1:
                    return Escrita1Bim;
                case 2:
                    return Escrita2Bim;
                case 3:
                    return Escrita3Bim;
                case 4:
                    return Escrita4Bim;
                default:
                    return string.Empty;
            }
        }

        public string ObterTextoHipoteseEscrita(int bimestre)
        {
            var hipoteseEscrita = ObterHipoteseEscrita(bimestre);
            switch (hipoteseEscrita)
            {
                case "PS":
                    return "Pré-Silábico";
                case "SSV":
                    return "Silábico sem valor";
                case "SCV":
                    return "Silábico com valor";
                case "SA":
                    return "Silábico alfabético";
                case "A":
                    return "Alfabético";
                default:
                    return "";
            }
            
        }
    }
}
