using MediatR;
using SME.SGP.Aplicacao.Queries;
using SME.SGP.Infra.Consts;
using SME.SGP.Infra.Dtos.ProvaSP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.MapeamentoEstudantes.ServicosFakes
{
    public class ObterAvaliacoesExternasProvaSPAlunoQueryFake : IRequestHandler<ObterAvaliacoesExternasProvaSPAlunoQuery, IEnumerable<AvaliacaoExternaProvaSPDto>>
    {
        public ObterAvaliacoesExternasProvaSPAlunoQueryFake()
        {}

        public Task<IEnumerable<AvaliacaoExternaProvaSPDto>> Handle(ObterAvaliacoesExternasProvaSPAlunoQuery request, CancellationToken cancellationToken)
        {
            var retorno = new List<AvaliacaoExternaProvaSPDto>()
                        {
                            new() { AreaConhecimento = "CIENCIAS DA NATUREZA", Proficiencia = 90.5, Nivel = "ABAIXO DO BÁSICO"},
                            new() { AreaConhecimento = "LINGUA PORTUGUES", Proficiencia = 179.5, Nivel = "BÁSICO"}
                        };
            return retorno;
        }
    }
}
