using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dominio.Entidades
{
    public class PeriodoEscolar : EntidadeBase
    {
        public int TipoCalendario { get; set; }
        public int Bimestre { get; set; }
        public DateTime PeriodoInicio { get; set; }
        public DateTime PeriodoFim { get; set; }

        public void Validar()
        {
            if (PeriodoFim < PeriodoInicio)
                throw new NegocioException($"{Bimestre}º Bimestre: A data de inicio não pode ser posterior a data de fim do periodo");
        }
    }
}
