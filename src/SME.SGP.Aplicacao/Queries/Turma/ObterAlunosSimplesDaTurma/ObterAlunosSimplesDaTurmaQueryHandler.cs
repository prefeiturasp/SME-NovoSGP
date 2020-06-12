using MediatR;
using Sentry;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAlunosSimplesDaTurmaQueryHandler : IRequestHandler<ObterAlunosSimplesDaTurmaQuery, IEnumerable<AlunoSimplesDto>>
    {
        private readonly IServicoEOL servicoEOL;

        public ObterAlunosSimplesDaTurmaQueryHandler(IServicoEOL servicoEOL)
        {
            this.servicoEOL = servicoEOL ?? throw new ArgumentNullException(nameof(servicoEOL));
        }

        public async Task<IEnumerable<AlunoSimplesDto>> Handle(ObterAlunosSimplesDaTurmaQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var alunosEOL = await servicoEOL.ObterAlunosPorTurma(request.TurmaCodigo);
                return MapearParaDto(alunosEOL);
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        private IEnumerable<AlunoSimplesDto> MapearParaDto(IEnumerable<AlunoPorTurmaResposta> alunosEOL)
        {
            foreach(var alunoEOL in alunosEOL)
            {
                yield return new AlunoSimplesDto()
                { 
                    NumeroChamada = alunoEOL.NumeroAlunoChamada,
                    Nome = alunoEOL.NomeAluno
                };
            }
        }
    }
}
