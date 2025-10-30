using MediatR;
using SME.SGP.Dominio.Interfaces.Repositorios;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands.ImportarArquivo.ProficienciaIdeb
{
    public class ExcluirImportacaoProficienciaIdebCommandHandler : IRequestHandler<ExcluirImportacaoProficienciaIdebCommand, bool>
    {
        private readonly IRepositorioProficienciaIdeb _repositorioProficienciaIdeb;
        public ExcluirImportacaoProficienciaIdebCommandHandler(IRepositorioProficienciaIdeb repositorioProficienciaIdeb)
        {
            _repositorioProficienciaIdeb = repositorioProficienciaIdeb ?? throw new ArgumentNullException(nameof(repositorioProficienciaIdeb));
        }

        public async Task<bool> Handle(ExcluirImportacaoProficienciaIdebCommand request, CancellationToken cancellationToken)
        {
            return await _repositorioProficienciaIdeb.ExcluirProficienciaAsync(request.AnoLetivo, request.CodigoUe, request.SerieAno, request.ComponenteCurricular);
        }
    }
}
