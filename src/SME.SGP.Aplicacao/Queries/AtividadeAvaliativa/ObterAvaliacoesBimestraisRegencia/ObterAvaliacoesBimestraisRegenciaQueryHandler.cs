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
    public class ObterAvaliacoesBimestraisRegenciaQueryHandler : IRequestHandler<ObterAvaliacoesBimestraisRegenciaQuery, IEnumerable<AtividadeAvaliativaRegencia>>
    {
        private readonly IRepositorioAtividadeAvaliativaRegencia repositorioAtividadeAvaliativaRegencia;

        public ObterAvaliacoesBimestraisRegenciaQueryHandler(IRepositorioAtividadeAvaliativaRegencia repositorioAtividadeAvaliativaRegencia)
        {
            this.repositorioAtividadeAvaliativaRegencia = repositorioAtividadeAvaliativaRegencia ?? throw new ArgumentNullException(nameof(repositorioAtividadeAvaliativaRegencia));
        }
        public async Task<IEnumerable<AtividadeAvaliativaRegencia>> Handle(ObterAvaliacoesBimestraisRegenciaQuery request, CancellationToken cancellationToken)
         => await repositorioAtividadeAvaliativaRegencia.ObterAvaliacoesBimestrais(request.TipoCalendarioId, request.TurmaId, request.DisciplinaId, request.Bimestre);
    }
}
