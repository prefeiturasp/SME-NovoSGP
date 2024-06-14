using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.MapeamentoEstudantes;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterInformacoesAtualizadasAlunoMapeamentoEstudanteQueryHandler : ConsultasBase, IRequestHandler<ObterInformacoesAtualizadasAlunoMapeamentoEstudanteQuery, InformacoesAtualizadasMapeamentoEstudanteAlunoDto>
    {
        public IRepositorioMapeamentoEstudante repositorioMapeamento { get; }

        public ObterInformacoesAtualizadasAlunoMapeamentoEstudanteQueryHandler(IContextoAplicacao contextoAplicacao, IRepositorioMapeamentoEstudante repositorioMapeamento) : base(contextoAplicacao)
        {
            this.repositorioMapeamento = repositorioMapeamento ?? throw new ArgumentNullException(nameof(repositorioMapeamento));
        }

        public Task<InformacoesAtualizadasMapeamentoEstudanteAlunoDto> Handle(ObterInformacoesAtualizadasAlunoMapeamentoEstudanteQuery request, CancellationToken cancellationToken)
        => repositorioMapeamento.ObterInformacoesAtualizadasAlunoMapeamentoEstudante(request.CodigoAluno, request.AnoLetivo, request.Bimestre);
    }
}
