import React from 'react';
import PropTypes from 'prop-types';
import styled from 'styled-components';
import { Select } from 'antd';
import Icon from 'antd/es/icon';
import shortid from 'shortid';
import { Field } from 'formik';
import { Base } from './colors';
import Label from './label';

const Container = styled.div`
  .ant-select {
    width: 100%;
  }

  .ant-select-arrow {
    color: ${Base.CinzaMako};
  }
  .ant-select-selection--single {
    align-items: center;
    display: flex;
    height: 38px;
  }
  .ant-select-selection__rendered {
    width: 98%;
  }
  .ant-select-selection__placeholder {
    display: block;
  }
  .ant-select .ant-select-search__field {
    display: block;
    max-width: 100% !important;
  }
  .ant-select-selection-selected-value {
    font-weight: bold;
  }
  .ant-select-selection--multiple {
    min-height: 38px;

    .ant-select-selection__placeholder {
      line-height: 25px;
    }

    .ant-select-selection__rendered {
      margin-top: 3px;
    }
  }

  div[class*='is-invalid'] {
    .ant-select-selection {
      border-color: #dc3545 !important;
    }
  }
`;

const Erro = styled.span`
  color: ${Base.Vermelho};
`;

const SelectComponent = React.forwardRef((props, ref) => {
  const {
    name,
    id,
    className,
    classNameContainer,
    onChange,
    label,
    valueText,
    valueOption,
    valueSelect,
    lista,
    placeholder,
    alt,
    multiple,
    disabled,
    form,
  } = props;

  const { Option } = Select;

  const possuiErro = () => {
    return form && form.errors[name] && form.touched[name];
  };

  const opcoesLista = () => {
    return (
      lista.length &&
      lista.map(item => {
        return (
          <Option key={shortid.generate()} value={`${item[valueOption]}`}>
            {`${item[valueText]}`}
          </Option>
        );
      })
    );
  };

  const campoComValidacoes = () => (
    <Field
      mode={multiple && 'multiple'}
      suffixIcon={<Icon type="caret-down" />}
      className={
        form
          ? `overflow-hidden ${possuiErro() ? 'is-invalid' : ''} ${className ||
              ''}`
          : ''
      }
      name={name}
      id={id || name}
      value={form.values[name]}
      placeholder={placeholder}
      notFoundContent="Sem dados"
      alt={alt}
      optionFilterProp="children"
      allowClear
      disabled={disabled}
      component={Select}
      type="input"
      onChange={e => {
        form.setFieldValue(name, e);
        onChange && onChange(e);
      }}
      innerRef={ref}
    >
      {opcoesLista()}
    </Field>
  );

  const campoSemValidacoes = () => (
    <Select
      mode={multiple && 'multiple'}
      suffixIcon={<Icon type="caret-down" />}
      className={`overflow-hidden ${className}`}
      name={name}
      id={id}
      onChange={onChange}
      value={valueSelect}
      placeholder={placeholder}
      notFoundContent="Sem dados"
      alt={alt}
      optionFilterProp="children"
      allowClear
      disabled={disabled}
      ref={ref}
    >
      {opcoesLista()}
    </Select>
  );
  return (
    <Container className={classNameContainer && classNameContainer}>
      {label ? <Label text={label} control={name} /> : ''}
      {form ? campoComValidacoes() : campoSemValidacoes()}
      {form ? <Erro>{form.errors[name]}</Erro> : ''}
    </Container>
  );
});

SelectComponent.propTypes = {
  name: PropTypes.string,
  id: PropTypes.string,
  className: PropTypes.string,
  onChange: PropTypes.func,
  label: PropTypes.string,
  valueText: PropTypes.string,
  valueOption: PropTypes.string.isRequired,
  valueSelect: PropTypes.any,
  lista: PropTypes.array,
  placeholder: PropTypes.string,
  disabled: PropTypes.bool,
};

export default SelectComponent;
