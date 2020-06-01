using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands.Aula.InserirAula
{
    public class InserirAulaUnicaCommandHandler : IRequestHandler<InserirAulaUnicaCommand, RetornoBaseDto>
    {
        private readonly IRepositorioAula repositorioAula;
        private readonly IServicoDiaLetivo servicoDiaLetivo;
        private readonly IRepositorioAtribuicaoCJ repositorioAtribuicaoCJ;

        public InserirAulaUnicaCommandHandler(IRepositorioAula repositorioAula,
                                              IServicoDiaLetivo servicoDiaLetivo,
                                              IRepositorioAtribuicaoCJ repositorioAtribuicaoCJ)
        {
            this.repositorioAula = repositorioAula ?? throw new System.ArgumentNullException(nameof(repositorioAula));
            this.servicoDiaLetivo = servicoDiaLetivo ?? throw new System.ArgumentNullException(nameof(servicoDiaLetivo));
            this.repositorioAtribuicaoCJ = repositorioAtribuicaoCJ ?? throw new System.ArgumentNullException(nameof(repositorioAtribuicaoCJ));
        }
        public async Task<RetornoBaseDto> Handle(InserirAulaUnicaCommand request, CancellationToken cancellationToken)
        {
            var retorno = new RetornoBaseDto();

            var aulasExistentes = await repositorioAula.ObterAulasPorDataTurmaDisciplinaProfessorRf(request.DataAula, request.Turma.CodigoTurma, request.ComponenteCurricularId, request.CodigoRfProfessor);
            if (aulasExistentes != null)
                throw new NegocioException("Já existe uma aula criada neste dia para este componente curricular");

            var temLiberacaoExcepcionalNessaData = servicoDiaLetivo.ValidaSeEhLiberacaoExcepcional(request.DataAula, request.TipoCalendario.Id, request.UeId);
            
            var diaLetivo = servicoDiaLetivo.ValidarSeEhDiaLetivo(request.DataAula, request.TipoCalendario.Id, null, request.UeId);

            if (!temLiberacaoExcepcionalNessaData && !diaLetivo)
                throw new NegocioException("Não é possível cadastrar essa aula pois a data informada está fora do período letivo.");

            return retorno;
        }
    }
}
