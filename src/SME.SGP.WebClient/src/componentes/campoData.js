import React from 'react';
import styled from 'styled-components';

import PropTypes from 'prop-types';
import { DatePicker } from 'antd';
import locale from 'antd/es/date-picker/locale/pt_BR';
import { Base } from './colors';
import 'moment/locale/pt-br';
import Label from './label';
import { Field } from 'formik';


const Campo = styled.div`
  span {
    color: ${Base.Vermelho};
  }

  span[class*='is-invalid'] {
    .ant-calendar-picker-input {
      border-color: #dc3545 !important;
    }
  }

  .ant-calendar-picker-input {
    height: 38px;
  }

  .ant-calendar-picker {
    width: 100%;
  }
`;

const CampoData = props => {
  const {
    formatoData,
    placeholder,
    label,
    name,
    id,
    form,
    desabilitado,
    className
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

  const campoDataAnt = () => {
    return (
      <Field
        disabled={desabilitado}
        locale={locale}
        format={formatoData}
        component={DatePicker}
        placeholder={placeholder}
        name={name}
        suffixIcon={<i className="fas fa-calendar-alt" />}
        name={name}
        id={id || name}
        onBlur={executaOnBlur}
        className={
          form
            ? `${possuiErro() ? 'is-invalid' : ''} ${className || ''}`
            : ''
        }
      />
    );
  };

  return (
    <>
      <Campo>
        <Label text={label} control={name} />
        {campoDataAnt()}
        {form ? <span>{form.errors[name]}</span> : ''}
      </Campo>
    </>
  );
};

CampoData.propTypes = {
  className: PropTypes.string,
  formatoData: PropTypes.string,
  placeholder: PropTypes.string,
  label: PropTypes.string,
  desabilitado: PropTypes.bool,
};

CampoData.defaultProps = {
  className: '',
  formatoData: 'DD/MM/YYYY HH:mm:ss',
  placeholder: 'placeholder',
  label: 'Label',
  desabilitado: false,
};

export default CampoData;
