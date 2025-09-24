using MediatR;
using SME.SGP.Dominio.Interfaces.Repositorios;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands.ImportarArquivo.ProficienciaIdeb
{
    public class ExcluirImportacaoProficienciaIdebPorAnoEscolaSerieCommandHandler : IRequestHandler<ExcluirImportacaoProficienciaIdebPorAnoEscolaSerieCommand, bool>
    {
        private readonly IRepositorioProficienciaIdeb _repositorioProficienciaIdeb;
        public ExcluirImportacaoProficienciaIdebPorAnoEscolaSerieCommandHandler(IRepositorioProficienciaIdeb repositorioProficienciaIdeb)
        {
            _repositorioProficienciaIdeb = repositorioProficienciaIdeb ?? throw new ArgumentNullException(nameof(repositorioProficienciaIdeb));
        }

        public async Task<bool> Handle(ExcluirImportacaoProficienciaIdebPorAnoEscolaSerieCommand request, CancellationToken cancellationToken)
        {
            return await _repositorioProficienciaIdeb.ExcluirPorAnoEscolaSerie(request.AnoLetivo, request.CodigoEolEscola, request.SerieAno);
        }
    }
}
