using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterListaSemestresRelatorioPAPQueryHandler : IRequestHandler<ObterListaSemestresRelatorioPAPQuery, List<SemestreAcompanhamentoDto>>
    {

        public Task<List<SemestreAcompanhamentoDto>> Handle(ObterListaSemestresRelatorioPAPQuery request, CancellationToken cancellationToken)
        {
            var lista = new List<SemestreAcompanhamentoDto>()
            {
                new SemestreAcompanhamentoDto(1, "Acompanhamento 1º Semestre", request.BimestreAtual == 2),
                new SemestreAcompanhamentoDto(2, "Acompanhamento 2º Semestre", request.BimestreAtual == 4)
            };

            return Task.FromResult(lista);
        }
    }
}
