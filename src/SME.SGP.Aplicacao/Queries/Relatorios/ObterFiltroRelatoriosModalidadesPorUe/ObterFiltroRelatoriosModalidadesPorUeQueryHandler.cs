﻿using System;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SME.SGP.Dominio.Enumerados;

namespace SME.SGP.Aplicacao
{
    public class ObterFiltroRelatoriosModalidadesPorUeQueryHandler : IRequestHandler<ObterFiltroRelatoriosModalidadesPorUeQuery, IEnumerable<OpcaoDropdownDto>>
    {
        private readonly IRepositorioAbrangencia repositorioAbrangencia;
        private readonly IMediator mediator;

        public ObterFiltroRelatoriosModalidadesPorUeQueryHandler(IRepositorioAbrangencia repositorioAbrangencia,IMediator mediator)
        {
            this.repositorioAbrangencia = repositorioAbrangencia ?? throw new System.ArgumentNullException(nameof(repositorioAbrangencia));
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<OpcaoDropdownDto>> Handle(ObterFiltroRelatoriosModalidadesPorUeQuery request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.CodigoUe == "-99")
                {
                    var todasAsModalidades = EnumExtensao.ListarDto<Modalidade>();
                    if (request.ModalidadesQueSeraoIgnoradas.NaoEhNulo() && request.ModalidadesQueSeraoIgnoradas.Any())
                    {
                        var idsIgnoradas = request.ModalidadesQueSeraoIgnoradas.Select(a => (int)a);
                        var listaTratada = todasAsModalidades.Where(m => !idsIgnoradas.Contains(m.Id));
                        return listaTratada.Select(c => new OpcaoDropdownDto(c.Id.ToString(), c.Descricao));
                    }
                    return todasAsModalidades.Select(c => new OpcaoDropdownDto(c.Id.ToString(), c.Descricao));
                }

                var listaAbrangencia = await repositorioAbrangencia.ObterModalidades(request.Login, request.Perfil, request.AnoLetivo, request.ConsideraHistorico, request.ModalidadesQueSeraoIgnoradas);

                var modalidades = await repositorioAbrangencia.ObterModalidadesPorUe(request.CodigoUe);

                return modalidades?.Where(m => listaAbrangencia.Contains((int)m))?.Select(c => new OpcaoDropdownDto(((int)c).ToString(), c.Name()));
            }
            catch (Exception ex)
            {
                await mediator.Send(new SalvarLogViaRabbitCommand($"Erro ao executar ObterFiltroRelatoriosModalidadesPorUeQueryHandler", LogNivel.Critico, LogContexto.Geral, ex.Message));
                throw;
            }
        }
    }
}
