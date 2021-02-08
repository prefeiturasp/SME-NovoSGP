import { Checkbox } from 'antd';
import PropTypes from 'prop-types';
import React from 'react';
import styled from 'styled-components';
import { Field } from 'formik';

import { Base } from './colors';

const Container = styled.div`
  .ant-checkbox-checked .ant-checkbox-inner {
    background-color: ${Base.Roxo};
    border-color: ${Base.Roxo};
  }
  .ant-checkbox-wrapper:hover .ant-checkbox-inner,
  .ant-checkbox:hover .ant-checkbox-inner,
  .ant-checkbox-input:focus + .ant-checkbox-inner {
    border-color: ${Base.Roxo};
  }
`;

const CheckboxGroup = props => {
  const { onChange, defaultValue, disabled, options, name, id, form } = props;

  const Error = styled.span`
    color: ${Base.Vermelho};
  `;

  const obterErros = () => {
    return form && form.touched[name] && form.errors[name] ? (
      <Error>
        <span>{form.errors[name]}</span>
      </Error>
    ) : (
      ''
    );
  };

  const campoComValidacoes = () => {
    return (
      <Field
        name={name}
        id={id || name}
        component={Checkbox.Group}
        options={options}
        defaultValue={form.values[name]}
        disabled={disabled}
        onChange={checkedValues => {
          form.setFieldValue(name, checkedValues);
          onChange(checkedValues);
          form.setFieldTouched(name, true, true);
        }}
      />
    );
  };

  const campoSemValidacoes = () => {
    return (
      <Checkbox.Group
        options={options}
        defaultValue={defaultValue}
        onChange={onChange}
        disabled={disabled}
      />
    );
  };

  return (
    <Container>
      {form ? campoComValidacoes() : campoSemValidacoes()}
      <br />
      {obterErros()}
    </Container>
  );
};

CheckboxGroup.propTypes = {
  onChange: PropTypes.func,
  defaultValue: PropTypes.oneOfType([PropTypes.array]),
  options: PropTypes.oneOfType([PropTypes.array]),
  disabled: PropTypes.bool,
  name: PropTypes.string,
  id: PropTypes.string,
  form: PropTypes.oneOfType([PropTypes.any]),
};

CheckboxGroup.defaultProps = {
  onChange: () => {},
  defaultValue: [],
  options: [],
  disabled: false,
  name: '',
  id: '',
  form: null,
};

export default CheckboxGroup;
