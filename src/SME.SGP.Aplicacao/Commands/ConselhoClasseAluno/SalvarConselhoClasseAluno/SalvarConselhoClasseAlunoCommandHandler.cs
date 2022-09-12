using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class SalvarConselhoClasseAlunoCommandHandler : IRequestHandler<SalvarConselhoClasseAlunoCommand, long>
    {
        private readonly IRepositorioConselhoClasseAluno repositorioConselhoClasseAluno;
        private readonly IMediator mediator;
        private readonly IRepositorioConselhoClasse repositorioConselhoClasse;
        private readonly IConsultasPeriodoFechamento consultasPeriodoFechamento;
        private readonly IConsultasConselhoClasse consultasConselhoClasse;

        public SalvarConselhoClasseAlunoCommandHandler(IRepositorioConselhoClasseAluno repositorioConselhoClasseAluno,IMediator mediator,IRepositorioConselhoClasse repositorioConselhoClasse,
            IConsultasPeriodoFechamento consultasPeriodoFechamento,IConsultasConselhoClasse consultasConselhoClasse)
        {
            this.repositorioConselhoClasseAluno = repositorioConselhoClasseAluno ?? throw new ArgumentNullException(nameof(repositorioConselhoClasseAluno));
            this.repositorioConselhoClasse = repositorioConselhoClasse ?? throw new ArgumentNullException(nameof(repositorioConselhoClasse));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.consultasPeriodoFechamento = consultasPeriodoFechamento ?? throw new ArgumentNullException(nameof(consultasPeriodoFechamento));
            this.consultasConselhoClasse = consultasConselhoClasse ?? throw new ArgumentNullException(nameof(consultasConselhoClasse));
        }

        public async Task<long> Handle(SalvarConselhoClasseAlunoCommand request,CancellationToken cancellationToken)
        {
            var fechamentoTurma = await mediator
                .Send(new ObterFechamentoTurmaPorIdAlunoCodigoQuery(request.ConselhoClasseAluno.ConselhoClasse.FechamentoTurmaId, request.ConselhoClasseAluno.AlunoCodigo));

            /*var alunoPossuiNotasTodosComponentesCurriculares = await mediator.Send(new VerificaNotasTodosComponentesCurricularesAlunoQuery(request.ConselhoClasseAluno.AlunoCodigo, 
                                                                                   fechamentoTurma.Turma, fechamentoTurma.PeriodoEscolar.Bimestre));
            if (!alunoPossuiNotasTodosComponentesCurriculares)
                throw new NegocioException("É necessário que todos os componentes tenham nota/conceito informados!");*/

            // Se não existir conselho de classe para o fechamento gera
            if (request.ConselhoClasseAluno.ConselhoClasse.Id == 0)
            {
                await GerarConselhoClasse(request.ConselhoClasseAluno.ConselhoClasse, fechamentoTurma);
                request.ConselhoClasseAluno.ConselhoClasseId = request.ConselhoClasseAluno.ConselhoClasse.Id;
            }
            else
                await repositorioConselhoClasse.SalvarAsync(request.ConselhoClasseAluno.ConselhoClasse);

            var conselhoClasseAlunoId = await repositorioConselhoClasseAluno.SalvarAsync(request.ConselhoClasseAluno);

            await mediator.Send(new InserirTurmasComplementaresCommand(fechamentoTurma.TurmaId, conselhoClasseAlunoId, request.ConselhoClasseAluno.AlunoCodigo));

            return conselhoClasseAlunoId;
        }
        public async Task<AuditoriaDto> GerarConselhoClasse(ConselhoClasse conselhoClasse, FechamentoTurma fechamentoTurma)
        {
            var conselhoClasseExistente = await mediator.Send(new ObterConselhoClassePorTurmaEPeriodoQuery(fechamentoTurma.TurmaId, fechamentoTurma.PeriodoEscolarId));

            if (conselhoClasseExistente != null)
                throw new NegocioException($"Já existe um conselho de classe gerado para a turma {fechamentoTurma.Turma.Nome}!");

            if (fechamentoTurma.PeriodoEscolarId.HasValue)
            {
                // Fechamento Bimestral
                if (!await consultasPeriodoFechamento.TurmaEmPeriodoDeFechamento(fechamentoTurma.Turma, DateTime.Today, fechamentoTurma.PeriodoEscolar.Bimestre))
                    throw new NegocioException($"Turma {fechamentoTurma.Turma.Nome} não esta em período de fechamento para o {fechamentoTurma.PeriodoEscolar.Bimestre}º Bimestre!");
            }
            else
            {
                // Fechamento Final
                if (fechamentoTurma.Turma.AnoLetivo >= DateTime.Now.Year)
                {
                    var validacaoConselhoFinal = await consultasConselhoClasse.ValidaConselhoClasseUltimoBimestre(fechamentoTurma.Turma);
                    if (!validacaoConselhoFinal.Item2)
                        throw new NegocioException($"Para acessar este aba você precisa registrar o conselho de classe do {validacaoConselhoFinal.Item1}º bimestre");
                }
            }

            await repositorioConselhoClasse.SalvarAsync(conselhoClasse);
            return (AuditoriaDto)conselhoClasse;
        }
        private async Task<IEnumerable<DisciplinaDto>> ObterComponentesTurmas(string[] turmasCodigo, bool ehEnsinoEspecial, int turnoParaComponentesCurriculares)
        {
            var componentesTurma = new List<DisciplinaDto>();
            Usuario usuarioAtual = await mediator.Send(new ObterUsuarioLogadoQuery());

            var componentesCurriculares = await mediator.Send(new ObterComponentesCurricularesPorTurmasCodigoQuery(turmasCodigo, usuarioAtual.PerfilAtual, usuarioAtual.Login, ehEnsinoEspecial, turnoParaComponentesCurriculares));
            if (componentesCurriculares != null && componentesCurriculares.Any())
                componentesTurma.AddRange(componentesCurriculares);
            else throw new NegocioException("Não localizado disciplinas para a turma no EOL!");

            return componentesTurma;
        }
        
    }
}
