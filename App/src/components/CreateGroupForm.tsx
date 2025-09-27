import React, { useState } from 'react';
import { View, Text, TextInput, Button, StyleSheet } from 'react-native';
import { Colors } from '@/constants/Colors';

interface CreateGroupFormProps {
  onSubmit: (name: string, description: string) => void;
  onBack: () => void;
}

const CreateGroupForm: React.FC<CreateGroupFormProps> = ({ onSubmit, onBack }) => {
  const [groupName, setGroupName] = useState('');
  const [groupDescription, setGroupDescription] = useState('');

  return (
    <View style={styles.container}>
      <Text style={styles.title}>Create a New Group</Text>
      <TextInput
        style={styles.input}
        placeholder="Group Name"
        value={groupName}
        onChangeText={setGroupName}
      />
      <TextInput
        style={styles.input}
        placeholder="Group Description"
        value={groupDescription}
        onChangeText={setGroupDescription}
      />
      <View style={styles.buttonRow}>
        <Button title="Back" onPress={onBack} color={Colors.light.tabIconDefault} />
        <Button title="OK" onPress={() => onSubmit(groupName, groupDescription)} color={Colors.light.tint} disabled={!groupName.trim()} />
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
  input: {
    width: '100%',
    maxWidth: 340,
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
  buttonRow: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    width: '100%',
    maxWidth: 340,
    marginTop: 12,
  },
});

export default CreateGroupForm;
