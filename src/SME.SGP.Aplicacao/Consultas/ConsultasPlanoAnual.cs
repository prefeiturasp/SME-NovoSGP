using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsultasPlanoAnual : IConsultasPlanoAnual
    {
        private readonly IConsultasObjetivoAprendizagem consultasObjetivoAprendizagem;
        private readonly IRepositorioComponenteCurricular repositorioComponenteCurricular;
        private readonly IRepositorioPeriodoEscolar repositorioPeriodoEscolar;
        private readonly IRepositorioPlanoAnual repositorioPlanoAnual;
        private readonly IRepositorioTipoCalendario repositorioTipoCalendario;
        private readonly IRepositorioTurma repositorioTurma;
        private readonly IServicoEOL servicoEOL;
        private readonly IServicoUsuario servicoUsuario;

        public ConsultasPlanoAnual(IRepositorioPlanoAnual repositorioPlanoAnual,
                                   IConsultasObjetivoAprendizagem consultasObjetivoAprendizagem,
                                   IRepositorioPeriodoEscolar repositorioPeriodoEscolar,
                                   IRepositorioTipoCalendario repositorioTipoCalendario,
                                   IRepositorioTurma repositorioTurma,
                                   IRepositorioComponenteCurricular repositorioComponenteCurricular,
                                   IServicoUsuario servicoUsuario,
                                   IServicoEOL servicoEOL)
        {
            this.repositorioPlanoAnual = repositorioPlanoAnual ?? throw new System.ArgumentNullException(nameof(repositorioPlanoAnual));
            this.consultasObjetivoAprendizagem = consultasObjetivoAprendizagem ?? throw new System.ArgumentNullException(nameof(consultasObjetivoAprendizagem));
            this.repositorioPeriodoEscolar = repositorioPeriodoEscolar ?? throw new ArgumentNullException(nameof(repositorioPeriodoEscolar));
            this.repositorioTipoCalendario = repositorioTipoCalendario ?? throw new ArgumentNullException(nameof(repositorioTipoCalendario));
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
            this.repositorioComponenteCurricular = repositorioComponenteCurricular ?? throw new ArgumentNullException(nameof(repositorioComponenteCurricular));
            this.servicoUsuario = servicoUsuario ?? throw new ArgumentNullException(nameof(servicoUsuario));
            this.servicoEOL = servicoEOL ?? throw new ArgumentNullException(nameof(servicoEOL));
        }

        public async Task<PlanoAnualCompletoDto> ObterBimestreExpandido(FiltroPlanoAnualBimestreExpandidoDto filtro)
        {
            var planoAnualLista = new List<PlanoAnualCompletoDto>();

            var bimestres = filtro.ModalidadePlanoAnual == Modalidade.EJA ? 2 : 4;

            var filtroPlanoAnualDto = ObtenhaFiltro(filtro.AnoLetivo, filtro.ComponenteCurricularEolId, filtro.EscolaId, filtro.TurmaId, 0);

            for (int i = 1; i <= bimestres; i++)
            {
                filtroPlanoAnualDto.Bimestre = i;

                planoAnualLista.Add(await ObterPorEscolaTurmaAnoEBimestre(filtroPlanoAnualDto));
            }

            var periodosEscolares = ObterPeriodoEscolar(filtro.AnoLetivo, filtro.ModalidadePlanoAnual);

            if (periodosEscolares == null)
                return null;

            var retorno = planoAnualLista.FirstOrDefault(x => VerificaSeBimestreEhExpandido(periodosEscolares, x.Bimestre));

            return retorno;
        }

        public async Task<long> ObterIdPlanoAnualPorAnoEscolaBimestreETurma(int ano, string escolaId, long turmaId, int bimestre, long disciplinaId)
        {
            var plano = repositorioPlanoAnual.ObterPlanoAnualSimplificadoPorAnoEscolaBimestreETurma(ano, escolaId, turmaId, bimestre, disciplinaId);
            return plano != null ? plano.Id : 0;
        }

        public async Task<PlanoAnualObjetivosDisciplinaDto> ObterObjetivosEscolaTurmaDisciplina(FiltroPlanoAnualDisciplinaDto filtro)
        {
            var planoAnual = repositorioPlanoAnual.ObterPlanoObjetivosEscolaTurmaDisciplina(filtro.AnoLetivo,
                                                            filtro.EscolaId,
                                                            filtro.TurmaId,
                                                            filtro.Bimestre,
                                                            filtro.ComponenteCurricularEolId,
                                                            filtro.DisciplinaId);
            if (planoAnual == null)
                return null;

            var objetivosAprendizagem = await consultasObjetivoAprendizagem.Listar();

            if (objetivosAprendizagem is null || !objetivosAprendizagem.Any())
                throw new NegocioException("Não foi possível carregar os objetivos de aprendizagem por conta de problemas de comunicação com o Currículo da Cidade.");

            if (planoAnual.IdsObjetivosAprendizagem == null)
                return planoAnual;

            foreach (var idObjetivo in planoAnual.IdsObjetivosAprendizagem)
            {
                var objetivo = objetivosAprendizagem.FirstOrDefault(c => c.Id == idObjetivo);

                if (objetivo != null)
                    planoAnual.ObjetivosAprendizagem.Add(objetivo);
            }

            return planoAnual;
        }

        public async Task<PlanoAnualCompletoDto> ObterPorEscolaTurmaAnoEBimestre(FiltroPlanoAnualDto filtroPlanoAnualDto, bool seNaoExistirRetornaNovo = true)
        {
            var planoAnual = repositorioPlanoAnual.ObterPlanoAnualCompletoPorAnoEscolaBimestreETurma(filtroPlanoAnualDto.AnoLetivo, filtroPlanoAnualDto.EscolaId, filtroPlanoAnualDto.TurmaId, filtroPlanoAnualDto.Bimestre, filtroPlanoAnualDto.ComponenteCurricularEolId);

            if (planoAnual != null)
            {
                var objetivosAprendizagem = await consultasObjetivoAprendizagem.Listar();

                if (planoAnual.IdsObjetivosAprendizagem == null)
                    return planoAnual;

                foreach (var idObjetivo in planoAnual.IdsObjetivosAprendizagem)
                {
                    var objetivo = objetivosAprendizagem.FirstOrDefault(c => c.Id == idObjetivo);
                    if (objetivo != null)
                    {
                        planoAnual.ObjetivosAprendizagem.Add(objetivo);
                    }
                }
            }
            else if (seNaoExistirRetornaNovo)
                planoAnual = ObterNovoPlanoAnual(filtroPlanoAnualDto.TurmaId, filtroPlanoAnualDto.AnoLetivo, filtroPlanoAnualDto.EscolaId);

            return planoAnual;
        }

        public async Task<IEnumerable<PlanoAnualCompletoDto>> ObterPorUETurmaAnoEComponenteCurricular(string ueId, string turmaId, int anoLetivo, long componenteCurricularEolId)
        {
            var periodos = ObterPeriodoEscolar(turmaId, anoLetivo);
            var dataAtual = DateTime.Now.Date;
            var listaPlanoAnual = repositorioPlanoAnual.ObterPlanoAnualCompletoPorAnoUEETurma(anoLetivo, ueId, turmaId, componenteCurricularEolId);
            var componentesCurricularesEol = repositorioComponenteCurricular.Listar();
            if (listaPlanoAnual != null && listaPlanoAnual.Any())
            {
                var objetivosAprendizagem = await consultasObjetivoAprendizagem.Listar();
                foreach (var planoAnual in listaPlanoAnual)
                {
                    var periodo = periodos.FirstOrDefault(c => c.Bimestre == planoAnual.Bimestre);
                    if (periodo == null)
                        throw new NegocioException("Plano anual com data fora do período escolar. Contate o suporte.");
                    if (periodo.PeriodoFim.Local() >= dataAtual && periodo.PeriodoInicio.Local() <= dataAtual)
                    {
                        planoAnual.Obrigatorio = true;
                    }
                    if (planoAnual.IdsObjetivosAprendizagem == null)
                        continue;

                    foreach (var idObjetivo in planoAnual.IdsObjetivosAprendizagem)
                    {
                        var objetivo = objetivosAprendizagem.FirstOrDefault(c => c.Id == idObjetivo);
                        if (objetivo != null)
                        {
                            var componenteCurricularEol = componentesCurricularesEol.FirstOrDefault(c => c.CodigoJurema == objetivo.IdComponenteCurricular);
                            if (componenteCurricularEol != null)
                            {
                                objetivo.ComponenteCurricularEolId = componenteCurricularEol.CodigoEOL;
                            }
                            planoAnual.ObjetivosAprendizagem.Add(objetivo);
                        }
                    }
                }
                if (listaPlanoAnual.Count() != periodos.Count())
                {
                    var periodosFaltantes = periodos.Where(c => !listaPlanoAnual.Any(p => p.Bimestre == c.Bimestre));
                    var planosFaltantes = ObterNovoPlanoAnualCompleto(turmaId, anoLetivo, ueId, periodosFaltantes, dataAtual).ToList();
                    planosFaltantes.AddRange(listaPlanoAnual);
                    listaPlanoAnual = planosFaltantes;
                }
            }
            else
                listaPlanoAnual = ObterNovoPlanoAnualCompleto(turmaId, anoLetivo, ueId, periodos, dataAtual);
            return listaPlanoAnual.OrderBy(c => c.Bimestre);
        }

        public async Task<IEnumerable<TurmaParaCopiaPlanoAnualDto>> ObterTurmasParaCopia(int turmaId, long componenteCurricular)
        {
            var usuarioLogado = await servicoUsuario.ObterUsuarioLogado();
            var turmasEOL = await servicoEOL.ObterTurmasParaCopiaPlanoAnual(usuarioLogado.CodigoRf, componenteCurricular, turmaId);
            if (turmasEOL != null && turmasEOL.Any())
            {
                var idsTurmas = turmasEOL.Select(c => c.TurmaId.ToString());
                turmasEOL = repositorioPlanoAnual.ValidaSeTurmasPossuemPlanoAnual(idsTurmas.ToArray());
            }
            return turmasEOL;
        }

        public bool ValidarPlanoAnualExistente(FiltroPlanoAnualDto filtroPlanoAnualDto)
        {
            return repositorioPlanoAnual.ValidarPlanoExistentePorAnoEscolaTurmaEBimestre(filtroPlanoAnualDto.AnoLetivo, filtroPlanoAnualDto.EscolaId, filtroPlanoAnualDto.TurmaId, filtroPlanoAnualDto.Bimestre, filtroPlanoAnualDto.ComponenteCurricularEolId);
        }

        private static PlanoAnualCompletoDto ObterPlanoAnualPorBimestre(string turmaId, int anoLetivo, string ueId, int bimestre, bool obrigatorio)
        {
            return new PlanoAnualCompletoDto
            {
                Bimestre = bimestre,
                Migrado = false,
                EscolaId = ueId,
                TurmaId = turmaId,
                AnoLetivo = anoLetivo,
                Obrigatorio = obrigatorio
            };
        }

        private ModalidadeTipoCalendario ModalidadeParaModalidadeTipoCalendario(Modalidade modalidade)
        {
            switch (modalidade)
            {
                case Modalidade.EJA:
                    return ModalidadeTipoCalendario.EJA;

                default:
                    return ModalidadeTipoCalendario.FundamentalMedio;
            }
        }

        private FiltroPlanoAnualDto ObtenhaFiltro(int anoLetivo, long componenteCurricularEolId, string escolaId, string turmaId, int bimestre)
        {
            return new FiltroPlanoAnualDto
            {
                AnoLetivo = anoLetivo,
                ComponenteCurricularEolId = componenteCurricularEolId,
                EscolaId = escolaId,
                TurmaId = turmaId,
                Bimestre = bimestre
            };
        }

        private PlanoAnualCompletoDto ObterNovoPlanoAnual(string turmaId, int anoLetivo, string ueId)
        {
            var periodos = ObterPeriodoEscolar(turmaId, anoLetivo);

            var periodo = periodos.FirstOrDefault(c => c.PeriodoFim >= DateTime.Now.Date && c.PeriodoInicio <= DateTime.Now.Date);

            return new PlanoAnualCompletoDto
            {
                Bimestre = periodo.Bimestre,
                Migrado = false,
                EscolaId = ueId,
                TurmaId = turmaId,
            };
        }

        private IEnumerable<PlanoAnualCompletoDto> ObterNovoPlanoAnualCompleto(string turmaId, int anoLetivo, string ueId, IEnumerable<PeriodoEscolar> periodos, DateTime dataAtual)
        {
            var listaPlanoAnual = new List<PlanoAnualCompletoDto>();
            foreach (var periodo in periodos)
            {
                var obrigatorio = false;
                if (periodo.PeriodoFim.Local() >= dataAtual && periodo.PeriodoInicio.Local() <= dataAtual)
                {
                    obrigatorio = true;
                }
                listaPlanoAnual.Add(ObterPlanoAnualPorBimestre(turmaId, anoLetivo, ueId, periodo.Bimestre, obrigatorio));
            }
            return listaPlanoAnual;
        }

        private IEnumerable<PeriodoEscolar> ObterPeriodoEscolar(string turmaId, int anoLetivo)
        {
            var turma = repositorioTurma.ObterPorId(turmaId);
            if (turma == null)
            {
                throw new NegocioException("Turma não encontrada.");
            }
            var modalidade = turma.ModalidadeCodigo == Modalidade.EJA ? ModalidadeTipoCalendario.EJA : ModalidadeTipoCalendario.FundamentalMedio;
            var tipoCalendario = repositorioTipoCalendario.BuscarPorAnoLetivoEModalidade(anoLetivo, modalidade);
            if (tipoCalendario == null)
            {
                throw new NegocioException("Tipo de calendário não encontrado.");
            }

            var periodos = repositorioPeriodoEscolar.ObterPorTipoCalendario(tipoCalendario.Id);
            if (periodos == null)
            {
                throw new NegocioException("Período escolar não encontrado.");
            }
            return periodos;
        }

        private IEnumerable<PeriodoEscolar> ObterPeriodoEscolar(int anoLetivo, Modalidade modalidade)
        {
            var modalidadeTipoCalendario = ModalidadeParaModalidadeTipoCalendario(modalidade);

            var tipoCalendario = repositorioTipoCalendario.BuscarPorAnoLetivoEModalidade(anoLetivo, modalidadeTipoCalendario);

            if (tipoCalendario == null)
                return null;

            var periodoEscolar = repositorioPeriodoEscolar.ObterPorTipoCalendario(tipoCalendario.Id);

            return periodoEscolar;
        }

        private bool VerificaSeBimestreEhExpandido(IEnumerable<PeriodoEscolar> periodosEscolares, int bimestre)
        {
            var periodo = periodosEscolares.FirstOrDefault(p => p.Bimestre == bimestre);

            if (periodo == null)
                return false;

            var dataAtual = DateTime.Now;

            return periodo.PeriodoInicio <= dataAtual && periodo.PeriodoFim >= dataAtual;
        }
    }
}