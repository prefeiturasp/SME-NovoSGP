import { InputNumber } from 'antd';
import { Field } from 'formik';
import PropTypes from 'prop-types';
import React from 'react';
import styled from 'styled-components';
import { Base } from './colors';
import Label from './label';

const Campo = styled.div`
  span {
    color: ${Base.Vermelho};
  }
  .campo {
    margin-bottom: 5px;
  }
  .ant-input-number {
    height: 38px;
  }

  height: 45px;
`;

const CampoNumero = React.forwardRef((props, ref) => {
  const {
    name,
    id,
    form,
    className,
    classNameCampo,
    type,
    maskType,
    placeholder,
    onChange,
    onKeyDown,
    value,
    desabilitado,
    maxlength,
    label,
    semMensagem,
    max,
  } = props;

  const possuiErro = () => {
    return form && form.errors[name] && form.touched[name];
  };

  const executaOnBlur = event => {
    const { relatedTarget } = event;
    if (relatedTarget && relatedTarget.getAttribute('type') === 'button') {
      event.preventDefault();
    }
  };

  return (
    <>
      <Campo className={classNameCampo}>
        {label ? <Label text={label} control={name || ''} /> : ''}
        {form ? (
          <>
            {' '}
            <Field
              name={name}
              id={id || name}
              className={`form-control campo ${
                possuiErro() ? 'is-invalid' : ''
              } ${className || ''} ${desabilitado ? 'desabilitado' : ''}`}
              component={InputNumber}
              type={maskType && maskType}
              readOnly={desabilitado}
              onBlur={executaOnBlur}
              maxLength={maxlength || ''}
              innerRef={ref}
              onKeyDown={onKeyDown}
              onChange={value => {
                form.setFieldValue(name, value);
                form.setFieldTouched(name, true);
                onChange(value);
              }}
            />
            {!semMensagem ? <span>{form.errors[name]}</span> : ''}
          </>
        ) : (
          <InputNumber
            ref={ref}
            placeholder={placeholder}
            onChange={onChange}
            readOnly={desabilitado}
            onKeyDown={onKeyDown}
            value={value}
            max={max}
          />
        )}
      </Campo>
    </>
  );
});

CampoNumero.propTypes = {
  onChange: PropTypes.func,
  semMensagem: PropTypes.bool,
  form: () => {},
};

CampoNumero.defaultProps = {
  onChange: () => {},
  semMensagem: false,
  form: PropTypes.oneOfType([
    PropTypes.symbol,
    PropTypes.any,
    PropTypes.func,
    PropTypes.object,
  ]),
};

export default CampoNumero;
