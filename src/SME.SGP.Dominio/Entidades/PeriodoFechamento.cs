using System;
using System.Collections.Generic;
using System.Linq;

namespace SME.SGP.Dominio
{
    public class PeriodoFechamento : EntidadeBase
    {
        public PeriodoFechamento(TipoCalendario tipoCalendario, long? dreId, long? ueId)
        {
            if (tipoCalendario == null)
            {
                throw new NegocioException("O tipo de calendário não foi encontrado.");
            }
            TipoCalendario = tipoCalendario;
            DreId = dreId;
            UeId = ueId;
            periodoFechamentoBimestre = new List<PeriodoFechamentoBimestre>();
        }

        protected PeriodoFechamento()
        {
        }

        public long? DreId { get; set; }
        public IEnumerable<PeriodoFechamentoBimestre> PeriodoFechamentoBimestre => periodoFechamentoBimestre;
        public TipoCalendario TipoCalendario { get; set; }
        public long TipoCalendarioId { get; set; }
        public long? UeId { get; set; }
        private List<PeriodoFechamentoBimestre> periodoFechamentoBimestre { get; set; }

        public void AdicionarPeriodoFechamentoBimestre(PeriodoEscolar periodoEscolar, DateTime inicioFechamento, DateTime finalFechamento)
        {
            if (TipoCalendario == null)
            {
                throw new NegocioException("O tipo de calendário não foi encontrado.");
            }

            if (periodoEscolar.TipoCalendarioId != TipoCalendarioId)
            {
                throw new NegocioException("O período escolar deve ser do mesmo tipo de calendário.");
            }

            if (TipoCalendario.AnoLetivo != inicioFechamento.Year || TipoCalendario.AnoLetivo != finalFechamento.Year)
            {
                throw new NegocioException("As datas do período de fechamento devem ser no mesmo ano do tipo de calendário informado.");
            }

            if (inicioFechamento > finalFechamento)
            {
                throw new NegocioException("A data de início do fechamento deve ser menor que a data final.");
            }

            if (periodoFechamentoBimestre.Any(c => c.PeriodoEscolar.Bimestre == periodoEscolar.Bimestre))
            {
                throw new NegocioException("Esse período escolar já foi adicionado.");
            }

            var periodoComDataInvalida = periodoFechamentoBimestre.FirstOrDefault(c => c.PeriodoEscolar.Bimestre < periodoEscolar.Bimestre && c.FinalDoFechamento > inicioFechamento);
            if (periodoComDataInvalida != null)
            {
                throw new NegocioException($"A data de início do fechamento do {periodoEscolar.Bimestre}º Bimestre deve ser menor que a data final do {periodoComDataInvalida.PeriodoEscolar.Bimestre}º Bimestre.");
            }

            periodoFechamentoBimestre.Add(new PeriodoFechamentoBimestre(periodoEscolar, inicioFechamento, finalFechamento));
        }

        public void ValidarIntervaloDatasDreEUe(List<PeriodoFechamentoBimestre> periodoFechamentoSMEDRE)
        {
            var tipoFechamentoASerValidado = UeId.HasValue ? "UE" : "Dre";
            if (periodoFechamentoSMEDRE == null)
            {
                throw new NegocioException("O período de fechamento da SME não foi encontrado.");
            }
            foreach (var periodoSME in periodoFechamentoSMEDRE)
            {
                var periodoUE = PeriodoFechamentoBimestre.FirstOrDefault(c => c.PeriodoEscolar.Bimestre == periodoSME.PeriodoEscolar.Bimestre);
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

        public void ValidarQuantidadePeriodos()
        {
            if (TipoCalendario.Modalidade == ModalidadeTipoCalendario.FundamentalMedio && periodoFechamentoBimestre.Count != 4)
            {
                throw new NegocioException("Calendários de Fundamental e Médio devem possuir 4 Bimestres.");
            }
            else if (TipoCalendario.Modalidade == ModalidadeTipoCalendario.EJA && periodoFechamentoBimestre.Count != 2)
            {
                throw new NegocioException("Calendários de EJA devem possuir 2 Bimestres.");
            }
        }
    }
}