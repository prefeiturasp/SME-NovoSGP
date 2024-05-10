import React, { useState } from 'react';
import PropTypes from 'prop-types';
import { Input, Button } from 'antd';
import { InputEstilo } from './estilo';
import { Label } from '~/componentes';

const InputBusca = ({ label, placeholder, onClick, onPressEnter, valor, onChange, disabled }) => {

  return (
    <InputEstilo>
      <Label text={label} />
      <Input
        value={valor}
        placeholder={placeholder}
        onChange={onChange}
        onPressEnter={onPressEnter}
        prefix={
          <Button onClick={onClick} type="link">
            <i className="fa fa-search fa-lg" />
          </Button>
        }
        disabled={disabled}
        allowClear
      />
    </InputEstilo>
  );
};

InputBusca.propTypes = {
  label: PropTypes.string,
  placeholder: PropTypes.string,
  onClick: PropTypes.func,
  onPressEnter: PropTypes.func,
  onChange: PropTypes.func,
  disabled: PropTypes.bool,
  valor: PropTypes.string,
};

InputBusca.defaultProps = {
  label: '',
  placeholder: '',
  onClick: null,
  onPressEnter: null,
  onChange: null,
  disabled: false,
  valor: '',
};

export default InputBusca;
