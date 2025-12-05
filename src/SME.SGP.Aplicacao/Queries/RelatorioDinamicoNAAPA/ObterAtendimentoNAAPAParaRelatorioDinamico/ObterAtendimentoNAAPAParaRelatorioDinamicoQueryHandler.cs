using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interfaces;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAtendimentoNAAPAParaRelatorioDinamicoQueryHandler : ConsultasBase, IRequestHandler<ObterAtendimentoNAAPAParaRelatorioDinamicoQuery, RelatorioDinamicoNAAPADto>
    {
        private readonly IRepositorioRelatorioDinamicoNAAPA repositorio;
        private readonly IRepositorioQuestionario repositorioQuestionario;
        

        public ObterAtendimentoNAAPAParaRelatorioDinamicoQueryHandler(
                            IContextoAplicacao contextoAplicacao,
                            IRepositorioRelatorioDinamicoNAAPA repositorio,
                            IRepositorioQuestionario repositorioQuestionario) : base(contextoAplicacao)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
            this.repositorioQuestionario = repositorioQuestionario ?? throw new ArgumentNullException(nameof(repositorioQuestionario));
        }

        public async Task<RelatorioDinamicoNAAPADto> Handle(ObterAtendimentoNAAPAParaRelatorioDinamicoQuery request, CancellationToken cancellationToken)
        {
            string[] nomesComponentesTotalizadoresAtendimento = { "PROCEDIMENTO_DE_TRABALHO", "TIPO_DO_ATENDIMENTO" };
            var questoesParaTotalizadoresAtendimento = await repositorioQuestionario.ObterQuestoesPorNomesComponentes(nomesComponentesTotalizadoresAtendimento, TipoQuestionario.EncaminhamentoNAAPA);
            

            return await repositorio.ObterRelatorioDinamicoNAAPA(request.Filtro, Paginacao, questoesParaTotalizadoresAtendimento);
        }
    }
}
