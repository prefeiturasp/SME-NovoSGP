using MediatR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterFotosSemestreAlunoUseCase : AbstractUseCase, IObterFotosSemestreAlunoUseCase
    {
        public ObterFotosSemestreAlunoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<IEnumerable<(byte[], string, string)>> Executar(long acompanhamentoSemestreId)
        {
            var miniaturas = await mediator.Send(new ObterMiniaturasFotosSemestreAlunoQuery(acompanhamentoSemestreId));

            return await DownloadMiniaturas(miniaturas);
        }

        private async Task<IEnumerable<(byte[], string, string)>> DownloadMiniaturas(IEnumerable<Arquivo> miniaturas)
        {
            var arquivos = new List<(byte[], string, string)>();

            foreach(var miniatura in miniaturas)
            {
                var arquivoFisico = await mediator.Send(new DownloadArquivoCommand(miniatura.Codigo, miniatura.Nome, miniatura.Tipo));

                arquivos.Add((arquivoFisico, miniatura.TipoConteudo, miniatura.Nome));
            }

            return arquivos;
        }
    }
}
