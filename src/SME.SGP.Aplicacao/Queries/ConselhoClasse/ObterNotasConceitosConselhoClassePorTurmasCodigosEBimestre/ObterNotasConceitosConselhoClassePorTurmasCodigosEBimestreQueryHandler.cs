using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio.Constantes;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterNotasConceitosConselhoClassePorTurmasCodigosEBimestreQueryHandler : IRequestHandler<
        ObterNotasConceitosConselhoClassePorTurmasCodigosEBimestreQuery, IEnumerable<NotaConceitoBimestreComponenteDto>>
    {
        private readonly IRepositorioConselhoClasseNotaConsulta repositorioConselhoClasseNota;
        private readonly IRepositorioCache repositorioCache;

        public ObterNotasConceitosConselhoClassePorTurmasCodigosEBimestreQueryHandler(
            IRepositorioConselhoClasseNotaConsulta repositorioConselhoClasseNota, IRepositorioCache repositorioCache)
        {
            this.repositorioConselhoClasseNota = repositorioConselhoClasseNota ??
                                                 throw new ArgumentNullException(nameof(repositorioConselhoClasseNota));
            this.repositorioCache = repositorioCache ?? throw new ArgumentNullException(nameof(repositorioCache));
        }

        public async Task<IEnumerable<NotaConceitoBimestreComponenteDto>> Handle(ObterNotasConceitosConselhoClassePorTurmasCodigosEBimestreQuery request, CancellationToken cancellationToken)
        {
            var retorno = new List<NotaConceitoBimestreComponenteDto>();

            foreach (var turmaCodigo in request.TurmasCodigos)
            {
                var notasConceitosConselhoClasse = (await repositorioCache.ObterAsync(
                        string.Format(NomeChaveCache.NOTA_CONCEITO_CONSELHO_CLASSE_TURMA_BIMESTRE, turmaCodigo,
                            request.Bimestre),
                        async () => await repositorioConselhoClasseNota
                            .ObterNotasConceitosConselhoClassePorTurmaCodigoEBimestreAsync(turmaCodigo,
                                request.Bimestre == 0 ? null : request.Bimestre, tipoCalendario: request.TipoCalendario),
                        "Obter notas ou conceitos do conselho de classe"))
                    .ToList();

                if (notasConceitosConselhoClasse.Any())
                    retorno.AddRange(notasConceitosConselhoClasse);
            }

            return retorno;
        }
    }
}