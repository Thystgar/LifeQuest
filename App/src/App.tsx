
import React from 'react';
import { View, Image, Button, Text } from 'react-native';

import { createBottomTabNavigator } from '@react-navigation/bottom-tabs';
import { NavigationContainer } from '@react-navigation/native';
import { enableScreens } from 'react-native-screens';

import QuestsTab from '@/navigation/screens/Quest';
import RewardsTab from '@/navigation/screens/Reward';
import AccountHeader from '@/components/AccountHeader';

import { useAuth } from '@/hooks/useAuth';


enableScreens();

const Tab = createBottomTabNavigator();

export function App(): React.JSX.Element {
  const { signIn, signOut, isUserAuthenticated } = useAuth();

  return (
    <NavigationContainer>
      {!isUserAuthenticated ? (
        <View style={{ flex: 1, justifyContent: 'center', alignItems: 'center' }}>
          <Text>Please sign in with Microsoft</Text>
          <Button title={'Sign in'} onPress={signIn} />
        </View>
      ) : (
        <>
          <AccountHeader />
          <Button title="Sign out" onPress={signOut} />
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
      )}
    </NavigationContainer>
  );
}
