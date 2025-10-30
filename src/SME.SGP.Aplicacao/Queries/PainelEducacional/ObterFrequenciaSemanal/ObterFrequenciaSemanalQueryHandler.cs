using MediatR;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra.Dtos.PainelEducacional.Frequencia;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterFrequenciaSemanal
{
    public class ObterFrequenciaSemanalQueryHandler : IRequestHandler<ObterFrequenciaSemanalQuery, IEnumerable<DadosParaConsolidarFrequenciaSemanalAlunoDto>>
    {
        private readonly IRepositorioPainelEducacionalConsolidacaoFrequenciaDiariaConsulta repositorioPainelEducacionalConsolidacaoFrequenciaDiaria;

        public ObterFrequenciaSemanalQueryHandler(IRepositorioPainelEducacionalConsolidacaoFrequenciaDiariaConsulta repositorioPainelEducacionalConsolidacaoFrequenciaDiaria)
        {
            this.repositorioPainelEducacionalConsolidacaoFrequenciaDiaria = repositorioPainelEducacionalConsolidacaoFrequenciaDiaria;
        }

        public async Task<IEnumerable<DadosParaConsolidarFrequenciaSemanalAlunoDto>> Handle(ObterFrequenciaSemanalQuery request, CancellationToken cancellationToken)
        {
           return await repositorioPainelEducacionalConsolidacaoFrequenciaDiaria.ObterFrequenciaSemanal(request.DataAulas);
        }
    }
}
