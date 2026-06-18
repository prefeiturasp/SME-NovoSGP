using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Metrica.Worker.Repositorios.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Metrica.Worker.Queries
{
    public class ObterHorasGradeComponenteCurricularTurmaQueryHandler : IRequestHandler<ObterHorasGradeComponenteCurricularTurmaQuery, int>
    {
        private readonly IRepositorioSGPConsulta repositorioSGP;
        private const string ComponenteCurricularSRM = "1030";

        public ObterHorasGradeComponenteCurricularTurmaQueryHandler(IRepositorioSGPConsulta repositorioSGP)
        {
            this.repositorioSGP = repositorioSGP ?? throw new ArgumentNullException(nameof(repositorioSGP));
        }

        public async Task<int> Handle(ObterHorasGradeComponenteCurricularTurmaQuery request, CancellationToken cancellationToken)
        {
            if (request.EhRegencia)
                return request.Turma.ObterHorasGradeRegencia();

            if (request.ComponenteCurricularCodigo.Equals(ComponenteCurricularSRM)) 
                return 4;

            int.TryParse(request.Turma.Ano, out int ano);
            // Busca grade a partir dos dados da abrangencia da turma
            var grade = await repositorioSGP.ObterGradeTurmaAno(request.Turma.Ue.TipoEscola, request.Turma.ModalidadeCodigo,
                                                                request.Turma.QuantidadeDuracaoAula, ano, request.Turma.AnoLetivo.ToString());
            if (grade.EhNulo())
                return 0;

            return await repositorioSGP.ObterHorasComponente(grade.Id, new long[] { long.Parse(request.ComponenteCurricularCodigo) }, ano);
        }
    }
}
