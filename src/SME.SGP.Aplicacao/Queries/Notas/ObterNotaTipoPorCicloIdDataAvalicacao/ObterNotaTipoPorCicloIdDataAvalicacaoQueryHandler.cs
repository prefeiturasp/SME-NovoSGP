using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;

namespace SME.SGP.Aplicacao
{
    public class ObterNotaTipoPorCicloIdDataAvalicacaoQueryHandler : IRequestHandler<ObterNotaTipoPorCicloIdDataAvalicacaoQuery,NotaTipoValor>
    {
        private readonly IRepositorioNotaTipoValorConsulta repositorioConsulta;

        public ObterNotaTipoPorCicloIdDataAvalicacaoQueryHandler(IRepositorioNotaTipoValorConsulta consulta)
        {
            repositorioConsulta = consulta ?? throw new ArgumentNullException(nameof(consulta));
        }

        public async Task<NotaTipoValor> Handle(ObterNotaTipoPorCicloIdDataAvalicacaoQuery request, CancellationToken cancellationToken)
        {
            return await repositorioConsulta.ObterPorCicloIdDataAvalicacao(request.CicloId,request.DataAvalicao);
        }
    }
}