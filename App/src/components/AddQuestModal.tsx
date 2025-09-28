import React from 'react';
import { View, Text, TextInput, Button, StyleSheet, TouchableOpacity, Modal } from 'react-native';
import { Quest } from '@/api';

interface AddQuestModalProps {
  visible: boolean;
  quest: Quest | null;
  setQuest: React.Dispatch<React.SetStateAction<Quest | null>>;
  onSubmit: () => void;
  onClose: () => void;
}

const AddQuestModal: React.FC<AddQuestModalProps> = ({ visible, quest, setQuest, onSubmit, onClose }) => {
  return (
    <Modal
      transparent={true}
      visible={visible}
      onRequestClose={onClose}
    >
      <TouchableOpacity
        style={styles.modalOverlay}
        activeOpacity={1}
        onPressOut={onClose}
      >
        <View style={styles.modalContainer}>
          <View style={styles.modalView}>
            <Text style={styles.modalText}>{'Add New Quest'}</Text>
            <TextInput
              style={styles.input}
              placeholder="Name"
              value={quest?.name}
              onChangeText={name => setQuest(prev => prev ? { ...prev, name } : null)}
            />
            <TextInput
              style={styles.input}
              placeholder="Description"
              value={quest?.description}
              onChangeText={description => setQuest(prev => prev ? { ...prev, description } : null)}
            />
            <TextInput
              style={styles.input}
              placeholder="Value"
              value={quest?.value !== undefined && quest?.value !== 0 ? quest.value.toString() : ''}
              onChangeText={points => setQuest(prev => prev ? { ...prev, value: parseInt(points)} : null)}
              keyboardType="numeric"
            />
            <View style={styles.buttonContainer}>
              <Button title="Submit" onPress={onSubmit} />
            </View>
          </View>
        </View>
      </TouchableOpacity>
    </Modal>
  );
};

const styles = StyleSheet.create({
  modalOverlay: {
    flex: 1,
    backgroundColor: 'rgba(0,0,0,0.3)',
    justifyContent: 'center',
    alignItems: 'center',
  },
  modalContainer: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
  },
  modalView: {
    backgroundColor: '#fff',
    borderRadius: 8,
    padding: 24,
    alignItems: 'center',
    shadowColor: '#000',
    shadowOffset: { width: 0, height: 2 },
    shadowOpacity: 0.25,
    shadowRadius: 4,
    elevation: 5,
    minWidth: 300,
  },
  modalText: {
    fontSize: 20,
    fontWeight: 'bold',
    marginBottom: 16,
  },
  input: {
    width: 220,
    height: 40,
    borderColor: '#ccc',
    borderWidth: 1,
    borderRadius: 6,
    marginBottom: 12,
    paddingHorizontal: 10,
    fontSize: 16,
  },
  buttonContainer: {
    marginTop: 8,
    width: '100%',
  },
});

export default AddQuestModal;
