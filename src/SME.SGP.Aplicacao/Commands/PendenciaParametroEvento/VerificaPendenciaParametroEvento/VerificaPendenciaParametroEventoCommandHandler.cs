using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class VerificaPendenciaParametroEventoCommandHandler : IRequestHandler<VerificaPendenciaParametroEventoCommand, bool>
    {
        private readonly IMediator mediator;

        public VerificaPendenciaParametroEventoCommandHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(VerificaPendenciaParametroEventoCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var anoAtual = DateTime.Now.Year;

                var dataInicioGeracaoPendencia = DateTime.Parse(await mediator.Send(new ObterValorParametroSistemaTipoEAnoQuery(Dominio.TipoParametroSistema.DataInicioGeracaoPendencias, anoAtual)));
                if (DateTime.Now >= dataInicioGeracaoPendencia)
                {
                    var tipoCalendarioId = await mediator.Send(new ObterIdTipoCalendarioPorAnoLetivoEModalidadeQuery(Dominio.Modalidade.Fundamental, anoAtual, 0));
                    if (tipoCalendarioId > 0)
                        await VerificaPendenciaEventosCalendario(tipoCalendarioId, anoAtual);
                }

                return true;
            }
            catch (Exception ex)
            {
                await mediator.Send(new SalvarLogViaRabbitCommand($"Erro na verificação da pendência do calendário da UE.", LogNivel.Negocio, LogContexto.Evento, ex.Message));
                return false;
            }
        }

        private async Task VerificaPendenciaEventosCalendario(long tipoCalendarioId, int anoAtual)
        {
            var ues = await mediator.Send(new ObterUEsPorModalidadeCalendarioQuery(ModalidadeTipoCalendario.FundamentalMedio));
            var tiposEscolasValidos = ObterTiposDeEscolasValidos();
            ues = ues?.Where(ue => tiposEscolasValidos.Contains(ue.TipoEscola)).ToList();

            if (ues != null)
            {
                foreach (var ue in ues)
                {
                    try
                    {
                        var pendenciaCalendarioUe = await ObterPendenciaCalendarioUe(tipoCalendarioId, ue.Id);
                        var pendenciasParametroEventoUe = await ObterPendenciasParametroEventoPorPendenciaId(pendenciaCalendarioUe?.PendenciaId);

                        var listaValidacoesEvento = new List<(bool gerarPedencia, long parametroSistemaId, int quantidadeEventos)>();
                        listaValidacoesEvento.Add(await ValidarQuantidadeEventosPorTipo(tipoCalendarioId, ue, anoAtual, pendenciasParametroEventoUe, TipoEvento.ConselhoDeClasse));
                        listaValidacoesEvento.Add(await ValidarQuantidadeEventosPorTipo(tipoCalendarioId, ue, anoAtual, pendenciasParametroEventoUe, TipoEvento.ReuniaoAPM));
                        listaValidacoesEvento.Add(await ValidarQuantidadeEventosPorTipo(tipoCalendarioId, ue, anoAtual, pendenciasParametroEventoUe, TipoEvento.ReuniaoConselhoEscola));
                        listaValidacoesEvento.Add(await ValidarQuantidadeEventosPorTipo(tipoCalendarioId, ue, anoAtual, pendenciasParametroEventoUe, TipoEvento.ReuniaoPedagogica));

                        if (listaValidacoesEvento.Any(a => a.gerarPedencia))
                        {
                            var pendenciaCalendarioUeId = pendenciaCalendarioUe.EhNulo() ? await GerarPendenciaCalendarioUe(tipoCalendarioId, ue) : pendenciaCalendarioUe?.Id ?? 0;

                            await GerarPendenciaParametroEvento(pendenciaCalendarioUeId, listaValidacoesEvento.Where(a => a.gerarPedencia));
                        }
                    }
                    catch (Exception ex)
                    {
                        await mediator.Send(new SalvarLogViaRabbitCommand($"Erro na verificação da pendência do calendário da UE.", LogNivel.Negocio, LogContexto.Evento, ex.Message));
                    }
                }
            }
        }

        private static TipoEscola[] ObterTiposDeEscolasValidos()
            => new[]
            {
                TipoEscola.EMEF,
                TipoEscola.EMEFM,
                TipoEscola.EMEBS,
                TipoEscola.CEUEMEF
            };

        private async Task<PendenciaCalendarioUe> ObterPendenciaCalendarioUe(long tipoCalendarioId, long ueId)
        {
            var pendenciasCalendario = await mediator.Send(new ObterPendenciasCalendarioUeQuery(tipoCalendarioId, ueId, TipoPendencia.CadastroEventoPendente));

            return pendenciasCalendario.NaoEhNulo() && pendenciasCalendario.Any() ?
                    pendenciasCalendario.First() : null;
        }

        private async Task<long> GerarPendenciaCalendarioUe(long tipoCalendarioId, Ue ue)
        {
            var nomeTipoCalendario = await mediator.Send(new ObterNomeTipoCalendarioPorIdQuery(tipoCalendarioId));
            var descricao = new StringBuilder();

            descricao.AppendLine($"<i>DRE:</i><b> DRE - {ue.Dre.Abreviacao}</b><br />");
            descricao.AppendLine($"<i>UE:</i><b> {ue.TipoEscola.ShortName()} - {ue.Nome}</b><br />");
            descricao.AppendLine($"<i>Calendário:</i><b> {nomeTipoCalendario}</b><br />");
            descricao.AppendLine($"<i>Eventos pendentes de cadastro:</i><br />");

            var instrucao = "Acesse a tela de Eventos e realize o cadastro dos eventos relatados acima.";

            return await mediator.Send(new SalvarPendenciaCalendarioUeCommand(tipoCalendarioId, ue, descricao.ToString(), instrucao, TipoPendencia.CadastroEventoPendente));
        }

        private async Task<IEnumerable<PendenciaParametroEventoDto>> ObterPendenciasParametroEventoPorPendenciaId(long? pendenciaId)
        {
            var pendenciasParametrosEvento = Enumerable.Empty<PendenciaParametroEventoDto>();
            if (pendenciaId.HasValue && pendenciaId > 0)
                pendenciasParametrosEvento = await mediator.Send(new ObterPendenciasParametroEventoPorPendenciaQuery(pendenciaId.Value));

            return pendenciasParametrosEvento;
        }

        private async Task<(bool gerarPedencia, long parametroSistemaId, int quantidadeEventos)> ValidarQuantidadeEventosPorTipo(long tipoCalendarioId, Ue ue, int anoAtual, IEnumerable<PendenciaParametroEventoDto> pendenciasParametroEventoUe, TipoEvento tipoEvento)
        {
            var parametroQuantidadeEventos = await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(ObterTipoParametroPorTipoEvento(tipoEvento), anoAtual));
            var eventos = await mediator.Send(new ObterEventosPorTipoECalendarioUeQuery(tipoCalendarioId, ue.CodigoUe, tipoEvento));

            if (EventosInsuficientes(eventos, int.Parse(parametroQuantidadeEventos.Valor)))
            {
                var pendenciaParametroEvento = pendenciasParametroEventoUe.FirstOrDefault(c => c.ParametroSistemaId == parametroQuantidadeEventos.Id);
                return (pendenciaParametroEvento.EhNulo(), parametroQuantidadeEventos.Id, int.Parse(parametroQuantidadeEventos.Valor) - eventos.Count());
            }

            return (false, 0, 0);
        }

        private bool EventosInsuficientes(IEnumerable<Evento> eventos, int quantidadeEventosParametro)
        {
            return eventos.EhNulo()
                || eventos.Count() < quantidadeEventosParametro;
        }

        private TipoParametroSistema ObterTipoParametroPorTipoEvento(TipoEvento tipoEvento)
        {
            switch (tipoEvento)
            {
                case TipoEvento.ConselhoDeClasse:
                    return TipoParametroSistema.QuantidadeEventosConselhoClasse;
                case TipoEvento.ReuniaoAPM:
                    return TipoParametroSistema.QuantidadeEventosAPM;
                case TipoEvento.ReuniaoConselhoEscola:
                    return TipoParametroSistema.QuantidadeEventosConselhoEscolar;
                case TipoEvento.ReuniaoPedagogica:
                    return TipoParametroSistema.QuantidadeEventosPedagogicos;
                default:
                    throw new NegocioException("Tipo de evento não relacionado com tipo de parâmetro do sistema!");
            }
        }

        private async Task GerarPendenciaParametroEvento(long pendenciaCalendarioUeId, IEnumerable<(bool gerarPedencia, long parametroSistemaId, int quantidadeEventos)> pendenciasEventos)
        {
            foreach (var pendenciaEvento in pendenciasEventos)
                await mediator.Send(new SalvarPendenciaParametroEventoCommand(pendenciaCalendarioUeId, pendenciaEvento.parametroSistemaId, pendenciaEvento.quantidadeEventos)); ;
        }
    }
}
