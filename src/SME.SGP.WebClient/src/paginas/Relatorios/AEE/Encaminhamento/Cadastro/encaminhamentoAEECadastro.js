import PropTypes from 'prop-types';
import React, { useCallback, useEffect } from 'react';
import { useDispatch } from 'react-redux';
import { Card } from '~/componentes';
import { Cabecalho } from '~/componentes-sgp';
import {
  setDadosEncaminhamento,
  setDadosEstudanteObjectCardEncaminhamento,
  setDadosSecaoLocalizarEstudante,
  setExibirLoaderEncaminhamentoAEE,
} from '~/redux/modulos/encaminhamentoAEE/actions';
import { erros } from '~/servicos';
import ServicoEncaminhamentoAEE from '~/servicos/Paginas/Relatorios/AEE/ServicoEncaminhamentoAEE';
import BotoesAcoesEncaminhamentoAEE from './Componentes/botoesAcoesEncaminhamentoAEE';
import LoaderEncaminhamento from './Componentes/LoaderEncaminhamento/loaderEncaminhamento';
import SecaoEncaminhamentoCollapse from './Componentes/SecaoEncaminhamento/secaoEncaminhamentoCollapse';
import SecaoLocalizarEstudanteCollapse from './Componentes/SecaoLocalizarEstudante/secaoLocalizarEstudanteCollapse';

const EncaminhamentoAEECadastro = ({ match }) => {
  const dispatch = useDispatch();

  const obterEncaminhamentoPorId = useCallback(async () => {
    const encaminhamentoId = match?.params?.id;

    dispatch(setExibirLoaderEncaminhamentoAEE(true));
    const resultado = await ServicoEncaminhamentoAEE.obterEncaminhamentoPorId(
      encaminhamentoId
    )
      .catch(e => erros(e))
      .finally(() => dispatch(setExibirLoaderEncaminhamentoAEE(false)));

    if (resultado?.data) {
      const { aluno, turma } = resultado?.data;

      const dadosObjectCard = {
        nome: aluno.nome,
        numeroChamada: aluno.numeroAlunoChamada,
        dataNascimento: aluno.dataNascimento,
        codigoEOL: aluno.codigoAluno,
        situacao: aluno.situacao,
        dataSituacao: aluno.dataSituacao,
      };
      dispatch(setDadosEstudanteObjectCardEncaminhamento(dadosObjectCard));

      const dadosSecaoLocalizarEstudante = {
        anoLetivo: turma.anoLetivo,
        codigoAluno: aluno.codigoAluno,
        codigoTurma: turma.codigo,
        turmaId: turma.id,
      };
      dispatch(setDadosSecaoLocalizarEstudante(dadosSecaoLocalizarEstudante));

      dispatch(setDadosEncaminhamento(resultado?.data));
    }
  }, [match, dispatch]);

  useEffect(() => {
    const encaminhamentoId = match?.params?.id;
    if (encaminhamentoId) {
      obterEncaminhamentoPorId();
    }
  }, [match, obterEncaminhamentoPorId, dispatch]);

  return (
    <LoaderEncaminhamento>
      <Cabecalho pagina="Encaminhamento AEE" />
      <Card>
        <div className="col-md-12">
          <div className="row">
            <div className="col-md-12 d-flex justify-content-end pb-4">
              <BotoesAcoesEncaminhamentoAEE match={match} />
            </div>
            {match?.params?.id ? (
              ''
            ) : (
              <div className="col-md-12 mb-2">
                <SecaoLocalizarEstudanteCollapse />
              </div>
            )}
            <div className="col-md-12 mb-2">
              <SecaoEncaminhamentoCollapse match={match} />
            </div>
          </div>
        </div>
      </Card>
    </LoaderEncaminhamento>
  );
};

EncaminhamentoAEECadastro.propTypes = {
  match: PropTypes.oneOfType([PropTypes.object]),
};

EncaminhamentoAEECadastro.defaultProps = {
  match: {},
};

export default EncaminhamentoAEECadastro;
