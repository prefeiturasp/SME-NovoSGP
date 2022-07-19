using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class CriarCacheAulaPrevistaCommand : IRequest<IEnumerable<AulaPrevista>>
    {
        public CriarCacheAulaPrevistaCommand(string nomeChave, long codigoUe)
        {
            NomeChave = nomeChave;
            CodigoUe = codigoUe;
        }

        public long CodigoUe { get; set; }
        public string NomeChave { get; set; }
    }
}