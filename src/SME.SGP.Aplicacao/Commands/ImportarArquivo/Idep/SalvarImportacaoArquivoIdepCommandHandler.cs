using MediatR;
using SME.SGP.Aplicacao.Commands.ImportarArquivo.Idep;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces.Repositorios;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands.ImportarArquivo.Ideb
{
    public class SalvarImportacaoArquivoIdepCommandHandler : IRequestHandler<SalvarImportacaoArquivoIdepCommand, Dominio.Entidades.Idep>
    {
        private readonly IRepositorioIdep repositorioIdep;
        public SalvarImportacaoArquivoIdepCommandHandler(IRepositorioIdep repositorioIdep)
        {
            this.repositorioIdep = repositorioIdep ?? throw new ArgumentNullException(nameof(repositorioIdep));
        }

        public async Task<Dominio.Entidades.Idep> Handle(SalvarImportacaoArquivoIdepCommand request, CancellationToken cancellationToken)
        {
            var dto = MapearParaEntidade(request);
            var registroAtual = await repositorioIdep.ObterRegistroIdepAsync(dto.AnoLetivo, (short)dto.SerieAno, dto.CodigoEOLEscola);

            if (registroAtual != null)
            {
                registroAtual.Nota = dto.Nota;
                dto = registroAtual;
            }

            await repositorioIdep.SalvarAsync(dto);

            return dto;
        }

        private Dominio.Entidades.Idep MapearParaEntidade(SalvarImportacaoArquivoIdepCommand request)
        => new Dominio.Entidades.Idep()
        {
            AnoLetivo = request.ArquivoIdep.AnoLetivo,
            SerieAno = (SerieAnoIndiceDesenvolvimentoEnum)request.ArquivoIdep.SerieAno,
            CodigoEOLEscola = request.ArquivoIdep.CodigoEOLEscola,
            Nota = request.ArquivoIdep.Nota,
        };
    }
}
