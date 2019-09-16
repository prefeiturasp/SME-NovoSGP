import { combineReducers } from 'redux';

import notificacoesLista from './notificacoes/reducers';
import notificacoes from './alertas/reducers';
import bimestres from './planoAnual/reducers';
import usuario from './usuario/reducers';
import navegacao from './navegacao/reducers';

export default combineReducers({
  notificacoesLista,
  notificacoes,
  bimestres,
  usuario,
  navegacao,
});
