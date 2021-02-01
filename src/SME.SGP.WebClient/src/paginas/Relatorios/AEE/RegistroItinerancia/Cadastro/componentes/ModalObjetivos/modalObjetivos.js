import PropTypes from 'prop-types';
import React, { useEffect, useState } from 'react';

import {
  CampoTexto,
  CheckboxComponent,
  ModalConteudoHtml,
} from '~/componentes';

import { confirmar } from '~/servicos';

import { TituloEstilizado } from './modalObjetivos.css';

const ModalObjetivos = ({
  modalVisivel,
  setModalVisivel,
  objetivosSelecionados,
  setObjetivosSelecionados,
}) => {
  const [listaObjetivos, setListaObjetivos] = useState([
    {
      key: '1',
      objetivo: 'Mapeamento dos estudantes público da educação especial',
      detalhamento: false,
      apenasUmaUe: true,
    },
    {
      key: '2',
      objetivo: 'Reorganização e/ou remanejamento de apoios e serviços',
      detalhamento: false,
      apenasUmaUe: true,
    },
    {
      key: '3',
      objetivo: 'Atendimento de solicitação da U.E',
      detalhamento: true,
      apenasUmaUe: true,
    },
    {
      key: '4',
      objetivo: 'Acompanhamento professor de sala regular',
      detalhamento: false,
      apenasUmaUe: false,
    },
    {
      key: '5',
      objetivo: 'Acompanhamento professor de SRM',
      detalhamento: false,
      apenasUmaUe: false,
    },
    {
      key: '6',
      objetivo: 'Ação Formativa em JEIF',
      detalhamento: false,
      apenasUmaUe: false,
    },
    {
      key: '7',
      objetivo: 'Reunião',
      detalhamento: false,
      apenasUmaUe: false,
    },
    {
      key: '8',
      objetivo: 'Outros',
      detalhamento: true,
      apenasUmaUe: true,
    },
  ]);
  const [objetivosSelecionadosModal, setObjetivosSelecionadosModal] = useState(
    listaObjetivos
  );
  const [modoEdicao, setModoEdicao] = useState(false);

  const esconderModal = () => setModalVisivel(false);

  const perguntarSalvarListaUsuario = async () => {
    const resposta = await confirmar(
      'Atenção',
      'Suas alterações não foram salvas, deseja salvar agora?'
    );
    return resposta;
  };

  const onConfirmarModal = () => {
    const arraySelecionados = objetivosSelecionadosModal.filter(
      item => item.checked
    );
    setObjetivosSelecionados(arraySelecionados);
    setModoEdicao(false);
    esconderModal();
  };

  const fecharModal = async () => {
    esconderModal();
    if (modoEdicao) {
      const ehPraSalvar = await perguntarSalvarListaUsuario();
      if (ehPraSalvar) {
        onConfirmarModal();
      }
    }
  };

  const onChangeCheckbox = item => {
    setObjetivosSelecionadosModal(estadoAntigo =>
      estadoAntigo.map(objetivo => {
        if (objetivo.key === item.key) {
          return {
            ...objetivo,
            checked: !objetivo.checked,
          };
        }
        return objetivo;
      })
    );
    setModoEdicao(true);
  };

  const onChangeCampoTexto = (evento, item) => {
    const texto = evento.target?.value;
    setObjetivosSelecionadosModal(estadoAntigo =>
      estadoAntigo.map(objetivo => {
        if (objetivo.key === item.key) {
          return {
            ...objetivo,
            detalhamentoTexto: texto,
          };
        }
        return objetivo;
      })
    );
    setModoEdicao(true);
  };

  useEffect(() => {
    if (Object.keys(objetivosSelecionados).length) {
      objetivosSelecionados.map(objetivo =>
        setObjetivosSelecionadosModal(estadoAntigo =>
          estadoAntigo.map(estado => {
            if (estado.key === objetivo.key) {
              return objetivo;
            }
            return estado;
          })
        )
      );
    }
  }, [objetivosSelecionados]);

  return (
    <ModalConteudoHtml
      titulo="Objetivos da itinerância"
      visivel={modalVisivel}
      onClose={fecharModal}
      onConfirmacaoPrincipal={onConfirmarModal}
      onConfirmacaoSecundaria={fecharModal}
      labelBotaoPrincipal="Adicionar objetivos"
      labelBotaoSecundario="Cancelar"
      closable
      width="50%"
      fecharAoClicarFora
      fecharAoClicarEsc
    >
      <div className="col-md-12 mt-n2">
        <div className="row mb-3">
          <TituloEstilizado>Selecione os objetivos</TituloEstilizado>
        </div>
        {objetivosSelecionadosModal &&
          objetivosSelecionadosModal.map(item => {
            const textoUe = item.apenasUmaUe
              ? '(apenas uma unidade)'
              : '(uma ou mais unidades)';

            return (
              <>
                <CheckboxComponent
                  key={item.key}
                  className="mb-3 ml-n2"
                  label={`${item.objetivo} ${textoUe}`}
                  name={`objetivo-${item.key}`}
                  onChangeCheckbox={() => onChangeCheckbox(item)}
                  disabled={false}
                  checked={item.checked}
                />
                {item.detalhamento && (
                  <div className="mb-3 pl-3 mr-n3">
                    <CampoTexto
                      height="76"
                      onChange={evento => onChangeCampoTexto(evento, item)}
                      type="textarea"
                      value={item.detalhamentoTexto}
                      desabilitado={!item.checked}
                    />
                  </div>
                )}
              </>
            );
          })}
      </div>
    </ModalConteudoHtml>
  );
};

ModalObjetivos.defaultProps = {
  modalVisivel: false,
  objetivosSelecionados: [],
  setModalVisivel: () => {},
  setObjetivosSelecionados: () => {},
};

ModalObjetivos.propTypes = {
  modalVisivel: PropTypes.bool,
  objetivosSelecionados: PropTypes.oneOfType([PropTypes.any]),
  setModalVisivel: PropTypes.func,
  setObjetivosSelecionados: PropTypes.func,
};

export default ModalObjetivos;
