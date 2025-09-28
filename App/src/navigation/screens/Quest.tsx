import React, { useEffect, useState } from 'react';
import { View, Text, StyleSheet, TouchableOpacity } from 'react-native';
import AddQuestModal from '@/components/AddQuestModal';
import { SwipeListView } from 'react-native-swipe-list-view';
import { Quest, QuestStatus } from '@/api';
import { useApi } from '@/hooks/useApi';
import { useAccount } from '@/hooks/useAccount';
import { useAuth } from '@/hooks/useAuth';

export default function QuestsTab() {
  
  const { fetchQuests, completeQuest, addQuest } = useApi();
  const { account, onPointChange } = useAccount();

  const { isUserAuthenticated } = useAuth();
  
  const [quests, setQuests] = useState<Quest[]>([]);
  const [modalVisible, setModalVisible] = useState(false);
  const [quest, setQuest] = useState<Quest | null>(new Quest());

  useEffect(() => {
    fetchData();
  }, [isUserAuthenticated]);

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
      onPointChange();
    } catch (error) {
      console.error(error);
    }
  };

  const handleAddQuest = async () => {
    console.log('Adding quest:', quest);
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
        data={quests.sort((a, b) => a.name.localeCompare(b.name))}
        keyExtractor={(quest) => quest.id}
        renderItem={({ item }) => (
          <View style={styles.rowFront}>
            <View style={{ flex: 1 }}>
              <Text style={[styles.quest, item.status == QuestStatus.Completed && styles.completedQuest]}>{item.name}</Text>
              <Text style={styles.description}>{item.description}</Text>
            </View>
            <Text style={styles.value}>{item.value} points</Text>
          </View>
        )}
        renderHiddenItem={({ item }) => (
          <View style={styles.rowBack}>
            <Text style={styles.backText}>Complete</Text>
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
            <AddQuestModal
              visible={modalVisible}
              quest={quest}
              setQuest={setQuest}
              onSubmit={handleAddQuest}
              onClose={() => setModalVisible(false)}
            />
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
  description: {
    fontSize: 14,
    color: '#888',
    marginTop: 2,
  },
  value: {
    fontSize: 16,
    fontWeight: 'bold',
    color: '#333',
    marginLeft: 10,
    alignSelf: 'center',
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