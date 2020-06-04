using System;
using System.Collections.Generic;
using System.Linq;

namespace SME.SGP.Dominio
{
    public class FechamentoReabertura : EntidadeBase
    {
        public FechamentoReabertura()
        {
            Status = EntidadeStatus.Aprovado;
            bimestres = new List<FechamentoReaberturaBimestre>();
            Excluido = false;
        }

        public IEnumerable<FechamentoReaberturaBimestre> Bimestres { get { return bimestres; } }

        public string Descricao { get; set; }
        public Dre Dre { get; set; }
        public long? DreId { get; set; }
        public bool Excluido { get; set; }
        public DateTime Fim { get; set; }
        public DateTime Inicio { get; set; }
        public bool Migrado { get; set; }
        public EntidadeStatus Status { get; set; }
        public TipoCalendario TipoCalendario { get; set; }
        public long TipoCalendarioId { get; set; }
        public Ue Ue { get; set; }
        public long? UeId { get; set; }
        public WorkflowAprovacao WorkflowAprovacao { get; set; }
        public long? WorkflowAprovacaoId { get; set; }
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

        public void AdicionarBimestres(IEnumerable<FechamentoReaberturaBimestre> listaBimestres)
        {
            bimestres.AddRange(listaBimestres);
        }

        public void AprovarWorkFlow()
        {
            if (Status == EntidadeStatus.AguardandoAprovacao)
                Status = EntidadeStatus.Aprovado;
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

        public bool DeveCriarEventos()
        {
            return EhParaUe() && Status == EntidadeStatus.Aprovado;
        }

        public bool EhParaDre()
        {
            return UeId is null && !(DreId is null) && DreId > 0;
        }

        public bool EhParaSme()
        {
            return (DreId is null) && (UeId is null);
        }

        public bool EhParaUe()
        {
            return !(DreId is null) && DreId > 0 && !(UeId is null) && UeId > 0;
        }

        public bool EstaNoRangeDeDatas(DateTime dataInicio, DateTime datafim)
        {
            return (Inicio.Date <= dataInicio.Date && Fim >= datafim.Date)
            || (Inicio.Date <= datafim.Date && Fim >= datafim.Date)
            || (Inicio.Date >= dataInicio.Date && Fim <= datafim.Date);
        }

        public void Excluir()
        {
            if (Excluido)
                throw new NegocioException($"Não é possível excluir o fechamento {Id} pois o mesmo já se encontra excluído.");

            Excluido = true;
        }

        public object ObterBimestresNumeral()
        {
            return string.Join(",", bimestres.Select(a => $"{a.Bimestre.ToString()}º").ToArray());
        }

        public bool[] ObterBimestresSelecionados()
        {
            bool[] bimestresArray = new bool[TipoCalendario.QuantidadeDeBimestres()];

            foreach (var bimestre in bimestres)
            {
                bimestresArray[bimestre.Bimestre - 1] = true;
            }
            return bimestresArray;
        }

        public void PodeSalvar(IEnumerable<FechamentoReabertura> fechamentosCadastrados, Usuario usuario)
        {
            if (Inicio > Fim)
                throw new NegocioException("A data início não pode ser maior que a data fim.");

            if (usuario.EhPerfilUE())
            {
                if (!EhParaUe())
                    throw new NegocioException("Perfil Ue deverá somente cadastrar reabertura de fechamento para Ues.");
            }
            else if (usuario.EhPerfilDRE())
            {
                if (EhParaSme())
                    throw new NegocioException("Perfil Dre deverá somente cadastrar reabertura de fechamento para Dres e Ues.");
            }

            VerificaFechamentosHierarquicos(fechamentosCadastrados);
            VerificaFechamentosNoMesmoPeriodo(fechamentosCadastrados);
        }

        public void ReprovarWorkFlow()
        {
            if (Status == EntidadeStatus.AguardandoAprovacao)
                Status = EntidadeStatus.Recusado;
        }

        public void VerificaStatus()
        {
            if (EhParaUe() && TipoCalendario.AnoLetivo < DateTime.Today.Year)
            {
                Status = EntidadeStatus.AguardandoAprovacao;
            }
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
                var fechamentosSME = fechamentosCadastrados.Where(a => a.EhParaSme()).ToList();
                if (fechamentosSME is null || !fechamentosSME.Any())
                    throw new NegocioException("Não há Reabertura de Fechamento cadastrado pela SME.");

                if (!PodePersistirNesteNasDatas(fechamentosSME.Select(a => { return (a.Inicio.Date, a.Fim.Date); })))
                    throw new NegocioException("Não há Reabertura de Fechamento cadastrado pela SME para este período.");
            }
            else if (EhParaUe())
            {
                var fechamentos = fechamentosCadastrados.Where(a => a.EhParaDre() && a.DreId == DreId).ToList();

                if (fechamentos is null || !fechamentos.Any())
                {
                    fechamentos = fechamentosCadastrados.Where(a => a.EhParaSme()).ToList();
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
                if (!(fechamentosSME is null) && fechamentosSME.Any())
                {
                    foreach (var fechamento in fechamentosSME)
                    {
                        if (EstaNoRangeDeDatas(fechamento.Inicio, fechamento.Fim))
                            throw new NegocioException($"Não é possível persistir pois já existe uma reabertura cadastrada que começa em {fechamento.Inicio.ToString("dd/MM/yyyy")} e termina em {fechamento.Fim.ToString("dd/MM/yyyy")}");
                    }
                }
            }
            else if (EhParaDre())
            {
                var fechamentosDre = fechamentosCadastrados.Where(a => a.EhParaDre() && a.DreId == DreId);
                if (!(fechamentosDre is null) && fechamentosDre.Any())
                {
                    foreach (var fechamento in fechamentosDre)
                    {
                        if (EstaNoRangeDeDatas(fechamento.Inicio, fechamento.Fim))
                            throw new NegocioException($"Não é possível persistir pois já existe uma reabertura cadastrada que começa em {fechamento.Inicio.ToString("dd/MM/yyyy")} e termina em {fechamento.Fim.ToString("dd/MM/yyyy")}");
                    }
                }
            }
            else if (EhParaUe())
            {
                var fechamentosUe = fechamentosCadastrados.Where(a => a.EhParaUe() && a.UeId == UeId);
                if (!(fechamentosUe is null) && fechamentosUe.Any())
                {
                    foreach (var fechamento in fechamentosUe)
                    {
                        if (EstaNoRangeDeDatas(fechamento.Inicio, fechamento.Fim))
                            throw new NegocioException($"Não é possível persistir pois já existe uma reabertura cadastrada que começa em {fechamento.Inicio.ToString("dd/MM/yyyy")} e termina em {fechamento.Fim.ToString("dd/MM/yyyy")}");
                    }
                }
            }
        }

        public bool DataDentroPeriodo(DateTime data)
        {
            return Inicio.Date <= data.Date && Fim.Date >= data.Date;
        }
    }
}