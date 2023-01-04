using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;
using SME.SGP.Infra.Interfaces;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterRespostaEnderecoAlunoEncaminhamentoNAAPAPorIdQueryHandler : ConsultasBase, IRequestHandler<ObterRespostaEnderecoAlunoEncaminhamentoNAAPAPorIdQuery, RespostaEncaminhamentoNAAPA>
    {
        private readonly IRepositorioRespostaEncaminhamentoNAAPA repositorioEncaminhamentoNaapaResposta;
        public ObterRespostaEnderecoAlunoEncaminhamentoNAAPAPorIdQueryHandler(IContextoAplicacao contextoAplicacao, IRepositorioRespostaEncaminhamentoNAAPA repositorioEncaminhamentoNaapaResposta) : base(contextoAplicacao)
        {
            this.repositorioEncaminhamentoNaapaResposta = repositorioEncaminhamentoNaapaResposta ?? throw new ArgumentNullException(nameof(repositorioEncaminhamentoNaapaResposta));
        }

        public async Task<RespostaEncaminhamentoNAAPA> Handle(ObterRespostaEnderecoAlunoEncaminhamentoNAAPAPorIdQuery request, CancellationToken cancellationToken)
        {
            return (await repositorioEncaminhamentoNaapaResposta.ObterRespostaEnderecoResidencialPorEncaminhamentoId(request.EncaminhamentoNAAPAId)).FirstOrDefault();
        }
    }
}
