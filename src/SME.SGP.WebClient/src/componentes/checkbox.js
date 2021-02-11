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

const CheckboxComponent = props => {
  const {
    label,
    onChangeCheckbox,
    defaultChecked,
    className,
    disabled,
    checked,
    name,
    id,
    form,
  } = props;

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
        component={Checkbox}
        onChange={e => {
          form.setFieldValue(name, e.target.checked);
          onChangeCheckbox(e);
          form.setFieldTouched(name, true, true);
        }}
        defaultChecked={defaultChecked}
        disabled={disabled}
        checked={form.values[name]}
      >
        {label}
      </Field>
    );
  };

  const campoSemValidacoes = () => {
    return (
      <Checkbox
        onChange={onChangeCheckbox}
        defaultChecked={defaultChecked}
        disabled={disabled}
        checked={checked}
      >
        {label}
      </Checkbox>
    );
  };

  return (
    <Container className={className}>
      {form ? campoComValidacoes() : campoSemValidacoes()}
      <br />
      {obterErros()}
    </Container>
  );
};

CheckboxComponent.propTypes = {
  label: PropTypes.string,
  onChangeCheckbox: PropTypes.func,
  defaultChecked: PropTypes.bool,
  className: PropTypes.string,
  disabled: PropTypes.bool,
  checked: PropTypes.bool,
  name: PropTypes.string,
  id: PropTypes.string,
  form: PropTypes.oneOfType([PropTypes.any]),
};

CheckboxComponent.defaultProps = {
  label: 'Checkbox Component',
  onChangeCheckbox: () => {},
  defaultChecked: false,
  className: '',
  disabled: false,
  checked: false,
  name: '',
  id: '',
  form: null,
};

export default CheckboxComponent;
