using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterFechamentoTurmaPorIdAlunoCodigoQueryHandler : IRequestHandler<ObterFechamentoTurmaPorIdAlunoCodigoQuery, FechamentoTurma>
    {
        private readonly IRepositorioFechamentoTurma repositorioFechamentoTurma;
        private readonly IMediator mediator;

        public ObterFechamentoTurmaPorIdAlunoCodigoQueryHandler(IRepositorioFechamentoTurma repositorioFechamentoTurma, IMediator mediator)
        {
            this.repositorioFechamentoTurma = repositorioFechamentoTurma ?? throw new ArgumentNullException(nameof(repositorioFechamentoTurma));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        public async Task<FechamentoTurma> Handle(ObterFechamentoTurmaPorIdAlunoCodigoQuery request, CancellationToken cancellationToken)
        {
            var fechamentoTurma = await repositorioFechamentoTurma.ObterCompletoPorIdAsync(request.FechamentoTurmaId);
            if (fechamentoTurma == null && !request.EhAnoAnterior)
                throw new NegocioException("Fechamento da turma não localizado");

            //Se for dos tipos 2 e 7, deve utilizar o fechamento da turma do tipo 1.
            //Caso não exista, gerar;
            if (fechamentoTurma != null && fechamentoTurma.Turma.EhTurmaEdFisicaOuItinerario())
            {
                //Obter a turma do tipo 1 do aluno
                var tiposTurma = new List<TipoTurma>() { TipoTurma.Regular };
                var turmasRegularesDoAluno = await mediator.Send(new ObterTurmaCodigosAlunoPorAnoLetivoAlunoTipoTurmaQuery(fechamentoTurma.Turma.AnoLetivo, request.AlunoCodigo, tiposTurma));
                if (turmasRegularesDoAluno == null && !turmasRegularesDoAluno.Any())
                    throw new NegocioException($"Não foi possível obter a turma Regular do aluno {request.AlunoCodigo}");

                var turmaRegularCodigo = turmasRegularesDoAluno.FirstOrDefault();
                var turmaRegularId = await mediator.Send(new ObterTurmaIdPorCodigoQuery(turmaRegularCodigo));
                if (turmaRegularId == 0)
                    throw new NegocioException($"Não foi possível obter a turma Regular do aluno {request.AlunoCodigo} na base do SGP");

                //TODO: Trazer infos completas para não precisar buscar novamente
                var fechamentoDaTurmaRegular = await mediator.Send(new ObterFechamentoPorTurmaPeriodoQuery() { TurmaId = turmaRegularId, PeriodoEscolarId = fechamentoTurma.PeriodoEscolarId ?? 0 });
                if (fechamentoDaTurmaRegular == null)
                {
                    //Gerar fechamento para a turma Regular
                    var fechamentoParaIncluirDto = new IncluirFechamentoDto(turmaRegularId, fechamentoTurma.PeriodoEscolarId);

                    var fechamentoParaUtilizarId = await mediator.Send(new IncluirFechamentoFinalCommand(fechamentoParaIncluirDto));

                    if (fechamentoParaUtilizarId <= 0)
                        throw new NegocioException("Não foi possível gerar o fechamento para a turma Regular.");

                    fechamentoTurma = await repositorioFechamentoTurma.ObterCompletoPorIdAsync(fechamentoParaUtilizarId);

                }
                else fechamentoTurma = await repositorioFechamentoTurma.ObterCompletoPorIdAsync(fechamentoDaTurmaRegular.Id);

            }

            return fechamentoTurma;
        }
    }
}
