using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ConsolidarTurmaConselhoClasseAlunoCommand : IRequest<bool>
    {
        public ConsolidarTurmaConselhoClasseAlunoCommand(string alunoCodigo, long turmaId, int bimestre, bool inativo)
        {
            AlunoCodigo = alunoCodigo;
            TurmaId = turmaId;
            Bimestre = bimestre;
            Inativo = inativo;
        }

        public string AlunoCodigo { get; set; }
        public long TurmaId { get; set; }
        public int Bimestre { get; set; }
        public bool Inativo { get; set; }
    }
}
