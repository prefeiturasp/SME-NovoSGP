using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class VerificarPendenciaAulaDiasNaoLetivosCommandHandler : IRequestHandler<VerificarPendenciaAulaDiasNaoLetivosCommand, bool>
    {
        private readonly IMediator mediator;

        public VerificarPendenciaAulaDiasNaoLetivosCommandHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(VerificarPendenciaAulaDiasNaoLetivosCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var anoAtual = DateTime.Now.Year;
                var tipoCalendarioId = await mediator.Send(new ObterIdTipoCalendarioPorAnoLetivoEModalidadeQuery(Modalidade.Fundamental, anoAtual, 0));
                if (tipoCalendarioId > 0)
                    await VerificaPendenciasAulaDiasNaoLetivos(tipoCalendarioId, ModalidadeTipoCalendario.FundamentalMedio, anoAtual);

                tipoCalendarioId = await mediator.Send(new ObterIdTipoCalendarioPorAnoLetivoEModalidadeQuery(Modalidade.EJA, anoAtual, 1));
                if (tipoCalendarioId > 0)
                    await VerificaPendenciasAulaDiasNaoLetivos(tipoCalendarioId, ModalidadeTipoCalendario.EJA, anoAtual);

                tipoCalendarioId = await mediator.Send(new ObterIdTipoCalendarioPorAnoLetivoEModalidadeQuery(Modalidade.EJA, anoAtual, 2));
                if (tipoCalendarioId > 0)
                    await VerificaPendenciasAulaDiasNaoLetivos(tipoCalendarioId, ModalidadeTipoCalendario.EJA, anoAtual);

                return true;

            }
            catch (Exception ex)
            {
                await mediator.Send(new SalvarLogViaRabbitCommand($"Erro na verificação da pendência do calendário da UE.", LogNivel.Negocio, LogContexto.Aula, ex.Message));
                return false;
            }
        }

        private async Task VerificaPendenciasAulaDiasNaoLetivos(long tipoCalendarioId, ModalidadeTipoCalendario modalidadeCalendario, int anoAtual)
        {
            var periodosEscolares = await mediator.Send(new ObterPeridosEscolaresPorTipoCalendarioIdQuery(tipoCalendarioId));
            var diasLetivosENaoLetivos = await mediator.Send(new ObterDiasPorPeriodosEscolaresComEventosLetivosENaoLetivosQuery(periodosEscolares, tipoCalendarioId));
            var aulas = await mediator.Send(new ObterAulasReduzidaPorTipoCalendarioQuery(tipoCalendarioId, ObterTiposDeEscolasValidos()));

            var diasComEventosNaoLetivos = diasLetivosENaoLetivos.Where(e => e.EhNaoLetivo);

            if (aulas != null)
            {
                var listaAgrupada = aulas
                    .Where(a => diasComEventosNaoLetivos.Any(d => d.Data == a.Data && d.UesIds.Contains(a.CodigoUe)))
                    .GroupBy(x => new { x.TurmaId, x.DisciplinaId, x.ProfessorRf }).ToList();

                var motivos = diasComEventosNaoLetivos
                    .Where(d => aulas.Any(a => a.Data == d.Data && d.UesIds.Contains(a.CodigoUe)))
                    .Select(d => new { data = d.Data, motivo = d.Motivo, UesIds = d.UesIds }).ToList();

                foreach (var turmas in listaAgrupada)
                {
                    try
                    {
                        var pendenciaId = await mediator.Send(new ObterPendenciaAulaPorTurmaIdDisciplinaIdQuery(turmas.Key.TurmaId, turmas.Key.DisciplinaId, turmas.Key.ProfessorRf, TipoPendencia.AulaNaoLetivo));

                        var pendenciaExistente = pendenciaId != 0;

                        var ue = await mediator.Send(new ObterUEPorTurmaCodigoQuery(turmas.Key.TurmaId));

                        if (!pendenciaExistente)
                        {
                            pendenciaId = await mediator.Send(new SalvarPendenciaCommand(TipoPendencia.AulaNaoLetivo, 0, await ObterDescricao(turmas.FirstOrDefault(), TipoPendencia.AulaNaoLetivo), ObterInstrucoes()));
                            await mediator.Send(new SalvarPendenciaPerfilCommand(pendenciaId, ObterCodigoPerfis())); 
                            var professor = await mediator.Send(new ObterProfessorDaTurmaPorAulaIdQuery(turmas.FirstOrDefault().aulaId));

                            await mediator.Send(new SalvarPendenciaUsuarioCommand(pendenciaId, professor.Id));
                            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaTratarAtribuicaoPendenciaUsuarios, new FiltroTratamentoAtribuicaoPendenciaDto(pendenciaId, ue.Id), Guid.NewGuid()));
                        }

                        foreach (var aula in turmas)
                        {
                            var pendenciaAulaId = await mediator.Send(new ObterPendenciaAulaPorAulaIdQuery(aula.aulaId, TipoPendencia.AulaNaoLetivo));
                            if (pendenciaAulaId == 0)
                            {
                                var motivo = motivos.FirstOrDefault(m => m.data == aula.Data && m.UesIds.Contains(aula.CodigoUe))?.motivo;
                                await mediator.Send(new SalvarPendenciaAulaDiasNaoLetivosCommand(aula.aulaId, motivo, pendenciaId));
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        await mediator.Send(new SalvarLogViaRabbitCommand($"Erro na verificação da pendência do calendário da UE.", LogNivel.Negocio, LogContexto.Aula, ex.Message));
                    }
                }
            }
        }

        private List<PerfilUsuario> ObterCodigoPerfis()
                 => new List<PerfilUsuario> { PerfilUsuario.CP }; 

        private static TipoEscola[] ObterTiposDeEscolasValidos()
            => new[]
            {
                TipoEscola.EMEF,
                TipoEscola.EMEFM,
                TipoEscola.EMEBS,
                TipoEscola.CEUEMEF
            };

        private async Task<string> ObterDescricao(AulaReduzidaDto aula, TipoPendencia tipoPendencia)
        {
            var componenteCurricular = await ObterComponenteCurricular(long.Parse(aula.DisciplinaId));
            var mensagem = new StringBuilder();

            mensagem.AppendLine($"<i>{tipoPendencia.Name()}</i>");
            mensagem.AppendLine("<br />");
            mensagem.AppendLine($"<i>Componente Curricular: {componenteCurricular?.Nome ?? aula.DisciplinaId}</i><br />");
            mensagem.AppendLine($"<i>Professor: {aula.Professor}({aula.ProfessorRf})</i><br />");

            return mensagem.ToString();
        }

        private string ObterInstrucoes()
            => "Você precisa excluir estas aulas no Calendário do Professor ou entrar em contato com a gestão da UE para ajustar o calendário da escola.";

        private async Task<DisciplinaDto> ObterComponenteCurricular(long componenteCurricularId)
        {
            var componentes = await mediator.Send(new ObterComponentesCurricularesPorIdsQuery(new[] { componenteCurricularId }));
            return componentes.FirstOrDefault();
        }
    }
}
