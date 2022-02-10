﻿using MediatR;
using SME.SGP.Aplicacao.Commands.Aulas.ReaverAulaDiarioBordoExcluida;
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
            var diariosBordoComAulaExcluida = new List<DiarioBordo>();
            var aulasAExcluirComFrequenciaRegistrada = new List<DateTime>();
            var turma = request.Turma;

            var contadorAulasCriadas = 0;
            var contadorAulasExcluidas = 0;

            string codigoDisciplina = "512";
            string criadoPor = "Sistema";

            var idsAulasAExcluir = new List<long>();
            var timerGeral = Stopwatch.StartNew();

            var diasNaoLetivos = DeterminaDiasNaoLetivos(diasParaCriarAula, turma);
            var diasLetivos = DeterminaDiasLetivos(diasParaCriarAula, diasNaoLetivos.Select(dnl => dnl.Data).Distinct(), turma);

            var aulas = (await mediator.Send(new ObterAulasDaTurmaPorTipoCalendarioQuery(turma.CodigoTurma, tipoCalendarioId, criadoPor)))?.ToList();

            if (aulas == null || !aulas.Any())
            {
                var periodoTurmaConsiderado = diasParaCriarAula
                    .Where(c => !turma.DataInicio.HasValue || c.Data.Date >= turma.DataInicio)?.ToList();

                diariosBordoComAulaExcluida.AddRange(await mediator
                        .Send(new RecuperarDiarioBordoComAulasExcluidasQuery(turma.CodigoTurma, codigoDisciplina, tipoCalendarioId, periodoTurmaConsiderado.Select(p => p.Data).ToArray())));

                var aulasCriacao = from ac in ObterAulasParaCriacao(tipoCalendarioId, periodoTurmaConsiderado, diasLetivos, diasNaoLetivos, turma)
                                   where !diariosBordoComAulaExcluida.Select(db => db.Aula.DataAula).Contains(ac.DataAula)
                                   select ac;

                aulasACriar.AddRange(aulasCriacao);
            }
            else
            {
                var diasSemAula = diasLetivos
                    .Where(c => !aulas.Any(a => a.DataAula == c.Data) && (!turma.DataInicio.HasValue || c.Data.Date >= turma.DataInicio))?
                    .OrderBy(a => a.Data)?
                    .Distinct()
                    .ToList();

                if (diasSemAula != null && diasSemAula.Any())
                {
                    diariosBordoComAulaExcluida.AddRange(await mediator
                        .Send(new RecuperarDiarioBordoComAulasExcluidasQuery(turma.CodigoTurma, codigoDisciplina, tipoCalendarioId, diasSemAula.Select(d => d.Data).ToArray())));
                }

                var diasCriacaoAula = (from d in diasSemAula
                                       where !diariosBordoComAulaExcluida.Select(db => db.Aula.DataAula).Contains(d.Data)
                                       select d).ToList();

                aulasACriar.AddRange(ObterListaDeAulas(diasCriacaoAula, tipoCalendarioId, turma).ToList());

                IEnumerable<Aula> aulasDaTurmaParaExcluir = ObterAulasParaExcluir(diasNaoLetivos, turma, aulas.Where(a => !a.AulaCJ), diasLetivos);
                await ExcluirAulas(aulasAExcluirComFrequenciaRegistrada, idsAulasAExcluir, aulasDaTurmaParaExcluir.ToList());

                var aulasForaDoPeriodo = aulas.Where(c => diasForaDoPeriodo.Contains(c.DataAula) && !c.AulaCJ);
                if (aulasForaDoPeriodo != null && aulasForaDoPeriodo.Any())
                    await ExcluirAulas(aulasAExcluirComFrequenciaRegistrada, idsAulasAExcluir, aulasForaDoPeriodo.ToList());
            }

            if (aulasAExcluirComFrequenciaRegistrada.Any())
            {
                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaNotificacaoExclusaoAulasComFrequencia,
                    new NotificarExclusaoAulasComFrequenciaDto(turma, aulasAExcluirComFrequenciaRegistrada), Guid.NewGuid(), null));

                aulasAExcluirComFrequenciaRegistrada.Clear();
            }

            if (diariosBordoComAulaExcluida.Any())
            {
                diariosBordoComAulaExcluida
                    .ForEach(db =>
                    {
                        mediator.Send(new ReaverAulaDiarioBordoExcluidaCommand(db.AulaId, db.Id)).Wait();
                    });
            }

            if (aulasACriar.Any())
                contadorAulasCriadas = CriarAulas(aulasACriar, contadorAulasCriadas);

            if (idsAulasAExcluir.Any())
                contadorAulasExcluidas = await ExcluirAulas(contadorAulasExcluidas, idsAulasAExcluir);

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
        private IEnumerable<Aula> ObterAulasParaExcluir(IEnumerable<DiaLetivoDto> diasNaoLetivos, Turma turma, IEnumerable<Aula> aulas, IEnumerable<DiaLetivoDto> diasLetivos)
        {
            var aulasExclusao = new List<Aula>();
            var aulasNaoExcluidasOrdenadas = aulas
                .Where(a => !a.Excluido)
                .OrderBy(a => a.DataAula)
                .ThenBy(a => a.Id);

            foreach (var aula in aulasNaoExcluidasOrdenadas)
            {
                var excluirAula = ((diasNaoLetivos != null && diasNaoLetivos.Any(a => a.Data == aula.DataAula)) ||
                                  !turma.DataInicio.HasValue ||
                                  aula.DataAula.Date < turma.DataInicio.Value.Date ||
                                  aulas.Any(a => a.DataAula.Date.Equals(aula.DataAula.Date) && !a.Excluido && a.Id < aula.Id)) &&
                                  !diasLetivos.Any(d => d.Data == aula.DataAula);

                if (excluirAula)
                    aulasExclusao.Add(aula);
            }

            return aulasExclusao;
        }

        private IEnumerable<Aula> ObterAulasParaCriacao(long tipoCalendarioId, IEnumerable<DiaLetivoDto> diasDoPeriodo, IEnumerable<DiaLetivoDto> diasLetivos, IEnumerable<DiaLetivoDto> diasNaoLetivos, Turma turma)
        {
            var diasParaCriar = diasDoPeriodo.Where(l => diasLetivos != null && diasLetivos.Any(n => n.Data == l.Data) || (diasNaoLetivos == null || !diasNaoLetivos.Any(n => n.Data == l.Data)))?.ToList();

            return ObterListaDeAulas(diasParaCriar?.DistinctBy(c => c.Data)?.ToList(), tipoCalendarioId, turma);
        }

        private IList<DiaLetivoDto> DeterminaDiasNaoLetivos(IEnumerable<DiaLetivoDto> diasDoPeriodo, Turma turma)
        {
            return diasDoPeriodo.Where(c => c.ExcluirAulaSME ||
                                            ((c.PossuiEventoDre(turma.Ue.Dre.CodigoDre) || c.PossuiEventoUe(turma.Ue.CodigoUe)) && c.EhNaoLetivo))?
                                .OrderBy(c => c.Data).ToList();
        }

        private IList<DiaLetivoDto> DeterminaDiasLetivos(IEnumerable<DiaLetivoDto> diasDoPeriodo, IEnumerable<DateTime> diasNaoLetivos, Turma turma)
        {
            return diasDoPeriodo.Where(c => c.CriarAulaSME ||
                                            ((c.PossuiEventoDre(turma.Ue.Dre.CodigoDre) || c.PossuiEventoUe(turma.Ue.CodigoUe)) && c.EhLetivo) ||
                                            (c.NaoPossuiDre && c.NaoPossuiUe && c.EhLetivo && !diasNaoLetivos.Contains(c.Data.Date)))?
                                .OrderBy(c => c.Data)?.ToList();
        }

        private IEnumerable<Aula> ObterListaDeAulas(List<DiaLetivoDto> diasLetivos, long tipoCalendarioId, Turma turma)
        {
            var lista = new List<Aula>();
            if (diasLetivos != null && diasLetivos.Any())
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
