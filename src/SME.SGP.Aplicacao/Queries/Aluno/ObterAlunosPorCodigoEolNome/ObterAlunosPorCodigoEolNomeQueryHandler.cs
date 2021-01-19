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
                var alunosEOL = await servicoEOL.ObterAlunosPorNomeCodigoEol(request.AnoLetivo, request.CodigoUe, request.CodigoTurma, request.Nome, request.CodigoEOL);

                var alunoSimplesDto = new List<AlunoSimplesDto>();

                foreach (var alunoEOL in alunosEOL.OrderBy(a => a.NomeAluno))
                {
                    var alunoPorTurmaResposta = await servicoEOL.ObterDadosAluno(alunoEOL.CodigoAluno, int.Parse(request.AnoLetivo));

                    var alunoSimples = new AlunoSimplesDto()
                    {
                        Codigo = alunoEOL.CodigoAluno,
                        Nome = alunoEOL.NomeAluno,
                        CodigoTurma = alunoPorTurmaResposta.Where(e => e.CodigoAluno == alunoEOL.CodigoAluno).FirstOrDefault().CodigoTurma.ToString()
                    };
                    alunoSimplesDto.Add(alunoSimples);
                }

                return alunoSimplesDto;
            }
            catch (Exception e)
            {
                throw e;
            }
        }        
    }
}
