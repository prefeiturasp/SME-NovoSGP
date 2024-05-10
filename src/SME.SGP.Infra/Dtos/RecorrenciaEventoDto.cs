using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Infra
{
    public class RecorrenciaEventoDto
    {
        public DateTime? DataFim { get; set; }

        [DataRequerida(ErrorMessage = "A data inicial da recorrência é obrigatória.")]
        public DateTime DataInicio { get; set; }

        public int? DiaDeOcorrencia { get; set; }
        public IEnumerable<DayOfWeek> DiasDaSemana { get; set; }

        [EnumeradoRequirido(ErrorMessage = "O padrão de recorrência é obrigatório.")]
        public PadraoRecorrencia Padrao { get; set; }

        public PadraoRecorrenciaMensal? PadraoRecorrenciaMensal { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "A frequência de repetição da recorrência é obrigatória.")]
        public int RepeteACada { get; set; }
    }
}