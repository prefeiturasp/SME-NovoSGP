using System;

namespace SME.SGP.Infra
{
    public class FiltroSemanaDto
    {
        public DateTime Inicio { get; set; }
        public DateTime Fim { get; set; }

        public string Descricao
        {
            get {
                return $"{Inicio:dd/MM/yyyy} até {Fim:dd/MM/yyyy}";
            }
        }

        public string Valor
        {
            get
            {
                return $"{Inicio:ddMMyyyy}_{Fim:ddMMyyyy}";
            }
        }
    }
}
