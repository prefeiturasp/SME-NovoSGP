import PropTypes from 'prop-types';
import React, { useCallback, useEffect, useState } from 'react';

import {
  Colors,
  Loader,
  ModalConteudoHtml,
  SelectComponent,
} from '~/componentes';
import { FiltroHelper } from '~/componentes-sgp';

import { AbrangenciaServico, confirmar, erros } from '~/servicos';

import { BotaoEstilizado, TextoEstilizado } from './modalUE.css';

const ModalUE = ({
  modalVisivel,
  permiteApenasUmaUe,
  temAlunosSelecionados,
  setModalVisivel,
  setUnEscolaresSelecionados,
  unEscolaresSelecionados,
  setModoEdicaoItinerancia,
  desabilitarBotaoExcluir,
}) => {
  const [anoLetivo] = useState(window.moment().format('YYYY'));
  const [uesSelecionadas, setUESSelecionadas] = useState(
    unEscolaresSelecionados
  );
  const [listaDres, setListaDres] = useState([]);
  const [listaUes, setListaUes] = useState([]);
  const [modoEdicao, setModoEdicao] = useState(false);
  const [dreId, setDreId] = useState();
  const [ueId, setUeId] = useState();
  const [carregandoDres, setCarregandoDres] = useState(false);
  const [carregandoUes, setCarregandoUes] = useState(false);
  const [inputsDesabilitado, setInputsDesabilitado] = useState(false);

  const removerUES = key => {
    setUESSelecionadas(estadoAntigo =>
      estadoAntigo.filter(item => item.key !== key)
    );
    setModoEdicao(true);
  };

  const esconderModal = () => setModalVisivel(false);

  const perguntarSalvarListaUES = async () => {
    const resposta = await confirmar(
      'Atenção',
      'Suas alterações não foram salvas, deseja salvar agora?'
    );
    return resposta;
  };

  const onConfirmarModal = () => {
    setUnEscolaresSelecionados(uesSelecionadas);
    setModoEdicao(false);
    setModoEdicaoItinerancia(true);
    esconderModal();
  };

  const fecharModal = async () => {
    esconderModal();
    if (modoEdicao) {
      const ehPraSalvar = await perguntarSalvarListaUES();
      if (ehPraSalvar) {
        onConfirmarModal();
      }
    }
  };

  const onChangeDre = dre => {
    setDreId(dre);
    setListaUes([]);
    setUeId();
  };

  const onChangeUe = ue => {
    setUeId(ue);
    if (dreId && ue) {
      const ueSelecionada = listaUes.find(
        item => String(item.valor) === String(ue)
      );
      setUESSelecionadas(estadoAntigo => {
        const unidade = estadoAntigo.find(
          item => item.ueId === ueSelecionada.id
        );
        if (unidade) {
          return estadoAntigo;
        }
        return [
          ...estadoAntigo,
          {
            key: ueSelecionada.valor,
            ueId: ueSelecionada.id,
            descricao: ueSelecionada.desc,
            codigoUe: ue,
            podeRemover: true,
            ehInfantil: ueSelecionada.ehInfantil,
            codigoDre: dreId,
          },
        ];
      });
      setModoEdicao(true);
      setUeId();
    }
  };

  const obterDres = useCallback(async () => {
    if (anoLetivo) {
      setCarregandoDres(true);
      const resposta = await AbrangenciaServico.buscarDres(
        `v1/abrangencias/false/dres?anoLetivo=${anoLetivo}`
      )
        .catch(e => erros(e))
        .finally(() => setCarregandoDres(false));

      if (resposta?.data?.length) {
        const lista = resposta.data
          .map(item => ({
            desc: item.nome,
            valor: String(item.codigo),
            abrev: item.abreviacao,
            id: item.id,
          }))
          .sort(FiltroHelper.ordenarLista('desc'));
        setListaDres(lista);

        if (lista && lista.length && lista.length === 1) {
          setDreId(lista[0].valor);
        }
        return;
      }
    }
    setListaDres([]);
    setDreId(undefined);
  }, [anoLetivo]);

  useEffect(() => {
    if (anoLetivo) {
      obterDres();
    }
  }, [anoLetivo, obterDres]);

  const obterUes = useCallback(async () => {
    if (anoLetivo && dreId) {
      setCarregandoUes(true);
      const resposta = await AbrangenciaServico.buscarUes(
        dreId,
        `v1/abrangencias/false/dres/${dreId}/ues?anoLetivo=${anoLetivo}`,
        true
      )
        .catch(e => erros(e))
        .finally(() => setCarregandoUes(false));

      if (resposta?.data) {
        const lista = resposta.data.map(item => ({
          desc: item.nome,
          valor: String(item.codigo),
          id: item.id,
          ehInfantil: item.ehInfantil,
        }));

        if (lista?.length === 1) {
          const ue = lista[0].valor;
          setUeId(ue);
          const ueSelecionada = lista.find(
            item => String(item.valor) === String(ue)
          );
          setUESSelecionadas([
            {
              key: ueSelecionada.valor,
              descricao: ueSelecionada.desc,
              codigoUe: ue,
              codigoDre: dreId,
              ueId: ueSelecionada.id,
              ehInfantil: ueSelecionada.ehInfantil,
            },
          ]);
        }
        setListaUes(lista);
        return;
      }
    }
    setListaUes([]);
  }, [dreId, anoLetivo]);

  useEffect(() => {
    if (dreId) {
      obterUes();
      return;
    }
    setUeId();
    setListaUes([]);
  }, [dreId, obterUes]);

  useEffect(() => {
    if (!inputsDesabilitado && uesSelecionadas.length && permiteApenasUmaUe) {
      setInputsDesabilitado(true);
    }
    if (!uesSelecionadas.length) {
      setInputsDesabilitado(false);
    }
  }, [inputsDesabilitado, permiteApenasUmaUe, uesSelecionadas]);

  return (
    <ModalConteudoHtml
      titulo="Selecione a(s) Unidade(s) Escolar(es)"
      visivel={modalVisivel}
      esconderBotaoSecundario
      onClose={fecharModal}
      onConfirmacaoPrincipal={onConfirmarModal}
      labelBotaoPrincipal="Confirmar"
      closable
      width="50%"
      fecharAoClicarFora
      fecharAoClicarEsc
    >
      <div
        className={`col-md-12 position-relative d-flex mb-${
          permiteApenasUmaUe ? 5 : 4
        } p-0`}
      >
        <div className="col-6 p-0">
          <Loader loading={carregandoDres} tip="">
            <SelectComponent
              id="dre"
              label="Diretoria Regional de Educação (DRE)"
              lista={listaDres}
              valueOption="valor"
              valueText="desc"
              disabled={listaDres?.length === 1 || inputsDesabilitado}
              onChange={onChangeDre}
              valueSelect={dreId}
              placeholder="Diretoria Regional De Educação (DRE)"
            />
          </Loader>
        </div>
        <div className="col-6 pr-0">
          <Loader loading={carregandoUes} tip="">
            <SelectComponent
              id="ue"
              label="Unidade Escolar (UE)"
              lista={listaUes}
              valueOption="valor"
              valueText="desc"
              disabled={
                !dreId ||
                listaUes?.length === 1 ||
                inputsDesabilitado ||
                temAlunosSelecionados
              }
              onChange={onChangeUe}
              valueSelect={ueId}
              placeholder="Unidade Escolar (UE)"
            />
          </Loader>
        </div>
        {permiteApenasUmaUe && (
          <TextoEstilizado>
            De acordo com os objetivos selecionados você pode selecionar apenas
            uma Unidade Escolar. Edite os objetivos caso precise selecionar mais
            de uma unidade.
          </TextoEstilizado>
        )}
      </div>
      {uesSelecionadas
        ?.sort(FiltroHelper.ordenarLista('descricao'))
        .map(({ key, descricao }) => (
          <div
            className="col-md-12 d-flex justify-content-between mb-4 p-0"
            key={`${key}`}
          >
            <span>{descricao}</span>
            {listaUes?.length > 1 && (
              <BotaoEstilizado
                id="btn-excluir"
                icon="trash-alt"
                iconType="far"
                color={Colors.CinzaBotao}
                onClick={() => removerUES(key)}
                height="13px"
                width="13px"
                disabled={desabilitarBotaoExcluir}
              />
            )}
          </div>
        ))}
    </ModalConteudoHtml>
  );
};

ModalUE.defaultProps = {
  modalVisivel: false,
  permiteApenasUmaUe: false,
  setModalVisivel: () => {},
  setUnEscolaresSelecionados: () => {},
  unEscolaresSelecionados: [],
  setModoEdicaoItinerancia: () => {},
  desabilitarBotaoExcluir: false,
  temAlunosSelecionados: false,
};

ModalUE.propTypes = {
  modalVisivel: PropTypes.bool,
  permiteApenasUmaUe: PropTypes.bool,
  setModalVisivel: PropTypes.func,
  setUnEscolaresSelecionados: PropTypes.func,
  unEscolaresSelecionados: PropTypes.oneOfType([PropTypes.any]),
  setModoEdicaoItinerancia: PropTypes.func,
  desabilitarBotaoExcluir: PropTypes.bool,
  temAlunosSelecionados: PropTypes.bool,
};

export default ModalUE;
