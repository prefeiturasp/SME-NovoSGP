using MediatR;
using Org.BouncyCastle.Ocsp;
using Sentry;
using SME.SGP.Aplicacao.Queries.Aula.ObterAulasDaTurma;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
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
            var diasParaExcluirAula = request.DiasLetivos;
            var aulasACriar = new List<Aula>();
            var aulasAExcluir = new List<Aula>();
            var aulasComErro = new List<Aula>();
            var aulasAExcluirComFrequenciaRegistrada = new List<DateTime>();
            var turmas = request.Turmas;

            var contadorAulasCriadas = 0;
            var contadorAulasExcluidas = 0;

            var idsAulasAExcluir = new List<long>();
            foreach (var turma in turmas)
            {
                var aulas = await mediator.Send(new ObterAulasDaTurmaPorTipoCalendarioQuery(turma.CodigoTurma, tipoCalendarioId));
                if (aulas == null)
                    aulasACriar.AddRange(ObterAulasParaCriacao(tipoCalendarioId, diasParaCriarAula, turma));
                else
                {
                    if (!aulas.Any())
                        aulasACriar.AddRange(ObterAulasParaCriacao(tipoCalendarioId, diasParaCriarAula, turma));
                    else
                    {
                        var diasSemAula = diasParaCriarAula.Where(c => !aulas.Any(a => a.DataAula == c.Data && !a.Excluido));

                        aulasACriar.AddRange(ObterAulasParaCriacao(tipoCalendarioId, diasSemAula, turma));
                        IEnumerable<Aula> aulasDaTurmaParaExcluir = ObterAulasParaExcluir(diasParaExcluirAula, turma, aulas);
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

                if (idsAulasAExcluir.Count >= 1000)
                    contadorAulasExcluidas = await ExcluirAulas(contadorAulasExcluidas, idsAulasAExcluir);

                if (aulasAExcluirComFrequenciaRegistrada.Any())
                {
                    await mediator.Send(new PublicarFilaSgpCommand(RotasRabbit.RotaNotificacaoExclusaoAulasComFrequencia,
                        new NotificarExclusaoAulasComFrequenciaDto(turma, aulasAExcluirComFrequenciaRegistrada), Guid.NewGuid(), null));

                    //await mediator.Send(new NotificarExclusaoAulaComFrequenciaCommand(turma, aulasAExcluirComFrequenciaRegistrada));
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
            return true;
        }

        private int CriarAulas(List<Aula> aulasACriar, int contadorAulasCriadas)
        {
            repositorioAula.SalvarVarias(aulasACriar);
            contadorAulasCriadas = contadorAulasCriadas + aulasACriar.Count;
            Console.WriteLine($"Criadas {contadorAulasCriadas} aulas");
            aulasACriar.Clear();
            return contadorAulasCriadas;
        }

        private async Task<int> ExcluirAulas(int contadorAulasExcluidas, List<long> idsAulasAExcluir)
        {
            await repositorioAula.ExcluirPeloSistemaAsync(idsAulasAExcluir.ToArray());
            contadorAulasExcluidas = contadorAulasExcluidas + idsAulasAExcluir.Count;
            Console.WriteLine($"Excluidas {contadorAulasExcluidas} aulas");
            idsAulasAExcluir.Clear();
            return contadorAulasExcluidas;
        }
        private static IEnumerable<Aula> ObterAulasParaExcluir(IEnumerable<DiaLetivoDto> diasParaExcluirAula, Turma turma, IEnumerable<Aula> aulas)
        {
            var diasDaUe = diasParaExcluirAula.Where(c => !c.EhLetivo && (c.UesIds.Contains(turma.Ue.CodigoUe) || !c.UesIds.Any()));
            return aulas.Where(c => diasDaUe.Any(a => a.Data == c.DataAula && !c.Excluido));
        }


        private IEnumerable<Aula> ObterAulasParaCriacao(long tipoCalendarioId, IEnumerable<DiaLetivoDto> diasParaCriarAula, Turma turma)
        {
            var diasDaUe = diasParaCriarAula.Where(c => c.EhLetivo && c.UesIds.Contains(turma.Ue.CodigoUe));
            if (diasDaUe.Any())
            {
                return ObterDiasDeAulas(diasDaUe, tipoCalendarioId, turma);
            }
            return ObterDiasDeAulas(diasParaCriarAula.Where(c => c.EhLetivo && !c.UesIds.Any()), tipoCalendarioId, turma);
        }

        private IEnumerable<Aula> ObterDiasDeAulas(IEnumerable<DiaLetivoDto> diaLetivos, long tipoCalendarioId, Turma turma)
        {
            return diaLetivos.Select(c => new Aula
            {
                DataAula = c.Data,
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
}
