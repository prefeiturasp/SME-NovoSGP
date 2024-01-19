using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterRegistroAcaoComTurmaPorIdQuery : IRequest<RegistroAcaoBuscaAtiva>
    {
        public ObterRegistroAcaoComTurmaPorIdQuery(long registroAcaoId)
        {
            RegistroAcaoId = registroAcaoId;
        }

        public long RegistroAcaoId { get; }
    }

    public class ObterRegistroAcaoComTurmaPorIdQueryValidator : AbstractValidator<ObterRegistroAcaoComTurmaPorIdQuery>
    {
        public ObterRegistroAcaoComTurmaPorIdQueryValidator()
        {
            RuleFor(a => a.RegistroAcaoId)
                .NotEmpty()
                .WithMessage("O id do registro de ação é necessário para pesquisa.");
        }
    }
}
