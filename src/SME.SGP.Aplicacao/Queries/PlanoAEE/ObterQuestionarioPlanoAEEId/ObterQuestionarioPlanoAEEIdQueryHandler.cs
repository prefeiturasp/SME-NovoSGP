using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterQuestionarioPlanoAEEIdQueryHandler : IRequestHandler<ObterQuestionarioPlanoAEEIdQuery, long>
    {
        private readonly IRepositorioQuestionario repositorioQuestionario;

        public ObterQuestionarioPlanoAEEIdQueryHandler(IRepositorioQuestionario repositorioQuestionario)
        {
            this.repositorioQuestionario = repositorioQuestionario ?? throw new ArgumentNullException(nameof(repositorioQuestionario));
        }

        public async Task<long> Handle(ObterQuestionarioPlanoAEEIdQuery request, CancellationToken cancellationToken)
            => await repositorioQuestionario.ObterQuestionarioIdPorTipo((int)TipoQuestionario.PlanoAEE);
    }
}
