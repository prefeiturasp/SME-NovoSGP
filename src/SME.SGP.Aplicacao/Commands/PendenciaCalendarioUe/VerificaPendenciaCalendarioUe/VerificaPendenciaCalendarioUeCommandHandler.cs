using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class VerificaPendenciaCalendarioUeCommandHandler : IRequestHandler<VerificaPendenciaCalendarioUeCommand, bool>
    {
        private readonly IMediator mediator;

        public VerificaPendenciaCalendarioUeCommandHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(VerificaPendenciaCalendarioUeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var anoAtual = DateTime.Now.Year;
                var tipoCalendarioId = await mediator.Send(new ObterIdTipoCalendarioPorAnoLetivoEModalidadeQuery(Dominio.Modalidade.Fundamental, anoAtual, 0));
                if (tipoCalendarioId > 0)
                    await VerificaPendenciasNoCalendario(tipoCalendarioId, Dominio.ModalidadeTipoCalendario.FundamentalMedio);

                tipoCalendarioId = await mediator.Send(new ObterIdTipoCalendarioPorAnoLetivoEModalidadeQuery(Dominio.Modalidade.EJA, anoAtual, 1));
                if (tipoCalendarioId > 0)
                    await VerificaPendenciasNoCalendario(tipoCalendarioId, Dominio.ModalidadeTipoCalendario.EJA);

                tipoCalendarioId = await mediator.Send(new ObterIdTipoCalendarioPorAnoLetivoEModalidadeQuery(Dominio.Modalidade.EJA, anoAtual, 2));
                if (tipoCalendarioId > 0)
                    await VerificaPendenciasNoCalendario(tipoCalendarioId, Dominio.ModalidadeTipoCalendario.EJA);

                return true;
            }
            catch (Exception ex)
            {
                await mediator.Send(new SalvarLogViaRabbitCommand($"Erro na verificação da pendência do calendário da UE.", LogNivel.Negocio, LogContexto.Calendario, ex.Message));
                return false;
            }
        }

        private async Task VerificaPendenciasNoCalendario(long tipoCalendarioId, ModalidadeTipoCalendario modalidadeCalendario)
        {
            var ues = await mediator.Send(new ObterUEsPorModalidadeCalendarioQuery(modalidadeCalendario));
            var tiposEscolasValidos = ObterTiposDeEscolasValidos();
            ues = ues?.Where(ue => tiposEscolasValidos.Contains(ue.TipoEscola)).ToList();

            foreach (var ue in ues)
            {
                try
                {
                    if (!(await mediator.Send(new ExistePendenciaDiasLetivosCalendarioUeQuery(tipoCalendarioId, ue.Id))))
                    {
                        var diasLetivos = await mediator.Send(new ObterQuantidadeDiasLetivosPorCalendarioQuery(tipoCalendarioId, ue.Dre.CodigoDre, ue.CodigoUe));
                        if (diasLetivos.EstaAbaixoPermitido)
                            await GerarPendenciaCalendarioUe(ue, diasLetivos.Dias, tipoCalendarioId);
                    }
                }
                catch (Exception ex)
                {
                    await mediator.Send(new SalvarLogViaRabbitCommand($"Erro na verificação da pendência do calendário da UE.", LogNivel.Negocio, LogContexto.Calendario, ex.Message));
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

        private async Task GerarPendenciaCalendarioUe(Ue ue, int diasLetivos, long tipoCalendarioId)
        {
            var nomeTipoCalendario = await mediator.Send(new ObterNomeTipoCalendarioPorIdQuery(tipoCalendarioId));

            var descricao = new StringBuilder();
            descricao.AppendLine($"<i>DRE:</i><b> DRE - {ue.Dre.Abreviacao}</b><br />");
            descricao.AppendLine($"<i>UE:</i><b> {ue.TipoEscola.ShortName()} - {ue.Nome}</b><br />");
            descricao.AppendLine($"<i>Calendário:</i><b> {nomeTipoCalendario}</b><br />");
            descricao.AppendLine($"<i>Quantidade de dias letivos:</i><b> {diasLetivos}</b><br />");

            var instrucao = "Acesse a tela de Calendário Escolar e confira os eventos da sua UE.";

            await mediator.Send(new SalvarPendenciaCalendarioUeCommand(tipoCalendarioId, ue, descricao.ToString(), instrucao, TipoPendencia.CalendarioLetivoInsuficiente));

        }
    }
}
