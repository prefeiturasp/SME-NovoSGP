import PropTypes from 'prop-types';
import React, { useCallback, useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { Loader } from '~/componentes';
import {
  setAuditoriaRelatorioSemestral,
  setAvancos,
  setDadosRelatorioSemestral,
  setDificuldades,
  setEncaminhamentos,
  setHistoricoEstudante,
  setOutros,
  setRelatorioSemestralEmEdicao,
  setDesabilitarCampos,
} from '~/redux/modulos/relatorioSemestral/actions';
import { erros } from '~/servicos/alertas';
import ServicoRelatorioSemestral from '~/servicos/Paginas/Relatorios/PAP/ServicoRelatorioSemestral/ServicoRelatorioSemestral';
import AuditoriaRelatorioSemestral from './AuditoriaRelatorioSemestral/auditoriaRelatorioSemestral';
import Avancos from './CamposDescritivos/Avancos/avancos';
import Dificuldades from './CamposDescritivos/Dificuldades/dificuldades';
import Encaminhamentos from './CamposDescritivos/Encaminhamentos/encaminhamentos';
import HistoricoEstudante from './CamposDescritivos/HistoricoEstudante/historicoEstudante';
import Outros from './CamposDescritivos/Outros/outros';
import { verificaSomenteConsulta } from '~/servicos/servico-navegacao';

const DadosRelatorioSemestral = props => {
  const { codigoTurma } = props;

  // const usuario = useSelector(store => store.usuario);
  // const permissoesTela = usuario.permissoes[RotasDto.RELATORIO_SEMESTRAL];
  // TODO Não tem permissao ainda, descomentar linhas acima quando tiver!

  const dadosAlunoObjectCard = useSelector(
    store => store.relatorioSemestral.dadosAlunoObjectCard
  );

  const { codigoEOL, desabilitado } = dadosAlunoObjectCard;

  const dispatch = useDispatch();

  const [dadosIniciais, setDadosIniciais] = useState({
    historicoEstudante: '',
    dificuldades: '',
    encaminhamentos: '',
    avancos: '',
    outros: '',
  });

  const [exibir, setExibir] = useState(true);
  const [carregando, setCarregando] = useState(false);

  const validaPermissoes = useCallback(
    novoRegistro => {
      // TODO Não tem permissao ainda, quando tiver remover valores fixos!
      const permissoesTela = {
        podeAlterar: false,
        podeConsultar: true,
        podeExcluir: false,
        podeIncluir: true,
      };

      const somenteConsulta = verificaSomenteConsulta(permissoesTela);

      const desabilitar = novoRegistro
        ? somenteConsulta || !permissoesTela.podeIncluir
        : somenteConsulta || !permissoesTela.podeAlterar;

      dispatch(setDesabilitarCampos(desabilitar));
    },
    [dispatch]
  );

  const onChangeCampos = useCallback(
    (valor, campo) => {
      const dadosDto = dadosIniciais;
      dadosDto[campo] = valor;
      setDadosIniciais(dadosDto);
    },
    [dadosIniciais]
  );

  const onChangeHistoricoEstudante = useCallback(
    valor => {
      dispatch(setHistoricoEstudante(valor));
      onChangeCampos(valor, 'historicoEstudante');
    },
    [dispatch, onChangeCampos]
  );

  const onChangeDificuldades = useCallback(
    valor => {
      dispatch(setDificuldades(valor));
      onChangeCampos(valor, 'dificuldades');
    },
    [dispatch, onChangeCampos]
  );

  const onChangeEncaminhamentos = useCallback(
    valor => {
      dispatch(setEncaminhamentos(valor));
      onChangeCampos(valor, 'encaminhamentos');
    },
    [dispatch, onChangeCampos]
  );

  const onChangeAvancos = useCallback(
    valor => {
      dispatch(setAvancos(valor));
      onChangeCampos(valor, 'avancos');
    },
    [dispatch, onChangeCampos]
  );

  const onChangeOutros = useCallback(
    valor => {
      dispatch(setOutros(valor));
      onChangeCampos(valor, 'outros');
    },
    [dispatch, onChangeCampos]
  );

  const setarDados = useCallback(
    dados => {
      const novoRegistro = !dados.id;
      validaPermissoes(novoRegistro);
      // TODO Setar os ids e dados importantes!
      const valores = {
        id: dados.id,
      };
      dispatch(setDadosRelatorioSemestral(valores));
    },
    [dispatch, validaPermissoes]
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

  const obterDadosCamposDescritivos = useCallback(
    async (codigoAluno, turma) => {
      setCarregando(true);

      // TODO Revisar consulta!
      const resposta = await ServicoRelatorioSemestral.obterDadosCamposDescritivos(
        turma,
        codigoAluno
      ).catch(e => erros(e));

      if (resposta && resposta.data) {
        onChangeHistoricoEstudante(resposta.data.historicoEstudante);
        setarDados(resposta.data);
        setarAuditoria(resposta.data);
        setExibir(true);
      } else {
        setExibir(false);
      }
      setCarregando(false);
    },
    [onChangeHistoricoEstudante, setarDados, setarAuditoria]
  );

  useEffect(() => {
    // TODO Revisar!
    if (codigoTurma && codigoEOL) {
      obterDadosCamposDescritivos(codigoEOL, codigoTurma);
    }
  }, [codigoTurma, codigoEOL, obterDadosCamposDescritivos]);

  const setarRelatorioSemestralEmEdicao = emEdicao => {
    dispatch(setRelatorioSemestralEmEdicao(emEdicao));
  };

  return (
    <Loader className={carregando ? 'text-center' : ''} loading={carregando}>
      {exibir ? (
        <>
          <HistoricoEstudante
            alunoDesabilitado={desabilitado}
            onChange={valor => {
              onChangeHistoricoEstudante(valor);
              setarRelatorioSemestralEmEdicao(true);
            }}
            dadosIniciais={dadosIniciais}
          />
          <Dificuldades
            alunoDesabilitado={desabilitado}
            onChange={valor => {
              onChangeDificuldades(valor);
              setarRelatorioSemestralEmEdicao(true);
            }}
            dadosIniciais={dadosIniciais}
          />
          <Encaminhamentos
            alunoDesabilitado={desabilitado}
            onChange={valor => {
              onChangeEncaminhamentos(valor);
              setarRelatorioSemestralEmEdicao(true);
            }}
            dadosIniciais={dadosIniciais}
          />
          <Avancos
            alunoDesabilitado={desabilitado}
            onChange={valor => {
              onChangeAvancos(valor);
              setarRelatorioSemestralEmEdicao(true);
            }}
            dadosIniciais={dadosIniciais}
          />
          <Outros
            alunoDesabilitado={desabilitado}
            onChange={valor => {
              onChangeOutros(valor);
              setarRelatorioSemestralEmEdicao(true);
            }}
            dadosIniciais={dadosIniciais}
          />

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
};

DadosRelatorioSemestral.defaultProps = {
  codigoTurma: '',
};

export default DadosRelatorioSemestral;
