import React, { useState } from 'react';
import { View, Button, Text, TextInput, StyleSheet } from 'react-native';
import { Colors } from '@/constants/Colors';
import CreateGroupForm from '@/components/CreateGroupForm';
import { useApi } from '@/hooks/useApi';
import { NewGroup } from '@/api';
import { useAccount } from '@/hooks/useAccount';

const JoinGroupComponent: React.FC = () => {
  const [inviteCode, setInviteCode] = useState('');
  const [showCreateForm, setShowCreateForm] = useState(false);
  const { joinGroup, createGroup } = useApi();
  const { onGroupJoin, account } = useAccount();

  const handleJoinGroup = async () => {
    await joinGroup(inviteCode);
    await onGroupJoin();
  };

  const handleCreateGroup = () => {
    setShowCreateForm(true);
  };

  const handleCreateGroupSubmit = async (name: string, description: string) => {
    await createGroup(new NewGroup(name, description));
    await onGroupJoin();
    setShowCreateForm(false);
  };

  const handleBack = () => {
    setShowCreateForm(false);
  };

  if (showCreateForm) {
    return <CreateGroupForm onSubmit={handleCreateGroupSubmit} onBack={handleBack} />;
  }

  return (
    <View style={styles.container}>
      <Text style={styles.label}>Welcome {account?.name}</Text>
      <View style={styles.section}>
        <Text style={styles.label}>Join group by token:</Text>
        <TextInput
          style={styles.input}
          placeholder="Enter group invite code"
          value={inviteCode}
          onChangeText={setInviteCode}
        />
        <Button
          title="Join Group"
          onPress={handleJoinGroup}
          disabled={!inviteCode.trim()}
          color={inviteCode.trim() ? '#4CAF50' : undefined}
        />
      </View>
      <View style={styles.section}>
        <Text style={styles.label}>Or create a new group:</Text>
        <Button title="Create Group" onPress={handleCreateGroup} />
      </View>
    </View>
  );
};

const styles = StyleSheet.create({
  container: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
    paddingHorizontal: 24,
    paddingVertical: 32,
    backgroundColor: Colors.light.background,
  },
  title: {
    fontSize: 22,
    fontWeight: 'bold',
    marginBottom: 28,
    color: Colors.light.text,
    textAlign: 'center',
  },
  section: {
    width: '100%',
    maxWidth: 340,
    marginBottom: 24,
    alignItems: 'center',
    padding: 18,
    borderRadius: 16,
    backgroundColor: '#B3E5FC',
    shadowColor: '#000',
    shadowOpacity: 0.07,
    shadowRadius: 6,
    elevation: 2,
    borderWidth: 2,
    borderColor: Colors.light.tint,
  },
  label: {
    fontSize: 16,
    marginBottom: 10,
    color: Colors.light.icon,
    fontWeight: '600',
    textAlign: 'center',
  },
  input: {
    width: '100%',
    height: 44,
    borderColor: Colors.light.tabIconDefault,
    borderWidth: 1,
    borderRadius: 10,
    paddingHorizontal: 12,
    marginBottom: 18,
    backgroundColor: Colors.light.background,
    fontSize: 15,
    color: Colors.light.text,
  },
});

export default JoinGroupComponent;
