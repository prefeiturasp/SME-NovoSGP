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
            FechamentosCicloSondagem = new List<PeriodoFechamentoCicloSondagem>();
        }

        public PeriodoFechamento()
        {
            FechamentosBimestre = new List<PeriodoFechamentoBimestre>();
            FechamentosCicloSondagem = new List<PeriodoFechamentoCicloSondagem>();
        }

        public Dre Dre { get; set; }
        public long? DreId { get; set; }
        public bool Migrado { get; set; }
        public Ue Ue { get; set; }
        public long? UeId { get; set; }
        public List<PeriodoFechamentoBimestre> FechamentosBimestre { get; set; }
        public List<PeriodoFechamentoCicloSondagem> FechamentosCicloSondagem { get; set; }
        public Aplicacao Aplicacao { get; set; }
        public void AdicionarDre(Dre dre)
        {
            if (dre.NaoEhNulo())
            {
                DreId = dre.Id;
                Dre = dre;
            }
        }

        public PeriodoFechamentoBimestre AdaptarCicloSondagemParaBimestre(PeriodoFechamentoCicloSondagem ciclo, long? tipoCalendarioId)
        {
            var ano = ciclo.InicioDoFechamento.Year;
            return new PeriodoFechamentoBimestre
            {
                Id = ciclo.Id,
                PeriodoFechamentoId = ciclo.PeriodoFechamentoId,
                InicioDoFechamento = ciclo.InicioDoFechamento,
                FinalDoFechamento = ciclo.FinalDoFechamento,
                PeriodoFechamento = ciclo.PeriodoFechamento,
                PeriodoEscolar = new PeriodoEscolar
                {
                    Bimestre = ciclo.Ciclo,
                    TipoCalendarioId = tipoCalendarioId ?? 0,
                    PeriodoInicio = new DateTime(ano, 1, 1),
                    PeriodoFim = new DateTime(ano, 12, 31),
                },
            };
        }

        public void AdicionarFechamentoBimestre(PeriodoFechamentoBimestre fechamentoBimestre)
        {
            if (FechamentosBimestre.Any(c => c.PeriodoEscolarId > 0 && c.PeriodoEscolar.Bimestre == fechamentoBimestre.PeriodoEscolar.Bimestre))
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

        public void AdicionarFechamentoCicloSondagem(PeriodoFechamentoCicloSondagem ciclo)
        {
            if (ciclo.Id == 0 && ciclo.PeriodoFechamentoId > 0 || FechamentosCicloSondagem.Any(c => c.Ciclo == ciclo.Ciclo))
            {
                throw new NegocioException("Esse ciclo dessa aplicação já foi adicionados.");
            }

            ValidarPeriodoCicloSondagemInicioFim(ciclo);
            ValidarPeriodoCicloSondagemConcomitante(ciclo);

            FechamentosCicloSondagem.Add(ciclo);
        }

        public void ValidarPeriodoCicloSondagemInicioFim(PeriodoFechamentoCicloSondagem  fechamentoCiclo)
        {
            if (fechamentoCiclo.InicioDoFechamento > fechamentoCiclo.FinalDoFechamento)
            {
                throw new NegocioException($"A data de início do fechamento do {fechamentoCiclo.Ciclo}º Ciclo deve ser menor que a data final.");
            }
        }

        public void ValidarPeriodoCicloSondagemConcomitante(PeriodoFechamentoCicloSondagem fechamentoCiclo)
        {
            var periodoComDataInvalida = FechamentosCicloSondagem.FirstOrDefault(c => c.Ciclo < fechamentoCiclo.Ciclo && c.FinalDoFechamento > fechamentoCiclo.InicioDoFechamento);
            if (periodoComDataInvalida.NaoEhNulo())
            {
                throw new NegocioException($"A data de início do fechamento do {fechamentoCiclo.Ciclo}º Ciclo deve ser maior que a data final do {periodoComDataInvalida.Ciclo}º Ciclo.");
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

        public PeriodoFechamentoCicloSondagem ObterFechamentoCicloSondagem(PeriodoFechamentoCicloSondagem ciclo)
        {
            return FechamentosCicloSondagem.FirstOrDefault(c => c.Ciclo == ciclo.Ciclo && c.PeriodoFechamentoId == ciclo.PeriodoFechamentoId);
        }

        public bool ExisteFechamentoEmAberto(DateTime hoje)
        {
            return FechamentosBimestre.Any(a => a.DataDentroPeriodo(hoje));            
        }
    }
}