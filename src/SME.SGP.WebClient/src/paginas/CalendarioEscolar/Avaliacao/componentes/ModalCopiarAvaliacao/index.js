import React, { useState, useEffect, useCallback, useReducer } from 'react';
import t from 'prop-types';
import shortid from 'shortid';

// Redux
import { useSelector, useDispatch } from 'react-redux';
import { setLoaderModal } from '~/redux/modulos/loader/actions';

// Componentes
import {
  ModalConteudoHtml,
  Button,
  Colors,
  CampoData,
  Grid,
  Loader,
} from '~/componentes';
import TurmasDropDown from '~/componentes-sgp/TurmasDropDown';

// Styles
import { Row } from './styles';

// Serviço
import api from '~/servicos/api';
import PlanoAulaServico from '~/servicos/Paginas/PlanoAula';
import AvaliacaoServico from '~/servicos/Paginas/Calendario/ServicoAvaliacao';
import { erros, erro, sucesso } from '~/servicos/alertas';

// Reducer
import Reducer, {
  estadoInicial,
  adicionarAvaliacao,
  excluirAvaliacao,
  carregarData,
  selecionarTurma,
  diasParaHabilitar,
  selecionarData,
  erroData,
} from './reducer';

// Funções
import { valorNuloOuVazio } from '~/utils/funcoes/gerais';

