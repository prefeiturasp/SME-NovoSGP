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
    public class ExcluirPreDefinicaoFrequenciaCommandHandler : IRequestHandler<ExcluirPreDefinicaoFrequenciaCommand, bool>
    {
        private readonly IRepositorioFrequenciaPreDefinida repositorioFrequenciaPreDefinida;

        public ExcluirPreDefinicaoFrequenciaCommandHandler(IRepositorioFrequenciaPreDefinida repositorioFrequenciaPreDefinida)
        {
            this.repositorioFrequenciaPreDefinida = repositorioFrequenciaPreDefinida ?? throw new ArgumentNullException(nameof(repositorioFrequenciaPreDefinida));
        }

        public async Task<bool> Handle(ExcluirPreDefinicaoFrequenciaCommand request, CancellationToken cancellationToken)
        {
            await repositorioFrequenciaPreDefinida.RemoverPorCCIdETurmaId(request.ComponenteCurricularId, request.TurmaId);
            return true;
        }
    }
}
