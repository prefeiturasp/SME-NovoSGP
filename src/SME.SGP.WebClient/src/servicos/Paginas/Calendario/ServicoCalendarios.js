import moment from 'moment';
import modalidadeDto from '~/dtos/modalidade';
import modalidadeTipoCalendario from '~/dtos/modalidadeTipoCalendario';
import api from '~/servicos/api';
import { store } from '~/redux';
import {
  setExibirLoaderFrequenciaPlanoAula,
  setListaDadosFrequencia,
} from '~/redux/modulos/frequenciaPlanoAula/actions';
import { erros } from '~/servicos/alertas';

class ServicoCalendarios {
  obterTiposCalendario = async anoLetivo => {
    if (!anoLetivo) anoLetivo = moment().year();

    return api
      .get(`v1/calendarios/tipos/anos/letivos/${anoLetivo}`)
      .then(resposta => resposta)
      .catch(() => []);
  };

  converterModalidade = modalidadeCalendario => {
    let modalidade = modalidadeDto.FUNDAMENTAL;
    if (modalidadeCalendario === modalidadeTipoCalendario.EJA) {
      modalidade = modalidadeDto.EJA;
    } else if (modalidadeCalendario === modalidadeTipoCalendario.Infantil) {
      modalidade = modalidadeDto.INFANTIL;
    }
    return modalidade;
  };

  gerarRelatorio = payload => {
    return api.post('v1/relatorios/calendarios/impressao', payload);
  };

  obterTiposCalendarioAutoComplete = (descricao = '') => {
    return api.get(`v1/calendarios/tipos/anos-letivos?descricao=${descricao}`);
  };

  obterDatasDeAulasDisponiveis = (
    anoLetivo,
    turma,
    codigoComponenteCurricular
  ) => {
    const url = `v1/calendarios/frequencias/aulas/datas/${anoLetivo}/turmas/${turma}/disciplinas/${codigoComponenteCurricular}`;
    return api.get(url);
  };

  obterListaFrequencia = async () => {
    const { dispatch } = store;
    const state = store.getState();

    const { frequenciaPlanoAula } = state;

    const { aulaId } = frequenciaPlanoAula;

    dispatch(setExibirLoaderFrequenciaPlanoAula(true));
    const frequenciaAlunos = await api
      .get(`v1/calendarios/frequencias`, { params: { aulaId } })
      .finally(() => dispatch(setExibirLoaderFrequenciaPlanoAula(false)))
      .catch(e => erros(e));

    if (frequenciaAlunos && frequenciaAlunos.data) {
      dispatch(setListaDadosFrequencia(frequenciaAlunos.data));
    } else {
      dispatch(setListaDadosFrequencia({}));
    }
  };

  salvarFrequencia = params => {
    return api.post(`v1/calendarios/frequencias`, params);
  };
}

export default new ServicoCalendarios();
