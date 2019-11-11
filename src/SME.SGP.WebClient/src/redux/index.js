import { persistStore } from 'redux-persist';
import thunk from 'redux-thunk';
import createStore from './createStore';
import persistReducers from './persistReducers';

import rootReducer from './modulos/reducers';

const middlewares = [thunk];

const store = createStore(persistReducers(rootReducer), middlewares);
const persistor = persistStore(store);

export { store, persistor };
