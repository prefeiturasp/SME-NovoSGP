using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExecutarMigracaoRelatorioPAPPorIdUseCase : AbstractUseCase, IExecutarMigracaoRelatorioPAPPorIdUseCase
    {
        private const long ID_SECAO_SEMESTRAL_OUTROS = 5;
        private const string NOME_COMPONENTE_DIFIC_APRESENTADAS = "DIFIC_APRESENTADAS";
        private const string NOME_COMPONENTE_OBS = "OBS_OBS";

        public ExecutarMigracaoRelatorioPAPPorIdUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var migrador = param.ObterObjetoMensagem<MigradorRelatorioPAPDto>();
            var alunoPAP = await mediator.Send(new ObterRelatorioSemestralPorIdAlunoPAPQuery(migrador.IdRelatorioSemestralAlunoPAP));
            var alunoTurma = (await mediator.Send(new ObterTurmaAlunoPorCodigoAlunoQuery(alunoPAP.AlunoCodigo)))?.FirstOrDefault(); ;
            var secoesPAPSemestral = await mediator.Send(new ObterSecoesPAPSemestralQuery());
            var questoes = await mediator.Send(new ObterQuestoesMigracaoQuery());
            var respostaDificuldadeApresentada = await ObterSecaoDificuldadeApresentada(questoes);

            var dtoRelatorioPeriodico = Converte(
                                            alunoPAP,
                                            alunoTurma?.NomeAluno,
                                            migrador.IdPeriodoRelatorioPAP,
                                            secoesPAPSemestral,
                                            questoes,
                                            respostaDificuldadeApresentada);

            if (dtoRelatorioPeriodico.NaoEhNulo())
            {
                await SalvarRelatorioPAP(dtoRelatorioPeriodico);
            }

            return true;
        }

        private async Task SalvarRelatorioPAP(RelatorioPAPDto relatorioPAPDto)
        {
            var resultado = new ResultadoRelatorioPAPDto();
            var relatorioTurma = await mediator.Send(new SalvarRelatorioPeriodicoTurmaPAPCommand(relatorioPAPDto.TurmaId, relatorioPAPDto.periodoRelatorioPAPId));
            var relatorioAluno = await PersistirRelatorioAluno(relatorioPAPDto, relatorioTurma.Id);

            resultado.PAPTurmaId = relatorioTurma.Id;
            resultado.PAPAlunoId = relatorioAluno.Id;

            foreach (var secao in relatorioPAPDto.Secoes)
            {
                await SalvarSecao(secao, relatorioAluno.Id);
            }
        }

        private async Task<RelatorioPeriodicoPAPAluno> PersistirRelatorioAluno(RelatorioPAPDto relatorioPAPDto, long relatorioTurmaId)
        {
            if (relatorioPAPDto.PAPAlunoId.HasValue)
                return await mediator.Send(new ObterRelatorioPeriodicoAlunoPAPQuery(relatorioPAPDto.PAPAlunoId.Value));

            return await mediator.Send(new SalvarRelatorioPeriodicoAlunoPAPCommand(relatorioPAPDto.AlunoCodigo,
                                                                                   relatorioPAPDto.AlunoNome,
                                                                                   relatorioTurmaId));
        }

        private async Task SalvarSecao(RelatorioPAPSecaoDto secao, long relatorioAlunoId)
        {
            var relatorioSecao = await PersistirRelatorioSecao(secao, relatorioAlunoId);

            foreach (var questoes in secao.Respostas.GroupBy(q => q.QuestaoId))
            {
                await SalvarQuestao(relatorioSecao.Id, questoes.Key, questoes);
            }
        }

        private async Task<RelatorioPeriodicoPAPSecao> PersistirRelatorioSecao(RelatorioPAPSecaoDto secao, long relatorioAlunoId)
        {
            if (!secao.Respostas.Any())
                throw new NegocioException(string.Format(MensagemNegocioComuns.NENHUMA_QUESTAO_FOI_ENCONTRADA_NA_SECAO_X, secao.SecaoId));

            if (secao.Id.HasValue)
            {
                var relatorioSecao = await mediator.Send(new ObterRelatorioPeriodicoSecaoPAPQuery(secao.Id.Value));

                await mediator.Send(new AlterarRelatorioPeriodicoSecaoPAPCommand(relatorioSecao));

                return relatorioSecao;
            }

            return await mediator.Send(new SalvarRelatorioPeriodicoSecaoPAPCommand(secao.SecaoId, relatorioAlunoId));
        }

        private async Task SalvarQuestao(long relatorioSecaoId, long questaoId, IEnumerable<RelatorioPAPRespostaDto> respostas)
        {
            var relatorioQuestao = await mediator.Send(new SalvarRelatorioPeriodicoQuestaoPAPCommand(relatorioSecaoId, questaoId));

            await SalvarResposta(respostas, relatorioQuestao.Id);
        }

        private async Task SalvarResposta(IEnumerable<RelatorioPAPRespostaDto> respostas, long relatorioQuestaoId)
        {
            foreach (var resposta in respostas)
            {
                await mediator.Send(new SalvarRelatorioPeriodicoRespostaPAPCommand(resposta.Resposta, resposta.TipoQuestao, relatorioQuestaoId));
            }
        }

        private async Task<RelatorioPAPSecaoDto> ObterSecaoDificuldadeApresentada(IEnumerable<Questao> questoes)
        {
            const long ID_SECAO_PERIODICO_DIFIC_APRES = 2;
            const int ORDEM_MIGRACAO = 56;
            var questao = questoes.FirstOrDefault(questao => questao.NomeComponente == NOME_COMPONENTE_DIFIC_APRESENTADAS);
            var opcaoResposta = await mediator.Send(new ObterOpcoesRespostaPorQuestaoIdQuery(questao.Id));
            var idResposta = opcaoResposta.LastOrDefault(opcao => opcao.Ordem == ORDEM_MIGRACAO).Id;

            return new RelatorioPAPSecaoDto()
            {
                SecaoId = ID_SECAO_PERIODICO_DIFIC_APRES,
                Respostas = new List<RelatorioPAPRespostaDto>()
                {
                     new RelatorioPAPRespostaDto()
                    {
                        QuestaoId = questao.Id,
                        Resposta = idResposta.ToString(),
                        TipoQuestao = questao.Tipo
                    }
                }
            };
        }

        private RelatorioPAPDto Converte(
                            RelatorioSemestralPAPAluno relatorioSemestral, 
                            string nomeAluno,
                            long  idPeriodo,
                            IEnumerable<SecaoRelatorioSemestralPAP> secoesPAPSemestral, 
                            IEnumerable<Questao> questoes,
                            RelatorioPAPSecaoDto secaoDificuldadeApresentada)
        {

            var secoes = Converte(relatorioSemestral.Secoes, secoesPAPSemestral, questoes).ToList();

            if (secoes.Any())
            {
                secoes.Add(secaoDificuldadeApresentada);

                return new RelatorioPAPDto
                {
                    AlunoCodigo = relatorioSemestral.AlunoCodigo,
                    AlunoNome = nomeAluno,
                    TurmaId = relatorioSemestral.RelatorioSemestralTurmaPAP.TurmaId,
                    periodoRelatorioPAPId = idPeriodo,
                    Secoes = secoes
                };
            }

            return null;
        }
        
        private IEnumerable<RelatorioPAPSecaoDto> Converte(List<RelatorioSemestralPAPAlunoSecao> secoesSemestral, IEnumerable<SecaoRelatorioSemestralPAP> secoesPAPSemestral, IEnumerable<Questao> questoes)
        {
            var secoes = new List<RelatorioPAPSecaoDto>();

            secoes.Add(ObterSecaoAvancoAprendizagem(secoesSemestral, secoesPAPSemestral, questoes));
            secoes.Add(ObterSecaoObservacao(secoesSemestral, secoesPAPSemestral, questoes));

            return secoes.Where(secao => secao.NaoEhNulo());
        }

        private RelatorioPAPSecaoDto ObterSecaoObservacao(
                                            List<RelatorioSemestralPAPAlunoSecao> secoesSemestral,
                                            IEnumerable<SecaoRelatorioSemestralPAP> secoesPAPSemestral,
                                            IEnumerable<Questao> questoes)
        {
            const long ID_SECAO_PERIODICO_OBS = 4;
            var secoesAvanco = secoesPAPSemestral.FirstOrDefault(secao => secao.Id == ID_SECAO_SEMESTRAL_OUTROS);

            if (secoesAvanco.NaoEhNulo())
            {
                var secaoAluno = secoesSemestral.FirstOrDefault(item => item.SecaoRelatorioSemestralPAPId == secoesAvanco.Id);

                if (secaoAluno.NaoEhNulo() && !string.IsNullOrEmpty(secaoAluno.Valor))
                {
                    var questao = questoes.FirstOrDefault(questao => questao.NomeComponente == NOME_COMPONENTE_OBS);

                    return new RelatorioPAPSecaoDto()
                    {
                        SecaoId = ID_SECAO_PERIODICO_OBS,
                        Respostas = new List<RelatorioPAPRespostaDto>()
                        {
                            new RelatorioPAPRespostaDto()
                            {
                                QuestaoId = questao.Id,
                                Resposta = secaoAluno.Valor,
                                TipoQuestao = questao.Tipo
                            }
                        }
                    };
                }
            }

            return null;
        }

        private RelatorioPAPSecaoDto ObterSecaoAvancoAprendizagem(
                                            List<RelatorioSemestralPAPAlunoSecao> secoesSemestral,
                                            IEnumerable<SecaoRelatorioSemestralPAP> secoesPAPSemestral,
                                            IEnumerable<Questao> questoes)
        {
            const long ID_SECAO_AVANC_APREND_BIMES = 3;
            const string NOME_COMPONENTE_AVANC_APREND_BIMES = "AVANC_APREND_BIMES";

            var secoesAvanco = secoesPAPSemestral.Where(secao => secao.Id != ID_SECAO_SEMESTRAL_OUTROS);
            var resposta = new StringBuilder();

            foreach (var secao in secoesAvanco)
            {
                var secaoAluno = secoesSemestral.FirstOrDefault(item => item.SecaoRelatorioSemestralPAPId == secao.Id);

                if (secaoAluno.NaoEhNulo() && !string.IsNullOrEmpty(secaoAluno.Valor))
                {
                    resposta.AppendLine($"<strong>{secao.Nome}</strong>");
                    resposta.AppendLine($"<strong>{secao.Descricao}</strong>");
                    resposta.AppendLine(secaoAluno.Valor);
                    resposta.AppendLine("<br>");
                }
            }

            if (resposta.Length > 0) {
                var questao = questoes.FirstOrDefault(questao => questao.NomeComponente == NOME_COMPONENTE_AVANC_APREND_BIMES);

                return new RelatorioPAPSecaoDto()
                {
                    SecaoId = ID_SECAO_AVANC_APREND_BIMES,
                    Respostas = new List<RelatorioPAPRespostaDto>()
                    {
                         new RelatorioPAPRespostaDto()
                        {
                            QuestaoId = questao.Id,
                            Resposta = resposta.ToString(),
                            TipoQuestao = questao.Tipo
                        }
                    }
                };
            }

            return null;
        }
    }
}
