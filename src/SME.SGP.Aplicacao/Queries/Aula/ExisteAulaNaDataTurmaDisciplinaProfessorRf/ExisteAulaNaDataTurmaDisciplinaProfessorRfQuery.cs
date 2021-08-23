using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;

namespace SME.SGP.Aplicacao
{
    public class ExisteAulaNaDataTurmaDisciplinaProfessorRfQuery : IRequest<bool>
    {
        public ExisteAulaNaDataTurmaDisciplinaProfessorRfQuery(DateTime dataAula, string turmaCodigo, string disciplinaId, string professorRf, TipoAula tipoAula)
        {
            DataAula = dataAula;
            TurmaCodigo = turmaCodigo;
            DisciplinaId = disciplinaId;
            ProfessorRf = professorRf;
            TipoAula = tipoAula;
        }

        public DateTime DataAula { get; }
        public string TurmaCodigo { get; }
        public string DisciplinaId { get; }
        public string ProfessorRf { get; }
        public TipoAula TipoAula { get; }
    }

    public class ExisteAulaNaDataTurmaDisciplinaProfessorRfQueryValidator : AbstractValidator<ExisteAulaNaDataTurmaDisciplinaProfessorRfQuery>
    {
        public ExisteAulaNaDataTurmaDisciplinaProfessorRfQueryValidator()
        {
            RuleFor(a => a.DataAula)
                .NotEmpty()
                .WithMessage("A data da aula deve ser informada para consulta de existência de aulas");

            RuleFor(a => a.TurmaCodigo)
                .NotEmpty()
                .WithMessage("O código da turma deve ser informado para consulta de existência de aulas");

            RuleFor(a => a.DisciplinaId)
                .NotEmpty()
                .WithMessage("O código do componente curricular deve ser informado para consulta de existência de aulas");

            RuleFor(a => a.ProfessorRf)
                .NotEmpty()
                .WithMessage("O RF do professor deve ser informado para consulta de existência de aulas");
        }
    }
}
