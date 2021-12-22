
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPendenciaRegistroIndividualCodigosAlunosPorPendenciaQueryHandler : IRequestHandler<ObterPendenciaRegistroIndividualCodigosAlunosPorPendenciaQuery, IEnumerable<AlunoPorTurmaResposta>>
    {
        private readonly IRepositorioPendenciaRegistroIndividual repositorioPendenciaRegistroIndividual;
        private readonly IRepositorioTurmaConsulta repositorioTurmaConsulta;
        private readonly IMediator mediator;

        public ObterPendenciaRegistroIndividualCodigosAlunosPorPendenciaQueryHandler(IRepositorioPendenciaRegistroIndividual repositorioPendenciaRegistroIndividual,
            IRepositorioTurmaConsulta repositorioTurmaConsulta, IMediator mediator)
        {
            this.repositorioPendenciaRegistroIndividual = repositorioPendenciaRegistroIndividual;
            this.repositorioTurmaConsulta = repositorioTurmaConsulta;
            this.mediator = mediator;
        }

        public async Task<IEnumerable<AlunoPorTurmaResposta>> Handle(ObterPendenciaRegistroIndividualCodigosAlunosPorPendenciaQuery request, CancellationToken cancellationToken)
        {
            var pendenciaRegistroIndividual = await repositorioPendenciaRegistroIndividual.ObterPendenciaRegistroIndividualPorPendenciaESituacao(request.PendenciaId,
                SituacaoPendencia.Pendente, SituacaoPendenciaRegistroIndividualAluno.Pendente);
            if (pendenciaRegistroIndividual is null)
                throw new NegocioException("A pendência por ausência de registro individual não foi encontrada.");

            var turma = await repositorioTurmaConsulta.ObterPorId(pendenciaRegistroIndividual.TurmaId);
            if (turma is null)
                throw new NegocioException("A turma não foi encontrada.");

            var alunosDaTurma = await mediator.Send(new ObterAlunosPorTurmaQuery(turma.CodigoTurma));
            return alunosDaTurma
                .Where(x => pendenciaRegistroIndividual.Alunos.Any(y => y.CodigoAluno.ToString() == x.CodigoAluno))
                .OrderBy(x => x.NomeAluno)
                .ToList();
        }
    }
}