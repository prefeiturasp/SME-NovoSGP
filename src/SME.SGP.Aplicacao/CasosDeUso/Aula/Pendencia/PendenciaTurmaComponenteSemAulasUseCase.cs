using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class PendenciaTurmaComponenteSemAulasUseCase : AbstractUseCase, IPendenciaTurmaComponenteSemAulasUseCase
    {
        private readonly IUnitOfWork unitOfWork;
        public PendenciaTurmaComponenteSemAulasUseCase(IMediator mediator, IUnitOfWork unitOfWork) : base(mediator)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var filtro = param.ObterObjetoMensagem<TurmaDTO>();

            var periodoEscolar = await mediator.Send(new ObterPeriodoEscolarAtualQuery(filtro.TurmaId, DateTimeExtension.HorarioBrasilia()));
            if (periodoEscolar == null)
                return false;

            var diasAposInicioPeriodo = await mediator.Send(new DiasAposInicioPeriodoLetivoComponenteSemAulaQuery());
            var periodoInicio = periodoEscolar.PeriodoInicio.AddDays(diasAposInicioPeriodo);
            if (periodoInicio.Date >= DateTimeExtension.HorarioBrasilia().Date)
                return false;

            var turmasCodigo = new string[1] { filtro.TurmaCodigo };
            var turmasDreUe = await mediator.Send(new ObterTurmasDreUePorCodigosQuery(turmasCodigo));
            var turmaDreUe = turmasDreUe.FirstOrDefault();

            var gerouPendencia = false;
            var componentes = await mediator.Send(new ObterComponentesCurricularesEOLPorTurmasCodigoQuery(turmasCodigo));
            foreach (var componente in componentes)
            {
                if (componente.Regencia)
                    return false;

                var possuiAulaNoPeriodo = await mediator.Send(new PossuiAulaCadastradaPorPeriodoTurmaDisciplinaQuery(periodoEscolar.PeriodoInicio, periodoEscolar.PeriodoFim, turmaDreUe.CodigoTurma, componente.Codigo.ToString()));
                if (!possuiAulaNoPeriodo)
                {
                    var professorTitular = await mediator.Send(new ObterProfessorTitularPorTurmaEComponenteCurricularQuery(turmaDreUe.CodigoTurma, componente.Codigo.ToString()));
                    if (professorTitular == null)
                        continue;

                    //-> TODO caso não tenha o professor titular gerar a pendencia para o CP? 

                    var possuiPendencia = await mediator.Send(new ExistePendenciaProfessorPorTurmaEComponenteQuery(turmaDreUe.Id, componente.Codigo, periodoEscolar.Id, professorTitular.ProfessorRf, TipoPendencia.ComponenteSemAula));
                    if (!possuiPendencia)
                    {
                        try
                        {
                            unitOfWork.IniciarTransacao();

                            var pendenciaId = await mediator.Send(new SalvarPendenciaCommand
                            {
                                TipoPendencia = TipoPendencia.ComponenteSemAula,
                                DescricaoComponenteCurricular = componente.Descricao,
                                TurmaAnoComModalidade = turmaDreUe.AnoComModalidade(),
                                DescricaoUeDre = turmaDreUe.ObterEscola(),
                                TurmaId = turmaDreUe.Id,
                                Titulo = PendenciaConstants.ObterTituloPendenciaComponenteSemAula(componente.Descricao),
                                Descricao = PendenciaConstants.ObterDescricaoPendenciaComponenteSemAula(professorTitular.ProfessorNome, professorTitular.ProfessorRf, periodoEscolar.Bimestre, componente.Descricao, turmaDreUe.NomeComModalidade(), turmaDreUe.ObterEscola())
                            });

                            await mediator.Send(new SalvarPendenciaProfessorCommand(pendenciaId, turmaDreUe.Id, componente.Codigo, professorTitular.ProfessorRf, periodoEscolar.Id));

                            await SalvarPendenciaUsuario(pendenciaId, professorTitular.ProfessorRf);

                            unitOfWork.PersistirTransacao();

                            gerouPendencia = true;
                        }
                        catch
                        {
                            unitOfWork.Rollback();
                            throw;
                        }
                    }
                }
            }

            return gerouPendencia;
        }

        private async Task SalvarPendenciaUsuario(long pendenciaId, string professorRf)
        {
            var usuarioId = await mediator.Send(new ObterUsuarioIdPorRfOuCriaQuery(professorRf));
            await mediator.Send(new SalvarPendenciaUsuarioCommand(pendenciaId, usuarioId));
        }
    }
}
