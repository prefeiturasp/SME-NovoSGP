using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands
    {
        public class AtualizarVariosComponentesCurricularesCommandHandler : IRequestHandler<AtualizarVariosComponentesCurricularesCommand, bool>
        {
            private readonly IRepositorioComponenteCurricularEntity repositorioComponenteCurricular;

            public AtualizarVariosComponentesCurricularesCommandHandler(IRepositorioComponenteCurricularEntity repositorioComponenteCurricular)
            {
                this.repositorioComponenteCurricular = repositorioComponenteCurricular ?? throw new ArgumentNullException(nameof(repositorioComponenteCurricular));
            }

            public async Task<bool> Handle(AtualizarVariosComponentesCurricularesCommand request, CancellationToken cancellationToken)
            {
                await repositorioComponenteCurricular.AlterarVariosAsync(request.ComponentesCurriculares);

                return true;
            }
        }
    }
