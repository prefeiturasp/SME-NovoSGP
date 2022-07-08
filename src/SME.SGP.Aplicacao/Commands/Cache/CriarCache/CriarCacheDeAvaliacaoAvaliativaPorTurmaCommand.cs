using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class CriarCacheDeAvaliacaoAvaliativaPorTurmaCommand : IRequest<IEnumerable<NotaConceito>>
    {
        public CriarCacheDeAvaliacaoAvaliativaPorTurmaCommand(string codigoTurma, string nomeChave)
        {
            CodigoTurma = codigoTurma;
            NomeChave = nomeChave;
        }
        public string CodigoTurma { get; set; }
        public string NomeChave { get; set; }
    }
}
