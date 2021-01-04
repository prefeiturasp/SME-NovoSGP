using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ListarOcorrenciasQueryHandler : IRequestHandler<ListarOcorrenciasQuery, IEnumerable<OcorrenciaListagemDto>>
    {
        private readonly IRepositorioOcorrencia repositorioOcorrencia;

        public ListarOcorrenciasQueryHandler(IRepositorioOcorrencia repositorioOcorrencia)
        {
            this.repositorioOcorrencia = repositorioOcorrencia ?? throw new ArgumentNullException(nameof(repositorioOcorrencia));
        }

        public async Task<IEnumerable<OcorrenciaListagemDto>> Handle(ListarOcorrenciasQuery request, CancellationToken cancellationToken)
        {
            var listagem = await repositorioOcorrencia.Listar(request.Titulo, request.AlunoNome, request.DataOcorrenciaInicio, request.DataOcorrenciaFim);
            return listagem;
        }
    }
}
