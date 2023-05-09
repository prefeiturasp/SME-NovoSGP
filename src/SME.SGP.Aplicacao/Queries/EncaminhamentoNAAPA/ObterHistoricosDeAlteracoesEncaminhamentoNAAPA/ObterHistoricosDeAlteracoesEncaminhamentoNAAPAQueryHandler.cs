using MediatR;
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

        public ObterHistoricosDeAlteracoesEncaminhamentoNAAPAQueryHandler(IRepositorioQuestao repositorioQuestao, IMediator mediator)
        {
            camposInseridos = new List<string>();
            camposAlterados = new List<string>();
            this.repositorioQuestao = repositorioQuestao ?? throw new ArgumentNullException(nameof(repositorioQuestao));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            
        }

        public async Task<EncaminhamentoNAAPAHistoricoAlteracoes> Handle(ObterHistoricosDeAlteracoesEncaminhamentoNAAPAQuery request, CancellationToken cancellationToken)
        {
            this.usuarioLogado = await mediator.Send(new ObterUsuarioLogadoQuery());

            await ExecuteValidacaoAlteracaoCamposDaSecao(request.EncaminhamentoNAAPASecaoAlterado, request.EncaminhamentoNAAPASecaoExistente);

            return ObterHistoricoAlteracaoSecao(request.EncaminhamentoNAAPASecaoAlterado, request.EncaminhamentoNAAPASecaoExistente);
        }

        private EncaminhamentoNAAPAHistoricoAlteracoes ObterHistoricoAlteracaoSecao(EncaminhamentoNAAPASecaoDto encaminhamentoNAAPAAlterado, EncaminhamentoNAAPASecao encaminhamentoSecaoExistente)
        {
            if (camposInseridos.Any() || camposAlterados.Any())
            {
                return new EncaminhamentoNAAPAHistoricoAlteracoes()
                {
                    EncaminhamentoNAAPAId = encaminhamentoSecaoExistente.EncaminhamentoNAAPAId,
                    SecaoEncaminhamentoNAAPAId = encaminhamentoNAAPAAlterado.SecaoId,
                    DataHistorico = DateTimeExtension.HorarioBrasilia(),
                    TipoHistorico = TipoHistoricoAlteracoesEncaminhamentoNAAPA.Alteracao,
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

        private async Task ExecuteValidacaoAlteracaoCamposDaSecao(EncaminhamentoNAAPASecaoDto encaminhamentoNAAPASecaoAlterado, EncaminhamentoNAAPASecao encaminhamentoSecaoExistente)
        {
            foreach (var questaoAlterada in encaminhamentoNAAPASecaoAlterado.Questoes.GroupBy(q => q.QuestaoId))
            {
                var questaoExistente = encaminhamentoSecaoExistente?.Questoes?.Find(questao => questao.QuestaoId == questaoAlterada.Key);

                await AdicionarCamposInseridos(questaoExistente, questaoAlterada);
                await AdicionarCamposAlterados(questaoExistente, questaoAlterada);
            }

            AdicionarCamposExcluidos(encaminhamentoSecaoExistente, encaminhamentoNAAPASecaoAlterado);
        }


        private async Task AdicionarCamposInseridos(QuestaoEncaminhamentoNAAPA questaoExistente, IGrouping<long, EncaminhamentoNAAPASecaoQuestaoDto> respostas)
        {
            var novasRespostas = respostas.ToList().Find(c => c.RespostaEncaminhamentoId == 0);

            if (novasRespostas != null)
            {
                var ids = new long[] { novasRespostas.QuestaoId };
                var questao = questaoExistente?.Questao ?? (await repositorioQuestao.ObterQuestoesPorIds(ids)).FirstOrDefault();
                var nomeQuestao = questao?.Nome;

                if (CampoPodeSerIncluido(questaoExistente, novasRespostas))
                    camposInseridos.Add(nomeQuestao);
                else if (CampoPodeSerAlterado(questaoExistente, novasRespostas))
                    camposAlterados.Add(nomeQuestao);
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

        private bool CampoPodeSerIncluido(
                        QuestaoEncaminhamentoNAAPA questaoExistente, 
                        EncaminhamentoNAAPASecaoQuestaoDto respostasEncaminhamento)
        {
            if (EnumExtension.EhUmDosValores(respostasEncaminhamento.TipoQuestao, new Enum[] { TipoQuestao.Checkbox, TipoQuestao.ComboMultiplaEscolha }))
                return questaoExistente == null || !questaoExistente.Respostas.Any();

            if (EhCampoLista(respostasEncaminhamento))
                return !String.IsNullOrEmpty(respostasEncaminhamento.Resposta) && respostasEncaminhamento.Resposta != "[]";

            return true;
        }

        private bool CampoPodeSerAlterado(
                            QuestaoEncaminhamentoNAAPA questaoExistente,
                            EncaminhamentoNAAPASecaoQuestaoDto respostasEncaminhamento)
        {
            if (EhCampoLista(respostasEncaminhamento))
                return ((questaoExistente == null && respostasEncaminhamento.Resposta != "[]") ||
                        (questaoExistente?.Respostas?.FirstOrDefault()?.Texto != respostasEncaminhamento.Resposta));

            return EnumExtension.EhUmDosValores(respostasEncaminhamento.TipoQuestao, new Enum[] { TipoQuestao.Checkbox, TipoQuestao.ComboMultiplaEscolha });
        }

        private async Task AdicionarCamposAlterados(QuestaoEncaminhamentoNAAPA questaoExistente, IGrouping<long, EncaminhamentoNAAPASecaoQuestaoDto> respostasEncaminhamento)
        { 
            var respostasExistentes = questaoExistente?.Respostas?.Where(s => respostasEncaminhamento.Any(c => c.RespostaEncaminhamentoId == s.Id));

            if (respostasExistentes != null)
            {
                foreach (var respostaExistente in respostasExistentes)
                {
                    var respostaAlterada = respostasEncaminhamento.ToList().Find(resposta => resposta.RespostaEncaminhamentoId == respostaExistente.Id);

                    if (await CampoFoiAlterado(respostaExistente, respostaAlterada))
                        camposAlterados.Add(questaoExistente.Questao?.Nome);
                }
            }
        }

        private async Task<bool> CampoFoiAlterado(RespostaEncaminhamentoNAAPA RespostaAtual, EncaminhamentoNAAPASecaoQuestaoDto respostaAlteracao)
        {
            var funcoes = new List<Func<RespostaEncaminhamentoNAAPA, EncaminhamentoNAAPASecaoQuestaoDto, bool?>> { CampoPorTextoFoiAlterado, CampoPorRespostaIdFoiAlterado, CampoPorJsonFoiAlterado };

            foreach(var funcao in funcoes)
            {
                var foiAlterado = funcao(RespostaAtual, respostaAlteracao);

                if (foiAlterado.HasValue)
                    return foiAlterado.Value;
            }

            return await CampoPorArquivoFoiAlterado(RespostaAtual, respostaAlteracao);
        }

        private async Task<bool> CampoPorArquivoFoiAlterado(RespostaEncaminhamentoNAAPA RespostaAtual, EncaminhamentoNAAPASecaoQuestaoDto respostaAlteracao)
        {
            if (!string.IsNullOrEmpty(respostaAlteracao.Resposta) && respostaAlteracao.TipoQuestao == TipoQuestao.Upload)
                return RespostaAtual.ArquivoId != await mediator.Send(new ObterArquivoIdPorCodigoQuery(Guid.Parse(respostaAlteracao.Resposta)));

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
    }
}
