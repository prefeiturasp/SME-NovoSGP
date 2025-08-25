using MediatR;
using SME.SGP.Aplicacao.Commands.ImportarArquivo.Idep;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces.Repositorios;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands.ImportarArquivo.Ideb
{
    public class SalvarImportacaoArquivoIdepCommandHandler : IRequestHandler<SalvarImportacaoArquivoIdepCommand, ArquivoIdep>
    {
        private readonly IRepositorioArquivoIdep repositorioArquivoIbep;
        public SalvarImportacaoArquivoIdepCommandHandler(IRepositorioArquivoIdep repositorioArquivoIbep)
        {
            this.repositorioArquivoIbep = repositorioArquivoIbep ?? throw new ArgumentNullException(nameof(repositorioArquivoIbep));
        }

        public async Task<ArquivoIdep> Handle(SalvarImportacaoArquivoIdepCommand request, CancellationToken cancellationToken)
        {
            var arquivoIdep = MapearParaEntidade(request);

            await repositorioArquivoIbep.SalvarAsync(arquivoIdep);

            return arquivoIdep;
        }

        private ArquivoIdep MapearParaEntidade(SalvarImportacaoArquivoIdepCommand request)
        => new ArquivoIdep()
        {
            AnoLetivo = request.ArquivoIdep.AnoLetivo,
            SerieAno = request.ArquivoIdep.SerieAno,
            CodigoEOLEscola = request.ArquivoIdep.CodigoEOLEscola,
            Nota = request.ArquivoIdep.Nota,
        };
    }
}
