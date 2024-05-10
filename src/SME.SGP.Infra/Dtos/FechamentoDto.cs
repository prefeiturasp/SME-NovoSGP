﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Infra
{
    public class FechamentoDto : AuditoriaDto
    {
        public FechamentoDto()
        {
            FechamentosBimestres = new List<FechamentoBimestreDto>();
        }

        public bool ConfirmouAlteracaoHierarquica { get; set; }
        public string DreId { get; set; }
        public IEnumerable<FechamentoBimestreDto> FechamentosBimestres { get; set; }
        public bool Migrado { get; set; }

        [Required(ErrorMessage = "O tipo de calendário é obrigatório")]
        public long? TipoCalendarioId { get; set; }

        public string UeId { get; set; }
        public bool EhRegistroExistente { get; set; }
    }
}