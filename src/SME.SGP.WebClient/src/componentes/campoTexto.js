import { Input } from 'antd';
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
  .ant-input {
    height: 38px;
  }
`;

const CampoTexto = React.forwardRef((props, ref) => {
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
    style,
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
              component={type || 'input'}
              type={maskType && maskType}
              readOnly={desabilitado}
              onBlur={executaOnBlur}
              maxLength={maxlength || ''}
              innerRef={ref}
              onKeyDown={onKeyDown}
              onChange={e => {
                form.setFieldValue(name, e.target.value);
                form.setFieldTouched(name, true);
                onChange(e);
              }}
              style={style}
            />
            {!semMensagem ? <span>{form.errors[name]}</span> : ''}
          </>
        ) : (
          <Input
            ref={ref}
            placeholder={placeholder}
            onChange={onChange}
            disabled={desabilitado}
            onKeyDown={onKeyDown}
            value={value}
          />
        )}
      </Campo>
    </>
  );
});

CampoTexto.propTypes = {
  onChange: PropTypes.func,
  semMensagem: PropTypes.bool,
};

CampoTexto.defaultProps = {
  onChange: () => {},
  semMensagem: false,
};

export default CampoTexto;
