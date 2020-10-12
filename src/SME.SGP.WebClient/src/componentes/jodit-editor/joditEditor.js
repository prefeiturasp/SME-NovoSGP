import React, {
  useEffect,
  useRef,
  forwardRef,
  useLayoutEffect,
  useState,
} from 'react';
import PropTypes from 'prop-types';
import api from '~/servicos/api';
import { Jodit } from 'jodit';
import 'jodit/build/jodit.min.css';
import { urlBase } from '~/servicos/variaveis';

const JoditEditor = forwardRef(
  ({ value, onChange, onBlur, tabIndex, name }, ref) => {
    const textArea = useRef(null);
    const [url, setUrl] = useState('');

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
      allowResizeY: false,
      readonly: false,
      enableDragAndDropFileToEditor: true,
      uploader: {
        url: `${url}/file/upload`, // 'http://localhost:5000/file/upload',
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
        defaultHandlerSuccess: function(dados) {
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
        defaultHandlerError: function(e) {
          debugger;
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
      console.log('interno');
      urlBase().then(resposta => {
        config.uploader.url = `${resposta}/file/upload`;
        const blurHandler = value => {
          onBlur && onBlur(value);
        };

        const changeHandler = value => {
          onChange && onChange(value);
        };

        const element = textArea.current;
        textArea.current = Jodit.make(element, config);
        textArea.current.value = value;

        textArea.current.events.on('blur', () =>
          blurHandler(textArea.current.value)
        );
        textArea.current.events.on('change', (novo, antigo) =>
          changeHandler(textArea.current.value)
        );

        textArea.current.workplace.tabIndex = tabIndex || -1;
        setUrl(resposta);
      });
    }, []);

    return <textarea ref={textArea} name={name} />;
  }
);

JoditEditor.propTypes = {
  value: PropTypes.string,
  tabIndex: PropTypes.number,
  config: PropTypes.object,
  onChange: PropTypes.func,
  onBlur: PropTypes.func,
};

export default JoditEditor;
