using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;

namespace SME.SGP.Aplicacao
{
    public class ObterNotaTipoValorPorTurmaIdQueryHandler : IRequestHandler<ObterNotaTipoValorPorTurmaIdQuery,NotaTipoValor>
    {
        private readonly IRepositorioNotaTipoValorConsulta repositorioNotaTipoValorConsulta;

        public ObterNotaTipoValorPorTurmaIdQueryHandler(IRepositorioNotaTipoValorConsulta notaTipoValorConsulta)
        {
            repositorioNotaTipoValorConsulta = notaTipoValorConsulta ??
                                               throw new ArgumentNullException(nameof(notaTipoValorConsulta));
        }

        public async Task<NotaTipoValor> Handle(ObterNotaTipoValorPorTurmaIdQuery request, CancellationToken cancellationToken)
        {
            if (request.Turma.EhCELP())
                return new NotaTipoValor() { TipoNota = TipoNota.Conceito };

            return await repositorioNotaTipoValorConsulta.ObterPorTurmaIdAsync(request.Turma.Id, request.Turma.TipoTurma);
        }
    }
}