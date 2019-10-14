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
}) => {
  return (
    <>
      <Campo className={className}>
        {label ? <Label text={label} control={name || ''} /> : ''}
        {
          <>
            <Field
              name={name}
              id={id || name}
              component={Radio.Group}
              name={name}
              options={opcoes}
              onChange={e => {
                form.setFieldValue(name, e.target.value);
                onChange(e);
              }}
              defaultValue={valorInicial}
              disabled={desabilitado}
              value={form.values[name]}
            />
            <br />
            <Error>
              <span>{form.errors[name]}</span>
            </Error>
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
