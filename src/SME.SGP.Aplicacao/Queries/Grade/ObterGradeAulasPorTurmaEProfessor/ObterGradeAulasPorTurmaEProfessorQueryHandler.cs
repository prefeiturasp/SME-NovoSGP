using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Utilitarios;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterGradeAulasPorTurmaEProfessorQueryHandler : IRequestHandler<ObterGradeAulasPorTurmaEProfessorQuery, GradeComponenteTurmaAulasDto>
    {
        private readonly IRepositorioTurma repositorioTurma;
        private readonly IRepositorioAula repositorioAula;
        private readonly IRepositorioGrade repositorioGrade;
        private readonly IServicoUsuario servicoUsuario;
        private readonly IMediator mediator;

        public ObterGradeAulasPorTurmaEProfessorQueryHandler(IRepositorioTurma repositorioTurma,
                                                             IRepositorioAula repositorioAula,
                                                             IRepositorioGrade repositorioGrade,
                                                             IServicoUsuario servicoUsuario,
                                                             IMediator mediator)
        {
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
            this.repositorioAula = repositorioAula ?? throw new ArgumentNullException(nameof(repositorioAula));
            this.repositorioGrade = repositorioGrade ?? throw new ArgumentNullException(nameof(repositorioGrade));
            this.servicoUsuario = servicoUsuario ?? throw new ArgumentNullException(nameof(servicoUsuario));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<GradeComponenteTurmaAulasDto> Handle(ObterGradeAulasPorTurmaEProfessorQuery request, CancellationToken cancellationToken)
        {
            var semana = UtilData.ObterSemanaDoAno(request.DataAula);

            var turma = await repositorioTurma.ObterTurmaComUeEDrePorCodigo(request.TurmaCodigo);
            if (turma == null)
                throw new NegocioException("Turma não localizada.");

            // verifica se é regencia de classe
            var horasGrade = await TratarHorasGrade(request.ComponenteCurricular, turma, request.EhRegencia);
            if (horasGrade == 0)
                return null;

            if (string.IsNullOrEmpty(request.CodigoRf))
            {
                var usuario = await servicoUsuario.ObterUsuarioLogado();
                request.CodigoRf = usuario.CodigoRf;
            }

            var horascadastradas = await ObtenhaHorasCadastradas(request.ComponenteCurricular, semana, request.DataAula, request.CodigoRf, turma, request.EhRegencia);
            var aulasRestantes = horasGrade - horascadastradas;

            return new GradeComponenteTurmaAulasDto
            {
                QuantidadeAulasGrade = horasGrade,
                QuantidadeAulasRestante = aulasRestantes,
                PodeEditar = aulasRestantes > 1 && !(request.EhRegencia && turma.EhEJA())
            };
        }

        private async Task<int> TratarHorasGrade(long componenteCurricularId, Turma turma, bool ehRegencia)
        {
            if (ehRegencia)
                return turma.ObterHorasGradeRegencia();

            if (componenteCurricularId == 1030)
                return 4;

            int ano;
            int.TryParse(turma.Ano, out ano);

            // Busca grade a partir dos dados da abrangencia da turma
            var grade = await repositorioGrade.ObterGradeTurma(turma.Ue.TipoEscola, turma.ModalidadeCodigo, turma.QuantidadeDuracaoAula);
            if (grade == null)
                return 0;

            return await repositorioGrade.ObterHorasComponente(grade.Id, componenteCurricularId, ano);
        }

        private async Task<int> ObtenhaHorasCadastradas(long componenteCurricular, int semana, DateTime dataAula, string codigoRf, Turma turma, bool ehRegencia)
        {
            var ehExperienciaPedagogica = await mediator.Send(new AulaDeExperienciaPedagogicaQuery(componenteCurricular));
            if (ehRegencia)
                return ehExperienciaPedagogica ?
                    await repositorioAula.ObterQuantidadeAulasTurmaExperienciasPedagogicasDia(turma.CodigoTurma, dataAula) :
                    await repositorioAula.ObterQuantidadeAulasTurmaComponenteCurricularDiaProfessor(turma.CodigoTurma, componenteCurricular.ToString(), dataAula, codigoRf);

            // Busca horas aula cadastradas para a disciplina na turma
            return ehExperienciaPedagogica ?
                await repositorioAula.ObterQuantidadeAulasTurmaExperienciasPedagogicasSemana(turma.CodigoTurma, semana) :
                await repositorioAula.ObterQuantidadeAulasTurmaDisciplinaSemanaProfessor(turma.CodigoTurma, componenteCurricular.ToString(), semana, codigoRf, dataAula);
        }
    }
}
