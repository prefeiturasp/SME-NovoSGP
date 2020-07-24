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
    public class ObterAlunosSimplesDaTurmaQueryHandler : IRequestHandler<ObterAlunosSimplesDaTurmaQuery, IEnumerable<AlunoSimplesDto>>
    {
        private readonly IServicoEol servicoEOL;

        public ObterAlunosSimplesDaTurmaQueryHandler(IServicoEol servicoEOL)
        {
            this.servicoEOL = servicoEOL ?? throw new ArgumentNullException(nameof(servicoEOL));
        }

        public async Task<IEnumerable<AlunoSimplesDto>> Handle(ObterAlunosSimplesDaTurmaQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var alunosEOL = await servicoEOL.ObterAlunosPorTurma(request.TurmaCodigo);
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
                var situacao = alunoEOL.SituacaoMatricula == null ? "" : $"({alunoEOL.SituacaoMatricula})";
                yield return new AlunoSimplesDto()
                {
                    Codigo = alunoEOL.CodigoAluno,
                    NumeroChamada = alunoEOL.NumeroAlunoChamada,
                    Nome = $"{alunoEOL.NomeAluno} {situacao}"
                };
            }
        }
    }
}
