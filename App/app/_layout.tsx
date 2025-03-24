import React from 'react';
import { View, StyleSheet, Image } from 'react-native';
import { createBottomTabNavigator } from '@react-navigation/bottom-tabs';

import Quests from './tabs/quests';
import Rewards from './tabs/rewards';
import Settings from './tabs/settings';
import Account from './tabs/account';

const Tab = createBottomTabNavigator();

export default function RootLayout() {
  return (
    <View style={styles.container}>
      <Tab.Navigator>
        <Tab.Screen 
          name="Quests" 
          component={Quests} 
          options={{
            tabBarIcon: ({ color, size }) => (
              <Image
                source={require('../assets/images/quests.png')}
                style={{ width: size, height: size, tintColor: color }}
              />
            ),
          }}
        />
        <Tab.Screen 
          name="Rewards" 
          component={Rewards} 
          options={{
            tabBarIcon: ({ color, size }) => (
              <Image
                source={require('../assets/images/rewards.png')}
                style={{ width: size, height: size, tintColor: color }}
              />
            ),
          }}
        />
        <Tab.Screen 
          name="Settings" 
          component={Settings} 
          options={{
            tabBarIcon: ({ color, size }) => (
              <Image
                source={require('../assets/images/settings.png')}
                style={{ width: size, height: size, tintColor: color }}
              />
            ),
          }}
        />
        <Tab.Screen 
          name="Account" 
          component={Account} 
          options={{
            tabBarIcon: ({ color, size }) => (
              <Image
                source={require('../assets/images/account.png')}
                style={{ width: size, height: size, tintColor: color }}
              />
            ),
          }}
        />
      </Tab.Navigator>
    </View>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
  },
});
