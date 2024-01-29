using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterIndicativoPendenciasAulasPorTipoQuery : IRequest<PendenciaPaginaInicialListao>
    {
        public ObterIndicativoPendenciasAulasPorTipoQuery(Turma turma, 
                                                          Usuario usuario, 
                                                          string disciplinaId,
                                                          int anoLetivo,
                                                          int bimestre)
        {
            DisciplinaId = disciplinaId;
            TurmaId = turma.CodigoTurma;
            AnoLetivo = anoLetivo;
            Bimestre = bimestre;
            VerificaDiarioBordo = turma.EhTurmaInfantil && !usuario.EhProfessorCjInfantil();
            VerificaFrequencia = !turma.EhTurmaInfantil || !usuario.EhProfessorCjInfantil();
            VerificaAvaliacao = !turma.EhTurmaInfantil;
            VerificaPlanoAula = !turma.EhTurmaInfantil;
            ProfessorCj = usuario.EhProfessorCj();
            ProfessorNaoCj = usuario.EhProfessor();
            ProfessorRf = usuario.CodigoRf;
            EhGestor = usuario.EhGestorEscolar();

        }

        public string DisciplinaId { get; set; }
        public string TurmaId { get; set; }
        public int AnoLetivo { get; }
        public int Bimestre { get; }
        public bool VerificaDiarioBordo { get; }
        public bool VerificaFrequencia { get; }
        public bool VerificaAvaliacao { get; }
        public bool VerificaPlanoAula { get; }
        public bool ProfessorCj { get; }
        public bool ProfessorNaoCj { get; }
        public string ProfessorRf { get; }
        public bool EhGestor { get; }
    }

    public class ObterIndicativoPendenciasAulasPorTipoQueryValidator : AbstractValidator<ObterIndicativoPendenciasAulasPorTipoQuery>
    {
        public ObterIndicativoPendenciasAulasPorTipoQueryValidator()
        {
            RuleFor(c => c.DisciplinaId)
                .NotEmpty()
                .WithMessage("O código da disciplina deve ser informado para consulta de pendência na aula.");

            RuleFor(c => c.TurmaId)
                .NotEmpty()
                .WithMessage("O código da turma deve ser informado para consulta de pendência na aula.");
            
            RuleFor(c => c.AnoLetivo)
                .NotEmpty()
                .WithMessage("O ano letivo deve ser informado para consulta de pendência na aula.");            

            RuleFor(c => c.Bimestre)
                .NotEmpty()
                .WithMessage("O bimestre deve ser informado para consulta de pendência na aula.");
        }
    }
}
