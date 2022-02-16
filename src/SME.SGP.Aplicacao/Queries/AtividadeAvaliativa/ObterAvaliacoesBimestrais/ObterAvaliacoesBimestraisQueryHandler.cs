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
    public class ObterAvaliacoesBimestraisQueryHandler : IRequestHandler<ObterAvaliacoesBimestraisQuery, IEnumerable<AtividadeAvaliativaDisciplina>>
    {
        private readonly IRepositorioAtividadeAvaliativaDisciplina repositorioAtividadeAvaliativaDisciplina;

        public ObterAvaliacoesBimestraisQueryHandler(IRepositorioAtividadeAvaliativaDisciplina repositorioAtividadeAvaliativaDisciplina)
        {
            this.repositorioAtividadeAvaliativaDisciplina = repositorioAtividadeAvaliativaDisciplina ?? throw new ArgumentNullException(nameof(repositorioAtividadeAvaliativaDisciplina));
        }
        public async Task<IEnumerable<AtividadeAvaliativaDisciplina>> Handle(ObterAvaliacoesBimestraisQuery request, CancellationToken cancellationToken)
         => await repositorioAtividadeAvaliativaDisciplina.ObterAvaliacoesBimestrais(request.TipoCalendarioId, request.TurmaId, request.DisciplinaId, request.Bimestre);
    }
}
