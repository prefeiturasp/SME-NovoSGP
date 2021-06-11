using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterMiniaturasFotosSemestreAlunoQuery : IRequest<IEnumerable<MiniaturaFotoDto>>
    {
        public ObterMiniaturasFotosSemestreAlunoQuery(long acompanhamentoSemestreId, int quantidadeFotos)
        {
            AcompanhamentoSemestreId = acompanhamentoSemestreId;
            QuantidadeFotos = quantidadeFotos;
        }

        public long AcompanhamentoSemestreId { get; }
        public int QuantidadeFotos { get; }
    }

    public class ObterMiniaturasFotosSemestreAlunoQueryValidator : AbstractValidator<ObterMiniaturasFotosSemestreAlunoQuery>
    {
        public ObterMiniaturasFotosSemestreAlunoQueryValidator()
        {
            RuleFor(a => a.AcompanhamentoSemestreId)
                .NotEmpty()
                .WithMessage("O id do acompanhamento do estudante/criança no semestre deve ser informado para consulta de suas fotos");

            RuleFor(a => a.QuantidadeFotos)
                .NotEmpty()
                .WithMessage("A quantidade de fotos do acompanhamento do estudante/criança no semestre deve ser informada para consulta de suas fotos");
        }
    }
}
