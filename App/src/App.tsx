
import React, { useEffect } from 'react';
import { View, StyleSheet, Image, Button, Text } from 'react-native';

import { createBottomTabNavigator } from '@react-navigation/bottom-tabs';
import { NavigationContainer } from '@react-navigation/native';
import { enableScreens } from 'react-native-screens';

import { Account } from './api';
import { useApi } from './hooks/useApi';

import * as AuthSession from 'expo-auth-session';
import * as WebBrowser from 'expo-web-browser';
import Constants from 'expo-constants';

import QuestsTab from './navigation/screens/Quest';
import RewardsTab from './navigation/screens/Reward';
import AccountHeader from './components/AccountHeader';

import AccountContext from './contexts/AccountContext';
import AuthContext from './contexts/AuthContext';
import { Authorization } from './contexts/AuthContext';


enableScreens();

const Tab = createBottomTabNavigator();

export function App(): React.JSX.Element {
  const [account, setAccount] = React.useState<Account | null>(null);
  const [user, setUser] = React.useState<Authorization | null>(null);

  const config = {
    issuer: 'https://cb6668ea-6846-40df-936d-1dbd5deadc52.ciamlogin.com/cb6668ea-6846-40df-936d-1dbd5deadc52/v2.0',
    clientId: '8c14ca47-92da-4673-a63b-1cf4f9c40653',
    redirectUrl: 'com.lifequest://auth',  // react-native
    //redirectUrl: 'exp://localhost:8081',
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
  
  const redirectUri = Constants.executionEnvironment == 'storeClient' 
    ? AuthSession.makeRedirectUri() // expo go
    : AuthSession.makeRedirectUri({ native: config.redirectUrl }); // react-native
    
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
    console.log('Response: ', response);
    const handleTokenExchange = async () => {
      console.log('Continuing with sign-in...');
      if (response?.type === 'success' && response?.params?.code && request?.codeVerifier) {
        const body = {
          code: response.params.code,
          grant_type: "authorization_code",
          client_id: config.clientId,
          redirect_uri: redirectUri,
          code_verifier: request?.codeVerifier,
        };

        console.log('Token exchange body:', body);

        fetch("https://cb6668ea-6846-40df-936d-1dbd5deadc52.ciamlogin.com/cb6668ea-6846-40df-936d-1dbd5deadc52/oauth2/v2.0/token", {
          method: "POST",
          headers: {
            "Content-Type": "application/x-www-form-urlencoded",
          },
          body: new URLSearchParams(body).toString(),
        })
        .then((resp) => resp.json())
        .then((data) => 
          {console.log("DATA =>", data);
            if (data.access_token) {
              setUser(new Authorization(data.access_token));
            } else {
              setUser(null);
            }
          })
        .catch((error) => {
          console.error('Token exchange error:', error);
        });
      };
      console.log('Auth response:', response);
    } 
    handleTokenExchange();
  }, [response]);

//   const signIn = async () => {
//     console.log('Request:', request);

//     await promptAsync()
//     .then(() => {
//       const handleTokenExchange = async () => {
//         console.log('Continuing with sign-in...');
//         if (response?.type === 'success' && response?.params?.code && request?.codeVerifier) {
//           const body = new URLSearchParams({
//             code: response.params.code,
//             grant_type: "authorization_code",
//             client_id: config.clientId,
//             redirect_uri: redirectUri,
//             code_verifier: request?.codeVerifier,
//           }).toString()

//         fetch("https://cb6668ea-6846-40df-936d-1dbd5deadc52.ciamlogin.com/cb6668ea-6846-40df-936d-1dbd5deadc52/oauth2/v2.0/token", {
//           method: "POST",
//           headers: {
//             "Content-Type": "application/x-www-form-urlencoded",
//           },
//           body,
//         })
//         .then((tokenResult) => {
//           console.log('Token result:', tokenResult);
//         })
//         .catch((error) => {
//           console.error('Token exchange error:', error);
//         });
//       };
//       handleTokenExchange();
//         // const handleTokenExchange = async () => {
//         //   try {
//         //     // Log the code and redirectUri for debugging
//         //     console.log('Exchanging code new:', response.params.code);
//         //     const tokenResult = await AuthSession.exchangeCodeAsync(
//         //       {
//         //         clientId: config.clientId,
//         //         redirectUri,
//         //         code: response.params.code,
//         //       },
//         //       discovery
//         //     );
//         //     console.log('Exchanged token new:', tokenResult.accessToken);
//         //     setUser(new Authorization(tokenResult.accessToken));
//         //     console.log('Auth response:', tokenResult);
//         //   } catch (error) {
//         //     setUser(null);
//         //     console.error('Token exchange error:', error);
//         //   }
//         // };
//         // handleTokenExchange();
//     }})
//     .catch((error) => {
//       console.error('Prompt async error:', error);
//     });
// };

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
