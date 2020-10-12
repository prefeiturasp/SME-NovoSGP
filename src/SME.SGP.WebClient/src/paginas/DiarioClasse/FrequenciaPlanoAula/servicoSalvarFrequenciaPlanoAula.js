import { erros, sucesso } from '~/servicos/alertas';
import { store } from '~/redux';
import { salvarDadosAulaFrequencia } from '~/redux/modulos/calendarioProfessor/actions';
import { ServicoCalendarios } from '~/servicos';
import {
  setModoEdicaoFrequencia,
  setExibirCardFrequencia,
  setExibirLoaderFrequenciaPlanoAula,
  setDataSelecionadaFrequenciaPlanoAula,
} from '~/redux/modulos/frequenciaPlanoAula/actions';

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

    const salvouFrequencia = await ServicoCalendarios.salvarFrequencia(
      valorParaSalvar
    )
      .finally(() => dispatch(setExibirLoaderFrequenciaPlanoAula(false)))
      .catch(e => erros(e));

    if (salvouFrequencia && salvouFrequencia.status === 200) {
      sucesso('Frequência realizada com sucesso.');
      dispatch(setExibirCardFrequencia(false));
      dispatch(setModoEdicaoFrequencia(false));
      return true;
    }

    return false;
  };

  salvarPlanoAula = () => {
    sucesso('WOOOOWO');
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

    // Limpa os dados salvos quando entra na tela pelo calendário!
    dispatch(salvarDadosAulaFrequencia());

    const salvouComSucesso = salvouFrequencia && salvouPlanoAula;

    if (salvouComSucesso) {
      dispatch(setDataSelecionadaFrequenciaPlanoAula());
    }
    return salvouComSucesso;
  };
}

export default new ServicoSalvarFrequenciaPlanoAula();
