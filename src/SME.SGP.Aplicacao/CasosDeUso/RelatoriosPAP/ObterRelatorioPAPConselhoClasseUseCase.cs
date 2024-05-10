using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.Questionario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterRelatorioPAPConselhoClasseUseCase : IObterRelatorioPAPConselhoClasseUseCase
    {
        private readonly IMediator mediator;
        private const string NOME_COMPONENTE_SECAO_FREQUENCIA_TURMA_PAP = "SECAO_FREQUENCIA";

        public ObterRelatorioPAPConselhoClasseUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<SecaoQuestoesDTO>> Executar(string codigoTurma, string codigoAluno, int bimestre)
        {
            var retorno = new List<SecaoQuestoesDTO>();
            var turma = await mediator.Send(new ObterTurmaPorCodigoQuery(codigoTurma)) ?? throw new NegocioException(MensagemNegocioTurma.TURMA_NAO_ENCONTRADA);
            var relatorioPAP = await mediator.Send(new ObterRelatorioPAPAlunoConselhoClasseQuery(turma.AnoLetivo, codigoAluno, bimestre, turma.ModalidadeTipoCalendario));
            if (relatorioPAP.NaoEhNulo())
            {
                var periodoPAP = await mediator.Send(new PeriodoRelatorioPAPQuery(relatorioPAP.PeriodoRelatorioPAPId));
                var secoesRelatorioPAP = await this.mediator.Send(new ObterSecoesPAPQuery(relatorioPAP.TurmaCodigo, relatorioPAP.AlunoCodigo, relatorioPAP.PeriodoRelatorioPAPId));
                foreach (var secao in secoesRelatorioPAP.Secoes.Where(s => !s.NomeComponente.Equals(NOME_COMPONENTE_SECAO_FREQUENCIA_TURMA_PAP)).OrderBy(s => s.Ordem))
                {
                    var questoesSecao = await this.mediator.Send(new ObterQuestionarioPAPQuery(relatorioPAP.TurmaCodigo, relatorioPAP.AlunoCodigo,
                                                                                               periodoPAP, secao.QuestionarioId, secao.PAPSecaoId));
                    retorno.Add(new SecaoQuestoesDTO()
                    {
                        Id = secao.Id,
                        Nome = secao.Nome,
                        NomeComponente = secao.NomeComponente,
                        Ordem = secao.Ordem,
                        QuestionarioId = secao.QuestionarioId,
                        Questoes = questoesSecao,   
                        TipoQuestionario = TipoQuestionario.RelatorioPAP
                    }); ;
                }
            }
            return retorno;
        }
    }
}
