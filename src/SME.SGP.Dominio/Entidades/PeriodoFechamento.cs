using System;
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

            FechamentosBimestre = new List<PeriodoFechamentoBimestre>();
        }

        public PeriodoFechamento()
        {
            FechamentosBimestre = new List<PeriodoFechamentoBimestre>();
        }

        public Dre Dre { get; set; }
        public long? DreId { get; set; }
        public bool Migrado { get; set; }
        public Ue Ue { get; set; }
        public long? UeId { get; set; }
        public List<PeriodoFechamentoBimestre> FechamentosBimestre { get; set; }
        public Aplicacao Aplicacao { get; set; }
        public void AdicionarDre(Dre dre)
        {
            if (dre.NaoEhNulo())
            {
                DreId = dre.Id;
                Dre = dre;
            }
        }

        public void AdicionarFechamentoBimestre(PeriodoFechamentoBimestre fechamentoBimestre)
        {
            if (FechamentosBimestre.Any(c => c.PeriodoEscolar.Bimestre == fechamentoBimestre.PeriodoEscolar.Bimestre))
            {
                throw new NegocioException("Esse período escolar já foi adicionado.");
            }

            ValidarPeriodoInicioFim(fechamentoBimestre);
            ValidarPeriodoConcomitante(fechamentoBimestre);
 
            FechamentosBimestre.Add(fechamentoBimestre);
        }

        public void ValidarPeriodoInicioFim(PeriodoFechamentoBimestre fechamentoBimestre)
        {
            if (fechamentoBimestre.InicioDoFechamento > fechamentoBimestre.FinalDoFechamento)
            {
                throw new NegocioException($"A data de início do fechamento do {fechamentoBimestre.PeriodoEscolar.Bimestre}º Bimestre deve ser menor que a data final.");
            }
        }

        public void ValidarPeriodoConcomitante(PeriodoFechamentoBimestre fechamentoBimestre)
        {
            var periodoComDataInvalida = FechamentosBimestre.FirstOrDefault(c => c.PeriodoEscolar.Bimestre < fechamentoBimestre.PeriodoEscolar.Bimestre && c.FinalDoFechamento > fechamentoBimestre.InicioDoFechamento);
            if (periodoComDataInvalida.NaoEhNulo())
            {
                throw new NegocioException($"A data de início do fechamento do {fechamentoBimestre.PeriodoEscolar.Bimestre}º Bimestre deve ser maior que a data final do {periodoComDataInvalida.PeriodoEscolar.Bimestre}º Bimestre.");
            }
        }

        public void AdicionarUe(Ue ue)
        {
            if (ue.NaoEhNulo())
            {
                UeId = ue.Id;
                Ue = ue;
            }
        }

        public PeriodoFechamentoBimestre ObterFechamentoBimestre(long periodoEscolarId)
        {
            return FechamentosBimestre.FirstOrDefault(c => c.PeriodoEscolarId == periodoEscolarId);
        }

        public bool ExisteFechamentoEmAberto(DateTime hoje)
        {
            return FechamentosBimestre.Any(a => a.DataDentroPeriodo(hoje));            
        }
    }
}