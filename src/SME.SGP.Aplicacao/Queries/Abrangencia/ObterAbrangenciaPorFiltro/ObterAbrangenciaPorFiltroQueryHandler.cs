using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAbrangenciaPorFiltroQueryHandler : IRequestHandler<ObterAbrangenciaPorFiltroQuery, IEnumerable<AbrangenciaFiltroRetorno>>
    {
        private readonly IRepositorioAbrangencia repositorioAbrangencia;

        public ObterAbrangenciaPorFiltroQueryHandler(IRepositorioAbrangencia repositorioAbrangencia)
        {
            this.repositorioAbrangencia = repositorioAbrangencia ?? throw new ArgumentNullException(nameof(repositorioAbrangencia));
        }

        public async Task<IEnumerable<AbrangenciaFiltroRetorno>> Handle(ObterAbrangenciaPorFiltroQuery request, CancellationToken cancellationToken)
        {
            var retorno = await repositorioAbrangencia.ObterAbrangenciaPorFiltro(request.Texto, request.Usuario.Login, request.Usuario.PerfilAtual, request.ConsideraHistorico);

            return retorno ?? throw new NegocioException("Não foi encontrada a abrangência do usuário informado");
        }
    }
}
