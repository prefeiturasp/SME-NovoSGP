
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
        private readonly IMediator mediator;

        public CriarAulasRegenciaAutomaticamenteCommandHandler(IRepositorioAula repositorioAula, IMediator mediator)
        {
            this.repositorioAula = repositorioAula ?? throw new ArgumentNullException(nameof(repositorioAula));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(CriarAulasRegenciaAutomaticamenteCommand request, CancellationToken cancellationToken)
        {
            var tipoCalendarioId = request.TipoCalendarioId;
            var diasParaCriarAula = request.DiasLetivos;
            var diasForaDoPeriodo = request.DiasForaDoPeriodoEscolar;
            var aulasACriar = new List<Aula>();
            var aulasAExcluirComFrequenciaRegistrada = new List<DateTime>();
            var dadosTurmas = request.DadosTurmas;
            var ueCodigo = request.UeCodigo;
            var modalidade = request.Modalidade;

            var contadorAulasCriadas = 0;
            var contadorAulasExcluidas = 0;

            var idsAulasAExcluir = new List<long>();

            foreach (var dadoTurma in dadosTurmas)
            {
                var aulas = (List<Aula>)await mediator.Send(new ObterAulasDaTurmaPorTipoCalendarioQuery(dadoTurma.TurmaCodigo, tipoCalendarioId));
                aulas.AddRange(await repositorioAula.ObterAulasExcluidasComDiarioDeBordoAtivos(dadoTurma.TurmaCodigo, tipoCalendarioId));
                var professorTitular = await mediator.Send(new ObterProfessorTitularPorTurmaEComponenteCurricularQuery(dadoTurma.TurmaCodigo, dadoTurma.ComponenteCurricularCodigo));
                var professorRf = professorTitular != null ? professorTitular.ProfessorRf : "";

                var aulasCriarComDataInicio = diasParaCriarAula.Where(c => dadoTurma.DataInicioTurma != null && c.Data.Date >= dadoTurma.DataInicioTurma)?.ToList();
                if (aulas == null)
                    aulasACriar.AddRange(ObterAulasParaCriacao(tipoCalendarioId, aulasCriarComDataInicio, dadoTurma, ueCodigo, modalidade, professorRf));
                else
                {
                    if (!aulas.Any())
                        aulasACriar.AddRange(ObterAulasParaCriacao(tipoCalendarioId, aulasCriarComDataInicio, dadoTurma, ueCodigo, modalidade, professorRf));
                    else
                    {
                        var diasLetivos = DeterminaDiasLetivos(diasParaCriarAula, request.UeCodigo);
                        var diasNaoLetivos = DeterminaDiasNaoLetivos(diasParaCriarAula, request.UeCodigo);
                        var diasSemAula = diasLetivos
                            .Where(c => !aulas.Any(a => a.DataAula == c.Data) && (dadoTurma.DataInicioTurma != null && c.Data.Date >= dadoTurma.DataInicioTurma))?
                            .OrderBy(a => a.Data)?
                            .Distinct()
                            .ToList();

                        var aulasParaCriacao = ObterAulasParaCriacao(tipoCalendarioId, diasSemAula, dadoTurma, ueCodigo, modalidade, professorRf)?.ToList();

                        if (aulasParaCriacao != null)
                        {
                            for (int a = 0; a < aulasParaCriacao.Count; a++)
                                aulasACriar.Add(aulasParaCriacao[a]);
                        }

                        IEnumerable<Aula> aulasDaTurmaParaExcluir = ObterAulasParaExcluir(diasParaCriarAula.ToList(), dadoTurma, aulas, request.UeCodigo);
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
                    var turma = await mediator.Send(new ObterTurmaComUeEDrePorCodigoQuery(dadoTurma.TurmaCodigo));
                    await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaNotificacaoExclusaoAulasComFrequencia,
                        new NotificarExclusaoAulasComFrequenciaDto(turma, aulasAExcluirComFrequenciaRegistrada), Guid.NewGuid(), null));

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


        private async Task ExcluirAulas(List<DateTime> aulasAExcluirComFrequenciaRegistrada, List<long> idsAulasAExcluir, List<Aula> aulasDaTurmaParaExcluir)
        {
            if (aulasDaTurmaParaExcluir != null)
            {
                foreach (var aula in aulasDaTurmaParaExcluir)
                {
                    var existeFrequencia = await mediator.Send(new ObterAulaPossuiFrequenciaQuery(aula.Id));
                    if (existeFrequencia)
                        aulasAExcluirComFrequenciaRegistrada.Add(aula.DataAula);
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
        private IEnumerable<Aula> ObterAulasParaExcluir(IEnumerable<DiaLetivoDto> diasDoPeriodo, DadosTurmaAulasAutomaticaDto turma, IEnumerable<Aula> aulas, string ueCodigo)
        {
            var diasLetivos = DeterminaDiasLetivos(diasDoPeriodo, ueCodigo);
            var diasNaoLetivos = DeterminaDiasNaoLetivos(diasDoPeriodo, ueCodigo);

            var diasParaExcluir = diasDoPeriodo.Where(l => diasLetivos != null && !diasLetivos.Any(n => n.Data == l.Data) &&
                                                           (diasNaoLetivos != null && diasNaoLetivos.Any(n => n.Data == l.Data)))?
                                               .OrderBy(c => c.Data)?
                                               .ToList();

            return aulas.Where(c => (diasParaExcluir != null && diasParaExcluir.Any(a => a.Data == c.DataAula && !c.Excluido)) ||
                                   (turma.DataInicioTurma != null && c.DataAula.Date < turma.DataInicioTurma.Date))
                        .OrderBy(c => c.DataAula);
        }

        private IEnumerable<Aula> ObterAulasParaCriacao(long tipoCalendarioId, IEnumerable<DiaLetivoDto> diasDoPeriodo, DadosTurmaAulasAutomaticaDto turma, string ueCodigo, Modalidade modalidade, string rfProfessor)
        {
            var diasLetivos = DeterminaDiasLetivos(diasDoPeriodo, ueCodigo);
            var diasNaoLetivos = DeterminaDiasNaoLetivos(diasDoPeriodo, ueCodigo);

            var diasParaCriar = diasDoPeriodo.Where(l => diasLetivos != null && diasLetivos.Any(n => n.Data == l.Data) || (diasNaoLetivos == null || !diasNaoLetivos.Any(n => n.Data == l.Data)))?.ToList();

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
                                       (!c.PossuiEvento && c.EhLetivo && !c.PossuiEventoSME() && !c.PossuiEventoUe(ueCodigo)) ||
                                       c.CriarAulaSME())?.OrderBy(c => c.Data)?.ToList();
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
                        UeId =ueCodigo,
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
