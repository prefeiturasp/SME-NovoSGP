using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ObterConsultaFrequenciaGeralAlunoQuery : IRequest<string>
    {
        public ObterConsultaFrequenciaGeralAlunoQuery(string alunoCodigo, string turmaCodigo)
        {
            AlunoCodigo = alunoCodigo;
            TurmaCodigo = turmaCodigo;
        }

        public ObterConsultaFrequenciaGeralAlunoQuery(string alunoCodigo, string turmaCodigo, string componenteCurricularCodigo)
            : this(alunoCodigo, turmaCodigo)
        {
            ComponenteCurricularCodigo = componenteCurricularCodigo;
        }

        public string AlunoCodigo { get; set; }
        public string TurmaCodigo { get; set; }
        public string ComponenteCurricularCodigo { get; set; }
    }

    public class ObterConsultaFrequenciaGeralAlunoQueryValidator : AbstractValidator<ObterConsultaFrequenciaGeralAlunoQuery>
    {
        public ObterConsultaFrequenciaGeralAlunoQueryValidator()
        {
            RuleFor(a => a.AlunoCodigo)
                .NotEmpty()
                .WithMessage("É preciso informar o código do aluno para consultar a frequência");
            RuleFor(a => a.TurmaCodigo)
               .NotEmpty()
               .WithMessage("É preciso informar o código da turma para consultar a frequência");
        }
    }
}
