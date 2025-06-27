using MediatR;
using SME.SGP.Aplicacao.Commands.Aulas.ReaverAulaDiarioBordoExcluida;
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
    public class CriarAulasInfantilERegenciaAutomaticamenteCommandHandler : IRequestHandler<CriarAulasInfantilERegenciaAutomaticamenteCommand, bool>
    {
        private const string AUDITORIA_SISTEMA = "SISTEMA";
        private readonly IRepositorioAula repositorioAula;
        private readonly IMediator mediator;

        public CriarAulasInfantilERegenciaAutomaticamenteCommandHandler(IRepositorioAula repositorioAula,
                                                                        IMediator mediator)
        {
            this.repositorioAula = repositorioAula ?? throw new ArgumentNullException(nameof(repositorioAula));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(CriarAulasInfantilERegenciaAutomaticamenteCommand request, CancellationToken cancellationToken)
        {
            List<(Aula aula, long? planoAulaId)> aulasACriar = new();
            List<DiarioBordo> diariosBordoComAulaExcluida = new();
            List<DateTime> aulasAExcluirComFrequenciaRegistrada = new();
            List<long> idsAulasAExcluir = new();
            var tipoCalendarioId = request.TipoCalendarioId;
            var diasParaCriarAula = request.DiasLetivos;
            var diasForaDoPeriodo = request.DiasForaDoPeriodoEscolar;
            var turma = request.Turma;
            var dadosDisciplinaAulaCriada = request.DadosAulaCriadaAutomaticamente.ComponenteCurricular;
            var quantidadeAulas = request.DadosAulaCriadaAutomaticamente.QuantidadeAulas;
            var rfProfessor = request.DadosAulaCriadaAutomaticamente.RfProfessor;
            var contadorAulasCriadas = 0;
            var contadorAulasExcluidas = 0;
            var diasNaoLetivos = DeterminaDiasNaoLetivos(diasParaCriarAula, turma);
            var diasLetivos = DeterminaDiasLetivos(diasParaCriarAula, diasNaoLetivos.Select(dnl => dnl.Data).Distinct(), turma);

            var aulas = await mediator
                .Send(new ObterAulasDaTurmaPorTipoCalendarioQuery(turma.CodigoTurma, tipoCalendarioId, AUDITORIA_SISTEMA), cancellationToken);

            var aulasCriadasOuExcluidasPorUsuario = await ObterAulasTurmaCriadasExcluidasUsuario(turma.CodigoTurma, tipoCalendarioId, 
                                                                                                 request.CodigosDisciplinasConsideradas.ToArray(), 
                                                                                                 cancellationToken);
                
            var aulasCriadasPeloSistema = aulas.Except(aulasCriadasOuExcluidasPorUsuario);
            if (aulas.NaoPossuiRegistros())
            { // Esse if não faz sentido pois o método FiltrarAulasCriacao ira excluir todas as aulasCriadasPeloSistema que estão em aulasCriadasOuExcluidasPorUsuario
              // Então, se o método não retornar nada não há necessidade de continuar o processamento.
              // ObterAulasParaCriacao usam os dados de aulasCriadasPeloSistema como base para fazer o retorno
                var periodoTurmaConsiderado = FiltrarDiasAposInicioTurma(diasParaCriarAula, turma);
                await RecuperarDiariosBordoComAulaExcluida(request.CodigosDisciplinasConsideradas.ToArray(), tipoCalendarioId, diariosBordoComAulaExcluida, turma.CodigoTurma, periodoTurmaConsiderado, cancellationToken);
                var aulasCriacao = FiltrarAulasCriacao(await ObterAulasParaCriacao(request, periodoTurmaConsiderado, diasLetivos, diasNaoLetivos, aulasCriadasPeloSistema),
                                                       diariosBordoComAulaExcluida, aulasCriadasOuExcluidasPorUsuario);
                aulasACriar.AddRange(aulasCriacao);
            }
            else
            {
                var diasSemAula = FiltrarDiasLetivosSemAula(diasLetivos, aulas, turma);
                if (diasSemAula.PossuiRegistros())
                    await RecuperarDiariosBordoComAulaExcluida(request.CodigosDisciplinasConsideradas.ToArray(), tipoCalendarioId, diariosBordoComAulaExcluida, turma.CodigoTurma, diasSemAula, cancellationToken);

                var diasCriacaoAula = FiltrarDiasCriacaoAula(diasSemAula, diariosBordoComAulaExcluida, aulasCriadasOuExcluidasPorUsuario);
                aulasACriar.AddRange(await ObterListaDeAulas(diasCriacaoAula, tipoCalendarioId, turma, aulasCriadasPeloSistema, dadosDisciplinaAulaCriada, quantidadeAulas, rfProfessor));

                var aulasDaTurmaParaExcluir = ObterAulasParaExcluir(diasNaoLetivos, turma, aulas.Where(a => !a.AulaCJ), diasLetivos);
                ExcluirAulas(aulasAExcluirComFrequenciaRegistrada, idsAulasAExcluir, aulasDaTurmaParaExcluir.ToList());

                var aulasForaDoPeriodo = aulas.Where(c => diasForaDoPeriodo.Contains(c.DataAula) && !c.AulaCJ);
                if (aulasForaDoPeriodo.PossuiRegistros())
                    ExcluirAulas(aulasAExcluirComFrequenciaRegistrada, idsAulasAExcluir, aulasForaDoPeriodo.ToList());
            }

            await NotificarExclusaoAulasComFrequencia(aulasAExcluirComFrequenciaRegistrada, turma, cancellationToken);
            await ReaverAulasExcluidasComDiarioBordo(diariosBordoComAulaExcluida, cancellationToken);

            contadorAulasCriadas = await CriarAulas(aulasACriar, contadorAulasCriadas);
            contadorAulasExcluidas = await ExcluirAulas(contadorAulasExcluidas, idsAulasAExcluir, cancellationToken);
            return true;
        }

        private async Task ReaverAulasExcluidasComDiarioBordo(IEnumerable<DiarioBordo> diariosBordoComAulaExcluida, CancellationToken cancellationToken)
        {
            foreach (var db in diariosBordoComAulaExcluida)
                await mediator.Send(new ReaverAulaDiarioBordoExcluidaCommand(db.AulaId, db.Id), cancellationToken);
        }

        private async Task NotificarExclusaoAulasComFrequencia(List<DateTime> aulasAExcluirComFrequenciaRegistrada, Turma turma, CancellationToken cancellationToken)
        {
            if (aulasAExcluirComFrequenciaRegistrada.NaoPossuiRegistros())
                return;
            
            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpAula.RotaNotificacaoExclusaoAulasComFrequencia,
                new NotificarExclusaoAulasComFrequenciaDto(turma, aulasAExcluirComFrequenciaRegistrada), Guid.NewGuid(), null), cancellationToken);

            aulasAExcluirComFrequenciaRegistrada.Clear();
        }

        private static List<DiaLetivoDto> FiltrarDiasCriacaoAula(IEnumerable<DiaLetivoDto> diasSemAula, IEnumerable<DiarioBordo> diariosBordoComAulaExcluida, IEnumerable<Dominio.Aula> aulasCriadasOuExcluidasPorUsuario)
            => (from d in diasSemAula
                where !diariosBordoComAulaExcluida.Select(db => db.Aula.DataAula).Contains(d.Data) &&
                      !aulasCriadasOuExcluidasPorUsuario.Select(a => a.DataAula).Distinct().Contains(d.Data)
                select d).ToList();
        
        private static IEnumerable<(Aula aula, long? plano_aula_id)> FiltrarAulasCriacao(IEnumerable<(Aula aula, long? plano_aula_id)> aulasParaCriacao, IEnumerable<DiarioBordo> diariosBordoComAulaExcluida, IEnumerable<Dominio.Aula> aulasCriadasOuExcluidasPorUsuario)
        => from ac in aulasParaCriacao
           where !diariosBordoComAulaExcluida.Select(db => db.Aula.DataAula).Contains(ac.aula.DataAula) &&
                 !aulasCriadasOuExcluidasPorUsuario.Select(a => a.DataAula).Distinct().Contains(ac.aula.DataAula)
           select ac;

        private async Task<IEnumerable<Dominio.Aula>> ObterAulasTurmaCriadasExcluidasUsuario(string codigoTurma, long tipoCalendarioId, string[] codigosDisciplinasConsideradas,
                                                                                             CancellationToken cancellationToken)
        {
            return (await mediator.Send(new ObterAulasDaTurmaPorTipoCalendarioQuery(codigoTurma, tipoCalendarioId), cancellationToken))
                    .Where(a => codigosDisciplinasConsideradas.Contains(a.DisciplinaId) &&
                            ((!a.CriadoPor.Equals(AUDITORIA_SISTEMA, StringComparison.InvariantCultureIgnoreCase) && !a.Excluido) ||
                            (!(a.AlteradoPor?.Equals(AUDITORIA_SISTEMA, StringComparison.InvariantCultureIgnoreCase) ?? true) && a.Excluido)));
        }

        private async Task RecuperarDiariosBordoComAulaExcluida(string[] codigosDisciplinasConsideradas, long tipoCalendarioId, List<DiarioBordo> diariosBordoComAulaExcluida, string codigoTurma, List<DiaLetivoDto> periodoTurmaConsiderado, CancellationToken cancellationToken)
        {
            var diariosBordoComAulaExcluidaRecuperados = await mediator
                .Send(new RecuperarDiarioBordoComAulasExcluidasQuery(codigoTurma, codigosDisciplinasConsideradas, tipoCalendarioId, periodoTurmaConsiderado.Select(p => p.Data).Distinct().ToArray()), cancellationToken);

            if (diariosBordoComAulaExcluidaRecuperados.NaoEhNulo() && diariosBordoComAulaExcluidaRecuperados.Any())
                diariosBordoComAulaExcluida.AddRange(diariosBordoComAulaExcluidaRecuperados);
        }

        private static void ExcluirAulas(List<DateTime> aulasAExcluirComFrequenciaRegistrada, List<long> idsAulasAExcluir, List<Aula> aulasDaTurmaParaExcluir)
        {
            if (aulasDaTurmaParaExcluir.NaoEhNulo())
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

        private async Task<int> CriarAulas(List<(Aula aula, long? planoAulaId)> aulasACriar, int contadorAulasCriadas)
        {
            if (aulasACriar.PossuiRegistros())
            {
                await repositorioAula.SalvarVarias(aulasACriar);
                contadorAulasCriadas += aulasACriar.Count;
                aulasACriar.Clear();
            }
            return contadorAulasCriadas;
        }

        private async Task<int> ExcluirAulas(int contadorAulasExcluidas, List<long> idsAulasAExcluir, CancellationToken cancellationToken)
        {
            if (idsAulasAExcluir.PossuiRegistros())
            {
                foreach (var idAula in idsAulasAExcluir.Distinct())
                {
                    await mediator.Send(new ExcluirPendenciaAulaCommand(idAula, TipoPendencia.Frequencia), cancellationToken);
                    await mediator.Send(new ExcluirPendenciaDiarioPorAulaIdCommand(idAula), cancellationToken);
                }

                await repositorioAula.ExcluirPeloSistemaAsync(idsAulasAExcluir.ToArray());
                contadorAulasExcluidas += idsAulasAExcluir.Count;
                idsAulasAExcluir.Clear();
            }
            return contadorAulasExcluidas;
        }

        private static IEnumerable<Aula> ObterAulasParaExcluir(IEnumerable<DiaLetivoDto> diasNaoLetivos, Turma turma, IEnumerable<Aula> aulas, IEnumerable<DiaLetivoDto> diasLetivos)
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

                var naoEstaNosDiasLetivos = !diasLetivos.Any(d => (d.PossuiEventoSME || d.PossuiEventoDre(turma.Ue.Dre.CodigoDre) || d.PossuiEventoUe(turma.Ue.CodigoUe)) && d.Data == aula.DataAula);

                var excluirAula = ((diasNaoLetivos.NaoEhNulo() && diasNaoLetivos.Any(a => a.Data == aula.DataAula) &&
                                    naoEstaNosDiasLetivos && !aula.DadosComplementares.PossuiFrequencia) ||
                                    !turma.DataInicio.HasValue || aula.DataAula.Date < turma.DataInicio.Value.Date ||
                                    aulasMesmoDia.Any(a => a.Id < aula.Id && a.DadosComplementares.PossuiFrequencia) ||
                                    aulasMesmoDia.Any(a => a.Id > aula.Id && !aula.DadosComplementares.PossuiFrequencia) ||
                                    aulasMesmoDia.Any(a => a.Id < aula.Id && a.DadosComplementares.PossuiFrequencia && aula.DadosComplementares.PossuiFrequencia) ||
                                    aulasMesmoDia.Any(a => a.Id < aula.Id && !a.DadosComplementares.PossuiFrequencia && !aula.DadosComplementares.PossuiFrequencia));

                if (!excluirAula)
                    excluirAula = VerificaSeFoiAulaCriadaNoFimDeSemanaAutomaticaSemEventoLetivo(aula, diasLetivos);

                if (excluirAula)
                    aulasExclusao.Add(aula);
            }

            return aulasExclusao;
        }

        public static bool VerificaSeFoiAulaCriadaNoFimDeSemanaAutomaticaSemEventoLetivo(Aula aula, IEnumerable<DiaLetivoDto> diasLetivos)
            => aula.DataAula.FimDeSemana() && aula.CriadoPor.ToUpper() == AUDITORIA_SISTEMA && !diasLetivos.Any(d => d.Data == aula.DataAula);        

        private async Task<IEnumerable<(Aula aula, long? plano_aula_id)>> ObterAulasParaCriacao(CriarAulasInfantilERegenciaAutomaticamenteCommand request,
                                                                                                IEnumerable<DiaLetivoDto> diasDoPeriodo, 
                                                                                                IEnumerable<DiaLetivoDto> diasLetivos, 
                                                                                                IEnumerable<DiaLetivoDto> diasNaoLetivos, 
                                                                                                IEnumerable<Aula> aulasCriadasPeloSistema)
        {
            var diasParaCriar = diasDoPeriodo
                .Where(l => !l.Data.FimDeSemana() && (diasLetivos.NaoEhNulo() && diasLetivos.Any(n => n.Data == l.Data) || diasNaoLetivos.EhNulo() || !diasNaoLetivos.Any(n => n.Data == l.Data)))?
                .ToList();

            return await ObterListaDeAulas(diasParaCriar?.DistinctBy(c => c.Data)?.ToList(), 
                                           request.TipoCalendarioId, 
                                           request.Turma, 
                                           aulasCriadasPeloSistema, 
                                           request.DadosAulaCriadaAutomaticamente.ComponenteCurricular, 
                                           request.DadosAulaCriadaAutomaticamente.QuantidadeAulas, 
                                           request.DadosAulaCriadaAutomaticamente.RfProfessor);
        }

        private static IList<DiaLetivoDto> DeterminaDiasNaoLetivos(IEnumerable<DiaLetivoDto> diasDoPeriodo, Turma turma)
            => diasDoPeriodo.Where(c => c.ExcluirAulaSME || ((c.PossuiEventoDre(turma.Ue.Dre.CodigoDre) || c.PossuiEventoUe(turma.Ue.CodigoUe)) && c.EhNaoLetivo))?
                            .OrderBy(c => c.Data).ToList();
        private static List<DiaLetivoDto> FiltrarDiasAposInicioTurma(IEnumerable<DiaLetivoDto> diasParaCriarAula, Turma turma)
            => diasParaCriarAula
                    .Where(c => !turma.DataInicio.HasValue || c.Data.Date >= turma.DataInicio)?.ToList();

        private static List<DiaLetivoDto> FiltrarDiasLetivosSemAula(IEnumerable<DiaLetivoDto> diasLetivos, IEnumerable<Dominio.Aula> aulas, Turma turma)
            => diasLetivos
                    .Where(c => !aulas.Any(a => !a.Excluido && a.DataAula == c.Data) && (!turma.DataInicio.HasValue || c.Data.Date >= turma.DataInicio) && !c.Data.FimDeSemana())?
                    .OrderBy(a => a.Data)?
                    .Distinct()
                    .ToList();

        private static IList<DiaLetivoDto> DeterminaDiasLetivos(IEnumerable<DiaLetivoDto> diasDoPeriodo, IEnumerable<DateTime> diasNaoLetivos, Turma turma)
            => diasDoPeriodo.Where(c => !c.Data.FimDeSemana() && c.CriarAulaSME ||
                                        ((c.PossuiEventoDre(turma.Ue.Dre.CodigoDre) && !c.Data.FimDeSemana() || c.PossuiEventoUe(turma.Ue.CodigoUe)) && !c.Data.FimDeSemana() && c.EhLetivo) ||                                         
                                        ((c.NaoPossuiDre || c.NaoPossuiUe) && (!c.Data.FimDeSemana() || c.Data.FimDeSemana() && c.PossuiEvento) && c.EhLetivo && !diasNaoLetivos.Contains(c.Data.Date)) ||
                                        (c.EhNaoLetivo && c.UesIds.Any() && !c.PossuiEventoUe(turma.Ue.CodigoUe)))?
                            .OrderBy(c => c.Data)?
                            .ToList();

        private async Task<IEnumerable<(Aula aula, long? planoAulaId)>> ObterListaDeAulas(List<DiaLetivoDto> diasLetivos, long tipoCalendarioId, Turma turma, IEnumerable<Aula> aulasCriadasPeloSistema, (string id, string nome) dadosDisciplina, int quantidade, string rfProfessor)
        {
            var lista = new List<(Aula aula, long? planoAulaId)>();
            if (diasLetivos.PossuiRegistros())
            {
                foreach (var diaLetivo in diasLetivos)
                {
                    if (lista.Select(l => l.aula.DataAula.Date).Contains(diaLetivo.Data.Date)) continue;

                    var aulaExcluida = ObterAulaExcluidaDiaLetivo(aulasCriadasPeloSistema, diaLetivo);
                    if (aulaExcluida.NaoEhNulo())
                    {
                        var planoAula = await ObterPlanoAulaExcluidaDiaLetivo(aulasCriadasPeloSistema, aulaExcluida, diaLetivo);
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
                            ProfessorRf = rfProfessor,
                            CriadoPor = AUDITORIA_SISTEMA
                        }, null));
                    }
                }
            }

            return lista;
        }

        private Aula ObterAulaExcluidaDiaLetivo(IEnumerable<Aula> aulasCriadasPeloSistema, DiaLetivoDto diaLetivo)
        {
            return aulasCriadasPeloSistema
                        .Where(a => a.DataAula.Date.Equals(diaLetivo.Data.Date) && a.Excluido)
                        .OrderByDescending(a => a.CriadoEm)
                        .ThenBy(a => a.DadosComplementares.PossuiFrequencia)
                        .ThenBy(a => !a.DadosComplementares.RegistroFrequenciaExcluido)
                        .FirstOrDefault();
        }
        private async Task<PlanoAula> ObterPlanoAulaExcluidaDiaLetivo(IEnumerable<Aula> aulasCriadasPeloSistema, Aula aula, DiaLetivoDto diaLetivo)
        {
            var aulaComPlano = (from a in aulasCriadasPeloSistema
                                where a.DataAula.Date.Equals(diaLetivo.Data) &&
                                     !aula.DadosComplementares.PossuiPlanoAula &&
                                     a.Excluido &&
                                     a.DadosComplementares.PossuiPlanoAula &&
                                     !a.Id.Equals(aula.Id)
                                select a)
                                            .OrderByDescending(a => a.CriadoEm)
                                            .FirstOrDefault();

            return aulaComPlano.NaoEhNulo() ? (await mediator.Send(new ObterPlanoAulaPorAulaIdQuery(aulaComPlano.Id))) : null;
        }
    }
}
