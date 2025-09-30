using MediatR;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra.Dtos.Frequencia;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.Frequencia.ObterFrequenciaPorLimitePercentual
{
    public class ObterQuantidadeAbaixoFrequenciaPorTurmaQueryHandler : IRequestHandler<ObterQuantidadeAbaixoFrequenciaPorTurmaQuery, IEnumerable<QuantitativoAlunosFrequenciaBaixaPorTurmaDto>>
    {
        private readonly IRepositorioConsolidacaoFrequenciaTurmaConsulta repositorioFrequenciaTurmaConsulta;
        public ObterQuantidadeAbaixoFrequenciaPorTurmaQueryHandler(IRepositorioConsolidacaoFrequenciaTurmaConsulta repositorioFrequenciaTurmaConsulta)
        {
            this.repositorioFrequenciaTurmaConsulta = repositorioFrequenciaTurmaConsulta;
        }
        public async Task<IEnumerable<QuantitativoAlunosFrequenciaBaixaPorTurmaDto>> Handle(ObterQuantidadeAbaixoFrequenciaPorTurmaQuery request, CancellationToken cancellationToken)
        {
            return await repositorioFrequenciaTurmaConsulta.ObterQuantitativoAlunosFrequenciaBaixaPorTurma(request.AnoLetivo, TipoTurma.Programa);
        }
    }
}