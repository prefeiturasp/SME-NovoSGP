using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterFrequenciaPreDefinidaPorAlunoETurmaQueryHandler : IRequestHandler<ObterFrequenciaPreDefinidaPorAlunoETurmaQuery, TipoFrequencia>
    {
        private readonly IRepositorioFrequenciaPreDefinida repositorioFrequenciaPreDefinida;

        public ObterFrequenciaPreDefinidaPorAlunoETurmaQueryHandler(IRepositorioFrequenciaPreDefinida repositorioFrequenciaPreDefinida)
        {
            this.repositorioFrequenciaPreDefinida = repositorioFrequenciaPreDefinida ?? throw new ArgumentNullException(nameof(repositorioFrequenciaPreDefinida));
        }

        public async Task<TipoFrequencia> Handle(ObterFrequenciaPreDefinidaPorAlunoETurmaQuery request, CancellationToken cancellationToken)
        {
            var retorno = await repositorioFrequenciaPreDefinida.ObterPorTurmaECCEAlunoCodigo(request.TurmaId, request.ComponenteCurricularId, request.AlunoCodigo);
            if (retorno != null)
                return retorno.Tipo;

            return TipoFrequencia.C;
        }
    }
}
