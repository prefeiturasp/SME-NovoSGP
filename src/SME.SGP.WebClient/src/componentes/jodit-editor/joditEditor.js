import { Field } from 'formik';
import { Jodit } from 'jodit';
import 'jodit/build/jodit.min.css';
import PropTypes from 'prop-types';
import React, {
  useEffect,
  useLayoutEffect,
  useRef,
  useState,
  forwardRef,
} from 'react';
import styled from 'styled-components';
import { store } from '~/redux';
import { urlBase } from '~/servicos/variaveis';
import { Base } from '../colors';
import Label from '../label';

const Campo = styled.div`
  .campo-invalido {
    .jodit-container {
      border-color: #dc3545 !important;
    }
  }
`;

const JoditEditor = forwardRef((props, ref) => {
  const {
    value,
    onChange,
    tabIndex,
    desabilitar,
    height,
    label,
    name,
    id,
    form,
    temErro,
    mensagemErro,
    validarSeTemErro,
  } = props;

  const textArea = useRef(null);

  const [url, setUrl] = useState('');
  const { token } = store.getState().usuario;

  const [validacaoComErro, setValidacaoComErro] = useState(false);

  const changeHandler = valor => {
    if (onChange) {
      textArea.current.value = valor;
      onChange(valor);
    }
  };

  const config = {
    events: {
      afterRemoveNode: node => {
        if (node.nodeName === 'IMG') {
          console.log('Removendo imagem');
          //TODO REQUEST REMOVENDO A IMAGEM, EX:
          //fetch("delete.php", { image: node.src });
        }
      },
    },
    language: 'pt_br',
    height,
    allowResizeY: false,
    disabled: desabilitar,
    enableDragAndDropFileToEditor: true,
    uploader: {
      url: `${url}/file/upload`,
      headers: {
        Authorization: `Bearer ${token}`,
      },
      isSuccess: resp => {
        return resp;
      },
      process: resp => {
        return {
          files: resp.data.files,
          path: resp.data.path,
          baseurl: resp.data.baseurl,
          error: resp.data.error,
          message: resp.data.message,
        };
      },
      defaultHandlerSuccess: dados => {
        let i;
        const field = 'files';
        if (dados[field] && dados[field].length) {
          for (i = 0; i < dados[field].length; i += 1) {
            const file = dados.baseurl + dados[field][i];
            if (dados.path.endsWith('mp4')) {
              textArea.current.selection.insertHTML(
                `<video width="600" height="240" controls><source src="${file}" type="video/mp4"></video>`
              );
            } else textArea.current.selection.insertImage(file);
          }
        }
      },
      defaultHandlerError: e => {
        console.log('defaultHandlerError');
        console.log(e);
      },
    },
    iframe: true,
    showWordsCounter: false,
    showXPathInStatusbar: false,
    buttons:
      '|,bold,ul,ol,|,outdent,indent,|,font,fontsize,brush,paragraph,|,file,video,table,link,|,align,undo,redo,\n,|',
  };

  useLayoutEffect(() => {
    if (ref) {
      if (typeof ref === 'function') {
        ref(textArea.current);
      } else {
        ref.current = textArea.current;
      }
    }
  }, [textArea]);

  useEffect(() => {
    if (!url) {
      urlBase().then(resposta => {
        setUrl(resposta);
      });
    }
  }, [url]);

  useEffect(() => {
    if (url) {
      const element = textArea.current || '';
      if (textArea?.current && config) {
        if (textArea?.current?.type === 'textarea') {
          textArea.current = Jodit.make(element, config);
          textArea.current.workplace.tabIndex = tabIndex;
        }
      }
    }
  }, [url]);

  const possuiErro = () => {
    return (
      (form && form.errors[name] && form.touched[name]) ||
      temErro ||
      validacaoComErro
    );
  };

  const editorComValidacoes = () => {
    config.events.change = () => {
      const texto = textArea?.current?.text?.trim();

      let valorAtual = '';
      if (texto) {
        valorAtual = textArea.current.value;
      }
      if (textArea.current.value !== form.values[name]) {
        changeHandler(valorAtual);
        form.setFieldValue(name, valorAtual || '');
        form.setFieldTouched(name, true, true);
      }
    };

    if (
      form &&
      textArea?.current &&
      textArea.current.value !== form.values[name]
    ) {
      textArea.current.value = form.values[name] || '';
    }

    return (
      <Campo>
        <div
          className={validacaoComErro || possuiErro() ? 'campo-invalido' : ''}
        >
          <Field name={name} id={id}>
            {() => (
              <textarea ref={textArea} hidden={!textArea?.current?.isJodit} />
            )}
          </Field>
        </div>
      </Campo>
    );
  };

  const editorSemValidacoes = () => {
    config.events.change = () => {
      const texto = textArea?.current?.text?.trim();
      if (validarSeTemErro) {
        setValidacaoComErro(validarSeTemErro(texto));
      }
      if (texto) {
        changeHandler(textArea.current.value);
      } else {
        changeHandler('');
      }
    };

    return (
      <Campo>
        <div
          className={validacaoComErro || possuiErro() ? 'campo-invalido' : ''}
        >
          <textarea
            ref={textArea}
            id={id}
            hidden={!textArea?.current?.isJodit}
            value={value}
          />
        </div>
      </Campo>
    );
  };

  const obterErros = () => {
    return (form && form.touched[name] && form.errors[name]) ||
      temErro ||
      validacaoComErro ? (
      <span style={{ color: `${Base.Vermelho}` }}>
        {(form && form.errors[name]) || mensagemErro}
      </span>
    ) : (
      ''
    );
  };

  return (
    <>
      {label ? <Label text={label} control={name} /> : ''}
      {form ? editorComValidacoes() : editorSemValidacoes()}
      {obterErros()}
    </>
  );
});

JoditEditor.propTypes = {
  value: PropTypes.string,
  tabIndex: PropTypes.number,
  onChange: PropTypes.func,
  desabilitar: PropTypes.bool,
  height: PropTypes.string,
  label: PropTypes.string,
  name: PropTypes.string,
  id: PropTypes.string,
  form: PropTypes.oneOfType([PropTypes.any]),
  temErro: PropTypes.bool,
  mensagemErro: PropTypes.string,
  validarSeTemErro: PropTypes.func,
};

JoditEditor.defaultProps = {
  value: '',
  tabIndex: -1,
  onChange: null,
  desabilitar: false,
  height: 'auto',
  label: '',
  name: '',
  id: 'editor',
  form: null,
  temErro: false,
  mensagemErro: '',
  validarSeTemErro: null,
};

export default JoditEditor;
