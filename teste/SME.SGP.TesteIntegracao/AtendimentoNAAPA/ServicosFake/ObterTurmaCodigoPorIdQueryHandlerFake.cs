using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmaCodigoPorIdQueryHandlerFakeNAAPA : IRequestHandler<ObterTurmaCodigoPorIdQuery, string>
    {
        private readonly IRepositorioTurmaConsulta repositorioTurma;

        public ObterTurmaCodigoPorIdQueryHandlerFakeNAAPA(IRepositorioTurmaConsulta repositorioTurma)
        {
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
        }

        public async Task<string> Handle(ObterTurmaCodigoPorIdQuery request, CancellationToken cancellationToken)
        {
            var turmas = new List<Turma>()
              {
                  new Turma() {
                      Id = 1,
                      CodigoTurma = "1",
                  }              
            };

            return await Task.Run(() => turmas.Where(turma => turma.Id == request.TurmaId).FirstOrDefault().CodigoTurma);
        }
    }
}
