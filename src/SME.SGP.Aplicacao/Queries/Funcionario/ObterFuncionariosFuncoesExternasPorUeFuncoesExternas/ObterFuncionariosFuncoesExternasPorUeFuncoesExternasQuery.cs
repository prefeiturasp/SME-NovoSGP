using MediatR;
using SME.SGP.Infra.Dtos;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    internal class ObterFuncionariosFuncoesExternasPorUeFuncoesExternasQuery : IRequest<IEnumerable<FuncionarioFuncaoExternaDTO>>
    {
        public string UeCodigo;
        public IEnumerable<int> FuncoesExternasIds;

        public ObterFuncionariosFuncoesExternasPorUeFuncoesExternasQuery(string ueCodigo, IEnumerable<int> funcoesExternasIds, string dreCodigo)
        {
            this.UeCodigo = ueCodigo;
            this.FuncoesExternasIds = funcoesExternasIds;
            DreCodigo = dreCodigo;
        }

        public string DreCodigo { get; internal set; }
    }
}