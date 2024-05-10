import { Tooltip } from 'antd';
import PropTypes from 'prop-types';
import React, { useCallback, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import styled from 'styled-components';
import SelectComponent from '~/componentes/select';
import {
  setExpandirLinha,
  setNotaConceitoPosConselhoAtual,
} from '~/redux/modulos/conselhoClasse/actions';
import { erro } from '~/servicos/alertas';
import ServicoConselhoClasse from '~/servicos/Paginas/ConselhoClasse/ServicoConselhoClasse';
import { CampoAlerta, CampoCentralizado } from './campoNotaConceito.css';

export const Combo = styled.div`
  width: 105px;
  height: 35px;
  display: inline-block;
`;

const CampoConceito = props => {
  const {
    notaPosConselho,
    listaTiposConceitos,
    id,
    idCampo,
    codigoComponenteCurricular,
    alunoDesabilitado,
  } = props;

  const idCamposNotasPosConselho = useSelector(
    store => store.conselhoClasse.idCamposNotasPosConselho[idCampo]
  );

  const notaConceitoPosConselhoAtual = useSelector(
    store => store.conselhoClasse.notaConceitoPosConselhoAtual
  );

  const desabilitarCampos = useSelector(
    store => store.conselhoClasse.desabilitarCampos
  );

  const dentroPeriodo = useSelector(
    store => store.conselhoClasse.dentroPeriodo
  );

  const podeEditarNota = useSelector(
    store => store.conselhoClasse.podeEditarNota
  );

  const [notaValorAtual, setNotaValorAtual] = useState(notaPosConselho);
  const [abaixoDaMedia, setAbaixoDaMedia] = useState(false);

  const [idNotaPosConselho] = useState(id);

  const validaSeEstaAbaixoDaMedia = useCallback(
    valorAtual => {
      const tipoConceito = listaTiposConceitos.find(
        item => item.id === valorAtual
      );

      if (tipoConceito && !tipoConceito.aprovado) {
        setAbaixoDaMedia(true);
      } else {
        setAbaixoDaMedia(false);
      }
    },
    [listaTiposConceitos]
  );

  const dispatch = useDispatch();

  const mostrarJustificativa = () => {
    dispatch(setNotaConceitoPosConselhoAtual({}));
    const novaLinha = {};
    novaLinha[idCampo] = true;
    dispatch(setExpandirLinha(novaLinha));
  };

  const setNotaPosConselho = (
    conceito,
    ehEdicao,
    justificativa = null,
    auditoria = null
  ) => {
    dispatch(
      setNotaConceitoPosConselhoAtual({
        id: idNotaPosConselho || idCamposNotasPosConselho,
        codigoComponenteCurricular,
        conceito: conceito ? Number(conceito) : null,
        ehEdicao,
        justificativa,
        auditoria,
        idCampo,
      })
    );
  };

  const onClickMostrarJustificativa = async () => {
    mostrarJustificativa();
    const dados = await ServicoConselhoClasse.obterNotaPosConselho(
      idNotaPosConselho || idCamposNotasPosConselho
    ).catch(e => erro(e));
    if (dados && dados.data) {
      const { nota, justificativa } = dados.data;
      const auditoria = {
        criadoEm: dados.data.criadoEm,
        criadoPor: dados.data.criadoPor,
        criadoRf: dados.data.criadoRf,
        alteradoPor: dados.data.alteradoPor,
        alteradoEm: dados.data.alteradoEm,
        alteradoRf: dados.data.alteradoRf,
      };
      setNotaPosConselho(nota, false, justificativa, auditoria);
    }
  };

  const onChangeConceito = (valorNovo, validarMedia) => {
    if (
      notaConceitoPosConselhoAtual &&
      notaConceitoPosConselhoAtual.idCampo &&
      notaConceitoPosConselhoAtual.idCampo !== idCampo
    ) {
      return;
    }
    if (!desabilitarCampos) {
      setNotaValorAtual(valorNovo);
      mostrarJustificativa();
      setNotaPosConselho(valorNovo, true);
      if (validarMedia) {
        validaSeEstaAbaixoDaMedia(valorNovo);
      }
      setNotaValorAtual(valorNovo);
    }
  };

  const campoConceitoPosConselho = (abaixoMedia, validarMedia) => {
    return (
      <Combo>
        <SelectComponent
          onChange={valorNovo => onChangeConceito(valorNovo, validarMedia)}
          valueOption="id"
          valueText="valor"
          lista={listaTiposConceitos}
          valueSelect={notaValorAtual ? String(notaValorAtual) : ''}
          showSearch
          placeholder="Conceito"
          className={abaixoMedia ? 'borda-abaixo-media' : ''}
          disabled={
            (alunoDesabilitado && !podeEditarNota) ||
            !podeEditarNota ||
            desabilitarCampos ||
            !dentroPeriodo
          }
        />
      </Combo>
    );
  };

  return (
    <>
      {idNotaPosConselho || idCamposNotasPosConselho ? (
        <CampoCentralizado>
          <CampoAlerta>
            {campoConceitoPosConselho(false, false)}
            <Tooltip
              title="Ver Justificativa"
              placement="bottom"
              overlayStyle={{ fontSize: '12px' }}
            >
              <div className="icone" onClick={onClickMostrarJustificativa}>
                <i className="fas fa-user-edit" />
              </div>
            </Tooltip>
          </CampoAlerta>
        </CampoCentralizado>
      ) : (
        <Tooltip
          placement="bottom"
          title={abaixoDaMedia ? 'Abaixo da Média' : ''}
        >
          {campoConceitoPosConselho(abaixoDaMedia, true)}
        </Tooltip>
      )}
    </>
  );
};

CampoConceito.propTypes = {
  notaPosConselho: PropTypes.oneOfType([PropTypes.any]),
  listaTiposConceitos: PropTypes.oneOfType([PropTypes.array]),
  id: PropTypes.oneOfType([PropTypes.number]),
  idCampo: PropTypes.oneOfType([PropTypes.string]),
  codigoComponenteCurricular: PropTypes.oneOfType([PropTypes.any]),
  alunoDesabilitado: PropTypes.bool,
};

CampoConceito.defaultProps = {
  notaPosConselho: '',
  listaTiposConceitos: [],
  id: null,
  idCampo: '',
  codigoComponenteCurricular: '',
  alunoDesabilitado: false,
};

export default CampoConceito;
