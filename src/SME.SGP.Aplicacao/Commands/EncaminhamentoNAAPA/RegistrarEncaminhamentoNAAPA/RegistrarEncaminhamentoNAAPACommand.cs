using FluentValidation;
using MediatR;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class RegistrarEncaminhamentoNAAPACommand : IRequest<ResultadoAtendimentoNAAPADto>
    {
        public long TurmaId { get; set; }
        public SituacaoNAAPA Situacao { get; set; }
        public string AlunoNome { get; set; }
        public string AlunoCodigo { get; set; }

        public RegistrarEncaminhamentoNAAPACommand()
        {
        }

        public RegistrarEncaminhamentoNAAPACommand(long turmaId, string alunoNome, string alunoCodigo, SituacaoNAAPA situacao)
        {
            TurmaId = turmaId;
            AlunoNome = alunoNome;
            AlunoCodigo = alunoCodigo;
            Situacao = situacao;
        }
    }
    public class RegistrarEncaminhamentoNAAPACommandValidator : AbstractValidator<RegistrarEncaminhamentoNAAPACommand>
    {
        public RegistrarEncaminhamentoNAAPACommandValidator()
        {
            RuleFor(x => x.TurmaId)
                   .GreaterThan(0)
                   .WithMessage("A turma deve ser informada para o registro do encaminhamento NAAPA!");
        }
    }
}
