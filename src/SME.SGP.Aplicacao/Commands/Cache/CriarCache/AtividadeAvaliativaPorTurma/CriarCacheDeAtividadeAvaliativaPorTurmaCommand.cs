using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class CriarCacheDeAtividadeAvaliativaPorTurmaCommand : IRequest<IEnumerable<NotaConceito>>
    {
        public CriarCacheDeAtividadeAvaliativaPorTurmaCommand(string codigoTurma)
        {
            CodigoTurma = codigoTurma;
        }

        public string CodigoTurma { get; }
    }
}
