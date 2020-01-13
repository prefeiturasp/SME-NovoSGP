import React from 'react';
import PropTypes from 'prop-types';

// Componentes
import { Button, Colors } from '~/componentes';

// Styles
import { ButtonGroupEstilo } from './styles';

function ButtonGroup({
  somenteConsulta,
  permissoesTela,
  temItemSelecionado,
  onClickVoltar,
  onClickExcluir,
  onClickNovo,
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
      <Button
        label="Excluir"
        color={Colors.Vermelho}
        border
        className="mr-2"
        disabled={!permissoesTela.podeExcluir && temItemSelecionado}
        onClick={onClickExcluir}
      />
      <Button
        label="Novo"
        color={Colors.Roxo}
        border
        bold
        className="mr-0"
        onClick={onClickNovo}
        disabled={somenteConsulta || !permissoesTela.podeIncluir}
      />
    </ButtonGroupEstilo>
  );
}

ButtonGroup.propTypes = {
  somenteConsulta: PropTypes.bool,
  permissoesTela: PropTypes.objectOf(PropTypes.object),
  temItemSelecionado: PropTypes.bool,
  onClickVoltar: PropTypes.func,
  onClickExcluir: PropTypes.func,
  onClickNovo: PropTypes.func,
};

ButtonGroup.defaultProps = {
  somenteConsulta: false,
  permissoesTela: {},
  temItemSelecionado: false,
  onClickVoltar: () => null,
  onClickExcluir: () => null,
  onClickNovo: () => null,
};

export default ButtonGroup;
