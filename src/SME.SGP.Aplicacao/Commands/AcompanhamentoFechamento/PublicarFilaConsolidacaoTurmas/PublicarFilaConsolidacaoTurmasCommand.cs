using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class PublicarFilaConsolidacaoTurmasCommand : IRequest<bool>
    {
    

        public PublicarFilaConsolidacaoTurmasCommand(ConsolidacaoTurmaDto consolidacaoTurma)
        {
            this.Bimestre = consolidacaoTurma.Bimestre;
            this.TurmaId = consolidacaoTurma.TurmaId;
        }

        public int Bimestre { get; set; }
        public long TurmaId { get; set; }
       
    }

    public class PublicarFilaConsolidacaoTurmasCommandValidator : AbstractValidator<PublicarFilaConsolidacaoTurmasCommand>
    {
        public PublicarFilaConsolidacaoTurmasCommandValidator()
        {
            

            RuleFor(c => c.Bimestre)
               .NotEmpty()
               .WithMessage("O Bimestre precisa ser informado.");

            RuleFor(c => c.TurmaId)
               .NotEmpty()
               .WithMessage("A TurmaCodigo precisa ser informado.");
        }
    }
}

