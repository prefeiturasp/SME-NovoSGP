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
            var acompanhamentosAlunos = mensagem.ObterObjetoMensagem<IEnumerable<AjusteRotaImagensAcompanhamentoAlunoDto>>();
            var listaAtualizacao = new List<(long id, string nomeAnterior, string nomeAtual)>();

            foreach (var acompanhamentoAluno in acompanhamentosAlunos)
            {
                using (var client = new WebClient())
                {
                    try
                    {
                        var arquivo = client.DownloadData(new Uri(acompanhamentoAluno.NomeCompleto));
                    }
                    catch (Exception)
                    {
                        try
                        {
                            var arquivo = client.DownloadData(new Uri(acompanhamentoAluno.NomeCompletoAlternativo));
                            listaAtualizacao.Add((acompanhamentoAluno.Id, acompanhamentoAluno.NomeCompleto, acompanhamentoAluno.NomeCompletoAlternativo));
                        }
                        catch (Exception)
                        {
                            try
                            {
                                var arquivo = client.DownloadData(new Uri(acompanhamentoAluno.NomeCompletoAlternativo2));
                                listaAtualizacao.Add((acompanhamentoAluno.Id, acompanhamentoAluno.NomeCompleto, acompanhamentoAluno.NomeCompletoAlternativo2));
                            }
                            catch (Exception)
                            {
                                try
                                {
                                    var arquivo = client.DownloadData(new Uri(acompanhamentoAluno.NomeCompletoAlternativo3));
                                    listaAtualizacao.Add((acompanhamentoAluno.Id, acompanhamentoAluno.NomeCompleto, acompanhamentoAluno.NomeCompletoAlternativo3));
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
