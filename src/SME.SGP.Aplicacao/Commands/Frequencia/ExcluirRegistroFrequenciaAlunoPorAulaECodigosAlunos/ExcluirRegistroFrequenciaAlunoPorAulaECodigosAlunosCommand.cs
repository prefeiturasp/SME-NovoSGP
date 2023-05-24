using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ExcluirRegistroFrequenciaAlunoPorAulaECodigosAlunosCommand : IRequest<bool>
    {
        public long AulaId { get; set; }
        public string[] CodigosAlunos { get; set; }

        public ExcluirRegistroFrequenciaAlunoPorAulaECodigosAlunosCommand(long aulaId, string[] codigosAlunos)
        {
            AulaId = aulaId;
            CodigosAlunos = codigosAlunos;
        }
    }

    public class ExcluirRegistroFrequenciaAlunoPorAulaECodigosAlunosCommandValidator : AbstractValidator<ExcluirRegistroFrequenciaAlunoPorAulaECodigosAlunosCommand>
    {
        public ExcluirRegistroFrequenciaAlunoPorAulaECodigosAlunosCommandValidator()
        {
            RuleFor(a => a.AulaId)
                .NotEmpty()
                .WithMessage("É necessário informar o identificador da aula para excluir os registros de frequência dos alunos dessa aula");

            RuleFor(a => a.CodigosAlunos)
                .NotEmpty()
                .WithMessage("É necessário informar a lista de códigos dos alunos para excluir os registros de frequência dos alunos dessa aula");

        }
    }
}
