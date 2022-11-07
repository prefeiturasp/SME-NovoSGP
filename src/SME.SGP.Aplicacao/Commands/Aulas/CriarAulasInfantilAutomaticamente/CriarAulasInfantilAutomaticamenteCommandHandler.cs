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
        private const string AUDITORIA_SISTEMA = "SISTEMA";

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
            var aulasACriar = new List<(Aula aula, long? planoAulaId)>();
            var diariosBordoComAulaExcluida = new List<DiarioBordo>();
            var aulasAExcluirComFrequenciaRegistrada = new List<DateTime>();
            var turma = request.Turma;
            var dadosDisciplinaAulaCriada = request.DadosAulaCriadaAutomaticamente.ComponenteCurricular;
            var quantidadeAulas = request.DadosAulaCriadaAutomaticamente.QuantidadeAulas;
            var rfProfessor = request.DadosAulaCriadaAutomaticamente.RfProfessor;

            var contadorAulasCriadas = 0;
            var contadorAulasExcluidas = 0;

            string criadoPor = "Sistema";
            var idsAulasAExcluir = new List<long>();

            var diasNaoLetivos = DeterminaDiasNaoLetivos(diasParaCriarAula, turma);
            var diasLetivos = DeterminaDiasLetivos(diasParaCriarAula, diasNaoLetivos.Select(dnl => dnl.Data).Distinct(), turma);

            var aulas = (await mediator.Send(new ObterAulasDaTurmaPorTipoCalendarioQuery(turma.CodigoTurma, tipoCalendarioId, criadoPor)))?.ToList();
            var aulasCriadasOuExcluidasPorUsuario = (await mediator.Send(new ObterAulasDaTurmaPorTipoCalendarioQuery(turma.CodigoTurma, tipoCalendarioId)))
                .Where(a => request.CodigosDisciplinasConsideradas.Contains(a.DisciplinaId) &&
                            ((!a.CriadoPor.Equals(criadoPor, StringComparison.InvariantCultureIgnoreCase) && !a.Excluido) ||
                            (!(a.AlteradoPor?.Equals(criadoPor, StringComparison.InvariantCultureIgnoreCase) ?? true) && a.Excluido)))
                .ToList();
            var aulasCriadasPeloSistema = aulas.Except(aulasCriadasOuExcluidasPorUsuario);

            if (aulas == null || !aulas.Any())
            {
                var periodoTurmaConsiderado = diasParaCriarAula
                    .Where(c => !turma.DataInicio.HasValue || c.Data.Date >= turma.DataInicio)?.ToList();

                diariosBordoComAulaExcluida.AddRange(await mediator
                        .Send(new RecuperarDiarioBordoComAulasExcluidasQuery(turma.CodigoTurma, request.CodigosDisciplinasConsideradas.ToArray(), tipoCalendarioId, periodoTurmaConsiderado.Select(p => p.Data).ToArray())));

                var aulasCriacao = from ac in ObterAulasParaCriacao(tipoCalendarioId, periodoTurmaConsiderado, diasLetivos, diasNaoLetivos, turma, aulasCriadasPeloSistema, dadosDisciplinaAulaCriada, quantidadeAulas, rfProfessor)
                                   where !diariosBordoComAulaExcluida.Select(db => db.Aula.DataAula).Contains(ac.aula.DataAula) &&
                                         !aulasCriadasOuExcluidasPorUsuario.Select(a => a.DataAula).Distinct().Contains(ac.aula.DataAula)
                                   select ac;

                aulasACriar.AddRange(aulasCriacao);
            }
            else
            {
                var diasSemAula = diasLetivos
                    .Where(c => !aulas.Any(a => !a.Excluido && a.DataAula == c.Data) && (!turma.DataInicio.HasValue || c.Data.Date >= turma.DataInicio))?
                    .OrderBy(a => a.Data)?
                    .Distinct()
                    .ToList();

                if (diasSemAula != null && diasSemAula.Any())
                {
                    diariosBordoComAulaExcluida.AddRange(await mediator
                        .Send(new RecuperarDiarioBordoComAulasExcluidasQuery(turma.CodigoTurma, request.CodigosDisciplinasConsideradas.ToArray(), tipoCalendarioId, diasSemAula.Select(d => d.Data).ToArray())));
                }

                var diasCriacaoAula = (from d in diasSemAula
                                       where !diariosBordoComAulaExcluida.Select(db => db.Aula.DataAula).Contains(d.Data) &&
                                             !aulasCriadasOuExcluidasPorUsuario.Select(a => a.DataAula).Distinct().Contains(d.Data)
                                       select d).ToList();

                aulasACriar.AddRange(ObterListaDeAulas(diasCriacaoAula, tipoCalendarioId, turma, aulasCriadasPeloSistema, dadosDisciplinaAulaCriada, quantidadeAulas, rfProfessor).ToList());

                var aulasDaTurmaParaExcluir = ObterAulasParaExcluir(diasNaoLetivos, turma, aulas.Where(a => !a.AulaCJ), diasLetivos);
                ExcluirAulas(aulasAExcluirComFrequenciaRegistrada, idsAulasAExcluir, aulasDaTurmaParaExcluir.ToList());

                var aulasForaDoPeriodo = aulas.Where(c => diasForaDoPeriodo.Contains(c.DataAula) && !c.AulaCJ);
                if (aulasForaDoPeriodo != null && aulasForaDoPeriodo.Any())
                    ExcluirAulas(aulasAExcluirComFrequenciaRegistrada, idsAulasAExcluir, aulasForaDoPeriodo.ToList());
            }

            if (aulasAExcluirComFrequenciaRegistrada.Any())
            {
                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpAula.RotaNotificacaoExclusaoAulasComFrequencia,
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


        private void ExcluirAulas(List<DateTime> aulasAExcluirComFrequenciaRegistrada, List<long> idsAulasAExcluir, List<Aula> aulasDaTurmaParaExcluir)
        {
            if (aulasDaTurmaParaExcluir != null)
            {
                foreach (var aula in aulasDaTurmaParaExcluir)
                {
                    if (aula.DadosComplementares.PossuiFrequencia && !aula.DadosComplementares.RegistroFrequenciaExcluido)
                        aulasAExcluirComFrequenciaRegistrada.Add(aula.DataAula);
                    else
                        idsAulasAExcluir.Add(aula.Id);
                }
            }
        }

        private int CriarAulas(List<(Aula aula, long? planoAulaId)> aulasACriar, int contadorAulasCriadas)
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
                var aulasMesmoDia = aulas
                    .Where(a => a.DataAula.Date.Equals(aula.DataAula.Date) && !a.Excluido)
                    .ToList();

                var excluirAula = ((diasNaoLetivos != null && diasNaoLetivos.Any(a => a.Data == aula.DataAula) &&
                                    !diasLetivos.Any(d => d.Data == aula.DataAula) && aula.DadosComplementares.PossuiFrequencia) ||
                                    !turma.DataInicio.HasValue || aula.DataAula.Date < turma.DataInicio.Value.Date ||
                                    aulasMesmoDia.Any(a => a.Id < aula.Id && a.DadosComplementares.PossuiFrequencia) ||
                                    aulasMesmoDia.Any(a => a.Id > aula.Id && !aula.DadosComplementares.PossuiFrequencia) ||
                                    aulasMesmoDia.Any(a => a.Id < aula.Id && a.DadosComplementares.PossuiFrequencia && aula.DadosComplementares.PossuiFrequencia) ||
                                    aulasMesmoDia.Any(a => a.Id < aula.Id && !a.DadosComplementares.PossuiFrequencia && !aula.DadosComplementares.PossuiFrequencia));

                if(!excluirAula)
                    excluirAula = VerificaSeFoiAulaCriadaNoFimDeSemanaAutomaticaSemEventoLetivo(aula, diasLetivos);

                if (excluirAula)
                    aulasExclusao.Add(aula);
            }

            return aulasExclusao;
        }

        public bool VerificaSeFoiAulaCriadaNoFimDeSemanaAutomaticaSemEventoLetivo(Aula aula, IEnumerable<DiaLetivoDto> diasLetivos)
            => aula.DataAula.FimDeSemana() && aula.CriadoPor.ToUpper() == AUDITORIA_SISTEMA && !diasLetivos.Any(d=> d.Data == aula.DataAula);

        private IEnumerable<(Aula aula, long? plano_aula_id)> ObterAulasParaCriacao(long tipoCalendarioId, IEnumerable<DiaLetivoDto> diasDoPeriodo, IEnumerable<DiaLetivoDto> diasLetivos, IEnumerable<DiaLetivoDto> diasNaoLetivos, Turma turma, IEnumerable<Aula> aulasCriadasPeloSistema, (string id, string nome) dadosDisciplina, int quantidade, string rfProfessor)
        {
            var diasParaCriar = diasDoPeriodo
                .Where(l => diasLetivos != null && diasLetivos.Any(n => n.Data == l.Data) || (diasNaoLetivos == null || !diasNaoLetivos.Any(n => n.Data == l.Data)))?
                .ToList();

            return ObterListaDeAulas(diasParaCriar?.DistinctBy(c => c.Data)?.ToList(), tipoCalendarioId, turma, aulasCriadasPeloSistema, dadosDisciplina, quantidade, rfProfessor);
        }

        private IList<DiaLetivoDto> DeterminaDiasNaoLetivos(IEnumerable<DiaLetivoDto> diasDoPeriodo, Turma turma)
        {
            return diasDoPeriodo.Where(c => c.ExcluirAulaSME ||
                                            ((c.PossuiEventoDre(turma.Ue.Dre.CodigoDre) || c.PossuiEventoUe(turma.Ue.CodigoUe)) && c.EhNaoLetivo))?
                                .OrderBy(c => c.Data).ToList();
        }

        private IList<DiaLetivoDto> DeterminaDiasLetivos(IEnumerable<DiaLetivoDto> diasDoPeriodo, IEnumerable<DateTime> diasNaoLetivos, Turma turma)
        {
            return diasDoPeriodo.Where(c => !c.Data.FimDeSemana() && c.CriarAulaSME ||
                                            ((c.PossuiEventoDre(turma.Ue.Dre.CodigoDre) || c.PossuiEventoUe(turma.Ue.CodigoUe)) && c.EhLetivo) ||
                                            (c.NaoPossuiDre && c.NaoPossuiUe && c.EhLetivo && !diasNaoLetivos.Contains(c.Data.Date)))?
                                .OrderBy(c => c.Data)?.ToList();
        }

        private IEnumerable<(Aula aula, long? planoAulaId)> ObterListaDeAulas(List<DiaLetivoDto> diasLetivos, long tipoCalendarioId, Turma turma, IEnumerable<Aula> aulasCriadasPeloSistema, (string id, string nome) dadosDisciplina, int quantidade, string rfProfessor)
        {
            var lista = new List<(Aula aula, long? planoAulaId)>();
            if (diasLetivos != null && diasLetivos.Any())
            {
                for (int d = 0; d < diasLetivos.Count; d++)
                {
                    var diaLetivo = diasLetivos.ElementAt(d);

                    if (lista.Select(l => l.aula.DataAula.Date).Contains(diaLetivo.Data.Date)) continue;

                    var aulaExcluida = aulasCriadasPeloSistema
                        .Where(a => a.DataAula.Date.Equals(diaLetivo.Data.Date) && a.Excluido)
                        .OrderByDescending(a => a.CriadoEm)
                        .ThenBy(a => a.DadosComplementares.PossuiFrequencia)
                        .ThenBy(a => !a.DadosComplementares.RegistroFrequenciaExcluido)
                        .FirstOrDefault();

                    if (aulaExcluida != null)
                    {
                        var aulaComPlano = (from a in aulasCriadasPeloSistema
                                            where a.DataAula.Date.Equals(diaLetivo.Data) &&
                                                 !aulaExcluida.DadosComplementares.PossuiPlanoAula &&
                                                 a.Excluido &&
                                                 a.DadosComplementares.PossuiPlanoAula &&
                                                 !a.Id.Equals(aulaExcluida.Id)
                                            select a)
                                            .OrderByDescending(a => a.CriadoEm)
                                            .FirstOrDefault();

                        var planoAula = aulaComPlano != null ? mediator.Send(new ObterPlanoAulaPorAulaIdQuery(aulaComPlano.Id)).Result : null;
                        lista.Add((aulaExcluida, planoAula?.Id));
                    }
                    else
                    {
                        lista.Add((new Aula
                        {
                            DataAula = diaLetivo.Data,
                            DisciplinaId = dadosDisciplina.id,
                            DisciplinaNome = dadosDisciplina.nome,
                            Quantidade = quantidade,
                            RecorrenciaAula = RecorrenciaAula.AulaUnica,
                            TipoAula = TipoAula.Normal,
                            TipoCalendarioId = tipoCalendarioId,
                            TurmaId = turma.CodigoTurma,
                            UeId = turma.Ue.CodigoUe,
                            ProfessorRf = rfProfessor
                        }, null));
                    }
                }
            }

            return lista;
        }
    }
}
