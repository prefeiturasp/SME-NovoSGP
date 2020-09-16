import PropTypes from 'prop-types';
import React from 'react';
import Button from '~/componentes/button';
import { Colors } from '~/componentes/colors';
import { URL_HOME } from '~/constantes/url';
import history from '~/servicos/history';

const BotoesAcoesPlanoAnual = props => {
  const { onClickCancelar, onClickSalvar, ehTurmaInfantil } = props;

  const onSalvar = async () => {
    onClickSalvar();
  };

  const onClickVoltar = async () => {
    history.push(URL_HOME);
  };

  const onCancelar = async () => {
    onClickCancelar();
  };

  return (
    <>
      <Button
        id="btn-voltar-plano-anual"
        label="Voltar"
        icon="arrow-left"
        color={Colors.Azul}
        border
        className="mr-3"
        onClick={onClickVoltar}
      />
      <Button
        id="btn-cancelar-plano-anual"
        label="Cancelar"
        color={Colors.Roxo}
        border
        className="mr-3"
        onClick={onCancelar}
        disabled={ehTurmaInfantil}
      />
      <Button
        id="btn-salvar-plano-anual"
        label="Salvar"
        color={Colors.Roxo}
        border
        bold
        onClick={onSalvar}
        disabled={ehTurmaInfantil}
      />
    </>
  );
};

BotoesAcoesPlanoAnual.propTypes = {
  onClickCancelar: PropTypes.func,
  onClickSalvar: PropTypes.bool,
  ehTurmaInfantil: PropTypes.func,
};

BotoesAcoesPlanoAnual.defaultProps = {
  onClickCancelar: () => {},
  onClickSalvar: () => {},
  ehTurmaInfantil: false,
};

export default BotoesAcoesPlanoAnual;
