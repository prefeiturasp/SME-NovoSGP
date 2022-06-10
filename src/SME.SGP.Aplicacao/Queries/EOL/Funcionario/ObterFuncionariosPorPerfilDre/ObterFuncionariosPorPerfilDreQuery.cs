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
    public class ObterFuncionariosPorPerfilDreQuery : IRequest<IEnumerable<UsuarioEolRetornoDto>>
    {
        public ObterFuncionariosPorPerfilDreQuery(Guid codigoPerfil, string codigoDre)
        {
            CodigoPerfil = codigoPerfil;
            CodigoDre = codigoDre;
        }
        public Guid CodigoPerfil { get; set; }
        public string CodigoDre { get; set; }
    }

    public class ObterFuncionariosPorPerfilDreQueryValidator : AbstractValidator<ObterFuncionariosPorPerfilDreQuery>
    {
        public ObterFuncionariosPorPerfilDreQueryValidator()
        {
            RuleFor(x => x.CodigoPerfil).NotEmpty().WithMessage("O Código do perfil deve ser informado para obter funcionarios por perfil e dre");
            RuleFor(x => x.CodigoDre).NotEmpty().WithMessage("O Código do perfil deve ser informado para obter funcionarios por perfil e dre");
        }
    }
}
