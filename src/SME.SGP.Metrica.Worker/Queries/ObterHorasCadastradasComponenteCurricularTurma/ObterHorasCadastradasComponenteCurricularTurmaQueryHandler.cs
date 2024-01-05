using MediatR;
using Nest;
using Newtonsoft.Json;
using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Utilitarios;
using SME.SGP.Metrica.Worker.Repositorios.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Metrica.Worker.Queries
{
    public class ObterHorasCadastradasComponenteCurricularTurmaQueryHandler : IRequestHandler<ObterHorasCadastradasComponenteCurricularTurmaQuery, int>
    {
        private readonly IRepositorioSGPConsulta repositorioSGP;
        
        public ObterHorasCadastradasComponenteCurricularTurmaQueryHandler(IRepositorioSGPConsulta repositorioSGP)
        {
            this.repositorioSGP = repositorioSGP ?? throw new ArgumentNullException(nameof(repositorioSGP));
        }

        public async Task<int> Handle(ObterHorasCadastradasComponenteCurricularTurmaQuery request, CancellationToken cancellationToken)
        {
            var semana = UtilData.ObterSemanaDoAno(request.DataAula);
            var ehExperienciaPedagogica = EhComponenteExperienciaPedagogica(request.ComponenteCurricularCodigo);
            if (request.EhRegencia)
                return ehExperienciaPedagogica ?
                    await repositorioSGP.ObterQuantidadeAulasTurmaExperienciasPedagogicasDia(request.Turma.CodigoTurma, request.DataAula) :
                    await repositorioSGP.ObterQuantidadeAulasTurmaComponenteCurricularDia(request.Turma.CodigoTurma, request.ComponenteCurricularCodigo, request.DataAula);

            // Busca horas aula cadastradas para a disciplina na turma
            return ehExperienciaPedagogica ?
                await repositorioSGP.ObterQuantidadeAulasTurmaExperienciasPedagogicasSemana(request.Turma.CodigoTurma, semana, request.ComponenteCurricularCodigo) :
                await repositorioSGP.ObterQuantidadeAulasTurmaDisciplinaSemana(request.Turma.CodigoTurma, request.ComponenteCurricularCodigo, semana);
        }

        private bool EhComponenteExperienciaPedagogica(string componenteCurricularCodigo)
        => new string[] { "1214", "1215", "1216", "1217", "1218", "1219", "1220", "1221", "1222", "1223" }.Contains(componenteCurricularCodigo);
    }
}
