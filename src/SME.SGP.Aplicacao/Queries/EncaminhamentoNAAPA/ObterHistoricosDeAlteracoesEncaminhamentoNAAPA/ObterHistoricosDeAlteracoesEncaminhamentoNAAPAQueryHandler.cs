﻿using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterHistoricosDeAlteracoesEncaminhamentoNAAPAQueryHandler : IRequestHandler<ObterHistoricosDeAlteracoesEncaminhamentoNAAPAQuery, EncaminhamentoNAAPAHistoricoAlteracoes>
    {
        private const string SECAO_ITINERANCIA = "QUESTOES_ITINERACIA";
        private List<string> camposInseridos;
        private List<string> camposAlterados;
        private readonly IRepositorioQuestao repositorioQuestao;
        private readonly IMediator mediator;
        private Usuario usuarioLogado;
        private List<Questao> Questoes;

        public ObterHistoricosDeAlteracoesEncaminhamentoNAAPAQueryHandler(IRepositorioQuestao repositorioQuestao, IMediator mediator)
        {
            camposInseridos = new List<string>();
            camposAlterados = new List<string>();
            this.repositorioQuestao = repositorioQuestao ?? throw new ArgumentNullException(nameof(repositorioQuestao));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            
        }

        public async Task<EncaminhamentoNAAPAHistoricoAlteracoes> Handle(ObterHistoricosDeAlteracoesEncaminhamentoNAAPAQuery request, CancellationToken cancellationToken)
        {
            this.usuarioLogado = await mediator.Send(ObterUsuarioLogadoQuery.Instance);
            await CarreguarQuestoesAInserir(request.EncaminhamentoNAAPASecaoAlterado);
            await ExecuteValidacaoAlteracaoCamposDaSecao(request.EncaminhamentoNAAPASecaoAlterado, request.EncaminhamentoNAAPASecaoExistente, request.TipoHistoricoAlteracoes);

            return ObterHistoricoAlteracaoSecao(request.EncaminhamentoNAAPASecaoAlterado, request.EncaminhamentoNAAPASecaoExistente, request.TipoHistoricoAlteracoes);
        }

        private EncaminhamentoNAAPAHistoricoAlteracoes ObterHistoricoAlteracaoSecao(
                                                        EncaminhamentoNAAPASecaoDto encaminhamentoNAAPAAlterado, 
                                                        EncaminhamentoNAAPASecao encaminhamentoSecaoExistente,
                                                        TipoHistoricoAlteracoesEncaminhamentoNAAPA tipoHistoricoAlteracoes)
        {
            if (camposInseridos.Any() || camposAlterados.Any())
            {
                return new EncaminhamentoNAAPAHistoricoAlteracoes()
                {
                    EncaminhamentoNAAPAId = encaminhamentoSecaoExistente.EncaminhamentoNAAPAId,
                    SecaoEncaminhamentoNAAPAId = encaminhamentoNAAPAAlterado.SecaoId,
                    DataHistorico = DateTimeExtension.HorarioBrasilia(),
                    TipoHistorico = tipoHistoricoAlteracoes,
                    CamposAlterados = ObterCamposFormatados(camposAlterados),
                    CamposInseridos = ObterCamposFormatados(camposInseridos),
                    DataAtendimento = ObterDataDoAtendimento(encaminhamentoNAAPAAlterado, encaminhamentoSecaoExistente),
                    UsuarioId = usuarioLogado.Id
                };
            }
        
            return null;
        }

        private string ObterDataDoAtendimento(EncaminhamentoNAAPASecaoDto encaminhamentoNAAPAAlterado, EncaminhamentoNAAPASecao encaminhamentoSecaoExistente)
        {
            if (SecaoEhItinerancia(encaminhamentoSecaoExistente))
            {
                var questoesData = encaminhamentoNAAPAAlterado.Questoes.FindAll(questao => questao.TipoQuestao == TipoQuestao.Data);

                return questoesData?.FirstOrDefault()?.Resposta;
            }

            return null;
        }

        private bool SecaoEhItinerancia(EncaminhamentoNAAPASecao encaminhamentoSecaoExistente)
        {
            return encaminhamentoSecaoExistente?.SecaoEncaminhamentoNAAPA?.NomeComponente == SECAO_ITINERANCIA;
        }

        private string ObterCamposFormatados(List<string> campos)
        {
            if (campos.Any())
                return String.Join(" | ", campos.Distinct().ToArray());

            return string.Empty;
        }

        private async Task ExecuteValidacaoAlteracaoCamposDaSecao(
                                EncaminhamentoNAAPASecaoDto encaminhamentoNAAPASecaoAlterado, 
                                EncaminhamentoNAAPASecao encaminhamentoSecaoExistente,
                                TipoHistoricoAlteracoesEncaminhamentoNAAPA tipoHistoricoAlteracoes)
        {
            foreach (var questaoAlterada in encaminhamentoNAAPASecaoAlterado.Questoes.GroupBy(q => q.QuestaoId))
            {
                var questaoExistente = encaminhamentoSecaoExistente?.Questoes?.Find(questao => questao.QuestaoId == questaoAlterada.Key);

                await AdicionarCamposInseridos(questaoExistente, questaoAlterada, tipoHistoricoAlteracoes);
                await AdicionarCamposAlterados(questaoExistente, questaoAlterada);
            }

            AdicionarCamposExcluidos(encaminhamentoSecaoExistente, encaminhamentoNAAPASecaoAlterado);
        }

        private async Task AdicionarCamposInseridos(
                            QuestaoEncaminhamentoNAAPA questaoExistente, 
                            IGrouping<long, EncaminhamentoNAAPASecaoQuestaoDto> respostas,
                            TipoHistoricoAlteracoesEncaminhamentoNAAPA tipoHistoricoAlteracoes)
        {
            var novasRespostas = respostas.ToList().Find(c => c.RespostaEncaminhamentoId == 0);

            if (novasRespostas != null)
            {
                var questao = questaoExistente?.Questao ?? Questoes?.FirstOrDefault(questao => questao.Id == novasRespostas.QuestaoId);

                if (tipoHistoricoAlteracoes == TipoHistoricoAlteracoesEncaminhamentoNAAPA.Inserido)
                {
                    if (CampoPodeSerInserido(novasRespostas)) 
                        camposInseridos.Add(await ObterNomeQuestao(questao));
                }
                else if (CampoPodeSerAlterado(questaoExistente, novasRespostas))
                    camposAlterados.Add(await ObterNomeQuestao(questao));
            }
        }

        private bool CampoPodeSerInserido(EncaminhamentoNAAPASecaoQuestaoDto respostasEncaminhamento)
        {
            if (respostasEncaminhamento.TipoQuestao == TipoQuestao.TurmasPrograma)
                return respostasEncaminhamento.Resposta != "[]";

            if (EhCampoLista(respostasEncaminhamento))
                return respostasEncaminhamento.Resposta != "\"\"";

            return !string.IsNullOrEmpty(respostasEncaminhamento.Resposta);
        }

        private async Task CarreguarQuestoesAInserir(EncaminhamentoNAAPASecaoDto encaminhamentoNAAPASecaoAlterado)
        {
            var questoesIds = encaminhamentoNAAPASecaoAlterado.Questoes.Where(questao => questao.RespostaEncaminhamentoId == 0)
                                                                       .Select(questao => questao.QuestaoId).Distinct();

            if (questoesIds.Any())
            {
                Questoes = (await repositorioQuestao.ObterQuestoesPorIds(questoesIds.ToArray())).ToList();
            }
        }

        private void AdicionarCamposExcluidos(EncaminhamentoNAAPASecao secaoExistente, EncaminhamentoNAAPASecaoDto encaminhamentoNAAPASecaoAlterado)
        {
            var idsQuestaoExistente = secaoExistente?.Questoes?.Select(questao => questao.QuestaoId);
            var idsQuestaoAlterado = encaminhamentoNAAPASecaoAlterado.Questoes.Select(questao => questao.QuestaoId);
            var idsQuestaoRemovidas = idsQuestaoExistente?.Except(idsQuestaoAlterado);

            if (idsQuestaoRemovidas != null && idsQuestaoRemovidas.Any())
            {
                var questoes = secaoExistente?.Questoes?.FindAll(questao => idsQuestaoRemovidas.Contains(questao.QuestaoId));
                var nomeQuestoes = questoes.Select(questao => questao.Questao.Nome).ToList();

                camposAlterados.AddRange(nomeQuestoes);
            }
        }

        private bool CampoPodeSerAlterado(
                            QuestaoEncaminhamentoNAAPA questaoExistente,
                            EncaminhamentoNAAPASecaoQuestaoDto respostasEncaminhamento)
        {
            if (EhCampoLista(respostasEncaminhamento))
                return ((questaoExistente == null && respostasEncaminhamento.Resposta != "[]") ||
                        (!string.IsNullOrEmpty(questaoExistente?.Respostas?.FirstOrDefault()?.Texto) && ObterCampoJsonSemId(questaoExistente?.Respostas?.FirstOrDefault()?.Texto) != ObterCampoJsonSemId(respostasEncaminhamento.Resposta)));

            if (EnumExtension.EhUmDosValores(respostasEncaminhamento.TipoQuestao, new Enum[] { TipoQuestao.Checkbox, TipoQuestao.ComboMultiplaEscolha, TipoQuestao.Upload }))
                return questaoExistente == null || questaoExistente.Respostas.Any();

            return false;
        }

        private async Task AdicionarCamposAlterados(QuestaoEncaminhamentoNAAPA questaoExistente, IGrouping<long, EncaminhamentoNAAPASecaoQuestaoDto> respostasEncaminhamento)
        {
            if (EnumExtension.EhUmDosValores(questaoExistente?.Questao.Tipo, new Enum[] { TipoQuestao.ComboMultiplaEscolha, TipoQuestao.Upload }))
            {
                if (await RespostaFoiRemovida(questaoExistente, respostasEncaminhamento))
                    camposAlterados.Add(await ObterNomeQuestao(questaoExistente.Questao));
            }
            else
            {
                var respostasExistentes = questaoExistente?.Respostas?.Where(s => respostasEncaminhamento.Any(c => c.RespostaEncaminhamentoId == s.Id));

                if (respostasExistentes != null)
                {
                    foreach (var respostaExistente in respostasExistentes)
                    {
                        var respostaAlterada = respostasEncaminhamento.ToList().Find(resposta => resposta.RespostaEncaminhamentoId == respostaExistente.Id);

                        if (CampoFoiAlterado(respostaExistente, respostaAlterada))
                            camposAlterados.Add(await ObterNomeQuestao(questaoExistente.Questao));
                    }
                }
            }
        }

        private async Task<long[]> ObterArquivosIdRespostas(IGrouping<long, EncaminhamentoNAAPASecaoQuestaoDto> respostasEncaminhamento)
        {
            var arquivosId = new List<long>();
            foreach (var item in respostasEncaminhamento.Where(resposta => !string.IsNullOrEmpty(resposta.Resposta)))
            {
                var id = await mediator.Send(new ObterArquivoIdPorCodigoQuery(Guid.Parse(item.Resposta)));
                arquivosId.Add(id);
            }
            return arquivosId.ToArray();
        }

        private async Task<bool> RespostaFoiRemovida(QuestaoEncaminhamentoNAAPA questaoExistente, IGrouping<long, EncaminhamentoNAAPASecaoQuestaoDto> respostasEncaminhamento)
        {
            if (EnumExtension.EhUmDosValores(questaoExistente.Questao.Tipo, new Enum[] { TipoQuestao.ComboMultiplaEscolha }))
                return questaoExistente.Respostas.Any(resposta => !respostasEncaminhamento.Any(respostaEncaminhamento => respostaEncaminhamento.Resposta.Equals(resposta.RespostaId.ToString())));
            else
            if (EnumExtension.EhUmDosValores(questaoExistente.Questao.Tipo, new Enum[] { TipoQuestao.Upload }))
            {
                var arquivosId = await ObterArquivosIdRespostas(respostasEncaminhamento);
                return questaoExistente.Respostas.Any(resposta => !arquivosId.Any(id => id == (resposta.ArquivoId.Value)));
            }
            else
                return false;

        }

        private bool CampoFoiAlterado(RespostaEncaminhamentoNAAPA RespostaAtual, EncaminhamentoNAAPASecaoQuestaoDto respostaAlteracao)
        {
            var funcoes = new List<Func<RespostaEncaminhamentoNAAPA, EncaminhamentoNAAPASecaoQuestaoDto, bool?>> { CampoPorTextoFoiAlterado, CampoPorRespostaIdFoiAlterado, CampoPorJsonFoiAlterado };

            foreach(var funcao in funcoes)
            {
                var foiAlterado = funcao(RespostaAtual, respostaAlteracao);

                if (foiAlterado.HasValue)
                    return foiAlterado.Value;
            }
            return false;
        }

        private bool? CampoPorTextoFoiAlterado(RespostaEncaminhamentoNAAPA RespostaAtual, EncaminhamentoNAAPASecaoQuestaoDto respostaAlteracao)
        {
            if (EnumExtension.EhUmDosValores(respostaAlteracao.TipoQuestao, new Enum[] { TipoQuestao.Frase, TipoQuestao.Texto, TipoQuestao.EditorTexto,
                                                                                         TipoQuestao.Data, TipoQuestao.Numerico }))
                return RespostaAtual.Texto != respostaAlteracao.Resposta;

            return null;
        }

        private bool? CampoPorJsonFoiAlterado(RespostaEncaminhamentoNAAPA RespostaAtual, EncaminhamentoNAAPASecaoQuestaoDto respostaAlteracao)
        {
            if (EhCampoLista(respostaAlteracao))
                return ObterCampoJsonSemId(RespostaAtual.Texto) != ObterCampoJsonSemId(respostaAlteracao.Resposta);
            
            return null;
        }

        private bool? CampoPorRespostaIdFoiAlterado(RespostaEncaminhamentoNAAPA RespostaAtual, EncaminhamentoNAAPASecaoQuestaoDto respostaAlteracao)
        {
            if (EnumExtension.EhUmDosValores(respostaAlteracao.TipoQuestao, new Enum[] { TipoQuestao.Radio, TipoQuestao.Combo, TipoQuestao.Checkbox, TipoQuestao.ComboMultiplaEscolha }))
                return RespostaAtual.RespostaId != (!string.IsNullOrEmpty(respostaAlteracao.Resposta) ? long.Parse(respostaAlteracao.Resposta) : null);

            return null;
        }

        private bool EhCampoLista(EncaminhamentoNAAPASecaoQuestaoDto respostaAlteracao)
        {
            return EnumExtension.EhUmDosValores(respostaAlteracao.TipoQuestao, new Enum[] { TipoQuestao.Endereco, TipoQuestao.ContatoResponsaveis,
                                                                                            TipoQuestao.AtividadesContraturno, TipoQuestao.TurmasPrograma});
        }

        private string ObterCampoJsonSemId(string campo)
        {
            return Regex.Replace(campo, @"""id"":""(.*?)""", "");
        }

        private async Task<string> ObterNomeQuestao(Questao questao)
        {
            var nomeQuestao = await ObterNomeQuestaoObservacao(questao);

            return string.IsNullOrEmpty(nomeQuestao) ? questao?.Nome : nomeQuestao;
        }

        private async Task<string> ObterNomeQuestaoObservacao(Questao questaoFilha)
        {
            if ((questaoFilha != null) && (questaoFilha.NomeComponente?.StartsWith("OBS_") == true))
            {
                var nomeComponentePai = questaoFilha.NomeComponente.Substring(4);
                var questaoPai = await repositorioQuestao.ObterPorNomeComponente(nomeComponentePai);

                return $"{questaoPai?.Nome} - {questaoFilha.Nome}"; 
            }

            return string.Empty;
        }
    }
}
