using MediatR;
using Minio.DataModel;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

            var periodoEscolar = await ObterPeriodoEscolarLetivoValido(filtro.TurmaId);
            if (periodoEscolar.EhNulo())
                return false;
            var turmaDreUe = await ObterTurmaDreUe(filtro.TurmaCodigo);
            var componentes = await ObterComponentesCurricularesTurma(filtro.TurmaCodigo);

            if (componentes.Any(cc => cc.Regencia))
                return false;

            return await GerarPendencia(periodoEscolar, turmaDreUe, componentes);
        }

        private async Task<bool> GerarPendencia(PeriodoEscolar periodoEscolar, Turma turmaDreUe, IEnumerable<ComponenteCurricularEol> componentes)
        {
            
            var gerouPendencia = false;
            foreach (var componente in componentes)
            {
                var possuiAulaNoPeriodo = await mediator.Send(new PossuiAulaCadastradaPorPeriodoTurmaDisciplinaQuery(periodoEscolar.PeriodoInicio, periodoEscolar.PeriodoFim, turmaDreUe.CodigoTurma, componente.Codigo.ToString()));
                if (!possuiAulaNoPeriodo)
                {
                    var professorTitular = await mediator.Send(new ObterProfessorTitularPorTurmaEComponenteCurricularQuery(turmaDreUe.CodigoTurma, componente.Codigo.ToString()));
                    if (professorTitular.EhNulo())
                        continue;

                    var possuiPendencia = await mediator.Send(new ExistePendenciaProfessorPorTurmaEComponenteQuery(turmaDreUe.Id, componente.Codigo, 
                                                                                                                   periodoEscolar.Id, professorTitular.ProfessorRf, 
                                                                                                                   TipoPendencia.ComponenteSemAula));
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

        private async Task<IEnumerable<ComponenteCurricularEol>> ObterComponentesCurricularesTurma(string turmaCodigo)
        {
            var turmasCodigo = new string[1] { turmaCodigo };
            return await mediator.Send(new ObterComponentesCurricularesEOLPorTurmasCodigoQuery(turmasCodigo));
        }

        private async Task<Turma> ObterTurmaDreUe(string turmaCodigo)
        {
            var turmasCodigo = new string[1] { turmaCodigo };
            var turmasDreUe = await mediator.Send(new ObterTurmasDreUePorCodigosQuery(turmasCodigo));
            return turmasDreUe.FirstOrDefault();
        }

        private async Task<PeriodoEscolar> ObterPeriodoEscolarLetivoValido(long turmaId)
        {
            var periodoEscolar = await mediator.Send(new ObterPeriodoEscolarAtualQuery(turmaId, DateTimeExtension.HorarioBrasilia()));
            var diasAposInicioPeriodo = await mediator.Send(DiasAposInicioPeriodoLetivoComponenteSemAulaQuery.Instance);
            if (periodoEscolar.NaoEhNulo())
            {
                var periodoInicio = periodoEscolar.PeriodoInicio.AddDays(diasAposInicioPeriodo);
                if (periodoInicio.Date < DateTimeExtension.HorarioBrasilia().Date)
                    return periodoEscolar;
            }
            return null;
        }

        private async Task SalvarPendenciaUsuario(long pendenciaId, string professorRf)
        {
            var usuarioId = await mediator.Send(new ObterUsuarioIdPorRfOuCriaQuery(professorRf));
            await mediator.Send(new SalvarPendenciaUsuarioCommand(pendenciaId, usuarioId));
        }
    }
}
