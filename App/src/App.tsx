
import React from 'react';
import { View, Image, Text } from 'react-native';

import { createBottomTabNavigator } from '@react-navigation/bottom-tabs';
import { NavigationContainer } from '@react-navigation/native';
import { enableScreens } from 'react-native-screens';

import QuestsTab from '@/navigation/screens/Quest';
import RewardsTab from '@/navigation/screens/Reward';
import AccountHeader from '@/components/AccountHeader';
import JoinGroupComponent from '@/components/JoinGroupComponent';
import SignInComponent from '@/components/SignInComponent';

import { useAuth } from '@/hooks/useAuth';
import { useAccount } from './hooks/useAccount';


enableScreens();

const Tab = createBottomTabNavigator();

export function App(): React.JSX.Element {
  const { signIn, isUserAuthenticated } = useAuth();
  const { isMemberOfGroup } = useAccount();

  return (
    <NavigationContainer>
      {!isUserAuthenticated ? (
        <SignInComponent/>
      ) : (
        !isMemberOfGroup ? (
          <JoinGroupComponent/>
        ) : (
        <>
          <AccountHeader />
          <Tab.Navigator>
            <Tab.Screen
              name="Quests"
              component={QuestsTab}
              options={{
                headerShown: false,
                tabBarIcon: ({ color, size }) => (
                  <Image
                    source={require('@/assets/images/quests.png')}
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
                    source={require('@/assets/images/rewards.png')}
                    style={{ width: size, height: size, tintColor: color }}
                  />
                ),
              }}
            />
          </Tab.Navigator>
        </>
      ))}
    </NavigationContainer>
  );
}
