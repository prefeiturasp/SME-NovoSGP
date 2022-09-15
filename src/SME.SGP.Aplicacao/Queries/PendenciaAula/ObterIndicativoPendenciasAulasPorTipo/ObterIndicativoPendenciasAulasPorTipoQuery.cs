using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterIndicativoPendenciasAulasPorTipoQuery : IRequest<PendenciaPaginaInicialListao>
    {
        public ObterIndicativoPendenciasAulasPorTipoQuery(string disciplinaId,
                                                          string turmaId,
                                                          int anoLetivo,
                                                          int bimestre,
                                                          bool verificaDiarioBordo = false,
                                                          bool verificaFrequencia = false,
                                                          bool verificaAvaliacao = false,
                                                          bool verificaPlanoAula = false,
                                                          bool professorCj = false,
                                                          bool professorNaoCj = false,
                                                          string professorRf = "",
                                                          bool ehGestor = false)
        {
            DisciplinaId = disciplinaId;
            TurmaId = turmaId;
            AnoLetivo = anoLetivo;
            Bimestre = bimestre;
            VerificaDiarioBordo = verificaDiarioBordo;
            VerificaFrequencia = verificaFrequencia;
            VerificaAvaliacao = verificaAvaliacao;
            VerificaPlanoAula = verificaPlanoAula;
            ProfessorCj = professorCj;
            ProfessorNaoCj = professorNaoCj;
            ProfessorRf = professorRf;
            EhGestor = ehGestor;

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
