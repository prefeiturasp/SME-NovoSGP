import React, { useCallback, useEffect, useMemo, useState } from 'react';
import shortid from 'shortid';

import { useDispatch, useSelector } from 'react-redux';
import {
  CardCollapse,
  Auditoria,
  CampoData,
  JoditEditor,
  Loader,
} from '~/componentes';

import { CONFIG_COLLAPSE_REGISTRO_INDIVIDUAL } from '~/constantes';
import RotasDto from '~/dtos/rotasDto';

import {
  resetDataNovoRegistro,
  setAuditoriaNovoRegistro,
  setDadosParaSalvarNovoRegistro,
  setDadosRegistroAtual,
  setDesabilitarCampos,
  setRegistroIndividualEmEdicao,
} from '~/redux/modulos/registroIndividual/actions';

import {
  erros,
  ServicoRegistroIndividual,
  verificaSomenteConsulta,
} from '~/servicos';

import SugestaoTopico from '../SugestaoTopico/sugestaoTopico';

const NovoRegistroIndividual = () => {
  const dataAtual = window.moment();
  const [expandir, setExpandir] = useState(false);
  const [exibirCollapse, setExibirCollapse] = useState(false);
  const [data, setData] = useState(dataAtual);
  const [desabilitarNovoRegistro, setDesabilitarNovoRegistro] = useState(false);
  const [carregandoNovoRegistro, setCarregandoNovoRegistro] = useState(false);

  const auditoriaNovoRegistroIndividual = useSelector(
    store => store.registroIndividual.auditoriaNovoRegistroIndividual
  );
  const componenteCurricularSelecionado = useSelector(
    store => store.registroIndividual.componenteCurricularSelecionado
  );
  const dadosAlunoObjectCard = useSelector(
    store => store.registroIndividual.dadosAlunoObjectCard
  );
  const podeRealizarNovoRegistro = useSelector(
    store => store.registroIndividual.podeRealizarNovoRegistro
  );
  const resetDataNovoRegistroIndividual = useSelector(
    store => store.registroIndividual.resetDataNovoRegistroIndividual
  );
  const dadosRegistroAtual = useSelector(
    store => store.registroIndividual.dadosRegistroAtual
  );

  const turmaSelecionada = useSelector(state => state.usuario.turmaSelecionada);
  const permissoes = useSelector(state => state.usuario.permissoes);
  const permissoesTela = permissoes[RotasDto.REGISTRO_INDIVIDUAL];

  const turmaId = turmaSelecionada?.id || 0;
  const alunoCodigo = dadosAlunoObjectCard?.codigoEOL;

  const ehMesmoAluno = useMemo(
    () => String(alunoCodigo) === String(dadosRegistroAtual?.alunoCodigo),
    [alunoCodigo, dadosRegistroAtual]
  );
  const registro = useMemo(
    () => (ehMesmoAluno ? dadosRegistroAtual?.registro : ''),
    [dadosRegistroAtual, ehMesmoAluno]
  );
  const idSecao = useMemo(() => (ehMesmoAluno ? dadosRegistroAtual?.id : ''), [
    dadosRegistroAtual,
    ehMesmoAluno,
  ]);
  const auditoria = useMemo(
    () => (ehMesmoAluno ? auditoriaNovoRegistroIndividual : null),
    [auditoriaNovoRegistroIndividual, ehMesmoAluno]
  );

  const dispatch = useDispatch();

  useEffect(() => {
    if (podeRealizarNovoRegistro && dadosAlunoObjectCard) {
      setExpandir(true);
      setExibirCollapse(podeRealizarNovoRegistro);
      return;
    }
    setExibirCollapse(false);
  }, [podeRealizarNovoRegistro, setExibirCollapse, dadosAlunoObjectCard]);

  const validaPermissoes = useCallback(() => {
    const novoRegistro = verificaSomenteConsulta(permissoesTela);

    const desabilitar = novoRegistro || !podeRealizarNovoRegistro;

    dispatch(setDesabilitarCampos(!!desabilitar));
  }, [dispatch, permissoesTela, podeRealizarNovoRegistro]);

  const mudarEditor = useCallback(
    novoRegistro => {
      dispatch(
        setDadosParaSalvarNovoRegistro({
          id: idSecao,
          registro: novoRegistro,
          data: data.set({ hour: 0, minute: 0, second: 0 }),
          alunoCodigo,
        })
      );
      dispatch(setRegistroIndividualEmEdicao(true));
      dispatch(setDesabilitarCampos(true));
    },
    [alunoCodigo, data, dispatch, idSecao]
  );

  const validarSeTemErro = valorEditado => {
    return !valorEditado;
  };

  const obterRegistroIndividualPorData = useCallback(
    async dataEscolhida => {
      setCarregandoNovoRegistro(true);
      const retorno = await ServicoRegistroIndividual.obterRegistroIndividualPorData(
        {
          alunoCodigo,
          componenteCurricular: componenteCurricularSelecionado,
          data: dataEscolhida,
          turmaId,
        }
      )
        .catch(e => erros(e))
        .finally(() => setCarregandoNovoRegistro(false));

      const resposta = retorno?.data;
      const ehMesmoCodigo =
        String(resposta?.alunoCodigo) === String(alunoCodigo);
      const dataAtualFormatada = window.moment().format('YYYY-MM-DD');
      const ehDataAnterior = window
        .moment(dataAtualFormatada)
        .isAfter(data.format('YYYY-MM-DD'));

      setDesabilitarNovoRegistro(false);
      if (resposta && ehMesmoCodigo) {
        if (ehDataAnterior) {
          setDesabilitarNovoRegistro(true);
          return;
        }

        dispatch(
          setDadosRegistroAtual({
            id: resposta?.id,
            registro: resposta?.registro,
            data: resposta?.data,
            alunoCodigo: resposta?.alunoCodigo,
          })
        );
        dispatch(setAuditoriaNovoRegistro(resposta?.auditoria));
      }
    },
    [componenteCurricularSelecionado, data, dispatch, alunoCodigo, turmaId]
  );

  useEffect(() => {
    const dataEscolhida = data && data.format('MM-DD-YYYY');
    const temDadosAlunos = Object.keys(dadosAlunoObjectCard).length;

    if (temDadosAlunos && podeRealizarNovoRegistro && dataEscolhida) {
      dispatch(setAuditoriaNovoRegistro(null));
      dispatch(setDadosParaSalvarNovoRegistro({}));
      dispatch(setDadosRegistroAtual({}));
      obterRegistroIndividualPorData(dataEscolhida);
    }
  }, [
    dadosAlunoObjectCard,
    data,
    dispatch,
    obterRegistroIndividualPorData,
    podeRealizarNovoRegistro,
  ]);

  useEffect(() => {
    if (resetDataNovoRegistroIndividual) {
      dispatch(resetDataNovoRegistro(false));
      setData(dataAtual);
    }
  }, [data, dispatch, dataAtual, resetDataNovoRegistroIndividual]);

  useEffect(() => {
    if (podeRealizarNovoRegistro) {
      validaPermissoes();
    }
  }, [validaPermissoes, podeRealizarNovoRegistro]);

  const desabilitarData = dataCorrente => {
    const anoLetivo = turmaSelecionada?.anoLetivo;
    return (
      dataCorrente &&
      (dataCorrente < window.moment(`${anoLetivo}-01-01`) ||
        dataCorrente > window.moment())
    );
  };

  const expandirAlternado = useCallback(() => setExpandir(!expandir), [
    expandir,
  ]);

  const resetarDados = () => {
    dispatch(setRegistroIndividualEmEdicao(false));
    dispatch(setDesabilitarCampos(false));
  };

  const mudarData = valor => {
    if (valor) {
      setData(valor);
      resetarDados();
    }
  };

  useEffect(() => {
    const elementos = document.getElementsByClassName(
      'ant-calendar-picker-clear'
    );
    const setarDataVazia = () => {
      setData('');
    };

    if (elementos.length) {
      elementos[0].addEventListener('click', setarDataVazia, false);
    }
    return () => {
      if (elementos.length) {
        elementos[0].removeEventListener('click', setarDataVazia);
      }
    };
  });

  return (
    <>
      {exibirCollapse && !dadosAlunoObjectCard.desabilitado && (
        <div key={shortid.generate()} className="px-4 pt-4">
          <CardCollapse
            configCabecalho={CONFIG_COLLAPSE_REGISTRO_INDIVIDUAL}
            styleCardBody={{ paddingTop: 12 }}
            key={`${idSecao}-collapse-key`}
            titulo="Novo registro individual"
            indice={`${idSecao}-collapse-indice`}
            alt={`${idSecao}-alt`}
            show={expandir}
            onClick={expandirAlternado}
          >
            <div className="col-3 p-0 pb-2">
              <CampoData
                name="data"
                placeholder="Selecione"
                valor={data}
                formatoData="DD/MM/YYYY"
                onChange={mudarData}
                desabilitarData={desabilitarData}
              />
            </div>
            {permissoesTela.podeIncluir && (
              <div className="col-12 p-0 pb-3">
                <SugestaoTopico valorData={data} />
              </div>
            )}
            <div className="pt-2">
              <Loader ignorarTip loading={carregandoNovoRegistro}>
                <div style={{ minHeight: 200 }}>
                  <JoditEditor
                    validarSeTemErro={validarSeTemErro}
                    mensagemErro="Campo obrigatório"
                    id={`secao-${idSecao}-editor`}
                    value={registro}
                    onChange={mudarEditor}
                    desabilitar={
                      desabilitarNovoRegistro || !permissoesTela.podeIncluir
                    }
                  />
                </div>
                {auditoria && (
                  <div className="mt-1 ml-n3">
                    <Auditoria
                      ignorarMarginTop
                      criadoEm={auditoria.criadoEm}
                      criadoPor={auditoria.criadoPor}
                      criadoRf={auditoria.criadoRF}
                      alteradoPor={auditoria.alteradoPor}
                      alteradoEm={auditoria.alteradoEm}
                      alteradoRf={auditoria.alteradoRF}
                    />
                  </div>
                )}
              </Loader>
            </div>
          </CardCollapse>
        </div>
      )}
    </>
  );
};

export default NovoRegistroIndividual;
