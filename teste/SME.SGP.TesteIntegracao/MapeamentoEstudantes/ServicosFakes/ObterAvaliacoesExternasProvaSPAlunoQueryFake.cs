using MediatR;
using SME.SGP.Aplicacao.Queries;
using SME.SGP.Infra.Dtos.ProvaSP;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.MapeamentoEstudantes.ServicosFakes
{
    public class ObterAvaliacoesExternasProvaSPAlunoQueryFake : IRequestHandler<ObterAvaliacoesExternasProvaSPAlunoQuery, IEnumerable<AvaliacaoExternaProvaSPDto>>
    {
        public ObterAvaliacoesExternasProvaSPAlunoQueryFake()
        {}

        public Task<IEnumerable<AvaliacaoExternaProvaSPDto>> Handle(ObterAvaliacoesExternasProvaSPAlunoQuery request, CancellationToken cancellationToken)
        => Task.Run<IEnumerable<AvaliacaoExternaProvaSPDto>>(
                () => new List<AvaliacaoExternaProvaSPDto>()
                          {
                            new() { AreaConhecimento = "CIENCIAS DA NATUREZA", Proficiencia = 90.5, Nivel = "ABAIXO DO BÁSICO"},
                            new() { AreaConhecimento = "LINGUA PORTUGUES", Proficiencia = 179.5, Nivel = "BÁSICO"}
                          });
           
    }
}
