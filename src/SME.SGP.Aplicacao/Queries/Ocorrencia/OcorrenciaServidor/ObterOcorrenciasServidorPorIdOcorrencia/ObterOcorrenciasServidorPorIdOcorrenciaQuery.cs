using System.Collections.Generic;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ObterOcorrenciasServidorPorIdOcorrenciaQuery : IRequest<IEnumerable<Dominio.OcorrenciaServidor>>
    {
        public  long IdOcorrencia { get; set; }

        public ObterOcorrenciasServidorPorIdOcorrenciaQuery(long idOcorrencia)
        {
            IdOcorrencia = idOcorrencia;
        }
    }
}