using MediatR;

namespace SME.SGP.Aplicacao
{
    public class RegistrarFrequenciaTurmaEvasaoCommand : IRequest<long>
    {
        public RegistrarFrequenciaTurmaEvasaoCommand(long turmaId, int mes,
            int quantidadeAlunosAbaixo50Porcento, int quantidadeAlunox0Porcento)
        {
            TurmaId = turmaId;
            Mes = mes;
            QuantidadeAlunosAbaixo50Porcento = quantidadeAlunosAbaixo50Porcento;
            QuantidadeAlunox0Porcento = quantidadeAlunox0Porcento;
        }

        public long TurmaId { get; set; }
        public int Mes { get; set; }
        public int QuantidadeAlunosAbaixo50Porcento { get; set;  }
        public int QuantidadeAlunox0Porcento { get; set; }
    }
}
