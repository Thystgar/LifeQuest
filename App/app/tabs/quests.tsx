import React, { useEffect, useState } from 'react';
import { View, Text, StyleSheet } from 'react-native';
import { SwipeListView } from 'react-native-swipe-list-view';
import { fetchQuests, completeQuest, reactivateQuest } from '../shared/api';
import { Quest, QuestStatus } from '../shared/types';

export default function Quests() {
  const [quests, setQuests] = useState<Quest[]>([]);

  useEffect(() => {
    const fetchData = async () => {
      try {
        const data = await fetchQuests();
        setQuests(data);
      } catch (error) {
        console.error(error);
      }
    };
    fetchData();
  }, []);

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

  const handleQuestReactivate = async (id: string) => {
    try {
      await reactivateQuest(id);
      setQuests((prevQuests) =>
        prevQuests.map((quest) =>
          quest.id === id ? { ...quest, status: QuestStatus.Active } : quest
        )
      );
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
          } else {
            handleQuestReactivate(rowKey);
          }
          rowMap[rowKey].closeRow();
        }}
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
});