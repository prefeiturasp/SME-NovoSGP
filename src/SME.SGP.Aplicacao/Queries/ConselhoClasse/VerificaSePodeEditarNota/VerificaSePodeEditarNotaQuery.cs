using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class VerificaSePodeEditarNotaQuery : IRequest<bool>
    {
        public VerificaSePodeEditarNotaQuery(string alunoCodigo, Turma turma, PeriodoEscolar periodoEscolar)
        {
            AlunoCodigo = alunoCodigo;
            Turma = turma;
            PeriodoEscolar = periodoEscolar;
        }

        public string AlunoCodigo { get; set; } 
        public Turma Turma { get; set; }
        public PeriodoEscolar PeriodoEscolar { get; set; }
    }
}
