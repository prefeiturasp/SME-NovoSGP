import PropTypes from 'prop-types';
import React from 'react';
import { useSelector } from 'react-redux';
import Button from '~/componentes/button';
import { Colors } from '~/componentes/colors';
import { confirmar } from '~/servicos/alertas';

const BotoesAcoessNotasConceitos = props => {
  const {
    onClickVoltar,
    onClickCancelar,
    onClickSalvar,
    desabilitarBotao,
  } = props;

  const modoEdicaoGeral = useSelector(
    store => store.notasConceitos.modoEdicaoGeral
  );

  const modoEdicaoGeralNotaFinal = useSelector(
    store => store.notasConceitos.modoEdicaoGeralNotaFinal
  );

  const onCancelar = async () => {
    if (modoEdicaoGeral || modoEdicaoGeralNotaFinal) {
      const confirmou = await confirmar(
        'Atenção',
        'Você não salvou as informações preenchidas.',
        'Deseja realmente cancelar as alterações?'
      );
      if (confirmou) {
        return onClickCancelar(true);
      }
    }
    return onClickCancelar(false);
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
        disabled={!modoEdicaoGeral && !modoEdicaoGeralNotaFinal}
      />
      <Button
        label="Salvar"
        color={Colors.Roxo}
        border
        bold
        className="mr-2"
        onClick={onClickSalvar}
        disabled={
          desabilitarBotao || (!modoEdicaoGeral && !modoEdicaoGeralNotaFinal)
        }
      />
    </>
  );
};

BotoesAcoessNotasConceitos.defaultProps = {
  onClickVoltar: PropTypes.func,
  onClickCancelar: PropTypes.func,
  onClickSalvar: PropTypes.func,
  desabilitarBotao: PropTypes.bold,
};

BotoesAcoessNotasConceitos.propTypes = {
  onClickVoltar: () => {},
  onClickCancelar: () => {},
  onClickSalvar: () => {},
  desabilitarBotao: false,
};

export default BotoesAcoessNotasConceitos;
