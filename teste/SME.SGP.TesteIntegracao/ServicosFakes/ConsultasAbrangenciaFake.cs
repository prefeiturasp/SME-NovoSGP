using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dto;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.ServicosFakes
{
    public class ConsultasAbrangenciaFake : IConsultasAbrangencia
    {
        public Task<IEnumerable<AbrangenciaFiltroRetorno>> ObterAbrangenciaPorfiltro(string texto, bool consideraHistorico, bool consideraNovosAnosInfantil = false)
        {
            throw new NotImplementedException();
        }

        public async Task<AbrangenciaFiltroRetorno> ObterAbrangenciaTurma(string turma, bool consideraHistorico = false)
        {
            var retorno =  new List<AbrangenciaFiltroRetorno>
            {
                new AbrangenciaFiltroRetorno
                {
                    Modalidade = Modalidade.EJA,
                    AnoLetivo = 2022,
                    Ano = "2",
                    CodigoDre = "1",
                    CodigoTurma = "2366531",
                    CodigoUe = "1",
                    TipoEscola = TipoEscola.EMEFM,
                    NomeDre = "DIRETORIA REGIONAL DE EDUCACAO SANTO AMARO",
                    NomeTurma ="2A",
                    NomeUe = "LINNEU PRESTES, PROF.",
                    Semestre = 2,
                    QtDuracaoAula = 2,
                    TipoTurno = 5,
                }
            };

            return retorno.Where(x => x.CodigoTurma == turma).FirstOrDefault();
        }

        public Task<IEnumerable<int>> ObterAnosLetivos(bool consideraHistorico, int anoMinimo)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<int>> ObterAnosLetivosTodos()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<OpcaoDropdownDto>> ObterAnosTurmasPorUeModalidade(string codigoUe, Modalidade modalidade, bool consideraHistorico)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<long>> ObterCodigoTurmasAbrangencia(string codigoUe, Modalidade modalidade, int periodo, bool consideraHistorico, int anoLetivo, int[] tipos, bool desconsideraNovosAnosInfantil = false)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<AbrangenciaDreRetornoDto>> ObterDres(Modalidade? modalidade, int periodo = 0, bool consideraHistorico = false, int anoLetivo = 0, string filtro = "")
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<int>> ObterSemestres(Modalidade modalidade, bool consideraHistorico, int anoLetivo = 0, string dreCodigo = null, string ueCodigo = null)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<AbrangenciaTurmaRetorno>> ObterTurmas(string codigoUe, Modalidade modalidade, int periodo, bool consideraHistorico, int anoLetivo, int[] tipos, bool desconsideraNovosAnosInfantil = false)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<AbrangenciaTurmaRetorno>> ObterTurmasPrograma(string codigoUe, Modalidade modalidade, int periodo = 0, bool consideraHistorico = false, int anoLetivo = 0)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<AbrangenciaTurmaRetorno>> ObterTurmasRegulares(string codigoUe, Modalidade modalidade, int periodo = 0, bool consideraHistorico = false, int anoLetivo = 0)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<AbrangenciaUeRetorno>> ObterUes(string codigoDre, Modalidade? modalidade, int periodo = 0, bool consideraHistorico = false, int anoLetivo = 0, bool consideraNovasUEs = false)
        {
            throw new NotImplementedException();
        }
    }
}
