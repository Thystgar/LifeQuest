import React from 'react';
import { View, StyleSheet, Image } from 'react-native';
import { createBottomTabNavigator } from '@react-navigation/bottom-tabs';

import Tasks from './tabs/tasks';
import Rewards from './tabs/rewards';
import Settings from './tabs/settings';
import Header from './header';

const Tab = createBottomTabNavigator();

export default function RootLayout() {
  return (
    <View style={styles.container}>
      <Header />
      <Tab.Navigator>
        <Tab.Screen 
          name="Tasks" 
          component={Tasks} 
          options={{
            tabBarIcon: ({ color, size }) => (
              <Image
                source={require('../assets/images/tasks.png')}
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
      </Tab.Navigator>
    </View>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
  },
});
