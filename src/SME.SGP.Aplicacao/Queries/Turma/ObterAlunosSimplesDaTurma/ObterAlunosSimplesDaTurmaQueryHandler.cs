using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
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
        
        private readonly IMediator mediator;

        public ObterAlunosSimplesDaTurmaQueryHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<AlunoSimplesDto>> Handle(ObterAlunosSimplesDaTurmaQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var alunosEOL =  await mediator.Send(new ObterAlunosEolPorTurmaQuery(request.TurmaCodigo));
                alunosEOL = alunosEOL.OrderBy(a => a.NomeAluno);
                return MapearParaDto(alunosEOL);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private IEnumerable<AlunoSimplesDto> MapearParaDto(IEnumerable<AlunoPorTurmaResposta> alunosEOL)
        {
            foreach (var alunoEOL in alunosEOL)
            {
                var situacao = alunoEOL.SituacaoMatricula.EhNulo() ? "" : $"({alunoEOL.SituacaoMatricula})";
                yield return new AlunoSimplesDto()
                {
                    Codigo = alunoEOL.CodigoAluno,
                    NumeroChamada = alunoEOL.ObterNumeroAlunoChamada(),
                    Nome = $"{alunoEOL.NomeValido()} {situacao}"
                };
            }
        }
    }
}
