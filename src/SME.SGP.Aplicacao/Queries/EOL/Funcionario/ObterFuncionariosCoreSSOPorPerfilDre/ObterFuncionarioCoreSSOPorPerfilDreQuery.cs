using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterFuncionarioCoreSSOPorPerfilDreQuery : IRequest<IEnumerable<UsuarioEolRetornoDto>>
    {
        public ObterFuncionarioCoreSSOPorPerfilDreQuery(Guid codigoPerfil, string codigoDre)
        {
            CodigoPerfil = codigoPerfil;
            CodigoDre = codigoDre;
        }
        public Guid CodigoPerfil { get; set; }
        public string CodigoDre { get; set; }
    }

    public class ObterFuncionarioCoreSSOPorPerfilDreQueryValidator : AbstractValidator<ObterFuncionarioCoreSSOPorPerfilDreQuery>
    {
        public ObterFuncionarioCoreSSOPorPerfilDreQueryValidator()
        {
            RuleFor(x => x.CodigoPerfil).NotEmpty().WithMessage("O Código do perfil precisa ser informado para obter funcionarios Core SSO");
            RuleFor(x => x.CodigoDre).NotEmpty().WithMessage("O Código da DRE precisa ser informado para obter funcionarios Core SSO");
        }
    }
}
