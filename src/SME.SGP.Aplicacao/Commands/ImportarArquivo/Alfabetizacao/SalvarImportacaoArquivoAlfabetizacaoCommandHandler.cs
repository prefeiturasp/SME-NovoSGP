using MediatR;
using SME.SGP.Dominio.Interfaces.Repositorios;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands.ImportarArquivo.Alfabetizacao
{
    public class SalvarImportacaoArquivoAlfabetizacaoCommandHandler : IRequestHandler<SalvarImportacaoArquivoAlfabetizacaoCommand, Dominio.TaxaAlfabetizacao>
    {
        private readonly IRepositorioTaxaAlfabetizacao repositorioAlfabetizacao;
        public SalvarImportacaoArquivoAlfabetizacaoCommandHandler(IRepositorioTaxaAlfabetizacao repositorioAlfabetizacao)
        {
            this.repositorioAlfabetizacao = repositorioAlfabetizacao ?? throw new ArgumentNullException(nameof(repositorioAlfabetizacao));
        }

        public async Task<Dominio.TaxaAlfabetizacao> Handle(SalvarImportacaoArquivoAlfabetizacaoCommand request, CancellationToken cancellationToken)
        {
            var entidade = MapearParaEntidade(request);
            var registroAtual = await repositorioAlfabetizacao.ObterRegistroAlfabetizacaoAsync(entidade.AnoLetivo, entidade.CodigoEOLEscola);

            if (registroAtual != null)
            {
                registroAtual.Taxa = request.ArquivoAlfabetizacao.TaxaAlfabetizacao;
                 await repositorioAlfabetizacao.SalvarAsync(registroAtual);
                return registroAtual;
            }

            await repositorioAlfabetizacao.SalvarAsync(entidade);

            return entidade;
        }

        private Dominio.TaxaAlfabetizacao MapearParaEntidade(SalvarImportacaoArquivoAlfabetizacaoCommand request)
            => new Dominio.TaxaAlfabetizacao()
            {
                AnoLetivo = request.ArquivoAlfabetizacao.AnoLetivo,
                CodigoEOLEscola = request.ArquivoAlfabetizacao.CodigoEOLEscola,
                Taxa = request.ArquivoAlfabetizacao.TaxaAlfabetizacao
            };
    }
}
