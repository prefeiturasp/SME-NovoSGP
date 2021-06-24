using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterRegistrosIndividuaisPorTurmaAlunoQuery : IRequest<IEnumerable<RegistroIndividualAlunoDTO>>
    {
        public ObterRegistrosIndividuaisPorTurmaAlunoQuery(long turmaCodigo, long alunoCodigo)
        {
            TurmaCodigo = turmaCodigo;
            AlunoCodigo = alunoCodigo;
            Modalidades = new[] { (int)Modalidade.InfantilPreEscola, (int)Modalidade.InfantilCEI};
        }

        public long AlunoCodigo { get; set; }
        public long TurmaCodigo { get; set; }
        public int[] Modalidades { get; set; }
    }

    public class ObterRegistrosIndividuaisPorTurmaAlunoQueryValidator : AbstractValidator<ObterRegistrosIndividuaisPorTurmaAlunoQuery>
    {
        public ObterRegistrosIndividuaisPorTurmaAlunoQueryValidator()
        {
            RuleFor(c => c.AlunoCodigo)
            .NotEmpty()
            .WithMessage("O Código do aluno deve ser informado para consulta de turmas com registros individuais.");

            RuleFor(c => c.TurmaCodigo)
            .NotEmpty()
            .WithMessage("O Código da Turma deve ser informado para consulta de turmas com registros individuais.");


        }
    }
}
