using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dominio.Entidades
{
    public class PeriodoEscolar : EntidadeBase
    {
        public long TipoCalendario { get; set; }
        public int Bimestre { get; set; }
        public DateTime PeriodoInicio { get; set; }
        public DateTime PeriodoFim { get; set; }

        public void ValidarIncioBimestre()
        {
            if (PeriodoFim < PeriodoInicio)
                throw new NegocioException($"{Bimestre}º Bimestre: A data de inicio não pode ser posterior a data de fim do periodo");
        }

        public void ValidarAnoBase(int anoBase)
        {
            if (PeriodoInicio.Year < anoBase)
                throw new NegocioException("O incio do periodo não pode ser anterior ao ano base");

            if (PeriodoFim.Year > anoBase)
                throw new NegocioException("O fim do periodo não pode ser posterior ao ano base");
        }
    }
}
