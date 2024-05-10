import PropTypes from 'prop-types';
import React from 'react';
import { useSelector } from 'react-redux';
import Button from '~/componentes/button';
import { Colors } from '~/componentes/colors';

const BotoesAcoesDiarioBordo = props => {
  const {
    onClickVoltar,
    onClickCancelar,
    modoEdicao,
    desabilitarCampos,
    turmaInfantil,
    validaAntesDoSubmit,
  } = props;

  const observacaoEmEdicao = useSelector(
    store => store.observacoesUsuario.observacaoEmEdicao
  );

  const novaObservacao = useSelector(
    store => store.observacoesUsuario.novaObservacao
  );

  return (
    <>
      <Button
        id="btn-voltar-ata-diario-bordo"
        label="Voltar"
        icon="arrow-left"
        color={Colors.Azul}
        border
        className="mr-3"
        onClick={() => onClickVoltar(observacaoEmEdicao, novaObservacao)}
      />
      <Button
        id="btn-cancelar-ata-diario-bordo"
        label="Cancelar"
        color={Colors.Roxo}
        border
        bold
        className="mr-3"
        onClick={onClickCancelar}
        disabled={!modoEdicao || desabilitarCampos}
      />
      <Button
        id="btn-gerar-ata-diario-bordo"
        label="Salvar"
        color={Colors.Roxo}
        border
        bold
        onClick={validaAntesDoSubmit}
        disabled={!modoEdicao || !turmaInfantil || desabilitarCampos}
      />
    </>
  );
};

BotoesAcoesDiarioBordo.propTypes = {
  onClickVoltar: PropTypes.func,
  onClickCancelar: PropTypes.func,
  validaAntesDoSubmit: PropTypes.func,
  modoEdicao: PropTypes.bool,
  desabilitarCampos: PropTypes.bool,
  turmaInfantil: PropTypes.bool,
};

BotoesAcoesDiarioBordo.defaultProps = {
  onClickVoltar: () => {},
  onClickCancelar: () => {},
  validaAntesDoSubmit: () => {},
  modoEdicao: false,
  desabilitarCampos: false,
  turmaInfantil: false,
};

export default BotoesAcoesDiarioBordo;
