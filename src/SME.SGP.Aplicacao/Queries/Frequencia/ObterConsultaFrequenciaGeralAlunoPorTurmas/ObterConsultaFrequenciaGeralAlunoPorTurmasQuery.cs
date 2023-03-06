using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterConsultaFrequenciaGeralAlunoPorTurmasQuery : IRequest<string>
    {
        public ObterConsultaFrequenciaGeralAlunoPorTurmasQuery(string alunoCodigo, string[] turmaCodigo)
        {
            AlunoCodigo = alunoCodigo;
            TurmaCodigo = turmaCodigo;
        }

        public ObterConsultaFrequenciaGeralAlunoPorTurmasQuery(string alunoCodigo, string[] turmaCodigo, string componenteCurricularCodigo, Turma turmaConsulta)
            : this(alunoCodigo, turmaCodigo)
        {
            ComponenteCurricularCodigo = componenteCurricularCodigo;
            TurmaConsulta = turmaConsulta;
        }

        public string AlunoCodigo { get; set; }
        public string[] TurmaCodigo { get; set; }
        public string ComponenteCurricularCodigo { get; set; }
        public Turma TurmaConsulta { get; set; } 
    }

    public class ObterConsultaFrequenciaGeralAlunoPorTurmasQueryValidator : AbstractValidator<ObterConsultaFrequenciaGeralAlunoPorTurmasQuery>
    {
        public ObterConsultaFrequenciaGeralAlunoPorTurmasQueryValidator()
        {
            RuleFor(a => a.AlunoCodigo)
                .NotEmpty()
                .WithMessage("É preciso informar o código do aluno para consultar a frequência");
            RuleFor(a => a.TurmaCodigo)
               .NotEmpty()
               .WithMessage("É preciso informar os códigos das turmas para consultar a frequência");
        }
    }
}
