using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;
using FluentValidation;

namespace SME.SGP.Aplicacao
{
    public class ObterFuncionariosPorRfUeNomeServidorQuery : IRequest<IEnumerable<UsuarioEolRetornoDto>>
    {
        public ObterFuncionariosPorRfUeNomeServidorQuery(string codigoRF,string codigoUE,string nomeServidor)
        {
            CodigoRF = codigoRF;
            CodigoUE = codigoUE;
            NomeServidor = nomeServidor;
        }

        public string CodigoRF { get; set; }
        public string CodigoUE { get; set; }
        public string NomeServidor { get; set; }
    }
    
    public class ObterFuncionariosPorRfUeNomeServidorQueryValidator : AbstractValidator<ObterFuncionariosPorRfUeNomeServidorQuery>
    {
        public ObterFuncionariosPorRfUeNomeServidorQueryValidator()
        {
            RuleFor(c => c.CodigoUE)
                .NotEmpty()
                .WithMessage("O código da Ue deve ser informado para obter funcionários.");
        }
    }
}
