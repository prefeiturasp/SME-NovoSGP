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
        private const string CODIGO_ALUNO_AVALIACAO_PROVA_SP_ABAIXO_BASICO = "4";
        public ObterAvaliacoesExternasProvaSPAlunoQueryFake()
        {}

        public Task<IEnumerable<AvaliacaoExternaProvaSPDto>> Handle(ObterAvaliacoesExternasProvaSPAlunoQuery request, CancellationToken cancellationToken)
        => Task.Run<IEnumerable<AvaliacaoExternaProvaSPDto>>(
                () => new List<AvaliacaoExternaProvaSPDto>()
                          {
                            new() { AreaConhecimento = "CIENCIAS DA NATUREZA", Proficiencia = 90.5, Nivel = request.AlunoCodigo.Equals(CODIGO_ALUNO_AVALIACAO_PROVA_SP_ABAIXO_BASICO) ? "ABAIXO DO BÁSICO" : "BÁSICO"},
                            new() { AreaConhecimento = "LINGUA PORTUGUES", Proficiencia = 179.5, Nivel = "BÁSICO"}
                          });
           
    }
}
