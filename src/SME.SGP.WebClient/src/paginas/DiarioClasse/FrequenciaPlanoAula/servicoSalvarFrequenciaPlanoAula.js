import { store } from '~/redux';
import {
  setDataSelecionadaFrequenciaPlanoAula,
  setExibirLoaderFrequenciaPlanoAula,
  setModoEdicaoFrequencia,
  setModoEdicaoPlanoAula,
} from '~/redux/modulos/frequenciaPlanoAula/actions';
import { erros, sucesso } from '~/servicos/alertas';
import ServicoFrequencia from '~/servicos/Paginas/DiarioClasse/ServicoFrequencia';
import ServicoPlanoAula from '~/servicos/Paginas/DiarioClasse/ServicoPlanoAula';

class ServicoSalvarFrequenciaPlanoAula {
  salvarFrequencia = async () => {
    const { dispatch } = store;
    const state = store.getState();

    const { frequenciaPlanoAula } = state;
    const { listaDadosFrequencia, aulaId } = frequenciaPlanoAula;

    const valorParaSalvar = {
      aulaId,
      listaFrequencia: listaDadosFrequencia.listaFrequencia,
    };

    dispatch(setExibirLoaderFrequenciaPlanoAula(true));

    const salvouFrequencia = await ServicoFrequencia.salvarFrequencia(
      valorParaSalvar
    )
      .finally(() => dispatch(setExibirLoaderFrequenciaPlanoAula(false)))
      .catch(e => erros(e));

    if (salvouFrequencia && salvouFrequencia.status === 200) {
      sucesso('Frequência realizada com sucesso.');
      dispatch(setModoEdicaoFrequencia(false));
      return true;
    }

    return false;
  };

  salvarPlanoAula = async () => {
    const { dispatch } = store;
    const state = store.getState();

    const { frequenciaPlanoAula } = state;
    const { dadosParaSalvarPlanoAula, aulaId } = frequenciaPlanoAula;

    let objetivosId = [];

    if (
      dadosParaSalvarPlanoAula &&
      dadosParaSalvarPlanoAula.objetivosAprendizagemAula.length
    ) {
      objetivosId = dadosParaSalvarPlanoAula.objetivosAprendizagemAula.map(
        item => item.id
      );
    }

    const valorParaSalvar = {
      descricao: dadosParaSalvarPlanoAula.descricao,
      desenvolvimentoAula: dadosParaSalvarPlanoAula.desenvolvimentoAula,
      recuperacaoAula: dadosParaSalvarPlanoAula.recuperacaoAula,
      licaoCasa: dadosParaSalvarPlanoAula.licaoCasa,
      aulaId,
      objetivosAprendizagemJurema: objetivosId,
    };

    dispatch(setExibirLoaderFrequenciaPlanoAula(true));

    const salvouPlanoAula = await ServicoPlanoAula.salvarPlanoAula(
      valorParaSalvar
    )
      .finally(() => dispatch(setExibirLoaderFrequenciaPlanoAula(false)))
      .catch(e => erros(e));

    if (salvouPlanoAula && salvouPlanoAula.status === 200) {
      sucesso('Plano de aula salvo com sucesso.');
      dispatch(setModoEdicaoPlanoAula(false));
      return true;
    }

    return false;
  };

  validarSalvarFrequenciPlanoAula = async () => {
    const { dispatch } = store;
    const state = store.getState();

    const { frequenciaPlanoAula } = state;

    const {
      listaDadosFrequencia,
      modoEdicaoFrequencia,
      modoEdicaoPlanoAula,
    } = frequenciaPlanoAula;

    let salvouFrequencia = true;
    let salvouPlanoAula = true;

    const permiteRegistroFrequencia = !listaDadosFrequencia.desabilitado;
    if (modoEdicaoFrequencia && permiteRegistroFrequencia) {
      salvouFrequencia = await this.salvarFrequencia();
    }

    if (modoEdicaoPlanoAula) {
      salvouPlanoAula = await this.salvarPlanoAula();
    }

    const salvouComSucesso = salvouFrequencia && salvouPlanoAula;

    if (salvouComSucesso) {
      dispatch(setDataSelecionadaFrequenciaPlanoAula());
    }
    return salvouComSucesso;
  };
}

export default new ServicoSalvarFrequenciaPlanoAula();
