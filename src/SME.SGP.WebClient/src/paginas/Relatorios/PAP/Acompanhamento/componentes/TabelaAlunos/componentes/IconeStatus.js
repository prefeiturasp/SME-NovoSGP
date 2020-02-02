import React from 'react';
import t from 'prop-types';
import styled from 'styled-components';

// Ant
import { Tooltip } from 'antd';

// Componentes
import { Base } from '~/componentes';

const Status = styled.div`
  background: ${Base.CinzaBarras};
  display: inline;
  padding: 1px 4px;
  border-radius: 50%;

  &.alerta {
    background-color: ${Base.LaranjaAlerta};
  }

  &.concluido {
    background-color: ${Base.Verde};
  }

  &::before {
    content: '\f00c';
    font-family: 'Font Awesome 5 Free';
    font-weight: 900;
    font-size: 10px;
    color: white;
  }
`;

function IconeStatus({ status }) {
  const listaStatus = [
    {
      valor: 0,
      tooltip: 'Não alterado',
      classe: '',
    },
    {
      valor: 1,
      tooltip: 'Parcialmente preenchido',
      classe: 'alerta',
    },
    {
      valor: 2,
      tooltip: 'Preenchimento concluído',
      classe: 'concluido',
    },
  ];

  const gerarClasseStatus = valor =>
    listaStatus.filter(x => x.valor === valor)[0].classe;

  const gerarDescricaoTooltip = valor =>
    listaStatus.filter(x => x.valor === valor)[0].tooltip;

  return (
    <Tooltip title={gerarDescricaoTooltip(status)}>
      <Status className={gerarClasseStatus(status)} />
    </Tooltip>
  );
}

IconeStatus.propTypes = {
  status: t.number,
};

IconeStatus.defaultProps = {
  status: 0,
};

export default IconeStatus;
