using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterAjudasDoSistemaQuery : IRequest<IEnumerable<AjudaDoSistemaDto>>
    {
        private static ObterAjudasDoSistemaQuery _instance;
        public static ObterAjudasDoSistemaQuery Instance => _instance ??= new();
    }
}