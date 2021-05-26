using MediatR;
using Sentry;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class CriarAulasInfantilAutomaticamenteCommandHandler : IRequestHandler<CriarAulasInfantilAutomaticamenteCommand, bool>
    {
        private readonly IRepositorioAula repositorioAula;
        private readonly IMediator mediator;

        public CriarAulasInfantilAutomaticamenteCommandHandler(IRepositorioAula repositorioAula,
                                                               IMediator mediator)
        {
            this.repositorioAula = repositorioAula;
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(CriarAulasInfantilAutomaticamenteCommand request, CancellationToken cancellationToken)
        {

            var tipoCalendarioId = request.TipoCalendarioId;
            var diasParaCriarAula = request.DiasLetivos;
            var diasForaDoPeriodo = request.DiasForaDoPeriodoEscolar;
            var aulasACriar = new List<Aula>();
            var aulasAExcluirComFrequenciaRegistrada = new List<DateTime>();
            var turmas = request.Turmas.ToList();

            var contadorAulasCriadas = 0;
            var contadorAulasExcluidas = 0;

            var idsAulasAExcluir = new List<long>();
            var timerGeral = Stopwatch.StartNew();

            for (int i = 0; i < turmas.Count; i++)
            {
                var turma = turmas[i];

                var aulas = (await mediator.Send(new ObterAulasDaTurmaPorTipoCalendarioQuery(turma.CodigoTurma, tipoCalendarioId)))?.ToList();                
                aulas.AddRange(await repositorioAula.ObterAulasExcluidasComDiarioDeBordoAtivos(turma.CodigoTurma, tipoCalendarioId));

                if (aulas == null)
                    aulasACriar.AddRange(ObterAulasParaCriacao(tipoCalendarioId, diasParaCriarAula.Where(c => !turma.DataInicio.HasValue || c.Data.Date >= turma.DataInicio)?.ToList(), turma));
                else
                {
                    if (!aulas.Any())
                        aulasACriar.AddRange(ObterAulasParaCriacao(tipoCalendarioId, diasParaCriarAula.Where(c => !turma.DataInicio.HasValue || c.Data.Date >= turma.DataInicio)?.ToList(), turma));
                    else
                    {
                        var diasSemAula = DeterminaDiasLetivos(diasParaCriarAula, turma)
                            .Where(c => !aulas.Any(a => a.DataAula == c.Data) && (!turma.DataInicio.HasValue || c.Data.Date >= turma.DataInicio))?
                            .OrderBy(a => a.Data)?
                            .Distinct()
                            .ToList();

                        var aulasParaCriacao = ObterAulasParaCriacao(tipoCalendarioId, diasSemAula, turma)?.ToList();

                        if (aulasParaCriacao != null)
                        {
                            for (int a = 0; a < aulasParaCriacao.Count; a++)
                                aulasACriar.Add(aulasParaCriacao[a]);
                        }
                        
                        IEnumerable<Aula> aulasDaTurmaParaExcluir = ObterAulasParaExcluir(diasParaCriarAula.ToList(), turma, aulas);
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

            Console.WriteLine($"Manutenção de aulas realizada em {timerGeral.Elapsed}");

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
        private IEnumerable<Aula> ObterAulasParaExcluir(IEnumerable<DiaLetivoDto> diasDoPeriodo, Turma turma, IEnumerable<Aula> aulas)
        {
            var diasLetivos = DeterminaDiasLetivos(diasDoPeriodo, turma);
            var diasNaoLetivos = DeterminaDiasNaoLetivos(diasDoPeriodo, turma);

            var diasParaExcluir = diasDoPeriodo.Where(l => diasLetivos != null && !diasLetivos.Any(n => n.Data == l.Data) &&
                                                           (diasNaoLetivos != null && diasNaoLetivos.Any(n => n.Data == l.Data)))?
                                               .OrderBy(c => c.Data)?
                                               .ToList();

            return aulas.Where(c => (diasParaExcluir != null && diasParaExcluir.Any(a => a.Data == c.DataAula && !c.Excluido)) ||
                                   (!turma.DataInicio.HasValue || c.DataAula.Date < turma.DataInicio.Value.Date))
                        .OrderBy(c => c.DataAula);
        }      

        private IEnumerable<Aula> ObterAulasParaCriacao(long tipoCalendarioId, IEnumerable<DiaLetivoDto> diasDoPeriodo, Turma turma)
        {
            var diasLetivos = DeterminaDiasLetivos(diasDoPeriodo, turma);
            var diasNaoLetivos = DeterminaDiasNaoLetivos(diasDoPeriodo, turma);

            var diasParaCriar = diasDoPeriodo.Where(l => diasLetivos != null && diasLetivos.Any(n => n.Data == l.Data) || (diasNaoLetivos == null || !diasNaoLetivos.Any(n => n.Data == l.Data)))?.ToList();

            return ObterListaDeAulas(diasParaCriar?.DistinctBy(c => c.Data)?.ToList(), tipoCalendarioId, turma);
        }

        private IList<DiaLetivoDto> DeterminaDiasNaoLetivos(IEnumerable<DiaLetivoDto> diasDoPeriodo, Turma turma)
        {
            return diasDoPeriodo.Where(c => (c.ExcluirAulaUe(turma.Ue.CodigoUe) && c.EhNaoLetivo) ||
                                            (!c.DreIds.Any() && !c.UesIds.Any() && c.EhNaoLetivo) ||
                                            c.ExcluirAulaSME || (c.UesIds.Any() && c.UesIds.Contains(turma.Ue.CodigoUe) && c.EhNaoLetivo))?.ToList();
        }

        private IList<DiaLetivoDto> DeterminaDiasLetivos(IEnumerable<DiaLetivoDto> diasDoPeriodo, Turma turma)
        {
            return diasDoPeriodo.Where(c => (c.CriarAulaUe(turma.Ue.CodigoUe) && c.EhLetivo) ||                                       
                                       (!c.DreIds.Any() && !c.UesIds.Any() && c.EhLetivo) ||
                                       c.CriarAulaSME())?.OrderBy(c => c.Data)?.ToList();
        }

        private IEnumerable<Aula> ObterListaDeAulas(List<DiaLetivoDto> diasLetivos, long tipoCalendarioId, Turma turma)
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
                        DisciplinaId = "512",
                        DisciplinaNome = "Regência de classe Infantil",
                        Quantidade = 1,
                        RecorrenciaAula = RecorrenciaAula.AulaUnica,
                        TipoAula = TipoAula.Normal,
                        TipoCalendarioId = tipoCalendarioId,
                        TurmaId = turma.CodigoTurma,
                        UeId = turma.Ue.CodigoUe,
                        ProfessorRf = "Sistema"
                    });
                }
            }

            return lista;
        }
    }
}
