import React, { useCallback, useEffect, useMemo, useState } from 'react';
import shortid from 'shortid';

import { useDispatch, useSelector } from 'react-redux';
import {
  CardCollapse,
  Auditoria,
  CampoData,
  Editor,
  Loader,
} from '~/componentes';

import { verificaSomenteConsulta } from '~/servicos/servico-navegacao';

import { CONFIG_COLLAPSE_REGISTRO_INDIVIDUAL } from '~/constantes';
import RotasDto from '~/dtos/rotasDto';

import {
  setAuditoriaNovoRegistro,
  setDesabilitarCampos,
  setRegistroIndividualEmEdicao,
  setDadosParaSalvarNovoRegistro,
  resetDataNovoRegistro,
} from '~/redux/modulos/registroIndividual/actions';

import { erros, ServicoRegistroIndividual } from '~/servicos';

const NovoRegistroIndividual = () => {
  const [expandir, setExpandir] = useState(false);
  const [exibirCollapse, setExibirCollapse] = useState(false);
  const [data, setData] = useState();
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
  const dadosPrincipaisRegistroIndividual = useSelector(
    store => store.registroIndividual.dadosPrincipaisRegistroIndividual
  );
  const dadosParaSalvarNovoRegistro = useSelector(
    store => store.registroIndividual.dadosParaSalvarNovoRegistro
  );
  const resetDataNovoRegistroIndividual = useSelector(
    store => store.registroIndividual.resetDataNovoRegistroIndividual
  );

  const { permissoes, turmaSelecionada } = useSelector(state => state.usuario);
  const permissoesTela = permissoes[RotasDto.RELATORIO_SEMESTRAL];
  const turmaId = turmaSelecionada?.id || 0;
  const alunoCodigo = dadosAlunoObjectCard?.codigoEOL;
  const dataAtual = window.moment();

  const ehMesmoAluno = useMemo(
    () =>
      String(alunoCodigo) === String(dadosParaSalvarNovoRegistro?.alunoCodigo),
    [alunoCodigo, dadosParaSalvarNovoRegistro]
  );
  const registro = useMemo(
    () => (ehMesmoAluno ? dadosParaSalvarNovoRegistro?.registro : ''),
    [dadosParaSalvarNovoRegistro, ehMesmoAluno]
  );
  const idSecao = useMemo(
    () => (ehMesmoAluno ? dadosParaSalvarNovoRegistro?.id : ''),
    [dadosParaSalvarNovoRegistro, ehMesmoAluno]
  );
  const auditoria = useMemo(
    () => (ehMesmoAluno ? auditoriaNovoRegistroIndividual : null),
    [auditoriaNovoRegistroIndividual, ehMesmoAluno]
  );

  const dispatch = useDispatch();

  useEffect(() => {
    const temDadosRegistros = Object.keys(dadosPrincipaisRegistroIndividual)
      .length;
    if (temDadosRegistros) {
      setExpandir(true);
      setExibirCollapse(
        dadosPrincipaisRegistroIndividual?.podeRealizarNovoRegistro
      );
    }
  }, [setExibirCollapse, dadosPrincipaisRegistroIndividual]);

  const validaPermissoes = useCallback(
    temDadosNovosRegistros => {
      const novoRegistro = verificaSomenteConsulta(permissoesTela);

      const desabilitar = novoRegistro || temDadosNovosRegistros;

      dispatch(setDesabilitarCampos(!!desabilitar));
    },
    [dispatch, permissoesTela]
  );

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
          setDadosParaSalvarNovoRegistro({
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

    if (
      temDadosAlunos &&
      dadosPrincipaisRegistroIndividual?.podeRealizarNovoRegistro &&
      dataEscolhida
    ) {
      dispatch(setAuditoriaNovoRegistro(null));
      dispatch(setDadosParaSalvarNovoRegistro({}));
      obterRegistroIndividualPorData(dataEscolhida);
    }
  }, [
    dispatch,
    dadosAlunoObjectCard,
    dadosPrincipaisRegistroIndividual,
    data,
    obterRegistroIndividualPorData,
  ]);

  useEffect(() => {
    if (!data) {
      setData(dataAtual);
    }
    if (resetDataNovoRegistroIndividual) {
      dispatch(resetDataNovoRegistro(false));
      setData(dataAtual);
    }
  }, [data, dispatch, dataAtual, resetDataNovoRegistroIndividual]);

  useEffect(() => {
    const temDadosNovosRegistros = Object.keys(dadosParaSalvarNovoRegistro)
      .length;
    if (temDadosNovosRegistros) {
      validaPermissoes(temDadosNovosRegistros);
    }
  }, [validaPermissoes, dadosParaSalvarNovoRegistro]);

  console.log(
    'dadosParaSalvarNovoRegistro ========> ',
    dadosParaSalvarNovoRegistro
  );

  return (
    <>
      {exibirCollapse && (
        <div key={shortid.generate()} className="px-4 pt-4">
          <CardCollapse
            configCabecalho={CONFIG_COLLAPSE_REGISTRO_INDIVIDUAL}
            styleCardBody={{ paddingTop: 12 }}
            key={`${idSecao}-collapse-key`}
            titulo="Novo registro individual"
            indice={`${idSecao}-collapse-indice`}
            alt={`${idSecao}-alt`}
            show={expandir}
            onClick={() => setExpandir(!expandir)}
          >
            <div className="col-3 p-0 pb-2">
              <CampoData
                name="data"
                placeholder="Selecione"
                valor={data}
                formatoData="DD/MM/YYYY"
                onChange={valor => setData(valor)}
              />
            </div>
            <div className="pt-1">
              <Loader ignorarTip loading={carregandoNovoRegistro}>
                <Editor
                  validarSeTemErro={validarSeTemErro}
                  mensagemErro="Campo obrigatório"
                  id={`secao-${idSecao}-editor`}
                  inicial={registro}
                  onChange={mudarEditor}
                  desabilitar={desabilitarNovoRegistro}
                />
              </Loader>
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
            </div>
          </CardCollapse>
        </div>
      )}
    </>
  );
};

export default NovoRegistroIndividual;
