using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Extensoes;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SyncAjusteImagensAcompanhamentoAprendizagemAlunoUseCase : AbstractUseCase, ISyncAjusteImagensAcompanhamentoAprendizagemAlunoUseCase
    {
        private readonly IRepositorioAcompanhamentoAlunoSemestre repositorio;

        public SyncAjusteImagensAcompanhamentoAprendizagemAlunoUseCase(IMediator mediator,
                                                                       IRepositorioAcompanhamentoAlunoSemestre repositorio) : base(mediator)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var acompanhamentoAluno = mensagem.ObterObjetoMensagem<FiltroAjusteImagensRAADto>();
            var imagens = await repositorio.ObterImagensParaAjusteRota(acompanhamentoAluno.Id);
            var listaAtualizacao = new List<(long id, string nomeAnterior, string nomeAtual)>();

            foreach (var imagem in imagens)
            {
                using (var client = new WebClient())
                {
                    byte[] arquivo = null;
                    Exception excecao = null;
                    if (!client.TryDownloadData(imagem.NomeCompleto, out arquivo, out excecao))
                    {
                        if (client.TryDownloadData(imagem.NomeCompletoAlternativo, out arquivo, out excecao))
                            listaAtualizacao.Add((imagem.Id, imagem.NomeCompleto, imagem.NomeCompletoAlternativo));
                        else if (client.TryDownloadData(imagem.NomeCompletoAlternativo2, out arquivo, out excecao))
                            listaAtualizacao.Add((imagem.Id, imagem.NomeCompleto, imagem.NomeCompletoAlternativo2));
                        else if (client.TryDownloadData(imagem.NomeCompletoAlternativo3, out arquivo, out excecao))
                            listaAtualizacao.Add((imagem.Id, imagem.NomeCompleto, imagem.NomeCompletoAlternativo3));
                        else
                            throw excecao;
                    }
                }
            }

            foreach(var atualizar in listaAtualizacao)
                await repositorio.AtualizarLinkImagem(atualizar.id, atualizar.nomeAnterior, atualizar.nomeAtual);

            return true;
        }
    }
}
