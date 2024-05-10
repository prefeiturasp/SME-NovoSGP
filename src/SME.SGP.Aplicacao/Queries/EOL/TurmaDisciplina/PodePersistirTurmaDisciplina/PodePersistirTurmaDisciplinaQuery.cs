using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class PodePersistirTurmaDisciplinaQuery : IRequest<bool>
    {
        public PodePersistirTurmaDisciplinaQuery(string criadoRF, string turmaCodigo, string componenteParaVerificarAtribuicao, long dataTick)
        {
            CriadoRF = criadoRF;
            TurmaCodigo = turmaCodigo;
            ComponenteParaVerificarAtribuicao = componenteParaVerificarAtribuicao;
            DataTick = dataTick;
        }

        public string CriadoRF { get;}
        public string TurmaCodigo { get; }
        public string ComponenteParaVerificarAtribuicao { get; }
        public long DataTick { get; }
    }

    public class PodePersistirTurmaDisciplinaQueryValidator : AbstractValidator<PodePersistirTurmaDisciplinaQuery>
    {
        public PodePersistirTurmaDisciplinaQueryValidator()
        {
            RuleFor(c => c.CriadoRF)
            .NotEmpty()
            .WithMessage("O Rf deve ser informado.");

            RuleFor(c => c.TurmaCodigo)
            .NotEmpty()
            .WithMessage("O código da turma deve ser informado.");

            RuleFor(c => c.ComponenteParaVerificarAtribuicao)
            .NotEmpty()
            .WithMessage("O ComponenteParaVerificarAtribuicao deve ser informado.");

            RuleFor(c => c.DataTick)
            .NotEmpty()
            .WithMessage("A data em tick deve ser informado.");

        }
    }
}
