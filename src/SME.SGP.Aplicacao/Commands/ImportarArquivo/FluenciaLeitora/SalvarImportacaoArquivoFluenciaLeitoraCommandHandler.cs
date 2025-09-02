using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces.Repositorios;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands.ImportarArquivo.FluenciaLeitora
{
    public class SalvarImportacaoArquivoFluenciaLeitoraCommandHandler : IRequestHandler<SalvarImportacaoArquivoFluenciaLeitoraCommand, Dominio.FluenciaLeitora>
    {
        private readonly IRepositorioFluenciaLeitora repositorioArquivoIbep;
        public SalvarImportacaoArquivoFluenciaLeitoraCommandHandler(IRepositorioFluenciaLeitora repositorioArquivoIbep)
        {
            this.repositorioArquivoIbep = repositorioArquivoIbep ?? throw new ArgumentNullException(nameof(repositorioArquivoIbep));
        }

        public async Task<Dominio.FluenciaLeitora> Handle(SalvarImportacaoArquivoFluenciaLeitoraCommand request, CancellationToken cancellationToken)
        {
            var arquivoFluenciaLeitora = MapearParaEntidade(request);

            await repositorioArquivoIbep.SalvarAsync(arquivoFluenciaLeitora);

            return arquivoFluenciaLeitora;
        }

        private Dominio.FluenciaLeitora MapearParaEntidade(SalvarImportacaoArquivoFluenciaLeitoraCommand request)
        => new Dominio.FluenciaLeitora()
        {
            CodigoEOLTurma = request.ArquivoFluenciaLeitora.CodigoEOLTurma,
            CodigoEOLAluno = request.ArquivoFluenciaLeitora.CodigoEOLAluno,
            Fluencia = request.ArquivoFluenciaLeitora.Fluencia,
            Periodo = request.ArquivoFluenciaLeitora.Periodo
        };
    }
}
