import React from 'react';
import PropTypes from 'prop-types';

import { Button, Colors } from '~/componentes';

const BotoesAcoes = ({ turmaInfantil, modoEdicao, desabilitarCampos }) => {
  const onClickVoltar = () => {};
  const onClickCancelar = () => {};
  const onClickNovo = () => {};

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
        onClick={onClickCancelar}
        disabled={!modoEdicao || !turmaInfantil || !desabilitarCampos}
      />
      <Button
        label="Novo"
        color={Colors.Roxo}
        bold
        className="mr-2"
        onClick={onClickNovo}
        disabled={!modoEdicao || !turmaInfantil || !desabilitarCampos}
      />
    </>
  );
};

BotoesAcoes.propTypes = {
  turmaInfantil: PropTypes.bool,
  modoEdicao: PropTypes.bool,
  desabilitarCampos: PropTypes.bool,
};

BotoesAcoes.defaultProps = {
  turmaInfantil: false,
  modoEdicao: false,
  desabilitarCampos: false,
};
export default BotoesAcoes;
