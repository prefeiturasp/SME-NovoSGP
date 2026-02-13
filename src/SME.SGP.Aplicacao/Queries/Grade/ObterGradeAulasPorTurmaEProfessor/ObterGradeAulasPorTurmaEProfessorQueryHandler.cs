using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Utilitarios;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterGradeAulasPorTurmaEProfessorQueryHandler : IRequestHandler<ObterGradeAulasPorTurmaEProfessorQuery, GradeComponenteTurmaAulasDto>
    {
        private readonly IRepositorioTurmaConsulta repositorioTurma;
        private readonly IRepositorioAulaConsulta repositorioAula;
        private readonly IRepositorioGrade repositorioGrade;
        private readonly IMediator mediator;

        public ObterGradeAulasPorTurmaEProfessorQueryHandler(IRepositorioTurmaConsulta repositorioTurma,
                                                             IRepositorioAulaConsulta repositorioAula,
                                                             IRepositorioGrade repositorioGrade,
                                                             IMediator mediator)
        {
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
            this.repositorioAula = repositorioAula ?? throw new ArgumentNullException(nameof(repositorioAula));
            this.repositorioGrade = repositorioGrade ?? throw new ArgumentNullException(nameof(repositorioGrade));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<GradeComponenteTurmaAulasDto> Handle(ObterGradeAulasPorTurmaEProfessorQuery request, CancellationToken cancellationToken)
        {
            var semana = UtilData.ObterSemanaDoAnoISO(request.DataAula);

            var turma = await repositorioTurma.ObterTurmaComUeEDrePorCodigo(request.TurmaCodigo);
            if (turma.EhNulo())
                throw new NegocioException("Turma não localizada.");

            // verifica se é regencia de classe
            var horasGrade = await TratarHorasGrade(request.ComponentesCurriculares, turma, request.EhRegencia);
            if (horasGrade == 0)
                return null;

            if (string.IsNullOrEmpty(request.CodigoRf))
            {
                var usuario = await mediator.Send(ObterUsuarioLogadoQuery.Instance, cancellationToken);
                request.CodigoRf = usuario.CodigoRf;
                request.EhGestor = usuario.EhGestorEscolar();
            }

            var horasCadastradas = await ObtenhaHorasCadastradas(request.ComponentesCurriculares, semana, request.DataAula, request.CodigoRf, turma, request.EhRegencia, request.EhGestor);
            var aulasRestantes = horasCadastradas > horasGrade ? 0 : (horasGrade - horasCadastradas);

            return new GradeComponenteTurmaAulasDto
            {
                QuantidadeAulasGrade = horasGrade,
                QuantidadeAulasRestante = aulasRestantes,
                PodeEditar = aulasRestantes > 1 && !(request.EhRegencia && turma.EhEJA())
            };
        }

        private async Task<int> TratarHorasGrade(long[] componentesCurricularesId, Turma turma, bool ehRegencia)
        {
            if (ehRegencia)
                return turma.ObterHorasGradeRegencia();

            if (componentesCurricularesId.Any(c => c == 1030))
                return 4;

            int.TryParse(turma.Ano, out int ano);

            // Busca grade a partir dos dados da abrangencia da turma
            var grade = await mediator.Send(new ObterGradePorTipoEscolaModalidadeDuracaoAnoQuery(turma.Ue.TipoEscola, turma.ModalidadeCodigo, turma.QuantidadeDuracaoAula, ano, turma.AnoLetivo.ToString()));
            if (grade.EhNulo())
                return 0;

            return await repositorioGrade.ObterHorasComponente(grade.Id, componentesCurricularesId, ano);
        }

        private async Task<int> ObtenhaHorasCadastradas(long[] componentesCurriculares, int semana, DateTime dataAula, string codigoRf, Turma turma, bool ehRegencia, bool ehGestor)
        {
            var ehExperienciaPedagogica = false;

            foreach (var componente in componentesCurriculares)
            {
                ehExperienciaPedagogica = await mediator.Send(new AulaDeExperienciaPedagogicaQuery(componente));

                if (ehExperienciaPedagogica)
                    break;
            }

            if (ehRegencia)
                return ehExperienciaPedagogica ?
                    await repositorioAula.ObterQuantidadeAulasTurmaExperienciasPedagogicasDia(turma.CodigoTurma, dataAula) :
                    await repositorioAula.ObterQuantidadeAulasTurmaComponenteCurricularDiaProfessor(turma.CodigoTurma, componentesCurriculares.Select(c => c.ToString()).ToArray(), dataAula, codigoRf, ehGestor);

            // Busca horas aula cadastradas para a disciplina na turma
            return ehExperienciaPedagogica ?
                await repositorioAula.ObterQuantidadeAulasTurmaExperienciasPedagogicasSemana(turma.CodigoTurma, semana, componentesCurriculares.Select(c => c.ToString()).ToArray()) :
                await repositorioAula.ObterQuantidadeAulasTurmaDisciplinaSemanaProfessor(turma.CodigoTurma, componentesCurriculares.Select(c => c.ToString()).ToArray(), semana, codigoRf, dataAula, ehGestor);
        }
    }
}
