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
        private readonly IRepositorioConselhoClasseAlunoConsulta repositorioConselhoClasseAluno;
        private readonly IRepositorioConselhoClasseConsulta repositorioConselhoClasse;
        private readonly IMediator mediator;

        public ObterConselhoClasseAlunoPorConselhoFechamentoAlunoCodigoQueryHandler(IRepositorioConselhoClasseAlunoConsulta repositorioConselhoClasseAluno,
                                                                                    IRepositorioConselhoClasseConsulta repositorioConselhoClasse,
                                                                                    IMediator mediator)
        {
            this.repositorioConselhoClasseAluno = repositorioConselhoClasseAluno ?? throw new ArgumentNullException(nameof(repositorioConselhoClasseAluno));
            this.repositorioConselhoClasse = repositorioConselhoClasse ?? throw new ArgumentNullException(nameof(repositorioConselhoClasse));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<ConselhoClasseAluno> Handle(ObterConselhoClasseAlunoPorConselhoFechamentoAlunoCodigoQuery request, CancellationToken cancellationToken)
        {
            ConselhoClasseAluno conselhoClasseAluno = await repositorioConselhoClasseAluno.ObterPorConselhoClasseAlunoCodigoAsync(request.ConselhoClasseId, request.AlunoCodigo);
            if (conselhoClasseAluno == null)
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
