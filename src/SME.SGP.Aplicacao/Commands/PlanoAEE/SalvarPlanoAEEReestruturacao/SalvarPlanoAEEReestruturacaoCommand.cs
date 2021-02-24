using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class SalvarPlanoAEEReestruturacaoCommand : IRequest<long>
    {
        public SalvarPlanoAEEReestruturacaoCommand(PlanoAEEReestruturacao reestruturacao)
        {
            Reestruturacao = reestruturacao;
        }

        public PlanoAEEReestruturacao Reestruturacao { get; }
    }

    public class SalvarPlanoAEEReestruturacaoCommandValidator : AbstractValidator<SalvarPlanoAEEReestruturacaoCommand>
    {
        public SalvarPlanoAEEReestruturacaoCommandValidator()
        {
            RuleFor(a => a.Reestruturacao)
                .NotEmpty()
                .WithMessage("A reestruturação do plano deve ser informada para o salvamento");

            RuleFor(a => a.Reestruturacao.Descricao)
                .NotEmpty()
                .WithMessage("É necessário informar a descrição da reestruturação para o salvamento");

            RuleFor(a => a.Reestruturacao.PlanoAEEVersaoId)
                .NotEmpty()
                .WithMessage("É necessário informar a versão do plano para o salvamento da reestruturação");
        }
    }
}

