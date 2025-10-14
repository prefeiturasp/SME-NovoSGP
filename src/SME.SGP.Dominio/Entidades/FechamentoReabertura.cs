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
            Bimestres = new List<FechamentoReaberturaBimestre>();
            Excluido = false;
        }

        public string Descricao { get; set; }
        public Dre Dre { get; set; }
        public long? DreId { get; set; }
        public bool Excluido { get; set; }
        public DateTime Fim { get; set; }
        public DateTime Inicio { get; set; }
        public bool Migrado { get; set; }
        public EntidadeStatus Status { get; set; }
        public TipoCalendario TipoCalendario { get; set; }
        public Aplicacao Aplicacao { get; set; }
        public long TipoCalendarioId { get; set; }
        public Ue Ue { get; set; }
        public long? UeId { get; set; }
        public WorkflowAprovacao WorkflowAprovacao { get; set; }
        public long? WorkflowAprovacaoId { get; set; }
        public List<FechamentoReaberturaBimestre> Bimestres { get; set; }
        public Usuario Aprovador { get; set; }
        public long? AprovadorId { get; set; }
        public DateTime? AprovadoEm { get; set; }

        public void Adicionar(FechamentoReaberturaBimestre bimestre)
        {
            if (bimestre.EhNulo())
                return;

            bimestre.FechamentoAbertura = this;
            bimestre.FechamentoAberturaId = Id;
            Bimestres.Add(bimestre);
        }

        public void AdicionarBimestres(IEnumerable<FechamentoReaberturaBimestre> listaBimestres)
        {
            Bimestres.AddRange(listaBimestres);
        }

        public void SobrescreverBimestres(IEnumerable<FechamentoReaberturaBimestre> listaBimestres)
        {
            Bimestres = listaBimestres.ToList();
        }

        public void AprovarWorkFlow()
        {
            if (Status != EntidadeStatus.AguardandoAprovacao)
                return;

            Status = EntidadeStatus.Aprovado;
            AprovadoEm = DateTime.Now;
        }

        public void AtualizarDre(Dre dre)
        {
            if (dre.EhNulo())
                return;

            Dre = dre;
            DreId = dre.Id;
        }

        public void AtualizarTipoCalendario(TipoCalendario tipoCalendario)
        {
            if (tipoCalendario.EhNulo())
                return;

            TipoCalendario = tipoCalendario;
            TipoCalendarioId = tipoCalendario.Id;
        }

        public void AtualizarUe(Ue ue)
        {
            if (ue.EhNulo())
                return;

            Ue = ue;
            UeId = ue.Id;
        }

        public void AtualizarAprovador(Usuario aprovador)
        {
            if (aprovador.EhNulo())
                return;

            Aprovador = aprovador;
            AprovadorId = aprovador.Id;
        }

        private bool EhParaDre()
        {
            return UeId is null && DreId > 0;
        }

        private bool EhParaSme()
        {
            return DreId is null && UeId is null;
        }

        public bool EhParaUe()
        {
            return DreId > 0 && UeId > 0;
        }

        private bool EstaNoRangeDeDatas(DateTime dataInicio, DateTime datafim)
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
            var bimestresOrdenados = Bimestres.OrderBy(a => a.Bimestre);
            return string.Join(",", bimestresOrdenados.Select(a => $"{a.Bimestre.ToString()}º").ToArray());
        }

        public bool[] ObterBimestresSelecionados()
        {
            var bimestresArray = new bool[TipoCalendario.QuantidadeDeBimestres()];

            for (int numero = 0; numero < TipoCalendario.QuantidadeDeBimestres(); numero++)
                bimestresArray[numero] = Bimestres.Any(a => a.Bimestre == numero + 1);

            return bimestresArray;
        }

        public void PodeSalvar(IEnumerable<FechamentoReabertura> fechamentosCadastrados, Usuario usuario)
        {
            if (Inicio > Fim)
                throw new NegocioException("A data início não pode ser maior que a data fim.");

            if (usuario.EhPerfilDRE() && VerificaAnoAtual())
                throw new NegocioException("Somente SME pode cadastrar reabertura no ano atual.");

            VerificaFechamentosNoMesmoPeriodo(fechamentosCadastrados);
        }

        public void ReprovarWorkFlow()
        {
            if (Status == EntidadeStatus.AguardandoAprovacao)
                Status = EntidadeStatus.Recusado;
        }

        public void VerificaStatus()
        {
            if (EhParaUe() && !VerificaAnoAtual())
            {
                Status = EntidadeStatus.AguardandoAprovacao;
            }
        }

        private bool VerificaAnoAtual()
        {
            return TipoCalendario.AnoLetivo == DateTime.Today.Year;
        }

        private void ConsistirFechamentoNoMesmoPeriodo(IEnumerable<FechamentoReabertura> fechamentosCadastrados)
        {
            foreach (var fechamento in fechamentosCadastrados.Where(fechamento => EstaNoRangeDeDatas(fechamento.Inicio, fechamento.Fim)))
                throw new NegocioException($"Não é possível persistir pois já existe uma reabertura cadastrada que começa em {fechamento.Inicio:dd/MM/yyyy} e termina em {fechamento.Fim:dd/MM/yyyy}");
        }

        private void VerificaFechamentosNoMesmoPeriodo(IEnumerable<FechamentoReabertura> fechamentosCadastrados)
        {
            if (EhParaSme())
            {
                var fechamentosSME = fechamentosCadastrados.Where(a => a.EhParaSme());
                if (fechamentosSME.NaoPossuiRegistros())
                    return;
                ConsistirFechamentoNoMesmoPeriodo(fechamentosSME);
            }
            else if (EhParaDre())
            {
                var fechamentosDre = (fechamentosCadastrados.Where(a => a.EhParaDre() && a.DreId == DreId)).ToList();
                if (!fechamentosDre.Any())
                    return;
                ConsistirFechamentoNoMesmoPeriodo(fechamentosDre);
            }
            else if (EhParaUe())
            {
                var fechamentosUe = fechamentosCadastrados.Where(a => a.EhParaUe() && a.UeId == UeId).ToList();
                if (!fechamentosUe.Any())
                    return;
                ConsistirFechamentoNoMesmoPeriodo(fechamentosUe);
            }
        }
    }
}