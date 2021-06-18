using MediatR;
using Sentry;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class TrataSincronizacaoInstitucionalTurmaCommandHandler : IRequestHandler<TrataSincronizacaoInstitucionalTurmaCommand, bool>
    {
        private readonly IRepositorioTurma repositorioTurma;
        private readonly IMediator mediator;

        public TrataSincronizacaoInstitucionalTurmaCommandHandler(IRepositorioTurma repositorioTurma, IMediator mediator)
        {
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(TrataSincronizacaoInstitucionalTurmaCommand request, CancellationToken cancellationToken)
        {
            var turmaEOL = request.TurmaEOL;

            if (request.TurmaSGP == null)
                return await IncluirTurmaAsync(turmaEOL, request.TurmaSGP);

            if (turmaEOL.Situacao == "C")
            {
                return await AtualizarTurmaParaHistoricaAsync(turmaEOL.Codigo.ToString());
            }

            if (turmaEOL.Situacao == "E")
                return await VerificarTurmaExtintaAsync(turmaEOL, request.TurmaSGP.Id);

            if (turmaEOL.Situacao == "O" || turmaEOL.Situacao == "A")
                return await IncluirTurmaAsync(turmaEOL, request.TurmaSGP);

            return true;
        }

        private async Task<bool> VerificarTurmaExtintaAsync(TurmaParaSyncInstitucionalDto turma, long turmaSgpId)
        {
            var anoAtual = DateTime.Now.Year;
            var tipoCalendarioId = await mediator.Send(new ObterIdTipoCalendarioPorAnoLetivoEModalidadeQuery(turma.CodigoModalidade, anoAtual, turma.Semestre));

            if (tipoCalendarioId > 0)
            {
                var periodosEscolares = await mediator.Send(new ObterPeriodosEscolaresPorTipoCalendarioIdQuery(tipoCalendarioId));
                if (periodosEscolares != null && periodosEscolares.Any())
                {
                    var primeiroPeriodo = periodosEscolares.OrderBy(x => x.Bimestre).First();

                    if (turma.DataStatusTurmaEscola.Date < primeiroPeriodo.PeriodoInicio.Date)
                    {
                        await ExcluirTurnaAsync(turma.Codigo.ToString(), turmaSgpId);
                        return true;
                    }
                    else
                    {
                        return await repositorioTurma.AtualizarTurmaSincronizacaoInstitucionalAsync(turma, true);
                    }
                }
                else
                {
                    await ExcluirTurnaAsync(turma.Codigo.ToString(), turmaSgpId);
                    return true;
                }
            }
            else
            {
                await ExcluirTurnaAsync(turma.Codigo.ToString(), turmaSgpId);
                return true;
            }
        }

        private async Task<bool> AtualizarTurmaParaHistoricaAsync(string turmaId)
        {
            var turmaAtualizada = await repositorioTurma.AtualizarTurmaParaHistorica(turmaId);

            if (!turmaAtualizada)
            {
                SentrySdk.CaptureMessage($"Não foi possível atualizar a turma id {turmaId} para histórica.");
                return false;
            }
            return true;
        }

        private async Task<bool> IncluirTurmaAsync(TurmaParaSyncInstitucionalDto turmaEol, Turma turmaSgp)
        {
            if (turmaSgp == null)
            {
                var ue = await mediator.Send(new ObterUeComDrePorCodigoQuery(turmaEol.UeCodigo));

                if (ue == null)
                {
                    SentrySdk.CaptureMessage($"Não foi possível Incluir a turma de código {turmaEol.Codigo}. Pois não foi encontrado a UE {turmaEol.UeCodigo}.");
                    return false;
                }
                await repositorioTurma.SalvarAsync(turmaEol, ue.Id);
            }
            else
            {
                if (turmaSgp.Nome != turmaEol.NomeTurma ||
                   turmaSgp.Ano != turmaEol.Ano ||
                   (int)turmaSgp.TipoTurma != turmaEol.TipoTurma ||
                   turmaSgp.AnoLetivo != turmaEol.AnoLetivo ||
                   turmaSgp.ModalidadeCodigo != turmaEol.CodigoModalidade ||
                   turmaSgp.Semestre != turmaEol.Semestre ||
                   turmaSgp.QuantidadeDuracaoAula != turmaEol.DuracaoTurno ||
                   turmaSgp.TipoTurno != turmaEol.TipoTurno ||
                   turmaSgp.EnsinoEspecial != turmaEol.EnsinoEspecial ||
                   turmaSgp.EtapaEJA != turmaEol.EtapaEJA ||
                   turmaSgp.SerieEnsino != turmaEol.SerieEnsino ||
                   turmaSgp.DataInicio.HasValue != turmaEol.DataInicioTurma.HasValue ||
                   (turmaSgp.DataInicio.HasValue && turmaEol.DataInicioTurma.HasValue && turmaSgp.DataInicio.Value.Date != turmaEol.DataInicioTurma.Value.Date) ||
                   turmaSgp.DataFim.HasValue != turmaEol.DataFim.HasValue ||
                   (turmaSgp.DataFim.HasValue && turmaEol.DataFim.HasValue && turmaSgp.DataFim.Value.Date != turmaEol.DataFim.Value.Date) ||
                   turmaSgp.NomeFiltro != turmaEol.NomeFiltro)


                {
                    await repositorioTurma.AtualizarTurmaSincronizacaoInstitucionalAsync(turmaEol);
                }

            }
            return true;
        }

        private async Task ExcluirTurnaAsync(string turmaCodigo, long turmaId)
            => await repositorioTurma.ExcluirTurmaExtintaAsync(turmaCodigo, turmaId);
    }
}
