import PropTypes from 'prop-types';
import React, { useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import Button from '~/componentes/button';
import { Colors } from '~/componentes/colors';
import { setModoEdicaoGeral } from '~/redux/modulos/notasConceitos/actions';

const BotoesAcoessNotasConceitos = props => {
  const dispatch = useDispatch();
  const modoEdicaoGeral = useSelector(
    store => store.notasConceitos.modoEdicaoGeral
  );

  const {
    onClickVoltar,
    onClickCancelar,
    onClickSalvar,
    desabilitarCampos,
  } = props;

  const onCancelar = async () => {
    if (!desabilitarCampos && modoEdicaoGeral) {
      if (
        window.confirm(
          `Você não salvou as informações preenchidas. Deseja realmente cancelar as alterações?`
        )
      ) {
        onClickCancelar(true);
        dispatch(setModoEdicaoGeral(false));
      } else {
        onClickCancelar(false);
      }
      // const confirmou = await confirmar(
      //   'Atenção',
      //   'Você não salvou as informações preenchidas.',
      //   'Deseja realmente cancelar as alterações?'
      // );
      // if (confirmou) {
      //   setModoEdicao(false);
      //   obterDadosBimestres(disciplinaSelecionada, 0);
      // }
    }
  };

  return (
    <>
      <Button
        label="Voltar"
        icon="arrow-left"
        color={Colors.Azul}
        border
        className="mr-2"
        onClick={onClickVoltar}
      />
      <Button
        label="Cancelar"
        color={Colors.Roxo}
        border
        className="mr-2"
        onClick={onCancelar}
        disabled={!modoEdicaoGeral}
      />
      <Button
        label="Salvar"
        color={Colors.Roxo}
        border
        bold
        className="mr-2"
        onClick={onClickSalvar}
        disabled={desabilitarCampos || !modoEdicaoGeral}
      />
    </>
  );
};

BotoesAcoessNotasConceitos.defaultProps = {
  onClickVoltar: PropTypes.func,
  onClickCancelar: PropTypes.func,
  onClickSalvar: PropTypes.func,
  desabilitarCampos: PropTypes.bool,
};

BotoesAcoessNotasConceitos.propTypes = {
  onClickVoltar: () => {},
  onClickCancelar: () => {},
  onClickSalvar: () => {},
  desabilitarCampos: false,
};

export default BotoesAcoessNotasConceitos;
