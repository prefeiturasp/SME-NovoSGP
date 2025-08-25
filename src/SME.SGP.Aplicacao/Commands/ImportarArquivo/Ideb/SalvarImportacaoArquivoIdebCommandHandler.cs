using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces.Repositorios;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands.ImportarArquivo.Ideb
{
    public class SalvarImportacaoArquivoIdebCommandHandler : IRequestHandler<SalvarImportacaoArquivoIdebCommand, ArquivoIdeb>
    {
        private readonly IRepositorioArquivoIdeb repositorioArquivoIbep;
        public SalvarImportacaoArquivoIdebCommandHandler(IRepositorioArquivoIdeb repositorioArquivoIbep)
        {
            this.repositorioArquivoIbep = repositorioArquivoIbep ?? throw new ArgumentNullException(nameof(repositorioArquivoIbep));
        }

        public async Task<ArquivoIdeb> Handle(SalvarImportacaoArquivoIdebCommand request, CancellationToken cancellationToken)
        {
            var arquivoIdeb = MapearParaEntidade(request);

            await repositorioArquivoIbep.SalvarAsync(arquivoIdeb);

            return arquivoIdeb;
        }

        private ArquivoIdeb MapearParaEntidade(SalvarImportacaoArquivoIdebCommand request)
        => new ArquivoIdeb()
        {
            AnoLetivo = request.ArquivoIdeb.AnoLetivo,
            SerieAno = request.ArquivoIdeb.SerieAno,
            CodigoEOLEscola = request.ArquivoIdeb.CodigoEOLEscola,
            Nota = request.ArquivoIdeb.Nota,
        };
    }
}
