using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAbrangenciaTurmasCPParaCopiaAvaliacaoQueryHandler : IRequestHandler<ObterAbrangenciaTurmasCPParaCopiaAvaliacaoQuery, IEnumerable<Turma>>
    {
        private readonly IRepositorioAbrangencia repositorioAbrangencia;

        public ObterAbrangenciaTurmasCPParaCopiaAvaliacaoQueryHandler(IRepositorioAbrangencia repositorioAbrangencia)
        {
            this.repositorioAbrangencia = repositorioAbrangencia ?? throw new ArgumentNullException(nameof(repositorioAbrangencia));
        }
        public Task<IEnumerable<Turma>> Handle(ObterAbrangenciaTurmasCPParaCopiaAvaliacaoQuery request, CancellationToken cancellationToken)
            => repositorioAbrangencia.ObterTurmasPorAbrangenciaCPParaCopiaAvaliacao(request.AnoLetivo, request.CodigoRf, request.ModalidadeTurma, request.Ano, request.TurmaIdReferencia);
    }
}
