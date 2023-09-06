using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class PodeCadastrarAulaNoDiaQueryHandler : IRequestHandler<PodeCadastrarAulaNoDiaQuery, bool>
    {
        private readonly IMediator mediator;

        public PodeCadastrarAulaNoDiaQueryHandler(IRepositorioAula repositorioAula, IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(PodeCadastrarAulaNoDiaQuery request, CancellationToken cancellationToken)
        {
            return !await ExisteAula(request) 
                && await PermiteTipoAula(request);
        }

        private async Task<bool> PermiteTipoAula(PodeCadastrarAulaNoDiaQuery request)
        {
            var turma = await mediator.Send(new ObterTurmaPorCodigoQuery(request.TurmaCodigo));
            var tipoCalendarioId = await mediator.Send(new ObterTipoCalendarioIdPorTurmaQuery(turma));

            return await PermiteAulaNormal(request, turma, tipoCalendarioId)
                || await PermiteAulaReposicao(request, turma, tipoCalendarioId);
        }

        private async Task<bool> PermiteAulaReposicao(PodeCadastrarAulaNoDiaQuery request, Turma turma, long tipoCalendarioId)
        {
            return request.TipoAula == TipoAula.Reposicao
                && (await PodeCadastrarAulaNoDia(request, turma, tipoCalendarioId)
                || await ExisteEventoDeReposicao(request, turma, tipoCalendarioId));
        }

        private async Task<bool> ExisteEventoDeReposicao(PodeCadastrarAulaNoDiaQuery request, Turma turma, long tipoCalendarioId)
        {
            var dreUe = await mediator.Send(new ObterCodigosDreUePorTurmaIdQuery(turma.Id));

            return await mediator.Send(new ExisteEventoNaDataPorTipoDreUEQuery(request.DataAula, tipoCalendarioId, TipoEvento.ReposicaoDeAula, dreUe.DreCodigo, dreUe.UeCodigo)) ||
                   await mediator.Send(new ExisteEventoNaDataPorTipoDreUEQuery(request.DataAula, tipoCalendarioId, TipoEvento.ReposicaoDoDia, dreUe.DreCodigo, dreUe.UeCodigo));
        }

        private async Task<bool> PermiteAulaNormal(PodeCadastrarAulaNoDiaQuery request, Turma turma, long tipoCalendarioId)
        {
            return request.TipoAula == TipoAula.Normal
                && await PodeCadastrarAulaNoDia(request, turma, tipoCalendarioId);
        }

        private async Task<bool> PodeCadastrarAulaNoDia(PodeCadastrarAulaNoDiaQuery request, Turma turma, long tipoCalendarioId)
        {
            var dreUe = await mediator.Send(new ObterCodigosDreUePorTurmaQuery(turma.CodigoTurma));

            return await mediator.Send(new ValidarSeEhDiaLetivoQuery(
                request.DataAula,
                tipoCalendarioId,
                dreUe.DreCodigo,
                dreUe.UeCodigo));
        }

        private async Task<bool> ExisteAula(PodeCadastrarAulaNoDiaQuery request)
        {
            var existeAula =  await mediator.Send(new ExisteAulaNaDataTurmaDisciplinaProfessorRfQuery(
                                                    request.DataAula,
                                                    request.TurmaCodigo,
                                                    request.ComponentesCurriculares.Select(c => c.ToString()).ToArray(),
                                                    request.ProfessorRf,
                                                    request.TipoAula));

            if (existeAula)
            {
                var perfilAtual = await mediator.Send(ObterPerfilAtualQuery.Instance);
                var aula = await mediator
                    .Send(new ObterAulasPorDataTurmaComponenteCurricularEProfessorQuery(request.DataAula, request.TurmaCodigo, request.ComponentesCurriculares, request.ProfessorRf));

                if (aula.Any() || perfilAtual != Guid.Empty)
                {
                    if ((perfilAtual.ToString() == PerfilUsuario.CJ.Name() || perfilAtual.ToString() == PerfilUsuario.CJ_INFANTIL.Name()) && !aula.Any(a => a.AulaCJ))
                        return false;
                }

                return true;
            }

            return false;
        }
            
    }
}
