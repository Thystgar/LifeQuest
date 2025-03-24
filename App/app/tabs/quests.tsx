import React, { useEffect, useState } from 'react';
import { View, Text, StyleSheet } from 'react-native';
import { SwipeListView } from 'react-native-swipe-list-view';

export default function Quests() {
  interface Quest {
    id: string;
    name: string;
    points: number;
    completed: boolean;
  }

  const [quests, setQuests] = useState<Quest[]>([]);

  useEffect(() => {
    const fetchQuests = async () => {
      try {
        const response = await fetch('http://10.0.2.2:8080/quests');
        if (!response.ok) {
          throw new Error(`HTTP error! status: ${response.status}`);
        }
        const data = await response.json();
        setQuests(data);
      } catch (error) {
        console.error('Error fetching quests:', error);
      }
    };

    fetchQuests();
  }, []);

  const handleQuestSwipe = async (id: string) => {
    try {
      const response = await fetch(`http://10.0.2.2:8080/quests/finish?id=${id}`, {
        method: 'POST',
      });

      if (response.ok) {
        setQuests((prevQuests) =>
          prevQuests.map((quest) =>
            quest.id === id ? { ...quest, completed: true } : quest
          )
        );
      } else {
        console.error('Failed to complete quest');
      }
    } catch (error) {
      console.error('Error completing quest:', error);
    }
  };

  const handleQuestReactivate = async (id: string) => {
    try {
      const response = await fetch(`http://10.0.2.2:8080/quests/reactivate?id=${id}`, {
        method: 'POST',
      });

      if (response.ok) {
        setQuests((prevQuests) =>
          prevQuests.map((quest) =>
            quest.id === id ? { ...quest, completed: false } : quest
          )
        );
      } else {
        console.error('Failed to reactivate quest');
      }
    } catch (error) {
      console.error('Error reactivating quest:', error);
    }
  };

  return (
    <View style={styles.container}>
      <SwipeListView
        data={quests.sort((a, b) => Number(a.completed) - Number(b.completed))}
        keyExtractor={(quest) => quest.id}
        renderItem={({ item }) => (
          <View style={styles.rowFront}>
            <Text style={[styles.quest, item.completed && styles.completedQuest]}>
              {item.name} - {item.points} points
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