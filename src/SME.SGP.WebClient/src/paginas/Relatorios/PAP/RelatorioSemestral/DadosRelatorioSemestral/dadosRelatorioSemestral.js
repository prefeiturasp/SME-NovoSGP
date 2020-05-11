import PropTypes from 'prop-types';
import React, { useCallback, useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { Loader } from '~/componentes';
import {
  setAuditoriaRelatorioSemestral,
  setDadosRelatorioSemestral,
} from '~/redux/modulos/relatorioSemestral/actions';
import { erros } from '~/servicos/alertas';
import ServicoRelatorioSemestral from '~/servicos/Paginas/Relatorios/PAP/RelatorioSemestral/ServicoRelatorioSemestral';
import AuditoriaRelatorioSemestral from './AuditoriaRelatorioSemestral/auditoriaRelatorioSemestral';
import MontarCamposRelatorioSemestral from './CamposRelatorioSemestral/montarCamposRelatorioSemestral';

const DadosRelatorioSemestral = props => {
  const { codigoTurma, semestreConsulta } = props;

  const dadosAlunoObjectCard = useSelector(
    store => store.relatorioSemestral.dadosAlunoObjectCard
  );

  const { codigoEOL } = dadosAlunoObjectCard;

  const dispatch = useDispatch();

  const [exibir, setExibir] = useState(false);
  const [carregando, setCarregando] = useState(false);

  const setarDados = useCallback(
    dados => {
      const novosDados = dados;
      novosDados.turmaCodigo = codigoTurma;
      novosDados.semestreConsulta = semestreConsulta;
      novosDados.alunoCodigo = codigoEOL;
      dispatch(setDadosRelatorioSemestral(dados));
    },
    [codigoEOL, codigoTurma, dispatch, semestreConsulta]
  );

  const setarAuditoria = useCallback(
    dados => {
      const { auditoria } = dados;
      if (auditoria) {
        const auditoriaDto = {
          criadoEm: auditoria.criadoEm,
          criadoPor: auditoria.criadoPor,
          criadoRF: auditoria.criadoRF,
          alteradoEm: auditoria.alteradoEm,
          alteradoPor: auditoria.alteradoPor,
          alteradoRF: auditoria.alteradoRF,
        };
        dispatch(setAuditoriaRelatorioSemestral(auditoriaDto));
      }
    },
    [dispatch]
  );

  const obterDadosCamposDescritivos = useCallback(async () => {
    setCarregando(true);

    const resposta = await ServicoRelatorioSemestral.obterDadosCamposDescritivos(
      codigoEOL,
      codigoTurma,
      semestreConsulta
    ).catch(e => erros(e));

    if (resposta && resposta.data) {
      setarDados(resposta.data);
      setarAuditoria(resposta.data);
      setExibir(true);
    } else {
      setExibir(false);
    }
    setCarregando(false);
  }, [codigoEOL, codigoTurma, semestreConsulta, setarAuditoria, setarDados]);

  useEffect(() => {
    if (codigoTurma && codigoEOL && semestreConsulta) {
      obterDadosCamposDescritivos();
    }
  }, [codigoTurma, codigoEOL, semestreConsulta, obterDadosCamposDescritivos]);

  return (
    <Loader className={carregando ? 'text-center' : ''} loading={carregando}>
      {exibir ? (
        <>
          <MontarCamposRelatorioSemestral />
          <AuditoriaRelatorioSemestral />
        </>
      ) : (
        ''
      )}
    </Loader>
  );
};

DadosRelatorioSemestral.propTypes = {
  codigoTurma: PropTypes.string,
  semestreConsulta: PropTypes.oneOfType([PropTypes.any]),
};

DadosRelatorioSemestral.defaultProps = {
  codigoTurma: '',
  semestreConsulta: '',
};

export default DadosRelatorioSemestral;
