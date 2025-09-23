using MediatR;
using SME.SGP.Aplicacao.Commands.ImportarArquivo.Idep;
using SME.SGP.Dominio.Interfaces.Repositorios;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands.ImportarArquivo.Ideb
{
    public class SalvarImportacaoArquivoIdepCommandHandler : IRequestHandler<SalvarImportacaoArquivoIdepCommand, Dominio.Idep>
    {
        private readonly IRepositorioIdep repositorioArquivoIbep;
        public SalvarImportacaoArquivoIdepCommandHandler(IRepositorioIdep repositorioArquivoIbep)
        {
            this.repositorioArquivoIbep = repositorioArquivoIbep ?? throw new ArgumentNullException(nameof(repositorioArquivoIbep));
        }

        public async Task<Dominio.Idep> Handle(SalvarImportacaoArquivoIdepCommand request, CancellationToken cancellationToken)
        {
            var arquivoIdep = MapearParaEntidade(request);

            await repositorioArquivoIbep.SalvarAsync(arquivoIdep);

            return arquivoIdep;
        }

        private Dominio.Idep MapearParaEntidade(SalvarImportacaoArquivoIdepCommand request)
        => new Dominio.Idep()
        {
            AnoLetivo = request.ArquivoIdep.AnoLetivo,
            SerieAno = request.ArquivoIdep.SerieAno,
            CodigoEOLEscola = request.ArquivoIdep.CodigoEOLEscola,
            Nota = request.ArquivoIdep.Nota,
        };
    }
}
