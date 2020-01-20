using System.Collections.Generic;
using System.Linq;

namespace SME.SGP.Dominio
{
    public class Fechamento : EntidadeBase
    {
        public Fechamento(long? dreId, long? ueId)
        {
            DreId = dreId;
            UeId = ueId;
            fechamentosBimestre = new List<FechamentoBimestre>();
        }

        protected Fechamento()
        {
            fechamentosBimestre = new List<FechamentoBimestre>();
        }

        public long? DreId { get; set; }
        public IEnumerable<FechamentoBimestre> FechamentosBimestre => fechamentosBimestre;
        public bool Migrado { get; set; }
        public long? UeId { get; set; }
        private List<FechamentoBimestre> fechamentosBimestre { get; set; }

        public void AdicionarFechamentoBimestre(PeriodoEscolar periodoEscolar, FechamentoBimestre fechamentoBimestre)
        {
            if (periodoEscolar == null)
            {
                throw new NegocioException("O período escolar não foi encontrado.");
            }
            if (periodoEscolar.TipoCalendarioId == 0 || periodoEscolar.TipoCalendario == null)
            {
                throw new NegocioException("O tipo de calendário não foi encontrado.");
            }

            if (periodoEscolar.TipoCalendario.AnoLetivo
                != fechamentoBimestre.InicioDoFechamento.Year || periodoEscolar.TipoCalendario.AnoLetivo != fechamentoBimestre.FinalDoFechamento.Year)
            {
                throw new NegocioException("As datas do período de fechamento devem ser no mesmo ano do tipo de calendário informado.");
            }

            //if (fechamentoBimestre.InicioDoFechamento > fechamentoBimestre.FinalDoFechamento)
            //{
            //    throw new NegocioException("A data de início do fechamento deve ser menor que a data final.");
            //}

            if (fechamentosBimestre.Any(c => c.PeriodoEscolar.Bimestre == periodoEscolar.Bimestre))
            {
                throw new NegocioException("Esse período escolar já foi adicionado.");
            }

            var periodoComDataInvalida = fechamentosBimestre.FirstOrDefault(c => c.PeriodoEscolar.Bimestre < periodoEscolar.Bimestre && c.FinalDoFechamento > fechamentoBimestre.InicioDoFechamento);
            if (periodoComDataInvalida != null)
            {
                throw new NegocioException($"A data de início do fechamento do {periodoEscolar.Bimestre}º Bimestre deve ser menor que a data final do {periodoComDataInvalida.PeriodoEscolar.Bimestre}º Bimestre.");
            }
            fechamentoBimestre.AdicionarPeriodoEscolar(periodoEscolar);
            fechamentosBimestre.Add(fechamentoBimestre);
        }

        public void ValidarIntervaloDatasDreEUe(List<FechamentoBimestre> periodoFechamentoSMEDRE)
        {
            var tipoFechamentoASerValidado = UeId.HasValue ? "UE" : "Dre";
            if (periodoFechamentoSMEDRE == null)
            {
                throw new NegocioException("O período de fechamento da SME não foi encontrado.");
            }
            foreach (var periodoSME in periodoFechamentoSMEDRE)
            {
                var periodoUE = FechamentosBimestre.FirstOrDefault(c => c.PeriodoEscolar.Bimestre == periodoSME.PeriodoEscolar.Bimestre);
                if (periodoUE == null)
                {
                    throw new NegocioException($"O período de fechamento da {tipoFechamentoASerValidado} no {periodoSME.PeriodoEscolar.Bimestre}º Bimestre não foi encontrado.");
                }
                if (periodoUE.InicioDoFechamento < periodoSME.InicioDoFechamento)
                {
                    throw new NegocioException($"A data de início de fechamento da {tipoFechamentoASerValidado} no {periodoSME.PeriodoEscolar.Bimestre}º deve ser maior que {periodoSME.InicioDoFechamento.ToString("DD/MM/YYYY")}.");
                }
                if (periodoUE.FinalDoFechamento > periodoSME.FinalDoFechamento)
                {
                    throw new NegocioException($"A data final do fechamento da {tipoFechamentoASerValidado} no {periodoSME.PeriodoEscolar.Bimestre}º deve ser menor que {periodoSME.FinalDoFechamento.ToString("DD/MM/YYYY")}.");
                }
            }
        }
    }
}