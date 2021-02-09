using MediatR;
using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries
{
    public class ObterItineranciaAlunoPorIdQueryHandler : IRequestHandler<ObterItineranciaAlunoPorIdQuery, IEnumerable<ItineranciaAlunoDto>>
    {
        private readonly IRepositorioItinerancia repositorioItinerancia;
        private readonly IMediator mediator;


        public ObterItineranciaAlunoPorIdQueryHandler(IRepositorioItinerancia repositorioItinerancia, IMediator mediator)
        {
            this.repositorioItinerancia = repositorioItinerancia ?? throw new ArgumentNullException(nameof(repositorioItinerancia));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<ItineranciaAlunoDto>> Handle(ObterItineranciaAlunoPorIdQuery request, CancellationToken cancellationToken)
        {
            var itineranciaAlunos = await repositorioItinerancia.ObterItineranciaAlunoPorId(request.Id);

            if (itineranciaAlunos == null || itineranciaAlunos.Any())
                throw new NegocioException("Não foi possivel obter a itinerancia do aluno");

            var alunos = new List<ItineranciaAlunoDto>();

            foreach (var itinerancia in itineranciaAlunos)
            {
                var alunoEol = await mediator.Send(new ObterAlunoPorCodigoEAnoQuery(itinerancia.CodigoAluno, DateTime.Now.Year));
                var nomeAluno = string.IsNullOrEmpty(alunoEol.Nome) ? "" : alunoEol.Nome;
                var turma = string.IsNullOrEmpty(alunoEol.TurmaEscola) ? "" : alunoEol.TurmaEscola;
                var itineranciaAluno = new ItineranciaAlunoDto()
                {
                    Id = itinerancia.Id,
                    CodigoAluno = itinerancia.CodigoAluno,
                    Nome = $"{nomeAluno} - {turma}",
                    Questoes = await mediator.Send(new ObterQuestoesItineranciaAlunoPorIdQuery(itinerancia.Id))
                };
                alunos.Add(itineranciaAluno);
            }

            return alunos;
        }
    }
}
