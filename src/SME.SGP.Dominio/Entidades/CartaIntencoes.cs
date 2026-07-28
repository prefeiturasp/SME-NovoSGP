using Dapper.Contrib.Extensions;

namespace SME.SGP.Dominio
{
    public class CartaIntencoes : EntidadeBase
    {
        public long TurmaId { get; set; }

        [Computed]
        public Turma Turma { get; set; }

        public long PeriodoEscolarId { get; set; }

        [Computed]
        public PeriodoEscolar PeriodoEscolar { get; set; }

        public long ComponenteCurricularId { get; set; }

        public string Planejamento { get; set; }

        public bool Excluido { get; set; }

        public void AdicionarPeriodoEscolar(PeriodoEscolar periodoEscolar)
        {
            PeriodoEscolar = periodoEscolar;
        }
    }
}
