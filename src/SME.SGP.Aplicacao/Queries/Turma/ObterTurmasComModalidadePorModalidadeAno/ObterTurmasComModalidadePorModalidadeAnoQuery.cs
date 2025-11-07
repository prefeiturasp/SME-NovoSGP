using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasComModalidadePorModalidadeAnoQuery : IRequest<IEnumerable<TurmaModalidadeSerieAnoDto>>
    {
        public ObterTurmasComModalidadePorModalidadeAnoQuery(int ano, List<long> ueId, IEnumerable<int> modalidades)
        {
            Ano = ano;
            UeId = ueId;
            Modalidades = modalidades;
        }

        public int Ano { get; }
        public List<long> UeId { get; }
        public IEnumerable<int> Modalidades { get; set; }
    }

    public class ObterTurmasComModalidadePorModalidadeAnoQueryValidator : AbstractValidator<ObterTurmasComModalidadePorModalidadeAnoQuery>
    {
        public ObterTurmasComModalidadePorModalidadeAnoQueryValidator()
        {
            RuleFor(a => a.Ano)
                .NotEmpty()
                .WithMessage("O ano deve ser informado para consulta de turmas e modalidades");

            RuleFor(a => a.UeId)
                .NotEmpty()
                .WithMessage("O identificador da UE deve ser informado para consulta de turmas e modalidades");

            RuleFor(a => a.Modalidades)
               .NotEmpty()
               .WithMessage("O identificador da Modalidade deve ser informado para consulta de turmas e modalidades");
        }
    }
}
