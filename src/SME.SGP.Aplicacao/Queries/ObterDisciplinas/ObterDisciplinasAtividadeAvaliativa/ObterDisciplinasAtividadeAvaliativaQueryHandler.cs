using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterDisciplinasAtividadeAvaliativaQueryHandler : IRequestHandler<ObterDisciplinasAtividadeAvaliativaQuery, IEnumerable<AtividadeAvaliativaDisciplina>>
    {
        private readonly IRepositorioAtividadeAvaliativaDisciplina repositorioAtividadeAvaliativaDisciplina;        

        public ObterDisciplinasAtividadeAvaliativaQueryHandler(IRepositorioAtividadeAvaliativaDisciplina repositorioAtividadeAvaliativaDisciplina)
        {
            this.repositorioAtividadeAvaliativaDisciplina = repositorioAtividadeAvaliativaDisciplina ?? throw new ArgumentNullException(nameof(repositorioAtividadeAvaliativaDisciplina));            
        }

        public async Task<IEnumerable<AtividadeAvaliativaDisciplina>> Handle(ObterDisciplinasAtividadeAvaliativaQuery request, CancellationToken cancellationToken)
        {
            if (request.EhRegencia)
                return await repositorioAtividadeAvaliativaDisciplina.ObterDisciplinasAtividadeAvaliativa(request.Avaliacao_id);
            return await repositorioAtividadeAvaliativaDisciplina.ListarPorIdAtividade(request.Avaliacao_id);
        }

    }
}
