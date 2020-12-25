import { Field } from 'formik';
import { Jodit } from 'jodit';
import 'jodit/build/jodit.min.css';
import PropTypes from 'prop-types';
import React, {
  forwardRef,
  useEffect,
  useLayoutEffect,
  useRef,
  useState,
} from 'react';
import styled from 'styled-components';
import { store } from '~/redux';
import { urlBase } from '~/servicos/variaveis';
import { Base } from '../colors';
import Label from '../label';
import { erro } from '~/servicos/alertas';

const Campo = styled.div`
  .campo-invalido {
    .jodit-container {
      border-color: #dc3545 !important;
    }
  }
`;

let CHANGE_DEBOUNCE_FLAG;

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

  const BOTOES_PADRAO =
    'bold,ul,ol,outdent,indent,font,fontsize,brush,paragraph,file,video,table,link,align,undo,redo,fullsize';

  const changeHandler = valor => {
    if (onChange) {
      if (!form) {
        textArea.current.value = valor;
      }
      onChange(valor);
    }
  };

  const config = {
    events: {
      afterRemoveNode: node => {
        if (node.nodeName === 'IMG') {
          // TODO Aqui chamar endpoint para remover a imagem no servidor!
        }
      },
    },
    disablePlugins: ['image-properties', 'inline-popup'],
    language: 'pt_br',
    height,
    disabled: desabilitar,
    enableDragAndDropFileToEditor: true,
    uploader: {
      buildData: data => {
        return new Promise((resolve, reject) => {
          const arquivo = data.getAll('files[0]')[0];
          if (
            arquivo.type.substring(0, 5) === 'image' ||
            arquivo.type.substring(0, 5) === 'video'
          ) {
            resolve(data);
          } else {
            const msg = 'Formato inválido';
            erro(msg);
            reject(new Error(msg));
          }
        });
      },
      url: `${url}/v1/arquivos/upload`,
      headers: {
        Authorization: `Bearer ${token}`,
      },
      isSuccess: resp => resp,
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
        if (dados?.path) {
          if (dados.path.endsWith('mp4')) {
            textArea.current.selection.insertHTML(
              `<video width="600" height="240" controls><source src="${dados.path}" type="video/mp4"></video>`
            );
          } else textArea.current.selection.insertImage(dados.path);
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
    buttons: BOTOES_PADRAO,
    buttonsXS: BOTOES_PADRAO,
    buttonsMD: BOTOES_PADRAO,
    buttonsSM: BOTOES_PADRAO,
    placeholder: '',
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

  const onChangePadrao = () => {
    const texto = textArea?.current?.text?.trim();

    if (validarSeTemErro) {
      let valorParaValidar = '';

      if (
        !texto ||
        textArea.current.value.includes('<video') ||
        textArea.current.value.includes('<img')
      ) {
        valorParaValidar = textArea.current.value;
      } else if (texto) {
        valorParaValidar = texto;
      }
      setValidacaoComErro(validarSeTemErro(valorParaValidar));
    }

    if (
      texto ||
      textArea.current.value.includes('<video') ||
      textArea.current.value.includes('<img')
    ) {
      changeHandler(textArea.current.value);
    } else {
      changeHandler('');
    }
  };

  const onChangeComForm = () => {
    const texto = textArea?.current?.text?.trim();
    let valorAtual = '';

    if (
      texto ||
      textArea.current.value.includes('<video') ||
      textArea.current.value.includes('<img')
    ) {
      valorAtual = textArea.current.value;
    }

    changeHandler(valorAtual);
    form.setFieldValue(name, valorAtual);
    form.setFieldTouched(name, true, true);
  };

  const beforeOnChange = () => {
    if (CHANGE_DEBOUNCE_FLAG) clearTimeout(CHANGE_DEBOUNCE_FLAG);

    CHANGE_DEBOUNCE_FLAG = setTimeout(() => {
      if (form) {
        onChangeComForm();
      } else {
        onChangePadrao();
      }
    }, 300);
  };

  useEffect(() => {
    if (url) {
      const element = textArea.current || '';
      if (textArea?.current && config) {
        if (textArea?.current?.type === 'textarea') {
          textArea.current = Jodit.make(element, config);

          textArea.current.events.on('change', () => {
            beforeOnChange();
          });

          textArea.current.workplace.tabIndex = tabIndex;
        }
      }
    }
  }, [url]);

  useEffect(() => {
    if (textArea && textArea.current) {
      textArea.current.value = value;
    }
  }, [textArea, value]);

  const possuiErro = () => {
    return (
      (form && form.errors[name] && form.touched[name]) ||
      temErro ||
      validacaoComErro
    );
  };

  const editorComValidacoes = () => {
    return (
      <Campo>
        <div
          className={validacaoComErro || possuiErro() ? 'campo-invalido' : ''}
        >
          <Field name={name} id={id} value={value}>
            {() => (
              <textarea ref={textArea} hidden={!textArea?.current?.isJodit} />
            )}
          </Field>
        </div>
      </Campo>
    );
  };

  const editorSemValidacoes = () => {
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
      {label ? <Label text={label} /> : ''}
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
