import React, { useState, useEffect } from 'react';
import PropTypes from 'prop-types';

// Form
import { Field } from 'formik';

// Componentes
import { Input, Button } from 'antd';

// Styles
import { InputRFEstilo } from './styles';

// Funções
import { valorNuloOuVazio } from '~/utils/funcoes/gerais';

function InputRF({
  pessoaSelecionada,
  onSelect,
  form,
  name,
  id,
  className,
  desabilitado,
  maxlength,
  onKeyDown,
  style,
}) {
  const [valor, setValor] = useState('');

  const onSubmitRF = rf => {
    onSelect({ rf });
  };

  const botao = (
    <Button onClick={() => onSubmitRF(valor)} disabled={!valor} type="link">
      <i className="fa fa-search fa-lg" />
    </Button>
  );

  const possuiErro = () => {
    return form && form.errors[name] && form.touched[name];
  };

  const executaOnBlur = event => {
    const { relatedTarget } = event;
    if (relatedTarget && relatedTarget.getAttribute('type') === 'button') {
      event.preventDefault();
    }
  };

  useEffect(() => {
    setValor(pessoaSelecionada && pessoaSelecionada.professorRf);
    form.setFieldValue(
      name,
      pessoaSelecionada && pessoaSelecionada.professorRf
    );
  }, [pessoaSelecionada]);

  useEffect(() => {
    if (form && form.initialValues) {
      setValor(form.initialValues.professorRf);
    }
  }, [form.initialValues]);

  useEffect(() => {
    const { values: valores } = form;
    if (valores && valorNuloOuVazio(valores.professorRf)) {
      setValor('');
    }
  }, [form.values]);

  return (
    <>
      {form ? (
        <InputRFEstilo>
          <Field
            name={name}
            id={id || name}
            className={`campo ${possuiErro() ? 'is-invalid' : ''} ${className ||
              ''} ${desabilitado ? 'desabilitado' : ''}`}
            component={Input}
            readOnly={desabilitado}
            onBlur={executaOnBlur}
            maxLength={maxlength || 7}
            value={valor}
            placeholder="Digite o RF"
            onKeyDown={onKeyDown}
            onChange={e => {
              form.setFieldValue(name, e.target.value);
              form.setFieldTouched(name, true, true);
              setValor(e.target.value);
            }}
            style={style}
            suffix={botao}
            onPressEnter={e => onSubmitRF(e.target.value)}
          />
          {form && form.touched[name] ? (
            <span className="mensagemErro">{form.errors[name]}</span>
          ) : (
            ''
          )}
        </InputRFEstilo>
      ) : (
        <InputRFEstilo>
          <Input
            value={valor}
            placeholder="Digite o RF"
            onChange={e => setValor(e.target.value)}
            onPressEnter={e => onSubmitRF(e.target.value)}
            suffix={botao}
          />
        </InputRFEstilo>
      )}
    </>
  );
}

InputRF.propTypes = {
  pessoaSelecionada: PropTypes.oneOfType([
    PropTypes.objectOf(PropTypes.object),
    PropTypes.any,
  ]),
  onSelect: PropTypes.func,
  form: PropTypes.oneOfType([
    PropTypes.objectOf(PropTypes.object),
    PropTypes.any,
  ]),
  name: PropTypes.string,
  id: PropTypes.string,
  className: PropTypes.string,
  desabilitado: PropTypes.bool,
  maxlength: PropTypes.number,
  onKeyDown: PropTypes.func,
  style: PropTypes.objectOf(PropTypes.object),
};

InputRF.defaultProps = {
  pessoaSelecionada: {},
  onSelect: () => null,
  form: {},
  name: '',
  id: '',
  className: '',
  desabilitado: false,
  maxlength: null,
  onKeyDown: () => null,
  style: {},
};

export default InputRF;
