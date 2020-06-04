using System;

namespace SME.SGP.Dominio
{
    public class Turma
    {
        public string Ano { get; set; }
        public int AnoLetivo { get; set; }
        public string CodigoTurma { get; set; }
        public DateTime DataAtualizacao { get; set; }
        public long Id { get; set; }
        public Modalidade ModalidadeCodigo { get; set; }
        public ModalidadeTipoCalendario ModalidadeTipoCalendario 
        { 
            get => ModalidadeCodigo == Modalidade.EJA ? 
                ModalidadeTipoCalendario.EJA : 
                ModalidadeTipoCalendario.FundamentalMedio; 
        }
        public string Nome { get; set; }
        public int QuantidadeDuracaoAula { get; set; }
        public int Semestre { get; set; }
        public int TipoTurno { get; set; }

        public Ue Ue { get; set; }
        public long UeId { get; set; }

        public void AdicionarUe(Ue ue)
        {
            if (ue != null)
            {
                Ue = ue;
                UeId = ue.Id;
            }
        }

        public ModalidadeTipoCalendario ObterModalidadeTipoCalendario()
        {
            if (ModalidadeCodigo == Modalidade.Fundamental || ModalidadeCodigo == Modalidade.Medio)
                return ModalidadeTipoCalendario.FundamentalMedio;
            else return ModalidadeTipoCalendario.EJA;
        }

        public bool MesmaModalidadePeriodoEscolar(ModalidadeTipoCalendario modalidade)
        {
            if (modalidade == ModalidadeTipoCalendario.EJA)
                return ModalidadeCodigo == Modalidade.EJA;
            else
                return ModalidadeCodigo != Modalidade.EJA;
        }

        public bool EhEJA()
            => ModalidadeCodigo == Modalidade.EJA;

        public int ObterHorasGradeRegencia()
            => EhEJA() ? 5 : 1;

        public int AnoTurmaInteiro => int.Parse(Ano);

        public bool EhTurmaFund1 => (ModalidadeCodigo == Modalidade.Fundamental && AnoTurmaInteiro >= 1 && AnoTurmaInteiro <= 5);
        public bool EhTurmaFund2 => (ModalidadeCodigo == Modalidade.Fundamental && AnoTurmaInteiro >= 6 && AnoTurmaInteiro <= 9);
        public bool EhTurmaEnsinoMedio => ModalidadeCodigo == Modalidade.Medio;

        public bool AulasReposicaoPrecisamAprovacao(int quantidadeAulasExistentesNoDia)
        {
            int.TryParse(Ano, out int anoTurma);
            return (EhTurmaFund1 || (EhEJA() && (anoTurma == 1 || anoTurma == 2)) && quantidadeAulasExistentesNoDia > 1) || 
                   (EhTurmaFund2 || (EhEJA() && (anoTurma == 3 || anoTurma == 4))) || 
                   (EhTurmaEnsinoMedio && quantidadeAulasExistentesNoDia > 2);
        }
    }
}