import './gesture-handler';

import '@expo/metro-runtime'; // Necessary for Fast Refresh on Web
import { registerRootComponent } from 'expo';
import Constants from 'expo-constants';

import { App } from './src/App';
import { AppRegistry } from 'react-native';

// registerRootComponent calls AppRegistry.registerComponent('main', () => App);
// It also ensures that whether you load the app in Expo Go or in a native build,
// the environment is set up appropriately
registerRootComponent(App);
if (Constants.executionEnvironment == 'storeClient') {
  console.log('Running in expo go');
  registerRootComponent(App);
} else {
  console.log('Running in native build');
  AppRegistry.registerComponent('lifequest', () => App);
}
