using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterConselhoClasseAlunoPorConselhoFechamentoAlunoCodigoQueryHandler : IRequestHandler<ObterConselhoClasseAlunoPorConselhoFechamentoAlunoCodigoQuery, ConselhoClasseAluno>
    {
        private readonly IRepositorioConselhoClasseAluno repositorioConselhoClasseAluno;
        private readonly IRepositorioConselhoClasse repositorioConselhoClasse;
        private readonly IRepositorioConselhoClasseAlunoConsulta repositorioConselhoClasseAlunoConsulta;
        private readonly IMediator mediator;

        public ObterConselhoClasseAlunoPorConselhoFechamentoAlunoCodigoQueryHandler(
            IRepositorioConselhoClasseAluno repositorioConselhoClasseAluno,
            IRepositorioConselhoClasse repositorioConselhoClasse,
            IRepositorioConselhoClasseAlunoConsulta repositorioConselhoClasseAlunoConsulta,
            IMediator mediator)
        {
            this.repositorioConselhoClasseAluno = repositorioConselhoClasseAluno ?? throw new ArgumentNullException(nameof(repositorioConselhoClasseAluno));
            this.repositorioConselhoClasse = repositorioConselhoClasse ?? throw new ArgumentNullException(nameof(repositorioConselhoClasse));
            this.repositorioConselhoClasseAlunoConsulta = repositorioConselhoClasseAlunoConsulta ?? throw new ArgumentNullException(nameof(repositorioConselhoClasseAlunoConsulta));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<ConselhoClasseAluno> Handle(ObterConselhoClasseAlunoPorConselhoFechamentoAlunoCodigoQuery request, CancellationToken cancellationToken)
        {
            ConselhoClasseAluno conselhoClasseAluno = await repositorioConselhoClasseAlunoConsulta.ObterPorConselhoClasseAlunoCodigoAsync(request.ConselhoClasseId, request.AlunoCodigo);
            if (conselhoClasseAluno.EhNulo())
            {
                ConselhoClasse conselhoClasse = null;
                if (request.ConselhoClasseId == 0)
                {
                    conselhoClasse = new ConselhoClasse() { FechamentoTurmaId = request.FechamentoTurmaId };
                    await repositorioConselhoClasse.SalvarAsync(conselhoClasse);
                }
                else
                    conselhoClasse = repositorioConselhoClasse.ObterPorId(request.ConselhoClasseId);

                conselhoClasseAluno = new ConselhoClasseAluno() { AlunoCodigo = request.AlunoCodigo, ConselhoClasse = conselhoClasse, ConselhoClasseId = conselhoClasse.Id };
                await repositorioConselhoClasseAluno.SalvarAsync(conselhoClasseAluno);
            }
            conselhoClasseAluno.ConselhoClasse.FechamentoTurma = await mediator.Send(new ObterFechamentoTurmaPorIdAlunoCodigoQuery(request.FechamentoTurmaId , request.AlunoCodigo));

            return conselhoClasseAluno;
        }
    }
}
