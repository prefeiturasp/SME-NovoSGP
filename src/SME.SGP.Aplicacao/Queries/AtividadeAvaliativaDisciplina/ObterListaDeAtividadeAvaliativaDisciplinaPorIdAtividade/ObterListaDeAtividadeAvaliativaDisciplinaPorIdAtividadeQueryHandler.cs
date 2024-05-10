using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;

namespace SME.SGP.Aplicacao
{
    public class ObterListaDeAtividadeAvaliativaDisciplinaPorIdAtividadeQueryHandler : IRequestHandler<ObterListaDeAtividadeAvaliativaDisciplinaPorIdAtividadeQuery,IEnumerable<AtividadeAvaliativaDisciplina>>
    {
        private readonly IRepositorioAtividadeAvaliativaDisciplina repositorioAtividadeAvaliativaDisciplina;

        public ObterListaDeAtividadeAvaliativaDisciplinaPorIdAtividadeQueryHandler(
            IRepositorioAtividadeAvaliativaDisciplina atividadeAvaliativaDisciplina)
        {
            repositorioAtividadeAvaliativaDisciplina = atividadeAvaliativaDisciplina ??
                                                       throw new ArgumentNullException(
                                                           nameof(atividadeAvaliativaDisciplina));
        }

        public async Task<IEnumerable<AtividadeAvaliativaDisciplina>> Handle(ObterListaDeAtividadeAvaliativaDisciplinaPorIdAtividadeQuery request, CancellationToken cancellationToken)
        {
            return await repositorioAtividadeAvaliativaDisciplina.ListarPorIdAtividade(request.AtividadeAvaliativaId);
        }
    }
}