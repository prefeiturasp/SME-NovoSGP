using FluentValidation;
using MediatR;
using SME.SGP.Dominio.Enumerados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AlterarConsolidacaoDashboardFrequenciaTurmaCommand : IRequest<bool>
    {
        public long Id { get; set; }
        public int QuantidadeRemotos { get; set; }
        public int QuantidadeAusentes { get; set; }
        public int QuantidadePresentes { get; set; }

        public AlterarConsolidacaoDashboardFrequenciaTurmaCommand(long id, int quantidadeRemotos, int quantidadeAusentes, int quantidadePresentes)
        {
            Id = id;
            QuantidadeRemotos = quantidadeRemotos;
            QuantidadeAusentes = quantidadeAusentes;
            QuantidadePresentes = quantidadePresentes;
        }
    }

    public class AlterarConsolidacaoDashboardFrequenciaTurmaCommandValidator : AbstractValidator<AlterarConsolidacaoDashboardFrequenciaTurmaCommand>
    {
        public AlterarConsolidacaoDashboardFrequenciaTurmaCommandValidator()
        {
            RuleFor(a => a.Id)
                .NotEmpty()
                .WithMessage("É necessário informar o id da consolidação já existente para poder ser feita a alteração dos novos dados");
        }
    }
}
