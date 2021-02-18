import { Radio } from 'antd';
import { Field } from 'formik';
import React from 'react';
import styled from 'styled-components';
import PropTypes from 'prop-types';
import { Base } from '~/componentes/colors';

import Label from './label';

const Campo = styled.div`
  .ant-radio-inner::after {
    background-color: ${Base.Roxo} !important;
  }
  .ant-radio-checked .ant-radio-inner {
    border-color: ${Base.Roxo} !important;
  }
  .ant-radio-wrapper:hover .ant-radio,
  .ant-radio:hover .ant-radio-inner,
  .ant-radio-input:focus + .ant-radio-inner {
    border-color: ${Base.Roxo} !important;
  }
  .ant-radio-group {
    white-space: nowrap;
    margin-bottom: 5px;
    border-radius: 0.15rem;
    width: ${props => (props.invalido ? 'fit-content' : 'auto')};
    padding-left: ${props => (props.invalido ? '2px' : '0px')};
    border: ${props =>
      props.invalido ? `1px solid  ${Base.Vermelho}` : 'none'};
  }

  label {
    font-weight: bold;
  }
`;

const Error = styled.span`
  color: ${Base.Vermelho};
`;

const RadioGroupButton = ({
  name,
  id,
  form,
  className,
  valorInicial,
  onChange,
  desabilitado,
  label,
  opcoes,
  value,
}) => {
  const obterErros = () => {
    return form && form.touched[name] && form.errors[name] ? (
      <Error>
        <span>{form.errors[name]}</span>
      </Error>
    ) : (
      ''
    );
  };

  const possuiErro = () => {
    return form && form.errors[name] && form.touched[name];
  };

  const campoComValidacoes = () => {
    return (
      <Field
        name={name}
        id={id || name}
        component={Radio.Group}
        options={opcoes}
        onChange={e => {
          form.setFieldValue(name, e.target.value);
          onChange(e);
          form.setFieldTouched(name, true, true);
        }}
        defaultValue={valorInicial}
        disabled={desabilitado}
        value={form.values[name]}
      />
    );
  };

  const campoSemValidacoes = () => {
    return (
      <Radio.Group
        id={id}
        options={opcoes}
        onChange={onChange}
        disabled={desabilitado}
        value={value || false}
      />
    );
  };

  return (
    <>
      <Campo className={className} invalido={possuiErro()}>
        {label ? <Label text={label} control={name || ''} /> : ''}
        {
          <>
            {form ? campoComValidacoes() : campoSemValidacoes()}
            {obterErros()}
          </>
        }
      </Campo>
    </>
  );
};

RadioGroupButton.propTypes = {
  className: PropTypes.string,
  label: PropTypes.string,
  desabilitado: PropTypes.bool,
  onChange: PropTypes.func,
};

RadioGroupButton.defaultProps = {
  className: '',
  label: '',
  desabilitado: false,
  onChange: () => {},
};

export default RadioGroupButton;
