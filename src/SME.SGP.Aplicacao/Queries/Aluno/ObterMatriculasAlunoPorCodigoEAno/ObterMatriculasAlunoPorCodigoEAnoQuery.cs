using FluentValidation;
using MediatR;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterMatriculasAlunoPorCodigoEAnoQuery : IRequest<IEnumerable<AlunoPorTurmaResposta>>
    {
        public ObterMatriculasAlunoPorCodigoEAnoQuery(string codigoAluno, int anoLetivo, bool historico = false, bool filtrarSituacao = true, bool tipoTurma = false)
        {
            CodigoAluno = codigoAluno;
            AnoLetivo = anoLetivo;
            Historico = historico;
            FiltrarSituacao = filtrarSituacao;
            TipoTurma = tipoTurma;
        }

        public string CodigoAluno { get; }
        public int AnoLetivo { get; }
        public bool Historico { get; set; }
        public bool FiltrarSituacao { get; set; }
        public bool TipoTurma { get; set; }
    }

    public class ObterMatriculasAlunoPorCodigoEAnoQueryValidator : AbstractValidator<ObterMatriculasAlunoPorCodigoEAnoQuery>
    {
        public ObterMatriculasAlunoPorCodigoEAnoQueryValidator()
        {
            RuleFor(a => a.CodigoAluno)
                .NotEmpty()
                .WithMessage("O código do aluno deve ser informado para consulta de suas matriculas");

            RuleFor(a => a.AnoLetivo)
                .NotEmpty()
                .WithMessage("O ano letivo deve ser informado para consulta das matriculas do aluno");
        }
    }
}
