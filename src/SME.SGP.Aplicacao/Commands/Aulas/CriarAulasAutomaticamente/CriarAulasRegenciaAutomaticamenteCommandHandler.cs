
using MediatR;
using Sentry;
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
    public class CriarAulasRegenciaAutomaticamenteCommandHandler : IRequestHandler<CriarAulasRegenciaAutomaticamenteCommand, bool>
    {
        private readonly IRepositorioAula repositorioAula;
        private readonly IRepositorioFrequencia repositorioFrequencia;
        private readonly IRepositorioPlanoAula repositorioPlanoAula;
        private readonly IMediator mediator;

        public CriarAulasRegenciaAutomaticamenteCommandHandler(IRepositorioAula repositorioAula,
                                                               IRepositorioFrequencia repositorioFrequencia,
                                                               IRepositorioPlanoAula repositorioPlanoAula,
                                                               IMediator mediator)
        {
            this.repositorioAula = repositorioAula ?? throw new ArgumentNullException(nameof(repositorioAula));
            this.repositorioFrequencia = repositorioFrequencia ?? throw new ArgumentNullException(nameof(repositorioFrequencia));
            this.repositorioPlanoAula = repositorioPlanoAula ?? throw new ArgumentNullException(nameof(repositorioPlanoAula));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(CriarAulasRegenciaAutomaticamenteCommand request, CancellationToken cancellationToken)
        {
            var tipoCalendarioId = request.TipoCalendarioId;
            var diasParaCriarAula = request.DiasLetivos;
            var diasForaDoPeriodo = request.DiasForaDoPeriodoEscolar;
            var aulasACriar = new List<Aula>();
            var aulasAExcluirComFrequenciaRegistrada = new List<(long id, DateTime data)>();
            var dadosTurmas = request.DadosTurmas;
            var ueCodigo = request.UeCodigo;
            var modalidade = request.Modalidade;

            var contadorAulasCriadas = 0;
            var contadorAulasExcluidas = 0;

            var idsAulasAExcluir = new List<long>();

            foreach (var dadoTurma in dadosTurmas)
            {
                var aulas = (List<Aula>)await mediator
                    .Send(new ObterAulasDaTurmaPorTipoCalendarioQuery(dadoTurma.TurmaCodigo, tipoCalendarioId, "Sistema"));

                var aulasCriadasPorUsuarios = mediator
                    .Send(new ObterAulasDaTurmaPorTipoCalendarioQuery(dadoTurma.TurmaCodigo, tipoCalendarioId)).Result
                    .Where(a => !a.CriadoPor.Equals("Sistema", StringComparison.InvariantCultureIgnoreCase));

                var idsDisciplinas = aulasCriadasPorUsuarios?.Select(a => Convert.ToInt64(a.DisciplinaId));

                if (idsDisciplinas == null || !idsDisciplinas.Any())
                    idsDisciplinas = aulas.Select(a => Convert.ToInt64(a.DisciplinaId));

                var componentesCurricularesAulas = await mediator
                    .Send(new ObterDisciplinasPorIdsQuery(idsDisciplinas.Distinct().ToArray()));

                var datasDesconsideradas = (from a in aulasCriadasPorUsuarios
                                            join cc in componentesCurricularesAulas
                                            on a.DisciplinaId equals cc.CodigoComponenteCurricular.ToString()
                                            where cc.Regencia
                                            select a.DataAula);

                

                var professorTitular = await mediator
                    .Send(new ObterProfessorTitularPorTurmaEComponenteCurricularQuery(dadoTurma.TurmaCodigo, dadoTurma.ComponenteCurricularCodigo));

                var professorRf = professorTitular != null ? professorTitular.ProfessorRf : "";

                var aulasCriarComDataInicio = diasParaCriarAula
                    .Where(c => dadoTurma.DataInicioTurma != null && c.Data.Date >= dadoTurma.DataInicioTurma)?.ToList();

                if (aulas == null)
                    aulasACriar.AddRange(ObterAulasParaCriacao(tipoCalendarioId, aulasCriarComDataInicio, dadoTurma, ueCodigo, modalidade, professorRf, datasDesconsideradas));
                else
                {
                    if (!aulas.Any())
                        aulasACriar.AddRange(ObterAulasParaCriacao(tipoCalendarioId, aulasCriarComDataInicio, dadoTurma, ueCodigo, modalidade, professorRf, datasDesconsideradas));
                    else
                    {
                        var diasLetivos = DeterminaDiasLetivos(diasParaCriarAula, request.UeCodigo);
                        var diasSemAula = diasLetivos
                            .Where(c => !aulas.Any(a => a.DataAula == c.Data) && (dadoTurma.DataInicioTurma != null && c.Data.Date >= dadoTurma.DataInicioTurma))?
                            .OrderBy(a => a.Data)?
                            .Distinct()
                            .ToList();

                        var aulasParaCriacao = ObterAulasParaCriacao(tipoCalendarioId, diasSemAula, dadoTurma, ueCodigo, modalidade, professorRf, datasDesconsideradas)?.ToList();

                        if (aulasParaCriacao != null)
                        {
                            for (int a = 0; a < aulasParaCriacao.Count; a++)
                                aulasACriar.Add(aulasParaCriacao[a]);
                        }

                        IEnumerable<Aula> aulasDaTurmaParaExcluir = ObterAulasParaExcluir(diasParaCriarAula.ToList(), dadoTurma, aulas, request.UeCodigo, datasDesconsideradas);
                        await ExcluirAulas(aulasAExcluirComFrequenciaRegistrada, idsAulasAExcluir, aulasDaTurmaParaExcluir.ToList());

                        var aulasForaDoPeriodo = aulas.Where(c => diasForaDoPeriodo.Contains(c.DataAula));
                        if (aulasForaDoPeriodo != null && aulasForaDoPeriodo.Any())
                            await ExcluirAulas(aulasAExcluirComFrequenciaRegistrada, idsAulasAExcluir, aulasForaDoPeriodo.ToList());
                    }
                }

                if (idsAulasAExcluir.Count >= 1000)
                    contadorAulasExcluidas = await ExcluirAulas(contadorAulasExcluidas, idsAulasAExcluir);

                if (aulasAExcluirComFrequenciaRegistrada.Any())
                {
                    await ExcluirFrequenciaRepetida(aulasAExcluirComFrequenciaRegistrada, aulasCriadasPorUsuarios, componentesCurricularesAulas);

                    var turma = await mediator.Send(new ObterTurmaComUeEDrePorCodigoQuery(dadoTurma.TurmaCodigo));

                    await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaNotificacaoExclusaoAulasComFrequencia,
                        new NotificarExclusaoAulasComFrequenciaDto(turma, aulasAExcluirComFrequenciaRegistrada.Select(ae => ae.data)), Guid.NewGuid(), null));

                    aulasAExcluirComFrequenciaRegistrada.Clear();
                }

                if (aulasACriar.Count >= 1000)
                    contadorAulasCriadas = CriarAulas(aulasACriar, contadorAulasCriadas);
            }

            if (aulasACriar.Any())
                contadorAulasCriadas = CriarAulas(aulasACriar, contadorAulasCriadas);

            if (idsAulasAExcluir.Any())
                contadorAulasExcluidas = await ExcluirAulas(contadorAulasExcluidas, idsAulasAExcluir);

            SentrySdk.AddBreadcrumb($"Foram excluídas {contadorAulasExcluidas} aulas.");
            SentrySdk.AddBreadcrumb($"Foram criadas {contadorAulasCriadas} aulas.");
            SentrySdk.CaptureMessage($"{DateTime.Now:dd/MM/yyyy HH:mm:ss} - Finalizada Rotina de manutenção de aulas do Infantil");
            return true;
        }

        private async Task ExcluirFrequenciaRepetida(IList<(long id, DateTime data)> aulasAExcluirComFrequenciaRegistrada, IEnumerable<Aula> aulasCriadasPorUsuarios, IEnumerable<DisciplinaDto> componentesCurriculares)
        {
            var idsAulasExclusaoLogica = new List<long>();
            var aulasEquivalentes = (from a in aulasCriadasPorUsuarios
                                     join ae in aulasAExcluirComFrequenciaRegistrada
                                     on a.DataAula.Date equals ae.data.Date
                                     join cc in componentesCurriculares
                                     on a.DisciplinaId equals cc.CodigoComponenteCurricular.ToString()
                                     where cc.Regencia
                                     select new
                                     {
                                         aulaCriadaPorUsuario = a,
                                         aulaComFrequenciaEquivalente = ae
                                     });

            foreach (var aulaComAjusteFrequencia in aulasEquivalentes)
            {
                var frequenciaAulaUsuario = await mediator
                    .Send(new ObterRegistroFrequenciaPorAulaIdQuery(aulaComAjusteFrequencia.aulaCriadaPorUsuario.Id));

                if (frequenciaAulaUsuario == null)
                {
                    var frequencia = await mediator
                        .Send(new ObterRegistroFrequenciaPorAulaIdQuery(aulaComAjusteFrequencia.aulaComFrequenciaEquivalente.id));
                    frequencia.AulaId = aulaComAjusteFrequencia.aulaCriadaPorUsuario.Id;
                    await repositorioFrequencia.SalvarAsync(frequencia);

                    var planoAulaCriadaPeloUsuario = await mediator.Send(new ObterPlanoAulaPorAulaIdQuery(aulaComAjusteFrequencia.aulaCriadaPorUsuario.Id));
                    var planoAulaSistema = await mediator.Send(new ObterPlanoAulaPorAulaIdQuery(aulaComAjusteFrequencia.aulaComFrequenciaEquivalente.id));

                    if (planoAulaCriadaPeloUsuario == null && planoAulaSistema != null)
                    {
                        planoAulaSistema.AulaId = aulaComAjusteFrequencia.aulaCriadaPorUsuario.Id;
                        await repositorioPlanoAula.SalvarAsync(planoAulaSistema);
                    }
                }
                else
                    await repositorioFrequencia.ExcluirFrequenciaAula(aulaComAjusteFrequencia.aulaComFrequenciaEquivalente.id);

                await repositorioAula.ExcluirPeloSistemaAsync(new long[] { aulaComAjusteFrequencia.aulaComFrequenciaEquivalente.id });
                aulasAExcluirComFrequenciaRegistrada.Remove(aulaComAjusteFrequencia.aulaComFrequenciaEquivalente);
            }
        }

        private async Task ExcluirAulas(List<(long, DateTime)> aulasAExcluirComFrequenciaRegistrada, List<long> idsAulasAExcluir, List<Aula> aulasDaTurmaParaExcluir)
        {
            if (aulasDaTurmaParaExcluir != null)
            {
                foreach (var aula in aulasDaTurmaParaExcluir)
                {
                    var existeFrequencia = await mediator.Send(new ObterAulaPossuiFrequenciaQuery(aula.Id));
                    if (existeFrequencia)
                        aulasAExcluirComFrequenciaRegistrada.Add((aula.Id, aula.DataAula));
                    else
                        idsAulasAExcluir.Add(aula.Id);
                }
            }
        }

        private int CriarAulas(List<Aula> aulasACriar, int contadorAulasCriadas)
        {
            repositorioAula.SalvarVarias(aulasACriar);
            contadorAulasCriadas = contadorAulasCriadas + aulasACriar.Count;
            aulasACriar.Clear();
            return contadorAulasCriadas;
        }

        private async Task<int> ExcluirAulas(int contadorAulasExcluidas, List<long> idsAulasAExcluir)
        {
            await repositorioAula.ExcluirPeloSistemaAsync(idsAulasAExcluir.ToArray());
            contadorAulasExcluidas = contadorAulasExcluidas + idsAulasAExcluir.Count;
            idsAulasAExcluir.Clear();
            return contadorAulasExcluidas;
        }
        private IEnumerable<Aula> ObterAulasParaExcluir(IEnumerable<DiaLetivoDto> diasDoPeriodo, DadosTurmaAulasAutomaticaDto turma, IEnumerable<Aula> aulas, string ueCodigo, IEnumerable<DateTime> datasDesconsideradas)
        {
            var diasLetivos = DeterminaDiasLetivos(diasDoPeriodo, ueCodigo);
            var diasNaoLetivos = DeterminaDiasNaoLetivos(diasDoPeriodo, ueCodigo);

            var diasParaExcluir = diasDoPeriodo.Where(l => (diasLetivos != null && !diasLetivos.Any(n => n.Data == l.Data) &&
                                                            diasNaoLetivos != null && diasNaoLetivos.Any(n => n.Data == l.Data)) ||
                                                            datasDesconsideradas.Contains(l.Data))?
                                               .OrderBy(c => c.Data)?
                                               .ToList();

            var aulasExcluir = new List<Aula>();

            aulasExcluir.AddRange(aulas.Where(c => (diasParaExcluir != null && diasParaExcluir.Any(a => a.Data == c.DataAula && !c.Excluido)) ||
                                                   (turma.DataInicioTurma.HasValue && c.DataAula.Date < turma.DataInicioTurma.Value.Date) ||
                                                   datasDesconsideradas.Contains(c.DataAula)));

            var aulasDuplicadas = aulas
                .OrderBy(a => a.Id)
                .GroupBy(a => a.DataAula)
                .Where(a => a.Count() > 1);

            var aulasMantidas = aulasDuplicadas
                .Select(a => a.First())
                .ToList();

            aulasExcluir.AddRange(aulasDuplicadas
                .SelectMany(a => a)
                .Except(aulasMantidas));

            return aulasExcluir
                .OrderBy(c => c.DataAula);
        }

        private IEnumerable<Aula> ObterAulasParaCriacao(long tipoCalendarioId, IEnumerable<DiaLetivoDto> diasDoPeriodo, DadosTurmaAulasAutomaticaDto turma, string ueCodigo, Modalidade modalidade, string rfProfessor, IEnumerable<DateTime> datasDesconsideradas)
        {
            var diasLetivos = DeterminaDiasLetivos(diasDoPeriodo, ueCodigo);
            var diasNaoLetivos = DeterminaDiasNaoLetivos(diasDoPeriodo, ueCodigo);

            var diasParaCriar = diasDoPeriodo
                .Where(l => (diasLetivos != null && diasLetivos.Any(n => n.Data == l.Data) ||
                             diasNaoLetivos == null || !diasNaoLetivos.Any(n => n.Data == l.Data)) &&
                             !datasDesconsideradas.Contains(l.Data))?
                .ToList();

            return ObterListaDeAulas(diasParaCriar?.DistinctBy(c => c.Data)?.ToList(), tipoCalendarioId, turma, ueCodigo, modalidade, rfProfessor);
        }

        private IList<DiaLetivoDto> DeterminaDiasNaoLetivos(IEnumerable<DiaLetivoDto> diasDoPeriodo, string ueCodigo)
        {
            return diasDoPeriodo.Where(c => (c.ExcluirAulaUe(ueCodigo) && c.EhNaoLetivo) ||
                                            (!c.DreIds.Any() && !c.UesIds.Any() && c.EhNaoLetivo) ||
                                            c.ExcluirAulaSME || (c.UesIds.Any() && c.UesIds.Contains(ueCodigo) && c.EhNaoLetivo) ||
                                            (c.Data.DayOfWeek == DayOfWeek.Sunday || c.Data.DayOfWeek == DayOfWeek.Saturday))?.ToList();
        }

        private IList<DiaLetivoDto> DeterminaDiasLetivos(IEnumerable<DiaLetivoDto> diasDoPeriodo, string ueCodigo)
        {
            return diasDoPeriodo.Where(c => (c.CriarAulaUe(ueCodigo) && c.EhLetivo) ||
                                       (!c.DreIds.Any() && !c.UesIds.Any() && c.EhLetivo) ||
                                       (!c.PossuiEvento && c.EhLetivo && !c.PossuiEventoSME && !c.PossuiEventoUe(ueCodigo)) ||
                                       c.CriarAulaSME)?.OrderBy(c => c.Data)?.ToList();
        }

        private IEnumerable<Aula> ObterListaDeAulas(List<DiaLetivoDto> diasLetivos, long tipoCalendarioId, DadosTurmaAulasAutomaticaDto turma, string ueCodigo, Modalidade modalidade, string rfProfessor)
        {
            var lista = new List<Aula>();
            if (diasLetivos != null)
            {
                for (int d = 0; d < diasLetivos.Count; d++)
                {
                    var diaLetivo = diasLetivos.ElementAt(d);
                    lista.Add(new Aula
                    {
                        DataAula = diaLetivo.Data,
                        DisciplinaId = turma.ComponenteCurricularCodigo,
                        DisciplinaNome = turma.ComponenteCurricularDescricao,
                        Quantidade = modalidade == Modalidade.EJA ? 5 : 1,
                        RecorrenciaAula = RecorrenciaAula.AulaUnica,
                        TipoAula = TipoAula.Normal,
                        TipoCalendarioId = tipoCalendarioId,
                        TurmaId = turma.TurmaCodigo,
                        UeId = ueCodigo,
                        ProfessorRf = rfProfessor,
                        CriadoPor = "Sistema",
                        CriadoRF = "0"
                    });
                }
            }

            return lista;
        }
    }
}
