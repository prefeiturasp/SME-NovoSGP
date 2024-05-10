import React from 'react';
import { useDispatch, useSelector } from 'react-redux';
import Button from '~/componentes/button';
import { Colors } from '~/componentes/colors';
import { URL_HOME } from '~/constantes';
import {
  setExibirCardCollapseFrequencia,
  setLimparDadosPlanoAula,
  setModoEdicaoFrequencia,
  setModoEdicaoPlanoAula,
} from '~/redux/modulos/frequenciaPlanoAula/actions';
import { confirmar, history } from '~/servicos';
import ServicoFrequencia from '~/servicos/Paginas/DiarioClasse/ServicoFrequencia';
import servicoSalvarFrequenciaPlanoAula from '../../servicoSalvarFrequenciaPlanoAula';

const BotoesAcoesFrequenciaPlanoAula = () => {
  const dispatch = useDispatch();

  const modoEdicaoFrequencia = useSelector(
    state => state.frequenciaPlanoAula.modoEdicaoFrequencia
  );

  const modoEdicaoPlanoAula = useSelector(
    state => state.frequenciaPlanoAula.modoEdicaoPlanoAula
  );

  const somenteConsulta = useSelector(
    state => state.frequenciaPlanoAula.somenteConsulta
  );

  const onClickSalvar = async () => {
    servicoSalvarFrequenciaPlanoAula.validarSalvarFrequenciPlanoAula();
  };

  const pergutarParaSalvar = () => {
    return confirmar(
      'Atenção',
      '',
      'Suas alterações não foram salvas, deseja salvar agora?'
    );
  };

  const irParaHome = () => {
    history.push(URL_HOME);
  };

  const onClickVoltar = async () => {
    if (modoEdicaoFrequencia || modoEdicaoPlanoAula) {
      const confirmado = await pergutarParaSalvar();
      if (confirmado) {
        const salvou = await servicoSalvarFrequenciaPlanoAula.validarSalvarFrequenciPlanoAula();
        if (salvou) {
          irParaHome();
        }
      } else {
        irParaHome();
      }
    } else {
      irParaHome();
    }
  };

  const onClickCancelar = async () => {
    if (modoEdicaoFrequencia || modoEdicaoPlanoAula) {
      const confirmou = await confirmar(
        'Atenção',
        'Você não salvou as informações preenchidas.',
        'Deseja realmente cancelar as alterações?'
      );
      if (confirmou) {
        ServicoFrequencia.obterListaFrequencia();
        dispatch(setExibirCardCollapseFrequencia(false));
        dispatch(setModoEdicaoFrequencia(false));
        dispatch(setLimparDadosPlanoAula());
        dispatch(setModoEdicaoPlanoAula(false));
      }
    }
  };

  return (
    <>
      <Button
        id="btn-voltar"
        label="Voltar"
        icon="arrow-left"
        color={Colors.Azul}
        border
        className="mr-3"
        onClick={onClickVoltar}
      />
      <Button
        id="btn-cancelar"
        label="Cancelar"
        color={Colors.Roxo}
        border
        className="mr-3"
        onClick={onClickCancelar}
        disabled={
          somenteConsulta || (!modoEdicaoFrequencia && !modoEdicaoPlanoAula)
        }
      />
      <Button
        id="btn-salvar"
        label="Salvar"
        color={Colors.Roxo}
        border
        bold
        onClick={onClickSalvar}
        disabled={
          somenteConsulta || (!modoEdicaoFrequencia && !modoEdicaoPlanoAula)
        }
      />
    </>
  );
};

export default BotoesAcoesFrequenciaPlanoAula;
