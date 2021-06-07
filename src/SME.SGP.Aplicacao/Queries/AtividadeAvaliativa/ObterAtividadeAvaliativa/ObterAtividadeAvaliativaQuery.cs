using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;

namespace SME.SGP.Aplicacao
{
    public class ObterAtividadeAvaliativaQuery : IRequest<AtividadeAvaliativa>
    {
        public DateTime DataAvaliacao { get; set; }
        public string DisciplinaId { get; set; }
        public string TurmaId { get; set; }
        public string UeId { get; set; }

        public ObterAtividadeAvaliativaQuery(DateTime dataAvaliacao, string disciplinaId, string turmaId, string ueId)
        {
            DataAvaliacao = dataAvaliacao;
            DisciplinaId = disciplinaId;
            TurmaId = turmaId;
            UeId = ueId;
        }
    }

    public class ObterAtividadeAvaliativaQueryValidator : AbstractValidator<ObterAtividadeAvaliativaQuery>
    {

        public ObterAtividadeAvaliativaQueryValidator()
        {
            RuleFor(c => c.DataAvaliacao)
                .NotEmpty()
                .WithMessage("A data avaliação deve ser informada.");
            RuleFor(c => c.DisciplinaId)
                .NotEmpty()
                .WithMessage("O id do componente curricular deve ser informado.");
            RuleFor(c => c.TurmaId)
                .NotEmpty()
                .WithMessage("O id da Turma deve ser informado.");
            RuleFor(c => c.UeId)
                .NotEmpty()
                .WithMessage("O id da UE deve ser informado.");
        }
    }
}
