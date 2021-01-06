import React from 'react';
import PropTypes from 'prop-types';

import { Button, Colors } from '~/componentes';

const BotoesAcoes = ({
  desabilitarCampos,
  modoEdicao,
  onClickCancelar,
  onClickCadastrar,
  onClickVoltar,
  turmaInfantil,
}) => {
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
        label="Cadastrar"
        color={Colors.Roxo}
        bold
        className="mr-2"
        onClick={onClickCadastrar}
        disabled={!modoEdicao || !turmaInfantil || !desabilitarCampos}
      />
    </>
  );
};

BotoesAcoes.propTypes = {
  onClickVoltar: PropTypes.func,
  onClickCancelar: PropTypes.func,
  onClickCadastrar: PropTypes.func,
  modoEdicao: PropTypes.bool,
  desabilitarCampos: PropTypes.bool,
  turmaInfantil: PropTypes.bool,
};

BotoesAcoes.defaultProps = {
  onClickVoltar: () => {},
  onClickCancelar: () => {},
  onClickCadastrar: () => {},
  modoEdicao: false,
  desabilitarCampos: false,
  turmaInfantil: false,
};
export default BotoesAcoes;
