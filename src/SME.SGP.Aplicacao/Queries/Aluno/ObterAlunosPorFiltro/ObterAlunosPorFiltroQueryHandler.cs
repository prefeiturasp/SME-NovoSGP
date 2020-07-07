using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAlunosPorFiltroQueryHandler : IRequestHandler<ObterAlunosPorFiltroQuery, IEnumerable<AlunoSimplesDto>>
    {
        private readonly IServicoEol servicoEOL;

        public ObterAlunosPorFiltroQueryHandler(IServicoEol servicoEOL)
        {
            this.servicoEOL = servicoEOL ?? throw new ArgumentNullException(nameof(servicoEOL));
        }

        public async Task<IEnumerable<AlunoSimplesDto>> Handle(ObterAlunosPorFiltroQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var alunosEOL = await servicoEOL.ObterAlunosPorFiltro(request.CodigoUe, request.AnoLetivo, request.NomeAluno, request.CodigoEol);
                alunosEOL = alunosEOL.OrderBy(a => a.NomeAluno);
                return MapearParaDto(alunosEOL);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private IEnumerable<AlunoSimplesDto> MapearParaDto(IEnumerable<AlunoPorTurmaResposta> alunosEOL)
        {
            foreach (var alunoEOL in alunosEOL)
            {
                yield return new AlunoSimplesDto()
                {
                    Codigo = alunoEOL.CodigoAluno,
                    NumeroChamada = alunoEOL.NumeroAlunoChamada,
                    Nome = alunoEOL.NomeAluno
                };
            }
        }
    }
}
