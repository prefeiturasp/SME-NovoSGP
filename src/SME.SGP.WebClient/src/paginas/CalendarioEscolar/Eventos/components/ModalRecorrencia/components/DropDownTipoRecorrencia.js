import React from 'react';
import PropTypes from 'prop-types';

// Components
import { Dropdown, Icon, Menu } from 'antd';

// Styles
import { DefaultDropDownLink } from '../styles';

function DropDownTipoRecorrencia({ onChange, value }) {
  const options = (
    <Menu>
      <Menu.Item
        onClick={() => onChange({ label: 'Semana(s)', value: 'S' })}
        key="0"
      >
        Semana(s)
      </Menu.Item>
      <Menu.Item
        onClick={() => onChange({ label: 'Mês(ses)', value: 'M' })}
        key="1"
      >
        Mês(ses)
      </Menu.Item>
    </Menu>
  );

  return (
    <Dropdown trigger={['click']} overlay={options}>
      <DefaultDropDownLink href="#">
        {value.label ? value.label : 'Selecione'} <Icon type="down" />
      </DefaultDropDownLink>
    </Dropdown>
  );
}

DropDownTipoRecorrencia.defaultProps = {
  value: {},
  onChange: () => {},
};

DropDownTipoRecorrencia.propTypes = {
  value: PropTypes.oneOfType([PropTypes.array, PropTypes.object]),
  onChange: PropTypes.func,
};

export default DropDownTipoRecorrencia;
