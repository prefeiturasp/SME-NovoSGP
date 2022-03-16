using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections;
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
                    try
                    {
                        var arquivo = client.DownloadData(new Uri(imagem.NomeCompleto));
                    }
                    catch (Exception)
                    {
                        try
                        {
                            var arquivo = client.DownloadData(new Uri(imagem.NomeCompletoAlternativo));
                            listaAtualizacao.Add((imagem.Id, imagem.NomeCompleto, imagem.NomeCompletoAlternativo));
                        }
                        catch (Exception)
                        {
                            try
                            {
                                var arquivo = client.DownloadData(new Uri(imagem.NomeCompletoAlternativo2));
                                listaAtualizacao.Add((imagem.Id, imagem.NomeCompleto, imagem.NomeCompletoAlternativo2));
                            }
                            catch (Exception)
                            {
                                try
                                {
                                    var arquivo = client.DownloadData(new Uri(imagem.NomeCompletoAlternativo3));
                                    listaAtualizacao.Add((imagem.Id, imagem.NomeCompleto, imagem.NomeCompletoAlternativo3));
                                }
                                catch (Exception)
                                {
                                    throw;
                                }
                            }
                        }
                    }
                }
            }

            foreach(var atualizar in listaAtualizacao)
            {
                await repositorio.AtualizarLinkImagem(atualizar.id, atualizar.nomeAnterior, atualizar.nomeAtual);
            }

            return true;
        }
    }
}
