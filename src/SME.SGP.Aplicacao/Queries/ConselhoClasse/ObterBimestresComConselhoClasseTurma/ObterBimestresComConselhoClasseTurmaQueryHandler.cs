using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterBimestresComConselhoClasseTurmaQueryHandler : IRequestHandler<ObterBimestresComConselhoClasseTurmaQuery, IEnumerable<BimestreComConselhoClasseTurmaDto>>
    {
        private readonly IRepositorioConselhoClasse repositorioConselhoClasse;
        public ObterBimestresComConselhoClasseTurmaQueryHandler(IRepositorioConselhoClasse repositorioConselhoClasse)
        {
            this.repositorioConselhoClasse = repositorioConselhoClasse;
        }
        public async Task<IEnumerable<BimestreComConselhoClasseTurmaDto>> Handle(ObterBimestresComConselhoClasseTurmaQuery request, CancellationToken cancellationToken)
        {
            return await repositorioConselhoClasse.ObterimestreComConselhoClasseTurmaAsync(request.Id);            
        }
    }
}
