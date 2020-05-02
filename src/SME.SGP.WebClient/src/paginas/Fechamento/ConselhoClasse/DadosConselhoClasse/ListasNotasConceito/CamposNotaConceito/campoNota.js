import { Tooltip } from 'antd';
import PropTypes from 'prop-types';
import React, { useCallback, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import CampoNumero from '~/componentes/campoNumero';
import {
  setExpandirLinha,
  setNotaConceitoPosConselhoAtual,
} from '~/redux/modulos/conselhoClasse/actions';
import { erro, erros } from '~/servicos/alertas';
import ServicoConselhoClasse from '~/servicos/Paginas/ConselhoClasse/ServicoConselhoClasse';
import ServicoNotaConceito from '~/servicos/Paginas/DiarioClasse/ServicoNotaConceito';
import { CampoAlerta, CampoCentralizado } from './campoNotaConceito.css';

const CampoNota = props => {
  const {
    id,
    notaPosConselho,
    idCampo,
    codigoComponenteCurricular,
    mediaAprovacao,
  } = props;

  const fechamentoPeriodoInicioFim = useSelector(
    store => store.conselhoClasse.fechamentoPeriodoInicioFim
  );

  const idCamposNotasPosConselho = useSelector(
    store => store.conselhoClasse.idCamposNotasPosConselho[idCampo]
  );

  const { periodoFechamentoFim } = fechamentoPeriodoInicioFim;

  const [notaValorAtual, setNotaValorAtual] = useState(notaPosConselho);
  const [abaixoDaMedia, setAbaixoDaMedia] = useState(false);

  const [idNotaPosConselho] = useState(id);

  const dispatch = useDispatch();

  const mostrarJustificativa = () => {
    dispatch(setNotaConceitoPosConselhoAtual({}));
    const novaLinha = {};
    novaLinha[idCampo] = true;
    dispatch(setExpandirLinha(novaLinha));
  };

  const setNotaPosConselho = (
    nota,
    ehEdicao,
    justificativa = null,
    auditoria = null
  ) => {
    dispatch(
      setNotaConceitoPosConselhoAtual({
        id: idNotaPosConselho || idCamposNotasPosConselho,
        codigoComponenteCurricular,
        nota,
        ehEdicao,
        justificativa,
        auditoria,
        idCampo,
      })
    );
  };

  const removerCaracteresInvalidos = texto => {
    return texto.replace(/[^0-9,.]+/g, '');
  };

  const validaSeEstaAbaixoDaMedia = useCallback(
    valor => {
      const valorAtual = removerCaracteresInvalidos(String(valor));
      if (valorAtual && valorAtual < mediaAprovacao) {
        setAbaixoDaMedia(true);
      } else {
        setAbaixoDaMedia(false);
      }
    },
    [mediaAprovacao]
  );

  const onChangeValor = async (valor, validarMedia) => {
    setNotaValorAtual(valor);
    const retorno = await ServicoNotaConceito.obterArredondamento(
      Number(valor),
      periodoFechamentoFim
    ).catch(e => erros(e));

    let notaArredondada = valor;
    if (retorno && retorno.data) {
      notaArredondada = retorno.data;
    }

    if (validarMedia) {
      validaSeEstaAbaixoDaMedia(notaArredondada);
    }
    mostrarJustificativa();
    setNotaPosConselho(valor, true);
    setNotaValorAtual(notaArredondada);
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

  const valorInvalido = valorNovo => {
    const regexValorInvalido = /[^0-9,.]+/g;
    return regexValorInvalido.test(String(valorNovo));
  };

  const editouCampo = (original, nova) => {
    const novaNotaOriginal = removerCaracteresInvalidos(String(original));
    const novaNota = removerCaracteresInvalidos(String(nova));
    if (novaNotaOriginal === '' && novaNota === '') {
      return false;
    }
    return novaNotaOriginal !== novaNota;
  };

  const campoNotaPosConselho = (abaixoMedia, validarMedia) => {
    return (
      <CampoNumero
        onChange={valorNovo => {
          const invalido = valorInvalido(valorNovo);
          if (!invalido && editouCampo(notaValorAtual, valorNovo)) {
            onChangeValor(valorNovo, validarMedia);
          }
        }}
        value={notaValorAtual}
        min={0}
        max={10}
        step={0.5}
        className={abaixoMedia ? 'borda-abaixo-media' : ''}
      />
    );
  };

  return (
    <>
      <CampoCentralizado>
        {idNotaPosConselho || idCamposNotasPosConselho ? (
          <CampoAlerta ehNota>
            {campoNotaPosConselho(false, false)}
            <div className="icone" onClick={onClickMostrarJustificativa}>
              <Tooltip
                title="Ver Justificativa"
                placement="bottom"
                overlayStyle={{ fontSize: '12px' }}
              >
                <i className="fas fa-user-edit" />
              </Tooltip>
            </div>
          </CampoAlerta>
        ) : (
          <Tooltip
            placement="bottom"
            title={abaixoDaMedia ? 'Abaixo da Média' : ''}
          >
            <CampoCentralizado>
              {campoNotaPosConselho(abaixoDaMedia, true)}
            </CampoCentralizado>
          </Tooltip>
        )}
      </CampoCentralizado>
    </>
  );
};

CampoNota.propTypes = {
  id: PropTypes.oneOfType([PropTypes.any]),
  notaPosConselho: PropTypes.oneOfType([PropTypes.any]),
  idCampo: PropTypes.oneOfType([PropTypes.string]),
  codigoComponenteCurricular: PropTypes.oneOfType([PropTypes.any]),
  mediaAprovacao: PropTypes.number,
};

CampoNota.defaultProps = {
  id: 0,
  notaPosConselho: '',
  idCampo: '',
  codigoComponenteCurricular: '',
  mediaAprovacao: 5,
};

export default CampoNota;
