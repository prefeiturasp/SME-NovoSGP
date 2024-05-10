import { Field } from 'formik';
import { Jodit } from 'jodit';
import 'jodit/build/jodit.min.css';
import PropTypes from 'prop-types';
import React, { forwardRef, useEffect, useRef, useState } from 'react';
import styled from 'styled-components';
import { store } from '~/redux';
import { erro } from '~/servicos/alertas';
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

let CHANGE_DEBOUNCE_FLAG;
const TAMANHO_MAXIMO_UPLOAD = 100;

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
    permiteInserirArquivo,
    readonly,
    removerToolbar,
    iframeStyle,
  } = props;

  const textArea = useRef(null);

  const [url, setUrl] = useState('');
  const { token } = store.getState().usuario;

  const [validacaoComErro, setValidacaoComErro] = useState(false);

  const BOTOES_PADRAO = !removerToolbar
    ? `bold,ul,ol,outdent,indent,font,fontsize,brush,paragraph,${
        permiteInserirArquivo ? 'file,' : ''
      }table,link,align,undo,redo`
    : '';

  const changeHandler = valor => {
    if (onChange) {
      onChange(valor);
    }
  };

  const excedeuLimiteMaximo = arquivo => {
    const tamanhoArquivo = arquivo.size / 1024 / 1024;
    return tamanhoArquivo > TAMANHO_MAXIMO_UPLOAD;
  };

  const config = {
    events: {
      afterRemoveNode: node => {
        if (node.nodeName === 'IMG') {
          // TODO Aqui chamar endpoint para remover a imagem no servidor!
        }
      },
      validarSeTemErro: () => {
        if (validarSeTemErro) {
          const texto = textArea?.current?.text?.trim();
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
      },
    },
    disablePlugins: ['image-properties', 'inline-popup'],
    language: 'pt_br',
    height,
    readonly: readonly || desabilitar,
    enableDragAndDropFileToEditor: true,
    uploader: {
      buildData: data => {
        return new Promise((resolve, reject) => {
          if (permiteInserirArquivo) {
            const arquivo = data.getAll('files[0]')[0];

            if (excedeuLimiteMaximo(arquivo)) {
              const msg = 'Tamanho máximo 100mb';
              erro(msg);
              reject(new Error(msg));
            }

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
          } else {
            reject(new Error('Não é possível inserir arquivo'));
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
          contentType: resp.data.contentType,
        };
      },
      defaultHandlerSuccess: dados => {
        if (dados?.path) {
          if (dados.contentType.startsWith('video')) {
            textArea.current.selection.insertHTML(
              `<video width="600" height="240" controls><source src="${dados.path}"></video>`
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
    style: {
      font: '16px Arial',
      overflow: 'none',
    },
    iframeStyle,
  };

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
    if (textArea?.current?.editorIsActive) {
      if (CHANGE_DEBOUNCE_FLAG) clearTimeout(CHANGE_DEBOUNCE_FLAG);

      CHANGE_DEBOUNCE_FLAG = setTimeout(() => {
        if (form) {
          onChangeComForm();
        } else {
          onChangePadrao();
        }
      }, 300);
    }
  };

  useEffect(() => {
    if (url) {
      const element = textArea.current || '';
      if (textArea?.current && config) {
        if (textArea?.current?.type === 'textarea') {
          textArea.current = Jodit.make(element, config);

          if (
            textArea?.current?.editorDocument?.getElementsByClassName(
              'jodit'
            )?.[0]?.style
          ) {
            textArea.current.editorDocument.getElementsByClassName(
              'jodit'
            )[0].style.cssText = 'overflow: auto;';
          }

          if (ref) {
            if (typeof ref === 'function') {
              ref(textArea.current);
            } else {
              ref.current = textArea.current;
            }
          }

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

  useEffect(() => {
    if (config && textArea?.current && textArea?.current?.type !== 'textarea') {
      textArea.current.setReadOnly(desabilitar);
    }
  }, [desabilitar]);

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
  permiteInserirArquivo: PropTypes.bool,
  readonly: PropTypes.bool,
  removerToolbar: PropTypes.bool,
  iframeStyle: PropTypes.string,
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
  permiteInserirArquivo: true,
  readonly: false,
  removerToolbar: false,
  iframeStyle: '',
};

export default JoditEditor;
