﻿using System;
using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace SME.SGP.Aplicacao
{
    public class ObterFotosSemestreAlunoUseCase : AbstractUseCase, IObterFotosSemestreAlunoUseCase
    {
        public ObterFotosSemestreAlunoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<IEnumerable<ArquivoDto>> Executar(long param)
        {
            var quantidade = await ObterQuantidadeFotos(param);
            var miniaturas = await mediator.Send(new ObterMiniaturasFotosSemestreAlunoQuery(param, quantidade));

            return await DownloadMiniaturas(miniaturas);
        }

        private async Task<int> ObterQuantidadeFotos(long acompanhamentoSemestreId)
        {
            var ano = await mediator.Send(new ObterAnoDoAcompanhamentoAlunoQuery(acompanhamentoSemestreId));
            if (ano == 0)
                throw new NegocioException("O ano do acompanhamento do estudante/criança não foi localizado");

            var parametroQuantidade = await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(TipoParametroSistema.QuantidadeImagensPercursoIndividualCrianca, ano));
            if (parametroQuantidade.EhNulo())
                throw new NegocioException("O parâmetro de quantidade de fotos do acompanhamento do estudante/criança não foi localizado");

            return int.Parse(parametroQuantidade.Valor);
        }

        private async Task<IEnumerable<ArquivoDto>> DownloadMiniaturas(IEnumerable<MiniaturaFotoDto> miniaturas)
        {
            var arquivos = new List<ArquivoDto>();

            try
            {
                foreach(var miniatura in miniaturas)
                {
                    var arquivoFisico = await mediator.Send(new DownloadArquivoCommand(miniatura.Codigo, miniatura.Nome, miniatura.Tipo));

                    arquivos.Add(new ArquivoDto()
                    {
                        Codigo = miniatura.CodigoFotoOriginal,
                        Nome = miniatura.Nome,
                        Download = (arquivoFisico, miniatura.TipoConteudo, miniatura.Nome)
                    });
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Falha ao carregar miniaturas das fotos do aluno", ex);
            }

            return arquivos;
        }
    }
}
