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
            var usuario = await mediator.Send(ObterUsuarioLogadoQuery.Instance);
            if (!PossuiPermissao(usuario))
                return await mediator.Send(new ObterTiposCalendariosPorBuscaQuery(descricao));

            var lstAbrangencia = new List<AbrangenciaFiltroRetorno>();

            var abrangencia = await mediator.Send(new ObterAbrangenciaPorFiltroQuery(string.Empty, false, usuario));
            lstAbrangencia.AddRange(abrangencia);

            var abrangenciaHistorica = await mediator.Send(new ObterAbrangenciaPorFiltroQuery(string.Empty, true, usuario));
            lstAbrangencia.AddRange(abrangenciaHistorica);

            //TODO: MELHORAR ISSO AQUI
            var anosLetivosHistorico = await consultasAbrangencia.ObterAnosLetivos(true, 0);
            var anosLetivos = await consultasAbrangencia.ObterAnosLetivos(false, 0);
            int[] anosLetivosTipoCalendario = anosLetivosHistorico.Union(anosLetivos.ToArray()).ToArray();    

            string[] codigosUes = lstAbrangencia.Select(a => a.CodigoUe)?.Distinct()?.ToArray();

            var modalidadesUes = await mediator.Send(new ObterModalidadesPorCodigosUeQuery(codigosUes));

            var modalidadesTipoCalendarioUes = modalidadesUes.Select(a => (int)a.ObterModalidadeTipoCalendario()).ToArray();             
            return await mediator.Send(new ObterTiposCalendariosPorAnosLetivoModalidadesQuery(anosLetivosTipoCalendario.Distinct().ToArray(), modalidadesTipoCalendarioUes, descricao));
        }

        private bool PossuiPermissao(Usuario usuario) => usuario.EhPerfilUE() || usuario.EhPerfilProfessor() || usuario.EhCCELP();
    }
}
