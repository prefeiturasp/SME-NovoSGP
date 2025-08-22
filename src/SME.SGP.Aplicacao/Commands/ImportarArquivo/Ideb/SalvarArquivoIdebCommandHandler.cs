using MediatR;
using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.ImportarArquivo;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands.ImportarArquivo.Ideb
{
    public class SalvarArquivoIdebCommandHandler : IRequestHandler<SalvarArquivoIdebCommand, ArquivoIdeb>
    {
        private readonly IRepositorioArquivoIdeb repositorioArquivoIbeb;
        public SalvarArquivoIdebCommandHandler(IRepositorioArquivoIdeb repositorioArquivoIbeb)
        {
            this.repositorioArquivoIbeb = repositorioArquivoIbeb ?? throw new ArgumentNullException(nameof(repositorioArquivoIbeb));
        }

        public async Task<ArquivoIdeb> Handle(SalvarArquivoIdebCommand request, CancellationToken cancellationToken)
        {
            var arquivoIdeb = MapearParaEntidade(request);

            await repositorioArquivoIbeb.SalvarAsync(arquivoIdeb);

            return arquivoIdeb;
        }

        private ArquivoIdeb MapearParaEntidade(SalvarArquivoIdebCommand request)
        => new ArquivoIdeb()
        {
            SerieAno = request.ArquivoIdeb.SerieAno,
            CodigoEOLEscola = request.ArquivoIdeb.CodigoEOLEscola,
            Nota = request.ArquivoIdeb.Nota,
        };
    }
}
