import React, { useCallback, useEffect } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import Cabecalho from '~/componentes-sgp/cabecalho';
import Alert from '~/componentes/alert';
import Card from '~/componentes/card';
import { RotasDto } from '~/dtos';
import { salvarDadosAulaFrequencia } from '~/redux/modulos/calendarioProfessor/actions';
import {
  limparDadosFrequenciaPlanoAula,
  setComponenteCurricularFrequenciaPlanoAula,
  setLimparDadosPlanoAula,
  setSomenteConsultaFrequenciaPlanoAula,
} from '~/redux/modulos/frequenciaPlanoAula/actions';
import { obterDescricaoNomeMenu, verificaSomenteConsulta } from '~/servicos';
import { ehTurmaInfantil } from '~/servicos/Validacoes/validacoesInfatil';
import BotoesAcoesFrequenciaPlanoAula from './DadosFrequenciaPlanoAula/BotoesAcoes/botoesAcoesFrequenciaPlanoAula';
import CamposFiltrarDadosFrequenciaPlanoAula from './DadosFrequenciaPlanoAula/CamposFiltrarDadosFrequenciaPlanoAula/camposFiltrarDadosFrequenciaPlanoAula';
import MontarListaFrequencia from './DadosFrequenciaPlanoAula/Frequencia/montarListaFrequencia';
import LoaderFrequenciaPlanoAula from './DadosFrequenciaPlanoAula/LoaderFrequenciaPlanoAula/loaderFrequenciaPlanoAula';
import AlertaDentroPeriodoFrequenciaPlanoAula from './DadosFrequenciaPlanoAula/PlanoAula/DadosPlanoAula/AlertaDentroPeriodo/alertaDentroPeriodoFrequenciaPlanoAula';
import PlanoAula from './DadosFrequenciaPlanoAula/PlanoAula/planoAula';

const FrequenciaPlanoAula = () => {
  const dispatch = useDispatch();

  const usuario = useSelector(state => state.usuario);

  const { turmaSelecionada } = usuario;

  const modalidadesFiltroPrincipal = useSelector(
    state => state.filtro.modalidades
  );

  const permissoesTela = usuario.permissoes[RotasDto.FREQUENCIA_PLANO_AULA];

  const resetarInfomacoes = useCallback(() => {
    dispatch(limparDadosFrequenciaPlanoAula());
  }, [dispatch]);

  useEffect(() => {
    if (ehTurmaInfantil(modalidadesFiltroPrincipal, turmaSelecionada)) {
      dispatch(setLimparDadosPlanoAula());
    }
  }, [dispatch, modalidadesFiltroPrincipal, turmaSelecionada]);

  const validaSomenteConsulta = useCallback(() => {
    const soConsulta = verificaSomenteConsulta(permissoesTela);
    dispatch(setSomenteConsultaFrequenciaPlanoAula(soConsulta));
  }, [dispatch, permissoesTela]);

  useEffect(() => {
    resetarInfomacoes();
    validaSomenteConsulta();
    dispatch(setComponenteCurricularFrequenciaPlanoAula());
    return () => {
      // Quando sair da tela vai executar para limpar os dados no redux!
      resetarInfomacoes();
      dispatch(salvarDadosAulaFrequencia());
    };
  }, [turmaSelecionada, resetarInfomacoes, validaSomenteConsulta, dispatch]);

  return (
    <LoaderFrequenciaPlanoAula>
      <>
        {!turmaSelecionada.turma ? (
          <Alert
            alerta={{
              tipo: 'warning',
              id: 'plano-anual-selecione-turma',
              mensagem: 'Você precisa escolher uma turma.',
              estiloTitulo: { fontSize: '18px' },
            }}
            className="mb-2"
          />
        ) : null}
        <AlertaDentroPeriodoFrequenciaPlanoAula />
        <Cabecalho
          pagina={obterDescricaoNomeMenu(
            RotasDto.FREQUENCIA_PLANO_AULA,
            modalidadesFiltroPrincipal,
            turmaSelecionada
          )}
        />
        <Card>
          <div className="col-md-12">
            <div className="row">
              <div className="col-md-12 d-flex justify-content-end pb-4">
                <BotoesAcoesFrequenciaPlanoAula />
              </div>
            </div>
            <div className="row">
              <CamposFiltrarDadosFrequenciaPlanoAula />
              <MontarListaFrequencia />
              <PlanoAula />
            </div>
          </div>
        </Card>
      </>
    </LoaderFrequenciaPlanoAula>
  );
};

export default FrequenciaPlanoAula;
