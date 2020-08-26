using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.Aluno.ObterAlunosPorCodigoEolNome
{
    public class ObterAlunosPorCodigoEolNomeQueryHandler : IRequestHandler<ObterAlunosPorCodigoEolNomeQuery, IEnumerable<AlunoSimplesDto>>
    {
        private readonly IServicoEol servicoEOL;

        public ObterAlunosPorCodigoEolNomeQueryHandler(IServicoEol servicoEOL)
        {
            this.servicoEOL = servicoEOL ?? throw new ArgumentNullException(nameof(servicoEOL));
        }

        public async Task<IEnumerable<AlunoSimplesDto>> Handle(ObterAlunosPorCodigoEolNomeQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var alunosEOL = await servicoEOL.ObterAlunosPorNomeCodigoEol(request.AnoLetivo, request.CodigoUe, request.Nome, request.CodigoEOL);
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
                    Nome = alunoEOL.NomeAluno
                };
            }
        }
    }
}
