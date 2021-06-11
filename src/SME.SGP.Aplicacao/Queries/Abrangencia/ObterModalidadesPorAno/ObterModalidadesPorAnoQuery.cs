using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterModalidadesPorAnoQuery : IRequest<IEnumerable<EnumeradoRetornoDto>>
    {
        public int AnoLetivo { get; set; }
        public bool ConsideraHistorico { get; set; }
        public IEnumerable<Modalidade> ModadlidadesQueSeraoIgnoradas { get; set; }
        public string Login { get; set; }
        public Guid Perfil { get; set; }

        public ObterModalidadesPorAnoQuery(int anoLetivo, bool consideraHistorico, string login, Guid perfil)
        {
            AnoLetivo = anoLetivo;
            ConsideraHistorico = consideraHistorico;
            Login = login;
            Perfil = perfil;
        }

        public ObterModalidadesPorAnoQuery(int anoLetivo, bool consideraHistorico, string login, Guid perfil, IEnumerable<Modalidade> modadlidadesQueSeraoIgnoradas)
            :this(anoLetivo, consideraHistorico, login, perfil)
        {
            ModadlidadesQueSeraoIgnoradas = modadlidadesQueSeraoIgnoradas;
        }
    }

    public class ObterModalidadesPorAnoQueryValidator : AbstractValidator<ObterModalidadesPorAnoQuery>
    {
        public ObterModalidadesPorAnoQueryValidator()
        {
            RuleFor(x => x.AnoLetivo)
                .NotEmpty()
                .WithMessage("O ano letivo deve ser informado.");

            RuleFor(x => x.Login)
                .NotEmpty()
                .WithMessage("O login do usuário logado deve ser informado.");

            RuleFor(x => x.Perfil)
                .NotEmpty()
                .WithMessage("O perfil do usuário logado deve ser informado.");
        }
    }
}