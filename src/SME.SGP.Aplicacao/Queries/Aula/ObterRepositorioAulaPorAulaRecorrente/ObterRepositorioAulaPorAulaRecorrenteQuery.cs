using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterRepositorioAulaPorAulaRecorrenteQuery : IRequest<IEnumerable<Aula>>
    {
        public long AulaPaiIdOrigem { get; set; }
        public long AulaOrigemId { get; set; }
        public DateTime FimRecorrencia { get; set; }

        public ObterRepositorioAulaPorAulaRecorrenteQuery(long aulaPaiIdOrigem, long aulaOrigemId, DateTime fimRecorrencia)
        {
            AulaPaiIdOrigem = aulaPaiIdOrigem;
            AulaOrigemId = aulaOrigemId;
            FimRecorrencia = fimRecorrencia;
        }
    }

    public class ObterRepositorioAulaPorAulaRecorrenteQueryValidator : AbstractValidator<ObterRepositorioAulaPorAulaRecorrenteQuery>
    {
        public ObterRepositorioAulaPorAulaRecorrenteQueryValidator()
        {
            RuleFor(x => x.AulaPaiIdOrigem)
                .NotEmpty()
                .WithMessage("A aula id pai de origem deve ser informada.");

            RuleFor(x => x.AulaOrigemId)
                .NotEmpty()
                .WithMessage("A aula origem id deve ser informada.");

            RuleFor(x => x.FimRecorrencia)
                .NotEmpty()
                .WithMessage("A data de fim recorrência deve ser informada.");
        }
    }
}
