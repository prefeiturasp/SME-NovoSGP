using SME.SGP.Dominio;
using System;
using System.Collections.Generic;

namespace SME.SGP.Infra.Dtos
{
    public class RecorrenciaEventoDto
    {
        public DateTime DataFinal { get; set; }
        public DateTime DataInicial { get; set; }
        public DateTime? DiaDeOcorrencia { get; set; }
        public IEnumerable<DayOfWeek> DiasDaSemana { get; set; }
        public PadraoRecorrencia Padrao { get; set; }
        public PadraoRecorrenciaMensal? PadraoRecorrenciaMensal { get; set; }
        public int RepeteACada { get; set; }
    }
}