using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAlunosAtivosSimplesDaTurmaQueryHandler: IRequestHandler<ObterAlunosAtivosSimplesDaTurmaQuery, IEnumerable<AlunoSituacaoDto>>
    {
        private readonly IServicoEOL servicoEOL;

        public ObterAlunosAtivosSimplesDaTurmaQueryHandler(IServicoEOL servicoEOL)
        {
            this.servicoEOL = servicoEOL ?? throw new ArgumentNullException(nameof(servicoEOL));
        }

        public async Task<IEnumerable<AlunoSituacaoDto>> Handle(ObterAlunosAtivosSimplesDaTurmaQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var situacoesAtivas = new List<SituacaoMatriculaAluno>()
                {
                    SituacaoMatriculaAluno.Ativo,
                    SituacaoMatriculaAluno.PendenteRematricula,
                    SituacaoMatriculaAluno.Rematriculado,
                    SituacaoMatriculaAluno.SemContinuidade,
                    SituacaoMatriculaAluno.Concluido,
                };

                var alunosEOL = await servicoEOL.ObterAlunosPorTurma(request.TurmaCodigo);
                return MapearParaDto(alunosEOL.Where(c => situacoesAtivas.Contains(c.CodigoSituacaoMatricula)));
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private IEnumerable<AlunoSituacaoDto> MapearParaDto(IEnumerable<AlunoPorTurmaResposta> alunosEOL)
        {
            foreach (var alunoEOL in alunosEOL)
            {
                yield return new AlunoSituacaoDto()
                {
                    Codigo = alunoEOL.CodigoAluno,
                    NumeroChamada = alunoEOL.NumeroAlunoChamada,
                    Nome = alunoEOL.NomeAluno,
                    Situacao = alunoEOL.CodigoSituacaoMatricula
                };
            }
        }
    }
}
