using FluentValidation;
using MediatR;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTotalCompensacoesComponenteNaoLancaNotaQuery : IRequest<IEnumerable<TotalCompensacoesComponenteNaoLancaNotaDto>>
    {
        public ObterTotalCompensacoesComponenteNaoLancaNotaQuery(string codigoTurma, int bimestre)
        {
            CodigoTurma = codigoTurma;
            Bimestre = bimestre;
        }
        public string CodigoTurma { get; set; }
        public int Bimestre { get; set; }
    }

    public class ObterTotalCompensacoesComponenteNaoLancaNotaQueryValidator : AbstractValidator<ObterTotalCompensacoesComponenteNaoLancaNotaQuery>
    {
        public ObterTotalCompensacoesComponenteNaoLancaNotaQueryValidator()
        {
            RuleFor(x => x.CodigoTurma).NotEmpty().WithMessage("O código da turma deve ser informado para obter o total de compensações");
            RuleFor(x => x.Bimestre).GreaterThanOrEqualTo(0).WithMessage("O bimestre  deve ser maior ou igual a 0 para obter o total de compensações");

        }
    }
}
