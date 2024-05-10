using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterFiltroRelatoriosModalidadesPorUeQuery : IRequest<IEnumerable<OpcaoDropdownDto>>
    {
        public ObterFiltroRelatoriosModalidadesPorUeQuery(string codigoUe, int anoLetivo, bool consideraHistorico, string login, Guid perfil)
        {
            CodigoUe = codigoUe;
            AnoLetivo = anoLetivo;
            ConsideraHistorico = consideraHistorico;
            Login = login;
            Perfil = perfil;
        }

        public ObterFiltroRelatoriosModalidadesPorUeQuery(string codigoUe, int anoLetivo, bool consideraHistorico, string login, Guid perfil, IEnumerable<Modalidade> modalidadesQueSeraoIgnoradas)
            : this(codigoUe, anoLetivo, consideraHistorico, login, perfil)
        {
            ModalidadesQueSeraoIgnoradas = modalidadesQueSeraoIgnoradas;
        }

        public string CodigoUe { get; }
        public int AnoLetivo { get; set; }
        public bool ConsideraHistorico { get; set; }
        public IEnumerable<Modalidade> ModalidadesQueSeraoIgnoradas { get; set; }
        public string Login { get; set; }
        public Guid Perfil { get; set; }
    }

    public class ObterFiltroRelatoriosModalidadesPorUeQueryValidator : AbstractValidator<ObterFiltroRelatoriosModalidadesPorUeQuery>
    {
        public ObterFiltroRelatoriosModalidadesPorUeQueryValidator()
        {
            RuleFor(c => c.CodigoUe)
            .NotEmpty()
            .WithMessage("O código da ue deve ser informado.");

            RuleFor(c => c.AnoLetivo)
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