function ModalCopiarAvaliacao({ show, disciplina, onClose, onSalvarCopias }) {
  const filtro = useSelector(store => store.usuario.turmaSelecionada);
  const carregando = useSelector(store => store.loader.loaderModal);
  const dispatch = useDispatch();
  const [listaTurmas, setListaTurmas] = useState([]);
  const [turmas, setTurmas] = useState([]);

  const [estado, disparar] = useReducer(Reducer, estadoInicial);

  useEffect(() => {
    async function buscaTurmas() {
      const { data } = await AvaliacaoServico.listarTurmasModal(
        filtro.turma,
        disciplina
      );

      if (data) {
        setListaTurmas(
          data.map(item => ({
            desc: item.nome,
            valor: item.codigo,
          }))
        );
      }
    }
    if (filtro.turma && disciplina) buscaTurmas();
  }, [
    filtro.unidadeEscolar,
    filtro.modalidade,
    filtro.ano,
    filtro.turma,
    disciplina,
  ]);

  const adicionarTurma = () => disparar(adicionarAvaliacao());

  const onClickExcluir = item => disparar(excluirAvaliacao(item.id));

  const onChangeTurma = useCallback(
    async (turma, linha) => {
      try {
        if (valorNuloOuVazio(turma)) {
          disparar(selecionarTurma({ id: linha.id, turmaId: turma }));
          disparar(selecionarData({ id: linha.id, data: '' }));
          disparar(erroData({ id: linha.id, turmaId: String(0) }));
          return;
        }

        disparar(carregarData({ id: linha.id, valor: true }));

        // TODO: Remover ano letivo chumbado
        const { data, status } = await api.get(
          `v1/calendarios/frequencias/aulas/datas/${filtro.anoLetivo}/turmas/${turma}/disciplinas/${disciplina}`
        );

        if (data && status === 200) {
          disparar(carregarData({ id: linha.id, valor: false }));
          disparar(selecionarTurma({ id: linha.id, turmaId: turma }));
          disparar(diasParaHabilitar({ id: linha.id, datas: data }));
        }
      } catch (error) {
        erro(error);
      }
    },
    [disciplina, filtro.anoLetivo]
  );

  const onChangeData = async (dataSelecionada, linha) => {
    try {
      disparar(selecionarData({ id: linha.id, data: dataSelecionada }));
      disparar(carregarData({ id: linha.id, valor: true }));
      const { data, status } = await AvaliacaoServico.verificarSeExiste({
        atividadeAvaliativaTurmaDatas: estado.turmas.map(x => ({
          dataAvaliacao: dataSelecionada,
          turmaId: x.turmaId,
          disciplinasId: [disciplina],
        })),
      });

      if (data && status === 200) {
        disparar(carregarData({ id: linha.id, valor: false }));
        const temErro = data.filter(x => x.erro === true);
        if (temErro.length > 0) {
          temErro.forEach(err => {
            disparar(erroData({ id: linha.id, turmaId: String(err.turmaId) }));
          });
        } else {
          disparar(erroData({ id: linha.id, turmaId: String(0) }));
        }
      }
    } catch (error) {
      disparar(carregarData({ id: linha.id, valor: false }));
      disparar(erroData({ id: linha.id, turmaId: String(0) }));
    }
  };

  const onCloseModal = () => {
    setTurmas([]);
    onClose();
  };

  const onClickSalvar = async () => {
    const dadosParaSalvar = estado.turmas.map(x => ({
      turmaId: x.turmaId,
      dataAvaliacao: x.data,
      turma: listaTurmas.filter(y => x.turmaId === y.valor),
    }));
    onSalvarCopias(dadosParaSalvar);
    onClose();
  };

  const desabilitarSalvar = useCallback(() => {
    const turmaNaoPreenchida = estado.turmas.some(x => !x.turmaId || !x.data);
    const ehVazioOuTemErro =
      estado.turmas.length < 1 || estado.turmas.some(x => x.temErro);
    const naoEstaCarregando =
      estado.turmas.some(x => x.carregandoData) || carregando;
    return ehVazioOuTemErro || turmaNaoPreenchida || naoEstaCarregando;
  }, [estado, carregando]);

  return (
    <ModalConteudoHtml
      titulo="Copiar avaliação"
      visivel={show}
      closable
      onClose={() => onCloseModal()}
      onConfirmacaoSecundaria={() => onCloseModal()}
      onConfirmacaoPrincipal={() => onClickSalvar()}
      labelBotaoPrincipal="Confirmar"
      labelBotaoSecundario="Descartar"
      desabilitarBotaoPrincipal={desabilitarSalvar()}
    >
      <Loader loading={carregando}>
        {estado.turmas.map(linha => (
          <Row key={shortid.generate()} className="row">
            <Grid cols={5}>
              <TurmasDropDown
                ueId={filtro.unidadeEscolar}
                modalidadeId={filtro.modalidade}
                valor={linha.turmaId}
                onChange={turma => onChangeTurma(turma, linha)}
                dados={
                  listaTurmas.filter(x => !estado.turmas.includes(x.turmaId)) ||
                  []
                }
              />
            </Grid>
            <Grid cols={5}>
              <CampoData
                valor={linha.data}
                onChange={data => onChangeData(data, linha)}
                name="dataInicio"
                placeholder="DD/MM/AAAA"
                formatoData="DD/MM/YYYY"
                temErro={linha.temErro}
                mensagemErro={linha.mensagemErro}
                diasParaHabilitar={linha.diasParaHabilitar}
                desabilitado={!linha.turmaId}
                carregando={linha.carregandoData}
              />
            </Grid>
            <Grid cols={2}>
              <Button
                id={shortid.generate()}
                label="Excluir"
                color={Colors.Roxo}
                border
                className="btnGroupItem"
                onClick={() => onClickExcluir(linha)}
              />
            </Grid>
          </Row>
        ))}
        <div style={{ display: 'flex', justifyContent: 'flex-end' }}>
          <Button
            id={shortid.generate()}
            label="Adicionar turma"
            color={Colors.Azul}
            border
            className="btnGroupItem"
            onClick={adicionarTurma}
          />
        </div>
      </Loader>
    </ModalConteudoHtml>
  );
}

ModalCopiarAvaliacao.propTypes = {
  show: t.bool,
  disciplina: t.string,
  onClose: t.func,
  onSalvarCopias: t.func,
};

ModalCopiarAvaliacao.defaultProps = {
  show: false,
  disciplina: null,
  onClose: null,
  onSalvarCopias: null,
};

export default ModalCopiarAvaliacao;
