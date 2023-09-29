using MediatR;
using Minio.DataModel;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasConsideradasNoConselhoQueryHandler : IRequestHandler<ObterTurmasConsideradasNoConselhoQuery, List<string>>
    {
        private readonly IMediator mediator;
        private const string PRIMEIRO_ANO_EM = "1";
        public ObterTurmasConsideradasNoConselhoQueryHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        public async Task<List<string>> Handle(ObterTurmasConsideradasNoConselhoQuery request, CancellationToken cancellationToken)
        {
            List<string> turmasCodigos = new List<string>();
            var turmas = await mediator.Send(new ObterTurmasPorCodigosQuery(request.TurmasCodigos.ToArray()));

            if ((turmas.Select(t => t.TipoTurma).Distinct().Count() == 1 || turmas.Select(t => t.TipoTurma == TipoTurma.Regular).Count() > 1) && request.TurmaSelecionada.ModalidadeCodigo != Modalidade.Medio)
                turmasCodigos = new List<string>() { request.TurmaSelecionada.CodigoTurma };
            else if (ValidaPossibilidadeMatricula2TurmasRegularesNovoEM(turmas, request.TurmaSelecionada))
                turmasCodigos = new List<string>() { request.TurmaSelecionada.CodigoTurma };

            if (request.TurmaSelecionada.ModalidadeCodigo == Modalidade.EJA && turmas.Any(t => t.TipoTurma == TipoTurma.EdFisica))
                turmasCodigos.Add(turmas.FirstOrDefault(t => t.TipoTurma == TipoTurma.EdFisica).CodigoTurma);

            return turmasCodigos.Any() ? turmasCodigos : request.TurmasCodigos.ToList();
        }

        private bool ValidaPossibilidadeMatricula2TurmasRegularesNovoEM(IEnumerable<Turma> turmasAluno, Turma turma)
           => turmasAluno.Select(t => t.TipoTurma).Distinct().Count() == 1 && turma.ModalidadeCodigo == Modalidade.Medio && (turma.AnoLetivo < 2021 || turma.Ano == PRIMEIRO_ANO_EM);
    }
}
