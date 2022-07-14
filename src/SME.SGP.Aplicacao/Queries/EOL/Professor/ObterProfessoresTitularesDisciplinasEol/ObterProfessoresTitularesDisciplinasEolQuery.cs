using System;
using System.Collections.Generic;
using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterProfessoresTitularesDisciplinasEolQuery : IRequest<IEnumerable<ProfessorTitularDisciplinaEol>>
    {
        public ObterProfessoresTitularesDisciplinasEolQuery(string turmaCodigo, DateTime? dataReferencia = null, string professorRf = null, bool realizaAgrupamento = true)
        {
            CodigoTurma = turmaCodigo;
            DataReferencia = dataReferencia;
            ProfessorRf = professorRf;
            RealizaAgrupamento = realizaAgrupamento;
        }

        public string CodigoTurma { get; set; }
        public DateTime? DataReferencia  { get; set; }
        public string ProfessorRf  { get; set; }
        public bool RealizaAgrupamento { get; set; }
    }

    public class ObterProfessoresTitularesDisciplinasEolQueryValidator : AbstractValidator<
            ObterProfessoresTitularesDisciplinasEolQuery>
    {
        public ObterProfessoresTitularesDisciplinasEolQueryValidator()
        {
            RuleFor(x => x.CodigoTurma).NotEmpty().WithMessage("Informe o Código da Turma para Obter Professores Titulares Disciplinas do EOL");
        } 
    }
}