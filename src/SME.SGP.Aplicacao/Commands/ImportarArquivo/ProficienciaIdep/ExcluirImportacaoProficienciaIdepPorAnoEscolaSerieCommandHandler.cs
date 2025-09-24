using MediatR;
using SME.SGP.Dominio.Interfaces.Repositorios;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands.ImportarArquivo.ProficienciaIdep
{
    public class ExcluirImportacaoProficienciaIdepPorAnoEscolaSerieCommandHandler : IRequestHandler<ExcluirImportacaoProficienciaIdepPorAnoEscolaSerieCommand, bool>
    {
        private readonly IRepositorioProficienciaIdep _repositorioProficienciaIdep;
        public ExcluirImportacaoProficienciaIdepPorAnoEscolaSerieCommandHandler(IRepositorioProficienciaIdep repositorioProficienciaIdep)
        {
            _repositorioProficienciaIdep = repositorioProficienciaIdep ?? throw new ArgumentNullException(nameof(repositorioProficienciaIdep));
        }

        public async Task<bool> Handle(ExcluirImportacaoProficienciaIdepPorAnoEscolaSerieCommand request, CancellationToken cancellationToken)
        {
            return await _repositorioProficienciaIdep.ExcluirPorAnoEscolaSerie(request.AnoLetivo, request.CodigoEolEscola, request.SerieAno);
        }
    }
}
