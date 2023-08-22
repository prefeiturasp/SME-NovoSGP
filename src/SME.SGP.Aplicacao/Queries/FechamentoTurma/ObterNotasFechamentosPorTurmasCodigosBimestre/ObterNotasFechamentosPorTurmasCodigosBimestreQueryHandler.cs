using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SME.SGP.Dominio.Constantes;

namespace SME.SGP.Aplicacao
{
    public class ObterNotasFechamentosPorTurmasCodigosBimestreQueryHandler :
        IRequestHandler<ObterNotasFechamentosPorTurmasCodigosBimestreQuery, IEnumerable<NotaConceitoBimestreComponenteDto>>
    {
        private readonly IRepositorioFechamentoNotaConsulta repositorioFechamentoNota;
        private readonly IRepositorioCache repositorioCache;

        public ObterNotasFechamentosPorTurmasCodigosBimestreQueryHandler(IRepositorioFechamentoNotaConsulta repositorioFechamentoNota,
            IRepositorioCache repositorioCache)
        {
            this.repositorioFechamentoNota = repositorioFechamentoNota ?? throw new ArgumentNullException(nameof(repositorioFechamentoNota));
            this.repositorioCache = repositorioCache ?? throw new ArgumentNullException(nameof(repositorioCache));
        }

        public async Task<IEnumerable<NotaConceitoBimestreComponenteDto>> Handle(ObterNotasFechamentosPorTurmasCodigosBimestreQuery request,
            CancellationToken cancellationToken)
        {
            var retorno = new List<NotaConceitoBimestreComponenteDto>();
        
            foreach (var turmaCodigo in request.TurmasCodigos)
            {
                var notasConceitos = (await repositorioCache.ObterAsync(string.Format(NomeChaveCache.FECHAMENTO_NOTA_TURMA_BIMESTRE, turmaCodigo, request.Bimestre),
                    async () => await repositorioFechamentoNota.ObterNotasPorTurmaCodigoEBimestreAsync(turmaCodigo, request.Bimestre, tipoCalendario: request.TipoCalendario),
                    "Obter notas do fechamento")).ToList();
            
                if (notasConceitos.Any())
                    retorno.AddRange(notasConceitos);
            }
            
            return retorno.Where(c => c.AlunoCodigo == request.AlunoCodigo);
        }
    }
}
