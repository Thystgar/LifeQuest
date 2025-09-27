import React from 'react';
import { View, Button, Text } from 'react-native';
import { useAuth } from '@/hooks/useAuth';

const SignInComponent: React.FC = () => {
  const { signIn } = useAuth();

  return (
    <View style={{ flex: 1, justifyContent: 'center', alignItems: 'center' }}>
      <Text>Please sign in with Microsoft</Text>
      <Button title={'Sign in'} onPress={signIn} />
    </View>
  );
};

export default SignInComponent;
