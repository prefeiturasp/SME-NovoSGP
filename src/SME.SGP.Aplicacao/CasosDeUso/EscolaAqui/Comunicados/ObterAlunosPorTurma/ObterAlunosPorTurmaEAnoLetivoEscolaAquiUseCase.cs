using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.EscolaAqui;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAlunosPorTurmaEAnoLetivoEscolaAquiUseCase : IObterAlunosPorTurmaEAnoLetivoEscolaAquiUseCase
    {
        private readonly IMediator mediator;

        public ObterAlunosPorTurmaEAnoLetivoEscolaAquiUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<AlunoPorTurmaResposta>> Executar(string codigoTurma, int anoLetivo)
        {
            var alunos = await mediator.Send(new ObterAlunosPorTurmaEAnoLetivoQuery(codigoTurma));

            if (alunos.Any())
                return alunos.Where(a => a.CodigoSituacaoMatricula == Dominio.SituacaoMatriculaAluno.Ativo || 
                a.CodigoSituacaoMatricula == Dominio.SituacaoMatriculaAluno.Concluido ||
                a.CodigoSituacaoMatricula == Dominio.SituacaoMatriculaAluno.PendenteRematricula ||
                a.CodigoSituacaoMatricula == Dominio.SituacaoMatriculaAluno.Rematriculado ||
                a.CodigoSituacaoMatricula == Dominio.SituacaoMatriculaAluno.SemContinuidade ||
                a.CodigoSituacaoMatricula == Dominio.SituacaoMatriculaAluno.ReclassificadoSaida);

            return alunos;
        }
    }
}
