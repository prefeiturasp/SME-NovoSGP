import React from 'react';
import PropTypes from 'prop-types';

// Componentes
import { Button, Colors } from '~/componentes';

// Styles
import { ButtonGroupEstilo } from './styles';

function ButtonGroup({
  form,
  modoEdicao,
  novoRegistro,
  somenteConsulta,
  permissoesTela,
  temItemSelecionado,
  labelBotaoPrincipal,
  desabilitarBotaoPrincipal,
  onClickVoltar,
  onClickExcluir,
  onClickBotaoPrincipal,
  onClickCancelar,
}) {
  return (
    <ButtonGroupEstilo className="col-md-12 d-flex justify-content-end p-0">
      <Button
        label="Voltar"
        icon="arrow-left"
        color={Colors.Azul}
        border
        className="btnGroupItem"
        onClick={onClickVoltar}
      />
      {typeof onClickCancelar === 'function' && (
        <Button
          label="Cancelar"
          color={Colors.Roxo}
          border
          className="btnGroupItem"
          onClick={() => onClickCancelar(form)}
          disabled={!modoEdicao || !novoRegistro}
        />
      )}
      <Button
        label="Excluir"
        color={Colors.Roxo}
        border
        className="btnGroupItem"
        disabled={
          (!permissoesTela.podeExcluir && !temItemSelecionado) ||
          novoRegistro === true ||
          (typeof temItemSelecionado === 'boolean' && !temItemSelecionado)
        }
        onClick={onClickExcluir}
      />
      <Button
        label={labelBotaoPrincipal}
        color={Colors.Roxo}
        border
        bold
        className="btnGroupItem"
        onClick={onClickBotaoPrincipal}
        disabled={
          somenteConsulta ||
          !permissoesTela.podeIncluir ||
          desabilitarBotaoPrincipal
        }
      />
    </ButtonGroupEstilo>
  );
}

ButtonGroup.propTypes = {
  form: PropTypes.oneOfType([PropTypes.object, PropTypes.func, PropTypes.any]),
  modoEdicao: PropTypes.bool,
  novoRegistro: PropTypes.bool,
  somenteConsulta: PropTypes.bool,
  desabilitarBotaoPrincipal: PropTypes.bool,
  labelBotaoPrincipal: PropTypes.string,
  permissoesTela: PropTypes.oneOfType([PropTypes.object, PropTypes.any]),
  temItemSelecionado: PropTypes.oneOfType([PropTypes.bool, PropTypes.any]),
  onClickVoltar: PropTypes.func,
  onClickExcluir: PropTypes.func,
  onClickCancelar: PropTypes.func,
  onClickBotaoPrincipal: PropTypes.func,
};

ButtonGroup.defaultProps = {
  form: {},
  modoEdicao: false,
  novoRegistro: true,
  somenteConsulta: false,
  labelBotaoPrincipal: '',
  desabilitarBotaoPrincipal: false,
  permissoesTela: {},
  temItemSelecionado: false,
  onClickVoltar: () => null,
  onClickExcluir: () => null,
  onClickCancelar: null,
  onClickBotaoPrincipal: () => null,
};

export default ButtonGroup;
