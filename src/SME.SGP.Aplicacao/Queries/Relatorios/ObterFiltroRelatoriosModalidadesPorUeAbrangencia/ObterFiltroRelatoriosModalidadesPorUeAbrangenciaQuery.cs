using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterFiltroRelatoriosModalidadesPorUeAbrangenciaQuery : IRequest<IEnumerable<OpcaoDropdownDto>>
    {
        public string CodigoUe { get; }
        public IEnumerable<Modalidade> ModalidadesQueSeraoIgnoradas { get; set; }
        public string Login { get; set; }
        public Guid Perfil { get; set; }

        public ObterFiltroRelatoriosModalidadesPorUeAbrangenciaQuery(string codigoUe, string login, Guid perfil, IEnumerable<Modalidade> modalidadesQueSeraoIgnoradas)
        {
            CodigoUe = codigoUe;
            Login = login;
            Perfil = perfil;
            ModalidadesQueSeraoIgnoradas = modalidadesQueSeraoIgnoradas;
        }
    }

    public class ObterFiltroRelatoriosModalidadesPorUeAbrangenciaQueryValidator : AbstractValidator<ObterFiltroRelatoriosModalidadesPorUeAbrangenciaQuery>
    {
        public ObterFiltroRelatoriosModalidadesPorUeAbrangenciaQueryValidator()
        {
            RuleFor(c => c.CodigoUe)
            .NotEmpty()
            .WithMessage("O código da ue deve ser informado.");

            RuleFor(x => x.Login)
                .NotEmpty()
                .WithMessage("O login do usuário logado deve ser informado.");

            RuleFor(x => x.Perfil)
                .NotEmpty()
                .WithMessage("O perfil do usuário logado deve ser informado.");
        }
    }
}