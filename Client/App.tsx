/**
 * Sample React Native App
 * https://github.com/facebook/react-native
 *
 * @format
 */

import React, { useEffect } from 'react';
import { View, StyleSheet, Image } from 'react-native';
import { createBottomTabNavigator } from '@react-navigation/bottom-tabs';
import { NavigationContainer } from '@react-navigation/native';
import { enableScreens } from 'react-native-screens';
import type { Account } from './shared/types';
import { fetchAccount } from './shared/api';

import QuestsTab from './tabs/quests';
import RewardsTab from './tabs/rewards';
import SettingsTab from './tabs/settings';
import AccountTab from './tabs/account';

enableScreens();

export const AccountContext = React.createContext<Account>({ name: '', points: 0 });

const Tab = createBottomTabNavigator();

function App(): React.JSX.Element {
  const [account, setAccount] = React.useState<Account>({ name: '', points: 0 });

  useEffect(() => {
    const loadAccount = async () => {
      // try {
      //   const accountData = await fetchAccount();
      //   setAccount(accountData);
      // } catch (error) {
      //   console.error('Failed to fetch account data:', error);
      // }
      setAccount({ name: 'John Doe', points: 100 }); // Mock data for testing
    };

    loadAccount();
  }, []);

  return (
    <AccountContext.Provider value={account}>
      <NavigationContainer>
        <Tab.Navigator>
          <Tab.Screen
            name="Quests"
            component={QuestsTab}
            options={{
              tabBarIcon: ({ color, size }) => (
                <Image
                  source={require('./assets/images/quests.png')}
                  style={{ width: size, height: size, tintColor: color }}
                />
              ),
            }}
          />
          <Tab.Screen
            name="Rewards"
            component={RewardsTab}
            options={{
              tabBarIcon: ({ color, size }) => (
                <Image
                  source={require('./assets/images/rewards.png')}
                  style={{ width: size, height: size, tintColor: color }}
                />
              ),
            }}
          />
          <Tab.Screen
            name="Settings"
            component={SettingsTab}
            options={{
              tabBarIcon: ({ color, size }) => (
                <Image
                  source={require('./assets/images/settings.png')}
                  style={{ width: size, height: size, tintColor: color }}
                />
              ),
            }}
          />
          <Tab.Screen
            name="Account"
            component={AccountTab}
            options={{
              tabBarIcon: ({ color, size }) => (
                <Image
                  source={require('./assets/images/account.png')}
                  style={{ width: size, height: size, tintColor: color }}
                />
              ),
            }}
          />
        </Tab.Navigator>
      </NavigationContainer>
    </AccountContext.Provider>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
  },
});

export default App;
