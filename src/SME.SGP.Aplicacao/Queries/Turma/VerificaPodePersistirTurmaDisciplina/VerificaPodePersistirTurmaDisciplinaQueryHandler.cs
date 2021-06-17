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
    public class VerificaPodePersistirTurmaDisciplinaQueryHandler : IRequestHandler<VerificaPodePersistirTurmaDisciplinaQuery, bool>
    {
        private readonly IRepositorioAtribuicaoCJ repositorioAtribuicaoCJ;
        private readonly IMediator mediator;

        public VerificaPodePersistirTurmaDisciplinaQueryHandler(IRepositorioAtribuicaoCJ repositorioAtribuicaoCJ, IMediator mediator)
        {
            this.repositorioAtribuicaoCJ = repositorioAtribuicaoCJ ?? throw new ArgumentNullException(nameof(repositorioAtribuicaoCJ));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(VerificaPodePersistirTurmaDisciplinaQuery request, CancellationToken cancellationToken)
        {
            if (!request.Usuario.EhProfessorCj())
                return await mediator.Send(new VerificaPodePersistirTurmaDisciplinaEOLQuery(request.Usuario, request.TurmaId, request.ComponenteCurricularId, request.Data));

            var atribuicaoCj = repositorioAtribuicaoCJ.ObterAtribuicaoAtiva(request.Usuario.CodigoRf);

            return atribuicaoCj != null && atribuicaoCj.Any();
        }
    }
}
