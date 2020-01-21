using System;
using System.Collections.Generic;
using System.Linq;

namespace SME.SGP.Dominio
{
    public class FechamentoReabertura : EntidadeBase
    {
        public FechamentoReabertura()
        {
            bimestres = new List<FechamentoReaberturaBimestre>();
        }

        public IEnumerable<FechamentoReaberturaBimestre> Bimestres { get { return bimestres; } }

        public string Descricao { get; set; }

        public Dre Dre { get; set; }

        public long? DreId { get; set; }

        public DateTime Fim { get; set; }

        public DateTime Inicio { get; set; }

        public bool Migrado { get; set; }

        public TipoCalendario TipoCalendario { get; set; }

        public long TipoCalendarioId { get; set; }

        public Ue Ue { get; set; }

        public long? UeId { get; set; }

        private List<FechamentoReaberturaBimestre> bimestres { get; set; }

        public void Adicionar(FechamentoReaberturaBimestre bimestre)
        {
            if (bimestre != null)
            {
                bimestre.FechamentoAbertura = this;
                bimestre.FechamentoAberturaId = this.Id;
                bimestres.Add(bimestre);
            }
        }

        public void AtualizarDre(Dre dre)
        {
            if (dre != null)
            {
                this.Dre = dre;
                this.DreId = dre.Id;
            }
        }

        public void AtualizarTipoCalendario(TipoCalendario tipoCalendario)
        {
            if (tipoCalendario != null)
            {
                this.TipoCalendario = tipoCalendario;
                this.TipoCalendarioId = tipoCalendario.Id;
            }
        }

        public void AtualizarUe(Ue ue)
        {
            if (ue != null)
            {
                this.Ue = ue;
                this.UeId = ue.Id;
            }
        }

        public bool EhParaDre()
        {
            return UeId is null;
        }

        public bool EhParaSme()
        {
            return (DreId is null) && (UeId is null);
        }

        public bool EhParaUe()
        {
            return !(DreId is null) && !(UeId is null);
        }

        public bool EstaNoRangeDeDatas(DateTime dataInicio, DateTime datafim)
        {
            return (Inicio.Date <= dataInicio.Date && Fim >= datafim.Date)
            || (Inicio.Date <= datafim.Date && Fim >= datafim.Date)
            || (Inicio.Date >= dataInicio.Date && Fim <= datafim.Date);
        }

        public void PodeSalvar(IEnumerable<FechamentoReabertura> fechamentosCadastrados)
        {
            if (Inicio > Fim)
                throw new NegocioException("A data início não pode ser maior que a data fim.");

            if (TipoCalendario.AnoLetivo != Inicio.Year || TipoCalendario.AnoLetivo != Fim.Year)
                throw new NegocioException("O ano não pode ser diferente do ano do Tipo de Calendário.");

            VerificaFechamentosHierarquicos(fechamentosCadastrados);
            VerificaFechamentosNoMesmoPeriodo(fechamentosCadastrados);
        }

        private bool PodePersistirNesteNasDatas(IEnumerable<(DateTime, DateTime)> datasDosFechamentosSME)
        {
            return datasDosFechamentosSME.Any(a => (Inicio.Date >= a.Item1.Date && Inicio.Date <= a.Item2.Date) &&
                    (Fim.Date > a.Item1.Date && Fim.Date <= a.Item2.Date));
        }

        private void VerificaFechamentosHierarquicos(IEnumerable<FechamentoReabertura> fechamentosCadastrados)
        {
            if (EhParaDre())
            {
                var fechamentosSME = fechamentosCadastrados.Where(a => a.DreId is null && a.UeId is null).ToList();
                if (fechamentosSME is null && !fechamentosSME.Any())
                    throw new NegocioException("Não há Reabertura de Fechamento cadastrado pela SME.");

                if (!PodePersistirNesteNasDatas(fechamentosSME.Select(a => { return (a.Inicio.Date, a.Fim.Date); })))
                    throw new NegocioException("Não há Reabertura de Fechamento cadastrado pela SME para este período.");
            }
            else if (EhParaUe())
            {
                var fechamentos = fechamentosCadastrados.Where(a => a.UeId is null && a.DreId == DreId).ToList();

                if (fechamentos is null && !fechamentos.Any())
                {
                    fechamentos = fechamentosCadastrados.Where(a => a.DreId is null && a.UeId is null).ToList();
                    if (fechamentos is null && !fechamentos.Any())
                        throw new NegocioException("Não há Reabertura de Fechamento cadastrado pela SME ou pela Dre.");
                }

                if (!PodePersistirNesteNasDatas(fechamentos.Select(a => { return (a.Inicio.Date, a.Fim.Date); })))
                    throw new NegocioException("Não há Reabertura de Fechamento cadastrado pela SME ou pela Dre.");
            }
        }

        private void VerificaFechamentosNoMesmoPeriodo(IEnumerable<FechamentoReabertura> fechamentosCadastrados)
        {
            if (EhParaSme())
            {
                var fechamentosSME = fechamentosCadastrados.Where(a => a.EhParaSme());
                if (!(fechamentosSME is null) && fechamentosCadastrados.Any())
                {
                    foreach (var fechamento in fechamentosSME)
                    {
                        if (EstaNoRangeDeDatas(fechamento.Inicio, fechamento.Fim))
                            throw new NegocioException($"Não é possível persistir pois já existe uma reabertura cadastrada que começa em {fechamento.Inicio.ToString("dd/MM/yyyy")} e finaliza em {fechamento.Fim.ToString("dd/MM/yyyy")}");
                    }
                }
            }
        }
    }
}