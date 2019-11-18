import React, { useState, useEffect } from 'react';
import PropTypes from 'prop-types';

// Form
import { Field } from 'formik';

// Componentes
import { Input, Button } from 'antd';

// Styles
import { InputRFEstilo } from './styles';

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
  onChange,
  style,
}) {
  const [valor, setValor] = useState('');

  useEffect(() => {
    setValor(pessoaSelecionada && pessoaSelecionada.rf);
    form.setFieldValue(name, pessoaSelecionada && pessoaSelecionada.rf);
  }, [pessoaSelecionada]);

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
    if (form && form.initialValues) {
      setValor(form.initialValues.professorRf);
    }
  }, [form.initialValues]);

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
              onSelect(e);
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
  pessoaSelecionada: PropTypes.objectOf(PropTypes.object),
  onSelect: PropTypes.func,
};

InputRF.defaultProps = {
  pessoaSelecionada: {},
  onSelect: () => null,
};

export default InputRF;
