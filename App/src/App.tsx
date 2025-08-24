
import React, { useEffect } from 'react';
import { View, StyleSheet, Image, Button, Text } from 'react-native';
import { createBottomTabNavigator } from '@react-navigation/bottom-tabs';
import { NavigationContainer } from '@react-navigation/native';
import { enableScreens } from 'react-native-screens';

import { useApi, Account } from './api';

import * as AuthSession from 'expo-auth-session';
import * as WebBrowser from 'expo-web-browser';

import QuestsTab from './navigation/screens/Quest';
import RewardsTab from './navigation/screens/Reward';
import AccountHeader from './components/AccountHeader';

import AccountContext from './contexts/AccountContext';
import AuthContext from './contexts/AuthContext';

enableScreens();

const Tab = createBottomTabNavigator();

export function App(): React.JSX.Element {
  const [account, setAccount] = React.useState<Account | null>(null);
  const [user, setUser] = React.useState<any | null>(null);

  const config = {
    issuer: 'https://cb6668ea-6846-40df-936d-1dbd5deadc52.ciamlogin.com/cb6668ea-6846-40df-936d-1dbd5deadc52/v2.0',
    clientId: '8c14ca47-92da-4673-a63b-1cf4f9c40653',
    redirectUrl: 'com.lifequest://auth', 
    //redirectUrl: 'http://localhost:8081',
    scopes: ['openid', 'profile', 'email', 'offline_access', 'api://5a4e418f-36f4-4546-9983-0f76427232d6/User'],
    serviceConfiguration: {
      authorizationEndpoint: 'https://cb6668ea-6846-40df-936d-1dbd5deadc52.ciamlogin.com/cb6668ea-6846-40df-936d-1dbd5deadc52/oauth2/v2.0/authorize',
      tokenEndpoint: 'https://cb6668ea-6846-40df-936d-1dbd5deadc52.ciamlogin.com/cb6668ea-6846-40df-936d-1dbd5deadc52/oauth2/v2.0/token',
      revocationEndpoint: 'https://cb6668ea-6846-40df-936d-1dbd5deadc52.ciamlogin.com/cb6668ea-6846-40df-936d-1dbd5deadc52/oauth2/v2.0/logout',
    },
  };

  WebBrowser.maybeCompleteAuthSession();

  const discovery = {
    authorizationEndpoint: config.serviceConfiguration.authorizationEndpoint,
    tokenEndpoint: config.serviceConfiguration.tokenEndpoint,
    revocationEndpoint: config.serviceConfiguration.revocationEndpoint,
  };
  const redirectUri = AuthSession.makeRedirectUri();
  const authRequestConfig = {
    clientId: config.clientId,
    scopes: config.scopes,
    redirectUri,
    responseType: 'code',
  };
  const [request, response, promptAsync] = AuthSession.useAuthRequest(authRequestConfig, discovery);

  const signIn = async () => {
    try {
      await promptAsync();
    } catch (error) {
      console.error('Authentication error:', error);
      setUser(null);
    }
  };

  React.useEffect(() => {
    if (response?.type === 'success') {
      setUser(response);
      console.log('Auth response:', response);
    } else if (response) {
      setUser(null);
    }
  }, [response]);

  const signOut = async () => {
    setUser(null);
    setAccount({ name: '', points: 0 });
  };

  const { fetchAccount } = useApi();
  useEffect(() => {
    if (!user) return;
    const loadAccount = async () => {
      try {
        const accountData = await fetchAccount();
        setAccount(accountData);
      } catch (error) {
        console.error('Failed to fetch account data:', error);
      }
    };
    loadAccount();
  }, [user]);

  return (
    <AuthContext.Provider value={user}>
      <AccountContext.Provider value={account}>
        <NavigationContainer>
          <AccountHeader />
          {!user ? (
            <View style={{ flex: 1, justifyContent: 'center', alignItems: 'center' }}>
              <Text>Please sign in with Microsoft</Text>
              <Button title={'Sign in'} onPress={signIn} />
            </View>
          ) : (
            <>
              <Button title="Sign out" onPress={signOut} />
              <Tab.Navigator>
                <Tab.Screen
                  name="Quests"
                  component={QuestsTab}
                  options={{
                    headerShown: false,
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
                    headerShown: false,
                    tabBarIcon: ({ color, size }) => (
                      <Image
                        source={require('./assets/images/rewards.png')}
                        style={{ width: size, height: size, tintColor: color }}
                      />
                    ),
                  }}
                />
              </Tab.Navigator>
            </>
          )}
        </NavigationContainer>
      </AccountContext.Provider>
    </AuthContext.Provider>
  );
}
