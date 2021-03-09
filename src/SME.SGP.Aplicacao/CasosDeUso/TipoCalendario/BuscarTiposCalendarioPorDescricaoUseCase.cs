using MediatR;
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

        public BuscarTiposCalendarioPorDescricaoUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
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

                var anosLetivos = lstAbrangencia.Select(a => a.AnoLetivo)?.Distinct()?.ToArray();
                var modalidades = lstAbrangencia.Select(a => (int)a.ModalidadeTipoCalendario)?.Distinct()?.ToArray();

                return await mediator.Send(new ObterTiposCalendariosPorAnosLetivoModalidadesQuery(anosLetivos, modalidades));

            }
            else
                return await mediator.Send(new ObterTipoCalendarioPorBuscaQuery(descricao));
        }
    }
}
