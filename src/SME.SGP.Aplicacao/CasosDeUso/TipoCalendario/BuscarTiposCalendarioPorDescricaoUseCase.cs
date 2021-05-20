using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dto;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class BuscarTiposCalendarioPorDescricaoUseCase : IBuscarTiposCalendarioPorDescricaoUseCase
    {
        private readonly IMediator mediator;
        private readonly IConsultasAbrangencia consultasAbrangencia;

        public BuscarTiposCalendarioPorDescricaoUseCase(IMediator mediator, IConsultasAbrangencia consultasAbrangencia)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
            this.consultasAbrangencia = consultasAbrangencia ?? throw new System.ArgumentNullException(nameof(consultasAbrangencia));
        }

        public async Task<IEnumerable<TipoCalendarioBuscaDto>> Executar(string descricao)
        {
            var usuario = await mediator.Send(new ObterUsuarioLogadoQuery());

            if (usuario.EhPerfilUE() || usuario.EhPerfilProfessor())
            {
                var lstAbrangencia = new List<AbrangenciaFiltroRetorno>();

                var abrangencia = await mediator.Send(new ObterAbrangenciaPorFiltroQuery(string.Empty, false, usuario));

                if (abrangencia != null && abrangencia.Any())
                    lstAbrangencia.AddRange(abrangencia);

                var abrangenciaHistorica = await mediator.Send(new ObterAbrangenciaPorFiltroQuery(string.Empty, true, usuario));

                if (abrangenciaHistorica != null && abrangenciaHistorica.Any())
                    lstAbrangencia.AddRange(abrangenciaHistorica);

                var anosLetivosHistorico = await consultasAbrangencia.ObterAnosLetivos(true);
                var anosLetivos = await consultasAbrangencia.ObterAnosLetivos(false);
                int[] anosLetivosTipoCalendario = anosLetivosHistorico.Union(anosLetivos.ToArray()).ToArray();

                string[] codigosUes = lstAbrangencia.Select(a => a.CodigoUe)?.Distinct()?.ToArray();

                var modalidadesUes = await mediator.Send(new ObterModalidadesPorCodigosUeQuery(codigosUes));

                var modalidadesTipoCalendarioUes = modalidadesUes.Select(a => a == Modalidade.EJA ? (int)ModalidadeTipoCalendario.EJA :
                                                                              a == Modalidade.Infantil ? (int)ModalidadeTipoCalendario.Infantil :
                                                                                (int)ModalidadeTipoCalendario.FundamentalMedio).ToArray();             

                return await mediator.Send(new ObterTiposCalendariosPorAnosLetivoModalidadesQuery(anosLetivosTipoCalendario.Distinct().ToArray(), modalidadesTipoCalendarioUes));
            }
            else
                return await mediator.Send(new ObterTipoCalendarioPorBuscaQuery(descricao));
        }
    }
}
