using MediatR;
using SME.SGP.Dominio.Interfaces.Repositorios;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands.ImportarArquivo.ProficienciaIdep
{
    public class ExcluirImportacaoProficienciaIdepCommandHandler : IRequestHandler<ExcluirImportacaoProficienciaIdepCommand, bool>
    {
        private readonly IRepositorioProficienciaIdep _repositorioProficienciaIdep;
        public ExcluirImportacaoProficienciaIdepCommandHandler(IRepositorioProficienciaIdep repositorioProficienciaIdep)
        {
            _repositorioProficienciaIdep = repositorioProficienciaIdep ?? throw new ArgumentNullException(nameof(repositorioProficienciaIdep));
        }

        public async Task<bool> Handle(ExcluirImportacaoProficienciaIdepCommand request, CancellationToken cancellationToken)
        {
            return await _repositorioProficienciaIdep.ExcluirProficienciaAsync(request.AnoLetivo, request.CodigoUe, request.SerieAno, request.ComponenteCurricular);
        }
    }
}
