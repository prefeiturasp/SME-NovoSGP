import { store } from '~/redux';
import { setListaTiposConceitos } from '~/redux/modulos/conselhoClasse/actions';
import { erros } from '~/servicos/alertas';
import api from '~/servicos/api';

class ServicoConselhoClasse {
  obterListaAlunos = (turmaCodigo, anoLetivo, periodo) => {
    const url = `v1/fechamentos/turmas/${turmaCodigo}/alunos/anos/${anoLetivo}/semestres/${periodo}`;
    return api.get(url);
  };

  obterFrequenciaAluno = alunoCodigo => {
    const url = `v1/calendarios/frequencias/alunos/${alunoCodigo}/geral`;
    return api.get(url);
  };

  obterAnotacoesRecomendacoes = (
    conselhoClasseId,
    fechamentoTurmaId,
    alunoCodigo
  ) => {
    const url = `v1/conselhos-classe/${conselhoClasseId ||
      0}/fechamentos/${fechamentoTurmaId}/alunos/${alunoCodigo}/recomendacoes`;
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

  obterSintese = (conselhoClasseId, fechamentoTurmaId, alunoCodigo) => {
    return api.get(
      `v1/conselhos-classe/${conselhoClasseId ||
        0}/fechamentos/${fechamentoTurmaId}/alunos/${alunoCodigo}/sintese`
    );
  };

  obterInformacoesPrincipais = (
    turmaCodigo,
    bimestre,
    alunoCodigo,
    ehFinal
  ) => {
    const url = `v1/conselhos-classe/turmas/${turmaCodigo}/bimestres/${bimestre}/alunos/${alunoCodigo}/final/${ehFinal}`;
    return api.get(url);
  };

  carregarListaTiposConceito = async periodoFim => {
    const { dispatch } = store;

    const lista = await api
      .get(`v1/avaliacoes/notas/conceitos?data=${periodoFim}`)
      .catch(e => erros(e));

    if (lista && lista.data && lista.data.length) {
      const novaLista = lista.data.map(item => {
        item.id = String(item.id);
        return item;
      });
      dispatch(setListaTiposConceitos(novaLista));
    }
    dispatch(setListaTiposConceitos([]));
  };

  acessarAbaFinalParecerConclusivo = (
    conselhoClasseId,
    fechamentoTurmaId,
    alunoCodigo
  ) => {
    const url = `v1/conselhos-classe/${conselhoClasseId}/fechamentos/${fechamentoTurmaId}/alunos/${alunoCodigo}/parecer`;
    return api.get(url);
  };
}

export default new ServicoConselhoClasse();
