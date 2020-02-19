using System.Collections.Generic;
using System.Linq;

namespace SME.SGP.Dominio
{
    public class PeriodoFechamento : EntidadeBase
    {
        public PeriodoFechamento(Dre dre, Ue ue)
        {
            AdicionarDre(dre);
            AdicionarUe(ue);

            fechamentosBimestre = new List<PeriodoFechamentoBimestre>();
        }

        protected PeriodoFechamento()
        {
            fechamentosBimestre = new List<PeriodoFechamentoBimestre>();
        }

        public Dre Dre { get; set; }
        public long? DreId { get; set; }
        public IEnumerable<PeriodoFechamentoBimestre> FechamentosBimestre => fechamentosBimestre;
        public bool Migrado { get; set; }
        public bool Excluido { get; set; }
        public Ue Ue { get; set; }
        public long? UeId { get; set; }
        private List<PeriodoFechamentoBimestre> fechamentosBimestre { get; set; }

        public void AdicionarDre(Dre dre)
        {
            if (dre != null)
            {
                DreId = dre.Id;
                Dre = dre;
            }
        }

        public void AdicionarFechamentoBimestre(PeriodoFechamentoBimestre fechamentoBimestre)
        {
            //if (periodoEscolar == null)
            //{
            //    throw new NegocioException("O período escolar não foi encontrado.");
            //}
            //if (periodoEscolar.TipoCalendarioId == 0 || periodoEscolar.TipoCalendario == null)
            //{
            //    throw new NegocioException("O tipo de calendário não foi encontrado.");
            //}

            //if (periodoEscolar.TipoCalendario.AnoLetivo
            //    != fechamentoBimestre.InicioDoFechamento.Year || periodoEscolar.TipoCalendario.AnoLetivo != fechamentoBimestre.FinalDoFechamento.Year)
            //{
            //    throw new NegocioException("As datas do período de fechamento devem ser no mesmo ano do tipo de calendário informado.");
            //}

            if (fechamentoBimestre.InicioDoFechamento > fechamentoBimestre.FinalDoFechamento)
            {
                throw new NegocioException("A data de início do fechamento deve ser menor que a data final.");
            }

            if (fechamentosBimestre.Any(c => c.PeriodoEscolar.Bimestre == fechamentoBimestre.PeriodoEscolar.Bimestre))
            {
                throw new NegocioException("Esse período escolar já foi adicionado.");
            }

            var periodoComDataInvalida = fechamentosBimestre.FirstOrDefault(c => c.PeriodoEscolar.Bimestre < fechamentoBimestre.PeriodoEscolar.Bimestre && c.FinalDoFechamento > fechamentoBimestre.InicioDoFechamento);
            if (periodoComDataInvalida != null)
            {
                throw new NegocioException($"A data de início do fechamento do {fechamentoBimestre.PeriodoEscolar.Bimestre}º Bimestre deve ser menor que a data final do {periodoComDataInvalida.PeriodoEscolar.Bimestre}º Bimestre.");
            }
            //fechamentoBimestre.AdicionarPeriodoEscolar(periodoEscolar);
            fechamentosBimestre.Add(fechamentoBimestre);
        }

        public void AdicionarUe(Ue ue)
        {
            if (ue != null)
            {
                UeId = ue.Id;
                Ue = ue;
            }
        }

        public PeriodoFechamentoBimestre ObterFechamentoBimestre(long periodoEscolarId)
        {
            return fechamentosBimestre.FirstOrDefault(c => c.PeriodoEscolarId == periodoEscolarId);
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
                var periodoUE = FechamentosBimestre.FirstOrDefault(c => c.PeriodoEscolar.Bimestre == periodoSME.PeriodoEscolar.Bimestre);
                if (periodoUE == null)
                {
                    throw new NegocioException($"O período de fechamento da {tipoFechamentoASerValidado} no {periodoSME.PeriodoEscolar.Bimestre}º Bimestre não foi encontrado.");
                }
                if (periodoUE.InicioDoFechamento.Date < periodoSME.InicioDoFechamento.Date)
                {
                    throw new NegocioException($"A data de início de fechamento da {tipoFechamentoASerValidado} no {periodoSME.PeriodoEscolar.Bimestre}º Bimestre deve ser maior que {periodoSME.InicioDoFechamento.ToString("dd/MM/yyyy")}.");
                }
                if (periodoUE.FinalDoFechamento.Date > periodoSME.FinalDoFechamento.Date)
                {
                    throw new NegocioException($"A data final do fechamento da {tipoFechamentoASerValidado} no {periodoSME.PeriodoEscolar.Bimestre}º Bimestre deve ser menor que {periodoSME.FinalDoFechamento.ToString("dd/MM/yyyy")}.");
                }
            }
        }
    }
}