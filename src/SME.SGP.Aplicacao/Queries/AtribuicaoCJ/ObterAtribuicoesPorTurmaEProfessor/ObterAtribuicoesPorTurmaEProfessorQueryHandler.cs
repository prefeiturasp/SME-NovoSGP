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
    public class ObterAtribuicoesPorTurmaEProfessorQueryHandler : IRequestHandler<ObterAtribuicoesPorTurmaEProfessorQuery, IEnumerable<AtribuicaoCJ>>
    {
        private readonly IRepositorioAtribuicaoCJ repositorioAtribuicaoCJ;

        public ObterAtribuicoesPorTurmaEProfessorQueryHandler(IRepositorioAtribuicaoCJ repositorioAtribuicaoCJ)
        {
            this.repositorioAtribuicaoCJ = repositorioAtribuicaoCJ ?? throw new ArgumentNullException(nameof(repositorioAtribuicaoCJ));
        }

        public async Task<IEnumerable<AtribuicaoCJ>> Handle(ObterAtribuicoesPorTurmaEProfessorQuery request, CancellationToken cancellationToken)
            => await repositorioAtribuicaoCJ.ObterPorFiltros(request.Modalidade,
                request.TurmaId,
                request.UeId,
                request.ComponenteCurricularId,
                request.UsuarioRf,
                request.UsuarioNome,
                request.Substituir,
                request.DreCodigo,
                request.TurmaIds,
                request.AnoLetivo
                );
    }
}
