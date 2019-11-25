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
        className="mr-2"
        onClick={onClickVoltar}
      />
      {modoEdicao && (
        <Button
          label="Cancelar"
          color={Colors.Roxo}
          border
          className="mr-2"
          onClick={() => onClickCancelar(form)}
          disabled={!modoEdicao || novoRegistro === false}
        />
      )}
      <Button
        label="Excluir"
        color={Colors.Vermelho}
        border
        className="mr-2"
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
        className="mr-0"
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
  somenteConsulta: PropTypes.bool,
  permissoesTela: PropTypes.objectOf(PropTypes.object),
  temItemSelecionado: PropTypes.oneOfType([PropTypes.bool, PropTypes.any]),
  onClickVoltar: PropTypes.func,
  onClickExcluir: PropTypes.func,
  onClickBotaoPrincipal: PropTypes.func,
};

ButtonGroup.defaultProps = {
  somenteConsulta: false,
  permissoesTela: {},
  temItemSelecionado: null,
  onClickVoltar: () => null,
  onClickExcluir: () => null,
  onClickBotaoPrincipal: () => null,
};

export default ButtonGroup;
