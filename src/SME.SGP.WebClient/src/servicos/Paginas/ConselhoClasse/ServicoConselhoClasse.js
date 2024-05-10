import { store } from '~/redux';
import {
  setListaTiposConceitos,
  setMarcadorParecerConclusivo,
} from '~/redux/modulos/conselhoClasse/actions';
import { erros } from '~/servicos/alertas';
import api from '~/servicos/api';

class ServicoConselhoClasse {
  obterListaAlunos = (turmaCodigo, anoLetivo, periodo) => {
    const url = `v1/fechamentos/turmas/${turmaCodigo}/alunos/anos/${anoLetivo}/semestres/${periodo}`;
    return api.get(url);
  };

  obterFrequenciaAluno = (alunoCodigo, turmaCodigo) => {
    const url = `v1/calendarios/frequencias/alunos/${alunoCodigo}/turmas/${turmaCodigo}/geral`;
    return api.get(url);
  };

  obterAnotacoesRecomendacoes = (
    conselhoClasseId,
    fechamentoTurmaId,
    alunoCodigo,
    codigoTurma,
    bimestre
  ) => {
    const url = `v1/conselhos-classe/${conselhoClasseId ||
      0}/fechamentos/${fechamentoTurmaId ||
      0}/alunos/${alunoCodigo}/turmas/${codigoTurma}/bimestres/${
      bimestre !== 'final' ? bimestre : 0
    }/recomendacoes`;
    return api.get(url);
  };

  obterBimestreAtual = modalidade => {
    return api.get(
      `v1/periodo-escolar/modalidades/${modalidade}/bimestres/atual`
    );
  };

  salvarRecomendacoesAlunoFamilia = params => {
    return api.post('v1/conselhos-classe/recomendacoes', params);
  };

  obterSintese = (
    conselhoClasseId,
    fechamentoTurmaId,
    alunoCodigo,
    turmaId,
    bimestre
  ) => {
    return api.get(
      `v1/conselhos-classe/${conselhoClasseId ||
        0}/fechamentos/${fechamentoTurmaId ||
        0}/alunos/${alunoCodigo}/turmas/${turmaId}/bimestres/${
        bimestre === 'final' ? 0 : bimestre
      }/sintese`
    );
  };

  obterInformacoesPrincipais = (
    turmaCodigo,
    bimestre,
    alunoCodigo,
    ehFinal,
    consideraHistorico
  ) => {
    const url = `v1/conselhos-classe/turmas/${turmaCodigo}/bimestres/${
      bimestre === 'final' ? 0 : bimestre
    }/alunos/${alunoCodigo}/final/${ehFinal}/consideraHistorico/${consideraHistorico}`;
    return api.get(url);
  };

  carregarListaTiposConceito = async periodoFim => {
    const { dispatch } = store;

    if (periodoFim) {
      const lista = await api
        .get(`v1/avaliacoes/notas/conceitos?data=${periodoFim}`)
        .catch(e => erros(e));

      if (lista && lista.data && lista.data.length) {
        const novaLista = lista.data.map(item => {
          item.id = String(item.id);
          return item;
        });
        dispatch(setListaTiposConceitos(novaLista));
      } else {
        dispatch(setListaTiposConceitos([]));
      }
    } else {
      dispatch(setListaTiposConceitos([]));
    }
  };

  acessarAbaFinalParecerConclusivo = (
    conselhoClasseId,
    fechamentoTurmaId,
    alunoCodigo,
    codigoTurma
  ) => {
    const url = `v1/conselhos-classe/${conselhoClasseId ||
      0}/fechamentos/${fechamentoTurmaId ||
      0}/alunos/${alunoCodigo}/turmas/${codigoTurma}/parecer`;
    return api.get(url);
  };

  obterNotasConceitosConselhoClasse = (
    conselhoClasseId,
    fechamentoTurmaId,
    alunoCodigo,
    turmaId,
    bimestre
  ) => {
    const url = `v1/conselhos-classe/${conselhoClasseId ||
      0}/fechamentos/${fechamentoTurmaId ||
      0}/alunos/${alunoCodigo}/turmas/${turmaId}/bimestres/${
      bimestre === 'final' ? 0 : bimestre
    }/notas`;
    return api.get(url);
  };

  salvarNotaPosConselho = (
    conselhoClasseId,
    fechamentoTurmaId,
    alunoCodigo,
    notaDto,
    codigoTurma,
    bimestre
  ) => {
    const url = `v1/conselhos-classe/${conselhoClasseId ||
      0}/notas/alunos/${alunoCodigo}/turmas/${codigoTurma}/bimestres/${
      bimestre !== 'final' ? bimestre : 0
    }/fechamento-turma/${fechamentoTurmaId || 0}`;
    return api.post(url, notaDto);
  };

  obterNotaPosConselho = id => {
    return api.get(`v1/conselhos-classe/detalhamento/${id}`);
  };

  gerarParecerConclusivo = (
    conselhoClasseId,
    fechamentoTurmaId,
    alunoCodigo
  ) => {
    const url = `v1/conselhos-classe/${conselhoClasseId ||
      0}/fechamentos/${fechamentoTurmaId || 0}/alunos/${alunoCodigo}/parecer`;
    return api.post(url);
  };

  setarParecerConclusivo = parecer => {
    const { dispatch } = store;
    let parecerAtual = '';
    if (parecer) {
      const { nome, id } = parecer;
      if (nome && id) {
        parecerAtual = parecer;
      }
    }
    dispatch(setMarcadorParecerConclusivo(parecerAtual));
  };

  gerarConselhoClasseTurma = (conselhoClasseId, fechamentoTurmaId) => {
    return api.get(
      `v1/conselhos-classe/${conselhoClasseId}/fechamentos/${fechamentoTurmaId}/imprimir`
    );
  };

  gerarConselhoClasseAluno = (
    conselhoClasseId,
    fechamentoTurmaId,
    alunoCodigo
  ) => {
    return api.get(
      `/v1/conselhos-classe/${conselhoClasseId}/fechamentos/${fechamentoTurmaId}/alunos/${alunoCodigo}/imprimir`
    );
  };

  obterDadosBimestres = turmaId => {
    return api.get(`/v1/conselhos-classe/turmas/${turmaId}/bimestres`);
  };
}

export default new ServicoConselhoClasse();
