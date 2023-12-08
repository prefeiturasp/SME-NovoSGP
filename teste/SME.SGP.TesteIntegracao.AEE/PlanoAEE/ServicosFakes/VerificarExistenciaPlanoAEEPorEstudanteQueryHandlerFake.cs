using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.TesteIntegracao.PlanoAEE.ServicosFakes
{
    public class VerificarExistenciaPlanoAEEPorEstudanteQueryHandlerFake : IRequestHandler<VerificarExistenciaPlanoAEEPorEstudanteQuery, PlanoAEEResumoDto>
    {
        public async Task<PlanoAEEResumoDto> Handle(VerificarExistenciaPlanoAEEPorEstudanteQuery request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(new PlanoAEEResumoDto()
            {
                Id =1,
                Numero = 1,
                Turma = "1A",
                PossuiEncaminhamentoAEE = false,
                EhAtendidoAEE = false,
                RfReponsavel = "2222222",
                NomeReponsavel = "Nome do Responsavel",
                Situacao = "Validado",
                CriadoEm = DateTimeExtension.HorarioBrasilia(),
                Versao = "1",
                CodigoAluno = "22222"
            });
        }
    }
}