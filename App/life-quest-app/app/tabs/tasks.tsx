import React, { useEffect, useState } from 'react';
import { View, Text, StyleSheet } from 'react-native';
import { SwipeListView } from 'react-native-swipe-list-view';

export default function Tasks() {
  interface TodoItem {
    id: string;
    name: string;
    points: number;
    completed: boolean;
  }

  const [todoItems, setTodoItems] = useState<TodoItem[]>([]);

  useEffect(() => {
    const fetchTasks = async () => {
      try {
        const response = await fetch('http://10.0.2.2:8080/tasks');
        if (!response.ok) {
          throw new Error(`HTTP error! status: ${response.status}`);
        }
        const data = await response.json();
        setTodoItems(data);
      } catch (error) {
        console.error('Error fetching tasks:', error);
      }
    };

    fetchTasks();
  }, []);

  const handleTaskSwipe = async (id: string) => {
    try {
      const response = await fetch(`http://10.0.2.2:8080/tasks/finish?id=${id}`, {
        method: 'POST',
      });

      if (response.ok) {
        setTodoItems((prevItems) =>
          prevItems.map((item) =>
            item.id === id ? { ...item, completed: true } : item
          )
        );
      } else {
        console.error('Failed to complete task');
      }
    } catch (error) {
      console.error('Error completing task:', error);
    }
  };

  const handleTaskReactivate = async (id: string) => {
    try {
      const response = await fetch(`http://10.0.2.2:8080/tasks/reactivate?id=${id}`, {
        method: 'POST',
      });

      if (response.ok) {
        setTodoItems((prevItems) =>
          prevItems.map((item) =>
            item.id === id ? { ...item, completed: false } : item
          )
        );
      } else {
        console.error('Failed to reactivate task');
      }
    } catch (error) {
      console.error('Error reactivating task:', error);
    }
  };

  return (
    <View style={styles.container}>
      <SwipeListView
        data={todoItems.sort((a, b) => Number(a.completed) - Number(b.completed))}
        keyExtractor={(item) => item.id}
        renderItem={({ item }) => (
          <View style={styles.rowFront}>
            <Text style={[styles.item, item.completed && styles.completedItem]}>
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
            handleTaskSwipe(rowKey);
          } else {
            handleTaskReactivate(rowKey);
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
  item: {
    fontSize: 18,
  },
  completedItem: {
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