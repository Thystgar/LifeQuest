import React, { useContext, useEffect, useState } from 'react';
import { View, Text, StyleSheet, TouchableOpacity, Modal, TextInput, Button } from 'react-native';
import { SwipeListView } from 'react-native-swipe-list-view';
import { Quest, QuestStatus, useApi } from '../../api';
import { AuthContext } from '../../contexts';

export default function QuestsTab() {
  const user = useContext(AuthContext);
  
  const { fetchQuests, completeQuest, addQuest } = useApi();

  const [quests, setQuests] = useState<Quest[]>([]);
  const [modalVisible, setModalVisible] = useState(false);
  const [quest, setQuest] = useState<Quest | null>(new Quest());

  useEffect(() => {
    fetchData();
  }, [user]);

  const fetchData = async () => {
    try {
      const data = await fetchQuests();
      console.log('Fetched quests:', data);
      setQuests(data);
    } catch (error) {
      console.error(error);
    }
  };

  const handleQuestSwipe = async (id: string) => {
    try {
      await completeQuest(id);
      setQuests((prevQuests) =>
        prevQuests.map((quest) =>
          quest.id === id ? { ...quest, status: QuestStatus.Completed } : quest
        )
      );
    } catch (error) {
      console.error(error);
    }
  };

  const handleAddQuest = async () => {
    try {
      if (quest) {
        await addQuest(quest);
      }
      fetchData();
      setModalVisible(false);
    } catch (error) {
      console.error(error);
    }
  };

  return (
    <View style={styles.container}>
      <SwipeListView
        data={quests.sort((a, b) => a.status === QuestStatus.Completed ? 1 : -1)}
        keyExtractor={(quest) => quest.id}
        renderItem={({ item }) => (
          <View style={styles.rowFront}>
            <Text style={[styles.quest, item.status == QuestStatus.Completed && styles.completedQuest]}>
              {item.name} - {item.value} points
            </Text>
          </View>
        )}
        renderHiddenItem={({ item }) => (
          <View style={styles.rowBack}>
            <Text style={styles.backText}>Complete</Text>
            <Text style={styles.backText}>Reactivate</Text>
          </View>
        )}
        leftOpenValue={75}
        rightOpenValue={-75}
        onRowOpen={(rowKey, rowMap, toValue) => {
          if (toValue > 0) {
            handleQuestSwipe(rowKey);
          }
          rowMap[rowKey].closeRow();
        }}
      />
            <TouchableOpacity
              style={styles.fab}
              onPress={() => {
                setQuest(new Quest());
                setModalVisible(true);
              }}
            >
              <Text style={styles.fabText}>+</Text>
            </TouchableOpacity>
            <Modal
              transparent={true}
              visible={modalVisible}
              onRequestClose={() => {
                setModalVisible(!modalVisible);
              }}
            >
              <TouchableOpacity
                style={styles.modalOverlay}
                activeOpacity={1}
                onPressOut={() => setModalVisible(false)}
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
                      onChangeText={description => setQuest(prev => prev ? { ...prev, description: description } : null)}
                      keyboardType="numeric"
                    />
                    <TextInput
                      style={styles.input}
                      placeholder="Value"
                      value={quest?.value !== undefined && quest?.value !== 0 ? quest.value.toString() : ''}
                      onChangeText={points => setQuest(prev => prev ? { ...prev, value: parseInt(points)} : null)}
                      keyboardType="numeric"
                    />
                    <View style={styles.buttonContainer}>
                      <Button title="Submit" onPress={handleAddQuest} />
                    </View>
                  </View>
                </View>
              </TouchableOpacity>
            </Modal>
    </View>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: '#fff',
  },
  rowFront: {
    backgroundColor: '#fff',
    borderBottomColor: '#ccc',
    borderBottomWidth: 1,
    padding: 10,
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
  },
  quest: {
    fontSize: 18,
  },
  completedQuest: {
    textDecorationLine: 'line-through',
    color: 'green',
  },
  rowBack: {
    alignItems: 'center',
    backgroundColor: '#DDD',
    flex: 1,
    flexDirection: 'row',
    justifyContent: 'space-between',
    paddingLeft: 15,
  },
  backText: {
    color: '#FFF',
  },
  fab: {
    position: 'absolute',
    width: 56,
    height: 56,
    alignItems: 'center',
    justifyContent: 'center',
    right: 20,
    bottom: 20,
    backgroundColor: '#03A9F4',
    borderRadius: 30,
    elevation: 8,
  },
  fabText: {
    fontSize: 24,
    color: 'white',
  },
  modalContainer: {
    width: '100%',
    flex: 1,
    justifyContent: 'flex-end',
    alignItems: 'flex-end',
    paddingBottom: 0,
  },
  modalView: {
    width: '100%',
    minHeight: 300,
    maxHeight: '90%',
    backgroundColor: 'white',
    borderRadius: 0, // No rounded corners
    padding: 35,
    alignItems: 'flex-start', // Align items to the left
    shadowColor: '#000',
    shadowOffset: {
      width: 0,
      height: -2,
    },
    shadowOpacity: 0.25,
    shadowRadius: 4,
    elevation: 5,
    position: 'absolute',
    bottom: 0,
    left: 0,
    right: 0,
  },
  modalText: {
    marginBottom: 15,
    textAlign: 'center',
    fontSize: 20,
    fontWeight: 'bold',
  },
  input: {
    height: 40,
    borderColor: 'gray',
    borderWidth: 1,
    marginBottom: 15,
    width: '90%',
    paddingHorizontal: 10,
  },
  buttonContainer: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    width: '90%',
  },
  modalOverlay: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
    backgroundColor: 'rgba(0, 0, 0, 0.5)',
  },
});